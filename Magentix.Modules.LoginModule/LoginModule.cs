﻿using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Regions;
using Magentix.Domain.Models.Users;
using Magentix.Localization.Properties;
using Magentix.Presentation.Common;
using Magentix.Presentation.Services;
using Magentix.Presentation.Services.Common;

namespace Magentix.Modules.LoginModule
{
    [ModuleExport(typeof(LoginModule))]
    public class LoginModule : VisibleModuleBase
    {
        readonly IRegionManager _regionManager;
        private readonly LoginView _loginView;
        private readonly IUserService _userService;

        [ImportingConstructor]
        public LoginModule(IRegionManager regionManager, LoginView loginView, IUserService userService)
            : base(regionManager, AppScreens.LoginScreen)
        {
            _regionManager = regionManager;
            _loginView = loginView;
            _userService = userService;
            SetNavigationCommand(Resources.Logout, Resources.Common, "Images/Logout.png", 99);
        }

        protected override bool CanNavigate(string arg)
        {
            return true;
        }

        protected override void OnNavigate(string obj)
        {
            _userService.LogoutUser();
        }

        protected override void OnInitialization()
        {
            _regionManager.RegisterViewWithRegion("MainRegion", typeof(LoginView));

            EventServiceFactory.EventService.GetEvent<GenericEvent<User>>().Subscribe(
                x =>
                {
                    if (x.Topic == EventTopicNames.UserLoggedOut)
                        Activate();
                });

            EventServiceFactory.EventService.GetEvent<GenericEvent<EventAggregator>>().Subscribe(
                x =>
                {
                    if (x.Topic == EventTopicNames.ShellInitialized) Activate();
                });

            EventServiceFactory.EventService.GetEvent<GenericEvent<string>>().Subscribe(
                x =>
                {
                    if (x.Topic == EventTopicNames.PinSubmitted)
                        PinEntered(x.Value);
                });
        }

        public void PinEntered(string pin)
        {
            _userService.LoginUser(pin);
        }

        public override object GetVisibleView()
        {
            return _loginView;
        }
    }
}
