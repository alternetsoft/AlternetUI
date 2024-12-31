using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Alternet.Base.Collections;

namespace Alternet.UI
{
    /// <summary>
    /// Default implementation of the <see cref="IPropertyGridItem"/> interface.
    /// </summary>
    public class PropertyGridItem : BaseControlItem, IPropertyGridItem
    {
        private readonly PropertyGridItemHandle? handle;
        private readonly string defaultName;
        private readonly string defaultLabel;
        private readonly object? defaultValue;
        private readonly IPropertyGrid owner;
        private readonly IPropertyGridNewItemParams? prm;
        private bool isCategory = false;
        private object? instance;
        private PropertyInfo? propInfo;
        private Collection<IPropertyGridItem>? children;
        private IPropertyGridItem? parent;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyGridItem"/> class.
        /// </summary>
        public PropertyGridItem(
            IPropertyGrid owner,
            PropertyGridItemHandle? handle,
            string label,
            string? name,
            object? defaultValue,
            IPropertyGridNewItemParams? prm)
        {
            this.owner = owner;
            this.handle = handle;
            this.defaultLabel = label;
            this.defaultValue = defaultValue;
            this.prm = prm;
            if (name == owner.GetPropNameAsLabel() || name == null)
                this.defaultName = label;
            else
                this.defaultName = name;
        }

        /// <inheritdoc/>
        public event EventHandler? PropertyChanged;

        /// <inheritdoc/>
        public event EventHandler? ButtonClick;

        /// <inheritdoc/>
        public virtual IPropertyGridNewItemParams? Params => prm;

        /// <inheritdoc/>
        public virtual IPropertyGridChoices? Choices { get; set; }

        /// <inheritdoc/>
        public virtual object? UserData { get; set; }

        /// <inheritdoc/>
        public virtual IPropertyGrid Owner { get => owner; }

        /// <inheritdoc/>
        public virtual bool CanHaveCustomEllipsis { get; set; } = true;

        /// <inheritdoc/>
        public PropertyGridEditKindAll PropertyEditorKind { get; set; } =
            PropertyGridEditKindAll.Other;

        /// <inheritdoc/>
        public virtual bool IsFlags => PropertyEditorKind == PropertyGridEditKindAll.EnumFlags;

        /// <inheritdoc/>
        public virtual IPropertyGridItem? Parent => parent;

        /// <inheritdoc/>
        public bool HasChildren
        {
            get
            {
                return children != null && children.Count > 0;
            }
        }

        /// <inheritdoc/>
        public virtual IList<IPropertyGridItem> Children
        {
            get
            {
                if(children == null)
                {
                    children = new();
                    children.ItemInserted += Children_ItemInserted;
                    children.ItemRemoved += Children_ItemRemoved;
                }

                return children;
            }
        }

        /// <inheritdoc/>
        public virtual object? Instance
        {
            get => instance;
            set => instance = value;
        }

        /// <inheritdoc/>
        public virtual PropertyInfo? PropInfo
        {
            get => propInfo;
            set => propInfo = value;
        }

        /// <inheritdoc/>
        public virtual PropertyGridItemHandle? Handle => handle;

        /// <inheritdoc/>
        public virtual string DefaultName => defaultName;

        /// <inheritdoc/>
        public virtual string DefaultLabel => defaultLabel;

        /// <inheritdoc/>
        public virtual object? DefaultValue => defaultValue;

        /// <inheritdoc/>
        public virtual Func<IPropertyGridItem?, object, PropertyInfo, object?>? GetValueFuncForReload
        { get; set; }

        /// <inheritdoc/>
        public virtual bool IsCategory
        {
            get => isCategory;
            set => isCategory = value;
        }

        /// <inheritdoc/>
        public virtual TypeConverter? TypeConverter { get; set; }

        /// <summary>
        /// Compares two specified <see cref="IPropertyGridItem"/> objects by their labels
        /// and returns an
        /// integer that indicates their relative position in the sort order.
        /// </summary>
        /// <param name="x">First item to compare.</param>
        /// <param name="y">Second item to compare.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relationship between the two
        /// comparands. Result value less than 0 means that <paramref name="x"/> precedes
        /// <paramref name="y"/> in the sort order.
        /// Result value equal to 0 means <paramref name="x"/> occurs in the same
        /// position as <paramref name="y"/>
        /// in the sort order. Result value greater than 0 means that <paramref name="x"/> follows
        /// <paramref name="y"/> in the sort order.
        /// </returns>
        public static int CompareByLabel(IPropertyGridItem x, IPropertyGridItem y)
        {
            return string.Compare(x.DefaultLabel, y.DefaultLabel);
        }

        /// <inheritdoc/>
        public void RaisePropertyChanged()
        {
            PropertyChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void RaiseButtonClick()
        {
            ButtonClick?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void AddChildren(IEnumerable<IPropertyGridItem> children)
        {
            foreach (var item in children)
                Children.Add(item);
        }

        private void Children_ItemRemoved(object? sender, int index, IPropertyGridItem item)
        {
            if (item is not PropertyGridItem sitem)
                return;
            sitem.parent = null;
        }

        private void Children_ItemInserted(object? sender, int index, IPropertyGridItem item)
        {
            if (item is not PropertyGridItem sitem)
                return;
            sitem.parent = this;
        }
    }
}
