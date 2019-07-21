using NUnit.Framework;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Isop.Tests.Server
{

    [TestFixture]
    public class HtmlApiGivenSingleIntAction: BaseFixture
    {
        private static Browser browser=GetBrowser<FakeIsopServerWithSingleIntAction>();

        [Test]
        public async Task Post_form_action_with()
        {
            // When
            var result = await browser.Post("/SingleIntAction/Action/", with =>
                {
                    with.HttpRequest();
                });

            // Then
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            StringAssert.Contains(string.Join("\n",(await result.Body("p")).Select(p => p.InnerText)), "param");
        }

    }
}
