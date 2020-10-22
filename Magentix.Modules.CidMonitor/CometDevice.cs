using Magentix.Presentation.Common.Services;
using Magentix.Presentation.Services;
using Magentix.Services;
using Magentix.Services.Common.Device;
using System;
using System.ComponentModel;
using System.IO.Ports;

namespace Magentix.Modules.CidMonitor
{
    internal class CometDevice : AbstractCidDevice
    {
        private SerialPort _port;

        private GenericModemSettings _settings;

        private int[] _buffer = new int[255];

        private int _pointer;

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

        public CometDevice(string key, string name, IApplicationState applicationState, ICacheService cacheService, IEntityService entityService, IApplicationStateSetter applicationStateSetter, IPrinterService printerService) : base(key, name, cacheService, applicationState, entityService, applicationStateSetter, printerService)
        {
        }

        protected override void DoFinalize()
        {
            this._port.DataReceived -= new SerialDataReceivedEventHandler(this.serialPort_DataReceived);
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
                this._port = new SerialPort()
                {
                    PortName = this.Settings.PortName,
                    BaudRate = 1200,
                    DataBits = 8,
                    StopBits = StopBits.One,
                    Parity = Parity.None
                };
                this._port.DataReceived += new SerialDataReceivedEventHandler(this.serialPort_DataReceived);
                this._port.ReadTimeout = 100;
                this._port.Open();
                return true;
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                InteractionService.UserIntraction.DisplayPopup("", "Comet CID Error", exception.Message, "DarkRed", null, null);
                flag = false;
            }
            return flag;
        }

        protected override Magentix.Services.Common.Device.DeviceType GetDeviceType()
        {
            return Magentix.Services.Common.Device.DeviceType.CallerId;
        }

        protected override AbstractSettings GetSettings()
        {
            return this.Settings;
        }

        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                for (int i = 0; i != this._port.BytesToRead; i++)
                {
                    this._buffer[this._pointer] = this._port.ReadByte();
                    this._pointer++;
                }
            }
            catch (TimeoutException)
            {
                base.ProcessPhoneNumber((new CometData(this._buffer)).getCIDNumber());
                this._buffer = new int[255];
                this._pointer = 0;
            }
            catch
            {
                base.ProcessPhoneNumber("");
            }
        }
    }
}
