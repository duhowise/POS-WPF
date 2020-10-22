using System;
using System.ComponentModel.Composition;
using System.Runtime.CompilerServices;
using Magentix.Infrastructure.Settings;
using Magentix.Persistance.Data;
using Magentix.Presentation.Services;
using Magentix.Presentation.Services.Common;
using Magentix.Services.Common;
using Microsoft.Practices.Prism.Events;

namespace Magentix.Modules.BackupModule.Actions
{
    [Export(typeof(IActionType))]
    public class ChangeDatabaseConnection : ActionType
    {
        private readonly IApplicationState _applicationState;

        private readonly IMethodQueue _methodQueue;

        [ImportingConstructor]
        public ChangeDatabaseConnection(IApplicationState applicationState, IMethodQueue methodQueue)
        {
            this._applicationState = applicationState;
            this._methodQueue = methodQueue;
        }

        protected override string GetActionKey()
        {
            return "ChangeDatabaseConnection";
        }

        protected override string GetActionName()
        {
            return "Change Database Connection";
        }

        protected override object GetDefaultData()
        {
            return new { ConnectionString = "", SaveToLocalSettings = false };
        }

        public override void Process(ActionData actionData)
        {
            if (this._applicationState.IsLocked)
            {
                this._methodQueue.Queue("ChangeDatabaseConnection", () => this.Process(actionData));
                return;
            }
            string asString = actionData.GetAsString("ConnectionString");
            WorkspaceFactory.UpdateConnection(asString);
            EventServiceFactory.EventService.PublishEvent<EventAggregator>("Reset Cache", true);
            if (actionData.GetAsBoolean("SaveToLocalSettings", false))
            {
                LocalSettings.ConnectionString = asString;
                LocalSettings.SaveSettings();
            }
        }
    }
}
