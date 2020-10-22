using Magentix.Domain.Models.Tickets;
using Magentix.Presentation.Services;
using Magentix.Presentation.Services.Common;
using Magentix.Services.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Magentix.Modules.TicketModule.ActionProcessors
{
    [Export(typeof(IActionType))]
    internal class CancelTicketPayments : ActionType
    {
        private readonly ITicketService _ticketService;

        [ImportingConstructor]
        public CancelTicketPayments(ITicketService ticketService)
        {
            this._ticketService = ticketService;
        }

        protected override string GetActionKey()
        {
            return "CancelTicketPayments";
        }

        protected override string GetActionName()
        {
            return "Cancel Ticket Payments";
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
                dataValue.Payments.ToList<Payment>().ForEach(new Action<Payment>(dataValue.RemovePayment));
                dataValue.ChangePayments.ToList<ChangePayment>().ForEach(new Action<ChangePayment>(dataValue.RemoveChangePayment));
                dataValue.PaidItems.ToList<PaidItem>().ForEach((PaidItem x) => dataValue.PaidItems.Remove(x));
                this._ticketService.RefreshAccountTransactions(dataValue);
                this._ticketService.RecalculateTicket(dataValue, false);
                dataValue.PublishEvent<Ticket>("RegenerateSelectedTicket");
            }
        }
    }
}