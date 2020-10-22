using Magentix.Infrastructure.Helpers;
using Magentix.Infrastructure.Settings;
using Magentix.Services;
using Magentix.Services.Common;
using Magentix.Services.Common.Device;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Magentix.Services.Implementations
{
    [Export(typeof(IDeviceService))]
    class DeviceService : IDeviceService
    {
        [ImportMany]
        public IEnumerable<IDeviceFactory> DeviceFactories
        {
            get;
            set;
        }
        public IEnumerable<IDevice> Devices { get; set; }

        public DeviceService()
        {
            this.Devices = Enumerable.Empty<IDevice>();
            if (!string.IsNullOrEmpty(LocalSettings.CallerIdDeviceName))
            {
                this.AddDevice(LocalSettings.CallerIdDeviceName);
            }
            LocalSettings.CallerIdDeviceName = "";
            LocalSettings.SaveSettings();
        }

        public IEnumerable<string> GetDeviceNames(DeviceType deviceType)
        {
            return Devices.Where(x => x.DeviceType == deviceType).Select(x => x.Name);
        }
        public IEnumerable<string> GetDeviceNames()
        {
            return
                from x in this.DeviceFactories
                select x.Name;
        }

        public void InitializeDevice(string deviceName)
        {
            this.CreateDevices();
            if (Devices.Any(x => x.Name == deviceName))
                Devices.Single(x => x.Name == deviceName).InitializeDevice();
        }

        public void FinalizeDevices()
        {
            Devices.ToList().ForEach(x => x.FinalizeDevice());
        }

        public IDevice GetDeviceByName(string deviceName)
        {
            this.CreateDevices();
            IDevice device = this.Devices.FirstOrDefault<IDevice>((IDevice x) => {
                if (x.Name != deviceName)
                {
                    return false;
                }
                else
                    return true;
            });
            if (device == null)
            {
                return null;
            }

            return device;
        }

        public DeviceInstallation AddDevice(string deviceName)
        {
            DeviceInstallation deviceInstallation = new DeviceInstallation(deviceName);
            List<DeviceInstallation> installedDevices = this.GetInstalledDevices();
            installedDevices.Add(deviceInstallation);
            LocalSettings.AdditionalDevices = JsonHelper.Serialize<List<DeviceInstallation>>(installedDevices);
            return deviceInstallation;
        }

        public object GetDeviceSettings(string deviceName, string key)
        {
            this.CreateDevices();
            IDevice device = this.Devices.FirstOrDefault<IDevice>((IDevice x) => {
                if (x.Name != deviceName)
                {
                    return false;
                }
                return x.Key == key;
            });
            if (device == null)
            {
                return null;
            }
            return device.GetSettingsObject();
        }

        public List<DeviceInstallation> GetInstalledDevices()
        {
            return JsonHelper.Deserialize<List<DeviceInstallation>>(LocalSettings.AdditionalDevices);
        }

        public void InitializeDevices()
        {
            this.FinalizeDevices();
            this.CreateDevices();
            this.Devices.ToList<IDevice>().ForEach((IDevice x) => x.InitializeDevice());
        }

        private IDeviceFactory GetDeviceFactory(string name)
        {
            return this.DeviceFactories.FirstOrDefault<IDeviceFactory>((IDeviceFactory x) => x.Name == name);
        }

        private void CreateDevices()
        {
            this.Devices = this.GetInstalledDevices().Select<DeviceInstallation, IDevice>((DeviceInstallation x) => {
                IDeviceFactory deviceFactory = this.GetDeviceFactory(x.DeviceName);
                if (deviceFactory == null)
                {
                    return null;
                }
                return deviceFactory.GenerateDevice(x.Key);
            }).Where<IDevice>((IDevice x) => x != null).ToList<IDevice>();
        }

        public void RemoveDevice(string deviceName, string key)
        {
            List<DeviceInstallation> installedDevices = this.GetInstalledDevices();
            DeviceInstallation deviceInstallation = installedDevices.FirstOrDefault<DeviceInstallation>((DeviceInstallation x) => {
                if (x.DeviceName != deviceName)
                {
                    return false;
                }
                return x.Key == key;
            });
            if (deviceInstallation != null)
            {
                installedDevices.Remove(deviceInstallation);
            }
            LocalSettings.AdditionalDevices = JsonHelper.Serialize<List<DeviceInstallation>>(installedDevices);
        }

        public void SaveDevices(IEnumerable<DeviceInstallation> deviceInstallations)
        {
            LocalSettings.AdditionalDevices = JsonHelper.Serialize<List<DeviceInstallation>>((
                from x in deviceInstallations
                where !string.IsNullOrEmpty(x.DeviceName)
                select x).ToList<DeviceInstallation>());
        }
    }
}
