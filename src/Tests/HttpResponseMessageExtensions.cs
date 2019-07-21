using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace Isop.Tests.Server
{
    public class HtmlElement
    {
        private readonly XElement _xElement;

        public HtmlElement(XElement xElement)
        {
            _xElement = xElement;
        }

        public string InnerText => _xElement.Value;
        public IDictionary<string,string> Attributes
        {
            get => _xElement.Attributes().ToDictionary(a => a.Name.LocalName, a => a.Value);
        }
    }
    public static class HttpResponseMessageExtensions
    {
        public static async Task<IEnumerable<HtmlElement>> Body(this HttpResponseMessage self, string elem)
        {
            using (var res = await self.Content.ReadAsStreamAsync())
            {
                var doc= XDocument.Load(res);
                return doc.Elements(elem).Select(e=>new HtmlElement(e));
            }
        }

        public static async Task<T> BodyDeserializeJson<T>(this HttpResponseMessage self)
        {
            using (var res = await self.Content.ReadAsStreamAsync())
            using (var txt=new StreamReader(res))
            using (var reader=new JsonTextReader(txt))
            {
                return new JsonSerializer().Deserialize<T>(reader);
            }
        }
    }
}