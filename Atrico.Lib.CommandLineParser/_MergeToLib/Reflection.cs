using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace Atrico.Lib.Common.Reflection
{
    public static class ReflectionHelpers
    {
        public static T GetCustomAttribute<T>(this MemberInfo member, bool inherit = false) where T : Attribute
        {
            return member.GetCustomAttributes(typeof(T), inherit).FirstOrDefault() as T;
        }
    }
}
