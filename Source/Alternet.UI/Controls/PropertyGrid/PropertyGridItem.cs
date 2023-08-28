using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        private object? instance;
        private PropertyInfo? propInfo;
        private IList<IPropertyGridItem>? children;

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

        public bool HasChildren
        {
            get
            {
                return children != null && children.Count > 0;
            }
        }

        public IList<IPropertyGridItem> Children
        {
            get
            {
                children ??= new List<IPropertyGridItem>();
                return children;
            }
        }

        public object? Instance
        {
            get => instance;
            internal set => instance = value;
        }

        public PropertyInfo? PropInfo
        {
            get => propInfo;
            internal set => propInfo = value;
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
