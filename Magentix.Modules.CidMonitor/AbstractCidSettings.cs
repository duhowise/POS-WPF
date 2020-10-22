using PropertyTools.DataAnnotations;
using Magentix.Services.Common.Device;
using System;
using System.Runtime.CompilerServices;

namespace Magentix.Modules.CidMonitor
{
    public class AbstractCidSettings : AbstractSettings
    {
        public string DepartmentName
        {
            get;
            set;
        }

        [Height(80, double.NaN, double.NaN)]
        [WideProperty]
        public string DetailFormat
        {
            get;
            set;
        }

        public string EntityScreenName
        {
            get;
            set;
        }

        public string EntityTypeName
        {
            get;
            set;
        }

        public string PopupColor
        {
            get;
            set;
        }

        public string PopupName
        {
            get;
            set;
        }

        public string SearchFormat
        {
            get;
            set;
        }

        public string TrimChars
        {
            get;
            set;
        }

        public string WidgetName
        {
            get;
            set;
        }

        public AbstractCidSettings()
        {
            this.TrimChars = "+90";
            this.PopupName = "";
        }
    }
}