using Microsoft.CSharp.RuntimeBinder;
using Magentix.Domain.Models.Tickets;
using Magentix.Infrastructure.Data;
using Magentix.Services.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Magentix.Modules.TicketModule.ActionProcessors
{
    [Export(typeof(IActionType))]
    internal class SelectOrders : ActionType
    {
        public SelectOrders()
        {
        }

        protected override string GetActionKey()
        {
            return "SelectOrders";
        }

        protected override string GetActionName()
        {
            return "Select Orders";
        }

        protected override object GetDefaultData()
        {
            return new { OrderState = "", OrderIds = "", SelectAll = false };
        }

        public override void Process(ActionData actionData)
        {
            Ticket dataValue = actionData.GetDataValue<Ticket>("Ticket");
            if (dataValue != null)
            {
                bool asBoolean = actionData.GetAsBoolean("SelectAll", false);
                dataValue.Orders.ToList<Order>().ForEach((Order x) => x.IsSelected = asBoolean);
                IDictionary<string, object> dataObject = (IDictionary<string, object>)((dynamic)actionData.DataObject);
                if (dataObject.ContainsKey("Order"))
                {
                    dataObject.Remove("Order");
                }
                string asString = actionData.GetAsString("OrderIds");
                if (!string.IsNullOrEmpty(asString))
                {
                    char[] chrArray = new char[] { ',' };
                    List<int> list = (
                        from x in asString.Split(chrArray)
                        where !string.IsNullOrEmpty(x)
                        select Convert.ToInt32(x)).ToList<int>();
                    foreach (Order order in dataValue.Orders)
                    {
                        order.IsSelected = list.Contains(order.Id);
                    }
                }
                string str = actionData.GetAsString("OrderState");
                if (!string.IsNullOrEmpty(str))
                {
                    string[] strArrays = str.Split(new char[] { ',' });
                    for (int i = 0; i < (int)strArrays.Length; i++)
                    {
                        string str1 = strArrays[i];
                        foreach (Order order1 in dataValue.Orders)
                        {
                            if (!str1.Contains<char>('='))
                            {
                                order1.IsSelected = order1.IsInState(str1);
                            }
                            else
                            {
                                string[] strArrays1 = str1.Split(new char[] { '=' });
                                order1.IsSelected = order1.IsInState(strArrays1[0], strArrays1[1]);
                            }
                        }
                    }
                }
                if (dataValue.Orders.Count<Order>((Order x) => x.IsSelected) == 1)
                {
                    dynamic obj = actionData.DataObject;
                    IList<Order> orders = dataValue.Orders;
                    obj.Order = orders.FirstOrDefault<Order>((Order x) => x.IsSelected);
                }
            }
        }
    }
}