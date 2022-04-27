using System;
using System.Collections.Generic;

namespace Domain.Commons.Entities
{
    public partial class Diretor
    {
        public Diretor()
        {
            Filme = new HashSet<Filme>();
        }

        public int Id { get; set; }
        public string Nome { get; set; } = null!;

        public virtual ICollection<Filme> Filme { get; set; }
    }
}
