using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using MyHubitatFunc.Interfaces.Conductors;
using MyHubitatFunc.Interfaces.Controllers;
using MyHubitatFunc.Interfaces.Providers;
using MyHubitatFunc.Models;

namespace MyHubitatFunc.Functions
{
    public class MyHubitatEvents
    {
        private readonly IEnvironmentProvider _environmentProvider;
        private readonly ISunriseSunsetInfoConductor _sunriseSunsetInfoConductor;
        private readonly IHubitatController _hubitatController;
        private readonly ILogger<MyHubitatEvents> _log;

        public MyHubitatEvents(
            IEnvironmentProvider environmentProvider,
            IHubitatController hubitatController,
            ILogger<MyHubitatEvents> log,
            ISunriseSunsetInfoConductor sunriseSunsetInfoConductor)
        {
            _hubitatController = hubitatController;
            _log = log;
            _environmentProvider = environmentProvider;
            _sunriseSunsetInfoConductor = sunriseSunsetInfoConductor;
        }

        [FunctionName("my-hubitat-events")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var hubitatEvent = HubitatEvent.FromJson(requestBody);

            if (hubitatEvent.Content.Value == "off" && await ShouldBeOn())
            {
                var commandUrl = await _hubitatController.SendCommand(hubitatEvent.Content.DeviceId, "on");

                _log.LogInformation($"Device: '{hubitatEvent.Content.DeviceId}' turned back 'on' after 'off' event.", new
                {
                    hubitatEvent,
                    commandUrl
                });
            }

            return new OkObjectResult("Success");
        }

        private async Task<bool> ShouldBeOn()
        {
            var latitude = _environmentProvider.GetEnvironmentVariable("HUBITAT_LATITUDE", EnvironmentVariableTarget.Process);
            var longitude = _environmentProvider.GetEnvironmentVariable("HUBITAT_LONGITUDE", EnvironmentVariableTarget.Process);

            var sunriseSunsetInfo = await _sunriseSunsetInfoConductor.GetSunriseSunsetInfoAsync(latitude, longitude);

            var sunrise = sunriseSunsetInfo.Results.Sunrise.AddMinutes(-15);
            var sunset = sunriseSunsetInfo.Results.Sunset.AddMinutes(15);

            return sunrise > DateTimeOffset.UtcNow || sunset < DateTimeOffset.UtcNow;
        }
    }
}
