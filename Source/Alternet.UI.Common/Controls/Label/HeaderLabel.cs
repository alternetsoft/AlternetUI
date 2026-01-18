using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements label control that looks like header.
    /// </summary>
    [ControlCategory("Other")]
    public partial class HeaderLabel : HiddenBorderedGraphicControl, ITextProperty
    {
        /// <summary>
        /// Gets or sets function which creates inner label.
        /// </summary>
        /// <remarks>
        /// Result of this function must support at least <see cref="ITextProperty"/>
        /// interface.
        /// </remarks>
        public static Func<AbstractControl> CreateInnerControl = CreateDefaultInnerControl;

        private readonly AbstractControl control;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderLabel"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public HeaderLabel(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderLabel"/> class.
        /// </summary>
        public HeaderLabel()
        {
            ParentBackColor = true;
            ParentForeColor = true;
            CanSelect = false;
            TabStop = false;
            control = CreateInnerControl?.Invoke() ?? CreateDefaultInnerControl();
            control.Parent = this;
        }

        /// <inheritdoc/>
        public override string Text
        {
            get
            {
                return (control as ITextProperty)?.Text ?? string.Empty;
            }

            set
            {
                if(control is ITextProperty instance)
                    instance.Text = value;
            }
        }

        /// <summary>
        /// Creates default inner label.
        /// </summary>
        public static AbstractControl CreateDefaultInnerControl()
        {
            var panel = new CardPanelHeader
            {
                TabHasBorder = false,
                Text = string.Empty,
            };
            panel.SelectFirstTab();
            return panel;
        }
    }
}
