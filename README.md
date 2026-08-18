# 📦 Almoxarifado API (.NET 9)

API RESTful robusta desenvolvida em **C# / .NET 9** com **Entity Framework Core** e **MySQL (Pomelo)** para gerenciamento completo de controle de estoque, funcionários, fornecedores e entrada de materiais via Nota Fiscal.

---

## 🛠️ Tecnologias Utilizadas

- **Linguagem / Framework:** C# | .NET 9.0
- **Acesso a Dados:** Entity Framework Core 9.0 (ORM)
- **Banco de Dados:** MySQL 8.0+ / MariaDB (Driver Pomelo)
- **Documentação & Testes Interativos:** OpenAPI / Swagger UI (Swashbuckle)
- **Arquitetura:** MVC / Web API com Padrão REST

---

## ⚙️ Funcionalidades e Módulos

### 👨‍💼 1. Gestão de Funcionários
- Cadastro, atualização, listagem e inativação de colaboradores.
- Controle por CPF único, cargo e departamento.

### 📋 2. Controle de Estoque & Itens
- Distinção entre produtos **Consumíveis** e **Patrimoniais/Retornáveis**.
- Cálculo dinâmico de **Estoque Total** (`QuantidadeDisponivel` + `QuantidadeEmpenhada`).
- Validação automática de código do item.

### 🏭 3. Fornecedores e Notas Fiscais *(Em desenvolvimento)*
- Cadastro completo de fornecedores por CNPJ.
- Entrada de mercadorias vinculadas a Nota Fiscal (NF-e).
- Atualização automática de saldo de estoque no recebimento.

---

## 🚀 Como Executar o Projeto Localmente

### Pré-requisitos
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [MySQL Server 8.0+](https://dev.mysql.com/downloads/installer/)

### 1. Clonar o repositório
```bash
git clone [https://github.com/seu-usuario/AlmoxarifadoAPI.git](https://github.com/seu-usuario/AlmoxarifadoAPI.git)
cd AlmoxarifadoAPI/Almoxarifado.API