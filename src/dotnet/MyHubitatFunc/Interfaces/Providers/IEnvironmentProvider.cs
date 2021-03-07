using System;

namespace MyHubitatFunc.Interfaces.Providers
{
    public interface IEnvironmentProvider
    {
        string GetEnvironmentVariable(string variable, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process);
    }
}