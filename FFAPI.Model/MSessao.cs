using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAAPI.Model
{
    public class MSessao
    {
        public Guid ID { get; set; }
        public Guid IDUsuario { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataValidade { get; set; }
    }
}
