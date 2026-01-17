# Heroes App 🦸‍♂️

[⬅️ Voltar ao README principal](../../README.md)

Aplicação web para gerenciamento de heróis, desenvolvida com Vue 3, TypeScript e PrimeVue.

## 📋 Sobre o Projeto

O Heroes App é uma aplicação frontend moderna que permite gerenciar informações de heróis. A aplicação foi desenvolvida como parte do desafio Viceri, utilizando as melhores práticas e tecnologias mais recentes do ecossistema Vue.js.

## 🚀 Tecnologias Utilizadas

- **Vue 3** (v3.5.24) - Framework JavaScript progressivo
- **TypeScript** (v5.9.3) - Superset JavaScript com tipagem estática
- **Vite** (v7.2.4) - Build tool e dev server de alta performance
- **Vue Router** (v4.6.4) - Roteamento oficial do Vue.js
- **Pinia** (v3.0.4) - Gerenciamento de estado oficial do Vue
- **PrimeVue** (v4.5.4) - Biblioteca de componentes UI
- **Axios** (v1.13.2) - Cliente HTTP para requisições
- **date-fns** (v4.1.0) - Biblioteca para manipulação de datas

## 📦 Pré-requisitos

Antes de começar, certifique-se de ter instalado:

- Node.js (versão 16 ou superior)
- npm ou yarn

## 🔧 Instalação

1. Clone o repositório:
```bash
git clone <url-do-repositorio>
cd desafio-viceri/frontend/heroes-app
```

2. Instale as dependências:
```bash
npm install
```

## 🎮 Comandos Disponíveis

### Modo de Desenvolvimento
Inicia o servidor de desenvolvimento com hot-reload:
```bash
npm run dev
```

### Build de Produção
Compila e minifica o projeto para produção:
```bash
npm run build
```

### Prévia da Build
Visualiza a build de produção localmente:
```bash
npm run preview
```

## 🏗️ Estrutura do Projeto

```
heroes-app/
├── public/              # Arquivos estáticos públicos
├── src/
│   ├── assets/         # Recursos estáticos (imagens, estilos, etc)
│   ├── components/     # Componentes Vue reutilizáveis
│   ├── App.vue         # Componente raiz da aplicação
│   ├── main.ts         # Ponto de entrada da aplicação
│   └── style.css       # Estilos globais
├── index.html          # Template HTML principal
├── package.json        # Dependências e scripts
├── tsconfig.json       # Configuração do TypeScript
├── vite.config.ts      # Configuração do Vite
└── README.md          # Este arquivo
```

## 🎨 Funcionalidades

- Gerenciamento completo de heróis (CRUD)
- Interface moderna e responsiva com PrimeVue
- Navegação entre páginas com Vue Router
- Gerenciamento de estado com Pinia
- Notificações toast para feedback ao usuário
- Diálogos de confirmação para ações críticas

## 🛠️ Configuração do Ambiente

O projeto utiliza Vite como bundler e oferece suporte a:

- Hot Module Replacement (HMR)
- TypeScript out-of-the-box
- Alias de imports (`@` aponta para `./src`)

## 📄 Licença

Este projeto foi desenvolvido como desafio técnico.
