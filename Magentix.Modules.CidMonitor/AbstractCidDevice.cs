using Magentix.Domain.Models.Entities;
using Magentix.Infrastructure.Data;
using Magentix.Localization.Properties;
using Magentix.Presentation.Common.Services;
using Magentix.Presentation.Common.Widgets;
using Magentix.Presentation.Services;
using Magentix.Presentation.Services.Common;
using Magentix.Services;
using Magentix.Services.Common;
using Magentix.Services.Common.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Threading;

namespace Magentix.Modules.CidMonitor
{
    internal abstract class AbstractCidDevice : AbstractDevice
    {
        private readonly ICacheService _cacheService;

        private readonly IApplicationState _applicationState;

        private readonly IEntityService _entityService;

        private readonly IApplicationStateSetter _applicationStateSettter;

        private readonly IPrinterService _printerService;

        protected EntityType CustomerType
        {
            get;
            set;
        }

        public string DepartmentName
        {
            get
            {
                return ((AbstractCidSettings)this.GetSettings()).DepartmentName;
            }
        }

        public string DetailFormat
        {
            get
            {
                return ((AbstractCidSettings)this.GetSettings()).DetailFormat;
            }
        }

        public string EntityScreenName
        {
            get
            {
                return ((AbstractCidSettings)this.GetSettings()).EntityScreenName;
            }
        }

        public string EntityTypeName
        {
            get
            {
                return ((AbstractCidSettings)this.GetSettings()).EntityTypeName;
            }
        }

        public string PopupColor
        {
            get
            {
                return ((AbstractCidSettings)this.GetSettings()).PopupColor;
            }
        }

        public string PopupName
        {
            get
            {
                return ((AbstractCidSettings)this.GetSettings()).PopupName;
            }
        }

        public string SearchFormat
        {
            get
            {
                return ((AbstractCidSettings)this.GetSettings()).SearchFormat;
            }
        }

        public string TrimChars
        {
            get
            {
                return ((AbstractCidSettings)this.GetSettings()).TrimChars;
            }
        }

        public string WidgetName
        {
            get
            {
                return ((AbstractCidSettings)this.GetSettings()).WidgetName;
            }
        }

        protected AbstractCidDevice(string key, string name, ICacheService cacheService, IApplicationState applicationState, IEntityService entityService, IApplicationStateSetter applicationStateSettter, IPrinterService printerService) : base(name, key)
        {
            this._cacheService = cacheService;
            this._applicationState = applicationState;
            this._entityService = entityService;
            this._applicationStateSettter = applicationStateSettter;
            this._printerService = printerService;
        }

        protected override Magentix.Services.Common.Device.DeviceType GetDeviceType()
        {
            return Magentix.Services.Common.Device.DeviceType.CallerId;
        }

        private void OnClick(object phoneNumber)
        {
            string str = phoneNumber.ToString();
            if (string.IsNullOrEmpty(this.CustomerType.PrimaryFieldName) && string.IsNullOrEmpty(this.SearchFormat))
            {
                if (this.CustomerType.EntityCustomFields.Any<EntityCustomField>((EntityCustomField x) => x.Name == Resources.Phone))
                {
                    str = string.Concat(Resources.Phone, ":", str);
                }
            }
            if (!string.IsNullOrEmpty(this.SearchFormat))
            {
                str = string.Format(this.SearchFormat, str);
            }
            if (!string.IsNullOrEmpty(this.DepartmentName))
            {
                this._applicationStateSettter.SetCurrentDepartment(this.DepartmentName);
            }
            if (!string.IsNullOrEmpty(this.EntityScreenName))
            {
                this._applicationStateSettter.SetSelectedEntityScreen(this.EntityScreenName);
            }
            if (string.IsNullOrEmpty(this.WidgetName))
            {
                OperationRequest<Entity>.Publish(Entity.GetNullEntity(this.CustomerType.Id), EventTopicNames.SelectEntity, EventTopicNames.EntitySelected, str, false);
                return;
            }
            WidgetEventData widgetEventDatum = new WidgetEventData()
            {
                WidgetName = this.WidgetName,
                Value = str
            };
            widgetEventDatum.PublishEvent<WidgetEventData>("SetWidgetValue");
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            string str = (!string.IsNullOrEmpty(this.EntityTypeName) ? this.EntityTypeName : "Customers");
            this.CustomerType = this._cacheService.GetEntityTypes().SingleOrDefault<EntityType>((EntityType x) => x.Name == str);
        }

        private void Process(string phoneNumber)
        {
            if (this.CustomerType != null)
            {
                string popupName = this.PopupName;
                string popupColor = this.PopupColor;
                if (string.IsNullOrEmpty(popupColor))
                {
                    popupColor = "DarkRed";
                }
                if (string.IsNullOrWhiteSpace(popupName))
                {
                    popupName = base.Name;
                }
                List<Entity> entities = this._entityService.SearchEntities(this.CustomerType, phoneNumber, "");
                if (entities.Count != 1)
                {
                    InteractionService.UserIntraction.DisplayPopup(popupName, phoneNumber, string.Concat(phoneNumber, " ", Resources.Calling, "..."), popupColor, new Action<object>(this.OnClick), phoneNumber);
                }
                else
                {
                    Entity entity = entities.First<Entity>();
                    string[] name = new string[] { entity.Name, " ", Resources.Calling, ".\r", entity.SearchString, "\r" };
                    string str = string.Concat(name);
                    if (!string.IsNullOrEmpty(this.DetailFormat))
                    {
                        str = this._printerService.ExecuteFunctions<Entity>(this.DetailFormat, entity);
                    }
                    InteractionService.UserIntraction.DisplayPopup(popupName, this.CustomerType.GetFormattedDisplayName(entity.Name, entity), str, popupColor, new Action<object>(this.OnClick), phoneNumber);
                }
            }
            this._applicationState.NotifyEvent(RuleEventNames.DeviceEventGenerated, new { DeviceName = base.Name, EventName = "CID_Event", EventData = phoneNumber });
        }

        protected void ProcessPhoneNumber(string phoneNumber)
        {
            string str = phoneNumber;
            if (string.IsNullOrWhiteSpace(str))
            {
                return;
            }
            str = str.Trim();
            if (!string.IsNullOrEmpty(this.TrimChars))
            {
                this.TrimChars.ToList<char>().ForEach((char x) => str = str.TrimStart(new char[] { x }));
            }
            if (string.IsNullOrWhiteSpace(str))
            {
                return;
            }
            Thread thread = new Thread(() => this._applicationState.MainDispatcher.Invoke(() => this.Process(str)));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }
    }
}