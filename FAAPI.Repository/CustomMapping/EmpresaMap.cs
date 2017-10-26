using Dapper.FluentMap.Dommel.Mapping;
using FAAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAAPI.Repository.CustomMapping
{
    public class EmpresaMap : DommelEntityMap<MEmpresa>
    {
        public EmpresaMap()
        {
            ToTable("Tb_Empresa");
            Map(p => p.ID).ToColumn("ID", Equals(false)).IsKey();
            Map(p => p.NomeFantasia).ToColumn("NomeFantasia", Equals(false));
        }

    }
}

