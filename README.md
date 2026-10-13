# Advanced Business Development with .NET
## Aula 02 de 04: Paginação, Ordenação e Filtros Avançados em APIs RESTful com MongoDB Atlas

Este repositório contém o código-fonte desenvolvido na Aula 02 da série de 4 aulas práticas da disciplina de Advanced Business Development with .NET. O objetivo principal desta segunda etapa é evoluir a nossa Minimal API para suportar consultas otimizadas e de alta performance no MongoDB Atlas, implementando paginação no servidor, ordenação flexível e filtros dinâmicos encapsulados em DTOs reutilizáveis.

---

### 📌 Tópicos Abordados na Aula 02
- Paginação Server-Side: Uso dos métodos `.Skip()` e `.Limit()` do driver oficial do MongoDB para fatiar dados diretamente no banco de dados.
- Envelope de Resposta (`PagedResult<T>`): Criação de um DTO genérico imutável para entregar a lista de dados acompanhada de metadados (`TotalRegistros`, `TotalPaginas`, `PaginaAtual`).
- Filtros Dinâmicos: Construção de consultas flexíveis com `Builders<T>.Filter`, suporte a busca insensível a maiúsculas/minúsculas (`Regex`) e faixas de valores (`Gte`, `Lte`).
- Ordenação Dinâmica: Aplicação de critérios de ordenação ascendente e descendente via `Builders<T>.Sort`.
- Desestruturação de Query Strings: Uso do atributo `[AsParameters]` do ASP.NET Core para mapear múltiplos parâmetros da URL direto para um `record`.

---

### 📂 Estrutura do Projeto
- AulasCSharp/
  - src/AulasCSharp/
    - Properties/launchSettings.json
    - Dominio/
      - Produto.cs
      - IProdutoRepositorio.cs
    - Dtos/
      - PagedResult.cs
      - ProdutoFiltroRequest.cs
    - Infraestrutura/
      - MongoProdutoRepositorio.cs
    - appsettings.json
    - Program.cs
  - AulasCSharp.sln

---

### 🚀 Como Executar o Projeto
1. Clone o repositório: git clone https://github.com/profalexresende/csharp_mongoDB_2026.git
2. Acesse a pasta da aplicação: cd csharp_mongoDB_2026/src/AulasCSharp
3. Configure a sua Connection String do MongoDB Atlas no arquivo appsettings.json no campo MongoDbSettings:ConnectionString.
4. Execute o projeto: dotnet run
5. Acesse a interface gráfica do Swagger para testar as rotas no endereço: https://localhost:7290/swagger

---

### 🗺️️ Próximas Aulas do Módulo (2 de 4)
- Aula 01: Persistência NoSQL com MongoDB Atlas e Padrão Repository.
- Aula 02 [Atual]: Paginação, Ordenação e Filtros Avançados em APIs RESTful.
- Aula 03: HATEOAS e Navegabilidade no Nível 3 da Maturidade RESTful.
- Aula 04: Segurança, Autenticação e Autorização com JWT e Swagger.

---

Prof. Alex Sander
