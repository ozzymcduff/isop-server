using Nancy;
using Isop.Server;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using Nancy.Testing;
using System;
namespace Isop.Tests.Server
{
    public class TestBootstrapperWithIsopServer <TISopServer>: ConfigurableBootstrapper 
        where TISopServer: class, IIsopServer
    {
        public TestBootstrapperWithIsopServer (Action<ConfigurableBootstrapperConfigurator> configuration):base(configuration)
        {
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer existingContainer)
        {
            base.ConfigureApplicationContainer(existingContainer);
            // Perform registation that should have an application lifetime
            existingContainer
                .Register<IIsopServer, TISopServer>();
        }
    }
}
