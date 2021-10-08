using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyHubitatFunc.Interfaces.Conductors;
using MyHubitatFunc.Interfaces.Controllers;
using MyHubitatFunc.Interfaces.Providers;
using MyHubitatFunc.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;

namespace MyHubitatFunc.Functions
{
    public class MyHubitatEvents
    {
        private readonly IEnvironmentProvider _environmentProvider;
        private readonly ISunriseSunsetInfoProvider _sunriseSunsetInfoProvider;
        private readonly IHubitatController _hubitatController;
        private readonly ILogger<MyHubitatEvents> _log;

        public MyHubitatEvents(
            IEnvironmentProvider environmentProvider,
            IHubitatController hubitatController,
            ILogger<MyHubitatEvents> log,
            ISunriseSunsetInfoProvider sunriseSunsetInfoProvider)
        {
            _hubitatController = hubitatController;
            _log = log;
            _environmentProvider = environmentProvider;
            _sunriseSunsetInfoProvider = sunriseSunsetInfoProvider;
        }

        [Function("my-hubitat-events")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequestData req)
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

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync("Success");

            return response;
        }

        private async Task<bool> ShouldBeOn()
        {
            var latitude = _environmentProvider.GetEnvironmentVariable("HUBITAT_LATITUDE", EnvironmentVariableTarget.Process);
            var longitude = _environmentProvider.GetEnvironmentVariable("HUBITAT_LONGITUDE", EnvironmentVariableTarget.Process);

            var sunriseSunsetInfo = await _sunriseSunsetInfoProvider.GetSunriseSunsetInfoAsync(latitude, longitude);

            var sunrise = sunriseSunsetInfo.Results.Sunrise.AddMinutes(-15);
            var sunset = sunriseSunsetInfo.Results.Sunset.AddMinutes(15);

            return sunrise > DateTimeOffset.UtcNow || sunset < DateTimeOffset.UtcNow;
        }
    }
}
