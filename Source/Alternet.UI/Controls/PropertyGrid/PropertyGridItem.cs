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
        private readonly string name;
        private readonly string label;
        private readonly object? defaultValue;
        private bool isCategory = false;

        public PropertyGridItem(IntPtr handle, string label, string? name, object? defaultValue)
        {
            this.handle = handle;
            this.label = label;
            this.defaultValue = defaultValue;
            if (name == PropertyGrid.NameAsLabel || name == null)
                this.name = label;
            else
                this.name = name;
        }

        public IntPtr Handle => handle;

        public string Name => name;

        public string Label => label;

        public object? DefaultValue => defaultValue;

        public bool IsCategory
        {
            get => isCategory;
            set => isCategory = value;
        }
    }
}
