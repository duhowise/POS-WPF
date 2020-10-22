using Magentix.Domain.Models.Automation;
using System;
using System.Runtime.CompilerServices;

namespace Magentix.Presentation.Common
{
    public interface ICommandExecutionService
    {
        bool ConfirmAutomationCommand(AutomationCommand automationCommand);

        bool ConfirmAutomationCommand(string commandName);

        void ExecuteAutomationCommand(string commandName, string commandValue, dynamic dataObject);
    }
}
