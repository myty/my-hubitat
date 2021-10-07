using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http;
using MyHubitatFunc.Conductors;
using MyHubitatFunc.Controllers;
using MyHubitatFunc.Interfaces.Conductors;
using MyHubitatFunc.Interfaces.Controllers;
using MyHubitatFunc.Interfaces.Providers;
using MyHubitatFunc.Providers;

namespace MyHubitatFunc
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices((context, services) =>
                {
                    services.AddHttpClient();
                    services.AddSingleton<IEnvironmentProvider, EnvironmentProvider>();
                    services.AddSingleton<ISunriseSunsetInfoConductor, SunriseSunsetInfoConductor>();
                    services.AddSingleton<IHubitatController>(sp =>
                    {
                        var environmentProvider = sp.GetService<IEnvironmentProvider>();
                        var httpClient = sp.GetService<HttpClient>();

                        var accessToken = environmentProvider.GetEnvironmentVariable("HUBITAT_ACCESS_TOKEN");
                        var hubitatConnection = environmentProvider.GetEnvironmentVariable("HUBITAT_CONNECTION");

                        return new HubitatController(accessToken, httpClient, hubitatConnection);
                    });
                })
                .Build();

            host.Run();
        }
    }
}