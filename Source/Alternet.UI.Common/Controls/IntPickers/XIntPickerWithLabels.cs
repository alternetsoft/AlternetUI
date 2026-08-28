using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// A control that combines an <see cref="XIntPicker"/> with optional prefix and suffix labels.
    /// </summary>
    public partial class XIntPickerWithLabels : TransparentPanel
    {
        /// <summary>
        /// The default margin between the label and the picker control.
        /// </summary>
        public static float DefaultLabelAndPickerMargin = 5;

        private readonly XIntPicker intPicker;
        private readonly Label suffixLabel;
        private readonly Label prefixLabel;

        /// <summary>
        /// Initializes a new instance of the <see cref="XIntPickerWithLabels"/> class.
        /// </summary>
        public XIntPickerWithLabels()
        {
            intPicker = CreateIntPicker();
            suffixLabel = CreateLabel();
            prefixLabel = CreateLabel();

            Layout = LayoutStyle.Horizontal;

            prefixLabel.VerticalAlignment = VerticalAlignment.Center;
            prefixLabel.MarginRight = DefaultLabelAndPickerMargin;
            prefixLabel.Parent = this;

            intPicker.VerticalAlignment = VerticalAlignment.Center;
            intPicker.ValueChanged += OnPickerValueChanged;
            OnPickerValueChanged(intPicker, EventArgs.Empty);
            intPicker.Parent = this;

            suffixLabel.VerticalAlignment = VerticalAlignment.Center;
            suffixLabel.MarginLeft = DefaultLabelAndPickerMargin;
            suffixLabel.Parent = this;
        }

        /// <summary>
        /// Occurs when the value of the <see cref="XIntPicker"/> control changes.
        /// </summary>
        public event EventHandler? ValueChanged
        {
            add
            {
                intPicker.ValueChanged += value;
            }

            remove
            {
                intPicker.ValueChanged -= value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum value of the <see cref="XIntPicker"/> control.
        /// </summary>
        public int Maximum
        {
            get => intPicker.Maximum;
            set => intPicker.Maximum = value;
        }

        /// <summary>
        /// Gets or sets the maximum value of the <see cref="XIntPicker"/> control.
        /// </summary>
        public int Minimum
        {
            get => intPicker.Minimum;
            set => intPicker.Minimum = value;
        }

        /// <summary>
        /// Gets or sets the value of the <see cref="XIntPicker"/> control.
        /// </summary>
        public int Value
        {
            get => intPicker.Value;
            set => intPicker.Value = value;
        }

        /// <summary>
        /// Gets or sets the text displayed in the prefix label of the <see cref="XIntPickerWithLabels"/> control.
        /// </summary>
        public virtual string PrefixText
        {
            get => prefixLabel.Text;
            set
            {
                if (PrefixText == value) return;
                prefixLabel.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the text displayed in the suffix label of the <see cref="XIntPickerWithLabels"/> control.
        /// </summary>
        public virtual string SuffixText
        {
            get => suffixLabel.Text;
            set
            {
                if (SuffixText == value) return;
                suffixLabel.Text = value;
            }
        }

        /// <summary>
        /// Gets the <see cref="XIntPicker"/> control contained within this <see cref="XIntPickerWithLabels"/> instance.
        /// </summary>
        public XIntPicker IntPicker => intPicker;

        /// <summary>
        /// Gets the <see cref="Label"/> control that serves as the suffix label for the <see cref="XIntPicker"/> control.
        /// </summary>
        public Label SuffixLabel => suffixLabel;

        /// <summary>
        /// Gets the <see cref="Label"/> control that serves as the prefix label for the <see cref="XIntPicker"/> control.
        /// </summary>
        public Label PrefixLabel => prefixLabel;

        /// <summary>
        /// Creates a new instance of the <see cref="Label"/> control
        /// to be used as the suffix label for the <see cref="XIntPicker"/> control.
        /// </summary>
        /// <returns> A new instance of the <see cref="Label"/> control. </returns>
        protected virtual Label CreateLabel()
        {
            return new Label();
        }

        /// <summary>
        /// Called when the value of the <see cref="XIntPicker"/> control changes.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnPickerValueChanged(object? sender, EventArgs e)
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="XIntPicker"/> control.
        /// </summary>
        /// <returns> A new instance of the <see cref="XIntPicker"/> control. </returns>
        protected virtual XIntPicker CreateIntPicker()
        {
            return new XIntPicker();
        }
    }
}
