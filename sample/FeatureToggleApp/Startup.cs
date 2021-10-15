using System;
using FeatureToggleApp.Configurations;
using FeatureToggleApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using Microsoft.OpenApi.Models;

namespace FeatureToggleApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<OpenWeatherConfiguration>(Configuration.GetSection("App:OpenWeather"));

            services.AddFeatureManagement()
                .UseDisabledFeaturesHandler(new RedirectDisabledFeatureHandler());

            // Регистрация OpenWeatherService и конфигурирование HTTP-клиента для него.
            services.AddHttpClient<OpenWeatherService>(
                "OpenWeather",
                (serviceProvider, client) =>
                {
                    // Настройка базового адреса, согласно конфигурации.
                    var config = serviceProvider.GetRequiredService<IOptions<OpenWeatherConfiguration>>().Value;
                    client.BaseAddress = new Uri(config.BaseAddress);
                });

            services.AddScopedFeature<IWeatherForecastService, OpenWeatherServiceStub, OpenWeatherService>(FeatureFlags.UseOpenWeatherStub);

            services.AddAutoMapper(typeof(Startup));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FeatureToggleApp", Version = "v1" });
            });

            bool suppressAuth = Configuration.GetValue<bool>($"FeatureManagement:{FeatureFlags.OnStart_SuppressAuth}");
            if (suppressAuth)
                services.AddRouting(r => r.SuppressCheckForUnhandledSecurityMetadata = true);
            else
            {
                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer();
                services.AddAuthorization(
                    options =>
                    options.AddPolicy(
                        "Something",
                        policy =>
                        {
                            policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                            policy.RequireClaim("Permission", "CanViewPage", "CanViewAnything");
                        }));
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IFeatureManager featureManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FeatureToggleApp v1"));
            }

            app.UseRouting();

            bool suppressAuth = featureManager.IsEnabledAsync(FeatureFlags.OnStart_SuppressAuth).GetAwaiter().GetResult();
            if (!suppressAuth)
            {
                app.UseAuthentication();
                app.UseAuthorization();
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
