﻿using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Magentix.Domain.Models.Entities;
using Magentix.Presentation.Common;
using Magentix.Presentation.Services;
using Magentix.Presentation.Services.Common;
using Magentix.Services;
using Magentix.Presentation.Common.Commands;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Magentix.Modules.EntityModule
{
    [Export]
    public class EntitySwitcherViewModel : ObservableObject
    {
        private readonly IRegionManager _regionManager;
        private readonly IApplicationState _applicationState;
        private readonly IApplicationStateSetter _applicationStateSetter;
        private readonly ICacheService _cacheService;
        private readonly EntitySelectorView _entitySelectorView;
        private readonly EntitySelectorViewModel _entitySelectorViewModel;
        private readonly EntitySearchView _entitySearchView;
        private readonly EntitySearchViewModel _entitySearchViewModel;
        private readonly EntityDashboardView _entityDashboardView;
        private readonly EntityDashboardViewModel _entityDashboardViewModel;
        private readonly IUserService _userService;


        public bool CanDisplayCancelSelectionButton
        {
            get
            {
                if (this._applicationState.CurrentDepartment.TicketCreationMethod == 1)
                {
                    return true;
                }
                return this._applicationState.IsLocked;
            }
        }

        public bool CanDisplayRemoveSelectionButton
        {
            get
            {
                return this._applicationState.IsLocked;
            }
        }

        public CaptionCommand<string> CancelSelectionCommand
        {
            get;
            set;
        }

        public CaptionCommand<string> RemoveSelectionCommand
        {
            get;
            set;
        }

        private OperationRequest<Entity> _currentOperationRequest;

        [ImportingConstructor]
        public EntitySwitcherViewModel(IRegionManager regionManager,
            IApplicationState applicationState, IApplicationStateSetter applicationStateSetter, ICacheService cacheService,
            EntitySelectorView entitySelectorView, EntitySelectorViewModel entitySelectorViewModel,
            EntitySearchView entitySearchView, EntitySearchViewModel entitySearchViewModel,
            EntityDashboardView entityDashboardView, EntityDashboardViewModel entityDashboardViewModel, IUserService userService)
        {
            _regionManager = regionManager;
            _applicationState = applicationState;
            _applicationStateSetter = applicationStateSetter;
            _cacheService = cacheService;
            _entitySelectorView = entitySelectorView;
            _entitySelectorViewModel = entitySelectorViewModel;
            _entitySearchView = entitySearchView;
            _entitySearchViewModel = entitySearchViewModel;
            _entityDashboardView = entityDashboardView;
            _entityDashboardViewModel = entityDashboardViewModel;
            _userService = userService;

            SelectEntityCategoryCommand = new DelegateCommand<EntityScreen>(OnSelectEntityCategoryExecuted);

            EventServiceFactory.EventService.GetEvent<GenericEvent<EventAggregator>>().Subscribe(
            x =>
            {
                if (x.Topic == EventTopicNames.ResetCache)
                {
                    _entityScreens = null;
                    _entitySwitcherButtons = null;
                    RaisePropertyChanged(() => EntitySwitcherButtons);
                }
            });

            EventServiceFactory.EventService.GetEvent<GenericEvent<OperationRequest<Entity>>>().Subscribe(x =>
            {
                if (x.Topic == EventTopicNames.SelectEntity)
                {
                    var ss = UpdateEntityScreens(x.Value);
                    _currentOperationRequest = x.Value;
                    ActivateEntityScreen(ss);
                    if (ss != null && ss.DisplayMode == 1)
                        _entitySearchViewModel.SearchString = x.Value.Data;
                }
            });

            this.CancelSelectionCommand = new CaptionCommand<string>("Cancel", new Action<string>(this.OnCancelSelection));
            this.RemoveSelectionCommand = new CaptionCommand<string>("Remove", new Action<string>(this.OnRemoveSelection), new Func<string, bool>(this.CanRemoveSelection));
        }

        private void OnCancelSelection(string obj)
        {
            this._currentOperationRequest.Publish(this._currentOperationRequest.SelectedItem);
        }

        private void OnRemoveSelection(string obj)
        {
            this._currentOperationRequest.Publish(Entity.GetNullEntity(this._currentOperationRequest.SelectedItem.EntityTypeId));
        }
        
        private bool CanRemoveSelection(string arg)
        {
            if (this._currentOperationRequest == null || this._currentOperationRequest.SelectedItem == null || this._currentOperationRequest.SelectedItem.Id <= 0)
            {
                return false;
            }
            return this._userService.IsUserPermittedFor(PermissionNames.RemoveEntity);
        }

        private EntityScreen UpdateEntityScreens(OperationRequest<Entity> value)
        {
            List<EntityScreen> entityScreens = this._applicationState.GetEntityScreens().ToList<EntityScreen>();

            if (!entityScreens.Any()) return null;
            _entityScreens = entityScreens.OrderBy(x => x.SortOrder).ToList();
            _entitySwitcherButtons = null;
            var selectedScreen = _applicationState.SelectedEntityScreen;
            if (value != null && value.SelectedItem != null && _applicationState.CurrentDepartment != null)
            {
                if (_applicationState.IsLocked || _applicationState.CurrentDepartment.TicketCreationMethod == 1)
                    _entityScreens = _entityScreens.Where(x => x.EntityTypeId == value.SelectedItem.EntityTypeId).OrderBy(x => x.SortOrder);
                if (!_entityScreens.Any())
                    return entityScreens.ElementAt(0);
                if (selectedScreen == null || selectedScreen.EntityTypeId != value.SelectedItem.EntityTypeId)
                {
                    selectedScreen = null;
                    if (!string.IsNullOrEmpty(value.Data))
                    {
                        selectedScreen = _entityScreens.Where(x => x.DisplayMode == 1).FirstOrDefault(x => x.EntityTypeId == value.SelectedItem.EntityTypeId);
                    }
                    if (selectedScreen == null)
                    {
                        selectedScreen = _entityScreens.FirstOrDefault(x => x.EntityTypeId == value.SelectedItem.EntityTypeId);
                    }
                }
                if (selectedScreen == null) selectedScreen = _entityScreens.ElementAt(0);
            }
            return selectedScreen ?? EntityScreens.ElementAt(0);
        }

        public DelegateCommand<EntityScreen> SelectEntityCategoryCommand { get; set; }

        private IEnumerable<EntityScreen> _entityScreens;
        public IEnumerable<EntityScreen> EntityScreens
        {
            get
            {
                if (_applicationState.CurrentDepartment == null) return new List<EntityScreen>();
                return _entityScreens ?? (_entityScreens = _applicationState.GetEntityScreens().OrderBy(x => x.SortOrder));
            }
        }

        private List<EntitySwitcherButtonViewModel> _entitySwitcherButtons;
        public List<EntitySwitcherButtonViewModel> EntitySwitcherButtons
        {
            get
            {
                return _entitySwitcherButtons ?? (_entitySwitcherButtons = EntityScreens.Select(
                    x => new EntitySwitcherButtonViewModel(x, _applicationState, EntityScreens.Count() > 1)).ToList());
            }
        }

        private void OnSelectEntityCategoryExecuted(EntityScreen obj)
        {
            ActivateEntityScreen(obj);
        }

        private void ActivateEntityScreen(EntityScreen entityScreen)
        {
            entityScreen = _applicationStateSetter.SetSelectedEntityScreen(entityScreen);
            _applicationStateSetter.SetCurrentTicketType(entityScreen != null ? _cacheService.GetTicketTypeById(entityScreen.TicketTypeId) : null);

            if (entityScreen != null)
            {
                if (entityScreen.DisplayMode == 1)
                    ActivateEntitySearcher(entityScreen);
                else if (entityScreen.DisplayMode == 2)
                    ActivateDashboard(entityScreen);
                else ActivateButtonSelector(entityScreen);
            }
            RaisePropertyChanged(() => EntitySwitcherButtons);
            base.RaisePropertyChanged<bool>(Expression.Lambda<Func<bool>>(Expression.Property(Expression.Constant(this, typeof(EntitySwitcherViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(EntitySwitcherViewModel).GetMethod("get_CanDisplayCancelSelectionButton").MethodHandle)), new ParameterExpression[0]));
            base.RaisePropertyChanged<bool>(Expression.Lambda<Func<bool>>(Expression.Property(Expression.Constant(this, typeof(EntitySwitcherViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(EntitySwitcherViewModel).GetMethod("get_CanDisplayRemoveSelectionButton").MethodHandle)), new ParameterExpression[0]));
            EntitySwitcherButtons.ForEach(x => x.Refresh());
        }

        private void ActivateDashboard(EntityScreen entityScreen)
        {
            _entityDashboardViewModel.Refresh(entityScreen, _currentOperationRequest);
            _regionManager.ActivateRegion(RegionNames.EntityScreenRegion, _entityDashboardView);
        }

        private void ActivateEntitySearcher(EntityScreen entityScreen)
        {
            _entitySearchViewModel.Refresh(entityScreen.EntityTypeId, entityScreen.StateFilter, _currentOperationRequest);
            _regionManager.ActivateRegion(RegionNames.EntityScreenRegion, _entitySearchView);
        }

        private void ActivateButtonSelector(EntityScreen entityScreen)
        {
            _entitySelectorViewModel.Refresh(entityScreen, entityScreen.StateFilter, _currentOperationRequest);
            _regionManager.ActivateRegion(RegionNames.EntityScreenRegion, _entitySelectorView);
        }
    }
}
