using Magentix.Modules.BackupModule;
using Magentix.Services.Common;
using System;
using System.ComponentModel.Composition;

namespace Magentix.Modules.BackupModule.Actions
{
    [Export(typeof(IActionType))]
    internal class ExecuteDatabaseTask : ActionType
    {
        private readonly DatabaseTaskManager _databaseTaskManager;

        [ImportingConstructor]
        public ExecuteDatabaseTask(DatabaseTaskManager databaseTaskManager)
        {
            this._databaseTaskManager = databaseTaskManager;
        }

        protected override string GetActionKey()
        {
            return "ExecuteDatabaseTask";
        }

        protected override string GetActionName()
        {
            return "Execute Database Task";
        }

        protected override object GetDefaultData()
        {
            return new { TaskName = "", SkipConfirmation = false, Arguments = "" };
        }

        public override void Process(ActionData actionData)
        {
            string asString = actionData.GetAsString("TaskName");
            string str = actionData.GetAsString("Arguments");
            bool asBoolean = actionData.GetAsBoolean("SkipConfirmation", false);
            if (!string.IsNullOrEmpty(asString))
            {
                this._databaseTaskManager.ExecuteTask(new DatabaseTask(asString, str), asBoolean);
            }
        }
    }
}
