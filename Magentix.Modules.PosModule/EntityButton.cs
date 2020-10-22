using System;
using System.Linq;
using Magentix.Domain.Models.Entities;
using Magentix.Domain.Models.Tickets;
using Magentix.Infrastructure.Settings;
using Magentix.Localization.Properties;

namespace Magentix.Modules.PosModule
{
    public class EntityButton
    {
        private readonly Ticket _selectedTicket;
        public EntityButton(EntityType model, Ticket selectedTicket)
        {
            _selectedTicket = selectedTicket;
            Model = model;
        }

        public EntityType Model { get; set; }
        public string Name
        {
            get
            {
                var format = Resources.Select_f;
                if (_selectedTicket != null && _selectedTicket.TicketEntities.Any(x => x.EntityTypeId == Model.Id))
                    format = Resources.Change_f;
                string entityName = Model.EntityName;
                if (Model.EntityName == "Customer")
                    entityName = Resources.Customer;
                else if (Model.EntityName == "Table")
                    entityName = Resources.Table;
                return string.Format(format, entityName).Replace(" ", Environment.NewLine);
            }
        }

        public string ImagePath
        {
            get
            {
                if(Model.Name == "Tables")
                    return LocalSettings.AppPath + "\\Images\\Ticket.png";
                else if (Model.Name == "Customers")
                        return LocalSettings.AppPath + "\\Images\\Customer.png";
                else
                    return "";
            }
        }
    }
}