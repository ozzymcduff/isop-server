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
        public static Browser GetBrowser()
        {
            return GetBrowser<FakeIsopServer>();
        }
        public static Browser GetBrowser<TISopServer>() where TISopServer : class, IIsopServer
        {
            var bootstrapper = new TestBootstrapperWithIsopServer<TISopServer>(with=>{
                with.DisableAutoRegistrations();
                with.Module<IndexModule>();
                with.Module<ControllerModule>();
            });
            var browser = new Browser(bootstrapper);
            return browser;
        }
    }
}

