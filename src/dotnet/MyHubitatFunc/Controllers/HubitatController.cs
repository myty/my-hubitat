using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MyHubitatFunc.Interfaces.Controllers;

namespace MyHubitatFunc.Controllers
{
    public class HubitatController : IHubitatController
    {
        private readonly string _accessToken;
        private readonly HttpClient _httpClient;
        private readonly string _hubitatConnection;
        public HubitatController(
            string accessToken,
            HttpClient httpClient,
            string hubitatConnection)
        {
            _accessToken = accessToken;
            _httpClient = httpClient;
            _hubitatConnection = hubitatConnection;
        }

        public async Task<string> SendCommand(long deviceId, string command, string secondaryValue = null)
        {
            var commandUrl = $"{BuildCommandUrl(deviceId, command, secondaryValue)}?access_token={_accessToken}";

            _ = await _httpClient.GetAsync(commandUrl);

            return commandUrl;
        }

        string BuildCommandUrl(long deviceId, string command, string secondaryValue = null)
        {
            var segments = new List<string>
            {
                _hubitatConnection,
                "devices",
                $"{deviceId}",
                command
            };

            if (!string.IsNullOrWhiteSpace(secondaryValue))
            {
                segments.Add(secondaryValue);
            }

            return string.Join("/", segments);
        }
    }
}