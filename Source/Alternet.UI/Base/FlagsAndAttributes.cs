using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal abstract class FLagsAndAttributes
    {
        public abstract bool HasFlag(string name);

        public abstract void AddFlag(string name);

        public abstract void RemoveFlag(string name);

        public abstract void ToggleFlag(string name);

        public abstract bool HasAttribute(string name);

        public abstract void RemoveAttribute(string name);

        public abstract void SetAttribute(string name, object? value);

        public abstract object? GetAttribute(string name);

        public abstract T GetAttribute<T>(string name, T defaultValue);
    }
}
