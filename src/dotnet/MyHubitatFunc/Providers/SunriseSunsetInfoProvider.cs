using System.Net.Http;
using System.Threading.Tasks;
using MyHubitatFunc.Interfaces.Conductors;
using MyHubitatFunc.Models;

namespace MyHubitatFunc.Providers
{
    public class SunriseSunsetInfoProvider : ISunriseSunsetInfoProvider
    {
        private readonly HttpClient _httpClient;

        public SunriseSunsetInfoProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<SunriseSunsetInfo> GetSunriseSunsetInfoAsync(string latitude, string longitude, string date = "today", bool formatted = false)
        {
            var formattedValue = formatted ? 1 : 0;
            var url = $"https://api.sunrise-sunset.org/json?lat={latitude}&lng={longitude}&date={date}&formatted={formattedValue}";
            var responseMessage = await _httpClient.GetAsync(url);

            var response = await responseMessage.Content.ReadAsStringAsync();

            return SunriseSunsetInfo.FromJson(response);

        }
    }
}