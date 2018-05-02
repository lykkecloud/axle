// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle
{
    using System;
    using System.Linq;
    using System.Reflection;

    internal static class AssemblyExtensions
    {
        public static string Attribute<T>(this ICustomAttributeProvider provider, Func<T, string> property)
        {
            var value = provider.GetCustomAttributes(typeof(T), false).Cast<T>().FirstOrDefault();
            return value == null ? string.Empty : property(value);
        }
    }
}
