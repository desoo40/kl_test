using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using System;

namespace KLTest
{
    [TestClass]
    public class HashFileTest
    {
        const string validToken = "gMJUS2SoSPaxEww8mPOY+A==";
        const string Ox = "0x";
        const string validHashMd5 = "AC90AD929D7F5D6DD5C06809AC8613C9";
        const string validHashSha1 = "BD8BBD7F603CF8B51097C0E416FBBE14F561A994";
        const string validHashSha256 = "B27CC938BE34A9455E567EEC0A27381A89D7C8348F8A721A6D167D34C53C4B4A";

        const string sURL = "https://opentip.kaspersky.com/api/v1/search/hash?request=";

        public static string WebReq(string url, string token)
        {
            try
            {
                var webRequest = WebRequest.Create(url);

                webRequest.Method = "GET";
                webRequest.Headers.Add("x-api-key", token);

                var response = (HttpWebResponse)webRequest.GetResponse();

                using Stream s = response.GetResponseStream();
                using StreamReader sr = new StreamReader(s);
                return sr.ReadToEnd();
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
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
        public void ToLowerHash()
        {
            var lowerHash = validHashMd5.ToLower();
            var req = WebReq(sURL + lowerHash, validToken);

            var info = JsonConvert.DeserializeObject<FileInfo>(req);

            Assert.IsTrue(info.AllFieldsNotNull(), "Hash in lower case gives wrong response");
        }

        [TestMethod]
        public void LowerUpperHashEquals()
        {
            var lowerHash = validHashMd5.ToLower();

            var upperReq = WebReq(sURL + validHashMd5, validToken);
            var lowerReq = WebReq(sURL + lowerHash, validToken);

            Assert.AreEqual(upperReq, lowerReq, "Same hash in lower and upper cases gives different response");
        }

        [TestMethod]
        public void IncorrectLenghtHash()
        {
            var incorrectLenght = validHashMd5 + "extra";
            var errorMes = "Hash with incorrect lenght doesn't give 400 (Bad Request) error";

            try
            {
                WebReq(sURL + incorrectLenght, validToken);
            }
            catch (WebException ex)
            {
                Assert.IsTrue(
                    ex.Message.Contains("400"),
                    $"Cought web exception, but not 400 \n {errorMes}"
                );

                return;
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            Assert.Fail(errorMes);
        }

        [TestMethod]
        public void EmptyStringHash()
        {
            var emptyStringHash = "";
            var errorMes = "Empty string hash doesn't give 400 (Bad Request) error";

            try
            {
                WebReq(sURL + emptyStringHash, validToken);
            }
            catch (WebException ex)
            {
                Assert.IsTrue(
                    ex.Message.Contains("400"),
                     $"Cought web exception, but not 400 \n {errorMes}"
                );

                return;
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            Assert.Fail(errorMes);
        }

        [TestMethod]
        public void EmptyFileHash()
        {
            var emptyFileHash = "D41D8CD98F00B204E9800998ECF8427E";
            var errorMes = "Empty file hash doesn't give 400 (Bad Request) error";

            try
            {
                WebReq(sURL + emptyFileHash, validToken);
            }
            catch (WebException ex)
            {
                Assert.IsTrue(
                    ex.Message.Contains("400"),
                    $"Cought web exception, but not 400 \n {errorMes}"
                );

                return;
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            Assert.Fail(errorMes);
        }

        [TestMethod]
        public void UnknownHash()
        {
            var unknownHash = "229d33b2d0d7d50441feb476a88d1373";
            var errorMes = "Unknown by app file hash doesn't give 400 (Bad Request) error";

            try
            {
                WebReq(sURL + unknownHash, validToken);
            }
            catch (WebException ex)
            {
                Assert.IsTrue(
                    ex.Message.Contains("400"),
                    $"Cought web exception, but not 400 \n {errorMes}"
                );

                return;
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            Assert.Fail(errorMes);
        }

        [TestMethod]
        public void IncorrectCharactersHash()
        {
            var incorrectCharsHash = "AZ90AG929D7F5D6DD5C06809AC8XXYY";
            var errorMes = "Hash with incorrect chars doesn't give 400 (Bad Request) error";

            try
            {
                WebReq(sURL + incorrectCharsHash, validToken);
            }
            catch (WebException ex)
            {
                Assert.IsTrue(
                    ex.Message.Contains("400"),
                    $"Cought web exception, but not 400 \n {errorMes}"
                );

                return;
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            Assert.Fail(errorMes);
        }

        [TestMethod]
        public void ExpiredToken()
        {
            var expiredToken = "9eXC2xJ8REyxkQUEm6iOXg=="; // till 5.09.2021
            var errorMes = "Expiered token doesn't give 401 (Unautorized) error";

            try
            {
                WebReq(sURL + validHashMd5, expiredToken);
            }
            catch (WebException ex)
            {
                Assert.IsTrue(
                    ex.Message.Contains("401"),
                    $"Cought web exception, but not 401 \n {errorMes}"
                );

                return;
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            Assert.Fail(errorMes);
        }

        [TestMethod]
        public void RevokeToken()
        {
            var revokedToken = "iTlZ104fQ+WofNbYc/EiEg==";
            var errorMes = "Revoked token doesn't give 401 (Unautorized) error";

            try
            {
                WebReq(sURL + validHashMd5, revokedToken);
            }
            catch (WebException ex)
            {
                Assert.IsTrue(
                    ex.Message.Contains("401"),
                    $"Cought web exception, but not 401 \n {errorMes}"
                );

                return;
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            Assert.Fail(errorMes);
        }

        [TestMethod]
        public void IncorrectLenghtToken()
        {
            var invalidLengtToken = "/cprfmOkT4emoi4rvpjBBA===";
            var errorMes = "Incorrect lenght token doesn't give 400 (Bad request) error";

            try
            {
                WebReq(sURL + validHashMd5, invalidLengtToken);
            }
            catch (WebException ex)
            {
                Assert.IsTrue(
                    ex.Message.Contains("400"),
                    $"Cought web exception, but not 400 \n {errorMes}"
                );

                return;
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            Assert.Fail(errorMes);
        }

        [TestMethod]
        public void IncorrectCharactersToken()
        {
            var invalidCharsToken = "/+3424LL34,,,,2342l";
            var errorMes = "Incorrect chars token doesn't give 400 (Bad request) error";

            try
            {
                WebReq(sURL + validHashMd5, invalidCharsToken);
            }
            catch (WebException ex)
            {
                Assert.IsTrue(
                    ex.Message.Contains("400"),
                    $"Cought web exception, but not 400 \n {errorMes}"
                );

                return;
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            Assert.Fail(errorMes);
        }

        [TestMethod]
        public void UnknownToken()
        {
            var unknownToken = "LudEmW89SC6U2Nc9/1Sd7g==";
            var errorMes = "Unknonw token doesn't give 401 (Unautorized) error";

            try
            {
                WebReq(sURL + validHashMd5, unknownToken);
            }
            catch (WebException ex)
            {
                Assert.IsTrue(
                    ex.Message.Contains("401"),
                    $"Cought web exception, but not 401 \n {errorMes}"
                );

                return;
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            Assert.Fail(errorMes);
        }

        [TestMethod]
        public void EmptyStringToken()
        {
            var emptyToken = "";
            var errorMes = "Unknonw token doesn't give 401 (Unautorized) error";

            try
            {
                WebReq(sURL + validHashMd5, emptyToken);
            }
            catch (WebException ex)
            {
                Assert.IsTrue(
                    ex.Message.Contains("401"),
                    $"Cought web exception, but not 401 \n {errorMes}"
                );
                return;
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            Assert.Fail(errorMes);

        }
    }
}

