using Desafio3Juros.Models;
using Desafio3Juros.Services;

namespace Desafio3CalculoJuros.Tests;

[TestClass]
public class JurosServiceTests
{
    private readonly JurosService _service = new();

    [TestMethod]
    public void DividaNaoVencidaNaoGeraJuros()
    {
        DateTime hoje = new(2026, 10, 7);
        Divida divida = CriarDivida(1000m, hoje.AddDays(1));

        decimal juros = _service.CalcularJuros(divida, hoje);

        Assert.AreEqual(0m, juros);
    }

    [TestMethod]
    public void DividaVencendoHojeNaoGeraJuros()
    {
        DateTime hoje = new(2026, 10, 7);
        Divida divida = CriarDivida(1000m, hoje);

        decimal juros = _service.CalcularJuros(divida, hoje);

        Assert.AreEqual(0m, juros);
        Assert.AreEqual(0, _service.CalcularDiasAtraso(divida.DataVencimento, hoje));
    }

    [TestMethod]
    public void DividaAtrasadaDeveCalcularJurosDeDoisVirgulaCincoPorCentoAoDia()
    {
        DateTime hoje = new(2026, 10, 7);
        Divida divida = CriarDivida(1000m, new DateTime(2026, 10, 4));

        decimal juros = _service.CalcularJuros(divida, hoje);

        Assert.AreEqual(75m, juros);
        Assert.AreEqual(3, _service.CalcularDiasAtraso(divida.DataVencimento, hoje));
    }

    [TestMethod]
    public void ValorNegativoDeveSerRejeitado()
    {
        DateTime hoje = new(2026, 10, 7);
        Divida divida = CriarDivida(-1m, hoje.AddDays(-1));

        Assert.ThrowsException<ArgumentException>(
            () => _service.CalcularJuros(divida, hoje));
    }

    private static Divida CriarDivida(
        decimal valor,
        DateTime dataVencimento) => new()
    {
        Valor = valor,
        DataVencimento = dataVencimento
    };
}
