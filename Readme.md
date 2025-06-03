# Pokedex API

Uma API RESTful desenvolvida com ASP.NET Core para consultar informações detalhadas sobre Pokémon, como dados básicos, sprites, sons e cadeia de evolução.
O projeto está estruturado utilizando Arquitetura Limpa, respeitando princípios SOLID e abordagem DDD.

## Tecnologias e Ferramentas Utilizadas

- C# / .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- AutoMapper
- MediatR
- FluentValidation
- Swagger
- Docker
- Fluent Validation
- XUnit
- Rennder

### Funcionalidades

-  **Buscar Pokémon por ID ou nome**  
  `GET /api/Pokemon/{idOrName}`  

-  **Obter 10 Pokémons aleatórios**  
  `GET /api/Pokemon/random`  

-  **Criar um Pokémon Master (treinador)**  
  `POST /api/PokemonMaster/create`  

-  **Buscar Pokémon Master por nome ou ID**  
  `GET /api/PokemonMaster/{masterIdOrName}`  

-  **Capturar Pokémons**  
  `POST /api/PokemonMaster/capture`  

-  **Listar Pokémons capturados**  
  `GET /api/PokemonMaster/{masterId}/captured`  

## Como Instalar e Executar o Projeto

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Git](https://git-scm.com/)

### Clonando o Repositório

```bash
git clone https://github.com/EduXimenes/pokedex.git
cd pokedex
```
### Executando com .NET CLI
```bash
cd src/Pokedex.API
dotnet run
```

### Testando a API
http://localhost:5000/swagger \
Ou o projeto publicado em Rennder\
https://pokedex-6056.onrender.com/

### Processo e desafios desenvolvimento
O primeiro passo foi dar uma boa explorada na API do Pokémon e testei os endpoints disponíveis e entendi como funciona o fluxo de evolução e percebi que o GET direto não traz as infos de evolução, mas notei que ele retorna um link pra species, e dentro desse retorno tem a URL da cadeia de evolução. Como não encontrei nenhum vínculo direto mais simples, segui esse caminho. Também reparei que o último Pokémon listado na API é o de ID 1025.\
Decidi como estruturar o projeto e optei por usar arquitetura limpa, DDD e princípios do SOLID pra manter tudo organizado e com boas práticas.\
Durante o GET de 10 pokémons aleatórios, encontrei um problema: alguns JSONs eram enormes e acabavam estourando o buffer da requisição, portanto, tive que contornar isso mudando de lib e ajustando a abordagem pra garantir que tudo funcionasse.
