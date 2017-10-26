using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using FAAPI.Repository.CustomMapping;


namespace FAAPI.Repository.Base
{
    public class DapperFluentMapInitializer
    {
        public static void Init()
        {
            FluentMapper.Initialize(config =>
            {
                config.AddMap(new PessoaMap());
                config.AddMap(new EmpresaMap());
                config.AddMap(new SessaoMap());
                config.AddMap(new UsuarioMap());


                config.ForDommel();
            });

        }
    }
}