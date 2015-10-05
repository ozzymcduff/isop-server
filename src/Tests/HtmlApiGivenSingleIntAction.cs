using Nancy;
using Nancy.Testing;
using NUnit.Framework;
using System.Linq;
using With;
namespace Isop.Tests.Server
{

    [TestFixture]
    public class HtmlApiGivenSingleIntAction: BaseFixture
    {
        private Browser browser;
        [TestFixtureSetUp]
        public void BeforeEachTest()
        {
            // Given
            browser = GetBrowser<FakeIsopServerWithSingleIntAction>();
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
            Assert.That(result.Body["p"].Select(p => p.InnerText).Join("\n"), Is.StringContaining("param"));
        }

    }
}
