using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Base.Collections;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control that contains a label and an <see cref="XIntPicker"/> control.
    /// </summary>
    [ControlCategory(KnownControlCategory.Editors)]
    public partial class UpDownAndLabel : GenericControlAndLabel<XIntPicker, Label>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpDownAndLabel"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public UpDownAndLabel(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpDownAndLabel"/> class.
        /// </summary>
        /// <param name="label">Label text.</param>
        public UpDownAndLabel(string label)
            : this()
        {
            Label.Text = label;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpDownAndLabel"/> class.
        /// </summary>
        public UpDownAndLabel()
        {
        }

        /// <summary>
        /// Occurs when value in the main inner control is changed.
        /// </summary>
        public event EventHandler? ValueChanged
        {
            add => MainControl.ValueChanged += value;
            remove => MainControl.ValueChanged -= value;
        }

        /// <summary>
        /// Gets or sets the value assigned to control.
        /// </summary>
        public virtual int Value
        {
            get
            {
                return MainControl.Value;
            }

            set
            {
                MainControl.Value = value;
            }
        }

        /// <summary>
        /// Gets or sets whether main inner control has border.
        /// </summary>
        public virtual bool HasInnerBorder
        {
            get
            {
                return MainControl.HasBorder;
            }

            set
            {
                MainControl.HasBorder = value;
            }
        }

        /// <inheritdoc cref="XIntPicker.IsEditable"/>
        public virtual bool IsEditable
        {
            get => MainControl.IsEditable;
            set => MainControl.IsEditable = value;
        }
    }
}
