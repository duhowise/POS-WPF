﻿using System.ComponentModel.Composition;
using Magentix.Localization.Properties;
using Magentix.Presentation.Services;
using Magentix.Services.Common;

namespace Magentix.Modules.SettingsModule.ActionProcessors
{
    [Export(typeof(IActionType))]
    class SetCurrentTerminal : ActionType
    {
        private readonly IApplicationStateSetter _applicationStateSetter;

        [ImportingConstructor]
        public SetCurrentTerminal(IApplicationStateSetter applicationStateSetter)
        {
            _applicationStateSetter = applicationStateSetter;
        }

        public override void Process(ActionData actionData)
        {
            var terminalName = actionData.GetAsString("TerminalName");
            if (!string.IsNullOrEmpty(terminalName))
                _applicationStateSetter.SetCurrentTerminal(terminalName);
        }

        protected override object GetDefaultData()
        {
            return new { TerminalName = "" };
        }

        protected override string GetActionName()
        {
            return Resources.SetCurrentTerminal;
        }

        protected override string GetActionKey()
        {
            return "SetCurrentTerminal";
        }
    }
}
