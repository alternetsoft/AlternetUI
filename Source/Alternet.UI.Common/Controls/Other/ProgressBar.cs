using System;
using System.ComponentModel;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a progress bar control.
    /// </summary>
    /// <remarks>
    /// A <see cref="ProgressBar"/> control visually indicates the progress of a lengthy operation.
    /// <para>
    /// The <see cref="Maximum"/> and <see cref="Minimum"/> properties define the range of values to
    /// represent the progress of a task. The <see cref="Minimum"/> property is typically set
    /// to a value of 0,
    /// and the <see cref="Maximum"/> property is typically set to a value indicating the
    /// completion of a task.
    /// For example, to properly display the progress when copying a group of files,
    /// the <see cref="Maximum"/> property could be set to the total number of files to be copied.
    /// </para>
    /// <para>
    /// The <see cref="Value"/> property represents the progress that the application has made
    /// toward completing
    /// the operation. The value displayed by the <see cref="ProgressBar"/> only approximates
    /// the current value of the Value property.
    /// Based on the size of the <see cref="ProgressBar"/>, the <see cref="Value"/> property
    /// determines when to increase the size of
    /// the visually highlighted bar.
    /// </para>
    /// </remarks>
    [DefaultProperty("Value")]
    [DefaultBindingProperty("Value")]
    [ControlCategory("Common")]
    public partial class ProgressBar : Control
    {
        private bool isIndeterminate;
        private int minimum;
        private int maximum = 100;
        private ProgressBarOrientation orientation;
        private int val;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressBar"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public ProgressBar(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressBar"/> class.
        /// </summary>
        public ProgressBar()
        {
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Value"/> property changes.
        /// </summary>
        public event EventHandler? ValueChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Minimum"/> property changes.
        /// </summary>
        public event EventHandler? MinimumChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Orientation"/> property changes.
        /// </summary>
        public event EventHandler? OrientationChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Maximum"/> property changes.
        /// </summary>
        public event EventHandler? MaximumChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Maximum"/> property changes.
        /// </summary>
        public event EventHandler? IsIndeterminateChanged;

        /// <summary>
        /// Gets or sets whether the <see cref="ProgressBar"/> shows actual values or generic,
        /// continuous progress
        /// feedback.
        /// </summary>
        /// <value><see langword="false"/> if the <see cref="ProgressBar"/> shows actual values;
        /// true if the <see
        /// cref="ProgressBar"/> shows generic progress. The default is
        /// <see langword="false"/>.</value>
        public virtual bool IsIndeterminate
        {
            get => isIndeterminate;
            set
            {
                if (DisposingOrDisposed)
                    return;
                if (isIndeterminate == value)
                    return;
                isIndeterminate = value;
                IsIndeterminateChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the horizontal or vertical orientation of the
        /// progress bar.
        /// </summary>
        /// <value>One of the <see cref="ProgressBarOrientation"/> values.</value>
        public virtual ProgressBarOrientation Orientation
        {
            get => orientation;
            set
            {
                if (DisposingOrDisposed)
                    return;
                if (orientation == value)
                    return;
                orientation = value;
                OrientationChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the current position of the progress bar.
        /// </summary>
        /// <value>The position within the range of the progress bar. The default is 0.</value>
        /// <remarks>
        /// The minimum and maximum values of the Value property are specified by the
        /// <see cref="Minimum"/> and <see cref="Maximum"/> properties.
        /// When the <see cref="Value"/> property is set, the new value is validated
        /// to be between the <see cref="Minimum"/> and <see cref="Maximum"/> values.</remarks>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The value specified is greater than the value of the <see cref="Maximum"/>
        /// property or the value specified is less than the value of the <see cref="Minimum"/>
        /// property.
        /// </exception>
        public virtual int Value
        {
            get
            {
                return val;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;

                value = CoerceValue(value);
                if (this.val == value)
                    return;
                this.val = value;
                RaiseValueChanged(EventArgs.Empty);
            }
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.ProgressBar;

        /// <summary>
        /// Gets or sets the minimum allowed value for the progress bar control.
        /// </summary>
        /// <value>The minimum allowed value for the progress bar control. The default value
        /// is 0.</value>
        /// <remarks>
        /// The minimum and maximum values of the Value property are specified by the
        /// <see cref="Minimum"/> and <see cref="Maximum"/> properties.
        ///  If the new <see cref="Minimum"/> property value is greater than the
        ///  <see cref="Maximum"/> property value,
        ///  the <see cref="Maximum"/> value is set equal to the <see cref="Minimum"/> value.
        ///  If the <see cref="Value"/> is less than the new <see cref="Minimum"/> value,
        ///  the <see cref="Value"/> property
        ///  is also set equal to the <see cref="Minimum"/> value.
        /// </remarks>
        public virtual int Minimum
        {
            get
            {
                return minimum;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                if (minimum == value)
                    return;

                minimum = value;

                if (value > Maximum)
                    Maximum = value;
                if (Value < value)
                    Value = value;

                MinimumChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the maximum allowed value for the progress bar control.
        /// </summary>
        /// <value>The maximum allowed value for the progress bar control. The default value
        /// is 100.</value>
        /// <remarks>
        ///  If the <see cref="Minimum"/> property is greater than the new <see cref="Maximum"/>
        ///  property, the <see cref="Minimum"/>
        ///  property value is set equal to the <see cref="Maximum"/> value.
        ///  If the current <see cref="Value"/> is greater than the new <see cref="Maximum"/>
        ///  value, the <see cref="Value"/> property
        ///  value is set equal to the <see cref="Maximum"/> value.
        /// </remarks>
        public virtual int Maximum
        {
            get
            {
                return maximum;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                if (maximum == value)
                    return;
                maximum = value;
                MaximumChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        [Browsable(false)]
        internal new Thickness Padding
        {
            get => base.Padding;
            set => base.Padding = value;
        }

        [Browsable(false)]
        internal new LayoutStyle? Layout
        {
            get => base.Layout;
            set => base.Layout = value;
        }

        [Browsable(false)]
        internal new bool CanSelect
        {
            get => base.CanSelect;
            set => base.CanSelect = value;
        }

        [Browsable(false)]
        internal new bool ParentFont
        {
            get => base.ParentFont;
            set => base.ParentFont = value;
        }

        [Browsable(false)]
        internal new string Title
        {
            get => base.Title;
            set => base.Title = value;
        }

        [Browsable(false)]
        internal new bool ParentForeColor
        {
            get => base.ParentForeColor;
            set => base.ParentForeColor = value;
        }

        [Browsable(false)]
        internal new bool ParentBackColor
        {
            get => base.ParentBackColor;
            set => base.ParentBackColor = value;
        }

        [Browsable(false)]
        internal new bool TabStop
        {
            get => base.TabStop;
            set => base.TabStop = value;
        }

        [Browsable(false)]
        internal new bool IsBold
        {
            get => base.IsBold;
            set => base.IsBold = value;
        }

        [Browsable(false)]
        internal new Color? ForegroundColor
        {
            get => base.ForegroundColor;
            set => base.ForegroundColor = value;
        }

        [Browsable(false)]
        internal new Color? BackgroundColor
        {
            get => base.BackgroundColor;
            set => base.BackgroundColor = value;
        }

        [Browsable(false)]
        internal new Font? Font
        {
            get => base.Font;
            set => base.Font = value;
        }

        [Browsable(false)]
        internal new string Text
        {
            get => base.Text;
            set => base.Text = value;
        }

        /// <summary>
        /// Raises the <see cref="ValueChanged"/> event and calls
        /// <see cref="OnValueChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        public void RaiseValueChanged(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnValueChanged(e);
            ValueChanged?.Invoke(this, e);
        }

        /// <inheritdoc/>
        protected override IControlHandler CreateHandler()
        {
            return ControlFactory.Handler.CreateProgressBarHandler(this);
        }

        /// <summary>
        /// Called when the value of the <see cref="Value"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnValueChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Coerces value to have the valid range.
        /// </summary>
        /// <param name="value">Value to coerce.</param>
        /// <returns></returns>
        protected virtual int CoerceValue(int value)
        {
            if (value < Minimum)
                return Minimum;

            if (value > Maximum)
                return Maximum;
            return value;
        }
    }
}