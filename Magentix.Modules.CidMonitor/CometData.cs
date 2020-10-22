using System;

namespace Magentix.Modules.CidMonitor
{
    internal class CometData
    {
        private const byte CALLER_ID = 128;

        private const byte TIMEDATE = 1;

        private const byte CALLING_NUMBER = 2;

        private const byte WHY_NO_NUMBER = 4;

        private const byte CALLING_NAME = 7;

        private const byte CALL_TYPE = 17;

        private string CIDNumber;

        private string CIDName;

        private int i;

        private int bytes_remaining;

        public CometData(int[] data)
        {
            if (data[0] == 128)
            {
                this.bytes_remaining = data[1];
                this.i = 2;
                while (this.bytes_remaining > 0)
                {
                    int num = 0;
                    int num1 = data[this.i];
                    switch (num1)
                    {
                        case 1:
                            {
                                this.bytes_remaining--;
                                this.i++;
                                num = data[this.i];
                                this.bytes_remaining--;
                                this.i++;
                                this.bytes_remaining -= num;
                                this.i += num;
                                continue;
                            }
                        case 2:
                            {
                                this.bytes_remaining--;
                                this.i++;
                                num = data[this.i];
                                this.bytes_remaining--;
                                this.i++;
                                string str = "";
                                while (num > 0)
                                {
                                    str = string.Concat(str, (char)data[this.i]);
                                    this.bytes_remaining--;
                                    this.i++;
                                    num--;
                                }
                                this.CIDNumber = str;
                                continue;
                            }
                        case 3:
                        case 5:
                        case 6:
                            {
                                CometData cometDatum = this;
                                cometDatum.bytes_remaining--;
                                this.i++;
                                num = data[this.i];
                                this.bytes_remaining--;
                                this.i++;
                                this.bytes_remaining -= num;
                                this.i += num;
                                continue;
                            }
                        case 4:
                            {
                                this.bytes_remaining--;
                                this.i++;
                                num = data[this.i];
                                this.bytes_remaining--;
                                this.i++;
                                this.bytes_remaining -= num;
                                this.i += num;
                                continue;
                            }
                        case 7:
                            {
                                this.bytes_remaining--;
                                this.i++;
                                num = data[this.i];
                                this.bytes_remaining--;
                                this.i++;
                                string str1 = "";
                                while (num > 0)
                                {
                                    str1 = string.Concat(str1, (char)data[this.i]);
                                    this.bytes_remaining--;
                                    this.i++;
                                    num--;
                                }
                                this.CIDName = str1;
                                continue;
                            }
                        default:
                            {
                                if (num1 == 17)
                                {
                                    this.bytes_remaining--;
                                    this.i++;
                                    num = data[this.i];
                                    this.bytes_remaining--;
                                    this.i++;
                                    this.bytes_remaining -= num;
                                    this.i += num;
                                    continue;
                                }
                                else
                                {
                                    goto case 6;
                                }
                            }
                    }
                }
            }
        }

        public string getCIDName()
        {
            return this.CIDName;
        }

        public string getCIDNumber()
        {
            return this.CIDNumber;
        }
    }
}
