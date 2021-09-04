using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.IO;
using System.Net;
using System.Diagnostics;
using Newtonsoft.Json;

namespace kl_test // Note: actual namespace depends on the project name.
{
    public class Program
    {
        public static async void WebReq(string url, string token)
        {
            try
            {
                var webRequest = WebRequest.Create(url);

                if (webRequest != null)
                {
                    webRequest.Method = "GET";
                    webRequest.Headers.Add("x-api-key", token);

                    var response = (HttpWebResponse)webRequest.GetResponse();

                    Console.WriteLine(response.StatusCode);

                    using (Stream s = response.GetResponseStream())
                    {
                        using (StreamReader sr = new StreamReader(s))
                        {
                            var jsonResponse = sr.ReadToEnd();

                            JsonConvert.DeserializeObject(jsonResponse);

                            Console.WriteLine(jsonResponse);
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex.ToString());
                
            }
        }

        public static async Task Main(string[] args)
        {
            var token = "iTlZ104fQ+WofNbYc/EiEg==";
            var hash = "65a3af8c01a1cc779503118bfbf6ae5b";
            var sURL = $"https://opentip.kaspersky.com/api/v1/search/hash?request={hash}";

            WebReq(sURL, token);
           

            Debug.Assert(1 == 1);
        }
    }
}