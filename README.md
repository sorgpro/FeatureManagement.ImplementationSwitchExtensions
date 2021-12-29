# FeatureManagement.ImplementationSwitchExtensions

## EN

FeatureManagement.ImplementationSwitchExtensions contains DI extension methods for switching implementation of interfaces.

## RU

### ��������

FeatureManagement.ImplementationSwitchExtensions �������� ������ ���������� [DI](https://docs.microsoft.com/ru-ru/dotnet/core/extensions/dependency-injection) ��� ������������ ���������� �����������.

��� ���������� ��������������� ���������������� ������������ NuGet-����� [Microsoft.FeatureManagement](https://www.nuget.org/packages/Microsoft.FeatureManagement/). ����������� ������������ [�����](https://docs.microsoft.com/ru-ru/azure/azure-app-configuration/use-feature-flags-dotnet-core?tabs=core5x "����������� �� ������������� ������ ������� � ���������� ASP.NET Core"). � ��������� ��������� � ������������� �������������� ����� ������������ � [���� ����� ������](https://andrewlock.net/series/adding-feature-flags-to-an-asp-net-core-app/ "Series: Adding feature flags to an ASP.NET Core app").

### ��������� ������� NuGet-������ FeatureManagement.ImplementationSwitchExtensions

����� ��� ������ ����������:

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

> ����� AddSingletonFeature �� ����� ���� �� ��������� ������������ ����� � �� ����������� ���������������� �� ����� ������ ����������. ��� ������� � ���, ��� �������� ���������� ���������� ���� ���� ���. ��� �� ����� � ����� �������� ��� "��� ���������". �������� �� ���� ������ ��� ���������� � ���� ��� �������������� ������������� ������ � `AddTransientFeature` � `AddScopedFeature`.

### ������������� NuGet-������ FeatureManagement.ImplementationSwitchExtensions

����������� � ��� ������� ��������� ISomething � ��� ���������� � ������ Something. ����� ����������� � DI ����������� ����� �� ��������� ��������, � ����������� �� [������� ������������� ������](https://docs.microsoft.com/ru-ru/dotnet/core/extensions/dependency-injection#service-lifetimes):

```csharp
services.AddTransient<ISomething, Something>();
```

���

```csharp
services.AddScoped<ISomething, Something>();
```

���

```csharp
services.AddSingleton<ISomething, Something>();
```

����� ������� �������������� ���������� ���������� ISomething, �������� � ������ SomethingStub. ����� ������� ��� ������ Something � SomethingStub ��������� ��������� ISomething.

��� ������������� ������������� ���������������� ��������� ������� ��������������� ������ � ����� ������������ ���������� `appsettings.json`:

```json
{
  "FeatureManagement": {
    "UseSomethingStub": false,
  },
  // ...
}
```

��� �� ���������� �������� ����������� ���������� �������������� ���������������� `Microsoft.FeatureManagement`:

```csharp
services.AddFeatureManagement();
```

�������� �������� ����������� ���������� ���������� � DI ��������������� ��������:

```csharp
services.AddTransientFeature<ISomething, SomethingStub, Something>("UseSomethingStub");
```

���

```csharp
services.AddScopedFeature<ISomething, SomethingStub, Something>("UseSomethingStub");
```

���

```csharp
services.AddSingletonFeature<ISomething, SomethingStub, Something>("UseSomethingStub");
```

� ��������� �������� ����������� ����������� ������ "UseSomethingStub" ������ ���� ����������� � ��������������� �������� � ����� ������������  `appsettings.json`: "FeatureManagement:UseSomethingStub".

�����! ���� �� ���������� ��� HttpClient � ������ Something, �� ����� ����������� �������� ������ ...Feature ���������� �������� ����������� HTTP-������� � ������ ��������:

```csharp
services.AddHttpClient<ISomething, Something>( //...
```

�� �����:

```csharp
services.AddHttpClient<Something>( //...
```

�� ������! ��� ������������ �������� "FeatureManagement:UseSomethingStub" ������������ ��������� ISomething ����� ����������� DI ��������������� ������� SomethingStub ��� Something.

����������� ������������� � [������](https://habr.com/ru/company/akbarsdigital/blog/597541/).
