using Magentix.Domain.Models.Tickets;
using Magentix.Infrastructure.Data;
using Magentix.Infrastructure.Helpers;
using Magentix.Infrastructure.Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Magentix.Domain.Models.Menus
{
    public class MenuItem : EntityClass
    {
        private IList<MenuItemTagValue> _menuItemTagValues;

        private IList<MenuItemPortion> _portions;

        private static MenuItem _all;

        public static MenuItem All
        {
            get
            {
                MenuItem menuItem = MenuItem._all;
                if (menuItem == null)
                {
                    menuItem = new MenuItem()
                    {
                        Name = "*"
                    };
                    MenuItem._all = menuItem;
                }
                return menuItem;
            }
        }

        public string Barcode
        {
            get;
            set;
        }

        public string CustomTags
        {
            get;
            set;
        }

        public string GroupCode
        {
            get;
            set;
        }

        private IList<MenuItemTagValue> MenuItemTagValues
        {
            get
            {
                IList<MenuItemTagValue> menuItemTagValues = this._menuItemTagValues;
                if (menuItemTagValues == null)
                {
                    List<MenuItemTagValue> menuItemTagValues1 = JsonHelper.Deserialize<List<MenuItemTagValue>>(this.CustomTags);
                    IList<MenuItemTagValue> menuItemTagValues2 = menuItemTagValues1;
                    this._menuItemTagValues = menuItemTagValues1;
                    menuItemTagValues = menuItemTagValues2;
                }
                return menuItemTagValues;
            }
        }

        public virtual IList<MenuItemPortion> Portions
        {
            get
            {
                return this._portions;
            }
            set
            {
                this._portions = value;
            }
        }

        public string Tag
        {
            get;
            set;
        }

        public string UserString
        {
            get
            {
                return string.Format("{0} [{1}]", base.Name, this.GroupCode);
            }
        }

        public MenuItem() : this(string.Empty)
        {
        }

        public MenuItem(string name)
        {
            base.Name = name;
            this._portions = new List<MenuItemPortion>();
        }

        public static OrderTag AddDefaultMenuItemProperty(OrderTagGroup item)
        {
            return item.AddOrderTag("", new decimal(0));
        }

        public static MenuItemPortion AddDefaultMenuPortion(MenuItem item)
        {
            return item.AddPortion("Normal", new decimal(0), LocalSettings.CurrencySymbol);
        }

        public MenuItemPortion AddPortion(string portionName, decimal price, string currencyCode)
        {
            MenuItemPortion menuItemPortion = new MenuItemPortion()
            {
                Name = portionName,
                Price = price,
                MenuItemId = base.Id
            };
            MenuItemPortion menuItemPortion1 = menuItemPortion;
            this.Portions.Add(menuItemPortion1);
            return menuItemPortion1;
        }

        public static MenuItem Create()
        {
            MenuItem menuItem = new MenuItem();
            MenuItem.AddDefaultMenuPortion(menuItem);
            return menuItem;
        }

        public IEnumerable<MenuItemTagValue> GetCustomTagValues()
        {
            return this.MenuItemTagValues;
        }

        internal MenuItemPortion GetPortion(string portionName)
        {
            MenuItemPortion menuItemPortion;
            using (IEnumerator<MenuItemPortion> enumerator = this.Portions.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    MenuItemPortion current = enumerator.Current;
                    if (current.Name != portionName)
                    {
                        continue;
                    }
                    menuItemPortion = current;
                    return menuItemPortion;
                }
                if (!string.IsNullOrEmpty(portionName) || this.Portions.Count <= 0)
                {
                    throw new Exception("Portion not found.");
                }
                return this.Portions[0];
            }
            //return menuItemPortion;
        }

        public string GetTagValue(string tagName)
        {
            MenuItemTagValue menuItemTagValue = this.MenuItemTagValues.SingleOrDefault<MenuItemTagValue>((MenuItemTagValue x) => x.TagName == tagName);
            if (menuItemTagValue == null)
            {
                return "";
            }
            return menuItemTagValue.TagValue;
        }

        public bool IsGroupEquals(string groupName)
        {
            return this.GroupCode == groupName;
        }

        public bool IsTaggedWith(string tagName, string tagValue)
        {
            return this.GetTagValue(tagName) == tagValue;
        }

        public void SetTagValue(string tagName, string tagValue)
        {
            MenuItemTagValue menuItemTagValue = this.MenuItemTagValues.SingleOrDefault<MenuItemTagValue>((MenuItemTagValue x) => x.TagName == tagName);
            if (menuItemTagValue != null)
            {
                menuItemTagValue.TagValue = tagValue;
            }
            else
            {
                MenuItemTagValue menuItemTagValue1 = new MenuItemTagValue()
                {
                    TagName = tagName,
                    TagValue = tagValue
                };
                menuItemTagValue = menuItemTagValue1;
                this.MenuItemTagValues.Add(menuItemTagValue);
            }
            if (string.IsNullOrEmpty(menuItemTagValue.TagValue))
            {
                this.MenuItemTagValues.Remove(menuItemTagValue);
            }
            this.CustomTags = JsonHelper.Serialize<IList<MenuItemTagValue>>(this.MenuItemTagValues);
            this._menuItemTagValues = null;
        }
    }
}
