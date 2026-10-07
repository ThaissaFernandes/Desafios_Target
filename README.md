# Desafio Técnico — Desenvolvedor de Sistemas Júnior

Soluções desenvolvidas para o processo seletivo da Target Sistemas.

Os desafios foram implementados em **C# e .NET**, com foco em lógica de programação, regras de negócio, validação de dados, organização do código e testes automatizados.

## Tecnologias

- C#
- .NET 10
- MSTest
- JSON
- Git

## Pré-requisitos

- .NET SDK 10 instalado e disponível no PATH.
- Git, caso o projeto seja clonado por um repositório Git.

Para verificar a versão do .NET instalada:

```bash
dotnet --version
```

## Estrutura do projeto

```text
Desafios-Tecnicos/
├── .gitignore
├── Desafios-Tecnicos.sln
├── README.md
├── desafio_dev.docx
│
├── Desafio_1_Comissao/
│   ├── Data/
│   │   └── vendas.json
│   ├── Models/
│   │   └── Venda.cs
│   ├── Services/
│   │   └── ComissaoService.cs
│   ├── Tests/
│   │   ├── ComissaoServiceTests.cs
│   │   └── Desafio1Comissao.Tests.csproj
│   ├── Program.cs
│   └── Desafio1Comissao.csproj
│
├── Desafio_2_Estoque/
│   ├── Data/
│   │   └── estoque.json
│   ├── Models/
│   │   ├── MovimentacaoEstoque.cs
│   │   └── Produto.cs
│   ├── Services/
│   │   └── EstoqueService.cs
│   ├── Tests/
│   │   ├── EstoqueServiceTests.cs
│   │   └── Desafio2Estoque.Tests.csproj
│   ├── Program.cs
│   └── Desafio2Estoque.csproj
│
└── Desafio_3_Calculo_Juros/
    ├── Models/
    │   └── Divida.cs
    ├── Services/
    │   └── JurosService.cs
    ├── Tests/
    │   ├── JurosServiceTests.cs
    │   └── Desafio3CalculoJuros.Tests.csproj
    ├── Program.cs
    └── Desafio3Juros.csproj
```

As pastas de código utilizam os nomes **Models** e **Services** para manter uma nomenclatura consistente entre os projetos.

## Desafio 1 — Cálculo de Comissão

O programa lê os registros de vendas fornecidos em `Data/vendas.json` e calcula a comissão correspondente a cada venda.

### Regras

- Venda abaixo de **R$ 100,00**: 0% de comissão.
- Venda de **R$ 100,00 até abaixo de R$ 500,00**: 1% de comissão.
- Venda a partir de **R$ 500,00**: 5% de comissão.

Os limites são inclusivos da seguinte forma:

- **R$ 100,00 pertence à faixa de 1%.**
- **R$ 500,00 pertence à faixa de 5%.**

### Premissa sobre o cálculo

A comissão é calculada **individualmente para cada venda**. O total de comissão de cada vendedor é obtido pela soma das comissões das suas vendas.

Portanto, uma venda de R$ 100,00 e outra de R$ 500,00 do mesmo vendedor são calculadas separadamente, respeitando as faixas correspondentes.

### Validações

- Nome do vendedor não pode ser vazio ou composto apenas por espaços.
- Valor da venda não pode ser negativo.

### Execução

```bash
dotnet run --project Desafio_1_Comissao/Desafio1Comissao.csproj
```

## Desafio 2 — Movimentação de Estoque

O programa permite realizar movimentações de entrada e saída dos produtos presentes no estoque inicial fornecido em `Data/estoque.json`.

Cada movimentação possui:

- Identificador único;
- Código do produto;
- Tipo da movimentação;
- Descrição;
- Quantidade.

Após cada movimentação, o programa informa o estoque final do produto movimentado.

### Regras e validações

- O produto informado precisa existir no estoque inicial.
- O tipo deve ser `entrada` ou `saida`.
- A quantidade deve ser maior que zero.
- Uma saída não pode ultrapassar o estoque disponível.
- O estoque inicial não pode ser negativo.
- A descrição da movimentação é obrigatória.
- O identificador da movimentação deve ser positivo e é gerado sequencialmente pelo programa durante a execução.
- Uma entrada que ultrapasse `int.MaxValue` é rejeitada para evitar overflow.

### Persistência

As movimentações **não são persistidas** em arquivo ou banco de dados. O `estoque.json` representa o estoque inicial e não é alterado pelo programa.

As movimentações realizadas permanecem em memória durante a execução, e o estoque atualizado é utilizado nas movimentações seguintes.

### Execução

```bash
dotnet run --project Desafio_2_Estoque/Desafio2Estoque.csproj
```

## Desafio 3 — Cálculo de Juros

O programa recebe o valor de uma dívida e sua data de vencimento e calcula os juros considerando a data atual.

### Regras e premissas

- A taxa é de **2,5% ao dia**.
- O cálculo utiliza **juros simples**, aplicados sobre o valor original da dívida.
- A fórmula utilizada é:

```text
Juros = Valor da dívida × 0,025 × Dias de atraso
```

- Dívidas ainda não vencidas não geram juros.
- Uma dívida com vencimento na data atual também não gera juros.
- O valor da dívida não pode ser negativo.
- A data deve ser informada no formato `dd/MM/yyyy`.

### Execução

```bash
dotnet run --project Desafio_3_Calculo_Juros/Desafio3Juros.csproj
```

## Cultura monetária

Os valores monetários exibidos e recebidos pelo programa utilizam a cultura **pt-BR**.

Exemplos:

```text
1000,50
R$ 1.000,50
```

Os arquivos JSON continuam utilizando números no formato JSON padrão, enquanto a formatação para apresentação ao usuário utiliza `pt-BR`.

## Testes automatizados

Cada desafio possui seu próprio projeto de testes utilizando **MSTest**.

Os testes cobrem, entre outros cenários:

### Comissão

- Venda abaixo de R$ 100,00;
- Limite de R$ 100,00;
- Valores abaixo de R$ 500,00;
- Limite de R$ 500,00;
- Valor de venda negativo;
- Vendedor vazio ou composto apenas por espaços;
- Cálculo individual da comissão por venda.

### Estoque

- Entrada válida;
- Saída válida;
- Quantidade zero;
- Quantidade negativa;
- Tipo de movimentação inválido;
- Saída com estoque insuficiente;
- Estoque inicial negativo;
- Entrada que causaria overflow de `int`;
- Descrição vazia;
- Identificador inválido.

### Juros

- Dívida não vencida;
- Vencimento na data atual;
- Dívida atrasada;
- Cálculo da taxa diária de 2,5%;
- Valor negativo.

## Comandos de compilação e teste

Na raiz da solução:

### Restaurar dependências

```bash
dotnet restore Desafios-Tecnicos.sln
```

### Compilar a solução

```bash
dotnet build Desafios-Tecnicos.sln
```

### Executar todos os testes

```bash
dotnet test Desafios-Tecnicos.sln
```

### Executar um desafio individualmente

```bash
dotnet run --project Desafio_1_Comissao/Desafio1Comissao.csproj
```

```bash
dotnet run --project Desafio_2_Estoque/Desafio2Estoque.csproj
```

```bash
dotnet run --project Desafio_3_Calculo_Juros/Desafio3Juros.csproj
```

## Organização do código

Os projetos foram separados em responsabilidades:

- **Models:** representam os dados utilizados pelas aplicações.
- **Services:** concentram as principais regras de negócio.
- **Program.cs:** responsável pela interação com o usuário, leitura dos dados e apresentação dos resultados.
- **Data:** contém os arquivos JSON utilizados como dados iniciais.
- **Tests:** contém os testes automatizados das regras de negócio.

Os arquivos JSON são localizados a partir de `AppContext.BaseDirectory`, permitindo que o programa encontre os dados corretamente independentemente do diretório a partir do qual o executável é iniciado.

O arquivo `desafio_dev.docx` contém o enunciado original fornecido para o desafio.

## Arquivos gerados

Arquivos e diretórios gerados durante a compilação ou execução, como `bin/`, `obj/`, executáveis, DLLs, PDBs, resultados de testes e arquivos de cache, não fazem parte do controle de versão e são ignorados pelo `.gitignore`.
