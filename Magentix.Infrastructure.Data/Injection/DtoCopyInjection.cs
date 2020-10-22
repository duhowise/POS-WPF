using Omu.ValueInjecter;
using Magentix.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;


namespace Magentix.Infrastructure.Data.Injection
{
    public class DtoCopyInjection : ConventionInjection
    {
        public DtoCopyInjection()
        {
        }

        protected override bool Match(ConventionInfo c)
        {
            bool name = c.SourceProp.Name == c.TargetProp.Name;
            bool value = c.SourceProp.Value != null;
            if (name)
            {
                return value;
            }
            return false;
        }

        protected override object SetValue(ConventionInfo c)
        {
            //DtoCopyInjection variable;
            //object obj;
            //object[] value;
            //object value1;
            //if (c.SourceProp.Type.IsValueType || c.SourceProp.Type == typeof(string))
            //{
            //    return c.SourceProp.Value;
            //}
            //if (!c.SourceProp.Type.IsGenericType)
            //{
            //    if (c.TargetProp.Value == null)
            //    {
            //        try
            //        {
            //            c.TargetProp.Value = Activator.CreateInstance(c.TargetProp.Type);
            //            value1 = c.TargetProp.Value;
            //            value = new object[] { c.SourceProp.Value };
            //            return value1.InjectFrom<EntityInjection>(value);
            //        }
            //        catch (Exception exception)
            //        {
            //            obj = null;
            //        }
            //        return obj;
            //    }
            //    value1 = c.TargetProp.Value;
            //    value = new object[] { c.SourceProp.Value };
            //    return value1.InjectFrom<EntityInjection>(value);
            //}
            //Type genericTypeDefinition = c.SourceProp.Type.GetGenericTypeDefinition();
            //if (genericTypeDefinition != null && genericTypeDefinition.GetInterfaces().Contains<Type>(typeof(IEnumerable)))
            //{
            //    Type genericArguments = c.TargetProp.Type.GetGenericArguments()[0];
            //    if (genericArguments.IsValueType || genericArguments == typeof(string))
            //    {
            //        return c.SourceProp.Value;
            //    }
            //    if (c.SourceProp.Type.GetGenericArguments()[0].GetInterfaces().Contains<Type>(typeof(IMatchable)))
            //    {
            //        foreach (IMatchable matchable in c.SourceProp.Value as IEnumerable<IMatchable>)
            //        {
            //            IMatchable matchable1 = matchable;
            //            if ((c.TargetProp.Value as IEnumerable<object>).SingleOrDefault<object>(new Func<object, bool>(matchable1.Matches)) != null)
            //            {
            //                continue;
            //            }
            //            object obj1 = Activator.CreateInstance(genericArguments);
            //            object[] objArray = new object[] { matchable };
            //            obj1.InjectFrom<DtoCopyInjection>(objArray);
            //            MethodInfo method = c.TargetProp.Value.GetType().GetMethod("Add");
            //            object value2 = c.TargetProp.Value;
            //            object[] objArray1 = new object[] { obj1 };
            //            method.Invoke(value2, objArray1);
            //        }
            //    }
            //    else if (((IEnumerable<PropertyInfo>)genericArguments.GetProperties()).Any<PropertyInfo>((PropertyInfo x) => x.Name == "Name"))
            //    {
            //        if (((IEnumerable<PropertyInfo>)c.SourceProp.Type.GetGenericArguments()[0].GetProperties()).Any<PropertyInfo>((PropertyInfo x) => x.Name == "Name"))
            //        {
            //            PropertyInfo propertyInfo = ((IEnumerable<PropertyInfo>)genericArguments.GetProperties()).Single<PropertyInfo>((PropertyInfo x) => x.Name == "Name");
            //            PropertyInfo propertyInfo1 = ((IEnumerable<PropertyInfo>)c.SourceProp.Type.GetGenericArguments()[0].GetProperties()).Single<PropertyInfo>((PropertyInfo x) => x.Name == "Name");
            //            MethodInfo methodInfo = c.TargetProp.Value.GetType().GetMethod("Remove");
            //            IEnumerable<object> objs = c.TargetProp.Value as IEnumerable<object>;
            //            IEnumerable<object> objs1 = c.SourceProp.Value as IEnumerable<object>;
            //            IEnumerable<object> objs2 = objs.Where<object>((object x) => return objs1.All<object>((object y) => propertyInfo.GetValue(x, null).ToString() != propertyInfo1.GetValue(y, null).ToString()));
            //            objs2.ToList<object>().ForEach((object x) => methodInfo.Invoke(c.TargetProp.Value, new object[] { x }));
            //            foreach (object obj2 in c.SourceProp.Value as IEnumerable<object>)
            //            {
            //                object obj3 = obj2;
            //                object obj4 = (c.TargetProp.Value as IEnumerable<object>).SingleOrDefault<object>((object z) => propertyInfo.GetValue(z, null).ToString() == propertyInfo1.GetValue(obj3, null).ToString());
            //                if (obj4 == null)
            //                {
            //                    if ((c.TargetProp.Value as IEnumerable<object>).Contains<object>(obj3))
            //                    {
            //                        continue;
            //                    }
            //                    obj3 = Activator.CreateInstance(genericArguments);
            //                    object obj5 = obj3;
            //                    object[] objArray2 = new object[] { obj2 };
            //                    obj5.InjectFrom<DtoCopyInjection>(objArray2);
            //                    MethodInfo method1 = c.TargetProp.Value.GetType().GetMethod("Add");
            //                    object value3 = c.TargetProp.Value;
            //                    object[] objArray3 = new object[] { obj3 };
            //                    method1.Invoke(value3, objArray3);
            //                }
            //                else
            //                {
            //                    object[] objArray4 = new object[] { obj3 };
            //                    obj4.InjectFrom<DtoCloneInjection>(objArray4);
            //                }
            //            }
            //        }
            //    }
            //}
            return c.TargetProp.Value;
        }
    }
}
