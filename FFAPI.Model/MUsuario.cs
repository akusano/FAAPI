using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAAPI.Model
{
    public class MUsuario
    {
        public Guid ID { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public Boolean Administrador { get; set; }
        public Boolean Ativo { get; set; }
        public MPessoa Pessoa { get; set; }
        public MEmpresa Empresa { get; set; }


        public Guid IDEmpresa
        { get { return Empresa.ID; } set { Empresa.ID = value; } }

        public Guid IDPessoa
        { get { return Pessoa.ID; } set { Pessoa.ID = value; } }


        
        public MUsuario()
        {
            this.Empresa = new MEmpresa();
            this.Pessoa = new MPessoa();
        }


    }
}
