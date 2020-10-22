﻿using System.Collections.Generic;
using Magentix.Infrastructure.Data;

namespace Magentix.Domain.Models.Automation
{
    public class AutomationCommand : EntityClass, IOrderable
    {
        public AutomationCommand()
        {
            _automationCommandMaps = new List<AutomationCommandMap>();
            FontSize = 30;
            ConfirmationType = 0;
        }

        public string ButtonHeader { get; set; }
        public string Color { get; set; }
        public int FontSize { get; set; }
        public string Values { get; set; }
        public bool ToggleValues { get; set; }
        public int SortOrder { get; set; }
        public int ConfirmationType { get; set; }

        private IList<AutomationCommandMap> _automationCommandMaps;
        public virtual IList<AutomationCommandMap> AutomationCommandMaps
        {
            get { return _automationCommandMaps; }
            set { _automationCommandMaps = value; }
        }

        public string UserString { get { return Name; } }
    }
}
