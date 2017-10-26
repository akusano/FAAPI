using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAAPI.Model.Auxiliar
{
    public class MRetorno
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public string Erro { get; set; }
        public EnumTipoRetorno TipoRetorno { get; set; }

    }
}
