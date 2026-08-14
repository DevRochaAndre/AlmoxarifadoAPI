# 📦 Almoxarifado.API

API RESTful para controle e gestão de almoxarifado desenvolvida em .NET integrada ao banco de dados MySQL.

---

## 🚀 Tecnologias Utilizadas

* **Linguagem:** C# (.NET)
* **Framework:** ASP.NET Core Web API
* **ORM:** Entity Framework Core
* **Provedor de Banco de Dados:** Pomelo.EntityFrameworkCore.MySql
* **Banco de Dados:** MySQL Server
* **Ferramentas:** EF Core Migrations, cURL

---

## 🏛️ Arquitetura e Estrutura do Projeto

O projeto organiza as responsabilidades em pastas bem definidas dentro da Web API:

```text
AlmoxarifadoAPI/
└── Almoxarifado.API/
    ├── Controllers/
    │   └── FuncionariosController.cs  # Endpoints REST (CRUD de Funcionários)
    ├── Data/
    │   └── AppDbContext.cs            # Contexto do Entity Framework (Mapeamento e Índices)
    ├── Migrations/                    # Histórico e scripts de evolução do banco MySQL
    ├── Models/
    │   └── Funcionario.cs             # Entidade de domínio 'Funcionario'
    ├── Properties/
    │   └── launchSettings.json        # Configurações de execução
    ├── appsettings.json               # String de conexão e variáveis de ambiente
    └── Program.cs                     # Configuração de Injeção de Dependência e Middlewares