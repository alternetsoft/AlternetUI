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
    /// Implements <see cref="UI.ComboBox"/> with attached label.
    /// </summary>
    [ControlCategory("Editors")]
    public partial class ComboBoxAndLabel : ControlAndLabel<ComboBox, Label>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComboBoxAndLabel"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public ComboBoxAndLabel(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComboBoxAndLabel"/> class.
        /// </summary>
        /// <param name="label">Label text.</param>
        public ComboBoxAndLabel(string label)
            : this()
        {
            Label.Text = label;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComboBoxAndLabel"/> class.
        /// </summary>
        public ComboBoxAndLabel()
        {
        }

        /// <summary>
        /// Occurs when <see cref="ComboBox.SelectedIndexChanged"/> event of the
        /// attached combo box control is changed.
        /// </summary>
        public event EventHandler? SelectedIndexChanged
        {
            add => ComboBox.SelectedIndexChanged += value;
            remove => ComboBox.SelectedIndexChanged -= value;
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

        /// <summary>
        /// Gets inner <see cref="ComboBox"/> control.
        /// </summary>
        [Browsable(false)]
        public ComboBox ComboBox => MainControl;

        /// <inheritdoc cref="ComboBox.SelectedItem"/>
        public virtual object? SelectedItem
        {
            get => ComboBox.SelectedItem;
            set => ComboBox.SelectedItem = value;
        }

        /// <inheritdoc cref="ComboBox.IsEditable"/>
        public virtual bool IsEditable
        {
            get => ComboBox.IsEditable;
            set => ComboBox.IsEditable = value;
        }

        /// <inheritdoc cref="ComboBox.SelectedIndex"/>
        public virtual int? SelectedIndex
        {
            get => ComboBox.SelectedIndex;
            set => ComboBox.SelectedIndex = value;
        }

        /// <inheritdoc cref="ListControl{T}.Items"/>
        [Content]
        public virtual BaseCollection<object> Items
        {
            get => ComboBox.Items;
            set => ComboBox.Items = value;
        }
    }
}
