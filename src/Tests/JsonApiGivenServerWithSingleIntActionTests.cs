using NUnit.Framework;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Isop.Tests.Server
{

    [TestFixture]
    public class JsonApiGivenServerWithSingleIntActionTests: BaseFixture
    {
        private static Browser browser= GetBrowser<FakeIsopServerWithSingleIntAction>(
                defaults: to => to.Accept("application/json")
            );

        public async Task Post_form_action2()
        {
            // When
            var result = await browser.Post("/SingleIntAction/Action/", with =>
                {
                    with.HttpRequest();
                    with.FormValue("param", "1");
                });

            // Then
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [Test]
        public async Task Post_form_action_with()
        {
            // When
            var result =await browser.Post("/SingleIntAction/Action/", with =>
                {
                    with.HttpRequest();
                });

            // Then
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            var response = (await result.BodyDeserializeJson<Isop.Server.Models.MissingArgument[]>()).Single();

            Assert.That(response.Argument, Is.EquivalentTo("param" ));
        }

        [Test]
        public async Task Post_form_action_with_wrong_value()
        {
            // When
            var result = await browser.Post("/SingleIntAction/Action/", with =>
                {
                    with.HttpRequest();
                    with.FormValue("param", "asdf");
                });

            // Then
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            var response = (await result.BodyDeserializeJson<Isop.Server.Models.TypeConversionFailed[]>()).Single();

            Assert.That(response.Argument, Is.EqualTo("param"), "Arg");
            Assert.That(response.TargetType, Is.EqualTo("System.Int32"), "TargetType");
            Assert.That(response.Value, Is.EqualTo("asdf"), "Value");
        }
    }

}
