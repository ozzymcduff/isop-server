using System;
using Nancy.Testing;
using Isop.Server;

namespace Isop.Tests.Server
{
    public class BaseFixture
    {
        public BaseFixture ()
        {
        }
        public static Browser GetBrowser(Action<ConfigurableBootstrapper.ConfigurableBootstrapperConfigurator> configuration=null, Action<BrowserContext> defaults=null)
        {
            return GetBrowser<FakeIsopServer>(configuration, defaults);
        }
        public static Browser GetBrowser<TISopServer>(Action<ConfigurableBootstrapper.ConfigurableBootstrapperConfigurator> configuration=null, Action<BrowserContext> defaults=null) where TISopServer : class, IIsopServer
        {
            var bootstrapper = new TestBootstrapperWithIsopServer<TISopServer>(with=>{
                with.Module<ControllerModule>();
                if (null!=configuration){
                    configuration(with);
                }else{
                    with.DisableAutoRegistrations();
                    with.Module<IndexModule>();
                }
            });
            var browser = new Browser(bootstrapper, defaults);
            return browser;
        }
    }
}

