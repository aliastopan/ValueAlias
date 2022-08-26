using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ValueAlias
{
    public abstract class ValueAlias<T> : IComparable where T : ValueAlias<T>
    {
        public int Value { get; }
        public string Alias { get; }

        protected ValueAlias(int value, string alias)
        {
            Value = value;
            Alias = alias.ToLower();
        }

        public override string ToString() => Alias;
        public IReadOnlyCollection<T> List => Enumerate().ToList().AsReadOnly();

        public static T FromValue(int value)
        {
            return Enumerate().Single(prop => prop.Value == value);
        }

        public static T FromAlias(string alias)
        {
            return Enumerate().Single(prop => string.Equals(prop.Alias, alias, StringComparison.OrdinalIgnoreCase));
        }

        public int CompareTo(object other)
        {
            return Value.CompareTo(((ValueAlias<T>)other).Value);
        }

        internal static IEnumerable<T> Enumerate()
        {
            var bindings = BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly;
            return typeof(T).GetFields(bindings)
                .Select(field => field.GetValue(null))
                .Cast<T>();
        }
    }
}