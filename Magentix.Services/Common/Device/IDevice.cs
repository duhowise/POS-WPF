using System;

namespace Magentix.Services.Common.Device
{
    public interface IDevice
    {
        Magentix.Services.Common.Device.DeviceType DeviceType
        {
            get;
        }

        string Key
        {
            get;
        }

        string Name
        {
            get;
        }

        void FinalizeDevice();

        object GetSettingsObject();

        void InitializeDevice();

        void SaveSettings();
    }
}