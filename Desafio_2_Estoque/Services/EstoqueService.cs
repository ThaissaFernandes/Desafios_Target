using Desafio2Estoque.Models;

namespace Desafio2Estoque.Services;

public class EstoqueService
{
    public int MovimentarEstoque(
        Produto produto,
        MovimentacaoEstoque movimentacao)
    {
        ValidarProduto(produto);
        ValidarMovimentacao(movimentacao);

        if (movimentacao.CodigoProduto != produto.CodigoProduto)
        {
            throw new ArgumentException(
                "A movimentação não corresponde ao produto informado.",
                nameof(movimentacao));
        }

        if (movimentacao.Tipo.Equals(
                "entrada",
                StringComparison.OrdinalIgnoreCase))
        {
            if (movimentacao.Quantidade > int.MaxValue - produto.Estoque)
            {
                throw new OverflowException(
                    "A quantidade da entrada excede o limite de estoque permitido.");
            }

            produto.Estoque += movimentacao.Quantidade;
            return produto.Estoque;
        }

        if (movimentacao.Tipo.Equals(
                "saida",
                StringComparison.OrdinalIgnoreCase))
        {
            if (movimentacao.Quantidade > produto.Estoque)
            {
                throw new InvalidOperationException(
                    "Não há estoque suficiente para realizar a saída.");
            }

            produto.Estoque -= movimentacao.Quantidade;
            return produto.Estoque;
        }

        throw new ArgumentException(
            "O tipo de movimentação deve ser 'entrada' ou 'saida'.",
            nameof(movimentacao));
    }

    public void ValidarProduto(Produto produto)
    {
        ArgumentNullException.ThrowIfNull(produto);

        if (produto.Estoque < 0)
        {
            throw new ArgumentException(
                "O estoque inicial não pode ser negativo.",
                nameof(produto));
        }
    }

    private static void ValidarMovimentacao(
        MovimentacaoEstoque movimentacao)
    {
        ArgumentNullException.ThrowIfNull(movimentacao);

        if (movimentacao.Quantidade <= 0)
        {
            throw new ArgumentException(
                "A quantidade deve ser maior que zero.",
                nameof(movimentacao));
        }

        if (movimentacao.Id <= 0)
        {
            throw new ArgumentException(
                "O identificador da movimentação deve ser maior que zero.",
                nameof(movimentacao));
        }

        if (string.IsNullOrWhiteSpace(movimentacao.Descricao))
        {
            throw new ArgumentException(
                "A descrição da movimentação é obrigatória.",
                nameof(movimentacao));
        }

        if (!movimentacao.Tipo.Equals(
                "entrada",
                StringComparison.OrdinalIgnoreCase) &&
            !movimentacao.Tipo.Equals(
                "saida",
                StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException(
                "O tipo de movimentação deve ser 'entrada' ou 'saida'.",
                nameof(movimentacao));
        }
    }
}
