using Magentix.Presentation.Common.Services;
using Magentix.Presentation.Services;
using Magentix.Services;
using Magentix.Services.Common.Device;
using System;
using System.ComponentModel;
using System.IO.Ports;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace Magentix.Modules.CidMonitor
{
    internal class AdvancedModemDevice : AbstractCidDevice
    {
        private StringBuilder _buffer = new StringBuilder();

        private SerialPort _port;

        private AdvancedModemSettings _settings;

        public AdvancedModemSettings Settings
        {
            get
            {
                AdvancedModemSettings advancedModemSetting = this._settings;
                if (advancedModemSetting == null)
                {
                    AdvancedModemSettings advancedModemSetting1 = base.LoadSettings<AdvancedModemSettings>();
                    AdvancedModemSettings advancedModemSetting2 = advancedModemSetting1;
                    this._settings = advancedModemSetting1;
                    advancedModemSetting = advancedModemSetting2;
                }
                return advancedModemSetting;
            }
        }

        public AdvancedModemDevice(string key, string name, IApplicationState applicationState, ICacheService cacheService, IEntityService entityService, IApplicationStateSetter applicationStateSetter, IPrinterService printerService) : base(key, name, cacheService, applicationState, entityService, applicationStateSetter, printerService)
        {
        }

        protected override void DoFinalize()
        {
            WeakEventManager<SerialPort, SerialDataReceivedEventArgs>.RemoveHandler(this._port, "DataReceived", new EventHandler<SerialDataReceivedEventArgs>(this.port_DataReceivedWithTimeout));
            WeakEventManager<SerialPort, SerialDataReceivedEventArgs>.RemoveHandler(this._port, "DataReceived", new EventHandler<SerialDataReceivedEventArgs>(this.port_DataReceived));
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
                if (this.Settings.DataBits > 0)
                {
                    this._port.DataBits = this.Settings.DataBits;
                }
                this._port.StopBits = this.Settings.StopBits;
                this._port.Parity = this.Settings.Parity;
                if (this.Settings.ReadTimeout > 0)
                {
                    this._port.ReadTimeout = this.Settings.ReadTimeout;
                }
                this._port.Open();
                if (this.Settings.DiscardBuffers)
                {
                    this._port.DiscardOutBuffer();
                    this._port.DiscardInBuffer();
                }
                this._port.RtsEnable = this.Settings.EnableRts;
                this._port.DtrEnable = this.Settings.EnableDtr;
                string initializationString = this.GetInitializationString();
                char[] chrArray = new char[] { '\r', '\n' };
                string[] strArrays = initializationString.Split(chrArray, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < (int)strArrays.Length; i++)
                {
                    string str = strArrays[i];
                    this._port.WriteLine(string.Concat(str, "\r"));
                }
                if (this.Settings.ReadTimeout <= 0)
                {
                    WeakEventManager<SerialPort, SerialDataReceivedEventArgs>.AddHandler(this._port, "DataReceived", new EventHandler<SerialDataReceivedEventArgs>(this.port_DataReceived));
                }
                else
                {
                    WeakEventManager<SerialPort, SerialDataReceivedEventArgs>.AddHandler(this._port, "DataReceived", new EventHandler<SerialDataReceivedEventArgs>(this.port_DataReceivedWithTimeout));
                }
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

        private void port_DataReceivedWithTimeout(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                this._buffer.Append(this._port.ReadExisting());
            }
            catch (TimeoutException)
            {
                base.ProcessPhoneNumber(this._buffer.ToString());
                this._buffer = new StringBuilder();
            }
            catch
            {
                base.ProcessPhoneNumber("");
            }
        }
    }
}
