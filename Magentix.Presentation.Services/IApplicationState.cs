using System.Collections.Generic;
using System.Windows.Threading;
using Magentix.Domain.Models.Accounts;
using Magentix.Domain.Models.Entities;
using Magentix.Domain.Models.Menus;
using Magentix.Domain.Models.Settings;
using Magentix.Domain.Models.Tickets;
using Magentix.Domain.Models.Users;
using Magentix.Presentation.Services.Common;
using Magentix.Services.Common;


namespace Magentix.Presentation.Services
{
    public interface IApplicationState
    {
        AppScreens ActiveAppScreen
        {
            get;
        }

        CurrentDepartmentData CurrentDepartment
        {
            get;
        }

        User CurrentLoggedInUser
        {
            get;
        }

        Terminal CurrentTerminal
        {
            get;
        }

        TicketType CurrentTicketType
        {
            get;
            set;
        }

        WorkPeriod CurrentWorkPeriod
        {
            get;
        }

        bool IsCurrentWorkPeriodOpen
        {
            get;
        }

        bool IsLandscape
        {
            get;
            set;
        }

        bool IsLocked
        {
            get;
        }

        Dispatcher MainDispatcher
        {
            get;
            set;
        }

        string NumberPadValue
        {
            get;
        }

        WorkPeriod PreviousWorkPeriod
        {
            get;
        }

        EntityScreen SelectedEntityScreen
        {
            get;
        }

        EntityScreen TempEntityScreen
        {
            get;
        }

        TicketType TempTicketType
        {
            get;
            set;
        }

        IEnumerable<AccountTransactionDocumentType> GetAccountTransactionDocumentTypes(int accountTypeId);

        IEnumerable<AutomationCommandData> GetAutomationCommands();

        IEnumerable<AccountTransactionDocumentType> GetBatchDocumentTypes(IEnumerable<string> accountTypeNamesList);

        IEnumerable<CalculationSelector> GetCalculationSelectors();

        IEnumerable<ChangePaymentType> GetChangePaymentTypes();

        IEnumerable<EntityScreen> GetEntityScreens();

        IEnumerable<OrderTagGroup> GetOrderTagGroups(string portionName, params int[] menuItemIds);

        IEnumerable<PaymentType> GetPaymentScreenPaymentTypes();

        ProductTimer GetProductTimer(int menuItemId);

        Printer GetReportPrinter();

        IEnumerable<TaxTemplate> GetTaxTemplates(int menuItemId);

        IEnumerable<EntityScreen> GetTicketEntityScreens(int ticketTypeId);

        IEnumerable<TicketTagGroup> GetTicketTagGroups();

        Printer GetTransactionPrinter();

        void NotifyEvent(string eventName, object dataObject);

        void ResetState();

        bool ShouldDisplayTicketView();
    }
}
