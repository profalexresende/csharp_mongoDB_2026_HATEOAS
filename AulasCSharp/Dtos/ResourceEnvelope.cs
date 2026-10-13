namespace AulasCSharp.Dtos;

// Wrapper genérico que une qualquer recurso de dados aos seus links HATEOAS
public record ResourceEnvelope<T>(
    T Data,
    List<LinkDto> Links
);