using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Magentix.Domain.Models.Menus
{
    [DataContract]
    public class MenuItemTagValue : IEquatable<MenuItemTagValue>
    {
        [DataMember(Name = "TN")]
        public string TagName
        {
            get;
            set;
        }

        [DataMember(Name = "TV")]
        public string TagValue
        {
            get;
            set;
        }

        public MenuItemTagValue()
        {
        }

        public bool Equals(MenuItemTagValue other)
        {
            if (other == null)
            {
                return false;
            }
            if (other.TagName != this.TagName)
            {
                return false;
            }
            return other.TagValue == this.TagValue;
        }

        public override bool Equals(object obj)
        {
            MenuItemTagValue menuItemTagValue = obj as MenuItemTagValue;
            if (menuItemTagValue == null)
            {
                return false;
            }
            return this.Equals(menuItemTagValue);
        }

        public override int GetHashCode()
        {
            return string.Concat(this.TagName, "_", this.TagValue).GetHashCode();
        }
    }
}
