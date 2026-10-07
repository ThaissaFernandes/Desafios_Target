using Desafio3Juros.Models;

namespace Desafio3Juros.Services;

public class JurosService
{
    private const decimal TaxaJurosDiaria = 0.025m;

    public decimal CalcularJuros(
        Divida divida,
        DateTime dataAtual)
    {
        ValidarDivida(divida);

        int diasAtraso = CalcularDiasAtraso(
            divida.DataVencimento,
            dataAtual);

        return divida.Valor * TaxaJurosDiaria * diasAtraso;
    }

    public int CalcularDiasAtraso(
        DateTime dataVencimento,
        DateTime dataAtual)
    {
        DateTime vencimento = dataVencimento.Date;
        DateTime calculo = dataAtual.Date;

        if (calculo <= vencimento)
        {
            return 0;
        }

        return (calculo - vencimento).Days;
    }

    public void ValidarDivida(Divida divida)
    {
        ArgumentNullException.ThrowIfNull(divida);

        if (divida.Valor < 0)
        {
            throw new ArgumentException(
                "O valor da dívida não pode ser negativo.",
                nameof(divida));
        }
    }
}
