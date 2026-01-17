# Heroes API

[⬅️ Voltar ao README principal](../README.md)

API RESTful para gerenciamento de super-heróis e seus superpoderes, desenvolvida em .NET 8.

## 📚 Documentação

- **[Arquitetura do Projeto](docs/architecture.md)** - Entenda a estrutura e organização do código
- **[Como Executar](docs/getting-started.md)** - Guia para configurar e rodar a aplicação
- **[Endpoints](docs/endpoints.md)** - Documentação completa de todas as rotas da API

## 🚀 Quick Start

```bash
# Navegar para o projeto principal
cd src/HeroesApi

# Restaurar dependências
dotnet restore

# Executar a aplicação
dotnet run
```

A API estará disponível em `http://localhost:5000` e a documentação Swagger em `http://localhost:5000/swagger`.

## 🛠️ Tecnologias

- .NET 8
- Entity Framework Core
- SQL Server
- Swagger/OpenAPI

## 📦 Estrutura do Projeto

```
backend/
├── src/
│   ├── HeroesApi/           # Projeto principal da API
│   ├── HeroesApi.Data/      # Camada de acesso a dados
│   └── HeroesApi.Shared/    # Código compartilhado
├── tests/                   # Testes unitários e de integração
└── docs/                    # Documentação adicional
```

## 📄 Licença

Este projeto foi desenvolvido como desafio técnico.
