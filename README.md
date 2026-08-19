# 📦 Almoxarifado API (.NET 10)

API RESTful robusta desenvolvida em **C# / .NET 10** com **Entity Framework Core** e **MySQL (Pomelo)** para gerenciamento completo de controle de estoque, funcionários, fornecedores, entrada de materiais via Nota Fiscal e auditoria de devoluções patrimoniais.

---

## 🛠️ Tecnologias Utilizadas

- **Linguagem / Framework:** C# | .NET 10.0
- **Acesso a Dados:** Entity Framework Core (ORM)
- **Banco de Dados:** MySQL 8.0+ / MariaDB (Driver Pomelo)
- **Documentação & Testes Interativos:** OpenAPI / Swagger UI
- **Arquitetura:** MVC / Web API com Padrão REST

---

## ⚙️ Funcionalidades e Módulos

### 👨‍💼 1. Gestão de Funcionários
- Cadastro, atualização, listagem e inativação de colaboradores.
- Controle por CPF único, cargo e e-mail corporativo.

### 📋 2. Controle de Estoque & Itens
- Distinção clara de regras de negócio para dois tipos de produtos:
  - **Consumíveis (1):** Baixa definitiva do estoque no atendimento.
  - **Retornáveis (2):** Transferência automática para o estoque empenhado durante o uso.
- Cálculo automático de **Estoque Total** (`QuantidadeDisponivel` + `QuantidadeEmpenhada`).
- Endpoint de ajuste e sincronização de saldos de estoque.

### 🔄 3. Requisições e Devoluções (Auditável)
- Ciclo de vida completo das requisições (*Pendente*, *Aprovada*, *Atendida*, *Recusada*, *Cancelada*).
- **Módulo de Devoluções Formalizado:** Registro de devolução de itens retornáveis com vínculo de funcionário, data/hora exata, observações e condição do equipamento (*ex: "Em perfeito estado"*).

### 🏭 4. Fornecedores e Entrada de Notas Fiscais
- Cadastro completo de fornecedores por CNPJ.
- Entrada de mercadorias vinculadas à Nota Fiscal (NF-e).
- Incremento automático e seguro do saldo de estoque no recebimento das notas.

---

## 🚀 Como Executar o Projeto Localmente

### Pré-requisitos
- [.NET 10.0 SDK](https://dotnet.microsoft.com/download)
- [MySQL Server 8.0+](https://dev.mysql.com/downloads/installer/)

### 1. Clonar o repositório
```bash
git clone [https://github.com/seu-usuario/AlmoxarifadoAPI.git](https://github.com/seu-usuario/AlmoxarifadoAPI.git)
cd AlmoxarifadoAPI/Almoxarifado.API