using System;
using NUnit.Framework;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace Isop.Tests.Server
{
    [TestFixture]
    public class HtmlApiTests: BaseFixture
    {
        private static Browser browser=GetBrowser();

        [Test]
        public async Task Should_return_global_parameters()
        {
            // When
            var result = await browser.Get("/", with =>
            {
                with.HttpRequest();
            });

            // Then
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(new[] { "Global" }, (await result.Body(".global_parameters input"))
                .Select(i => i.Attributes["name"]).ToArray());
        }

        [Test]
        public async Task Should_return_available_controllers()
        {
            // When
            var result = await browser.Get("/", with =>
            {
                with.HttpRequest();
            });

            // Then
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(new[] { "My" }, (await result.Body(".controllers a"))
                .Select(i => i.InnerText).ToArray());
        }

        [Test]
        public async Task When_get_controller_url_Should_return_header_and_available_actions()
        {
            // When
            var result = await browser.Get("/My/", with =>
            {
                with.HttpRequest();
            });

            // Then
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual("My", (await result.Body("h1")).Single().InnerText.Trim('\n', '\r', ' '));
            Assert.AreEqual(new[] { "Action", "Fail", "ActionWithGlobalParameter", "ActionWithObjectArgument" },
                (await result.Body("a")).Select(i => i.InnerText.Trim('\n', '\r', ' ')).ToArray());
        }

        [Test]
        public async Task Form_for_action_Should_contain_parameters()
        {
            // When
            var result = await browser.Get("/My/Action/", with =>
            {
                with.HttpRequest();
            });

            // Then
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual("Action", (await result.Body("h1")).Single().InnerText.Trim('\n', '\r', ' '));
            var names = (await result.Body("form input")).Select(i => i.Attributes["name"]).ToArray();
            Assert.AreEqual(new[] { "value" }, names);
        }


        [Test]
        public async Task Post_form_action()
        {
            var value = "value ' 3 ' \"_12 \"sdf";

            // When
            var result =await browser.Post("/My/Action/", with =>
            {
                with.HttpRequest();
                with.FormValue("value", value);
            });

            // Then
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            StringAssert.Contains(String.Join("\n",(await result.Body("p")).Select(p => p.InnerText)), HttpUtility.HtmlEncode("value=" + value));
        }
    }
}
