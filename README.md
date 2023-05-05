# FeatureManagement.ImplementationSwitchExtensions

## Description

FeatureManagement.ImplementationSwitchExtensions contains [DI](https://docs.microsoft.com/ru-ru/dotnet/core/extensions/dependency-injection) extension methods for swithing interface implimentations.

The [Microsoft.FeatureManagement](https://www.nuget.org/packages/Microsoft.FeatureManagement/) package is used to manag feature toggles. The official documentation [is here](https://docs.microsoft.com/ru-ru/azure/azure-app-configuration/use-feature-flags-dotnet-core?tabs=core5x). In [this article series](https://andrewlock.net/series/adding-feature-flags-to-an-asp-net-core-app/) you can get acquainted with examples how to set up and use the switches.

This document is also available in [Russian](README-ru.md).

## The NuGet FeatureManagement.ImplementationSwitchExtensions package methods prototypes

There are only three methods of expansion:

``` C#
public static IServiceCollection AddTransientFeature<TService, TFeatureOnImplementation, TFeatureOffImplementation>
    (this IServiceCollection services, string featureName)
    where TService : class
    where TFeatureOnImplementation : class, TService
    where TFeatureOffImplementation : class, TService;
```

``` C#
public static IServiceCollection AddScopedFeature<TService, TFeatureOnImplementation, TFeatureOffImplementation>
    (this IServiceCollection services, string featureName)
    where TService : class
    where TFeatureOnImplementation : class, TService
    where TFeatureOffImplementation : class, TService;
```

``` C#
public static IServiceCollection AddSingletonFeature<TService, TFeatureOnImplementation, TFeatureOffImplementation>
    (this IServiceCollection services, string featureName)
    where TService : class
    where TFeatureOnImplementation : class, TService
    where TFeatureOffImplementation : class, TService;
```

> The AddSingletonFeature method doesn't really do the job and doesn't switch functionality while the application is running. This is due to the fact that the creation of an instance occurs only once. However, I decided to add it for the sake of completeness. Perhaps, it can find a use for somebody due to its uniform use along with `AddTransientFeature` and `AddScopedFeature`.

## Using the NuGet FeatureManagement.ImplementationSwitchExtensions package

Lets suppose you have ISomething interface and its Something realization. The DI registration is then implemented via one of the following methodes, depending on its [time of servise](https://docs.microsoft.com/ru-ru/dotnet/core/extensions/dependency-injection#service-lifetimes):

``` C#
services.AddTransient<ISomething, Something>(); 
```

or

``` C#
services.AddScoped<ISomething, Something>();
```

or

``` C#
services.AddSingleton<ISomething, Something>();
```

You need to create an alternative ISomething alternative realisation e.g. in SomethingStub class. Thus, both classes Something and SomethingStub implement ISomething interface.  
To use feature toggle you need to have corresponding section in the `appsettings.json` application configuration file:

``` C#
{
  "FeatureManagement": {
    "UseSomethingStub": false,
  },
}
```

Also you need to add feature toggle `Microsoft.FeatureManagement` library registration:

``` C#
services.AddFeatureManagement();
```

Remains replace DI interface registration in an appropriate way:

``` C#
services.AddTransientFeature<ISomething, SomethingStub, Something>("UseSomethingStub");
```

or

``` C#
services.AddScopedFeature<ISomething, SomethingStub, Something>("UseSomethingStub");
```

or

``` C#
services.AddSingletonFeature<ISomething, SomethingStub, Something>("UseSomethingStub");
```

In the above registration examples the constant string "UseSomethingStub" must be of the same name as the corresponding section in configuration file `appsettings.json`: "FeatureManagement:UseSomethingStub".  
Important! If you allow HttpClient type in the Something class, then after registering with the ...Feature group methods you should change registration of HTTP-client from this variant:  

``` C#
services.AddHttpClient<ISomething, Something>( //...
```

to this:

``` C#
services.AddHttpClient<Something>( //... 
```

Done! When you toggle "FeatureManagement:UseSomethingStub" configuration value, the ISomething interface will be resolved by the DI with appropriate SomethingStub or Something class.  

Details of exploitation in this [article](https://habr.com/ru/company/akbarsdigital/blog/597541/) [ru].
