using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAAPI.Model
{
    public class MPessoa
    {
        public Guid ID { get; set; }
        public string Nome { get; set; }
        public string Documento { get; set; }
        public DateTime DataNascimentoRegistro { get; set; }
        public bool Validado { get; set; }
        public bool Ativo { get; set; }


        public void AtualizarDadosPessoa(MPessoa novo)
        {
            try
            {
                if (novo.Validado && !this.Validado)
                {
                    if (novo.Nome != null && novo.Nome.Trim().Length > 0)
                        this.Nome = novo.Nome;

                    if (novo.DataNascimentoRegistro != null)
                        this.DataNascimentoRegistro = novo.DataNascimentoRegistro;

                    if (novo.Documento != null && novo.Documento.Trim().Length > 0)
                        this.Documento = novo.Documento;

                    this.Validado = true;
                }
                else
                {
                    if ((this.Nome == null || this.Nome.Trim().Length < 1) && novo.Nome != null && novo.Nome.Trim().Length > 0)
                        this.Nome = novo.Nome;

                    if ((this.DataNascimentoRegistro == null ) && novo.DataNascimentoRegistro != null )
                        this.DataNascimentoRegistro = novo.DataNascimentoRegistro;

                    if ((this.Documento == null || this.Documento.Trim().Length < 1) && novo.Documento != null && novo.Documento.Trim().Length > 0)
                        this.Documento = novo.Documento;

                }
                this.Ativo = novo.Ativo;
            }
            catch (Exception)
            {
                
            }
        }

    }
}
