using Magentix.Infrastructure.Helpers;
using Magentix.Infrastructure.Settings;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Magentix.Services.Common.Device
{
    public abstract class AbstractDevice : IDevice
    {
        public Magentix.Services.Common.Device.DeviceType DeviceType
        {
            get
            {
                return this.GetDeviceType();
            }
        }

        private bool IsInitialized { get; set; }

        public string Name { get; set; }
        public string Key { get; set; }

        protected AbstractDevice(string name, string key)
        {
            this.Name = name;
            this.Key = key;
        }

        protected abstract void DoFinalize();

        protected abstract bool DoInitialize();

        public void FinalizeDevice()
        {
            if (!this.IsInitialized)
            {
                return;
            }
            this.OnFinalize();
            try
            {
                this.DoFinalize();
            }
            catch (Exception)
            {
                this.IsInitialized = false;
            }
            this.IsInitialized = false;
        }

        protected abstract Magentix.Services.Common.Device.DeviceType GetDeviceType();

        protected string GetSettingFileName()
        {
            return string.Format("{0}\\{1}_{2}SettingsJson.txt", LocalSettings.DataPath, this.Name, this.Key);
        }

        protected abstract AbstractSettings GetSettings();

        public object GetSettingsObject()
        {
            return this.GetSettings();
        }

        public void InitializeDevice()
        {
            this.SaveSettings();
            if (this.IsInitialized)
            {
                return;
            }
            this.OnInitialize();
            try
            {
                this.IsInitialized = this.DoInitialize();
            }
            catch (Exception)
            {
                this.IsInitialized = false;
            }
        }

        protected T LoadSettings<T>()
        where T : class, new()
        {
            if (!File.Exists(this.GetSettingFileName()))
            {
                return Activator.CreateInstance<T>();
            }
            return JsonHelper.Deserialize<T>(File.ReadAllText(this.GetSettingFileName()));
        }

        protected virtual void OnFinalize()
        {
        }

        protected virtual void OnInitialize()
        {
        }

        public void SaveSettings()
        {
            string str = JsonHelper.Serialize<AbstractSettings>(this.GetSettings());
            File.WriteAllText(this.GetSettingFileName(), str);
        }
    }
}
