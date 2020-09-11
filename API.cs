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
        private static readonly Regex PhoneMatcher = new Regex(@"\+?\d{11}");
        
        private readonly Topic _topic;

        public API(ConfigBuilder.Config config)
        {
            _topic = new Topic(config);
        }

        /// <summary>
        /// Возвращает основную информацию по номеру телефону
        /// </summary>
        public ApiResponse<SearchResult> GetByPhone(string phone, string countryCode = null, string source = null, bool force = false)
        {
            if (!force && (string.IsNullOrEmpty(phone) || !PhoneMatcher.IsMatch(phone))) throw new ArgumentException("Телефон заполнен неправильно");
            return _topic.CreateTopic<SearchResult>("https://pbssrv-centralevents.com/v2.5/search", source ?? "search", phone, countryCode);
        }

        /// <summary>
        /// Возвращает список тегов
        /// </summary>
        public ApiResponse<DetailsResult> GetTags(string phone, string countryCode = null, string source = null, bool force = false)
        {
            if (!force && (string.IsNullOrEmpty(phone) || !PhoneMatcher.IsMatch(phone))) throw new ArgumentException("Телефон заполнен неправильно");
            return _topic.CreateTopic<DetailsResult>("https://pbssrv-centralevents.com/v2.5/number-detail", source ?? "details", phone, countryCode);
        }
    }
}
