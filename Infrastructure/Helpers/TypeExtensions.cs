using System;
using System.Linq;
using System.Reflection;

namespace Infrastructure.Helpers
{
    public static class TypeExtensions
    {
        public static PropertyInfo[] GetFilteredProperties(this Type type)
        {
            return type.GetProperties().
                Where(pi => !Attribute.IsDefined(pi, typeof(SkipPropertyAttribute))).ToArray();
        }
    }
}
