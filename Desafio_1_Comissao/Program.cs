using System.Globalization;
using System.Text.Json;
using Desafio1Comissao.Models;
using Desafio1Comissao.Services;

string caminhoArquivo = Path.Combine(
    AppContext.BaseDirectory,
    "Data",
    "vendas.json");

if (!File.Exists(caminhoArquivo))
{
    Console.WriteLine("Arquivo de vendas não encontrado.");
    return;
}

try
{
    string json = File.ReadAllText(caminhoArquivo);

    using JsonDocument documento = JsonDocument.Parse(json);

    if (!documento.RootElement.TryGetProperty(
            "vendas",
            out JsonElement vendasJson))
    {
        Console.WriteLine("O arquivo não possui a propriedade 'vendas'.");
        return;
    }

    JsonSerializerOptions opcoes = new()
    {
        PropertyNameCaseInsensitive = true
    };

    List<Venda> vendas = JsonSerializer.Deserialize<List<Venda>>(
        vendasJson.GetRawText(),
        opcoes) ?? [];

    if (vendas.Count == 0)
    {
        Console.WriteLine("Nenhuma venda encontrada.");
        return;
    }

    ComissaoService comissaoService = new();

    foreach (Venda venda in vendas)
    {
        comissaoService.ValidarVenda(venda);
    }

    CultureInfo culturaBrasileira = CultureInfo.GetCultureInfo("pt-BR");

    Console.WriteLine("=== RELATÓRIO DE COMISSÕES ===");
    Console.WriteLine();

    var vendasPorVendedor = vendas
        .GroupBy(venda => venda.Vendedor.Trim())
        .OrderBy(grupo => grupo.Key);

    foreach (var grupo in vendasPorVendedor)
    {
        decimal totalVendas = grupo.Sum(venda => venda.Valor);
        decimal totalComissao = grupo.Sum(
            venda => comissaoService.CalcularComissao(venda));

        Console.WriteLine($"Vendedor: {grupo.Key}");
        Console.WriteLine(
            $"Total de vendas: {totalVendas.ToString("C", culturaBrasileira)}");
        Console.WriteLine(
            $"Comissão total: {totalComissao.ToString("C", culturaBrasileira)}");
        Console.WriteLine(new string('-', 50));
    }
}
catch (JsonException)
{
    Console.WriteLine(
        "Não foi possível ler o arquivo de vendas. Verifique se o JSON está válido.");
}
catch (IOException)
{
    Console.WriteLine("Não foi possível acessar o arquivo de vendas.");
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Erro de validação: {ex.Message}");
}
