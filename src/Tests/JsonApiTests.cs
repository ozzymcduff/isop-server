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
    public class JsonApiTests: BaseFixture
    {
        private Browser browser;
        [TestFixtureSetUp]
        public void BeforeEachTest()
        {
            browser = GetBrowser();
        }

        [Test]
        public void Should_return_global_parameters()
        {
            // When
            var result = browser.Get("/", with =>
            {
                with.HttpRequest();
            });

            // Then
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(new[] { "Global" }, result.Body.DeserializeJson<Isop.Server.Models.MethodTreeModel>().GlobalParameters
                .Map(i => i.Name).ToA());
        }

        [Test]
        public void Should_return_controllers()
        {
            // When
            var result = browser.Get("/", with =>
            {
                with.HttpRequest();
            });

            // Then
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(new[] { "My" }, result.Body.DeserializeJson<Isop.Server.Models.MethodTreeModel>().Controllers
                .Map(i => i.Name).ToA());
        }

        [Test]
        public void When_get_controller_url_Should_return_available_actions()
        {
            // When
            var result = browser.Get("/My/", with =>
            {
                with.HttpRequest();
            });

            // Then
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            var response = result.Body.DeserializeJson<Isop.Server.Models.Controller>();
            Assert.AreEqual("My", response.Name);
            Assert.AreEqual(new[] { "Action", "Fail", "ActionWithGlobalParameter", "ActionWithObjectArgument" },
                response.Methods.Map(i => i.Name).ToA());
        }

        [Test]
        public void Form_for_action_Should_contain_parameters()
        {
            // When
            var result = browser.Get("/My/Action/", with =>
            {
                with.HttpRequest();
            });

            // Then
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            var response = result.Body.DeserializeJson<Isop.Server.Models.Method>();

            Assert.AreEqual("Action", response.Name);
            var names = response.Parameters.Map(i => i.Name).ToA();
            Assert.AreEqual(new[] { "value" }, names);
        }


        [Test]
        public void Post_form_action()
        {
            var value = "value ' 3 ' \"_12 \"sdf";

            // When
            var result = browser.Post("/My/Action/", with =>
            {
                with.HttpRequest();
                with.FormValue("value", value);
            });

            // Then
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.That(result.Body["p"].Map(p => p.InnerText).Join("\n"), Is.StringContaining(HttpUtility.HtmlEncode("value=" + value)));
        }

    }
}
