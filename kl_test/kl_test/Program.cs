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
            
            var hash = "B27CC938BE34A9455E567EEC0A27381A89D7C8348F8A721A6D167D34C53C4B4A";

            sURL = $"https://opentip.kaspersky.com/api/v1/search/hash?request={hash}";



            var wrGETURL = WebRequest.Create(sURL);
            wrGETURL.Headers.Add("x-api-key", token);


            var resp = wrGETURL.GetResponse();

            Stream objStream;
            objStream = resp.GetResponseStream();

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