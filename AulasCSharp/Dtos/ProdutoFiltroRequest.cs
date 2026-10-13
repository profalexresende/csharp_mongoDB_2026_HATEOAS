namespace AulasCSharp.Dtos;

// Record para mapear a Query String com filtros, ordenação e paginação (com valores padrão)
public record ProdutoFiltroRequest(
    string? Nome = null,
    decimal? PrecoMinimo = null,
    decimal? PrecoMaximo = null,
    string? OrdenarPor = "nome", // Opções aceitas: "nome" ou "preco"
    bool Ascendente = true,
    int Pagina = 1,
    int TamanhoPagina = 10
);