using Desafio1Comissao.Models;

namespace Desafio1Comissao.Services;

public class ComissaoService
{
    private const decimal LimiteSemComissao = 100m;
    private const decimal LimiteComissaoUmPorCento = 500m;

    private const decimal TaxaUmPorCento = 0.01m;
    private const decimal TaxaCincoPorCento = 0.05m;

    public decimal CalcularComissao(Venda venda)
    {
        ValidarVenda(venda);

        decimal taxa = ObterTaxaComissao(venda.Valor);
        return venda.Valor * taxa;
    }

    public void ValidarVenda(Venda venda)
    {
        ArgumentNullException.ThrowIfNull(venda);

        if (string.IsNullOrWhiteSpace(venda.Vendedor))
        {
            throw new ArgumentException(
                "O nome do vendedor é obrigatório.",
                nameof(venda));
        }

        if (venda.Valor < 0)
        {
            throw new ArgumentException(
                "O valor da venda não pode ser negativo.",
                nameof(venda));
        }
    }

    private static decimal ObterTaxaComissao(decimal valor)
    {
        if (valor < LimiteSemComissao)
        {
            return 0m;
        }

        if (valor < LimiteComissaoUmPorCento)
        {
            return TaxaUmPorCento;
        }

        return TaxaCincoPorCento;
    }
}
