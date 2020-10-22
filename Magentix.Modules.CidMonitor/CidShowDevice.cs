using Axcidv5callerid;
using Magentix.Localization.Properties;
using Magentix.Presentation.Common.Services;
using Magentix.Presentation.Services;
using Magentix.Services;
using Magentix.Services.Common.Device;
using System;
using System.ComponentModel;

namespace Magentix.Modules.CidMonitor
{
    internal class CidShowDevice : AbstractCidDevice
    {
        private FrmMain _frmMain;

        private AbstractCidSettings _settings;

        public AbstractCidSettings Settings
        {
            get
            {
                AbstractCidSettings abstractCidSetting = this._settings;
                if (abstractCidSetting == null)
                {
                    AbstractCidSettings abstractCidSetting1 = base.LoadSettings<AbstractCidSettings>();
                    AbstractCidSettings abstractCidSetting2 = abstractCidSetting1;
                    this._settings = abstractCidSetting1;
                    abstractCidSetting = abstractCidSetting2;
                }
                return abstractCidSetting;
            }
        }

        public CidShowDevice(string key, string name, IApplicationState applicationState, ICacheService cacheService, IEntityService entityService, IApplicationStateSetter applicationStateSetter, IPrinterService printerService) : base(key, name, cacheService, applicationState, entityService, applicationStateSetter, printerService)
        {
        }

        private void axCIDv51_OnCallerID(object sender, ICIDv5Events_OnCallerIDEvent e)
        {
            try
            {
                base.ProcessPhoneNumber(e.phoneNumber);
            }
            catch (Exception)
            {
                base.ProcessPhoneNumber("");
            }
        }

        protected override void DoFinalize()
        {
            this._frmMain.axCIDv51.OnCallerID -= new ICIDv5Events_OnCallerIDEventHandler(this.axCIDv51_OnCallerID);
            this._frmMain.axCIDv51.Dispose();
            this._frmMain.Dispose();
            this._frmMain = null;
        }

        protected override bool DoInitialize()
        {
            bool flag;
            try
            {
                this._frmMain = new FrmMain();
                this._frmMain.axCIDv51.OnCallerID += new ICIDv5Events_OnCallerIDEventHandler(this.axCIDv51_OnCallerID);
                this._frmMain.axCIDv51.Start();
                flag = true;
            }
            catch (Exception)
            {
                InteractionService.UserIntraction.DisplayPopup("", Resources.Information, Resources.CallerIdDriverError, "DarkRed", null, null);
                flag = false;
            }
            return flag;
        }

        protected override AbstractSettings GetSettings()
        {
            return this.Settings;
        }
    }
}
