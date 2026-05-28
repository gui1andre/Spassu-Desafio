# Desafio Fullstack - Spassu (Gerenciamento de Pedidos)

Este projeto é uma aplicação de gerenciamento de pedidos, composta por uma API REST robusta desenvolvida em .NET 8 e um frontend moderno em React.

## 🚀 Tecnologias Utilizadas

### Backend
- **.NET 8** (C#)
- **Entity Framework Core** (ORM)
- **SQLite** (Banco de dados relacional leve e local)
- **ASP.NET Identity** (Autenticação e Autorização JWT)
- **FluentValidation** (Validação de dados)
- **AutoMapper** (Mapeamento de DTOs)
- **Swagger/OpenAPI** (Documentação da API)

### Frontend
- **React 19** (TypeScript)
- **Vite** (Build tool e Dev Server)
- **React Router 7** (Navegação SPA)
- **Fetch API** (Comunicação com o Backend)
- **Vanilla CSS** (Estilização)

---

## 🏛️ Arquitetura (Clean Architecture)

O projeto segue princípios de **Clean Architecture** e **DDD (Domain-Driven Design)** para garantir separação de responsabilidades, testabilidade e manutenibilidade:

1.  **Domain**: Contém as entidades de negócio (Pedido, ItemPedido), enums e interfaces de repositório. É o núcleo do projeto e não depende de nada externo.
2.  **Application**: Contém as regras de negócio, serviços (PedidoService, AuthService), DTOs, validadores e perfis de mapeamento.
3.  **Infrastructure**: Implementação do acesso a dados (DbContext, Repositórios), configurações do Entity Framework, Migrations e serviços de infraestrutura (Token JWT, Gerenciamento de Identidade).
4.  **API**: Ponto de entrada da aplicação, contendo os Controllers, Middlewares de erro e configuração de injeção de dependência.
5.  **Tests**: Testes unitários focados na lógica de domínio e entidades.

---

## 📦 Como Rodar o Projeto

Existem duas formas principais de executar a aplicação:

### 1. Via Docker (Recomendado)

Esta é a forma mais rápida, pois já configura o banco de dados, variáveis de ambiente e a comunicação entre os serviços automaticamente.

**Pré-requisitos:** Docker e Docker Compose instalados.

Na raiz do projeto, execute:
```bash
docker-compose up --build
```

- **Frontend:** [http://localhost](http://localhost)
- **Backend (Swagger):** [http://localhost:8080/swagger](http://localhost:8080/swagger)
- **Persistência:** O banco de dados SQLite é persistido em um volume Docker chamado `sqlite_data`.

---

### 2. Modo Desenvolvimento / Debug (Local)

Para rodar localmente e realizar debug via IDE (Visual Studio / VS Code):

#### Backend:
1. Navegue até `api/`.
2. Certifique-se de que a connection string no `appsettings.Development.json` está correta.
3. Execute o projeto `SpassuDesafio.API`. As migrações serão aplicadas automaticamente ao iniciar.
   - A API rodará geralmente em `https://localhost:7198` ou `http://localhost:5005`.

#### Frontend:
1. Navegue até `presentation/SpassuDesafioFront/`.
2. Instale as dependências:
   ```bash
   npm install
   ```
3. Verifique o arquivo `.env` e ajuste a `VITE_API_URL` para a URL da sua API local.
4. Inicie o servidor de desenvolvimento:
   ```bash
   npm run dev
   ```
   - O frontend rodará em `http://localhost:5173`.

---

## 🔐 Autenticação

A aplicação utiliza JWT para segurança.
1. Crie um usuário na página de **Registro**.
2. Faça **Login** para obter o token.
3. O token é armazenado no `localStorage` e enviado automaticamente no header `Authorization` de todas as requisições protegidas.

## 📝 Observações
- O projeto inclui um **Middleware de tratamento de exceções** global no backend.
- Validações são feitas tanto no frontend (UX) quanto no backend (Segurança) via **FluentValidation**.
- As migrações do banco de dados são executadas automaticamente no início da aplicação (tanto no Docker quanto local).
