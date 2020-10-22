using Magentix.Infrastructure.Data;

namespace Magentix.Domain.Models.Entities
{
    public class EntityScreenMap : AbstractMap
    {
        public int EntityScreenId
        {
            get;
            set;
        }

        public int Visibility
        {
            get;
            set;
        }

        public EntityScreenMap()
        {
        }

        public bool IsVisibleForPos()
        {
            if (this.Visibility == 0)
            {
                return true;
            }
            return this.Visibility == 1;
        }

        public bool IsVisibleForTicket()
        {
            if (this.Visibility == 0)
            {
                return true;
            }
            return this.Visibility == 2;
        }
    }
}
