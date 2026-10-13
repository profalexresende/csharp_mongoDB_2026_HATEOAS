namespace AulasCSharp.Dtos;

// DTO que representa um hiperlink RESTful (HATEOAS)
public record LinkDto(
    string Href,    // URL/URI completa para execução da ação
    string Rel,     // Relação da ação (ex: "self", "update", "delete", "first", "next")
    string Method   // Método HTTP a ser utilizado (GET, POST, PUT, DELETE)
);