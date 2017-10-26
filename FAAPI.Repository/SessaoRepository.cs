using FAAPI.Model;
using FAAPI.Repository.Base;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAAPI.Repository
{
    public class SessaoRepository : RepositoryBase<MSessao>
    {

        private const double _ValidadePadrao = 30;

        private double GetMinutosValidadeSessao()
        {
            double m = _ValidadePadrao;
            try
            {
                m = double.Parse(ConfigurationManager.AppSettings["ValidadeSessao"].ToString());
                return m;
            }
            catch (Exception)
            {
                return _ValidadePadrao;
            }
        }

        public MSessao GetSessaoValida(Guid id)
        {
            MSessao sessao;
            try
            {
                sessao = base.GetById(id);
                if (sessao.DataValidade >= DateTime.Now)
                {
                    sessao.DataValidade = DateTime.Now.AddMinutes(GetMinutosValidadeSessao());
                    base.Update(sessao);
                    return sessao;
                }
                else
                {
                    base.Delete(id);
                    return null;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
