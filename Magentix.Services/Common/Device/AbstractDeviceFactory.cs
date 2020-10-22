using System;
using System.Collections.Generic;

namespace Magentix.Services.Common.Device
{
    public abstract class AbstractDeviceFactory : IDeviceFactory
    {
        private IDictionary<string, IDevice> _cache = new Dictionary<string, IDevice>();

        public string Name
        {
            get
            {
                return this.GetName();
            }
        }

        protected AbstractDeviceFactory()
        {
        }

        public IDevice GenerateDevice(string key)
        {
            if (!this._cache.ContainsKey(key))
            {
                this._cache.Add(key, this.GetGenerateDeviceFunction()(key));
            }
            return this._cache[key];
        }

        public abstract Func<string, IDevice> GetGenerateDeviceFunction();

        protected abstract string GetName();
    }
}