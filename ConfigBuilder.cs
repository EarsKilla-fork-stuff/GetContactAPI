using System;

namespace GetContactAPI
{
    public class ConfigBuilder
    {
        public class Config
        {
            /// <summary>
            /// Токен из конфиг-файла
            /// </summary>
            public string Token { get; }

            /// <summary>
            /// Ключ для шифрования тела запроса
            /// <para/>Лежит в конфиг файле с названием ключа FINAL_KEY
            /// </summary>
            public string AesKey { get; }

            /// <summary>
            /// Ключ для шифрования подписи
            /// </summary>
            public string Key { get; internal set; } = "2Wq7)qkX~cp7)H|n_tc&o+:G_USN3/-uIi~>M+c ;Oq]E{t9)RC_5|lhAA_Qq%_4";

            /// <summary>
            /// Версия приложения
            /// </summary>
            public string AppVersion { get; internal set; } = "4.9.1";

            /// <summary>
            /// Версия ОС где было приложение
            /// </summary>
            public string Os { get; internal set; } = "android 5.0";

            /// <summary>
            /// ID устройства где было приложение
            /// </summary>
            public string DeviceId { get; internal set; } = "14130e29cebe9c39";

            /// <summary>
            /// User-Agent посылаемый приложением с устройства
            /// </summary>
            public string UserAgent { get; internal set; } = "Dalvik/2.1.0 (Linux; U; Android 5.0; NULL Build/NRD90M)";

            internal Config(string token, string aesKey)
            {
                Token = token;
                AesKey = aesKey;
            }
        }

        private readonly Config _config;

        public ConfigBuilder(string token, string aesKey)
        {
            _config = new Config(token, aesKey);
        }

        /// <summary>
        /// Заменить ключ для шифрования подписи
        /// </summary>
        /// <param name="value">Значение</param>
        public ConfigBuilder WithKey(string value)
            => SetProperty(c => c.Key = value);

        /// <summary>
        /// Заменить версию приложения
        /// </summary>
        /// <param name="value">Значение</param>
        public ConfigBuilder WithAppVersion(string value)
            => SetProperty(c => c.AppVersion = value);

        /// <summary>
        /// Заменить версию ОС
        /// </summary>
        /// <param name="value">Значение</param>
        public ConfigBuilder WithOs(string value)
            => SetProperty(c => c.Os = value);

        /// <summary>
        /// Заменить ID устройства
        /// </summary>
        /// <param name="value">Значение</param>
        public ConfigBuilder WithDeviceId(string value)
            => SetProperty(c => c.DeviceId = value);

        /// <summary>
        /// Заменить UserAgent посылаемый приложением
        /// </summary>
        /// <param name="value">Значение</param>
        public ConfigBuilder WithUserAgent(string value)
            => SetProperty(c => c.UserAgent = value);

        private ConfigBuilder SetProperty(Action<Config> fn)
        {
            fn.Invoke(_config);
            return this;
        }

        public Config Build()
            => _config;
    }
}