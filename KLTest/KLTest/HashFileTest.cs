using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System;

namespace KLTest
{
    [TestClass]
    public class HashFileTest
    {
        const string validToken = "MsZ03CyuQWiIk+LQEW017w==";
        const string Ox = "0x";
        const string validHashMd5 = "AC90AD929D7F5D6DD5C06809AC8613C9";
        const string validHashSha1 = "BD8BBD7F603CF8B51097C0E416FBBE14F561A994";
        const string validHashSha256 = "B27CC938BE34A9455E567EEC0A27381A89D7C8348F8A721A6D167D34C53C4B4A";

        const string sURL = "https://opentip.kaspersky.com/api/v1/search/hash?request=";

        const string webCatchAssertText = "Cought web exception, but not ";

        const string badRequestErrCode = "400";
        const string badRequestErrName = "Bad request";
        const string unauthorizedtErrCode = "401";
        const string unauthorizedtErrName = "Unauthorized";

        public static string WebReq(string url, string token)
        {
            var webRequest = WebRequest.Create(url);

            webRequest.Method = "GET";
            webRequest.Headers.Add("x-api-key", token);

            var response = (HttpWebResponse)webRequest.GetResponse();

            using var s = response.GetResponseStream();
            using var sr = new StreamReader(s);

            return sr.ReadToEnd();
        }

        public void AssertWebExeptionCatcher(string token, string hash, string errCode, string errMes)
        {
            try
            {
                WebReq(sURL + hash, token);
            }
            catch (WebException ex)
            {
                Assert.IsTrue(
                    ex.Message.Contains(errCode),
                    $"{webCatchAssertText} {errCode}\n {errMes}"
                );
                return;
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            Assert.Fail(errMes);
        }

        public void AssertIsJSONResponse(string hash, string token, string errMes)
        {
            try
            {
                var req = WebReq(sURL + hash, token);
                var info = JsonConvert.DeserializeObject<FileInfo>(req);

                Assert.IsTrue(info.AllFieldsNotNull(), errMes);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        public string ErrorStringBuilder(string inBegining, string errCode, string errName)
        {
            return $"{inBegining} doesn't give {errCode} ({errName}) error";
        }

       [TestMethod]
        public void CheckCorrectHashTokenMd5()
        {
            AssertIsJSONResponse(
                validHashMd5,
                validToken,
                "Md5 hash wrong response"
            );
        }

        [TestMethod]
        public void CheckCorrectHashTokenMd5_0x()
        {
            AssertIsJSONResponse(
               Ox + validHashMd5,
               validToken,
               "Md5 hash with 0x prefix wrong response"
           );
        }

        [TestMethod]
        public void CheckCorrectHashTokenSha1()
        {
            AssertIsJSONResponse(
               validHashSha1,
               validToken,
               "Sha1 hash wrong response"
           );
        }

        [TestMethod]
        public void CheckCorrectHashTokenSha1_0x()
        {
            AssertIsJSONResponse(
               Ox + validHashSha1,
               validToken,
               "Sha1 hash with 0x prefix wrong response"
           );
        }

        [TestMethod]
        public void CheckCorrectHashTokenSha256()
        {
            AssertIsJSONResponse(
               validHashSha256,
               validToken,
               "Sha256 hash wrong response"
           );
        }

        [TestMethod]
        public void CheckCorrectHashTokenSha256_0x()
        {
            AssertIsJSONResponse(
               Ox + validHashSha256,
               validToken,
               "Sha256 hash with 0x prefix wrong response"
           );
        }

        [TestMethod]
        public void TwoReapetedRequestsWithOneHash()
        {
            try
            {
                var firstReq = WebReq(sURL + validHashMd5, validToken);
                var secondReq = WebReq(sURL + validHashMd5, validToken);

                Assert.AreEqual(
                    firstReq,
                    secondReq,
                    "One request repeated two times gives different response"
                );
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void CheckThreeHashsGiveSameAnswer()
        {
            try
            {
                var firstReq = WebReq(sURL + validHashMd5, validToken);
                var secondReq = WebReq(sURL + validHashSha1, validToken);
                var thirdReq = WebReq(sURL + validHashSha256, validToken);

                Assert.AreEqual(firstReq, secondReq, "Md5 response doesn't match sha1 response");
                Assert.AreEqual(secondReq, thirdReq, "sha1 response doesn't match sha256 response");
                Assert.AreEqual(firstReq, thirdReq, "Md5 response doesn't match sha256 response");
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void ToLowerHash()
        {
            var lowerHash = validHashMd5.ToLower();

            AssertIsJSONResponse(
               lowerHash,
               validToken,
               "Hash in lower case gives wrong response"
           );
        }

        [TestMethod]
        public void LowerUpperHashEquals()
        {
            var lowerHash = validHashMd5.ToLower();

            var upperReq = WebReq(sURL + validHashMd5, validToken);
            var lowerReq = WebReq(sURL + lowerHash, validToken);

            Assert.AreEqual(
                upperReq,
                lowerReq,
                "Same hash in lower and upper cases gives different response"
            );
        }

        [TestMethod]
        public void IncorrectLenghtHash()
        {
            var incorrectLenght = validHashMd5 + "extra";

            AssertWebExeptionCatcher(
                  validToken,
                  incorrectLenght,
                  badRequestErrCode,
                  ErrorStringBuilder(
                      "Hash with incorrect lenght",
                      badRequestErrCode,
                      badRequestErrName
                  )
              );
        }

        [TestMethod]
        public void EmptyStringHash()
        {
            var emptyStringHash = "";

            AssertWebExeptionCatcher(
                  validToken,
                  emptyStringHash,
                  badRequestErrCode,
                  ErrorStringBuilder(
                      "Empty string hash",
                      badRequestErrCode,
                      badRequestErrName
                  )
              );
        }

        [TestMethod]
        public void EmptyFileHash()
        {
            var emptyFileHash = "D41D8CD98F00B204E9800998ECF8427E";

            AssertWebExeptionCatcher(
                 validToken,
                 emptyFileHash,
                 badRequestErrCode,
                 ErrorStringBuilder(
                     "Empty file hash",
                     badRequestErrCode,
                     badRequestErrName
                 )
             );
        }

        [TestMethod]
        public void UnknownHash()
        {
            var unknownHash = "229d33b2d0d7d50441feb476a88d1373";

            AssertWebExeptionCatcher(
                validToken,
                unknownHash,
                badRequestErrCode,
                ErrorStringBuilder(
                    "Unknown by app file hash",
                    badRequestErrCode,
                    badRequestErrName
                )
            );
        }

        [TestMethod]
        public void IncorrectCharactersHash()
        {
            var incorrectCharsHash = "AZ90AG929D7F5D6DD5C06809AC8XXYY";

            AssertWebExeptionCatcher(
                validToken,
                incorrectCharsHash,
                badRequestErrCode,
                ErrorStringBuilder(
                    "Hash with incorrect chars",
                    badRequestErrCode,
                    badRequestErrName
                )
            );
        }

        #region Tokens part
        [TestMethod]
        public void ExpiredToken()
        {
            var expiredToken = "9eXC2xJ8REyxkQUEm6iOXg=="; // till 5.09.2021

            AssertWebExeptionCatcher(
                expiredToken,
                validHashMd5,
                unauthorizedtErrCode,
                ErrorStringBuilder(
                    "Expiered token",
                    unauthorizedtErrCode,
                    unauthorizedtErrName
                )
            );
        }

        [TestMethod]
        public void RevokeToken()
        {
            var revokedToken = "iTlZ104fQ+WofNbYc/EiEg==";

            AssertWebExeptionCatcher(
                 revokedToken,
                 validHashMd5,
                 unauthorizedtErrCode,
                 ErrorStringBuilder(
                    "Revoked token",
                    unauthorizedtErrCode,
                    unauthorizedtErrName
                )
             );
        }

        [TestMethod]
        public void UnknownToken()
        {
            var unknownToken = "LudEmW89SC6U2Nc9/1Sd7g==";
            
            AssertWebExeptionCatcher(
                unknownToken,
                validHashMd5,
                unauthorizedtErrCode,
                ErrorStringBuilder(
                    "Unknown token",
                    unauthorizedtErrCode,
                    unauthorizedtErrName
                )
            );
        }

        [TestMethod]
        public void IncorrectLenghtToken()
        {
            var invalidLengtToken = "/cprfmOkT4emoi4rvpjBBA===";

            AssertWebExeptionCatcher(
                invalidLengtToken,
                validHashMd5,
                badRequestErrCode,
                ErrorStringBuilder(
                    "Incorrect lenght token",
                    badRequestErrCode,
                    badRequestErrName
                )
            );
        }

        [TestMethod]
        public void EmptyStringToken()
        {
            var emptyStringToken = "";

            AssertWebExeptionCatcher(
                emptyStringToken,
                validHashMd5,
                badRequestErrCode,
                ErrorStringBuilder(
                    "Empty string token",
                    badRequestErrCode,
                    badRequestErrName
                )
            );
        }

        [TestMethod]
        public void IncorrectCharactersToken()
        {
            var invalidCharsToken = "/+3424LL34,,,,2342l";

            AssertWebExeptionCatcher(
                invalidCharsToken,
                validHashMd5,
                badRequestErrCode,
                ErrorStringBuilder(
                    "Incorrect chars token",
                    badRequestErrCode,
                    badRequestErrName
                )
            );
        }
        #endregion
    }
}

