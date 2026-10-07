using System.Globalization;
using Desafio3Juros.Models;
using Desafio3Juros.Services;

CultureInfo culturaBrasileira = CultureInfo.GetCultureInfo("pt-BR");

Console.WriteLine("=== CÁLCULO DE JUROS POR ATRASO ===");
Console.WriteLine();

Console.Write("Digite o valor: R$ ");

if (!decimal.TryParse(
        Console.ReadLine(),
        NumberStyles.Number,
        culturaBrasileira,
        out decimal valor))
{
    Console.WriteLine("Valor inválido. Informe um valor no formato brasileiro, por exemplo, 1000,50.");
    return;
}

if (valor < 0)
{
    Console.WriteLine("O valor da dívida não pode ser negativo.");
    return;
}

Console.Write("Digite a data de vencimento (dd/MM/yyyy): ");

if (!DateTime.TryParseExact(
        Console.ReadLine(),
        "dd/MM/yyyy",
        CultureInfo.InvariantCulture,
        DateTimeStyles.None,
        out DateTime dataVencimento))
{
    Console.WriteLine("Data de vencimento inválida.\nUse o formato dd/MM/yyyy.");
    return;
}

Divida divida = new()
{
    Valor = valor,
    DataVencimento = dataVencimento
};

JurosService jurosService = new();
DateTime dataHoje = DateTime.Today;

try
{
    int diasAtraso = jurosService.CalcularDiasAtraso(
        divida.DataVencimento,
        dataHoje);

    decimal juros = jurosService.CalcularJuros(
        divida,
        dataHoje);

    decimal valorTotal = divida.Valor + juros;

    Console.WriteLine();
    Console.WriteLine("=== RESULTADO ===");
    Console.WriteLine(
        $"Valor original: {divida.Valor.ToString("C", culturaBrasileira)}");
    Console.WriteLine(
        $"Data de vencimento: {divida.DataVencimento:dd/MM/yyyy}");
    Console.WriteLine($"Data atual: {dataHoje:dd/MM/yyyy}");
    Console.WriteLine($"Dias de atraso: {diasAtraso}");
    Console.WriteLine(
        $"Juros: {juros.ToString("C", culturaBrasileira)}");
    Console.WriteLine(
        $"Valor total: {valorTotal.ToString("C", culturaBrasileira)}");
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Erro de validação: {ex.Message}");
}
