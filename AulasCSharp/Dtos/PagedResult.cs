namespace AulasCSharp.Dtos;

// Envelope de resposta paginada estendido para suportar links HATEOAS de navegabilidade
public record PagedResult<T>(
    IEnumerable<T> Itens,
    long TotalRegistros,
    int PaginaAtual,
    int TamanhoPagina,
    List<LinkDto>? Links = null
)
{
    public int TotalPaginas => (int)Math.Ceiling((double)TotalRegistros / TamanhoPagina);
    public bool HasPreviousPage => PaginaAtual > 1;
    public bool HasNextPage => PaginaAtual < TotalPaginas;
}