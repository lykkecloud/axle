// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

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
