using Nancy;
using Nancy.Testing;
using NUnit.Framework;
using System.Linq;
using With.Rubyfy;
using With;
using Nancy.Bootstrapper;
using Nancy.Helpers;
using Isop.Server;
using System.Collections.Generic;

namespace Isop.Tests.Server
{
    
    [TestFixture]
    public class JsonApiGivenServerWithSingleIntActionTests
    {
        private static Browser GetBrowser()
        {
            return GetBrowser<FakeIsopServer>();
        }
        private static Browser GetBrowser<TISopServer>() where TISopServer : class,IIsopServer
        {
            var bootstrapper = new TestBootstrapperWithIsopServer<TISopServer>();
            var browser = new Browser(bootstrapper, defaults: to => to.Accept("application/json"));
            return browser;
        }
        private Browser browser;
        [TestFixtureSetUp]
        public void BeforeEachTest ()
        {
            // Given
            browser = GetBrowser<FakeIsopServerWithSingleIntAction>();
        }

        class FakeIsopServerWithSingleIntAction : IsopServerFromBuild
        {
            public FakeIsopServerWithSingleIntAction()
                : base( ()=> new Build { typeof(Isop.Tests.FakeControllers.SingleIntAction) })
            {
            }
        }
        public void Post_form_action2()
        {
            // When
            var result = browser.Post("/SingleIntAction/Action/", with =>
                {
                    with.HttpRequest();
                    with.FormValue("param", "1");
                });

            // Then
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [Test]
        public void Post_form_action_with()
        {
            // When
            var result = browser.Post("/SingleIntAction/Action/", with =>
                {
                    with.HttpRequest();
                });

            // Then
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            var response = result.Body.DeserializeJson<Isop.Server.Models.MissingArgument[]>().Single();

            Assert.That(response.Argument, Is.EquivalentTo("param" ));
        }

        [Test]
        public void Post_form_action_with_wrong_value()
        {
            // When
            var result = browser.Post("/SingleIntAction/Action/", with =>
                {
                    with.HttpRequest();
                    with.FormValue("param", "asdf");
                });

            // Then
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            var response = result.Body.DeserializeJson<Isop.Server.Models.TypeConversionFailed[]>().Single();

            Assert.That(response.Argument, Is.EqualTo("param"), "Arg");
            Assert.That(response.TargetType, Is.EqualTo("System.Int32"), "TargetType");
            Assert.That(response.Value, Is.EqualTo("asdf"), "Value");
        }
    }
}
