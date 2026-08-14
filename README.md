# 📦 Almoxarifado.API

API RESTful para controle e gestão de almoxarifado desenvolvida em **.NET** integrada ao banco de dados **MySQL**.

---

## 🚀 Tecnologias Utilizadas

* **Linguagem:** C# (.NET)
* **Framework:** ASP.NET Core Web API
* **ORM:** Entity Framework Core
* **Provedor de Banco de Dados:** Pomelo.EntityFrameworkCore.MySql
* **Banco de Dados:** MySQL Server
* **Ferramentas:** EF Core Migrations

---

## 🏛️ Arquitetura e Estrutura do Projeto

O projeto segue a estrutura padrão Web API organizando responsabilidades em pastas bem definidas:

```text
AlmoxarifadoAPI/
└── Almoxarifado.API/
    ├── Data/
    │   └── AppDbContext.cs        # Contexto do Entity Framework (Mapeamento do Banco)
    ├── Migrations/                # Histórico e scripts de evolução do banco de dados
    ├── Models/
    │   └── Funcionario.cs         # Entidade de domínio 'Funcionario' (Data Annotations)
    ├── Properties/
    │   └── launchSettings.json    # Configurações de execução
    ├── appsettings.json           # String de conexão e variáveis de ambiente
    └── Program.cs                 # Configuração de Injeção de Dependência e Middlewares
