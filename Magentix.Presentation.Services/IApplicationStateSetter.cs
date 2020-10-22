using System.Collections.Generic;
using Magentix.Domain.Models.Entities;
using Magentix.Domain.Models.Tickets;
using Magentix.Domain.Models.Users;
using Magentix.Presentation.Services.Common;

namespace Magentix.Presentation.Services
{
    public interface IApplicationStateSetter
    {
        void InitializeSettings();

        void ResetWorkPeriods();

        void SetApplicationLocked(bool isLocked);

        void SetCurrentApplicationScreen(AppScreens appScreen);

        void SetCurrentDepartment(int departmentId);

        void SetCurrentDepartment(string departmentName);

        void SetCurrentLoggedInUser(User user);

        void SetCurrentTerminal(string terminalName);

        void SetCurrentTicketType(TicketType ticketType);

        void SetNumberpadValue(string value);

        EntityScreen SetSelectedEntityScreen(EntityScreen entityScreen);

        EntityScreen SetSelectedEntityScreen(string entityScreenName);
    }
}