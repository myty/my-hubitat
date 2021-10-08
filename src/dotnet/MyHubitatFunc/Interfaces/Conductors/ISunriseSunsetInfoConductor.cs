using System.Threading.Tasks;
using MyHubitatFunc.Models;

namespace MyHubitatFunc.Interfaces.Conductors
{
    public interface ISunriseSunsetInfoProvider
    {
        Task<SunriseSunsetInfo> GetSunriseSunsetInfoAsync(string latitude, string longitude, string date = "today", bool formatted = false);
    }
}