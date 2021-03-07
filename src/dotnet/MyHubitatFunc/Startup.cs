using System;
using System.Net.Http;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using MyHubitatFunc.Conductors;
using MyHubitatFunc.Controllers;
using MyHubitatFunc.Interfaces.Conductors;
using MyHubitatFunc.Interfaces.Controllers;
using MyHubitatFunc.Interfaces.Providers;
using MyHubitatFunc.Providers;

[assembly: FunctionsStartup(typeof(MyHubitatFunc.Startup))]

namespace MyHubitatFunc
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();
            builder.Services.AddSingleton<IEnvironmentProvider, EnvironmentProvider>();
            builder.Services.AddSingleton<ISunriseSunsetInfoConductor, SunriseSunsetInfoConductor>();
            builder.Services.AddSingleton<IHubitatController>(sp =>
            {
                var environmentProvider = sp.GetService<IEnvironmentProvider>();
                var httpClient = sp.GetService<HttpClient>();

                var accessToken = environmentProvider.GetEnvironmentVariable("HUBITAT_ACCESS_TOKEN");
                var hubitatConnection = environmentProvider.GetEnvironmentVariable("HUBITAT_CONNECTION");

                return new HubitatController(accessToken, httpClient, hubitatConnection);
            });
        }
    }
}