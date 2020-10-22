using Omu.ValueInjecter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Magentix.Infrastructure.Data.Injection
{
    public class DtoCloneInjection : ConventionInjection
    {
        public DtoCloneInjection()
        {
        }

        protected override bool Match(ConventionInfo c)
        {
            if (c.SourceProp.Name != c.TargetProp.Name)
            {
                return false;
            }
            return c.SourceProp.Value != null;
        }

        protected override object SetValue(ConventionInfo c)
        {
            if (c.SourceProp.Type.IsValueType || c.SourceProp.Type == typeof(string))
            {
                return c.SourceProp.Value;
            }
            if (c.SourceProp.Type.IsArray)
            {
                Array value = c.SourceProp.Value as Array;
                Array arrays = value.Clone() as Array;
                for (int i = 0; i < value.Length; i++)
                {
                    object obj = value.GetValue(i);
                    if (!obj.GetType().IsValueType && !(obj is string))
                    {
                        object obj1 = Activator.CreateInstance(obj.GetType());
                        object[] objArray = new object[] { obj };
                        arrays.SetValue(obj1.InjectFrom<CloneInjection>(objArray), i);
                    }
                }
                return arrays;
            }
            if (!c.SourceProp.Type.IsGenericType)
            {
                object obj2 = Activator.CreateInstance(c.SourceProp.Type);
                object[] value1 = new object[] { c.SourceProp.Value };
                return obj2.InjectFrom<CloneInjection>(value1);
            }
            if (!c.SourceProp.Type.GetGenericTypeDefinition().GetInterfaces().Contains<Type>(typeof(IEnumerable)))
            {
                return c.SourceProp.Value;
            }
            Type genericArguments = c.TargetProp.Type.GetGenericArguments()[0];
            if (genericArguments.IsValueType || genericArguments == typeof(string))
            {
                return c.SourceProp.Value;
            }
            Type type = typeof(List<>).MakeGenericType(new Type[] { genericArguments });
            object obj3 = Activator.CreateInstance(type);
            MethodInfo method = type.GetMethod("Add");
            foreach (object value2 in c.SourceProp.Value as IEnumerable)
            {
                object obj4 = Activator.CreateInstance(genericArguments);
                object[] objArray1 = new object[] { value2 };
                object obj5 = obj4.InjectFrom<CloneInjection>(objArray1);
                object[] objArray2 = new object[] { obj5 };
                method.Invoke(obj3, objArray2);
            }
            return obj3;
        }
    }
}
