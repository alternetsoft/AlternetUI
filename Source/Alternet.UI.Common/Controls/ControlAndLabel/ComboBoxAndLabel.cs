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
    /// Implements <see cref="UI.ComboBox"/> with attached <see cref="Label"/>.
    /// </summary>
    [ControlCategory("Editors")]
    public partial class ComboBoxAndLabel : ControlAndLabel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComboBoxAndLabel"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public ComboBoxAndLabel(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComboBoxAndLabel"/> class.
        /// </summary>
        /// <param name="label">Label text.</param>
        public ComboBoxAndLabel(string label)
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
        /// Gets main child control.
        /// </summary>
        [Browsable(false)]
        public new ComboBox MainControl => (ComboBox)base.MainControl;

        /// <summary>
        /// Gets main child control, same as <see cref="MainControl"/>.
        /// </summary>
        [Browsable(false)]
        public ComboBox ComboBox => (ComboBox)base.MainControl;

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
        public virtual IListControlItems<object> Items
        {
            get => ComboBox.Items;
            set => ComboBox.Items = value;
        }

        /// <inheritdoc/>
        public override void BindHandlerEvents()
        {
            base.BindHandlerEvents();
            Handler.TextChanged = null;
        }

        /// <inheritdoc/>
        protected override AbstractControl CreateControl() => new ComboBox();
    }
}
