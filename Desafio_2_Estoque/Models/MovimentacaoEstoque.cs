namespace Desafio2Estoque.Models;

public class MovimentacaoEstoque
{
    public int Id { get; set; }

    public int CodigoProduto { get; set; }

    public string Descricao { get; set; } = string.Empty;

    public string Tipo { get; set; } = string.Empty;

    public int Quantidade { get; set; }
}
