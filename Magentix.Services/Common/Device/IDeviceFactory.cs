using System;

namespace Magentix.Services.Common.Device
{
    public interface IDeviceFactory
    {
        string Name
        {
            get;
        }

        IDevice GenerateDevice(string key);
    }
}