using System;
using System.ComponentModel;
using System.Linq;

namespace Alternet.UI
{
    /// <summary>
    /// Enables the user to select a single option from a group of choices when
    /// paired with other <see cref="StdRadioButton"/> controls.
    /// <see cref="StdRadioButton"/> represents a generic radio button control.
    /// This control is implemented inside the library and can be used in the
    /// same way as a regular native <see cref="RadioButton"/> control. <see cref="StdRadioButton"/> is used when you need
    /// to have the same look and behavior for all platforms. <see cref="StdRadioButton"/> provides many additional features,
    /// which are not available in the native <see cref="RadioButton"/> control.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When the user selects one radio button (also known as an option button)
    /// within a group, the others clear automatically.
    /// All <see cref="StdRadioButton"/> controls in a given container, such as a
    /// <see cref="Panel"/>, constitute a group.
    /// To create multiple groups on one window, place each group in its own
    /// container, such as a <see cref="Panel"/>.
    /// </para>
    /// <para>
    /// <see cref="StdRadioButton"/> and <see cref="StdCheckBox"/> controls have a
    /// similar function: they offer choices a user can select or clear.
    /// The difference is that multiple <see cref="StdCheckBox"/> controls can be
    /// selected at the same time, but option buttons are mutually exclusive.
    /// </para>
    /// </remarks>
    [ControlCategory("Common")]
    public partial class StdRadioButton : GenericListItemControl
    {
        private static int suppressSiblingNotifyCounter;

        /// <summary>
        /// Initializes a new instance of the <see cref="StdRadioButton"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public StdRadioButton(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StdRadioButton"/> class with the specified text.
        /// </summary>
        /// <param name="text"></param>
        public StdRadioButton(string text)
            : this()
        {
            Text = text;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StdRadioButton"/> class.
        /// </summary>
        public StdRadioButton()
        {
            ItemDefaults.CheckBoxVisible = true;
            Item.IsRadioButton = true;
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.RadioButton;

        /// <inheritdoc/>
        public override void RaiseCheckedChanged()
        {
            if (DisposingOrDisposed)
                return;
            
            base.RaiseCheckedChanged();

            if (suppressSiblingNotifyCounter > 0)
                return;

            suppressSiblingNotifyCounter++;

            try
            {
                var siblings = Siblings.ToArray();

                foreach (var sibling in siblings)
                {
                    if (sibling is not StdRadioButton radioButton)
                        continue;
                    radioButton.IsChecked = false;
                }
            }
            finally
            {
                suppressSiblingNotifyCounter--;
            }
        }
    }
}