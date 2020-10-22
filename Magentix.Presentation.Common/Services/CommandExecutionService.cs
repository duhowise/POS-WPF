using Microsoft.CSharp.RuntimeBinder;
using Magentix.Domain.Models.Automation;
using Magentix.Domain.Models.Users;
using Magentix.Infrastructure.Data;
using Magentix.Presentation.Common;
using Magentix.Presentation.Services;
using Magentix.Services;
using System;
using System.ComponentModel.Composition;
using System.Runtime.CompilerServices;

namespace Magentix.Presentation.Common.Services
{
    [Export(typeof(ICommandExecutionService))]
    internal class CommandExecutionService : ICommandExecutionService
    {
        private readonly IDialogService _dialogService;

        private readonly IUserService _userService;

        private readonly IApplicationState _applicationState;

        private readonly ICacheService _cacheService;

        [ImportingConstructor]
        public CommandExecutionService(IDialogService dialogService, IUserService userService, IApplicationState applicationState, ICacheService cacheService)
        {
            this._dialogService = dialogService;
            this._userService = userService;
            this._applicationState = applicationState;
            this._cacheService = cacheService;
        }

        public bool ConfirmAutomationCommand(AutomationCommand automationCommand)
        {
            if (automationCommand == null)
            {
                return false;
            }
            if (this._applicationState.CurrentLoggedInUser.UserRole.IsAdmin || automationCommand.ConfirmationType != 2)
            {
                if (automationCommand.ConfirmationType != 1)
                {
                    return true;
                }
                return this._dialogService.Confirm(string.Format("Do you Confirm {0} Operation?", automationCommand.Name));
            }
            string str = this._dialogService.AskAdminPassword();
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            return this._userService.CanConfirmAdminPin(str);
        }

        public bool ConfirmAutomationCommand(string commandName)
        {
            return this.ConfirmAutomationCommand(this._cacheService.GetAutomationCommandByName(commandName));
        }

        public void ExecuteAutomationCommand(string commandName, string commandValue, dynamic dataObject)
        {
            if (dataObject != (dynamic)null)
            {
                dataObject.AutomationCommandName = commandName;
                dataObject.CommandValue = commandValue;
            }
            else
            {
                dataObject = new { AutomationCommandName = commandName, CommandValue = commandValue };
            }
            this._applicationState.NotifyEvent("AutomationCommandExecuted", dataObject);
        }
    }
}
