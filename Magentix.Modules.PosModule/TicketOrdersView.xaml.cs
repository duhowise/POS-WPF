﻿using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Controls;
using Microsoft.Practices.Prism.Events;
using Magentix.Domain.Models.Tickets;
using Magentix.Presentation.Services.Common;

namespace Magentix.Modules.PosModule
{
    /// <summary>
    /// Interaction logic for TicketOrdersView.xaml
    /// </summary>
    [Export]
    public partial class TicketOrdersView : UserControl
    {
        [ImportingConstructor]
        public TicketOrdersView(TicketOrdersViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();

            EventServiceFactory.EventService.GetEvent<GenericEvent<Order>>().Subscribe(
               x =>
               {
                   if (x.Topic == EventTopicNames.OrderAdded)
                       Scroller.ScrollToEnd();
               });


            EventServiceFactory.EventService.GetEvent<GenericEvent<EventAggregator>>().Subscribe(
                x =>
                {
                    if (x.Topic == EventTopicNames.ActivatePosView && !((TicketOrdersViewModel)DataContext).SelectedOrders.Any())
                    {
                        Scroller.ScrollToEnd();
                    }
                });
        }
    }
}
