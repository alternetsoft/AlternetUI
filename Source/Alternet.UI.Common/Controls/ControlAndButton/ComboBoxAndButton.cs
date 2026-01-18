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
    /// Implements <see cref="UI.ComboBox"/> with additional buttons.
    /// </summary>
    [ControlCategory("Editors")]
    public partial class ComboBoxAndButton : ControlAndButton
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComboBoxAndButton"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public ComboBoxAndButton(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComboBoxAndLabel"/> class.
        /// </summary>
        public ComboBoxAndButton()
        {
            SuspendHandlerTextChange();
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
        public virtual BaseCollection<object> Items
        {
            get => ComboBox.Items;
            set => ComboBox.Items = value;
        }

        /// <inheritdoc/>
        protected override bool NeedDefaultButton() => false;

        /// <inheritdoc/>
        protected override AbstractControl CreateControl() => new ComboBox();
    }
}
