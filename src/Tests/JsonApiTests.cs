using NUnit.Framework;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace Isop.Tests.Server
{
    [TestFixture]
    public class JsonApiTests: BaseFixture
    {
        private static Browser browser= GetBrowser(defaults: to => to.Accept("application/json"));
        

        [Test]
        public async Task Should_return_global_parameters()
        {
            // When
            var result =await browser.Get("/", with =>
            {
                with.HttpRequest();
            });

            // Then
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(new[] { "Global" }, (await result.BodyDeserializeJson<Isop.Server.Models.MethodTreeModel>()).GlobalParameters
                .Select(i => i.Name).ToArray());
        }

        [Test]
        public async Task Should_return_controllers()
        {
            // When
            var result =await browser.Get("/", with =>
            {
                with.HttpRequest();
            });

            // Then
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(new[] { "My" }, (await result.BodyDeserializeJson<Isop.Server.Models.MethodTreeModel>()).Controllers
                .Select(i => i.Name).ToArray());
        }

        [Test]
        public async Task When_get_controller_url_Should_return_available_actions()
        {
            // When
            var result = await browser.Get("/My/", with =>
            {
                with.HttpRequest();
            });

            // Then
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            var response = await result.BodyDeserializeJson<Isop.Server.Models.Controller>();
            Assert.AreEqual("My", response.Name);
            Assert.AreEqual(new[] { "Action", "Fail", "ActionWithGlobalParameter", "ActionWithObjectArgument" },
                response.Methods.Select(i => i.Name).ToArray());
        }

        [Test]
        public async Task Form_for_action_Should_contain_parameters()
        {
            // When
            var result =await browser.Get("/My/Action/", with =>
            {
                with.HttpRequest();
            });

            // Then
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            var response = await result.BodyDeserializeJson<Isop.Server.Models.Method>();

            Assert.AreEqual("Action", response.Name);
            var names = response.Parameters.Select(i => i.Name).ToArray();
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
            var elems = await result.Body("p");
            StringAssert.Contains(HttpUtility.HtmlEncode("value=" + value),
                string.Join("\n", elems.Select(p => p.InnerText)));
        }

    }
}
