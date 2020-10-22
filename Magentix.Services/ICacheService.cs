using Magentix.Domain.Models.Accounts;
using Magentix.Domain.Models.Automation;
using Magentix.Domain.Models.Entities;
using Magentix.Domain.Models.Inventory;
using Magentix.Domain.Models.Menus;
using Magentix.Domain.Models.Settings;
using Magentix.Domain.Models.Tasks;
using Magentix.Domain.Models.Tickets;
using Magentix.Services.Common;
using System;
using System.Collections.Generic;

namespace Magentix.Services
{
    public interface ICacheService
    {
        bool CanShowStateOnEndOfDayReport(string stateName, string state);

        bool CanShowStateOnProductReport(string stateName, string state);

        bool CanShowStateOnTicket(string stateName, string state);

        AccountTransactionType FindAccountTransactionType(int sourceAccountTypeId, int targetAccountTypeId, int defaultSourceId, int defaultTargetId);

        MenuItem FindMenuItemByBarcode(string insertedData);

        Account GetAccountById(int accountId);

        IEnumerable<AccountScreen> GetAccountScreens();

        AccountTransactionDocumentType GetAccountTransactionDocumentTypeById(int documentId);

        AccountTransactionDocumentType GetAccountTransactionDocumentTypeByName(string documentName);

        IEnumerable<AccountTransactionDocumentType> GetAccountTransactionDocumentTypes(int accountTypeId, int terminalId, int userRoleId);

        AccountTransactionType GetAccountTransactionTypeById(int id);

        AccountTransactionType GetAccountTransactionTypeByName(string name);

        int GetAccountTransactionTypeIdByName(string accountTransactionTypeName);

        AccountType GetAccountTypeById(int accountTypeId);

        int GetAccountTypeIdByName(string accountTypeName);

        IEnumerable<AccountType> GetAccountTypes();

        IEnumerable<AccountType> GetAccountTypesByName(IEnumerable<string> accountTypeNames);

        IEnumerable<AppAction> GetActions();

        IEnumerable<AppRule> GetAppRules(string eventName, int terminalId, int departmentId, int userRoleId, int ticketTypeId);

        AutomationCommand GetAutomationCommandByName(string automationCommand);

        IEnumerable<AutomationCommandData> GetAutomationCommands(int ticketTypeId, int terminalId, int departmentId, int userRoleId);

        IEnumerable<AccountTransactionDocumentType> GetBatchDocumentTypes(IEnumerable<string> accountTypeNames, int terminalId, int userRoleId);

        IEnumerable<CalculationSelector> GetCalculationSelectors(int ticketTypeId, int terminalId, int departmentId, int userRoleId);

        CalculationType GetCalculationTypeById(int id);

        CalculationType GetCalculationTypeByName(string name);

        int GetCalculationTypeIdByName(string calculationTypeName);

        string GetCalculationTypeNameById(int calculationTypeId);

        ChangePaymentType GetChangePaymentTypeById(int id);

        IEnumerable<ChangePaymentType> GetChangePaymentTypes(int ticketTypeId, int terminalId, int departmentId, int userRoleId);

        ForeignCurrency GetCurrencyById(int currencyId);

        string GetCurrencySymbol(int currencyId);

        IEnumerable<Entity> GetEntities(int entityTypeId, string stateData);

        Entity GetEntityById(int entityId);

        Entity GetEntityByName(string entityTypeName, string entityName);

        EntityScreen GetEntityScreenByName(string screenName);

        IEnumerable<EntityScreen> GetEntityScreens(int terminalId, int departmentId, int userRoleId);

        EntityType GetEntityTypeById(int entityTypeId);

        EntityType GetEntityTypeByName(string entityTypeName);

        int GetEntityTypeIdByEntityName(string entityName);

        string GetEntityTypeNameById(int entityTypeId);

        IEnumerable<EntityType> GetEntityTypes();

        IEnumerable<EntityType> GetEntityTypesByTicketType(int ticketTypeId);

        IEnumerable<ForeignCurrency> GetForeignCurrencies();

        IEnumerable<InventoryTransactionType> GetInventoryTransactionTypes();

        MenuItem GetMenuItem(Func<MenuItem, bool> expression);

        string GetMenuItemCustomTagData(int menuItemId, string fieldName);

        string GetMenuItemData(int menuItemId, Func<MenuItem, string> selector);

        IEnumerable<int> GetMenuItemIdsFromRecipeItems(int inventoryItemId);

        IEnumerable<int> GetMenuItemIdsFromRecipes();

        MenuItemPortion GetMenuItemPortion(int menuItemId, string portionName);

        IEnumerable<MenuItemPortion> GetMenuItemPortions(int menuItemId);

        OrderTagGroup GetOrderTagGroupByName(string tagName);

        OrderTagGroup GetOrderTagGroupByOrderTagName(string orderTag);

        IEnumerable<OrderTagGroup> GetOrderTagGroups(int ticketTypeId, int terminalId, int departmentId, int userRoleId, string portionName, params int[] menuItemIds);

        IEnumerable<PaymentType> GetPaymentScreenPaymentTypes(int ticketTypeId, int terminalId, int departmentId, int userRoleId);

        PaymentType GetPaymentTypeById(int paymentTypeId);

        PaymentType GetPaymentTypeByName(string paymentTypeName);

        int GetPaymentTypeIdByName(string paymentTypeName);

        string GetPaymentTypeNameById(int paymentTypeId);

        IEnumerable<Printer> GetPrinters();

        IEnumerable<PrinterTemplate> GetPrinterTemplates();

        PrintJob GetPrintJobByName(string name);

        ProductTimer GetProductTimer(int ticketTypeId, int terminalId, int departmentId, int userRoleId, int menuItemId);

        Recipe GetRecipe(string portionName, int menuItemId);

        ScreenMenu GetScreenMenu(int screenMenuId);

        string GetStateColor(string entityState);

        IEnumerable<State> GetStates(int stateType);

        TaskType GetTaskTypeByName(string taskTypeName);

        int GetTaskTypeIdByName(string taskTypeName);

        IEnumerable<string> GetTaskTypeNames();

        TaxTemplate GetTaxTemplateByName(string taxTemplateName);

        IEnumerable<TaxTemplate> GetTaxTemplates(int ticketTypeId, int terminalId, int departmentId, int userRoleId, int menuItemId);

        IEnumerable<EntityScreen> GetTicketEntityScreens(int ticketTypeId, int terminalId, int departmentId, int userRoleId);

        TicketTagGroup GetTicketTagGroupById(int id);

        TicketTagGroup GetTicketTagGroupByName(string name);

        IEnumerable<string> GetTicketTagGroupNames();

        IEnumerable<TicketTagGroup> GetTicketTagGroups(int ticketTypeId, int terminalId, int departmentId, int userRoleId);

        TicketType GetTicketTypeById(int ticketTypeId);

        int GetTicketTypeIdByName(string ticketTypeName);

        string GetTicketTypeNameById(int ticketTypeId);

        IEnumerable<TicketType> GetTicketTypes();

        IEnumerable<Warehouse> GetWarehouses();

        bool RecipeExists(int menuItemId, string portionName);

        void ResetCache();

        void ResetOrderTagCache();

        void ResetTicketTagCache();
    }
}
