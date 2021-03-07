using System.Threading.Tasks;

namespace MyHubitatFunc.Interfaces.Controllers
{
    public interface IHubitatController
    {
        Task<string> SendCommand(long deviceId, string command, string secondaryValue = null);
    }
}