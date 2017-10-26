using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAAPI.Model.Auxiliar
{
    public enum EnumTipoRetorno
    {
      Indefinido = 0,
      Sucesso = 1,
      SucessoSemRetorno = 10,
      Alerta = 50,
      ErroAcesso = 950,
      Erro = 999
    }
}
