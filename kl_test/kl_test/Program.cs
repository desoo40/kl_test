using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Configuration;
using Newtonsoft.Json;

namespace kl_test // Note: actual namespace depends on the project name.
{
    public class Program
    {
        const string revokedToken = "iTlZ104fQ+WofNbYc/EiEg==";
        const string expiredToken = "9eXC2xJ8REyxkQUEm6iOXg=="; // till 5.09.2021
        static readonly string validToken = ConfigurationManager.AppSettings["token"];
        const string invalidLengtToken = "/cprfmOkT4emoi4rvpjBBA===";
        const string invalidCharsToken = "/+3424kl234,,,,2342l";

        const string validHashMd5 = "ac90ad929d7f5d6dd5c06809ac8613c9";
        const string validHashSha1 = "BD8BBD7F603CF8B51097C0E416FBBE14F561A994";
        const string validHashSha256 = "B27CC938BE34A9455E567EEC0A27381A89D7C8348F8A721A6D167D34C53C4B4A";

        const string sURL = $"https://opentip.kaspersky.com/api/v1/search/hash?request=";


        public class FileInfo
        {
            public string Zone;
            public GeneralInfo FileGeneralInfo;

            public class GeneralInfo
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

        public static string WebReq(string url, string token)
        {
            try
            {
                var webRequest = WebRequest.Create(url);

                if (webRequest != null)
                {
                    webRequest.Method = "GET";
                    webRequest.Headers.Add("x-api-key", token);

                    var response = (HttpWebResponse)webRequest.GetResponse();

                    using (Stream s = response.GetResponseStream())
                    {
                        using (StreamReader sr = new StreamReader(s))
                        {
                            var jsonResponse = sr.ReadToEnd();

                            var inf = JsonConvert.DeserializeObject<FileInfo>(jsonResponse);
                            Console.WriteLine(jsonResponse);

                            return jsonResponse;
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex.ToString());
                throw ex;
            }

            return "";
        }

        public static async Task Main(string[] args)
        {
            CheckCorrectHashToken();
            CheckThreeHashsGiveSameAnswer();
            TwoReapetedRequestsWithOneHash();
            IncorrectLenghtHash();
            IncorrectCharactersHash();
        }

        private static void IncorrectCharactersHash()
        {
        }

        private static void IncorrectLenghtHash()
        {
        }

        private static void TwoReapetedRequestsWithOneHash()
        {
            
            var firstReq = WebReq(sURL + validHashMd5, validToken);
            var secondReq = WebReq(sURL + validHashMd5, validToken);

            Debug.Assert(firstReq == secondReq, "Checking for same answer with same hash");
        }

        private static void CheckThreeHashsGiveSameAnswer()
        {
            var firstReq = WebReq(sURL + validHashMd5, validToken);
            var secondReq = WebReq(sURL + validHashSha1, validToken);
            var thirddReq = WebReq(sURL + validHashSha256, validToken);

            Debug.Assert(firstReq == secondReq, "Checking for same answer with three types of hash");
            Debug.Assert(firstReq == thirddReq, "Checking for same answer with three types of hash");
        }

        private static void CheckCorrectHashToken()
        {
        }
    }
}