using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.IO;
using System.Net;
using System.Diagnostics;

namespace kl_test // Note: actual namespace depends on the project name.
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var token = "iTlZ104fQ+WofNbYc/EiEg==";
            var hash = "B27CC938BE34A9455E567EEC0A27381A89D7C8348F8A721A6D167D34C53C4B4A";
            var sURL = $"https://opentip.kaspersky.com/api/v1/search/hash?request={hash}";


            try
            {
                var webRequest = WebRequest.Create(sURL);

                if (webRequest != null)
                {
                    webRequest.Method = "GET";
                    webRequest.Headers.Add("x-api-key", token);


                    using (Stream s = webRequest.GetResponse().GetResponseStream())
                    {
                        using (StreamReader sr = new StreamReader(s))
                        {
                            var jsonResponse = sr.ReadToEnd();
                            Console.WriteLine($"Response: {jsonResponse}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Debug.Assert(1 == 1);
        }
    }
}