using Magentix.Presentation.Services;
using Magentix.Services;
using Magentix.Services.Common.Device;
using System;
using System.ComponentModel.Composition;
using System.Runtime.CompilerServices;

namespace Magentix.Modules.CidMonitor
{
    [Export(typeof(IDeviceFactory))]
    internal class AdvancedModemDeviceFactory : AbstractDeviceFactory
    {
        private readonly IApplicationState _applicationState;

        private readonly ICacheService _cacheService;

        private readonly IEntityService _entityService;

        private readonly IApplicationStateSetter _applicationStateSetter;

        private readonly IPrinterService _printerService;

        [ImportingConstructor]
        public AdvancedModemDeviceFactory(IApplicationState applicationState, ICacheService cacheService, IEntityService entityService, IApplicationStateSetter applicationStateSetter, IPrinterService printerService)
        {
            this._applicationState = applicationState;
            this._cacheService = cacheService;
            this._entityService = entityService;
            this._applicationStateSetter = applicationStateSetter;
            this._printerService = printerService;
        }

        public override Func<string, IDevice> GetGenerateDeviceFunction()
        {
            return (string x) => new AdvancedModemDevice(x, this.GetName(), this._applicationState, this._cacheService, this._entityService, this._applicationStateSetter, this._printerService);
        }

        protected override string GetName()
        {
            return "Advanced Generic Modem";
        }
    }
}
