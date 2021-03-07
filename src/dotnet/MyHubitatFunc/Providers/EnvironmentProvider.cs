using System;
using MyHubitatFunc.Interfaces.Providers;

namespace MyHubitatFunc.Providers
{
    public class EnvironmentProvider : IEnvironmentProvider
    {
        public string GetEnvironmentVariable(string variable, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
        {
            return Environment.GetEnvironmentVariable(variable, target);
        }
    }
}