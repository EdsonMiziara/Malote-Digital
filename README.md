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

```mermaid
graph TD
    %% Estilos de Alto Contraste para Dark/Light Mode
    classDef infra stroke:#0288d1,stroke-width:3px,stroke-dasharray: 5 5;
    classDef dominio stroke:#43a047,stroke-width:3px;
    classDef seguranca stroke:#e53935,stroke-width:3px;
    classDef db stroke:#8d6e63,stroke-width:3px;

    %% Camada de Entrada
    subgraph Camada_de_Entrada [1. Camada de Apresentação / API]
        A[Requisição HTTP / Upload de Arquivo] --> B[Minimal APIs - .NET 8]
        B --> C[Manipulação Segura de Stream - stream.Position = 0]
    end
    class B,C infra;

    %% Camada de Domínio
    subgraph Camada_de_Dominio [2. Núcleo do Negócio - Rich Domain Model]
        C --> D[Motor de Regras de Negócio]
        D --> E[Cálculo de Agendamento - Regra Dia Coringa 0]
        D --> F[Validador de Idempotência - Geração de Hash SHA-256]
        D --> G[Travas de Ciclo de Vida da Despesa]
    end
    class D,E,F,G dominio;

    %% Camada de Persistência e Segurança
    subgraph Camada_de_Persistencia [3. Infraestrutura de Dados & Auditoria]
        F & G & E --> H[Entity Framework Core - DbContext Customizado]
        
        subgraph Interceptador_Seguranca [Barreira de Segurança Estrita]
            H --> I[Sobrescrita do SaveChangesAsync]
            I --> J{Operação é UPDATE ou DELETE?}
            J -- Sim --> K[Bloqueio de Operação / Exception]
            J -- Não --> L[Permite Apenas Inserção - Histórico Append-Only]
        end
    end
    class H,I,J,K,L seguranca;

    %% Banco de Dados
    L --> M[(PostgreSQL)]
    class M db;
