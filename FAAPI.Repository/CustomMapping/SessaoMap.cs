using Dapper.FluentMap.Dommel.Mapping;
using FAAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAAPI.Repository.CustomMapping
{
    public class SessaoMap : DommelEntityMap<MSessao>
    {
        public SessaoMap()
        {
            ToTable("Tb_Sessao");
            Map(p => p.ID).ToColumn("ID", Equals(false)).IsKey();
            Map(p => p.IDUsuario).ToColumn("IDUsuario", Equals(false));
            Map(p => p.DataCriacao).ToColumn("DataCriacao", Equals(false));
            Map(p => p.DataValidade).ToColumn("DataValidade", Equals(false));

        }

    }
}

