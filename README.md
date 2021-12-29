# FeatureManagement.ImplementationSwitchExtensions

## EN

FeatureManagement.ImplementationSwitchExtensions contains DI extension methods for switching implementation of interfaces.

## RU

### Описание

FeatureManagement.ImplementationSwitchExtensions содержит методы расширения [DI](https://docs.microsoft.com/ru-ru/dotnet/core/extensions/dependency-injection) для переключения реализаций интерфейсов.

Для управления переключателями функциональности используется NuGet-пакет [Microsoft.FeatureManagement](https://www.nuget.org/packages/Microsoft.FeatureManagement/). Официальная документация [здесь](https://docs.microsoft.com/ru-ru/azure/azure-app-configuration/use-feature-flags-dotnet-core?tabs=core5x "Руководство по использованию флагов функций в приложении ASP.NET Core"). С примерами настройки и использования переключателей можно ознакомиться в [этой серии статей](https://andrewlock.net/series/adding-feature-flags-to-an-asp-net-core-app/ "Series: Adding feature flags to an ASP.NET Core app").

### Прототипы методов NuGet-пакета FeatureManagement.ImplementationSwitchExtensions

Всего три метода расширения:

```csharp
public static IServiceCollection AddTransientFeature<TService, TFeatureOnImplementation, TFeatureOffImplementation>
    (this IServiceCollection services, string featureName)
    where TService : class
    where TFeatureOnImplementation : class, TService
    where TFeatureOffImplementation : class, TService;
```

```csharp
public static IServiceCollection AddScopedFeature<TService, TFeatureOnImplementation, TFeatureOffImplementation>
    (this IServiceCollection services, string featureName)
    where TService : class
    where TFeatureOnImplementation : class, TService
    where TFeatureOffImplementation : class, TService;
```

```csharp
public static IServiceCollection AddSingletonFeature<TService, TFeatureOnImplementation, TFeatureOffImplementation>
    (this IServiceCollection services, string featureName)
    where TService : class
    where TFeatureOnImplementation : class, TService
    where TFeatureOffImplementation : class, TService;
```

> Метод AddSingletonFeature на самом деле не выполняет поставленных задач и не переключает функциональность во время работы приложения. Это связано с тем, что создание экземпляра происходит лишь один раз. Тем не менее я решил добавить его "для комплекта". Возможно он тоже найдет своё применение в виду его единообразного использования наряду с `AddTransientFeature` и `AddScopedFeature`.

### Использование NuGet-пакета FeatureManagement.ImplementationSwitchExtensions

Предположим у вас имеется интерфейс ISomething и его реализация в классе Something. Тогда регистрация в DI реализуется одним из следующих способов, в зависимости от [времени существования службы](https://docs.microsoft.com/ru-ru/dotnet/core/extensions/dependency-injection#service-lifetimes):

```csharp
services.AddTransient<ISomething, Something>();
```

или

```csharp
services.AddScoped<ISomething, Something>();
```

или

```csharp
services.AddSingleton<ISomething, Something>();
```

Нужно создать альтернативную реализацию интерфейса ISomething, например в классе SomethingStub. Таким образом оба класса Something и SomethingStub реализуют интерфейс ISomething.

Для использования переключателя функциональности требуется завести соответствующий раздел в файле конфигурации приложения `appsettings.json`:

```json
{
  "FeatureManagement": {
    "UseSomethingStub": false,
  },

  // ...
}
```

Так же необходимо добавить регистрацию библиотеки переключателей функциональности `Microsoft.FeatureManagement`:

```csharp
services.AddFeatureManagement();
```

Осталось заменить регистрацию реализации интерфейса в DI соответствующим способом:

```csharp
services.AddTransientFeature<ISomething, SomethingStub, Something>("UseSomethingStub");
```

или

```csharp
services.AddScopedFeature<ISomething, SomethingStub, Something>("UseSomethingStub");
```

или

```csharp
services.AddSingletonFeature<ISomething, SomethingStub, Something>("UseSomethingStub");
```

В указанных примерах регистрации константная строка "UseSomethingStub" должна быть одноименной с соответствующим разделом в файле конфигурации  `appsettings.json`: "FeatureManagement:UseSomethingStub".

Важно! Если вы разрешаете тип HttpClient в классе Something, то после регистрации методами группы ...Feature необходимо заменить регистрацию HTTP-клиента с такого варианта:

```csharp
services.AddHttpClient<ISomething, Something>( //...
```

на такой:

```csharp
services.AddHttpClient<Something>( //...
```

Всё готово! При переключении значения "FeatureManagement:UseSomethingStub" конфигурации интерфейс ISomething будет разрешаться DI соответствующим классом SomethingStub или Something.

Подробности использования в [статье](https://habr.com/ru/company/akbarsdigital/blog/597541/).
