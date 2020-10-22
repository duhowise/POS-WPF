using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Magentix.Infrastructure
{
    public static class ActionServices
    {
        private readonly static IDictionary<ActionServiceType, IList<Action>> Actions;

        static ActionServices()
        {
            ActionServices.Actions = new Dictionary<ActionServiceType, IList<Action>>();
        }

        public static void Execute(ActionServiceType serviceType)
        {
            if (ActionServices.Actions.ContainsKey(serviceType))
            {
                foreach (Action item in ActionServices.Actions[serviceType])
                {
                    item();
                }
            }
        }

        public static void Register(ActionServiceType serviceType, Action action)
        {
            if (!ActionServices.Actions.ContainsKey(serviceType))
            {
                ActionServices.Actions.Add(serviceType, new List<Action>());
            }
            ActionServices.Actions[serviceType].Add(action);
        }
    }
}
