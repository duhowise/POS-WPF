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
    internal class GenericModemDevice : AbstractCidDevice
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

        public GenericModemDevice(string key, string name, IApplicationState applicationState, ICacheService cacheService, IEntityService entityService, IApplicationStateSetter applicationStateSetter, IPrinterService printerService) : base(key, name, cacheService, applicationState, entityService, applicationStateSetter, printerService)
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
                this._port = new SerialPort(this.Settings.PortName);
                if (this.Settings.BaudRate > 0)
                {
                    this._port.BaudRate = this.Settings.BaudRate;
                }
                this._port.Open();
                this._port.DiscardOutBuffer();
                this._port.DiscardInBuffer();
                this._port.RtsEnable = true;
                string initializationString = this.GetInitializationString();
                char[] chrArray = new char[] { '\r', '\n' };
                string[] strArrays = initializationString.Split(chrArray, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < (int)strArrays.Length; i++)
                {
                    string str = strArrays[i];
                    this._port.WriteLine(string.Concat(str, "\r"));
                }
                this._port.DataReceived += new SerialDataReceivedEventHandler(this.port_DataReceived);
                return true;
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                InteractionService.UserIntraction.DisplayPopup("", "Generic Modem Error", exception.Message, "DarkRed", null, null);
                flag = false;
            }
            return flag;
        }

        protected override Magentix.Services.Common.Device.DeviceType GetDeviceType()
        {
            return Magentix.Services.Common.Device.DeviceType.CallerId;
        }

        private string GetInitializationString()
        {
            if (string.IsNullOrWhiteSpace(this.Settings.InitializationString))
            {
                return "AT+VCID=0\r\nAT+VCID=1";
            }
            return this.Settings.InitializationString;
        }

        private string GetMatchPattern()
        {
            if (string.IsNullOrWhiteSpace(this.Settings.MatchPattern))
            {
                return "NMBR.*=\\s?([0-9-\\s]+)";
            }
            return this.Settings.MatchPattern;
        }

        protected override AbstractSettings GetSettings()
        {
            return this.Settings;
        }

        private string GetTerminateString()
        {
            if (string.IsNullOrWhiteSpace(this.Settings.TerminateString))
            {
                return "";
            }
            return this.Settings.TerminateString;
        }

        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string str;
            try
            {
                string terminateString = this.GetTerminateString();
                if (!string.IsNullOrEmpty(terminateString))
                {
                    str = (terminateString == "new line" ? this._port.ReadLine() : this._port.ReadTo(terminateString));
                }
                else
                {
                    str = this._port.ReadExisting();
                }
                string str1 = str;
                string value = Regex.Match(str1, this.GetMatchPattern()).Groups[1].Value;
                base.ProcessPhoneNumber(value);
            }
            catch (Exception)
            {
                base.ProcessPhoneNumber("");
            }
        }
    }
}
