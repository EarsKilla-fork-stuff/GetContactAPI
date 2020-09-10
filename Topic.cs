using Newtonsoft.Json.Linq;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using GetContactAPI.Models;
using Newtonsoft.Json;

namespace GetContactAPI
{
    internal class Topic
    {
        private readonly ConfigBuilder.Config _config;

        public Topic(ConfigBuilder.Config config)
        {
            _config = config;
        }

        /// <summary>
        /// Формирование зашифрованного запроса
        /// </summary>
        /// <returns>Получение дешифрованного запроса в формате json</returns>
        public ApiResponse<T> CreateTopic<T>(string url, string source, string phone, string countryCode)
        {
            string ts = ((int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds).ToString(); // timespan (Unix)
            var reqObj = new
            {
                countryCode = countryCode ?? "RU",
                source,
                token = _config.Token,
                phoneNumber = phone
            };
            string req = JsonConvert.SerializeObject(reqObj, Formatting.None);
            string sig = Crypt.EncryptToSHA256(ts.Replace("\r\n", "") + "-" + req, _config.Key); // signature
            string crypt = Crypt.EncryptAes256ECB(req, _config.AesKey);

            return SendPost<T>(url, "{\"data\":\"" + crypt + "\"}", ts, sig);
        }

        /// <summary>
        /// Отправка готового запроса на сервер, с последующей дешифровкой
        /// </summary>
        private ApiResponse<T> SendPost<T>(string url, string data, string ts, string sig)
        {
            using (WebClient client = new WebClient { Encoding = Encoding.UTF8 })
            {
                client.Headers.Add(new NameValueCollection()
                {
                    {"X-App-Version", _config.AppVersion},
                    {"X-Token", _config.Token},
                    {"X-Os", _config.Os},
                    {"X-Client-Device-Id", _config.DeviceId},
                    {"Content-Type", "application/json; charset=utf-8"},
                    {"Accept-Encoding", "deflate"},
                    {"X-Req-Timestamp", ts},
                    {"X-Req-Signature", sig},
                    {"X-Encrypted", "1"}
                });
                client.Headers.Add(HttpRequestHeader.UserAgent, _config.UserAgent);

                string rawJsonResponse;
                try
                {
                    rawJsonResponse = client.UploadString(url, data); // отправляем запрос
                }
                catch (WebException webEx)
                {
                    // вытаскиваем ответ при ошибке
                    using (var rs = webEx.Response.GetResponseStream())
                    {
                        if (rs == null)
                            throw;

                        using (var sr = new StreamReader(rs))
                            rawJsonResponse = sr.ReadToEnd();
                    }
                }
                
                var rawResponse = JObject.Parse(rawJsonResponse);
                if (!rawResponse.TryGetValue("data", StringComparison.Ordinal, out var rawData))
                    throw new ApplicationException("Failed to get \"data\" from response!");

                var decryptedResponse = Crypt.DecryptAes256ECB(rawData.ToString(), _config.AesKey); // расшифровывем
                return JsonConvert.DeserializeObject<ApiResponse<T>>(decryptedResponse);
            }
        }
    }
}
