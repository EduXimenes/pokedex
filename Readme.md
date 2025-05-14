# Pokedex API

Uma API RESTful desenvolvida com ASP.NET Core para consultar informações detalhadas sobre Pokémon, incluindo dados básicos, sprites em Base64, sons e cadeia de evolução. Ideal para consumo por frontends modernos.

## 🚀 Tecnologias e Ferramentas Utilizadas

- **Linguagem:** C#
- **Framework:** ASP.NET Core 8
- **ORM:** Entity Framework Core
- **Banco de Dados:** SQLite
- **HTTP Client:** System.Net.Http
- **AutoMapper:** Mapeamento entre DTOs e Entidades
- **Swagger:** Documentação interativa da API
- **Docker:** Containerização da aplicação
- **Render:** Deploy gratuito da API


### 🧪 Funcionalidades
Buscar Pokémon por ID ou nome

Obter 10 Pokémon aleatórios

Buscar dados de evolução de Pokémon

Obter sprites em URL e Base64

Gerenciar Pokémon Masters

## 📦 Como Instalar e Executar o Projeto

### ✅ Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Git](https://git-scm.com/)
- [Docker](https://www.docker.com/) (opcional)

### 🔁 Clonando o Repositório

```bash
git clone https://github.com/seu-usuario/pokedex-api.git
cd pokedex-api

### Executando com .NET CLI

cd src/Pokedex.API
dotnet run


### Executando com Docker
docker build -t pokedex-api .
docker run -p 5000:80 pokedex-api

### Testando a API
http://localhost:5000/swagger/index.html


>  This is a challenge by [Coodesh](https://coodesh.com/)
