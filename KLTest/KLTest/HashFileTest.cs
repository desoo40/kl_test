using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using Newtonsoft.Json;

namespace KLTest;

[TestClass]
public class HashFileTest
{
    const string validToken = "KudEmW89SC6U2Nc9/1Sd7g==";
    const string Ox = "0x";
    const string validHashMd5 = "AC90AD929D7F5D6DD5C06809AC8613C9";
    const string validHashSha1 = "BD8BBD7F603CF8B51097C0E416FBBE14F561A994";
    const string validHashSha256 = "B27CC938BE34A9455E567EEC0A27381A89D7C8348F8A721A6D167D34C53C4B4A";

    const string sURL = $"https://opentip.kaspersky.com/api/v1/search/hash?request=";

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

                using Stream s = response.GetResponseStream();
                using StreamReader sr = new StreamReader(s);
                return sr.ReadToEnd();

            }
        }
        catch (WebException ex)
        {
            Console.WriteLine(ex.ToString());
            throw ex;
        }

        return "";
    }

    [TestMethod]
    public void CheckCorrectHashTokenMd5()
    {
        var req = WebReq(sURL + validHashMd5, validToken);
        var info = JsonConvert.DeserializeObject<FileInfo>(req);

        Assert.IsTrue(info.AllFieldsNotNull(), "Md5 hash wrong response");
    }
    
    [TestMethod]
    public void CheckCorrectHashTokenMd5_0x()
    {
        var req = WebReq(sURL + Ox + validHashMd5, validToken);
        var info = JsonConvert.DeserializeObject<FileInfo>(req);

        Assert.IsTrue(info.AllFieldsNotNull(), "Md5 hash with 0x prefix wrong response");
    }

    [TestMethod]
    public void CheckCorrectHashTokenSha1()
    {
        var req = WebReq(sURL + validHashSha1, validToken);
        var info = JsonConvert.DeserializeObject<FileInfo>(req);

        Assert.IsTrue(info.AllFieldsNotNull(), "Sha1 hash wrong response");
    }

    [TestMethod]
    public void CheckCorrectHashTokenSha1_0x()
    {
        var req = WebReq(sURL + Ox + validHashSha1, validToken);
        var info = JsonConvert.DeserializeObject<FileInfo>(req);

        Assert.IsTrue(info.AllFieldsNotNull(), "Sha1 hash with 0x prefix wrong response");
    }

    [TestMethod]
    public void CheckCorrectHashTokenSha256()
    {
        var req = WebReq(sURL + validHashSha256, validToken);
        var info = JsonConvert.DeserializeObject<FileInfo>(req);

        Assert.IsTrue(info.AllFieldsNotNull(), "Sha256 hash wrong response");
    }

    [TestMethod]
    public void CheckCorrectHashTokenSha256_0x()
    {
        var req = WebReq(sURL + Ox + validHashSha256, validToken);
        var info = JsonConvert.DeserializeObject<FileInfo>(req);

        Assert.IsTrue(info.AllFieldsNotNull(), "Sha256 hash with 0x prefix wrong response");
    }

    [TestMethod]
    public void TwoReapetedRequestsWithOneHash()
    {
        var firstReq = WebReq(sURL + validHashMd5, validToken);
        var secondReq = WebReq(sURL + validHashMd5, validToken);

        Assert.AreEqual(firstReq, secondReq, "One request repeated two times gives different response");
    }
    
    [TestMethod]
    public void CheckThreeHashsGiveSameAnswer()
    {
        var firstReq = WebReq(sURL + validHashMd5, validToken);
        var secondReq = WebReq(sURL + validHashSha1, validToken);
        var thirdReq = WebReq(sURL + validHashSha256, validToken);

        Assert.AreEqual(firstReq, secondReq, "Md5 response doesn't match sha1 response");
        Assert.AreEqual(secondReq, thirdReq, "sha1 response doesn't match sha256 response");
        Assert.AreEqual(firstReq, thirdReq, "Md5 response doesn't match sha256 response");
    }

    [TestMethod]
    public void IncorrectLenghtHash()
    {
        var incorrectLenght = validToken + "someInfo";

        try
        {
            WebReq(sURL + incorrectLenght, validToken);
        }
        catch (WebException ex)
        {
            Assert.IsTrue(ex.Message.Contains("400"));
        }
    }
    
    [TestMethod]
    public void EmptyStringHash()
    {
        var emptyStringHash = "";

        try
        {
            WebReq(sURL + emptyStringHash, validToken);
        }
        catch (WebException ex)
        {
            Assert.IsTrue(ex.Message.Contains("400"));
        }
    }

    [TestMethod]
    public void EmptyFileHash()
    {
        var emptyFileHash = "D41D8CD98F00B204E9800998ECF8427Eû";
        
        try
        {
            WebReq(sURL + emptyFileHash, validToken);
        }
        catch (WebException ex)
        {
            Assert.IsTrue(ex.Message.Contains("400"));
        }
    }

    [TestMethod]
    public void UnknownHash()
    {
        var unknownHash = "229d33b2d0d7d50441feb476a88d1373";
;
        try
        {
           WebReq(sURL + unknownHash, validToken);
        }
        catch (WebException ex)
        {
            Assert.IsTrue(ex.Message.Contains("400"));
        }
    }

    [TestMethod]
    public void IncorrectCharactersHash()
    {

    }

    [TestMethod]
    public void ExpiredToken()
    {
        var expiredToken = "9eXC2xJ8REyxkQUEm6iOXg=="; // till 5.09.2021
        try
        {
            WebReq(sURL + validHashMd5, expiredToken);
        }
        catch (WebException ex)
        {
            Assert.IsTrue(ex.Message.Contains("401"));
        }
    }

    [TestMethod]
    public void RevokeToken()
    {
        var revokedToken = "iTlZ104fQ+WofNbYc/EiEg==";
        try
        {
            WebReq(sURL + validHashMd5, revokedToken);
        }
        catch (WebException ex)
        {
            Assert.IsTrue(ex.Message.Contains("401"));
        }
    }

    [TestMethod]
    public void IncorrectLenghtToken()
    {
        var invalidLengtToken = "/cprfmOkT4emoi4rvpjBBA===";

        try
        {
            WebReq(sURL + validHashMd5, invalidLengtToken);
        }
        catch (WebException ex)
        {
            Assert.IsTrue(ex.Message.Contains("400"));
        }
    }

    [TestMethod]
    public void IncorrectCharactersToken()
    {
        var invalidCharsToken = "/+3424LL34,,,,2342l";

        try
        {
            WebReq(sURL + validHashMd5, invalidCharsToken);
        }
        catch (WebException ex)
        {
            Assert.IsTrue(ex.Message.Contains("400"));
        }
    }

    [TestMethod]
    public void UnknownToken()
    {
        var unknownToken = "LudEmW89SC6U2Nc9/1Sd7g==";

        try
        {
            WebReq(sURL + validHashMd5, unknownToken);
        }
        catch (WebException ex)
        {
            Assert.IsTrue(ex.Message.Contains("401"));
        }
    }

    [TestMethod]
    public void EmptyStringToken()
    {
        var emptyToken = "";

        try
        {
           WebReq(sURL + validHashMd5, emptyToken);
        }
        catch (WebException ex)
        {
            Assert.IsTrue(ex.Message.Contains("401"));
        }
        
    }
}