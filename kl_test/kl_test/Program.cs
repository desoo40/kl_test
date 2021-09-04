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
        public class FileInfo
        {
            public string Zone;

            public struct FileGeneralInfo
            {
                public string FileStatus;
                public string Sha1;
                public string Md5;
                public string Sha256;
                public string FirstSeen;
                public string LastSeen;
                public string Signer;
                public int Size;
                public string Type;
                public int HitsCount;
            }
        }

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

                            var inf = JsonConvert.DeserializeObject<FileInfo>(jsonResponse);

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
            var hash = "ac90ad929d7f5d6dd5c06809ac8613c9";
            var sURL = $"https://opentip.kaspersky.com/api/v1/search/hash?request={hash}";

            WebReq(sURL, token);
           

            Debug.Assert(1 == 1);
        }
    }
}