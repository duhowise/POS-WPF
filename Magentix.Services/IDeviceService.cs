using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magentix.Services.Common;
using Magentix.Services.Common.Device;

namespace Magentix.Services
{
    public interface IDeviceService
    {
        IEnumerable<string> GetDeviceNames(DeviceType deviceType);
        IEnumerable<string> GetDeviceNames();
        void InitializeDevice(string deviceName);
        void FinalizeDevices();
        IDevice GetDeviceByName(string deviceName);


        DeviceInstallation AddDevice(string deviceName);


        object GetDeviceSettings(string deviceName, string key);

        List<DeviceInstallation> GetInstalledDevices();

        void InitializeDevices();

        void RemoveDevice(string deviceName, string key);

        void SaveDevices(IEnumerable<DeviceInstallation> deviceInstallations);
    }
}
