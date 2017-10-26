using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FAAPI.Model;
using Dapper.FluentMap.Dommel.Mapping;

namespace FAAPI.Repository.CustomMapping
{
    public class PessoaMap : DommelEntityMap<MPessoa>
    {
        public PessoaMap()
        {
            ToTable("Tb_Pessoa");
            Map(p => p.ID).ToColumn("ID", Equals(false)).IsKey();
            Map(p => p.Nome).ToColumn("Nome", Equals(false));
            Map(p => p.Documento).ToColumn("Documento", Equals(false));
            Map(p => p.Ativo).ToColumn("Ativo", Equals(false));
            Map(p => p.Validado).ToColumn("Validado", Equals(false));
            Map(p => p.DataNascimentoRegistro).ToColumn("DataRegistro", Equals(false));

            //Map(p => p.Tabela).Ignore();

        }
    }
}
