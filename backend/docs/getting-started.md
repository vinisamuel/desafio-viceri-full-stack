# Como Executar

[← Voltar para o README](../README.md)

## 📋 Pré-requisitos

Antes de executar a aplicação, certifique-se de ter instalado:

- **.NET 8 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/8.0)
- **SQL Server** - Local ou remoto
- **Git** - Para clonar o repositório
- **IDE** (opcional) - Visual Studio 2022, VS Code ou Rider

## 🚀 Instalação

### 1. Clonar o Repositório

```bash
git clone <repository-url>
cd desafio-viceri/backend
```

### 2. Configurar Banco de Dados

Edite o arquivo `src/HeroesApi/appsettings.json` e configure a connection string:

```json
{
  "HeroesAppDatabase": {
    "ConnectionString": "Data Source=<INSTANCE>;Initial Catalog=heroesapp;User ID=<USERNAME>;Password=<PASSWORD>;TrustServerCertificate=True;",
    "Timeout": 60,
    "EnableLog": false
  }
}
```

**Substitua:**
- `<INSTANCE>` - Nome da instância do SQL Server (ex: `localhost` ou `localhost\\SQLEXPRESS`)
- `<USERNAME>` - Usuário do SQL Server
- `<PASSWORD>` - Senha do SQL Server

### 3. Restaurar Dependências

```bash
cd src/HeroesApi
dotnet restore
```

### 4. Executar Migrations

O Entity Framework Core criará automaticamente o banco de dados na primeira execução. Alternativamente, você pode executar manualmente:

```bash
dotnet ef database update --project ../HeroesApi.Data
```

### 5. Executar a Aplicação

```bash
dotnet run
```

A API iniciará em:
- **HTTP:** `http://localhost:5000`
- **HTTPS:** `https://localhost:5001` (apenas em produção)

## 🌐 Acessar a Documentação Swagger

Com a aplicação rodando, acesse:

```
http://localhost:5000/swagger
```

O Swagger UI permite testar todos os endpoints diretamente pelo navegador.

## 🔧 Comandos Úteis

### Compilar o Projeto

```bash
dotnet build
```

### Executar Testes

```bash
cd tests
dotnet test
```

### Criar Nova Migration

```bash
dotnet ef migrations add NomeDaMigration --project src/HeroesApi.Data --startup-project src/HeroesApi
```

### Reverter Migration

```bash
dotnet ef database update NomeDaMigrationAnterior --project src/HeroesApi.Data --startup-project src/HeroesApi
```

### Limpar Builds

```bash
dotnet clean
```

## 🐳 Docker (Opcional)

Se preferir executar com Docker:

```bash
# Build da imagem
docker build -t heroes-api .

# Executar container
docker run -p 5000:80 heroes-api
```

## 🔍 Verificar Status da API

Após iniciar, verifique se a API está funcionando:

```bash
curl http://localhost:5000/status
```

Resposta esperada: `Healthy`

## ⚙️ Configurações Adicionais

### CORS

A API está configurada para aceitar requisições de `http://localhost:5173` (frontend). Para adicionar outras origens, edite `Program.cs`:

```csharp
policy.WithOrigins(["http://localhost:5173", "http://outro-dominio.com"]);
```

### Logs

Os logs são configurados em `appsettings.json`. Para habilitar logs do banco de dados:

```json
{
  "HeroesAppDatabase": {
    "EnableLog": true
  }
}
```

### Environment

Para executar em modo de desenvolvimento:

```bash
dotnet run --environment Development
```

## 🐛 Troubleshooting

### Erro de Conexão com Banco de Dados

**Problema:** `Cannot open database "heroesapp"`

**Solução:**
1. Verifique se o SQL Server está rodando
2. Confirme as credenciais na connection string
3. Execute as migrations manualmente

### Porta já em Uso

**Problema:** `Address already in use`

**Solução:**
```bash
# Alterar porta em Properties/launchSettings.json
# ou especificar via variável de ambiente
dotnet run --urls "http://localhost:5005"
```

### Erro ao Restaurar Pacotes

**Problema:** `Unable to restore packages`

**Solução:**
```bash
# Limpar cache do NuGet
dotnet nuget locals all --clear

# Restaurar novamente
dotnet restore
```

## 📱 Testando a API

### Usando cURL

```bash
# Listar todos os heróis
curl http://localhost:5000/api/heroes

# Criar um herói
curl -X POST http://localhost:5000/api/heroes \
  -H "Content-Type: application/json" \
  -d '{"name":"Superman","superpowerIds":[1,2]}'
```

### Usando HTTPie

```bash
# Listar todos os heróis
http GET http://localhost:5000/api/heroes

# Criar um herói
http POST http://localhost:5000/api/heroes name="Batman" superpowerIds:='[1,3]'
```

---

**Navegação:**
- [← Voltar para o README](../README.md)
- [← Arquitetura](architecture.md)
- [Endpoints →](endpoints.md)
