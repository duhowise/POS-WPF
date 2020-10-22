using Magentix.Domain.Models.Tickets;
using Magentix.Infrastructure.Helpers;
using Magentix.Services.Common;
using System;
using System.ComponentModel.Composition;

namespace Magentix.Modules.TicketModule.ActionProcessors
{
    [Export(typeof(IActionType))]
    internal class UpdateTicketDate : ActionType
    {
        public UpdateTicketDate()
        {
        }

        protected override string GetActionKey()
        {
            return "UpdateTicketDate";
        }

        protected override string GetActionName()
        {
            return "Update Ticket Date";
        }

        protected override object GetDefaultData()
        {
            return new { Date = "" };
        }

        public override void Process(ActionData actionData)
        {
            Ticket dataValue = actionData.GetDataValue<Ticket>("Ticket");
            string asString = actionData.GetAsString("Date");
            if (dataValue != null && !string.IsNullOrWhiteSpace(asString))
            {
                dataValue.Date = DateFuncParser.Parse(asString, dataValue.Date);
            }
        }
    }
}