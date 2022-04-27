using System;
using System.Collections.Generic;

namespace Domain.Commons.Entities
{
    public partial class Filme
    {
        public int Id { get; set; }
        public int IdDiretor { get; set; }
        public string Nome { get; set; } = null!;

        public virtual Diretor IdDiretorNavigation { get; set; } = null!;
    }
}
