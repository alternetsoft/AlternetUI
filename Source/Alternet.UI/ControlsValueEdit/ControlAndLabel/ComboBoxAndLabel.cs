using System;
using System.Collections.Generic;
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
        public object? SelectedItem
        {
            get => ComboBox.SelectedItem;
            set => ComboBox.SelectedItem = value;
        }

        /// <inheritdoc cref="ComboBox.IsEditable"/>
        public bool IsEditable
        {
            get => ComboBox.IsEditable;
            set => ComboBox.IsEditable = value;
        }

        /// <inheritdoc cref="ComboBox.SelectedIndex"/>
        public int? SelectedIndex
        {
            get => ComboBox.SelectedIndex;
            set => ComboBox.SelectedIndex = value;
        }

        /// <inheritdoc cref="ListControl.Items"/>
        [Content]
        public Collection<object> Items
        {
            get => ComboBox.Items;
            set => ComboBox.Items = value;
        }

        /// <inheritdoc/>
        protected override Control CreateControl() => new ComboBox();
    }
}
