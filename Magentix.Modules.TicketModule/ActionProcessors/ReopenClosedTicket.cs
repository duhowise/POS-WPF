using Magentix.Domain.Models.Tickets;
using Magentix.Services.Common;
using System;
using System.ComponentModel.Composition;

namespace Magentix.Modules.TicketModule.ActionProcessors
{
    [Export(typeof(IActionType))]
    internal class ReopenClosedTicket : ActionType
    {
        public ReopenClosedTicket()
        {
        }

        protected override string GetActionKey()
        {
            return "ReopenClosedTicket";
        }

        protected override string GetActionName()
        {
            return "Reopen Closed Ticket";
        }

        protected override object GetDefaultData()
        {
            return new object();
        }

        public override void Process(ActionData actionData)
        {
            Ticket dataValue = actionData.GetDataValue<Ticket>("Ticket");
            if (dataValue != null)
            {
                dataValue.IsClosed = false;
            }
        }
    }
}