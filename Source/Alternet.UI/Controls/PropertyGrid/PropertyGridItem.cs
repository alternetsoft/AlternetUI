using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Alternet.Base.Collections;

namespace Alternet.UI
{
    internal class PropertyGridItem : IPropertyGridItem
    {
        private readonly IntPtr handle;
        private readonly string defaultName;
        private readonly string defaultLabel;
        private readonly object? defaultValue;
        private readonly PropertyGrid owner;
        private bool isCategory = false;
        private object? instance;
        private PropertyInfo? propInfo;
        private Collection<IPropertyGridItem>? children;
        private IPropertyGridItem? parent;

        public PropertyGridItem(
            PropertyGrid owner,
            IntPtr handle,
            string label,
            string? name,
            object? defaultValue)
        {
            this.owner = owner;
            this.handle = handle;
            this.defaultLabel = label;
            this.defaultValue = defaultValue;
            if (name == PropertyGrid.NameAsLabel || name == null)
                this.defaultName = label;
            else
                this.defaultName = name;
        }

        public event EventHandler? PropertyChanged;

        public object? UserData { get; set; }

        public IPropertyGrid Owner { get => owner; }

        public string PropertyEditorKind { get; set; } = "Other";

        public IPropertyGridItem? Parent => parent;

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
                if(children == null)
                {
                    children = new Collection<IPropertyGridItem>();
                    children.ItemInserted += Children_ItemInserted;
                    children.ItemRemoved += Children_ItemRemoved;
                }

                return children;
            }
        }

        public object? Instance
        {
            get => instance;
            set => instance = value;
        }

        public PropertyInfo? PropInfo
        {
            get => propInfo;
            set => propInfo = value;
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

        public static int CompareByLabel(IPropertyGridItem x, IPropertyGridItem y)
        {
            return string.Compare(x.DefaultLabel, y.DefaultLabel);
        }

        public void RaisePropertyChanged()
        {
            PropertyChanged?.Invoke(this, EventArgs.Empty);
        }

        private void Children_ItemRemoved(
            object? sender,
            CollectionChangeEventArgs<IPropertyGridItem> e)
        {
            if (e.Item is not PropertyGridItem item)
                return;
            item.parent = null;
        }

        private void Children_ItemInserted(
            object? sender,
            CollectionChangeEventArgs<IPropertyGridItem> e)
        {
            if (e.Item is not PropertyGridItem item)
                return;
            item.parent = this;
        }
    }
}
