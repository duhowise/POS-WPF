using Magentix.Presentation.Common.Services;
using Magentix.Presentation.Services;
using Magentix.Services;
using Magentix.Services.Common.Device;
using System;
using System.ComponentModel;
using System.IO.Ports;
using System.Text.RegularExpressions;


namespace Magentix.Modules.CidMonitor
{
    internal class HuginCallerIdDevice : AbstractCidDevice
    {
        private SerialPort _port;

        private GenericModemSettings _settings;

        public GenericModemSettings Settings
        {
            get
            {
                GenericModemSettings genericModemSetting = this._settings;
                if (genericModemSetting == null)
                {
                    GenericModemSettings genericModemSetting1 = base.LoadSettings<GenericModemSettings>();
                    GenericModemSettings genericModemSetting2 = genericModemSetting1;
                    this._settings = genericModemSetting1;
                    genericModemSetting = genericModemSetting2;
                }
                return genericModemSetting;
            }
        }

        public HuginCallerIdDevice(string key, string name, IApplicationState applicationState, ICacheService cacheService, IEntityService entityService, IApplicationStateSetter applicationStateSetter, IPrinterService printerService) : base(key, name, cacheService, applicationState, entityService, applicationStateSetter, printerService)
        {
        }

        protected override void DoFinalize()
        {
            this._port.DataReceived -= new SerialDataReceivedEventHandler(this.port_DataReceived);
            try
            {
                this._port.Close();
            }
            finally
            {
                this._port.Dispose();
                this._port = null;
            }
        }

        protected override bool DoInitialize()
        {
            bool flag;
            try
            {
                this._port = new SerialPort(this.Settings.PortName)
                {
                    BaudRate = 38400,
                    RtsEnable = false,
                    DtrEnable = false
                };
                this._port.Open();
                this._port.DiscardOutBuffer();
                this._port.DiscardInBuffer();
                this._port.DataReceived += new SerialDataReceivedEventHandler(this.port_DataReceived);
                return true;
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                InteractionService.UserIntraction.DisplayPopup("", "Hugin Caller ID Error", exception.Message, "DarkRed", null, null);
                flag = false;
            }
            return flag;
        }

        protected override Magentix.Services.Common.Device.DeviceType GetDeviceType()
        {
            return Magentix.Services.Common.Device.DeviceType.CallerId;
        }

        private string GetMatchPattern()
        {
            if (string.IsNullOrEmpty(this.Settings.MatchPattern))
            {
                return "L.: .{10}([0-9]+)";
            }
            return this.Settings.MatchPattern;
        }

        protected override AbstractSettings GetSettings()
        {
            return this.Settings;
        }

        private string GetTerminateString()
        {
            if (string.IsNullOrEmpty(this.Settings.TerminateString))
            {
                return null;
            }
            return this.Settings.TerminateString;
        }

        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string str = (!string.IsNullOrEmpty(this.GetTerminateString()) ? this._port.ReadTo(this.GetTerminateString()) : this._port.ReadTo("\r"));
                string value = Regex.Match(str, this.GetMatchPattern()).Groups[1].Value;
                base.ProcessPhoneNumber(value);
            }
            catch (Exception)
            {
                base.ProcessPhoneNumber("");
            }
        }
    }
}
