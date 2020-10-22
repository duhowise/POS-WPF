using PropertyTools.DataAnnotations;
using Magentix.Presentation.Common.ModelBase;
using System;
using System.ComponentModel;
using System.IO.Ports;
using System.Runtime.CompilerServices;


namespace Magentix.Modules.CidMonitor
{
    public class GenericModemSettings : AbstractCidSettings
    {
        private NameWithValue _portNameValue;

        public int BaudRate
        {
            get;
            set;
        }

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

        public string TerminateString
        {
            get;
            set;
        }

        public GenericModemSettings()
        {
            this.PortNameValue.UpdateValues(SerialPort.GetPortNames());
        }
    }
}