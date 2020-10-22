using Magentix.QLicense;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Magentix.License
{
    public class MagentixLicense : QLicense.LicenseEntity
    {
        [DisplayName("Trial Version")]
        [Category("License Options")]        
        [XmlElement("TrialVersion")]
        [ShowInLicenseInfo(true, "Trial Version", ShowInLicenseInfoAttribute.FormatType.String)]
        public bool TrialVersion { get; set; }

        [DisplayName("Trial Date")]
        [Category("License Options")]
        [XmlElement("TrialDate")]
        [ShowInLicenseInfo(true, "Trial Version", ShowInLicenseInfoAttribute.FormatType.String)]
        public DateTime TrialDate { get; set; }


        public MagentixLicense()
        {
            //Initialize app name for the license
            this.AppName = "MagentixPOS";
            TrialVersion = true;
            TrialDate = DateTime.Now;
        }

        public override LicenseStatus DoExtraValidation(out string validationMsg)
        {
            LicenseStatus _licStatus = LicenseStatus.UNDEFINED;
            validationMsg = string.Empty;

            switch (this.Type)
            {
                case LicenseTypes.Single:
                    //For Single License, check whether UID is matched
                    if (this.UID == LicenseHandler.GenerateUID(this.AppName))
                    {
                        _licStatus = LicenseStatus.VALID;
                    }
                    else
                    {
                        validationMsg = "The license is NOT for this copy!";
                        _licStatus = LicenseStatus.INVALID;                    
                    }
                    break;
                case LicenseTypes.Volume:
                    //No UID checking for Volume License
                    _licStatus = LicenseStatus.VALID;
                    break;
                default:
                    validationMsg = "Invalid license";
                    _licStatus= LicenseStatus.INVALID;
                    break;
            }

            return _licStatus;
        }
    }
}
