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

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler()
        {
            return GetEffectiveControlHandlerHactory().CreatePropertyGridHandler(this);
        }
    }
}