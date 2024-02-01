using System;
using System.Collections.Generic;
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
        private readonly IntPtr handle;
        private readonly string defaultName;
        private readonly string defaultLabel;
        private readonly object? defaultValue;
        private readonly PropertyGrid owner;
        private readonly IPropertyGridNewItemParams? prm;
        private bool isCategory = false;
        private object? instance;
        private PropertyInfo? propInfo;
        private Collection<IPropertyGridItem>? children;
        private IPropertyGridItem? parent;
        private IFlagsAndAttributes? flagsAndAttributes;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyGridItem"/> class.
        /// </summary>
        internal PropertyGridItem(
            PropertyGrid owner,
            IntPtr handle,
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
            if (name == PropertyGrid.NameAsLabel || name == null)
                this.defaultName = label;
            else
                this.defaultName = name;
        }

        /// <inheritdoc/>
        public event EventHandler? PropertyChanged;

        /// <inheritdoc/>
        public event EventHandler? ButtonClick;

        /// <inheritdoc/>
        public IFlagsAndAttributes FlagsAndAttributes
        {
            get
            {
                return flagsAndAttributes ??= Factory.CreateFlagsAndAttributes();
            }
        }

        /// <inheritdoc/>
        public IPropertyGridNewItemParams? Params => prm;

        /// <inheritdoc/>
        public IPropertyGridChoices? Choices { get; set; }

        /// <inheritdoc/>
        public object? UserData { get; set; }

        /// <inheritdoc/>
        public IPropertyGrid Owner { get => owner; }

        /// <inheritdoc/>
        public bool CanHaveCustomEllipsis { get; set; } = true;

        /// <inheritdoc/>
        public PropertyGridEditKindAll PropertyEditorKind { get; set; } =
            PropertyGridEditKindAll.Other;

        /// <inheritdoc/>
        public bool IsFlags => PropertyEditorKind == PropertyGridEditKindAll.EnumFlags;

        /// <inheritdoc/>
        public IPropertyGridItem? Parent => parent;

        /// <inheritdoc/>
        public bool HasChildren
        {
            get
            {
                return children != null && children.Count > 0;
            }
        }

        /// <inheritdoc/>
        public IList<IPropertyGridItem> Children
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
        public object? Instance
        {
            get => instance;
            set => instance = value;
        }

        /// <inheritdoc/>
        public PropertyInfo? PropInfo
        {
            get => propInfo;
            set => propInfo = value;
        }

        /// <inheritdoc/>
        public IntPtr Handle => handle;

        /// <inheritdoc/>
        public string DefaultName => defaultName;

        /// <inheritdoc/>
        public string DefaultLabel => defaultLabel;

        /// <inheritdoc/>
        public object? DefaultValue => defaultValue;

        /// <inheritdoc/>
        public Func<IPropertyGridItem?, object, PropertyInfo, object?>? GetValueFuncForReload
        { get; set; }

        /// <inheritdoc/>
        public bool IsCategory
        {
            get => isCategory;
            set => isCategory = value;
        }

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

        /// <summary>
        /// Compares two specified <see cref="EventInfo"/> objects by their names
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
        public static int CompareByName(EventInfo x, EventInfo y)
        {
            return string.Compare(x.Name, y.Name);
        }

        /// <summary>
        /// Compares two specified <see cref="PropertyInfo"/> objects by their names
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
        public static int CompareByName(PropertyInfo x, PropertyInfo y)
        {
            return string.Compare(x.Name, y.Name);
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
