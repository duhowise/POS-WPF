using Magentix.Domain.Models.Accounts;
using Magentix.Domain.Models.Automation;
using Magentix.Domain.Models.Entities;
using Magentix.Domain.Models.Inventory;
using Magentix.Domain.Models.Menus;
using Magentix.Domain.Models.Settings;
using Magentix.Domain.Models.Tasks;
using Magentix.Domain.Models.Tickets;
using Magentix.Infrastructure.Data;
using Magentix.Persistance;
using Magentix.Persistance.Data;
using Magentix.Services;
using Magentix.Services.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Magentix.Services.Implementations
{
    [Export(typeof(ICacheService))]
    internal class CacheService : ICacheService
    {
        private readonly ICacheDao _cacheDao;

        private readonly IPrinterDao _printerDao;

        private readonly EntityCache _entityCache;

        private IEnumerable<AppRule> _rules;

        private IEnumerable<AppAction> _actions;

        private IEnumerable<MenuItem> _menuItems;

        private IEnumerable<ProductTimer> _productTimers;

        private IEnumerable<OrderTagGroup> _orderTagGroups;

        private IEnumerable<TaxTemplate> _taxTemplates;

        private IEnumerable<TicketTagGroup> _ticketTagGroups;

        private IEnumerable<AccountTransactionDocumentType> _documentTypes;

        private IEnumerable<ChangePaymentType> _changePaymentTypes;

        private IEnumerable<PaymentType> _paymentTypes;

        private IEnumerable<AccountTransactionType> _accountTransactionTypes;

        private IEnumerable<EntityScreen> _entityScreens;

        private IEnumerable<AccountScreen> _accountScreens;

        private IEnumerable<ForeignCurrency> _foreignCurrencies;

        private IEnumerable<ScreenMenu> _screenMenus;

        private IEnumerable<TaskType> _taskTypes;

        private IEnumerable<TicketType> _ticketTypes;

        private IEnumerable<CalculationType> _calculationTypes;

        private IEnumerable<CalculationSelector> _calculationSelectors;

        private IEnumerable<AutomationCommand> _automationCommands;

        private IEnumerable<AccountType> _accountTypes;

        private IEnumerable<PrintJob> _printJobs;

        private IEnumerable<EntityType> _entityTypes;

        private IEnumerable<Warehouse> _warehouses;

        private IEnumerable<InventoryTransactionType> _inventoryTransactionTypes;

        private IEnumerable<Printer> _printers;

        private IEnumerable<PrinterTemplate> _printerTemplates;

        private IEnumerable<State> _states;

        private IEnumerable<Recipe> _recipes;

        public IEnumerable<AccountScreen> AccountScreens
        {
            get
            {
                IEnumerable<AccountScreen> accountScreens = this._accountScreens;
                if (accountScreens == null)
                {
                    IEnumerable<AccountScreen> accountScreens1 = this._cacheDao.GetAccountScreens();
                    IEnumerable<AccountScreen> accountScreens2 = accountScreens1;
                    this._accountScreens = accountScreens1;
                    accountScreens = accountScreens2;
                }
                return accountScreens;
            }
        }

        public IEnumerable<AccountTransactionType> AccountTransactionTypes
        {
            get
            {
                IEnumerable<AccountTransactionType> accountTransactionTypes = this._accountTransactionTypes;
                if (accountTransactionTypes == null)
                {
                    IEnumerable<AccountTransactionType> accountTransactionTypes1 = this._cacheDao.GetAccountTransactionTypes();
                    IEnumerable<AccountTransactionType> accountTransactionTypes2 = accountTransactionTypes1;
                    this._accountTransactionTypes = accountTransactionTypes1;
                    accountTransactionTypes = accountTransactionTypes2;
                }
                return accountTransactionTypes;
            }
        }

        public IEnumerable<AccountType> AccountTypes
        {
            get
            {
                IEnumerable<AccountType> accountTypes = this._accountTypes;
                if (accountTypes == null)
                {
                    IEnumerable<AccountType> accountTypes1 = this._cacheDao.GetAccountTypes();
                    IEnumerable<AccountType> accountTypes2 = accountTypes1;
                    this._accountTypes = accountTypes1;
                    accountTypes = accountTypes2;
                }
                return accountTypes;
            }
        }

        public IEnumerable<AppAction> Actions
        {
            get
            {
                IEnumerable<AppAction> appActions = this._actions;
                if (appActions == null)
                {
                    IEnumerable<AppAction> actions = this._cacheDao.GetActions();
                    IEnumerable<AppAction> appActions1 = actions;
                    this._actions = actions;
                    appActions = appActions1;
                }
                return appActions;
            }
        }

        public IEnumerable<AutomationCommand> AutomationCommands
        {
            get
            {
                IEnumerable<AutomationCommand> automationCommands = this._automationCommands;
                if (automationCommands == null)
                {
                    IEnumerable<AutomationCommand> automationCommands1 = this._cacheDao.GetAutomationCommands();
                    IEnumerable<AutomationCommand> automationCommands2 = automationCommands1;
                    this._automationCommands = automationCommands1;
                    automationCommands = automationCommands2;
                }
                return automationCommands;
            }
        }

        public IEnumerable<CalculationSelector> CalculationSelectors
        {
            get
            {
                IEnumerable<CalculationSelector> calculationSelectors = this._calculationSelectors;
                if (calculationSelectors == null)
                {
                    IEnumerable<CalculationSelector> calculationSelectors1 = this._cacheDao.GetCalculationSelectors();
                    IEnumerable<CalculationSelector> calculationSelectors2 = calculationSelectors1;
                    this._calculationSelectors = calculationSelectors1;
                    calculationSelectors = calculationSelectors2;
                }
                return calculationSelectors;
            }
        }

        public IEnumerable<CalculationType> CalculationTypes
        {
            get
            {
                IEnumerable<CalculationType> calculationTypes = this._calculationTypes;
                if (calculationTypes == null)
                {
                    IEnumerable<CalculationType> calculationTypes1 = this._cacheDao.GetCalculationTypes();
                    IEnumerable<CalculationType> calculationTypes2 = calculationTypes1;
                    this._calculationTypes = calculationTypes1;
                    calculationTypes = calculationTypes2;
                }
                return calculationTypes;
            }
        }

        public IEnumerable<ChangePaymentType> ChangePaymentTypes
        {
            get
            {
                IEnumerable<ChangePaymentType> changePaymentTypes = this._changePaymentTypes;
                if (changePaymentTypes == null)
                {
                    IEnumerable<ChangePaymentType> changePaymentTypes1 = this._cacheDao.GetChangePaymentTypes();
                    IEnumerable<ChangePaymentType> changePaymentTypes2 = changePaymentTypes1;
                    this._changePaymentTypes = changePaymentTypes1;
                    changePaymentTypes = changePaymentTypes2;
                }
                return changePaymentTypes;
            }
        }

        public IEnumerable<AccountTransactionDocumentType> DocumentTypes
        {
            get
            {
                IEnumerable<AccountTransactionDocumentType> accountTransactionDocumentTypes = this._documentTypes;
                if (accountTransactionDocumentTypes == null)
                {
                    IEnumerable<AccountTransactionDocumentType> accountTransactionDocumentTypes1 = this._cacheDao.GetAccountTransactionDocumentTypes();
                    IEnumerable<AccountTransactionDocumentType> accountTransactionDocumentTypes2 = accountTransactionDocumentTypes1;
                    this._documentTypes = accountTransactionDocumentTypes1;
                    accountTransactionDocumentTypes = accountTransactionDocumentTypes2;
                }
                return accountTransactionDocumentTypes;
            }
        }

        public IEnumerable<EntityScreen> EntityScreens
        {
            get
            {
                IEnumerable<EntityScreen> entityScreens = this._entityScreens;
                if (entityScreens == null)
                {
                    IEnumerable<EntityScreen> entityScreens1 = this._cacheDao.GetEntityScreens();
                    IEnumerable<EntityScreen> entityScreens2 = entityScreens1;
                    this._entityScreens = entityScreens1;
                    entityScreens = entityScreens2;
                }
                return entityScreens;
            }
        }

        public IEnumerable<EntityType> EntityTypes
        {
            get
            {
                IEnumerable<EntityType> entityTypes = this._entityTypes;
                if (entityTypes == null)
                {
                    IEnumerable<EntityType> entityTypes1 = this._cacheDao.GetEntityTypes();
                    IEnumerable<EntityType> entityTypes2 = entityTypes1;
                    this._entityTypes = entityTypes1;
                    entityTypes = entityTypes2;
                }
                return entityTypes;
            }
        }

        public IEnumerable<ForeignCurrency> ForeignCurrencies
        {
            get
            {
                IEnumerable<ForeignCurrency> foreignCurrencies = this._foreignCurrencies;
                if (foreignCurrencies == null)
                {
                    IEnumerable<ForeignCurrency> foreignCurrencies1 = this._cacheDao.GetForeignCurrencies();
                    IEnumerable<ForeignCurrency> foreignCurrencies2 = foreignCurrencies1;
                    this._foreignCurrencies = foreignCurrencies1;
                    foreignCurrencies = foreignCurrencies2;
                }
                return foreignCurrencies;
            }
        }

        public IEnumerable<InventoryTransactionType> InventoryTransactionTypes
        {
            get
            {
                IEnumerable<InventoryTransactionType> inventoryTransactionTypes = this._inventoryTransactionTypes;
                if (inventoryTransactionTypes == null)
                {
                    IEnumerable<InventoryTransactionType> inventoryTransactionTypes1 = this._cacheDao.GetInventoryTransactionTypes();
                    IEnumerable<InventoryTransactionType> inventoryTransactionTypes2 = inventoryTransactionTypes1;
                    this._inventoryTransactionTypes = inventoryTransactionTypes1;
                    inventoryTransactionTypes = inventoryTransactionTypes2;
                }
                return inventoryTransactionTypes;
            }
        }

        public IEnumerable<MenuItem> MenuItems
        {
            get
            {
                IEnumerable<MenuItem> menuItems = this._menuItems;
                if (menuItems == null)
                {
                    IEnumerable<MenuItem> menuItems1 = this._cacheDao.GetMenuItems();
                    IEnumerable<MenuItem> menuItems2 = menuItems1;
                    this._menuItems = menuItems1;
                    menuItems = menuItems2;
                }
                return menuItems;
            }
        }

        public IEnumerable<OrderTagGroup> OrderTagGroups
        {
            get
            {
                IEnumerable<OrderTagGroup> orderTagGroups = this._orderTagGroups;
                if (orderTagGroups == null)
                {
                    IEnumerable<OrderTagGroup> orderTagGroups1 = this._cacheDao.GetOrderTagGroups();
                    IEnumerable<OrderTagGroup> orderTagGroups2 = orderTagGroups1;
                    this._orderTagGroups = orderTagGroups1;
                    orderTagGroups = orderTagGroups2;
                }
                return orderTagGroups;
            }
        }

        public IEnumerable<PaymentType> PaymentTypes
        {
            get
            {
                IEnumerable<PaymentType> paymentTypes = this._paymentTypes;
                if (paymentTypes == null)
                {
                    IEnumerable<PaymentType> paymentTypes1 = this._cacheDao.GetPaymentTypes();
                    IEnumerable<PaymentType> paymentTypes2 = paymentTypes1;
                    this._paymentTypes = paymentTypes1;
                    paymentTypes = paymentTypes2;
                }
                return paymentTypes;
            }
        }

        public IEnumerable<Printer> Printers
        {
            get
            {
                IEnumerable<Printer> printers = this._printers;
                if (printers == null)
                {
                    IEnumerable<Printer> printers1 = this._printerDao.GetPrinters();
                    IEnumerable<Printer> printers2 = printers1;
                    this._printers = printers1;
                    printers = printers2;
                }
                return printers;
            }
        }

        protected IEnumerable<PrinterTemplate> PrinterTemplates
        {
            get
            {
                IEnumerable<PrinterTemplate> printerTemplates = this._printerTemplates;
                if (printerTemplates == null)
                {
                    IEnumerable<PrinterTemplate> printerTemplates1 = this._printerDao.GetPrinterTemplates();
                    IEnumerable<PrinterTemplate> printerTemplates2 = printerTemplates1;
                    this._printerTemplates = printerTemplates1;
                    printerTemplates = printerTemplates2;
                }
                return printerTemplates;
            }
        }

        public IEnumerable<PrintJob> PrintJobs
        {
            get
            {
                IEnumerable<PrintJob> printJobs = this._printJobs;
                if (printJobs == null)
                {
                    IEnumerable<PrintJob> printJobs1 = this._cacheDao.GetPrintJobs();
                    IEnumerable<PrintJob> printJobs2 = printJobs1;
                    this._printJobs = printJobs1;
                    printJobs = printJobs2;
                }
                return printJobs;
            }
        }

        public IEnumerable<ProductTimer> ProductTimers
        {
            get
            {
                IEnumerable<ProductTimer> productTimers = this._productTimers;
                if (productTimers == null)
                {
                    IEnumerable<ProductTimer> productTimers1 = this._cacheDao.GetProductTimers();
                    IEnumerable<ProductTimer> productTimers2 = productTimers1;
                    this._productTimers = productTimers1;
                    productTimers = productTimers2;
                }
                return productTimers;
            }
        }

        public IEnumerable<Recipe> Recipes
        {
            get
            {
                IEnumerable<Recipe> recipes = this._recipes;
                if (recipes == null)
                {
                    Expression<Func<Recipe, object>>[] expressionArray = new Expression<Func<Recipe, object>>[3];
                    ParameterExpression parameterExpression = Expression.Parameter(typeof(Recipe), "x");
                    MemberExpression memberExpression = Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(Recipe).GetMethod("get_Portion").MethodHandle));
                    ParameterExpression[] parameterExpressionArray = new ParameterExpression[] { parameterExpression };
                    expressionArray[0] = Expression.Lambda<Func<Recipe, object>>(memberExpression, parameterExpressionArray);
                    ParameterExpression parameterExpression1 = Expression.Parameter(typeof(Recipe), "x");
                    MemberExpression memberExpression1 = Expression.Property(parameterExpression1, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(Recipe).GetMethod("get_RecipeItems").MethodHandle));
                    ParameterExpression[] parameterExpressionArray1 = new ParameterExpression[] { parameterExpression1 };
                    expressionArray[1] = Expression.Lambda<Func<Recipe, object>>(memberExpression1, parameterExpressionArray1);
                    ParameterExpression parameterExpression2 = Expression.Parameter(typeof(Recipe), "x");
                    MethodInfo methodFromHandle = (MethodInfo)MethodBase.GetMethodFromHandle(typeof(Enumerable).GetMethod("Select", new Type[] { typeof(IEnumerable<RecipeItem>), typeof(Func<RecipeItem, InventoryItem>) }).MethodHandle);
                    Expression[] expressionArray1 = new Expression[] { Expression.Property(parameterExpression2, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(Recipe).GetMethod("get_RecipeItems").MethodHandle)), null };
                    ParameterExpression parameterExpression3 = Expression.Parameter(typeof(RecipeItem), "y");
                    MemberExpression memberExpression2 = Expression.Property(parameterExpression3, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(RecipeItem).GetMethod("get_InventoryItem").MethodHandle));
                    ParameterExpression[] parameterExpressionArray2 = new ParameterExpression[] { parameterExpression3 };
                    expressionArray1[1] = Expression.Lambda<Func<RecipeItem, InventoryItem>>(memberExpression2, parameterExpressionArray2);
                    MethodCallExpression methodCallExpression = Expression.Call(null, methodFromHandle, expressionArray1);
                    ParameterExpression[] parameterExpressionArray3 = new ParameterExpression[] { parameterExpression2 };
                    expressionArray[2] = Expression.Lambda<Func<Recipe, object>>(methodCallExpression, parameterExpressionArray3);
                    IEnumerable<Recipe> recipes1 = Dao.Query<Recipe>(expressionArray);
                    IEnumerable<Recipe> recipes2 = recipes1;
                    this._recipes = recipes1;
                    recipes = recipes2;
                }
                return recipes;
            }
        }

        public IEnumerable<AppRule> Rules
        {
            get
            {
                IEnumerable<AppRule> appRules = this._rules;
                if (appRules == null)
                {
                    IEnumerable<AppRule> rules = this._cacheDao.GetRules();
                    IEnumerable<AppRule> appRules1 = rules;
                    this._rules = rules;
                    appRules = appRules1;
                }
                return appRules;
            }
        }

        public IEnumerable<ScreenMenu> ScreenMenus
        {
            get
            {
                IEnumerable<ScreenMenu> screenMenus = this._screenMenus;
                if (screenMenus == null)
                {
                    IEnumerable<ScreenMenu> screenMenus1 = this._cacheDao.GetScreenMenus();
                    IEnumerable<ScreenMenu> screenMenus2 = screenMenus1;
                    this._screenMenus = screenMenus1;
                    screenMenus = screenMenus2;
                }
                return screenMenus;
            }
        }

        public IEnumerable<State> States
        {
            get
            {
                IEnumerable<State> states = this._states;
                if (states == null)
                {
                    IEnumerable<State> states1 = this._cacheDao.GetStates();
                    IEnumerable<State> states2 = states1;
                    this._states = states1;
                    states = states2;
                }
                return states;
            }
        }

        public IEnumerable<TaskType> TaskTypes
        {
            get
            {
                IEnumerable<TaskType> taskTypes = this._taskTypes;
                if (taskTypes == null)
                {
                    IEnumerable<TaskType> taskTypes1 = this._cacheDao.GetTaskTypes();
                    IEnumerable<TaskType> taskTypes2 = taskTypes1;
                    this._taskTypes = taskTypes1;
                    taskTypes = taskTypes2;
                }
                return taskTypes;
            }
        }

        public IEnumerable<TaxTemplate> TaxTemplates
        {
            get
            {
                IEnumerable<TaxTemplate> taxTemplates = this._taxTemplates;
                if (taxTemplates == null)
                {
                    IEnumerable<TaxTemplate> taxTemplates1 = this._cacheDao.GetTaxTemplates();
                    IEnumerable<TaxTemplate> taxTemplates2 = taxTemplates1;
                    this._taxTemplates = taxTemplates1;
                    taxTemplates = taxTemplates2;
                }
                return taxTemplates;
            }
        }

        public IEnumerable<TicketTagGroup> TicketTagGroups
        {
            get
            {
                IEnumerable<TicketTagGroup> ticketTagGroups = this._ticketTagGroups;
                if (ticketTagGroups == null)
                {
                    IEnumerable<TicketTagGroup> ticketTagGroups1 = this._cacheDao.GetTicketTagGroups();
                    IEnumerable<TicketTagGroup> ticketTagGroups2 = ticketTagGroups1;
                    this._ticketTagGroups = ticketTagGroups1;
                    ticketTagGroups = ticketTagGroups2;
                }
                return ticketTagGroups;
            }
        }

        public IEnumerable<TicketType> TicketTypes
        {
            get
            {
                IEnumerable<TicketType> ticketTypes = this._ticketTypes;
                if (ticketTypes == null)
                {
                    IEnumerable<TicketType> ticketTypes1 = this._cacheDao.GetTicketTypes();
                    IEnumerable<TicketType> ticketTypes2 = ticketTypes1;
                    this._ticketTypes = ticketTypes1;
                    ticketTypes = ticketTypes2;
                }
                return ticketTypes;
            }
        }

        public IEnumerable<Warehouse> Warehouses
        {
            get
            {
                IEnumerable<Warehouse> warehouses = this._warehouses;
                if (warehouses == null)
                {
                    IEnumerable<Warehouse> warehouses1 = this._cacheDao.GetWarehouses();
                    IEnumerable<Warehouse> warehouses2 = warehouses1;
                    this._warehouses = warehouses1;
                    warehouses = warehouses2;
                }
                return warehouses;
            }
        }

        [ImportingConstructor]
        public CacheService(ICacheDao cacheDao, IPrinterDao printerDao)
        {
            this._cacheDao = cacheDao;
            this._printerDao = printerDao;
            this._entityCache = new EntityCache();
        }

        public bool CanShowStateOnEndOfDayReport(string stateName, string state)
        {
            return this.States.Any<State>((State x) => {
                if (!(x.Name == state) && !(x.GroupName == stateName))
                {
                    return false;
                }
                return x.ShowOnEndOfDayReport;
            });
        }

        public bool CanShowStateOnProductReport(string stateName, string state)
        {
            return this.States.Any<State>((State x) => {
                if (!(x.Name == state) && !(x.GroupName == stateName))
                {
                    return false;
                }
                return x.ShowOnProductReport;
            });
        }

        public bool CanShowStateOnTicket(string stateName, string state)
        {
            return this.States.Any<State>((State x) => {
                if (!(x.Name == state) && !(x.GroupName == stateName))
                {
                    return false;
                }
                return x.ShowOnTicket;
            });
        }

        public AccountTransactionType FindAccountTransactionType(int sourceAccountTypeId, int targetAccountTypeId, int defaultSourceId, int defaultTargetId)
        {
            List<AccountTransactionType> list = this.AccountTransactionTypes.Where<AccountTransactionType>((AccountTransactionType x) => {
                if (x.SourceAccountTypeId != sourceAccountTypeId)
                {
                    return false;
                }
                return x.TargetAccountTypeId == targetAccountTypeId;
            }).ToList<AccountTransactionType>();
            if (defaultSourceId > 0 && list.Any<AccountTransactionType>((AccountTransactionType x) => x.DefaultSourceAccountId == defaultSourceId))
            {
                list = (
                    from x in list
                    where x.DefaultSourceAccountId == defaultSourceId
                    select x).ToList<AccountTransactionType>();
            }
            if (defaultTargetId > 0 && list.Any<AccountTransactionType>((AccountTransactionType x) => x.DefaultTargetAccountId == defaultTargetId))
            {
                list = (
                    from x in list
                    where x.DefaultTargetAccountId == defaultTargetId
                    select x).ToList<AccountTransactionType>();
            }
            return list.FirstOrDefault<AccountTransactionType>();
        }

        public MenuItem FindMenuItemByBarcode(string insertedData)
        {
            if (string.IsNullOrWhiteSpace(insertedData))
            {
                return null;
            }
            return this.MenuItems.FirstOrDefault<MenuItem>((MenuItem x) => x.Barcode == insertedData);
        }

        public Account GetAccountById(int accountId)
        {
            return Dao.SingleWithCache<Account>((Account x) => x.Id == accountId);
        }

        public IEnumerable<AccountScreen> GetAccountScreens()
        {
            return this.AccountScreens;
        }

        public AccountTransactionDocumentType GetAccountTransactionDocumentTypeById(int documentId)
        {
            return this.DocumentTypes.SingleOrDefault<AccountTransactionDocumentType>((AccountTransactionDocumentType x) => x.Id == documentId);
        }

        public AccountTransactionDocumentType GetAccountTransactionDocumentTypeByName(string documentName)
        {
            return this.DocumentTypes.SingleOrDefault<AccountTransactionDocumentType>((AccountTransactionDocumentType x) => x.Name == documentName);
        }

        public IEnumerable<AccountTransactionDocumentType> GetAccountTransactionDocumentTypes(int accountTypeId, int terminalId, int userRoleId)
        {
            IEnumerable<AccountTransactionDocumentTypeMap> accountTransactionDocumentTypeMaps = (
                from x in this.DocumentTypes
                where x.MasterAccountTypeId == accountTypeId
                select x).SelectMany<AccountTransactionDocumentType, AccountTransactionDocumentTypeMap>((AccountTransactionDocumentType x) => x.AccountTransactionDocumentTypeMaps).Where<AccountTransactionDocumentTypeMap>((AccountTransactionDocumentTypeMap x) => {
                    if (x.TerminalId == 0)
                    {
                        return true;
                    }
                    return x.TerminalId == terminalId;
                }).Where<AccountTransactionDocumentTypeMap>((AccountTransactionDocumentTypeMap x) => {
                    if (x.UserRoleId == 0)
                    {
                        return true;
                    }
                    return x.UserRoleId == userRoleId;
                });
            return
                from x in this.DocumentTypes
                where accountTransactionDocumentTypeMaps.Any<AccountTransactionDocumentTypeMap>((AccountTransactionDocumentTypeMap y) => y.AccountTransactionDocumentTypeId == x.Id)
                orderby x.SortOrder
                select x;
        }

        public AccountTransactionType GetAccountTransactionTypeById(int id)
        {
            return this.AccountTransactionTypes.Single<AccountTransactionType>((AccountTransactionType x) => x.Id == id);
        }

        public AccountTransactionType GetAccountTransactionTypeByName(string name)
        {
            return this.AccountTransactionTypes.SingleOrDefault<AccountTransactionType>((AccountTransactionType x) => x.Name == name);
        }

        public int GetAccountTransactionTypeIdByName(string accountTransactionTypeName)
        {
            if (!this.AccountTransactionTypes.Any<AccountTransactionType>((AccountTransactionType x) => x.Name == accountTransactionTypeName))
            {
                return 0;
            }
            return this.AccountTransactionTypes.Single<AccountTransactionType>((AccountTransactionType x) => x.Name == accountTransactionTypeName).Id;
        }

        public AccountType GetAccountTypeById(int accountTypeId)
        {
            return this.AccountTypes.Single<AccountType>((AccountType x) => x.Id == accountTypeId);
        }

        public int GetAccountTypeIdByName(string accountTypeName)
        {
            if (!this.AccountTypes.Any<AccountType>((AccountType x) => x.Name == accountTypeName))
            {
                return 0;
            }
            return this.AccountTypes.Single<AccountType>((AccountType x) => x.Name == accountTypeName).Id;
        }

        public IEnumerable<AccountType> GetAccountTypes()
        {
            return this.AccountTypes;
        }

        public IEnumerable<AccountType> GetAccountTypesByName(IEnumerable<string> accountTypeNames)
        {
            return
                from x in this.AccountTypes
                where accountTypeNames.Contains<string>(x.Name)
                select x;
        }

        public IEnumerable<AppAction> GetActions()
        {
            return this.Actions;
        }

        public IEnumerable<AppRule> GetAppRules(string eventName, int terminalId, int departmentId, int userRoleId, int ticketTypeId)
        {
            IEnumerable<AppRuleMap> appRuleMaps = (
                from x in this.Rules
                where x.EventName == eventName
                select x).SelectMany<AppRule, AppRuleMap>((AppRule x) => x.AppRuleMaps).Where<AppRuleMap>((AppRuleMap x) => {
                    if (x.TerminalId == 0)
                    {
                        return true;
                    }
                    return x.TerminalId == terminalId;
                }).Where<AppRuleMap>((AppRuleMap x) => {
                    if (x.DepartmentId == 0)
                    {
                        return true;
                    }
                    return x.DepartmentId == departmentId;
                }).Where<AppRuleMap>((AppRuleMap x) => {
                    if (x.UserRoleId == 0)
                    {
                        return true;
                    }
                    return x.UserRoleId == userRoleId;
                }).Where<AppRuleMap>((AppRuleMap x) => {
                    if (x.TicketTypeId == 0)
                    {
                        return true;
                    }
                    return x.TicketTypeId == ticketTypeId;
                });
            return
                from x in this.Rules
                where appRuleMaps.Any<AppRuleMap>((AppRuleMap y) => y.AppRuleId == x.Id)
                orderby x.SortOrder
                select x;
        }

        public AutomationCommand GetAutomationCommandByName(string automationCommand)
        {
            return this.AutomationCommands.FirstOrDefault<AutomationCommand>((AutomationCommand x) => x.Name == automationCommand);
        }

        public IEnumerable<AutomationCommandData> GetAutomationCommands(int ticketTypeId, int terminalId, int departmentId, int userRoleId)
        {
            IEnumerable<AutomationCommandMap> automationCommandMaps = this.AutomationCommands.SelectMany<AutomationCommand, AutomationCommandMap>((AutomationCommand x) => x.AutomationCommandMaps).Where<AutomationCommandMap>((AutomationCommandMap x) => {
                if (x.TicketTypeId == 0)
                {
                    return true;
                }
                return x.TicketTypeId == ticketTypeId;
            }).Where<AutomationCommandMap>((AutomationCommandMap x) => {
                if (x.TerminalId == 0)
                {
                    return true;
                }
                return x.TerminalId == terminalId;
            }).Where<AutomationCommandMap>((AutomationCommandMap x) => {
                if (x.DepartmentId == 0)
                {
                    return true;
                }
                return x.DepartmentId == departmentId;
            }).Where<AutomationCommandMap>((AutomationCommandMap x) => {
                if (x.UserRoleId == 0)
                {
                    return true;
                }
                return x.UserRoleId == userRoleId;
            });
            IEnumerable<AutomationCommandData> automationCommandDatum =
                from x in automationCommandMaps
                select new AutomationCommandData()
                {
                    AutomationCommand = this.AutomationCommands.First<AutomationCommand>((AutomationCommand y) => y.Id == x.AutomationCommandId),
                    DisplayOnPayment = x.DisplayOnPayment,
                    DisplayOnTicket = x.DisplayOnTicket,
                    DisplayOnOrders = x.DisplayOnOrders,
                    DisplayOnTicketList = x.DisplayOnTicketList,
                    DisplayUnderTicket = x.DisplayUnderTicket,
                    DisplayUnderTicket2 = x.DisplayUnderTicket2,
                    DisplayOnCommandSelector = x.DisplayOnCommandSelector,
                    EnabledStates = x.EnabledStates,
                    VisibleStates = x.VisibleStates
                };
            return
                from x in automationCommandDatum
                orderby x.AutomationCommand.SortOrder
                select x;
        }

        public IEnumerable<AccountTransactionDocumentType> GetBatchDocumentTypes(IEnumerable<int> accountTypeIds, int terminalId, int userRoleId)
        {
            IEnumerable<AccountTransactionDocumentTypeMap> accountTransactionDocumentTypeMaps = this.DocumentTypes.Where<AccountTransactionDocumentType>((AccountTransactionDocumentType x) => {
                if (!x.BatchCreateDocuments)
                {
                    return false;
                }
                return accountTypeIds.Contains<int>(x.MasterAccountTypeId);
            }).SelectMany<AccountTransactionDocumentType, AccountTransactionDocumentTypeMap>((AccountTransactionDocumentType x) => x.AccountTransactionDocumentTypeMaps).Where<AccountTransactionDocumentTypeMap>((AccountTransactionDocumentTypeMap x) => {
                if (x.TerminalId == 0)
                {
                    return true;
                }
                return x.TerminalId == terminalId;
            }).Where<AccountTransactionDocumentTypeMap>((AccountTransactionDocumentTypeMap x) => {
                if (x.UserRoleId == 0)
                {
                    return true;
                }
                return x.UserRoleId == userRoleId;
            });
            return
                from x in this.DocumentTypes
                where accountTransactionDocumentTypeMaps.Any<AccountTransactionDocumentTypeMap>((AccountTransactionDocumentTypeMap y) => y.AccountTransactionDocumentTypeId == x.Id)
                orderby x.SortOrder
                select x;
        }

        public IEnumerable<AccountTransactionDocumentType> GetBatchDocumentTypes(IEnumerable<string> accountTypeNamesList, int terminalId, int userRoleId)
        {
            IEnumerable<int> accountTypesByName =
                from x in this.GetAccountTypesByName(accountTypeNamesList)
                select x.Id;
            return this.GetBatchDocumentTypes(accountTypesByName, terminalId, userRoleId);
        }

        public IEnumerable<CalculationSelector> GetCalculationSelectors(int ticketTypeId, int terminalId, int departmentId, int userRoleId)
        {
            IEnumerable<CalculationSelectorMap> calculationSelectorMaps = this.CalculationSelectors.SelectMany<CalculationSelector, CalculationSelectorMap>((CalculationSelector x) => x.CalculationSelectorMaps).Where<CalculationSelectorMap>((CalculationSelectorMap x) => {
                if (x.TicketTypeId == 0)
                {
                    return true;
                }
                return x.TicketTypeId == ticketTypeId;
            }).Where<CalculationSelectorMap>((CalculationSelectorMap x) => {
                if (x.TerminalId == 0)
                {
                    return true;
                }
                return x.TerminalId == terminalId;
            }).Where<CalculationSelectorMap>((CalculationSelectorMap x) => {
                if (x.DepartmentId == 0)
                {
                    return true;
                }
                return x.DepartmentId == departmentId;
            }).Where<CalculationSelectorMap>((CalculationSelectorMap x) => {
                if (x.UserRoleId == 0)
                {
                    return true;
                }
                return x.UserRoleId == userRoleId;
            });
            return
                from x in this.CalculationSelectors
                where calculationSelectorMaps.Any<CalculationSelectorMap>((CalculationSelectorMap y) => y.CalculationSelectorId == x.Id)
                orderby x.SortOrder
                select x;
        }

        public CalculationType GetCalculationTypeById(int id)
        {
            return this.CalculationTypes.FirstOrDefault<CalculationType>((CalculationType x) => x.Id == id);
        }

        public CalculationType GetCalculationTypeByName(string name)
        {
            return this.CalculationTypes.FirstOrDefault<CalculationType>((CalculationType y) => y.Name == name);
        }

        public int GetCalculationTypeIdByName(string calculationTypeName)
        {
            CalculationType calculationTypeByName = this.GetCalculationTypeByName(calculationTypeName);
            if (calculationTypeByName == null)
            {
                return 0;
            }
            return calculationTypeByName.Id;
        }

        public string GetCalculationTypeNameById(int calculationTypeId)
        {
            CalculationType calculationTypeById = this.GetCalculationTypeById(calculationTypeId);
            if (calculationTypeById == null)
            {
                return "";
            }
            return calculationTypeById.Name;
        }

        public ChangePaymentType GetChangePaymentTypeById(int id)
        {
            return this.ChangePaymentTypes.Single<ChangePaymentType>((ChangePaymentType x) => x.Id == id);
        }

        public IEnumerable<ChangePaymentType> GetChangePaymentTypes(int ticketTypeId, int terminalId, int departmentId, int userRoleId)
        {
            IEnumerable<ChangePaymentTypeMap> changePaymentTypeMaps = this.ChangePaymentTypes.SelectMany<ChangePaymentType, ChangePaymentTypeMap>((ChangePaymentType x) => x.ChangePaymentTypeMaps).Where<ChangePaymentTypeMap>((ChangePaymentTypeMap x) => {
                if (x.TicketTypeId == 0)
                {
                    return true;
                }
                return x.TicketTypeId == ticketTypeId;
            }).Where<ChangePaymentTypeMap>((ChangePaymentTypeMap x) => {
                if (x.TerminalId == 0)
                {
                    return true;
                }
                return x.TerminalId == terminalId;
            }).Where<ChangePaymentTypeMap>((ChangePaymentTypeMap x) => {
                if (x.DepartmentId == 0)
                {
                    return true;
                }
                return x.DepartmentId == departmentId;
            }).Where<ChangePaymentTypeMap>((ChangePaymentTypeMap x) => {
                if (x.UserRoleId == 0)
                {
                    return true;
                }
                return x.UserRoleId == userRoleId;
            });
            return
                from x in this.ChangePaymentTypes
                where changePaymentTypeMaps.Any<ChangePaymentTypeMap>((ChangePaymentTypeMap y) => y.ChangePaymentTypeId == x.Id)
                orderby x.SortOrder
                select x;
        }

        public ForeignCurrency GetCurrencyById(int currencyId)
        {
            return this.GetForeignCurrencies().SingleOrDefault<ForeignCurrency>((ForeignCurrency x) => x.Id == currencyId);
        }

        public string GetCurrencySymbol(int currencyId)
        {
            if (currencyId == 0)
            {
                return "";
            }
            return this.GetForeignCurrencies().Single<ForeignCurrency>((ForeignCurrency x) => x.Id == currencyId).CurrencySymbol;
        }

        public IEnumerable<Entity> GetEntities(int entityTypeId, string stateData)
        {
            return this._entityCache.GetEntities(entityTypeId, stateData);
        }

        public Entity GetEntityById(int accountId)
        {
            return Dao.SingleWithCache<Entity>((Entity x) => x.Id == accountId);
        }

        public Entity GetEntityByName(string entityTypeName, string entityName)
        {
            EntityType entityType = this.EntityTypes.Single<EntityType>((EntityType x) => x.Name == entityTypeName);
            return this._cacheDao.GetEntityByName(entityType.Id, entityName) ?? Entity.GetNullEntity(entityType.Id);
        }

        public EntityScreen GetEntityScreenByName(string screenName)
        {
            return this.EntityScreens.FirstOrDefault<EntityScreen>((EntityScreen x) => x.Name == screenName);
        }

        public IEnumerable<EntityScreen> GetEntityScreens(int terminalId, int departmentId, int userRoleId)
        {
            IEnumerable<EntityScreenMap> entityScreenMaps = this.EntityScreens.SelectMany<EntityScreen, EntityScreenMap>((EntityScreen x) => x.EntityScreenMaps).Where<EntityScreenMap>((EntityScreenMap x) => {
                if (x.TerminalId == 0)
                {
                    return true;
                }
                return x.TerminalId == terminalId;
            }).Where<EntityScreenMap>((EntityScreenMap x) => {
                if (x.DepartmentId == 0)
                {
                    return true;
                }
                return x.DepartmentId == departmentId;
            }).Where<EntityScreenMap>((EntityScreenMap x) => {
                if (x.UserRoleId == 0)
                {
                    return true;
                }
                return x.UserRoleId == userRoleId;
            }).Where<EntityScreenMap>((EntityScreenMap x) => x.IsVisibleForPos());
            return
                from x in this.EntityScreens
                where entityScreenMaps.Any<EntityScreenMap>((EntityScreenMap y) => y.EntityScreenId == x.Id)
                orderby x.SortOrder
                select x;
        }

        public EntityType GetEntityTypeById(int entityTypeId)
        {
            return this.EntityTypes.SingleOrDefault<EntityType>((EntityType x) => x.Id == entityTypeId);
        }

        public EntityType GetEntityTypeByName(string entityTypeName)
        {
            return this.EntityTypes.SingleOrDefault<EntityType>((EntityType x) => x.Name == entityTypeName);
        }

        public int GetEntityTypeIdByEntityName(string entityName)
        {
            EntityType entityType = this.EntityTypes.FirstOrDefault<EntityType>((EntityType x) => x.Name == entityName) ?? this.EntityTypes.FirstOrDefault<EntityType>((EntityType x) => x.EntityName == entityName);
            if (entityType == null)
            {
                return 0;
            }
            return entityType.Id;
        }

        public string GetEntityTypeNameById(int entityTypeId)
        {
            EntityType entityTypeById = this.GetEntityTypeById(entityTypeId);
            if (entityTypeById == null)
            {
                return "";
            }
            return entityTypeById.Name;
        }

        public IEnumerable<EntityType> GetEntityTypes()
        {
            return this.EntityTypes;
        }

        public IEnumerable<EntityType> GetEntityTypesByTicketType(int ticketTypeId)
        {
            return
                from x in this.TicketTypes.Single<TicketType>((TicketType x) => x.Id == ticketTypeId).EntityTypeAssignments
                orderby x.SortOrder
                select this.GetEntityTypeById(x.EntityTypeId);
        }

        public IEnumerable<ForeignCurrency> GetForeignCurrencies()
        {
            return this.ForeignCurrencies;
        }

        public IEnumerable<InventoryTransactionType> GetInventoryTransactionTypes()
        {
            return this.InventoryTransactionTypes;
        }

        public MenuItem GetMenuItem(Func<MenuItem, bool> expression)
        {
            return this.MenuItems.SingleOrDefault<MenuItem>(expression);
        }

        public string GetMenuItemCustomTagData(int menuItemId, string fieldName)
        {
            MenuItem menuItem = this.MenuItems.FirstOrDefault<MenuItem>((MenuItem x) => x.Id == menuItemId);
            if (menuItem == null)
            {
                return "";
            }
            return menuItem.GetTagValue(fieldName);
        }

        public string GetMenuItemData(int menuItemId, Func<MenuItem, string> selector)
        {
            return (
                from x in this.MenuItems
                where x.Id == menuItemId
                select x).Select<MenuItem, string>(selector).FirstOrDefault<string>();
        }

        public IEnumerable<int> GetMenuItemIdsFromRecipeItems(int inventoryItemId)
        {
            return this.Recipes.Where<Recipe>((Recipe x) => {
                if (x.Portion == null)
                {
                    return false;
                }
                return x.RecipeItems.Any<RecipeItem>((RecipeItem y) => y.InventoryItem.Id == inventoryItemId);
            }).Select<Recipe, int>((Recipe x) => x.Portion.MenuItemId).Distinct<int>();
        }

        public IEnumerable<int> GetMenuItemIdsFromRecipes()
        {
            return (
                from x in this.Recipes
                where x.Portion != null
                select x.Portion.MenuItemId).Distinct<int>();
        }

        public MenuItemPortion GetMenuItemPortion(int menuItemId, string portionName)
        {
            MenuItem menuItem = this.GetMenuItem((MenuItem x) => x.Id == menuItemId);
            if (menuItem.Portions.Count == 0)
            {
                return null;
            }
            return menuItem.Portions.FirstOrDefault<MenuItemPortion>((MenuItemPortion x) => x.Name == portionName) ?? menuItem.Portions[0];
        }

        public IEnumerable<MenuItemPortion> GetMenuItemPortions(int menuItemId)
        {
            return this.GetMenuItem((MenuItem x) => x.Id == menuItemId).Portions;
        }

        public OrderTagGroup GetOrderTagGroupByName(string tagName)
        {
            return this.OrderTagGroups.FirstOrDefault<OrderTagGroup>((OrderTagGroup x) => x.Name == tagName);
        }

        public OrderTagGroup GetOrderTagGroupByOrderTagName(string orderTag)
        {
            return this.OrderTagGroups.FirstOrDefault<OrderTagGroup>((OrderTagGroup x) => x.OrderTags.Any<OrderTag>((OrderTag y) => y.Name == orderTag));
        }

        public IEnumerable<OrderTagGroup> GetOrderTagGroups(int ticketTypeId, int terminalId, int departmentId, int userRoleId, string portionName, params int[] menuItemIds)
        {
            IEnumerable<OrderTagGroup> orderTagGroups =
                from y in this.OrderTagGroups
                orderby y.SortOrder
                select y;
            return menuItemIds.Aggregate<int, IEnumerable<OrderTagGroup>>(orderTagGroups, (IEnumerable<OrderTagGroup> x, int y) => this.InternalGetOrderTagGroups(ticketTypeId, terminalId, departmentId, userRoleId, y, portionName));
        }

        public IEnumerable<PaymentType> GetPaymentScreenPaymentTypes(int ticketTypeId, int terminalId, int departmentId, int userRoleId)
        {
            IEnumerable<PaymentTypeMap> paymentTypeMaps = this.PaymentTypes.SelectMany<PaymentType, PaymentTypeMap>((PaymentType x) => x.PaymentTypeMaps).Where<PaymentTypeMap>((PaymentTypeMap x) => {
                if (x.TicketTypeId == 0)
                {
                    return true;
                }
                return x.TicketTypeId == ticketTypeId;
            }).Where<PaymentTypeMap>((PaymentTypeMap x) => {
                if (x.TerminalId == 0)
                {
                    return true;
                }
                return x.TerminalId == terminalId;
            }).Where<PaymentTypeMap>((PaymentTypeMap x) => {
                if (x.DepartmentId == 0)
                {
                    return true;
                }
                return x.DepartmentId == departmentId;
            }).Where<PaymentTypeMap>((PaymentTypeMap x) => {
                if (x.UserRoleId == 0)
                {
                    return true;
                }
                return x.UserRoleId == userRoleId;
            });
            return
                from x in this.PaymentTypes
                where paymentTypeMaps.Any<PaymentTypeMap>((PaymentTypeMap y) => y.PaymentTypeId == x.Id)
                orderby x.SortOrder
                select x;
        }

        public PaymentType GetPaymentTypeById(int paymentTypeId)
        {
            return this.PaymentTypes.Single<PaymentType>((PaymentType x) => x.Id == paymentTypeId);
        }

        public PaymentType GetPaymentTypeByName(string paymentTypeName)
        {
            return this.PaymentTypes.SingleOrDefault<PaymentType>((PaymentType x) => x.Name == paymentTypeName);
        }

        public int GetPaymentTypeIdByName(string paymentTypeName)
        {
            PaymentType paymentType = this.PaymentTypes.FirstOrDefault<PaymentType>((PaymentType x) => x.Name == paymentTypeName);
            if (paymentType == null)
            {
                return 0;
            }
            return paymentType.Id;
        }

        public string GetPaymentTypeNameById(int paymentTypeId)
        {
            PaymentType paymentType = this.PaymentTypes.SingleOrDefault<PaymentType>((PaymentType x) => x.Id == paymentTypeId);
            if (paymentType == null)
            {
                return "";
            }
            return paymentType.Name;
        }

        public IEnumerable<Printer> GetPrinters()
        {
            return this.Printers;
        }

        public IEnumerable<PrinterTemplate> GetPrinterTemplates()
        {
            return this.PrinterTemplates;
        }

        public PrintJob GetPrintJobByName(string name)
        {
            return this.PrintJobs.FirstOrDefault<PrintJob>((PrintJob x) => x.Name == name);
        }

        public ProductTimer GetProductTimer(int ticketTypeId, int terminalId, int departmentId, int userId, int menuItemId)
        {
            List<ProductTimer> list = this.ProductTimers.ToList<ProductTimer>();
            MenuItem menuItem = this.GetMenuItem((MenuItem x) => x.Id == menuItemId);
            IEnumerable<ProdcutTimerMap> prodcutTimerMaps = list.SelectMany<ProductTimer, ProdcutTimerMap>((ProductTimer x) => x.ProductTimerMaps).Where<ProdcutTimerMap>((ProdcutTimerMap x) => {
                if (x.TicketTypeId == 0)
                {
                    return true;
                }
                return x.TicketTypeId == ticketTypeId;
            }).Where<ProdcutTimerMap>((ProdcutTimerMap x) => {
                if (x.TerminalId == 0)
                {
                    return true;
                }
                return x.TerminalId == terminalId;
            }).Where<ProdcutTimerMap>((ProdcutTimerMap x) => {
                if (x.DepartmentId == 0)
                {
                    return true;
                }
                return x.DepartmentId == departmentId;
            }).Where<ProdcutTimerMap>((ProdcutTimerMap x) => {
                if (x.UserRoleId == 0)
                {
                    return true;
                }
                return x.UserRoleId == userId;
            }).Where<ProdcutTimerMap>((ProdcutTimerMap x) => {
                if (x.MenuItemGroupCode == null)
                {
                    return true;
                }
                return x.MenuItemGroupCode == menuItem.GroupCode;
            }).Where<ProdcutTimerMap>((ProdcutTimerMap x) => {
                if (x.MenuItemId == 0)
                {
                    return true;
                }
                return x.MenuItemId == menuItemId;
            });
            return list.FirstOrDefault<ProductTimer>((ProductTimer x) => prodcutTimerMaps.Any<ProdcutTimerMap>((ProdcutTimerMap y) => y.ProductTimerId == x.Id));
        }

        public Recipe GetRecipe(string portionName, int menuItemId)
        {
            return this.Recipes.Single<Recipe>((Recipe x) => {
                if (x.Portion.Name != portionName)
                {
                    return false;
                }
                return x.Portion.MenuItemId == menuItemId;
            });
        }

        public ScreenMenu GetScreenMenu(int screenMenuId)
        {
            if (!this.ScreenMenus.Any<ScreenMenu>((ScreenMenu x) => x.Id == screenMenuId))
            {
                return this.ScreenMenus.FirstOrDefault<ScreenMenu>();
            }
            return this.ScreenMenus.Single<ScreenMenu>((ScreenMenu x) => x.Id == screenMenuId);
        }

        public string GetStateColor(string state)
        {
            if (!this.States.Any<State>((State x) => x.Name == state))
            {
                return "Gainsboro";
            }
            return this.States.Single<State>((State x) => x.Name == state).Color;
        }

        public IEnumerable<State> GetStates(int stateType)
        {
            return
                from x in this.States
                where x.StateType == stateType
                select x;
        }

        public TaskType GetTaskTypeByName(string taskTypeName)
        {
            return this.TaskTypes.FirstOrDefault<TaskType>((TaskType x) => x.Name == taskTypeName);
        }

        public int GetTaskTypeIdByName(string taskTypeName)
        {
            TaskType taskType = this.TaskTypes.FirstOrDefault<TaskType>((TaskType x) => x.Name == taskTypeName);
            if (taskType == null)
            {
                return 0;
            }
            return taskType.Id;
        }

        public IEnumerable<string> GetTaskTypeNames()
        {
            return
                from x in this.TaskTypes
                select x.Name;
        }

        public TaxTemplate GetTaxTemplateByName(string taxTemplateName)
        {
            return this.TaxTemplates.FirstOrDefault<TaxTemplate>((TaxTemplate x) => x.Name == taxTemplateName);
        }

        public IEnumerable<TaxTemplate> GetTaxTemplates(int ticketTypeId, int terminalId, int departmentId, int userRoleId, int menuItemId)
        {
            MenuItem menuItem = this.GetMenuItem((MenuItem x) => x.Id == menuItemId);
            List<TaxTemplate> list = this.TaxTemplates.ToList<TaxTemplate>();
            IEnumerable<TaxTemplateMap> taxTemplateMaps = list.SelectMany<TaxTemplate, TaxTemplateMap>((TaxTemplate x) => x.TaxTemplateMaps).Where<TaxTemplateMap>((TaxTemplateMap x) => {
                if (x.TicketTypeId == 0)
                {
                    return true;
                }
                return x.TicketTypeId == ticketTypeId;
            }).Where<TaxTemplateMap>((TaxTemplateMap x) => {
                if (x.TerminalId == 0)
                {
                    return true;
                }
                return x.TerminalId == terminalId;
            }).Where<TaxTemplateMap>((TaxTemplateMap x) => {
                if (x.DepartmentId == 0)
                {
                    return true;
                }
                return x.DepartmentId == departmentId;
            }).Where<TaxTemplateMap>((TaxTemplateMap x) => {
                if (x.UserRoleId == 0)
                {
                    return true;
                }
                return x.UserRoleId == userRoleId;
            }).Where<TaxTemplateMap>((TaxTemplateMap x) => {
                if (x.MenuItemGroupCode == null)
                {
                    return true;
                }
                return x.MenuItemGroupCode == menuItem.GroupCode;
            }).Where<TaxTemplateMap>((TaxTemplateMap x) => {
                if (x.MenuItemId == 0)
                {
                    return true;
                }
                return x.MenuItemId == menuItemId;
            });
            return
                from x in list
                where taxTemplateMaps.Any<TaxTemplateMap>((TaxTemplateMap y) => y.TaxTemplateId == x.Id)
                orderby x.SortOrder
                select x;
        }

        public IEnumerable<EntityScreen> GetTicketEntityScreens(int ticketTypeId, int terminalId, int departmentId, int userRoleId)
        {
            IEnumerable<EntityScreenMap> entityScreenMaps = this.EntityScreens.SelectMany<EntityScreen, EntityScreenMap>((EntityScreen x) => x.EntityScreenMaps).Where<EntityScreenMap>((EntityScreenMap x) => {
                if (ticketTypeId == 0 || x.TicketTypeId == 0)
                {
                    return true;
                }
                return x.TicketTypeId == ticketTypeId;
            }).Where<EntityScreenMap>((EntityScreenMap x) => {
                if (x.TerminalId == 0)
                {
                    return true;
                }
                return x.TerminalId == terminalId;
            }).Where<EntityScreenMap>((EntityScreenMap x) => {
                if (x.DepartmentId == 0)
                {
                    return true;
                }
                return x.DepartmentId == departmentId;
            }).Where<EntityScreenMap>((EntityScreenMap x) => {
                if (x.UserRoleId == 0)
                {
                    return true;
                }
                return x.UserRoleId == userRoleId;
            }).Where<EntityScreenMap>((EntityScreenMap x) => x.IsVisibleForTicket());
            return this.EntityScreens.Where<EntityScreen>((EntityScreen x) => {
                if (x.EntityTypeId <= 0)
                {
                    return false;
                }
                return entityScreenMaps.Any<EntityScreenMap>((EntityScreenMap y) => y.EntityScreenId == x.Id);
            }).OrderBy<EntityScreen, int>((EntityScreen x) => x.SortOrder);
        }

        public TicketTagGroup GetTicketTagGroupById(int id)
        {
            return this.TicketTagGroups.FirstOrDefault<TicketTagGroup>((TicketTagGroup x) => x.Id == id);
        }

        public TicketTagGroup GetTicketTagGroupByName(string name)
        {
            return this.TicketTagGroups.FirstOrDefault<TicketTagGroup>((TicketTagGroup x) => x.Name == name);
        }

        public IEnumerable<string> GetTicketTagGroupNames()
        {
            return (
                from x in this.TicketTagGroups
                select x.Name).Distinct<string>();
        }

        public IEnumerable<TicketTagGroup> GetTicketTagGroups(int ticketTypeId, int terminalId, int departmentId, int userRoleId)
        {
            IEnumerable<TicketTagMap> ticketTagMaps = this.TicketTagGroups.SelectMany<TicketTagGroup, TicketTagMap>((TicketTagGroup x) => x.TicketTagMaps).Where<TicketTagMap>((TicketTagMap x) => {
                if (x.TicketTypeId == 0)
                {
                    return true;
                }
                return x.TicketTypeId == ticketTypeId;
            }).Where<TicketTagMap>((TicketTagMap x) => {
                if (x.TerminalId == 0)
                {
                    return true;
                }
                return x.TerminalId == terminalId;
            }).Where<TicketTagMap>((TicketTagMap x) => {
                if (x.DepartmentId == 0)
                {
                    return true;
                }
                return x.DepartmentId == departmentId;
            }).Where<TicketTagMap>((TicketTagMap x) => {
                if (x.UserRoleId == 0)
                {
                    return true;
                }
                return x.UserRoleId == userRoleId;
            });
            return
                from x in this.TicketTagGroups
                where ticketTagMaps.Any<TicketTagMap>((TicketTagMap y) => y.TicketTagGroupId == x.Id)
                orderby x.SortOrder
                select x;
        }

        public TicketType GetTicketTypeById(int ticketTypeId)
        {
            if (ticketTypeId == 0)
            {
                return null;
            }
            return this.TicketTypes.SingleOrDefault<TicketType>((TicketType x) => x.Id == ticketTypeId);
        }

        public int GetTicketTypeIdByName(string ticketTypeName)
        {
            TicketType ticketType = this.TicketTypes.FirstOrDefault<TicketType>((TicketType x) => x.Name == ticketTypeName);
            if (ticketType == null)
            {
                return 0;
            }
            return ticketType.Id;
        }

        public string GetTicketTypeNameById(int ticketTypeId)
        {
            TicketType ticketType = this.TicketTypes.FirstOrDefault<TicketType>((TicketType x) => x.Id == ticketTypeId);
            if (ticketType == null)
            {
                return "";
            }
            return ticketType.Name;
        }

        public IEnumerable<TicketType> GetTicketTypes()
        {
            return this.TicketTypes;
        }

        public IEnumerable<Warehouse> GetWarehouses()
        {
            return
                from x in this.Warehouses
                orderby x.SortOrder
                select x;
        }

        public IEnumerable<OrderTagGroup> InternalGetOrderTagGroups(int ticketTypeId, int terminalId, int departmentId, int userRoleId, int menuItemId, string portionName)
        {
            MenuItem menuItem = this.GetMenuItem((MenuItem x) => x.Id == menuItemId);
            List<OrderTagGroup> list = this.OrderTagGroups.ToList<OrderTagGroup>();
            IEnumerable<OrderTagMap> orderTagMaps = list.SelectMany<OrderTagGroup, OrderTagMap>((OrderTagGroup x) => x.OrderTagMaps).Where<OrderTagMap>((OrderTagMap x) => {
                if (x.TicketTypeId == 0)
                {
                    return true;
                }
                return x.TicketTypeId == ticketTypeId;
            }).Where<OrderTagMap>((OrderTagMap x) => {
                if (x.TerminalId == 0)
                {
                    return true;
                }
                return x.TerminalId == terminalId;
            }).Where<OrderTagMap>((OrderTagMap x) => {
                if (x.DepartmentId == 0)
                {
                    return true;
                }
                return x.DepartmentId == departmentId;
            }).Where<OrderTagMap>((OrderTagMap x) => {
                if (x.UserRoleId == 0)
                {
                    return true;
                }
                return x.UserRoleId == userRoleId;
            }).Where<OrderTagMap>((OrderTagMap x) => {
                if (x.MenuItemGroupCode == null || x.MenuItemGroupCode == menuItem.GroupCode)
                {
                    return true;
                }
                return x.MenuItemGroupCode == "*";
            }).Where<OrderTagMap>((OrderTagMap x) => {
                if (x.MenuItemId == 0)
                {
                    return true;
                }
                return x.MenuItemId == menuItemId;
            }).Where<OrderTagMap>((OrderTagMap x) => {
                if (string.IsNullOrEmpty(x.PortionName) || x.PortionName == portionName)
                {
                    return true;
                }
                return x.PortionName == "*";
            });
            return
                from x in list
                where orderTagMaps.Any<OrderTagMap>((OrderTagMap y) => y.OrderTagGroupId == x.Id)
                orderby x.SortOrder
                select x;
        }

        public bool RecipeExists(int menuItemId, string portionName)
        {
            return this.Recipes.Any<Recipe>((Recipe x) => {
                if (x.Portion.Name != portionName)
                {
                    return false;
                }
                return x.Portion.MenuItemId == menuItemId;
            });
        }

        public void ResetCache()
        {
            this._cacheDao.ResetCache();
            this._recipes = null;
            this._taxTemplates = null;
            this._inventoryTransactionTypes = null;
            this._warehouses = null;
            this._printers = null;
            this._printerTemplates = null;
            this._states = null;
            this._entityTypes = null;
            this._printJobs = null;
            this._accountTypes = null;
            this._automationCommands = null;
            this._calculationSelectors = null;
            this._calculationTypes = null;
            this._ticketTypes = null;
            this._taskTypes = null;
            this._screenMenus = null;
            this._foreignCurrencies = null;
            this._accountScreens = null;
            this._entityScreens = null;
            this._accountTransactionTypes = null;
            this._paymentTypes = null;
            this._changePaymentTypes = null;
            this._documentTypes = null;
            this._ticketTagGroups = null;
            this._orderTagGroups = null;
            this._productTimers = null;
            this._menuItems = null;
            this._rules = null;
            this._actions = null;
            this._entityCache.Reset();
        }

        public void ResetOrderTagCache()
        {
            this._orderTagGroups = null;
        }

        public void ResetTicketTagCache()
        {
            this._ticketTagGroups = null;
        }
    }
}
