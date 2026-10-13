using AulasCSharp.Dtos;

namespace AulasCSharp.Dominio;

// Interface do Repositório atualizada para consulta paginada e filtrada
public interface IProdutoRepositorio
{
    // Retorna os dados envelopados no PagedResult<Produto> com base nos parâmetros da requisição
    Task<PagedResult<Produto>> ObterPaginadoAsync(ProdutoFiltroRequest filtro);

    Task<Produto?> ObterPorIdAsync(Guid id);

    Task AdicionarAsync(Produto produto);
}