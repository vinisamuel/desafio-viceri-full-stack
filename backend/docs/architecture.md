# Arquitetura do Projeto

[← Voltar para o README](../README.md)

## 📐 Visão Geral

A Heroes API foi desenvolvida seguindo os princípios de **Clean Architecture** e **Separation of Concerns**, organizando o código em três projetos principais que representam diferentes responsabilidades.

## 🏗️ Estrutura de Projetos

### **HeroesApi** (Camada de Apresentação)
Projeto principal da API que expõe os endpoints REST.

**Responsabilidades:**
- Controllers e rotas da API
- Configuração de middlewares e pipeline HTTP
- Configuração de CORS, Swagger e Health Checks
- Validação de entrada de dados
- Tratamento de exceções global

**Principais componentes:**
- `Controllers/` - Controladores REST (Heroes, Superpowers)
- `Services/` - Serviços de negócio
- `Models/` - DTOs para comunicação com cliente
- `Middlewares/` - Tratamento de erros e interceptadores
- `Extensions/` - Métodos de extensão para configuração

### **HeroesApi.Data** (Camada de Dados)
Responsável pelo acesso e persistência de dados.

**Responsabilidades:**
- Contexto do Entity Framework Core
- Entidades do banco de dados
- Mapeamentos (Fluent API)
- Migrations
- Repositórios para acesso a dados

**Principais componentes:**
- `Contexts/` - DbContext do Entity Framework
- `Entities/` - Entidades do banco de dados
- `Mappings/` - Configurações de mapeamento ORM
- `Migrations/` - Histórico de alterações no banco
- `Repositories/` - Padrão Repository para acesso aos dados

### **HeroesApi.Shared** (Camada Compartilhada)
Código compartilhado entre os projetos.

**Responsabilidades:**
- Utilitários e helpers
- Sistema de notificações
- Configurações da aplicação
- Factories e extensões genéricas

**Principais componentes:**
- `NotificationWrapper/` - Sistema de notificação de erros/sucesso
- `Extensions/` - Extensões reutilizáveis
- `Settings/` - Classes de configuração
- `Factories/` - Padrão Factory para criação de objetos

## 🔄 Fluxo de Requisição

```
┌─────────────┐
│   Cliente   │
└──────┬──────┘
       │ HTTP Request
       ▼
┌─────────────────────────────┐
│   Middleware Pipeline       │
│   - Exception Handler       │
│   - CORS                    │
│   - Authentication          │
└──────────┬──────────────────┘
           │
           ▼
┌─────────────────────────────┐
│      Controller             │
│   - Validação de entrada    │
│   - Chamada ao Service      │
└──────────┬──────────────────┘
           │
           ▼
┌─────────────────────────────┐
│       Service               │
│   - Lógica de negócio       │
│   - Validações complexas    │
│   - Chamada ao Repository   │
└──────────┬──────────────────┘
           │
           ▼
┌─────────────────────────────┐
│      Repository             │
│   - Queries ao banco        │
│   - Entity Framework Core   │
└──────────┬──────────────────┘
           │
           ▼
┌─────────────────────────────┐
│     SQL Server              │
└─────────────────────────────┘
```

## 🎯 Padrões de Design Utilizados

### **Repository Pattern**
Abstração do acesso a dados, isolando a lógica de persistência.

### **Dependency Injection**
Todos os serviços e dependências são injetados via construtor, facilitando testes e manutenção.

### **Notification Pattern**
Sistema de notificações para comunicar erros e sucessos sem usar exceções para controle de fluxo.

### **DTO Pattern**
Objetos de transferência de dados (Data Transfer Objects) para separar as entidades do banco dos modelos expostos pela API.

## 🗄️ Banco de Dados

**Tecnologia:** SQL Server  
**ORM:** Entity Framework Core

### Entidades Principais

- **Hero** - Representa um super-herói
- **Superpower** - Representa um superpoder
- **HeroSuperpower** - Relacionamento muitos-para-muitos entre heróis e superpoderes

### Relacionamentos

```
Hero (1) ───< HeroSuperpower >─── (N) Superpower
```

## 🔧 Configuração e Extensões

O projeto utiliza métodos de extensão para organizar a configuração:

- `AddHeroesAppDatabase()` - Configura o contexto do banco de dados
- `AddBusinessServices()` - Registra os serviços de negócio
- `AddSwaggerDocumentation()` - Configura a documentação Swagger
- `AddCamelCaseJsonOptions()` - Configura serialização JSON em camelCase

## 🛡️ Tratamento de Erros

Sistema centralizado de tratamento de exceções usando middleware customizado (`GlobalExceptionHandler`), garantindo respostas consistentes e seguras.

## 📊 Health Checks

Endpoint `/status` disponível para monitoramento da saúde da aplicação.

---

**Navegação:**
- [← Voltar para o README](../README.md)
- [Como Executar →](getting-started.md)
- [Endpoints →](endpoints.md)
