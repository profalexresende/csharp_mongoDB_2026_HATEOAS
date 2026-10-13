# Advanced Business Development with .NET
## Aula 03 de 04: HATEOAS e Navegabilidade no Nível 3 da Maturidade RESTful

Este repositório contém o código-fonte desenvolvido na Aula 03 da série de 4 aulas práticas da disciplina de Advanced Business Development with .NET. O objetivo principal desta terceira etapa é elevar o nível de maturidade da nossa API para o Nível 3 do Modelo de Richardson, incorporando HATEOAS (Hypermedia As The Engine Of Application State) para auto-explicabilidade e navegabilidade dinâmica dos recursos.

---

### 📌 Tópicos Abordados na Aula 03
- Modelo de Maturidade de Richardson: Compreensão dos 4 níveis (0 a 3) e o papel do HATEOAS no Nível 3.
- Objeto de Hiperlinks (`LinkDto`): Estruturação de relações de navegação com `href`, `rel` e `method`.
- Resource Envelope (`ResourceEnvelope<T>`): Padronização de respostas unindo o payload de dados aos seus respectivos links de ação.
- Navegabilidade em Paginação: Links dinâmicos de paginação (`first`, `previous`, `next`, `last`) baseados no estado atual da consulta.
- Contexto de Requisição (`HttpContext`): Geração dinâmica de URIs absolutas a partir da requisição HTTP ativa.

---

### 📂 Estrutura do Projeto
- AulasCSharp/
  - src/AulasCSharp/
    - Properties/launchSettings.json
    - Dominio/
      - Produto.cs
      - IProdutoRepositorio.cs
    - Dtos/
      - LinkDto.cs
      - ResourceEnvelope.cs
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

### 🗺️ Próximas Aulas do Módulo
- Aula 01: Persistência NoSQL com MongoDB Atlas e Padrão Repository.
- Aula 02: Paginação, Ordenação e Filtros Avançados em APIs RESTful.
- Aula 03 [Atual]: HATEOAS e Navegabilidade no Nível 3 da Maturidade RESTful.
- Aula 04: Segurança, Autenticação e Autorização com JWT e Swagger.

---

Prof. Alex Sander
