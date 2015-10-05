using Nancy;
using Nancy.Testing;
using NUnit.Framework;
using System.Linq;

namespace Isop.Tests.Server
{

    [TestFixture]
    public class JsonApiGivenServerWithSingleIntActionTests: BaseFixture
    {
        private Browser browser;
        [TestFixtureSetUp]
        public void BeforeEachTest ()
        {
            // Given
            browser = GetBrowser<FakeIsopServerWithSingleIntAction>(
                defaults: to => to.Accept("application/json")
            );
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
