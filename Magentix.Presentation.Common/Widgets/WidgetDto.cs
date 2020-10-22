using Magentix.Domain.Models.Entities;
using Magentix.Infrastructure;
using System;
using System.Runtime.CompilerServices;

namespace Magentix.Presentation.Common.Widgets
{
    public class WidgetDto : IMatchable
    {
        public double Angle
        {
            get;
            set;
        }

        public bool AutoRefresh
        {
            get;
            set;
        }

        public int AutoRefreshInterval
        {
            get;
            set;
        }

        public int CornerRadius
        {
            get;
            set;
        }

        public string CreatorName
        {
            get;
            set;
        }

        public int Height
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Properties
        {
            get;
            set;
        }

        public double Scale
        {
            get;
            set;
        }

        public int Width
        {
            get;
            set;
        }

        public int XLocation
        {
            get;
            set;
        }

        public int YLocation
        {
            get;
            set;
        }

        public int Zindex
        {
            get;
            set;
        }

        public WidgetDto()
        {
        }

        public bool Matches(object other)
        {
            return this.Matches(other as Widget);
        }

        public bool Matches(Widget other)
        {
            if (other == null)
            {
                return false;
            }
            if (!(this.Name == other.Name) || !(this.CreatorName == other.CreatorName))
            {
                return false;
            }
            return this.Properties == other.Properties;
        }
    }
}
