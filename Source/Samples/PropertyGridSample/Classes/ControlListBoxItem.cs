using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace PropertyGridSample
{
    internal class ControlListBoxItem
        : TreeViewItem, IEquatable<ControlListBoxItem>, IComparable<ControlListBoxItem>
    {
        private object? instance;
        private object? propInstance;
        private object? eventInstance;
        private readonly Type type;

        public ControlListBoxItem(Type type, object? instance = null)
        {
            this.type = type;
            this.instance = instance;
            Text = type.FullName ?? type.Name;

            if (Text.StartsWith("Alternet.UI."))
                Text = Text.Remove(0, 9);
        }

        public Type InstanceType => type;

        public bool HasTicks { get; set; }

        public bool HasMargins { get; set; }

        public object? EventInstance
        {
            get
            {
                if (eventInstance == null)
                    return Instance;
                return eventInstance;
            }

            set
            {
                eventInstance = value;
            }
        }

        public object? PropInstance
        {
            get
            {
                if (propInstance == null)
                    return Instance;
                return propInstance;
            }

            set
            {
                propInstance = value;
            }
        }

        public object Instance
        {
            get
            {
                instance ??= CreateInstance(type);

                return instance!;
            }
        }

        public static object CreateInstance(Type type)
        {
            var instance = Activator.CreateInstance(type);
            if (!ObjectInit.Actions.TryGetValue(
                type,
                out Action<Object>? action))
                return instance!;
            action(instance!);
            return instance!;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not ControlListBoxItem item)
                return false;
            return EqualsInternal(item);
        }

        public override int GetHashCode()
        {
            return type.GetHashCode();
        }

        public bool Equals(ControlListBoxItem? other) => EqualsInternal(other);

        public int CompareTo(ControlListBoxItem? other)
        {
            return string.Compare(type.FullName, other?.type.FullName);
        }

        private bool EqualsInternal(ControlListBoxItem? other)
        {
            if (other is null)
                return false;
            return type == other.type;
        }
    }
}
