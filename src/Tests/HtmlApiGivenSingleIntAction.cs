using Nancy;
using Nancy.Testing;
using NUnit.Framework;
using System.Linq;
using With.Rubyfy;
using With;
using Nancy.Helpers;
using Isop.Server;
namespace Isop.Tests.Server
{

    [TestFixture]
    public class HtmlApiGivenSingleIntAction: BaseFixture
    {
        private Browser browser;
        [TestFixtureSetUp]
        public void BeforeEachTest()
        {
            browser = GetBrowser();
        }

        class FakeIsopServerWithSingleIntAction : IsopServerFromBuild
        {
            public FakeIsopServerWithSingleIntAction()
                : base(()=>new Build { typeof(Isop.Tests.FakeControllers.SingleIntAction) })
            {
            }
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
            Assert.That(result.Body["p"].Map(p => p.InnerText).Join("\n"), Is.StringContaining("param"));
        }

    }
}
