# Malote-Digital 

O **Malote-Digital** é um ecossistema de gerenciamento financeiro e automação de rotinas administrativas desenvolvido sob medida para otimizar o fluxo de caixa de condomínios residenciais. 

O sistema foi projetado para resolver um problema real: a conferência manual exaustiva de boletos, relatórios de DDA e extratos bancários, transformando processos propensos a erros humanos em uma esteira automatizada e auditável.

---

## O Problema & A Solução (Visão de Negócio)

Na gestão condominial tradicional, o controle de contas a pagar envolve o cruzamento manual de despesas recorrentes (água, luz, folha de pagamento) e despesas sazonais extraídas do DDA (boletos emitidos contra o CNPJ). 

O **Malote-Digital** centraliza e automatiza esse fluxo dividindo-o em duas frentes inteligentes:

1. **Gestão de Contas Fixas (Aba 1):** Cadastradas uma única vez, essas despesas são resetadas de forma autônoma para o status `Pendente` todo dia 1º do mês através de um Worker em background. A baixa é visual e manual, garantindo o controle estrito do operador.
2. **Conciliação Automatizada (Aba 2):** Processa arquivos de extrato bancário padrão mercado (`.ofx`), lê as transações de débito, executa um algoritmo de matching (por valor e janela de vencimento) e realiza a baixa automática das despesas extraordinárias vindas do DDA.

---

## 🛠️ Stack Técnica & Arquitetura

O projeto foi construído seguindo os princípios de **Clean Architecture** (Arquitetura Limpa) e os padrões **SOLID**, garantindo o desacoplamento total entre as regras de negócio e os provedores de infraestrutura.

* **Backend:** .NET 10 (ASP.NET Core / Minimal APIs)
* **Persistência & Banco de Dados:** Entity Framework Core (EF Core) e PostgreSQL
* **Engine de Validação:** FluentValidation
* **Criptografia & Segurança:** Cálculo de Hash SHA-256 para Idempotência
* **Testes Automatizados:** xUnit e FluentAssertions
* **CI/CD:** GitHub Actions (Pipeline automatizado de Build, Restore e Test a cada Push/PR)

### 🏗️ Estrutura da Solução (`.slnx`)

```text
├── MaloteDigital.Api/            # Camada de Apresentação (Endpoints, DTOs de Entrada, Filtros)
├── MaloteDigital.Domain/         # O Coração do Sistema (Entidades Ricas, Enums, Interfaces de Contrato)
├── MaloteDigital.Infrastructure/ # Provedores Externos (Implementação do Banco, Parsers de OFX, Workers)
└── MaloteDigital.UnitTests/      # Suíte de Testes Unitários de Alta Performance (xUnit)
