﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Threading;
using Magentix.Domain.Models.Accounts;
using Magentix.Domain.Models.Entities;
using Magentix.Domain.Models.Menus;
using Magentix.Domain.Models.Settings;
using Magentix.Domain.Models.Tickets;
using Magentix.Domain.Models.Users;
using Magentix.Infrastructure.Settings;
using Magentix.Persistance.Data;
using Magentix.Presentation.Services;
using Magentix.Presentation.Services.Common;
using Magentix.Services;
using Magentix.Services.Common;
using Stateless;

namespace Magentix.Presentation.Common.Services
{
    [Export(typeof(IApplicationState))]
    [Export(typeof(IApplicationStateSetter))]
    public class ApplicationState : AbstractService, IApplicationState, IApplicationStateSetter
    {
        private readonly IDepartmentService _departmentService;
        private readonly ISettingService _settingService;
        private readonly ICacheService _cacheService;
        private readonly IExpressionService _expressionService;
        private readonly INotificationService _notificationService;
        private readonly StateMachine<AppScreens, AppScreens> _screenState;

        [ImportingConstructor]
        public ApplicationState(IDepartmentService departmentService, ISettingService settingService,
            ICacheService cacheService, IExpressionService expressionService, INotificationService notificationService)
        {
            _screenState = new StateMachine<AppScreens, AppScreens>(() => ActiveAppScreen, state => ActiveAppScreen = state);
            _screenState.OnUnhandledTrigger(HandleTrigger);
            _departmentService = departmentService;
            _settingService = settingService;
            _cacheService = cacheService;
            _expressionService = expressionService;
            _notificationService = notificationService;
            SetCurrentDepartment(Department.Default);
            CurrentTicketType = TicketType.Default;
            _isLandscape = true;
        }

        public Dispatcher MainDispatcher { get; set; }
        public AppScreens ActiveAppScreen { get; private set; }
        public CurrentDepartmentData CurrentDepartment { get; private set; }
        public TicketType CurrentTicketType { get; set; }
        public TicketType TempTicketType { get; set; }
        public EntityScreen SelectedEntityScreen { get; private set; }
        public EntityScreen TempEntityScreen { get; set; }

        private bool _isLocked;
        public bool IsLocked
        {
            get { return _isLocked; }
        }

        private Terminal _terminal;
        public Terminal CurrentTerminal { get { return _terminal ?? (_terminal = GetCurrentTerminal()); } set { _terminal = value; } }

        private bool _isLandscape;
        public bool IsLandscape
        {
            get { return _isLandscape; }
            set
            {
                if (_isLandscape != value)
                {
                    _isLandscape = value;
                    EventServiceFactory.EventService.PublishEvent(IsLandscape
                                                                      ? EventTopicNames.EnableLandscape
                                                                      : EventTopicNames.DisableLandscape);
                }
            }
        }

        private User _currentLoggedInUser;
        public User CurrentLoggedInUser
        {
            get { return _currentLoggedInUser ?? User.Nobody; }
            private set { _currentLoggedInUser = value; }
        }

        private IEnumerable<WorkPeriod> _lastTwoWorkPeriods;
        public IEnumerable<WorkPeriod> LastTwoWorkPeriods
        {
            get { return _lastTwoWorkPeriods ?? (_lastTwoWorkPeriods = Dao.Last<WorkPeriod>(2)); }
        }

        public WorkPeriod CurrentWorkPeriod
        {
            get { return LastTwoWorkPeriods.LastOrDefault(); }
        }

        public WorkPeriod PreviousWorkPeriod
        {
            get { return LastTwoWorkPeriods.Count() > 1 ? LastTwoWorkPeriods.FirstOrDefault() : null; }
        }

        public bool IsCurrentWorkPeriodOpen
        {
            get
            {
                return CurrentWorkPeriod != null && CurrentWorkPeriod.StartDate == CurrentWorkPeriod.EndDate;
            }
        }

        public void SetCurrentLoggedInUser(User user)
        {
            CurrentLoggedInUser = user;
            SetLocalSetting("CURRENTUSER", user.Name);
        }

        public void SetCurrentDepartment(Department department)
        {
            if (department == null) return;
            if (CurrentDepartment == null || department != CurrentDepartment.Model)
            {
                CurrentDepartment = new CurrentDepartmentData { Model = department };
                CurrentDepartment.Model.PublishEvent(EventTopicNames.SelectedDepartmentChanged);
                SetCurrentTicketType(_cacheService.GetTicketTypeById(CurrentDepartment.TicketTypeId));
            }
            SetLocalSetting("DEPARTMENT", CurrentDepartment.Name);
        }

        public void SetCurrentDepartment(int departmentId)
        {
            SetCurrentDepartment(_departmentService.GetDepartment(departmentId));
        }

        public void SetCurrentApplicationScreen(AppScreens appScreen)
        {
            InteractionService.ClearMouseClickQueue();
            _screenState.Fire(appScreen);
        }

        private void HandleTrigger(AppScreens arg1, AppScreens arg2)
        {
            ActiveAppScreen = arg2;
            if (arg1 != arg2) new AppScreenChangeData(arg1, arg2).PublishEvent(EventTopicNames.Changed);
        }

        public EntityScreen SetSelectedEntityScreen(EntityScreen entityScreen)
        {
            if (IsLocked && TempEntityScreen == null) TempEntityScreen = SelectedEntityScreen;
            else if (!IsLocked && TempEntityScreen != null)
            {
                entityScreen = TempEntityScreen;
                TempEntityScreen = null;
            }
            SelectedEntityScreen = entityScreen;
            SetLocalSetting("ENTITYSCREEN", entityScreen != null ? entityScreen.Name : "");
            return entityScreen;
        }

        public void SetApplicationLocked(bool isLocked)
        {
            _isLocked = isLocked;
            SetLocalSetting("ISLOCKED", isLocked.ToString());
            (this as IApplicationState).PublishEvent(EventTopicNames.ApplicationLockStateChanged);
        }

        public void SetNumberpadValue(string value)
        {
            SetLocalSetting("NUMBERPAD", value);
        }

        public void SetCurrentTicketType(TicketType ticketType)
        {
            if (ticketType != CurrentTicketType)
            {
                CurrentTicketType = ticketType ?? TicketType.Default;
                CurrentTicketType.PublishEvent(EventTopicNames.TicketTypeChanged);
            }
        }

        public void SetCurrentTerminal(string terminalName)
        {
            _terminal = _settingService.GetTerminalByName(terminalName);
        }

        public string NumberPadValue
        {
            get { return _settingService.ReadLocalSetting("NUMBERPAD").StringValue; }
        }

        private Terminal GetCurrentTerminal()
        {
            if (!string.IsNullOrEmpty(LocalSettings.TerminalName))
            {
                var terminal = _settingService.GetTerminalByName(LocalSettings.TerminalName);
                if (terminal != null) return terminal;
            }
            var dterminal = _settingService.GetDefaultTerminal();
            return dterminal ?? Terminal.DefaultTerminal;
        }

        public void ResetWorkPeriods()
        {
            _lastTwoWorkPeriods = null;
        }

        public void SetLocalSetting(string settingName, string settingValue)
        {
            _settingService.ReadLocalSetting(settingName).StringValue = settingValue;
        }

        public override void Reset()
        {
            _cacheService.ResetCache();
            _departmentService.ResetCache();
            _settingService.ResetCache();
            _expressionService.ResetCache();
            _lastTwoWorkPeriods = null;
            _terminal = null;
        }

        public ProductTimer GetProductTimer(int menuItemId)
        {
            return _cacheService.GetProductTimer(CurrentTicketType.Id,
                                                 CurrentTerminal.Id,
                                                 CurrentDepartment.Id,
                                                 CurrentLoggedInUser.UserRole.Id,
                                                 menuItemId);
        }

        public IEnumerable<OrderTagGroup> GetOrderTagGroups(string portionName, params int[] menuItemIds)
        {
            return this._cacheService.GetOrderTagGroups(this.CurrentTicketType.Id, this.CurrentTerminal.Id, this.CurrentDepartment.Id, this.CurrentLoggedInUser.UserRole.Id, portionName, menuItemIds);
        }


        public IEnumerable<AccountTransactionDocumentType> GetAccountTransactionDocumentTypes(int accountTypeId)
        {
            return _cacheService.GetAccountTransactionDocumentTypes(accountTypeId,
                                                                    CurrentTerminal.Id,
                                                                    CurrentLoggedInUser.UserRole.Id);
        }

        public IEnumerable<AccountTransactionDocumentType> GetBatchDocumentTypes(IEnumerable<string> accountTypeNamesList)
        {
            return _cacheService.GetBatchDocumentTypes(accountTypeNamesList, CurrentTerminal.Id,
                                                       CurrentLoggedInUser.UserRole.Id);
        }

        public IEnumerable<PaymentType> GetPaymentScreenPaymentTypes()
        {
            return _cacheService.GetPaymentScreenPaymentTypes(CurrentTicketType.Id,
                                                            CurrentTerminal.Id,
                                                            CurrentDepartment.Id,
                                                            CurrentLoggedInUser.UserRole.Id);
        }

        public IEnumerable<ChangePaymentType> GetChangePaymentTypes()
        {
            return _cacheService.GetChangePaymentTypes(CurrentTicketType.Id,
                                                       CurrentTerminal.Id,
                                                       CurrentDepartment.Id,
                                                       CurrentLoggedInUser.UserRole.Id);
        }

        public IEnumerable<TicketTagGroup> GetTicketTagGroups()
        {
            return _cacheService.GetTicketTagGroups(CurrentTicketType.Id,
                                                    CurrentTerminal.Id,
                                                    CurrentDepartment.Id,
                                                    CurrentLoggedInUser.UserRole.Id);
        }

        public IEnumerable<AutomationCommandData> GetAutomationCommands()
        {
            return _cacheService.GetAutomationCommands(CurrentTicketType.Id,
                                                       CurrentTerminal.Id,
                                                       CurrentDepartment != null ? CurrentDepartment.Id : -1,
                                                       CurrentLoggedInUser.UserRole.Id);
        }

        public IEnumerable<CalculationSelector> GetCalculationSelectors()
        {
            return _cacheService.GetCalculationSelectors(CurrentTicketType.Id,
                                                         CurrentTerminal.Id,
                                                         CurrentDepartment.Id,
                                                         CurrentLoggedInUser.UserRole.Id);
        }

        public IEnumerable<EntityScreen> GetEntityScreens()
        {
            return _cacheService.GetEntityScreens(CurrentTerminal.Id,
                                                    CurrentDepartment.Id,
                                                    CurrentLoggedInUser.UserRole.Id);
        }

        public IEnumerable<EntityScreen> GetTicketEntityScreens(int ticketTypeId)
        {
            return this._cacheService.GetTicketEntityScreens(ticketTypeId, this.CurrentTerminal.Id, this.CurrentDepartment.Id, this.CurrentLoggedInUser.UserRole.Id);
        }

        public IEnumerable<TaxTemplate> GetTaxTemplates(int menuItemId)
        {
            return _cacheService.GetTaxTemplates(CurrentTicketType.Id,
                                                 CurrentTerminal.Id,
                                                 CurrentDepartment.Id,
                                                 CurrentLoggedInUser.UserRole.Id,
                                                 menuItemId);
        }

        public Printer GetReportPrinter()
        {
            return _cacheService.GetPrinters().FirstOrDefault(x => x.Id == CurrentTerminal.ReportPrinterId);
        }

        public Printer GetTransactionPrinter()
        {
            return _cacheService.GetPrinters().FirstOrDefault(x => x.Id == CurrentTerminal.TransactionPrinterId);
        }

        public void NotifyEvent(string eventName, object dataObject)
        {
            var terminalId = CurrentTerminal.Id;
            var departmentId = CurrentDepartment.Id;
            var roleId = CurrentLoggedInUser.UserRole.Id;
            var ticketTypeId = CurrentTicketType.Id;
            _notificationService.NotifyEvent(eventName, dataObject, terminalId, departmentId, roleId, ticketTypeId, x => x.PublishEvent(EventTopicNames.ExecuteEvent, true));
        }

        public void ResetState()
        {
            _departmentService.ResetCache();
            var did = CurrentDepartment.Id;
            CurrentDepartment = null;
            SetCurrentDepartment(did);
        }

        public bool ShouldDisplayTicketView()
        {
            if (this.CurrentDepartment.TicketCreationMethod == 1)
            {
                return true;
            }
            if (!this.HasEntityScreenForSelectedTicketType() && !this.IsAllScreensAreCustomScreens())
            {
                return true;
            }
            return false;
        }

        public void InitializeSettings()
        {
            this.SetCurrentTerminal(LocalSettings.TerminalName);
            this.SetLocalSetting("ISCURRENTWORKPERIODOPEN", (this.IsCurrentWorkPeriodOpen ? "TRUE" : "FALSE"));
        }

        public bool HasEntityScreenForSelectedTicketType()
        {
            return this.GetTicketEntityScreens(this.CurrentTicketType.Id).Any<EntityScreen>();
        }

        public bool IsAllScreensAreCustomScreens()
        {
            return this.GetEntityScreens().All<EntityScreen>((EntityScreen x) => x.EntityTypeId == 0);
        }

        public void SetCurrentDepartment(string departmentName)
        {
            this.SetCurrentDepartment(this._departmentService.GetDepartmentByName(departmentName));
        }

        public EntityScreen SetSelectedEntityScreen(string entityScreenName)
        {
            EntityScreen entityScreenByName;
            if (!string.IsNullOrEmpty(entityScreenName))
            {
                entityScreenByName = this._cacheService.GetEntityScreenByName(entityScreenName);
            }
            else
            {
                entityScreenByName = null;
            }
            return this.SetSelectedEntityScreen(entityScreenByName);
        }
    }
}
