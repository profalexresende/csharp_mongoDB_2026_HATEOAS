using AulasCSharp.Dominio;
using AulasCSharp.Dtos;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AulasCSharp.Infraestrutura;

public class MongoProdutoRepositorio : IProdutoRepositorio
{
    private readonly IMongoCollection<Produto> _collection;

    public MongoProdutoRepositorio(IConfiguration config)
    {
        var connectionString = config["MongoDbSettings:ConnectionString"];
        var databaseName = config["MongoDbSettings:DatabaseName"];

        if (string.IsNullOrEmpty(connectionString))
            throw new InvalidOperationException("A ConnectionString do MongoDB não foi configurada.");

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        _collection = database.GetCollection<Produto>("Produtos");
    }

    public async Task<PagedResult<Produto>> ObterPaginadoAsync(ProdutoFiltroRequest filtro)
    {
        // Instancia o construtor de filtros tipo-seguro do MongoDB
        var builder = Builders<Produto>.Filter;
        // Inicia com um filtro vazio (corresponde a trazer todos os documentos)
        var filterDef = builder.Empty;

        // Filtro 1: Nome usando Expressão Regular para busca case-insensitive ('i')
        if (!string.IsNullOrWhiteSpace(filtro.Nome))
            filterDef &= builder.Regex(p => p.Nome, new BsonRegularExpression(filtro.Nome, "i"));

        // Filtro 2: Preço maior ou igual (GTE) ao preço mínimo informado
        if (filtro.PrecoMinimo.HasValue)
            filterDef &= builder.Gte(p => p.Preco, filtro.PrecoMinimo.Value);

        // Filtro 3: Preço menor ou igual (LTE) ao preço máximo informado
        if (filtro.PrecoMaximo.HasValue)
            filterDef &= builder.Lte(p => p.Preco, filtro.PrecoMaximo.Value);

        // Executa a contagem total de documentos filtrados sem carregar a lista em memória
        var totalRegistros = await _collection.CountDocumentsAsync(filterDef);

        // Define o critério de ordenação com base no campo e na direção (ascendente/descendente)
        var sortDef = filtro.OrdenarPor?.ToLower() switch
        {
            "preco" => filtro.Ascendente
                ? Builders<Produto>.Sort.Ascending(p => p.Preco)
                : Builders<Produto>.Sort.Descending(p => p.Preco),
            _ => filtro.Ascendente
                ? Builders<Produto>.Sort.Ascending(p => p.Nome)
                : Builders<Produto>.Sort.Descending(p => p.Nome)
        };

        // Aplica o filtro, a ordenação e a fatia de paginação no servidor do MongoDB
        var itens = await _collection.Find(filterDef)
            .Sort(sortDef)
            .Skip((filtro.Pagina - 1) * filtro.TamanhoPagina)
            .Limit(filtro.TamanhoPagina)
            .ToListAsync();

        // Retorna o resultado envelopado com metadados
        return new PagedResult<Produto>(itens, totalRegistros, filtro.Pagina, filtro.TamanhoPagina);
    }

    public async Task<Produto?> ObterPorIdAsync(Guid id) =>
        await _collection.Find(p => p.Id == id).FirstOrDefaultAsync();

    public async Task AdicionarAsync(Produto produto) =>
        await _collection.InsertOneAsync(produto);
}