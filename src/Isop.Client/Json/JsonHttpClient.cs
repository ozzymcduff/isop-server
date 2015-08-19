﻿using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Isop.Client.Json
{
    public class JsonHttpClient : IJSonHttpClient
    {
        public async Task<JsonResponse> Request(Request jsonRequest)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(jsonRequest.Uri);
                {
                    var r = request;
                    r.Method = jsonRequest.Method;
                    //r.ContentType = "application/json; charset=utf-8";
                    r.Accept = "application/json";
                    if (!string.IsNullOrEmpty(jsonRequest.Data))
                    {
                        request.ContentType = "application/x-www-form-urlencoded";
                        var bytes = Encoding.UTF8.GetBytes(jsonRequest.Data);
                        using (var stream = await r.GetRequestStreamAsync())
                        {
                            stream.Write(bytes, 0, bytes.Length);
                        }
                    }
                };

                var response = await request.GetResponseAsync();
                if (jsonRequest.DoStream)
                {
                    return new JsonResponse(((HttpWebResponse)response).StatusCode, response.GetResponseStream());
                }
                using (var rstream = response.GetResponseStream())
                using (var reader = new StreamReader(rstream, Encoding.UTF8))
                {
                    var c = reader.ReadToEnd();
                    return new JsonResponse(((HttpWebResponse)response).StatusCode, c);
                }
            }
            catch (WebException ex)
            {
                throw GetRequestException(ex);
            }
            catch (Exception ex)
            {
                if (ex.GetBaseException() is WebException)
                    throw GetRequestException((WebException)ex.GetBaseException());
                throw;
            }
        }

        protected internal static RequestException GetRequestException(WebException ex)
        {
            if (ex.Response != null)
            {
                using (var rstream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(rstream, Encoding.UTF8))
                {
                    var c = reader.ReadToEnd();
                    var resp = ((HttpWebResponse)ex.Response);
                    return new RequestException(resp.StatusCode, c);
                }
            }
            return new RequestException(HttpStatusCode.InternalServerError, ex.Message);
        }
    }
}
