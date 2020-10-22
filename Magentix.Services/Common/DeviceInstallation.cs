using Magentix.Infrastructure;
using System;
using System.Runtime.CompilerServices;

namespace Magentix.Services.Common
{
    public class DeviceInstallation
    {
        public string Description
        {
            get;
            set;
        }

        public string DeviceName
        {
            get;
            set;
        }

        public string Key
        {
            get;
            set;
        }

        public DeviceInstallation()
        {
        }

        public DeviceInstallation(string deviceName)
        {
            this.DeviceName = deviceName;
            this.Key = ShortGuid.NewGuid();
        }
    }
}
