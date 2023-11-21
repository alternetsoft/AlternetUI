using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace PropertyGridSample
{
    internal class ControlListBoxItem : TreeViewItem
    {
        private object? instance;
        private object? propInstance;
        private object? eventInstance;
        private readonly Type type;

        public ControlListBoxItem(Type type, object? instance = null)
            : base()
        {
            this.type = type;
            this.instance = instance;
            this.Text = type.Name;
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
            if (!ObjectInitializers.Actions.TryGetValue(
                type,
                out Action<Object>? action))
                return instance!;
            action(instance!);
            return instance!;
        }
    }
}
