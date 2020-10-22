using Magentix.Domain.Models.Tickets;
using Magentix.Infrastructure.Data;
using System;
using System.Runtime.CompilerServices;

namespace Magentix.Presentation.Services
{
    public class CurrentDepartmentData
    {
        public int Id
        {
            get
            {
                if (this.Model == null)
                {
                    return 0;
                }
                return this.Model.Id;
            }
        }

        public Department Model
        {
            get;
            set;
        }

        public string Name
        {
            get
            {
                if (this.Model == null)
                {
                    return "";
                }
                return this.Model.Name;
            }
        }

        public string PriceTag
        {
            get
            {
                if (this.Model == null)
                {
                    return "";
                }
                return this.Model.PriceTag;
            }
        }

        public int ScreenMenuId
        {
            get
            {
                if (this.Model == null)
                {
                    return 0;
                }
                return this.Model.ScreenMenuId;
            }
        }

        public int TicketCreationMethod
        {
            get
            {
                if (this.Model == null)
                {
                    return 0;
                }
                return this.Model.TicketCreationMethod;
            }
        }

        public int TicketTypeId
        {
            get
            {
                if (this.Model == null)
                {
                    return 0;
                }
                return this.Model.TicketTypeId;
            }
        }

        public CurrentDepartmentData()
        {
        }
    }
}
