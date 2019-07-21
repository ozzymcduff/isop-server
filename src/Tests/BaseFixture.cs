using System;
using Isop.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using With;

namespace Isop.Tests.Server
{
    public class BaseFixture
    {

        public static Browser GetBrowser(Action<ConfigurationBuilder> configuration=null, Action<BrowserContext> defaults=null)
        {
            return GetBrowser<FakeIsopServer>(configuration, defaults);
        }
        public static Browser GetBrowser<TISopServer>(Action<ConfigurationBuilder> configuration=null, Action<BrowserContext> defaults=null) where TISopServer : class, IIsopServer
        {
            var _config = new ConfigurationBuilder()
                .Tap(configuration)
                .Build();
            var host = new WebHostBuilder()
                .ConfigureLogging((hostingContext, logging) => {
                    logging.AddConsole().AddDebug();
                })
                .UseConfiguration(_config)
                .UseStartup<TISopServer>()
                .UseDefaultServiceProvider((context, options) =>
                {
                    options.ValidateScopes = true;
                });
            return new Browser(new TestServer(host), defaults);
        }
    }
}

