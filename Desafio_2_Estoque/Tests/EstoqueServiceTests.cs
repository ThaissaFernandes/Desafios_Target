using Desafio2Estoque.Models;
using Desafio2Estoque.Services;

namespace Desafio2Estoque.Tests;

[TestClass]
public class EstoqueServiceTests
{
    private readonly EstoqueService _service = new();

    [TestMethod]
    public void EntradaDeveAumentarOEstoque()
    {
        Produto produto = CriarProduto(100);
        MovimentacaoEstoque movimentacao = CriarMovimentacao("entrada", 25);

        int resultado = _service.MovimentarEstoque(produto, movimentacao);

        Assert.AreEqual(125, resultado);
        Assert.AreEqual(125, produto.Estoque);
    }

    [TestMethod]
    public void SaidaDeveDiminuirOEstoque()
    {
        Produto produto = CriarProduto(100);
        MovimentacaoEstoque movimentacao = CriarMovimentacao("saida", 25);

        int resultado = _service.MovimentarEstoque(produto, movimentacao);

        Assert.AreEqual(75, resultado);
        Assert.AreEqual(75, produto.Estoque);
    }

    [TestMethod]
    public void QuantidadeZeroDeveSerRejeitada()
    {
        Produto produto = CriarProduto(100);
        MovimentacaoEstoque movimentacao = CriarMovimentacao("entrada", 0);

        Assert.ThrowsException<ArgumentException>(
            () => _service.MovimentarEstoque(produto, movimentacao));
    }

    [TestMethod]
    public void QuantidadeNegativaDeveSerRejeitada()
    {
        Produto produto = CriarProduto(100);
        MovimentacaoEstoque movimentacao = CriarMovimentacao("entrada", -1);

        Assert.ThrowsException<ArgumentException>(
            () => _service.MovimentarEstoque(produto, movimentacao));
    }

    [TestMethod]
    public void TipoInvalidoDeveSerRejeitado()
    {
        Produto produto = CriarProduto(100);
        MovimentacaoEstoque movimentacao = CriarMovimentacao("transferencia", 10);

        Assert.ThrowsException<ArgumentException>(
            () => _service.MovimentarEstoque(produto, movimentacao));
    }

    [TestMethod]
    public void SaidaMaiorQueOEstoqueDeveSerRejeitada()
    {
        Produto produto = CriarProduto(100);
        MovimentacaoEstoque movimentacao = CriarMovimentacao("saida", 101);

        Assert.ThrowsException<InvalidOperationException>(
            () => _service.MovimentarEstoque(produto, movimentacao));
    }

    [TestMethod]
    public void EstoqueInicialNegativoDeveSerRejeitado()
    {
        Produto produto = CriarProduto(-1);

        Assert.ThrowsException<ArgumentException>(
            () => _service.ValidarProduto(produto));
    }

    [TestMethod]
    public void EntradaQueExcedeIntMaxValueDeveSerRejeitada()
    {
        Produto produto = CriarProduto(int.MaxValue);
        MovimentacaoEstoque movimentacao = CriarMovimentacao("entrada", 1);

        Assert.ThrowsException<OverflowException>(
            () => _service.MovimentarEstoque(produto, movimentacao));
    }

    [TestMethod]
    public void CodigoDoProdutoDaMovimentacaoDeveCorresponderAoProduto()
    {
        Produto produto = CriarProduto(100);
        MovimentacaoEstoque movimentacao = CriarMovimentacao("entrada", 10);
        movimentacao.CodigoProduto = 999;

        Assert.ThrowsException<ArgumentException>(
            () => _service.MovimentarEstoque(produto, movimentacao));
    }

    [TestMethod]
    public void DescricaoVaziaDeveSerRejeitada()
    {
        Produto produto = CriarProduto(100);
        MovimentacaoEstoque movimentacao = CriarMovimentacao("entrada", 10);
        movimentacao.Descricao = "   ";

        Assert.ThrowsException<ArgumentException>(
            () => _service.MovimentarEstoque(produto, movimentacao));
    }

    [TestMethod]
    public void IdentificadorNaoPositivoDeveSerRejeitado()
    {
        Produto produto = CriarProduto(100);
        MovimentacaoEstoque movimentacao = CriarMovimentacao("entrada", 10);
        movimentacao.Id = 0;

        Assert.ThrowsException<ArgumentException>(
            () => _service.MovimentarEstoque(produto, movimentacao));
    }

    private static Produto CriarProduto(int estoque) => new()
    {
        CodigoProduto = 101,
        DescricaoProduto = "Caneta Azul",
        Estoque = estoque
    };

    private static MovimentacaoEstoque CriarMovimentacao(
        string tipo,
        int quantidade) => new()
    {
        Id = 1,
        CodigoProduto = 101,
        Tipo = tipo,
        Quantidade = quantidade,
        Descricao = "Movimentação de teste"
    };
}
