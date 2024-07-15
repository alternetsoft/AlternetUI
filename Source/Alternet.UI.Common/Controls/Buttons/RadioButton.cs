using System;
using System.ComponentModel;

namespace Alternet.UI
{
    /// <summary>
    /// Enables the user to select a single option from a group of choices when
    /// paired with other <see cref="RadioButton"/> controls.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When the user selects one radio button (also known as an option button)
    /// within a group, the others clear automatically.
    /// All <see cref="RadioButton"/> controls in a given container, such as a
    /// <see cref="Window"/>, constitute a group.
    /// To create multiple groups on one window, place each group in its own
    /// container, such as a <see cref="GroupBox"/>.
    /// </para>
    /// <para>
    /// <see cref="RadioButton"/> and <see cref="CheckBox"/> controls have a
    /// similar function: they offer choices a user can select or clear.
    /// The difference is that multiple <see cref="CheckBox"/> controls can be
    /// selected at the same time, but option buttons are mutually exclusive.
    /// </para>
    /// <para>
    /// Use the <see cref="IsChecked"/> property to get or set the state of a
    /// <see cref="RadioButton"/>.
    /// </para>
    /// </remarks>
    [ControlCategory("Common")]
    public partial class RadioButton : ButtonBase
    {
        private bool? reportedChecked;

        /// <summary>
        /// Occurs when the value of the <see cref="IsChecked"/> property changes.
        /// </summary>
        public event EventHandler? CheckedChanged;

        /// <summary>
        /// Gets or sets a value indicating whether the control is checked.
        /// </summary>
        /// <value><c>true</c> if the radio button is checked; otherwise,
        /// <c>false</c>.</value>
        public virtual bool IsChecked
        {
            get => Handler.IsChecked;
            set
            {
                if (IsChecked == value)
                    return;
                Handler.IsChecked = value;
                RaiseSiblingsCheckedChanged();
            }
        }

        /// <summary>
        /// Gets a <see cref="IRadioButtonHandler"/> associated with this class.
        /// </summary>
        [Browsable(false)]
        public new IRadioButtonHandler Handler => (IRadioButtonHandler)base.Handler;

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.RadioButton;

        [Browsable(false)]
        internal new string Title
        {
            get => base.Title;
            set => base.Title = value;
        }

        [Browsable(false)]
        internal new LayoutStyle? Layout
        {
            get => base.Layout;
            set => base.Layout = value;
        }

        /// <inheritdoc/>
        protected override IControlHandler CreateHandler()
        {
            return ControlFactory.Handler.CreateRadioButtonHandler(this);
        }

        /// <summary>
        /// Called when the value of the <see cref="IsChecked"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        protected virtual void OnCheckedChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Raises the <see cref="CheckedChanged"/> event and calls
        /// <see cref="OnCheckedChanged(EventArgs)"/>.
        /// </summary>
        protected virtual void RaiseCheckedChanged()
        {
            var newChecked = IsChecked;

            if (reportedChecked == newChecked)
                return;
            reportedChecked = newChecked;
            OnCheckedChanged(EventArgs.Empty);
            CheckedChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Calls <see cref="RaiseCheckedChanged"/> for all sibling <see cref="RadioButton"/> controls.
        /// </summary>
        protected virtual void RaiseSiblingsCheckedChanged()
        {
            var siblings = Parent?.Children;

            if (siblings is null || siblings.Count == 0)
            {
                RaiseCheckedChanged();
                return;
            }

            foreach (var sibling in siblings)
            {
                if (sibling is not RadioButton radioButton)
                    continue;
                radioButton.RaiseCheckedChanged();
            }
        }

        /// <inheritdoc/>
        protected override void BindHandlerEvents()
        {
            base.BindHandlerEvents();
            Handler.CheckedChanged = RaiseCheckedChanged;
        }

        /// <inheritdoc/>
        protected override void UnbindHandlerEvents()
        {
            base.UnbindHandlerEvents();
            Handler.CheckedChanged = null;
        }
    }
}