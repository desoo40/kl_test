using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.IO;
using System.Net;

namespace kl_test // Note: actual namespace depends on the project name.
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var token = "iTlZ104fQ+WofNbYc/EiEg==";

            string sURL;
            
            var hash = "ac90ad929d7f5d6dd5c06809ac8613c9";

            sURL = $"https://opentip.kaspersky.com/api/v1/search/hash?request={hash}";



            var wrGETURL = WebRequest.Create(sURL);
            wrGETURL.Headers.Add("x-api-key", token);

            Stream objStream;
            objStream = wrGETURL.GetResponse().GetResponseStream();

            StreamReader objReader = new StreamReader(objStream);

            string sLine = "";
            int i = 0;

            while (sLine != null)
            {
                i++;
                sLine = objReader.ReadLine();
                if (sLine != null)
                    Console.WriteLine("{0}:{1}", i, sLine);
            }
            Console.ReadLine();
        }
    }
}