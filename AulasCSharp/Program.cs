using AulasCSharp.Dominio;
using AulasCSharp.Dtos;
using AulasCSharp.Infraestrutura;

var builder = WebApplication.CreateBuilder(args);

// Configuração do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Permite acessar o HttpContext atual para gerar URLs absolutas dinamicamente
builder.Services.AddHttpContextAccessor();

// Injeção de Dependência do Repositório NoSQL
builder.Services.AddScoped<IProdutoRepositorio, MongoProdutoRepositorio>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ----------------------------------------------------------------------------------
// MÉTODOS AUXILIARES PARA GERAÇÃO DE LINKS HATEOAS
// ----------------------------------------------------------------------------------

// Gera links de ação para um recurso individual de Produto
static List<LinkDto> GerarLinksProduto(HttpContext httpContext, Guid id)
{
    var baseUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/api/produtos";

    return new List<LinkDto>
    {
        new LinkDto($"{baseUrl}/{id}", "self", "GET"),
        new LinkDto($"{baseUrl}/{id}", "update", "PUT"),
        new LinkDto($"{baseUrl}/{id}", "delete", "DELETE"),
        new LinkDto(baseUrl, "all-produtos", "GET")
    };
}

// Gera links de navegação para a resposta paginada da Aula 02
static List<LinkDto> GerarLinksPaginacao(HttpContext httpContext, PagedResult<Produto> resultado, ProdutoFiltroRequest filtro)
{
    var baseUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/api/produtos";
    var links = new List<LinkDto>();

    links.Add(new LinkDto($"{baseUrl}?pagina={resultado.PaginaAtual}&tamanhoPagina={resultado.TamanhoPagina}", "self", "GET"));
    links.Add(new LinkDto($"{baseUrl}?pagina=1&tamanhoPagina={resultado.TamanhoPagina}", "first", "GET"));

    if (resultado.HasPreviousPage)
    {
        links.Add(new LinkDto($"{baseUrl}?pagina={resultado.PaginaAtual - 1}&tamanhoPagina={resultado.TamanhoPagina}", "previous", "GET"));
    }

    if (resultado.HasNextPage)
    {
        links.Add(new LinkDto($"{baseUrl}?pagina={resultado.PaginaAtual + 1}&tamanhoPagina={resultado.TamanhoPagina}", "next", "GET"));
    }

    if (resultado.TotalPaginas > 0)
    {
        links.Add(new LinkDto($"{baseUrl}?pagina={resultado.TotalPaginas}&tamanhoPagina={resultado.TamanhoPagina}", "last", "GET"));
    }

    links.Add(new LinkDto(baseUrl, "create-produto", "POST"));

    return links;
}

// ----------------------------------------------------------------------------------
// ENDPOINTS DA MINIMAL API (EVOLUÍDOS PARA NÍVEL 3 REST)
// ----------------------------------------------------------------------------------

// GET /api/produtos: Mantém a paginação e os filtros da Aula 02, injetando HATEOAS
app.MapGet("/api/produtos", async ([AsParameters] ProdutoFiltroRequest filtro, IProdutoRepositorio repo, HttpContext httpContext) =>
{
    var resultado = await repo.ObterPaginadoAsync(filtro);

    // Envelopa cada produto da lista com os seus links de ação individual
    var itensComLinks = resultado.Itens.Select(p =>
        new ResourceEnvelope<Produto>(p, GerarLinksProduto(httpContext, p.Id))
    );

    // Gera os hiperlinks de navegação baseados na paginação
    var linksPaginacao = GerarLinksPaginacao(httpContext, resultado, filtro);

    var respostaHateoas = new
    {
        itens = itensComLinks,
        totalRegistros = resultado.TotalRegistros,
        paginaAtual = resultado.PaginaAtual,
        tamanhoPagina = resultado.TamanhoPagina,
        totalPaginas = resultado.TotalPaginas,
        links = linksPaginacao
    };

    return Results.Ok(respostaHateoas);
});

// GET /api/produtos/{id}: Retorna o produto filtrado por ID com links HATEOAS
app.MapGet("/api/produtos/{id:guid}", async (Guid id, IProdutoRepositorio repo, HttpContext httpContext) =>
{
    var produto = await repo.ObterPorIdAsync(id);

    if (produto is null)
        return Results.NotFound(new { mensagem = "Produto não encontrado." });

    var links = GerarLinksProduto(httpContext, produto.Id);
    var respostaHateoas = new ResourceEnvelope<Produto>(produto, links);

    return Results.Ok(respostaHateoas);
});

// POST /api/produtos: Retorna 201 Created acompanhado do recurso criado e seus links
app.MapPost("/api/produtos", async (CriarProdutoRequest request, IProdutoRepositorio repo, HttpContext httpContext) =>
{
    try
    {
        var produto = new Produto(request.Nome, request.Preco, request.Estoque);
        await repo.AdicionarAsync(produto);

        var links = GerarLinksProduto(httpContext, produto.Id);
        var respostaHateoas = new ResourceEnvelope<Produto>(produto, links);

        return Results.Created($"/api/produtos/{produto.Id}", respostaHateoas);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { erro = ex.Message });
    }
});

app.Run();

record CriarProdutoRequest(string Nome, decimal Preco, int Estoque);