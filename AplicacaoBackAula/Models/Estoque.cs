using System;
using System.Collections.Generic;

namespace AplicacaoBackAula.Models;

public partial class Estoque
{
    public int Iditem { get; set; }

    public int QuantidadeEstoque { get; set; }

    public virtual ICollection<Entradaproduto> Entradaprodutos { get; set; } = new List<Entradaproduto>();

    public virtual Produto IditemNavigation { get; set; } = null!;
}
