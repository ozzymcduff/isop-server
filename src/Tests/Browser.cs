using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using With;

namespace Isop.Tests.Server
{
    public class BrowserContext
    {
        /// <summary>
        /// Configures the request to be sent over HTTP.
        /// </summary>
        public void HttpRequest()
        {
        }

        internal object _Body { get; set; }


        internal IDictionary<string, string> _form; 

        public void FormValue(string key, string value)
        {
            if (_form==null)_form=new Dictionary<string, string>();
            _form.Add(key, value);
        }
        private string _accept; 

        public void Accept(string acceptHeader)
        {
            _accept = acceptHeader;
        }

        public HttpRequestMessage Configure(HttpRequestMessage request)
        {
            if (!string.IsNullOrEmpty(_accept))
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_accept));
            if (_form != null)
            {
                request.Content = new FormUrlEncodedContent(_form);
            }

            return request;
        }
    }
    public class Browser:IDisposable
    {        
        private static class CookieParser
        {
            private static readonly Regex CookieRegex=new Regex("^(?<name>[^=]+)=(?<values>.*)$");
            public static Cookie Parse(string header)
            {
                if (string.IsNullOrEmpty(header)) return null;
                var match = CookieRegex.Match(header);
                if (!match.Success) throw new Exception();
                var name = match.Groups["name"].Value;
                var values = match.Groups["values"].Value
                    .Split(new[] { "; " }, StringSplitOptions.RemoveEmptyEntries);
                return new Cookie(name, 
                    Uri.UnescapeDataString(values.First().Trim()));
            }
        }
        private Dictionary<string,Cookie> cookies=new Dictionary<string,Cookie>();
        private readonly HttpClient _client;
        private TestServer testServer;
        private readonly Action<BrowserContext> _defaults;


        public Browser(TestServer testServer, Action<BrowserContext> defaults)
        {
            this.testServer = testServer;
            _defaults = defaults;
            _client = testServer.CreateClient();
        }

        public async Task<HttpResponseMessage> Post(string path, Action<BrowserContext> with)
        {
            var conf = new BrowserContext();
            _defaults?.Invoke(conf);
            with?.Invoke(conf);
            var msg = conf.Configure(new HttpRequestMessage
            {
                RequestUri = Uris.Create(BaseAddress, path),
                Method = HttpMethod.Post,
                Content = new StringContent(JsonConvert.SerializeObject(conf._Body), Encoding.UTF8, "application/json")
            });
            IncludeCookies(msg);
            var result =  await _client.SendAsync(msg);
            ExtractCookies(result);
            return result;
        }

        private void IncludeCookies(HttpRequestMessage msg)
        {
            foreach (var cookie in cookies.Values)
            {
                msg.Headers.Add("Cookie", $"{Uri.EscapeDataString(cookie.Name)}={Uri.EscapeDataString(cookie.Value)}");
            }
        }

        private void ExtractCookies(HttpResponseMessage result)
        {
            if (!result.Headers.Contains("Set-Cookie")) return;
            var cookieValues = result.Headers
                .GetValues("Set-Cookie").Select(CookieParser.Parse);
            foreach (var cookie in cookieValues)
            {
                cookies[cookie.Name] = cookie;
            }
        }

        public static string BaseAddress { get; } = "https://localhost:5001";
        public static string Origin { get; } = "https://someotherthing";
        public async Task<HttpResponseMessage> Get(string path, Action<BrowserContext> with)
        {
            var conf=new BrowserContext();
            _defaults?.Invoke(conf);
            with?.Invoke(conf);
            var msg = conf.Configure(new HttpRequestMessage {RequestUri = Uris.Create(BaseAddress, path), Method = HttpMethod.Get});
            IncludeCookies(msg);
            var result = await _client.SendAsync(msg);
            ExtractCookies(result);
            return result;
        }


     
        public void Dispose()
        {
            _client?.Dispose();
            testServer?.Dispose();
        }
    }
}