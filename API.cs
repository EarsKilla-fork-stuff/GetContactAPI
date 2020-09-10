using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GetContactAPI.Models;

namespace GetContactAPI
{
    public class API
    {
        private readonly Topic _topic;

        public API(ConfigBuilder.Config config)
        {
            _topic = new Topic(config);
        }

        /// <summary>
        /// Возвращает основную информацию по номеру телефону
        /// </summary>
        public ApiResponse<SearchResult> GetByPhone(string phone, string countryCode = null)
        {
            if (String.IsNullOrEmpty(phone) || !Regex.IsMatch(phone, "\\+?\\d{11}")) throw new ArgumentException("Телефон заполнен неправильно");
            return _topic.CreateTopic<SearchResult>("https://pbssrv-centralevents.com/v2.5/search", "search", phone, countryCode);
        }

        /// <summary>
        /// Возвращает список тегов
        /// </summary>
        public ApiResponse<DetailsResult> GetTags(string phone, string countryCode = null)
        {
            if (String.IsNullOrEmpty(phone) || !Regex.IsMatch(phone, "\\+?\\d{11}")) throw new ArgumentException("Телефон заполнен неправильно");
            return _topic.CreateTopic<DetailsResult>("https://pbssrv-centralevents.com/v2.5/number-detail", "details", phone, countryCode);
        }
    }
}
