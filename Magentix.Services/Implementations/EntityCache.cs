using Magentix.Domain.Models.Entities;
using Magentix.Infrastructure.Data;
using Magentix.Persistance.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Magentix.Services.Implementations
{
    internal class EntityCache
    {
        private readonly IDictionary<int, IEnumerable<Entity>> _cache = new Dictionary<int, IEnumerable<Entity>>();

        public EntityCache()
        {
        }

        public IEnumerable<Entity> GetEntities(int entityTypeId, string stateData)
        {
            IEnumerable<Entity> entities;
            Func<EntityStateValue, int> func = null;
            if (!this._cache.ContainsKey(entityTypeId))
            {
                IEnumerable<Entity> entities1 = Dao.Query<Entity>((Entity x) => x.EntityTypeId == entityTypeId, new Expression<Func<Entity, object>>[0]);
                this._cache.Add(entityTypeId, entities1);
            }
            string str = "";
            string str1 = "";
            if (!string.IsNullOrEmpty(stateData))
            {
                if (!stateData.Contains("="))
                {
                    str = "*";
                    str1 = stateData;
                }
                else
                {
                    string[] strArrays = stateData.Split(new char[] { '=' });
                    str = strArrays[0];
                    str1 = strArrays[1];
                }
            }
            if (string.IsNullOrEmpty(str))
            {
                return this._cache[entityTypeId];
            }
            using (IReadOnlyWorkspace readOnlyWorkspace = WorkspaceFactory.CreateReadOnly())
            {
                IQueryable<int> nums =
                    from x in readOnlyWorkspace.Queryable<Entity>()
                    where x.EntityTypeId == entityTypeId
                    select x into y
                    select y.Id;
                List<EntityStateValue> list = (
                    from x in readOnlyWorkspace.Queryable<EntityStateValue>()
                    where nums.Contains<int>(x.EntityId)
                    select x).ToList<EntityStateValue>();
                entities = this._cache[entityTypeId].Where<Entity>((Entity x) =>
                {
                    IEnumerable<EntityStateValue> entityStateValues =
                        from y in list
                        where y.IsInState(str, str1)
                        select y;
                    if (func == null)
                    {
                        func = (EntityStateValue y) => y.EntityId;
                    }
                    return entityStateValues.Select<EntityStateValue, int>(func).Contains<int>(x.Id);
                });
            }
            return entities;
        }

        public void Reset()
        {
            this._cache.Clear();
        }
    }
}
