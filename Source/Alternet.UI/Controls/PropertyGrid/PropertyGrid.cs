using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Specialized grid for editing properties - in other words name = value pairs.
    /// </summary>
    /// <remarks>
    /// List of ready-to-use property classes include strings, numbers, flag sets, fonts,
    /// colors and many others. It is possible, for example, to categorize properties,
    /// set up a complete tree-hierarchy, add more than two columns, and set
    /// arbitrary per-property attributes.
    /// </remarks>
    public class PropertyGrid : Control
    {
        private static readonly string nameAsLabel = Native.PropertyGrid.NameAsLabel;

        /// <summary>
        /// Defines default visual style for the newly created
        /// <see cref="PropertyGrid"/> controls.
        /// </summary>
        public static PropertyGridCreateStyle DefaultCreateStyle { get; set; }
            = PropertyGridCreateStyle.DefaultStyle;

        /// <summary>
        /// Defines visual style and behavior of the <see cref="PropertyGrid"/> control.
        /// </summary>
        /// <remarks>
        /// When this property is changed, control is recreated.
        /// </remarks>
        public PropertyGridCreateStyle CreateStyle
        {
            get
            {
                return (PropertyGridCreateStyle)Handler.CreateStyle;
            }

            set
            {
                Handler.CreateStyle = (int)value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control has a border.
        /// </summary>
        public bool HasBorder
        {
            get
            {
                return NativeControl.HasBorder;
            }

            set
            {
                NativeControl.HasBorder = value;
            }
        }

        /// <inheritdoc/>
        public override ControlId ControlKind => ControlId.PropertyGrid;

        internal new NativePropertyGridHandler Handler
        {
            get
            {
                CheckDisposed();
                return (NativePropertyGridHandler)base.Handler;
            }
        }

        internal Native.PropertyGrid NativeControl => Handler.NativeControl;

        private string CorrectPropName(string? name)
        {
            if (name is null)
                return Native.PropertyGrid.NameAsLabel;
            return name;
        }

        public IPropertyGridItem CreateStringProperty(
            string label,
            string? name = null,
            string? value = null)
        {
            value ??= string.Empty;
            var handle = NativeControl.CreateStringProperty(label, CorrectPropName(name), value!);
            return new PropertyGridItem(handle);
        }

        public IPropertyGridItem CreateBoolProperty(
            string label,
            string? name = null,
            bool value = false)
        {
            var handle = NativeControl.CreateBoolProperty(label, CorrectPropName(name), value);
            return new PropertyGridItem(handle);
        }

        public IPropertyGridItem CreateIntProperty(
            string label,
            string? name = null,
            long value = 0)
        {
            var handle = NativeControl.CreateIntProperty(label, CorrectPropName(name), value);
            return new PropertyGridItem(handle);
        }

        public IPropertyGridItem CreateFloatProperty(
            string label,
            string? name = null,
            double value = 0.0)
        {
            var handle = NativeControl.CreateFloatProperty(label, CorrectPropName(name), value);
            return new PropertyGridItem(handle);
        }

        public IPropertyGridItem CreateUIntProperty(
            string label,
            string? name = null,
            ulong value = 0)
        {
            var handle = NativeControl.CreateUIntProperty(label, CorrectPropName(name), value);
            return new PropertyGridItem(handle);
        }

        public IPropertyGridItem CreateLongStringProperty(
            string label,
            string? name = null,
            string? value = null)
        {
            value ??= string.Empty;
            var handle = NativeControl.CreateLongStringProperty(
                label,
                CorrectPropName(name),
                value);
            return new PropertyGridItem(handle);
        }

        public IPropertyGridItem CreateDateProperty(
            string label,
            string? name = null,
            DateTime? value = null)
        {
            DateTime dt;

            if (value == null)
                dt = DateTime.Now;
            else
                dt = value.Value;

            var handle = NativeControl.CreateDateProperty(
                label,
                CorrectPropName(name),
                dt);
            return new PropertyGridItem(handle);
        }

        public void Clear()
        {
            NativeControl.Clear();
        }

        public void Add(IPropertyGridItem prop)
        {
            var result = NativeControl.Append(prop.Handle);
        }

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler()
        {
            return GetEffectiveControlHandlerHactory().CreatePropertyGridHandler(this);
        }
    }
}