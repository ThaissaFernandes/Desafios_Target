using Desafio1Comissao.Models;
using Desafio1Comissao.Services;

namespace Desafio1Comissao.Tests;

[TestClass]
public class ComissaoServiceTests
{
    private readonly ComissaoService _service = new();

    [TestMethod]
    public void VendaAbaixoDe100NaoGeraComissao()
    {
        Venda venda = new() { Vendedor = "João Silva", Valor = 99.99m };

        decimal resultado = _service.CalcularComissao(venda);

        Assert.AreEqual(0m, resultado);
    }

    [TestMethod]
    public void VendaDe100GeraComissaoDe1PorCento()
    {
        Venda venda = new() { Vendedor = "João Silva", Valor = 100m };

        decimal resultado = _service.CalcularComissao(venda);

        Assert.AreEqual(1m, resultado);
    }

    [TestMethod]
    public void VendaAbaixoDe500GeraComissaoDe1PorCento()
    {
        Venda venda = new() { Vendedor = "João Silva", Valor = 499.99m };

        decimal resultado = _service.CalcularComissao(venda);

        Assert.AreEqual(4.9999m, resultado);
    }

    [TestMethod]
    public void VendaDe500GeraComissaoDe5PorCento()
    {
        Venda venda = new() { Vendedor = "João Silva", Valor = 500m };

        decimal resultado = _service.CalcularComissao(venda);

        Assert.AreEqual(25m, resultado);
    }

    [TestMethod]
    public void VendaComValorNegativoDeveSerRejeitada()
    {
        Venda venda = new() { Vendedor = "João Silva", Valor = -1m };

        Assert.ThrowsException<ArgumentException>(
            () => _service.CalcularComissao(venda));
    }

    [TestMethod]
    public void VendaComVendedorVazioDeveSerRejeitada()
    {
        Venda venda = new() { Vendedor = "   ", Valor = 100m };

        Assert.ThrowsException<ArgumentException>(
            () => _service.CalcularComissao(venda));
    }

    [TestMethod]
    public void ComissaoDeveSerCalculadaPorVenda()
    {
        Venda vendaDe100 = new() { Vendedor = "Maria Souza", Valor = 100m };
        Venda vendaDe500 = new() { Vendedor = "Maria Souza", Valor = 500m };

        decimal totalComissao =
            _service.CalcularComissao(vendaDe100) +
            _service.CalcularComissao(vendaDe500);

        Assert.AreEqual(26m, totalComissao);
    }
}
