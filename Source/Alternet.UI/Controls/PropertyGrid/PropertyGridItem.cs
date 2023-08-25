using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class PropertyGridItem : IPropertyGridItem
    {
        private readonly IntPtr handle;
        private readonly string defaultName;
        private readonly string defaultLabel;
        private readonly object? defaultValue;
        private bool isCategory = false;

        public PropertyGridItem(IntPtr handle, string label, string? name, object? defaultValue)
        {
            this.handle = handle;
            this.defaultLabel = label;
            this.defaultValue = defaultValue;
            if (name == PropertyGrid.NameAsLabel || name == null)
                this.defaultName = label;
            else
                this.defaultName = name;
        }

        public IntPtr Handle => handle;

        public string DefaultName => defaultName;

        public string DefaultLabel => defaultLabel;

        public object? DefaultValue => defaultValue;

        public bool IsCategory
        {
            get => isCategory;
            set => isCategory = value;
        }
    }
}
