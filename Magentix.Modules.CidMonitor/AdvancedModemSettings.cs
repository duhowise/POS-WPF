using PropertyTools.DataAnnotations;
using Magentix.Presentation.Common.ModelBase;
using System;
using System.ComponentModel;
using System.IO.Ports;
using System.Runtime.CompilerServices;

namespace Magentix.Modules.CidMonitor
{
    public class AdvancedModemSettings : AbstractCidSettings
    {
        private NameWithValue _portNameValue;

        public int BaudRate
        {
            get;
            set;
        }

        public int DataBits
        {
            get;
            set;
        }

        public bool DiscardBuffers
        {
            get;
            set;
        }

        public bool EnableDtr
        {
            get;
            set;
        }

        public bool EnableRts
        {
            get;
            set;
        }

        [Category("Data Settings")]
        [Height(80, double.NaN, double.NaN)]
        [WideProperty]
        public string InitializationString
        {
            get;
            set;
        }

        public string MatchPattern
        {
            get;
            set;
        }

        public System.IO.Ports.Parity Parity
        {
            get;
            set;
        }

        [Browsable(false)]
        public string PortName
        {
            get
            {
                return this.PortNameValue.Text;
            }
            set
            {
                this.PortNameValue.Text = value;
            }
        }

        [Category("Port Settings")]
        [DisplayName("Port")]
        public NameWithValue PortNameValue
        {
            get
            {
                NameWithValue nameWithValue = this._portNameValue;
                if (nameWithValue == null)
                {
                    NameWithValue nameWithValue1 = new NameWithValue();
                    NameWithValue nameWithValue2 = nameWithValue1;
                    this._portNameValue = nameWithValue1;
                    nameWithValue = nameWithValue2;
                }
                return nameWithValue;
            }
        }

        public int ReadTimeout
        {
            get;
            set;
        }

        public System.IO.Ports.StopBits StopBits
        {
            get;
            set;
        }

        public string TerminateString
        {
            get;
            set;
        }

        public AdvancedModemSettings()
        {
            this.DiscardBuffers = true;
            this.EnableRts = true;
            this.DataBits = 8;
            this.StopBits = System.IO.Ports.StopBits.One;
            this.Parity = System.IO.Ports.Parity.None;
            this.BaudRate = 9600;
            this.PortNameValue.UpdateValues(SerialPort.GetPortNames());
        }
    }
}
