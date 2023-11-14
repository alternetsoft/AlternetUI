using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class FlagsAndAttributes : AdvDictionary<string, object>, IFlagsAndAttributes
    {
        public bool HasFlag(string name) => HasAttribute(name);

        public void AddFlag(string name) => TryAdd(name, AssemblyUtils.True);

        public void RemoveFlag(string name) => RemoveAttribute(name);

        public void ToggleFlag(string name)
        {
            if (HasFlag(name))
                RemoveFlag(name);
            else
                AddFlag(name);
        }

        public void SetFlag(string name, bool value)
        {
            if (value)
                AddFlag(name);
            else
                RemoveFlag(name);
        }

        public bool HasAttribute(string name)
        {
            return ContainsKey(name);
        }

        public bool RemoveAttribute(string name)
        {
            return Remove(name);
        }

        public void SetAttribute(string name, object? value)
        {
            RemoveAttribute(name);
            if(value is not null)
                this[name] = value;
        }

        public object? GetAttribute(string name)
        {
            if (TryGetValue(name, out var result))
                return result;
            return null;
        }

        public T? GetAttribute<T>(string name)
        {
            return (T?)GetAttribute(name);
        }

        public T GetAttribute<T>(string name, T defaultValue)
        {
            if (TryGetValue(name, out var result))
                return (T)result;
            return defaultValue;
        }
    }
}
