using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FAAPI.Model;
using Dapper.FluentMap.Dommel.Mapping;

namespace FAAPI.Repository.CustomMapping
{
    public class UsuarioMap : DommelEntityMap<MUsuario>
    {
        public UsuarioMap()
        {
            ToTable("Tb_Usuario");
            Map(p => p.ID).ToColumn("ID", Equals(false)).IsKey();
            Map(p => p.Email).ToColumn("Email", Equals(false));
            Map(p => p.Senha).ToColumn("Senha", Equals(false));
            Map(p => p.Administrador).ToColumn("Administrador", Equals(false));
            Map(p => p.Ativo).ToColumn("Ativo", Equals(false));
            Map(p => p.IDEmpresa).ToColumn("IDEmpresa", Equals(false));
            Map(p => p.IDPessoa).ToColumn("IDPessoa", Equals(false));


            Map(p => p.Pessoa).Ignore();
            Map(p => p.Empresa).Ignore();


        }
    }
}
