# GetContactAPI
Неофициальное .NET API для сервиса GetContact.

Позволяет по заданному номеру телефона узнать имя человека.

> Работает только с номерами KZ,RU,BY,KG,UA.
##### Но это не точно, можно попробовать указать страну (countryCode в методе GetByPhone) в формате [ISO 3166-1 alpha-2](https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2)

## Установка
1) Скачать [последнюю](https://github.com/SijyKijy/GetContactAPI/releases) ([NuGet](https://www.nuget.org/packages/GetContactAPI_SijyKijy/)) версию библиотеки.
2) Добавить её к вашему проекту.

## Использование
1. Создать экземпляр объекта `API`.
2. В параметре заполнить объект **Data** введя токен и зашифрованный ключ взятые из [конфига](#Конфиг).
3. Вызвать нужный метод: 
   * `GetByPhone()` - получить [данные](#Данные) пользователя по номеру телефона. 
   * `GetTags()` - получить [теги](#Теги) (*если доступны*) по номеру телефона.

Пример:
```C#
API api = new API(new ConfigBuilder("token", "aesKey (он же FINAL_KEY в конфиге)").Build());
// опционально: можно заменить DeviceId, Os, AppVersion используя методы до вызова метода Build

var phoneInfo = api.GetByPhone("+71234569780");
// опционально: можно указать кастомные countryCode и source, если не знаете, просто оставьте как есть

if (phoneInfo.Meta.IsRequestError)
{
    // ваша обработка ошибки
    return;
}

string name = phoneInfo.Response.Profile.DisplayName;
```

## Прочее

### Конфиг
Токен и ключ находятся в файле `GetContactSettingsPref.xml` по пути `/data/data/app.source.getcontact/shared_prefs/` на устройстве с установленным приложением.
> Устройство должно быть рутировано.
Токен лежит между `<string name="TOKEN">` и `</string>`
Ключ лежит между `<string name="FINAL_KEY">` и `</string>`

### Теги
> Тег - это информация, которая добавляется пользователями Getcontact к профилям других пользователей (псевдоним, имя и т. д.). Эти теги могут быть отредактированы отдельными лицами.

### Данные профиля
Объект содержащий в себе данные об:
* Отображаемом имени пользователя `DisplayName`.
* Имени пользователя `DisplayName` (в большинстве случаев `null`).
* Фамилии пользователя `Surname` (в большинстве случаев `null`).
* Стране `Country`.
* Количестве найденных тегов `TagCount`.
* Номере телефона `PhoneNumber`.
* Изображении профиля `ProfileImage` (в большинстве случаев `null`).
