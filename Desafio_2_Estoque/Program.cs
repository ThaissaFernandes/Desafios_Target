using System.Text.Json;
using Desafio2Estoque.Models;
using Desafio2Estoque.Services;

string caminhoArquivo = Path.Combine(
    AppContext.BaseDirectory,
    "Data",
    "estoque.json");

if (!File.Exists(caminhoArquivo))
{
    Console.WriteLine("Arquivo de estoque não encontrado.");
    return;
}

try
{
    string json = File.ReadAllText(caminhoArquivo);

    using JsonDocument documento = JsonDocument.Parse(json);

    if (!documento.RootElement.TryGetProperty(
            "estoque",
            out JsonElement estoqueJson))
    {
        Console.WriteLine("O arquivo não possui a propriedade 'estoque'.");
        return;
    }

    JsonSerializerOptions opcoes = new()
    {
        PropertyNameCaseInsensitive = true
    };

    List<Produto> produtos = JsonSerializer.Deserialize<List<Produto>>(
        estoqueJson.GetRawText(),
        opcoes) ?? [];

    if (produtos.Count == 0)
    {
        Console.WriteLine("Nenhum produto encontrado no estoque.");
        return;
    }

    EstoqueService estoqueService = new();

    foreach (Produto produto in produtos)
    {
        estoqueService.ValidarProduto(produto);
    }

    int proximoId = 1;
    bool continuar = true;

    Console.WriteLine("=== MOVIMENTAÇÃO DE ESTOQUE ===");
    Console.WriteLine();

    while (continuar)
    {
        Console.WriteLine("Produtos disponíveis:");
        Console.WriteLine();

        foreach (Produto produto in produtos)
        {
            Console.WriteLine(
                $"{produto.CodigoProduto} - " +
                $"{produto.DescricaoProduto} - " +
                $"Estoque: {produto.Estoque}");
        }

        Console.WriteLine();
        Console.Write("Digite o código do produto: ");

        if (!int.TryParse(Console.ReadLine(), out int codigoProduto))
        {
            Console.WriteLine("Código do produto inválido.");
            Console.WriteLine();
            continue;
        }

        Produto? produtoSelecionado = produtos.FirstOrDefault(
            produto => produto.CodigoProduto == codigoProduto);

        if (produtoSelecionado is null)
        {
            Console.WriteLine("Produto não encontrado.");
            Console.WriteLine();
            continue;
        }

        Console.Write("Digite o tipo da movimentação (entrada/saida): ");
        string tipo = Console.ReadLine()?.Trim() ?? string.Empty;

        if (!tipo.Equals("entrada", StringComparison.OrdinalIgnoreCase) &&
            !tipo.Equals("saida", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine(
                "Tipo de movimentação inválido. Informe 'entrada' ou 'saida'.");
            Console.WriteLine();
            continue;
        }

        Console.Write("Digite a quantidade: ");

        if (!int.TryParse(Console.ReadLine(), out int quantidade))
        {
            Console.WriteLine("Quantidade inválida.");
            Console.WriteLine();
            continue;
        }

        if (quantidade <= 0)
        {
            Console.WriteLine("A quantidade deve ser maior que zero.");
            Console.WriteLine();
            continue;
        }

        if (tipo.Equals("saida", StringComparison.OrdinalIgnoreCase) &&
            quantidade > produtoSelecionado.Estoque)
        {
            Console.WriteLine(
                "Não há estoque suficiente para realizar a saída.");
            Console.WriteLine();
            continue;
        }

        if (tipo.Equals("entrada", StringComparison.OrdinalIgnoreCase) &&
            quantidade > int.MaxValue - produtoSelecionado.Estoque)
        {
            Console.WriteLine(
                "A quantidade da entrada excede o limite de estoque permitido.");
            Console.WriteLine();
            continue;
        }

        Console.Write("Digite a descrição da movimentação: ");
        string descricao = Console.ReadLine()?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(descricao))
        {
            Console.WriteLine("A descrição da movimentação é obrigatória.");
            Console.WriteLine();
            continue;
        }

        MovimentacaoEstoque movimentacao = new()
        {
            Id = proximoId,
            CodigoProduto = produtoSelecionado.CodigoProduto,
            Tipo = tipo,
            Quantidade = quantidade,
            Descricao = descricao
        };

        try
        {
            int estoqueFinal = estoqueService.MovimentarEstoque(
                produtoSelecionado,
                movimentacao);

            Console.WriteLine();
            Console.WriteLine("Movimentação realizada com sucesso!");
            Console.WriteLine($"ID: {movimentacao.Id}");
            Console.WriteLine(
                $"Produto: {produtoSelecionado.DescricaoProduto}");
            Console.WriteLine($"Tipo: {movimentacao.Tipo}");
            Console.WriteLine($"Descrição: {movimentacao.Descricao}");
            Console.WriteLine(
                $"Quantidade movimentada: {movimentacao.Quantidade}");
            Console.WriteLine($"Estoque final: {estoqueFinal}");
            Console.WriteLine();

            proximoId++;
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine();
            Console.WriteLine($"Erro: {ex.Message}");
            Console.WriteLine();
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine();
            Console.WriteLine($"Erro: {ex.Message}");
            Console.WriteLine();
        }
        catch (OverflowException ex)
        {
            Console.WriteLine();
            Console.WriteLine($"Erro: {ex.Message}");
            Console.WriteLine();
        }

        Console.Write("Deseja realizar outra movimentação? (s/n): ");
        string resposta = Console.ReadLine()?.Trim().ToLowerInvariant() ?? "n";
        continuar = resposta == "s";
        Console.WriteLine();
    }
}
catch (JsonException)
{
    Console.WriteLine(
        "Não foi possível ler o arquivo de estoque. Verifique se o JSON está válido.");
}
catch (IOException)
{
    Console.WriteLine("Não foi possível acessar o arquivo de estoque.");
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Erro de validação: {ex.Message}");
}
