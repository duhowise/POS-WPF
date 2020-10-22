using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentValidation.Results;
using Magentix.Infrastructure.Settings;
using Magentix.Localization.Properties;
using Magentix.Presentation.Common.Commands;
using Magentix.Presentation.Common.ModelBase;
using Magentix.Presentation.Common.Services;
using Magentix.Presentation.Services.Common;
using Magentix.Services;
using Magentix.Services.Common;
using Magentix.Services.Common.Device;
using Microsoft.Practices.Prism.Events;

namespace Magentix.Modules.SettingsModule
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class SettingsViewModel : VisibleViewModelBase
    {
        private readonly ISettingService _settingService;
        private readonly IMessagingService _messagingService;
        private readonly IDeviceService _deviceService;

        [ImportingConstructor]
        public SettingsViewModel(ISettingService settingService, IMessagingService messagingService, IDeviceService deviceService)
        {
            _settingService = settingService;
            _messagingService = messagingService;
            _deviceService = deviceService;
            SaveSettingsCommand = new CaptionCommand<string>(Resources.Save, OnSaveSettings);
            StartMessagingServerCommand = new CaptionCommand<string>(Resources.StartClientNow, OnStartMessagingServer, CanStartMessagingServer);
            DisplayCommonAppPathCommand = new CaptionCommand<string>(Resources.DisplayAppPath, OnDisplayAppPath);
            DisplayUserAppPathCommand = new CaptionCommand<string>(Resources.DisplayUserPath, OnDisplayUserPath);
            EditCallerIdDeviceSettingsCommand = new CaptionCommand<string>(Resources.Settings, OnEditCallerIdDeviceSettings);

            AddDeviceCommand = new CaptionCommand<string>(Resources.Add, new Action<string>(this.OnAddDevice));
            RemoveDeviceCommand = new CaptionCommand<string>(Resources.Remove, new Action<string>(this.OnRemoveDevice), new Func<string, bool>(this.CanRemoveAdditionalDevice));
            EditDeviceSettingsCommand = new CaptionCommand<string>(Resources.Settings, new Action<string>(this.OnEditDeviceSettings), new Func<string, bool>(this.CanEditDeviceSettings));
        }

        public void OnDisplayUserPath(string obj)
        {
            var prc = new System.Diagnostics.Process { StartInfo = { FileName = LocalSettings.UserPath } };
            prc.Start();
        }

        public void OnDisplayAppPath(string obj)
        {
            var prc = new System.Diagnostics.Process { StartInfo = { FileName = LocalSettings.DataPath } };
            prc.Start();
        }

        private bool CanStartMessagingServer(string arg)
        {
            return _messagingService.CanStartMessagingClient();
        }

        private void OnStartMessagingServer(string obj)
        {
            _messagingService.StartMessagingClient();
        }

        private void OnSaveSettings(string obj)
        {
            this._deviceService.SaveDevices(this.Devices);
            LocalSettings.SaveSettings();
            this._deviceService.InitializeDevices();
            //Application.Current.MainWindow.FontWeight = (this.UseBoldFonts ? FontWeights.Bold : FontWeights.Normal);
            EventServiceFactory.EventService.PublishEvent<EventAggregator>(EventTopicNames.LocalSettingsChanged);
            this.PublishEvent<VisibleViewModelBase>(EventTopicNames.ViewClosed);

            //LocalSettings.SaveSettings();
            //_deviceService.InitializeDevices();
            //if (!string.IsNullOrEmpty(CallerIdDeviceName))
            //{
            //    var device = _deviceService.GetDeviceByName(CallerIdDeviceName);
            //    if (device != null)
            //    {
            //        device.SaveSettings();
            //        _deviceService.InitializeDevice(CallerIdDeviceName);
            //    }
            //}
            //EventServiceFactory.EventService.PublishEvent(EventTopicNames.LocalSettingsChanged);
            //((VisibleViewModelBase)this).PublishEvent(EventTopicNames.ViewClosed);
        }

        public ICaptionCommand SaveSettingsCommand { get; set; }
        public ICaptionCommand StartMessagingServerCommand { get; set; }
        public ICaptionCommand DisplayCommonAppPathCommand { get; set; }
        public ICaptionCommand DisplayUserAppPathCommand { get; set; }
        public ICaptionCommand EditCallerIdDeviceSettingsCommand { get; set; }

        public string TerminalName
        {
            get { return LocalSettings.TerminalName; }
            set { LocalSettings.TerminalName = value; }
        }

        public string ConnectionString
        {
            get { return LocalSettings.ConnectionString; }
            set { LocalSettings.ConnectionString = value; }
        }

        public string MessagingServerName
        {
            get { return LocalSettings.MessagingServerName; }
            set { LocalSettings.MessagingServerName = value; }
        }

        public int MessagingServerPort
        {
            get { return LocalSettings.MessagingServerPort; }
            set { LocalSettings.MessagingServerPort = value; }
        }

        public bool StartMessagingClient
        {
            get { return LocalSettings.StartMessagingClient; }
            set { LocalSettings.StartMessagingClient = value; }
        }

        public string CallerIdDeviceName
        {
            get { return LocalSettings.CallerIdDeviceName; }
            set { LocalSettings.CallerIdDeviceName = value; /*RaisePropertyChanged(() => CanEditDeviceSettings);*/ }
        }

        public string WindowScale
        {
            get { return LocalSettings.WindowScale.Equals(0) ? "100" : (LocalSettings.WindowScale * 100).ToString(CultureInfo.CurrentCulture); }
            set { LocalSettings.WindowScale = Convert.ToDouble(value) / 100; }
        }

        public string Language
        {
            get { return LocalSettings.CurrentLanguage; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    LocalSettings.CurrentLanguage = "";
                }
                else if (LocalSettings.SupportedLanguages.Contains(value))
                {
                    LocalSettings.CurrentLanguage = value;
                }
                else
                {
                    var ci = CultureInfo.GetCultureInfo(value);
                    if (LocalSettings.SupportedLanguages.Contains(ci.TwoLetterISOLanguageName))
                    {
                        LocalSettings.CurrentLanguage = ci.TwoLetterISOLanguageName;
                    }
                }
            }
        }

        public bool OverrideWindowsRegionalSettings
        {
            get { return LocalSettings.OverrideWindowsRegionalSettings; }
            set
            {
                LocalSettings.OverrideWindowsRegionalSettings = value;
                RaisePropertyChanged(() => OverrideWindowsRegionalSettings);
            }
        }

        private IEnumerable<string> _terminalNames;
        public IEnumerable<string> TerminalNames
        {
            get { return _terminalNames ?? (_terminalNames = _settingService.GetTerminals().Select(x => x.Name)); }
        }

        private IEnumerable<CultureInfo> _supportedLanguages;
        public IEnumerable<CultureInfo> SupportedLanguages
        {
            get
            {
                return _supportedLanguages ?? (_supportedLanguages =
                    LocalSettings.SupportedLanguages.Select(CultureInfo.GetCultureInfo).ToList().OrderBy(x => x.NativeName));
            }
        }

        public IEnumerable<string> CallerIdDeviceNames { get { return _deviceService.GetDeviceNames(); } }
        
        public void OnEditCallerIdDeviceSettings(string arg)
        {
            var device = _deviceService.GetDeviceByName(CallerIdDeviceName);
            if (device != null)
            {
                InteractionService.UserIntraction.EditProperties(device.GetSettingsObject());
            }
        }

        protected override string GetHeaderInfo()
        {
            return Resources.ProgramSettings;
        }

        public override Type GetViewType()
        {
            return typeof(SettingsView);
        }


        private List<DeviceInstallation> _devices;
        private DeviceInstallation _selectedDevice;
        public List<DeviceInstallation> Devices
        {
            get
            {
                List<DeviceInstallation> deviceInstallations = this._devices;
                if (deviceInstallations == null)
                {
                    List<DeviceInstallation> devices = this.GetDevices();
                    List<DeviceInstallation> deviceInstallations1 = devices;
                    this._devices = devices;
                    deviceInstallations = deviceInstallations1;
                }
                return deviceInstallations;
            }
        }
        public DeviceInstallation SelectedDevice
        {
            get
            {
                return this._selectedDevice;
            }
            set
            {
                this._selectedDevice = value;
                base.RaisePropertyChanged<DeviceInstallation>(System.Linq.Expressions.Expression.Lambda<Func<DeviceInstallation>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(SettingsViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(SettingsViewModel).GetMethod("get_SelectedDevice").MethodHandle)), new ParameterExpression[0]));
            }
        }
        public ICaptionCommand AddDeviceCommand
        {
            get;
            set;
        }
        public ICaptionCommand RemoveDeviceCommand
        {
            get;
            set;
        }
        public ICaptionCommand EditDeviceSettingsCommand
        {
            get;
            set;
        }

        private void AddDevice(string deviceName)
        {
            this._deviceService.SaveDevices(this.Devices);
            DeviceInstallation deviceInstallation = this._deviceService.AddDevice(deviceName);
            this._devices = null;
            base.RaisePropertyChanged<List<DeviceInstallation>>(System.Linq.Expressions.Expression.Lambda<Func<List<DeviceInstallation>>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(SettingsViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(SettingsViewModel).GetMethod("get_Devices").MethodHandle)), new ParameterExpression[0]));
            this.SelectedDevice = deviceInstallation;
        }

        public void OnEditDeviceSettings(string arg)
        {
            this._deviceService.SaveDevices(this.Devices);
            object deviceSettings = this._deviceService.GetDeviceSettings(this.SelectedDevice.DeviceName, this.SelectedDevice.Key);
            if (deviceSettings != null)
            {
                InteractionService.UserIntraction.EditProperties(deviceSettings);
            }
        }
        private void RemoveDevice(DeviceInstallation installation)
        {
            this._deviceService.SaveDevices(this.Devices);
            this._deviceService.RemoveDevice(installation.DeviceName, installation.Key);
            this._devices = null;
            base.RaisePropertyChanged<List<DeviceInstallation>>(System.Linq.Expressions.Expression.Lambda<Func<List<DeviceInstallation>>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(SettingsViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(SettingsViewModel).GetMethod("get_Devices").MethodHandle)), new ParameterExpression[0]));
        }
        private List<DeviceInstallation> GetDevices()
        {
            return this._deviceService.GetInstalledDevices();
        }

        private void OnAddDevice(string obj)
        {
            this.AddDevice("");
        }
        private void OnRemoveDevice(string obj)
        {
            this.RemoveDevice(this.SelectedDevice);
        }
        private bool CanRemoveAdditionalDevice(string arg)
        {
            return this.SelectedDevice != null;
        }
        private bool CanEditDeviceSettings(string arg)
        {
            return this.SelectedDevice != null;
        }
    }
}
