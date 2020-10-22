﻿using System.ComponentModel.Composition;
using Magentix.Domain.Models.Tickets;
using Magentix.Infrastructure.Settings;
using Magentix.Localization.Properties;
using Magentix.Presentation.Common;
using Magentix.Presentation.Services;
using Magentix.Services;
using Magentix.Services.Common;

namespace Magentix.Modules.PaymentModule
{
    [Export]
    public class ReturningAmountViewModel : ObservableObject
    {
        private readonly ICacheService _cacheService;
        private readonly IApplicationState _applicationState;
        private readonly PaymentEditor _paymentEditor;

        [ImportingConstructor]
        public ReturningAmountViewModel(ICacheService cacheService, IApplicationState applicationState,
             PaymentEditor paymentEditor)
        {
            _cacheService = cacheService;
            _applicationState = applicationState;
            _paymentEditor = paymentEditor;
        }

        private string _returningAmount;
        public string ReturningAmount
        {
            get { return _returningAmount; }
            set { _returningAmount = value; RaisePropertyChanged(() => ReturningAmount); }
        }

        public decimal GetReturningAmount(decimal tenderedAmount, decimal paymentDueAmount, ChangePaymentType changeTemplate)
        {
            var returningAmount = 0m;

            if (tenderedAmount > paymentDueAmount)
            {
                ReturningAmount = "";
                returningAmount = (tenderedAmount - paymentDueAmount);
                if (changeTemplate != null)
                {
                    var currency = _cacheService.GetCurrencyById(changeTemplate.Account.ForeignCurrencyId);
                    ReturningAmount = string.Format(Resources.ChangeAmount_f,
                            currency != null
                                ? string.Format(currency.CurrencySymbol, returningAmount / currency.ExchangeRate)
                                : returningAmount.ToString(LocalSettings.CurrencyFormat));
                }
            }

            if (string.IsNullOrEmpty(ReturningAmount))
                ReturningAmount = string.Format(Resources.ChangeAmount_f,
                    (returningAmount / _paymentEditor.ExchangeRate).ToString(LocalSettings.CurrencyFormat));

            if (returningAmount != 0)
            {
                _applicationState.NotifyEvent(RuleEventNames.ChangeAmountChanged,
                                               new
                                               {
                                                   Ticket = _paymentEditor.SelectedTicket,
                                                   TicketAmount = _paymentEditor.SelectedTicket.TotalAmount,
                                                   ChangeAmount = returningAmount,
                                                   TenderedAmount = tenderedAmount
                                               });
            }
            return returningAmount;
        }
    }
}
