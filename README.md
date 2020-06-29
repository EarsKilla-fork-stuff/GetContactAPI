# GetContactAPI
Неофициальное .NET API для сервиса GetContact.

Позволяет по заданному номеру телефона узнать имя человека.

> Работает только с номерами KZ,RU,BY,KG,UA.

## Установка
1) Скачать [последнюю](https://github.com/SijyKijy/GetContactAPI/releases) версию библиотеки.
2) Добавить её к вашему проекту.

## Использование
1. Создать экземпляр объекта `API`.
2. В параметре заполнить объект **Data** введя токен и зашифрованный ключ взятые из [конфига](#Конфиг).
3. Вызвать нужный метод: 
   * `GetByPhone()` - получить [данные](#Данные) пользователя по номеру телефона. 
   * `GetTags()` - получить [теги](#Теги) (*если доступны*) по номеру телефона.

Пример:
```C#
API api = new API(new Data(
   "token",
   "aes_key"
    ));

var profile = api.GetByPhone("+71234569780");

string name = profile.Name;
```

## Прочее

### Конфиг
Токен и ключ находятся в файле `GetContactSettingsPref.xml` по пути `/data/data/app.source.getcontact/shared_prefs/` на устройстве с установленным приложением.
> Устройство должно быть рутировано.

### Теги
> Тег - это информация, которая добавляется пользователями Getcontact к профилям других пользователей (псевдоним, имя и т. д.). Эти теги могут быть отредактированы отдельными лицами.

### Данные
Объект содержащий в себе данные об:
* Имени пользователя `Name`.
* Стране `Country`.
* Количестве найденных тегов `TagCount`.
* Количестве оставшихся запросов / максимальных запросов `DefaultSearchCount`.
* Количестве оставшихся запросов для тегов / максимальных запросов `TagSearchCount`.
