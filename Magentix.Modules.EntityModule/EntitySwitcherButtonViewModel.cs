using Magentix.Domain.Models.Entities;
using Magentix.Localization.Properties;
using Magentix.Presentation.Common;
using Magentix.Presentation.Services;
using System;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;

namespace Magentix.Modules.EntityModule
{
    public class EntitySwitcherButtonViewModel : ObservableObject
    {
        public EntityScreen Model { get; set; }
        private readonly IApplicationState _applicationState;
        private readonly bool _displayActiveScreen;

        public EntitySwitcherButtonViewModel(EntityScreen model, IApplicationState applicationState, bool displayActiveScreen)
        {
            Model = model;
            _applicationState = applicationState;
            _displayActiveScreen = displayActiveScreen;
        }

        public string Caption { 
            get 
            {
                Assembly assembly = this.GetType().Assembly;
                ResourceManager resourceManager = new ResourceManager("Resources", assembly);

                string resourceID = Regex.Replace(Model.Name, @"\s+", "");
                string myString = "";
                switch (resourceID)
                {
                    case "AllTables":
                        myString = Resources.AllTables;
                        break;
                    case "CustomerSearch":
                        myString = Resources.CustomerSearch;
                        break;
                    case "CustomerTickets":
                        myString = Resources.CustomerTickets;
                        break;
                    default:
                        myString = Model.Name;
                        break;
                }
                
                return _applicationState.IsLandscape ? myString : myString.Replace(" ", "\r");
            } 
        }
        public string ButtonColor { get { return Model != _applicationState.SelectedEntityScreen || !_displayActiveScreen ? "Gainsboro" : "Gray"; } }
        public void Refresh() { RaisePropertyChanged(() => ButtonColor); }
    }
}
