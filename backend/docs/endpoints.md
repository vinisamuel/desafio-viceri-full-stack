# Endpoints da API

[← Voltar para o README](../README.md)

## 📡 Base URL

```
http://localhost:5000/api
```

## 🦸 Heroes

Endpoints para gerenciamento de super-heróis.

### **GET** `/heroes`

Lista todos os heróis cadastrados.

**Resposta de Sucesso (200 OK):**
```json
[
  {
    "id": 1,
    "name": "Superman",
    "createdAt": "2024-01-15T10:30:00Z"
  },
  {
    "id": 2,
    "name": "Batman",
    "createdAt": "2024-01-15T11:00:00Z"
  }
]
```

**Exemplo de Requisição:**
```bash
curl http://localhost:5000/api/heroes
```

---

### **GET** `/heroes/{id}`

Busca um herói específico por ID, incluindo seus superpoderes.

**Parâmetros:**
- `id` (path, obrigatório) - ID do herói

**Resposta de Sucesso (200 OK):**
```json
{
  "id": 1,
  "name": "Superman",
  "createdAt": "2024-01-15T10:30:00Z",
  "superpowers": [
    {
      "id": 1,
      "name": "Super Força",
      "description": "Força sobre-humana"
    },
    {
      "id": 2,
      "name": "Voo",
      "description": "Capacidade de voar"
    }
  ]
}
```

**Resposta de Erro (404 Not Found):**
```json
{
  "errors": [
    {
      "message": "Herói não encontrado"
    }
  ]
}
```

**Exemplo de Requisição:**
```bash
curl http://localhost:5000/api/heroes/1
```

---

### **POST** `/heroes`

Cria um novo herói.

**Body (JSON):**
```json
{
  "name": "Wonder Woman",
  "superpowerIds": [1, 3, 5]
}
```

**Campos:**
- `name` (string, obrigatório) - Nome do herói (mínimo 3 caracteres)
- `superpowerIds` (array, obrigatório) - Lista de IDs dos superpoderes

**Resposta de Sucesso (201 Created):**
```json
{
  "id": 3,
  "name": "Wonder Woman",
  "createdAt": "2024-01-15T12:00:00Z",
  "superpowers": [
    {
      "id": 1,
      "name": "Super Força",
      "description": "Força sobre-humana"
    },
    {
      "id": 3,
      "name": "Agilidade",
      "description": "Movimentos extremamente rápidos"
    },
    {
      "id": 5,
      "name": "Resistência",
      "description": "Resistência a danos"
    }
  ]
}
```

**Resposta de Erro (400 Bad Request):**
```json
{
  "errors": [
    {
      "message": "Nome é obrigatório"
    },
    {
      "message": "Selecione pelo menos um superpoder"
    }
  ]
}
```

**Exemplo de Requisição:**
```bash
curl -X POST http://localhost:5000/api/heroes \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Wonder Woman",
    "superpowerIds": [1, 3, 5]
  }'
```

---

### **PUT** `/heroes/{id}`

Atualiza um herói existente.

**Parâmetros:**
- `id` (path, obrigatório) - ID do herói a ser atualizado

**Body (JSON):**
```json
{
  "name": "Superman (Clark Kent)",
  "superpowerIds": [1, 2, 4]
}
```

**Campos:**
- `name` (string, obrigatório) - Novo nome do herói
- `superpowerIds` (array, obrigatório) - Nova lista de superpoderes

**Resposta de Sucesso (200 OK):**
```json
{
  "id": 1,
  "name": "Superman (Clark Kent)",
  "createdAt": "2024-01-15T10:30:00Z",
  "superpowers": [
    {
      "id": 1,
      "name": "Super Força",
      "description": "Força sobre-humana"
    },
    {
      "id": 2,
      "name": "Voo",
      "description": "Capacidade de voar"
    },
    {
      "id": 4,
      "name": "Visão de Calor",
      "description": "Raios de calor pelos olhos"
    }
  ]
}
```

**Resposta de Erro (404 Not Found):**
```json
{
  "errors": [
    {
      "message": "Herói não encontrado"
    }
  ]
}
```

**Exemplo de Requisição:**
```bash
curl -X PUT http://localhost:5000/api/heroes/1 \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Superman (Clark Kent)",
    "superpowerIds": [1, 2, 4]
  }'
```

---

### **DELETE** `/heroes/{id}`

Remove um herói.

**Parâmetros:**
- `id` (path, obrigatório) - ID do herói a ser removido

**Resposta de Sucesso (200 OK):**
```json
{
  "message": "Herói removido com sucesso"
}
```

**Resposta de Erro (404 Not Found):**
```json
{
  "errors": [
    {
      "message": "Herói não encontrado"
    }
  ]
}
```

**Exemplo de Requisição:**
```bash
curl -X DELETE http://localhost:5000/api/heroes/1
```

---

## ⚡ Superpowers

Endpoints para gerenciamento de superpoderes.

### **GET** `/superpowers`

Lista todos os superpoderes disponíveis.

**Resposta de Sucesso (200 OK):**
```json
[
  {
    "id": 1,
    "name": "Super Força",
    "description": "Força sobre-humana",
    "createdAt": "2024-01-15T10:00:00Z"
  },
  {
    "id": 2,
    "name": "Voo",
    "description": "Capacidade de voar",
    "createdAt": "2024-01-15T10:00:00Z"
  },
  {
    "id": 3,
    "name": "Agilidade",
    "description": "Movimentos extremamente rápidos",
    "createdAt": "2024-01-15T10:00:00Z"
  }
]
```

**Exemplo de Requisição:**
```bash
curl http://localhost:5000/api/superpowers
```

---

## 🏥 Health Check

### **GET** `/status`

Verifica o status da aplicação.

**Resposta de Sucesso (200 OK):**
```
Healthy
```

**Exemplo de Requisição:**
```bash
curl http://localhost:5000/status
```

---

## 📝 Códigos de Status HTTP

A API utiliza os seguintes códigos de status:

| Código | Descrição |
|--------|-----------|
| **200 OK** | Requisição bem-sucedida |
| **201 Created** | Recurso criado com sucesso |
| **400 Bad Request** | Dados de entrada inválidos |
| **404 Not Found** | Recurso não encontrado |
| **500 Internal Server Error** | Erro interno do servidor |

## 🔒 Formato de Resposta de Erro

Todas as respostas de erro seguem o padrão:

```json
{
  "errors": [
    {
      "message": "Descrição do erro"
    }
  ]
}
```

## 📋 Validações

### Heroes

- **name**: 
  - Obrigatório
  - Mínimo de 3 caracteres
  - Máximo de 100 caracteres
  - Não pode ser duplicado

- **superpowerIds**:
  - Obrigatório
  - Deve conter pelo menos 1 superpoder
  - Todos os IDs devem existir no banco de dados

### Superpowers

Os superpoderes são pré-cadastrados no sistema e não podem ser criados via API (apenas consultados).

## 🧪 Testando com Swagger

A forma mais fácil de testar a API é através do Swagger UI:

1. Inicie a aplicação
2. Acesse `http://localhost:5000/swagger`
3. Expanda o endpoint desejado
4. Clique em "Try it out"
5. Preencha os parâmetros/body
6. Clique em "Execute"

## 💡 Exemplos de Fluxo Completo

### Criar um Novo Herói

```bash
# 1. Listar superpoderes disponíveis
curl http://localhost:5000/api/superpowers

# 2. Criar herói com superpoderes selecionados
curl -X POST http://localhost:5000/api/heroes \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Flash",
    "superpowerIds": [3, 6]
  }'

# 3. Consultar o herói criado
curl http://localhost:5000/api/heroes/4
```

### Atualizar um Herói

```bash
# 1. Consultar herói atual
curl http://localhost:5000/api/heroes/1

# 2. Atualizar dados
curl -X PUT http://localhost:5000/api/heroes/1 \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Superman Prime",
    "superpowerIds": [1, 2, 4, 7]
  }'

# 3. Verificar atualização
curl http://localhost:5000/api/heroes/1
```

---

**Navegação:**
- [← Voltar para o README](../README.md)
- [← Arquitetura](architecture.md)
- [← Como Executar](getting-started.md)
