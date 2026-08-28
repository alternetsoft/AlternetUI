using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Alternet.UI
{
    /// <summary>
    /// Enables the user to select a single option from a group of choices when
    /// paired with other <see cref="XRadioButton"/> controls.
    /// <see cref="XRadioButton"/> represents a generic radio button control.
    /// This control is implemented inside the library and can be used in the
    /// same way as a regular native radio button control. <see cref="XRadioButton"/> is used when you need
    /// to have the same look and behavior for all platforms. <see cref="XRadioButton"/>
    /// provides many additional features,
    /// which are not available in the native radio button control.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When the user selects one radio button (also known as an option button)
    /// within a group, the others clear automatically.
    /// All <see cref="XRadioButton"/> controls in a given container, such as a
    /// <see cref="Panel"/>, constitute a group.
    /// To create multiple groups on one window, place each group in its own
    /// container, such as a <see cref="Panel"/>.
    /// </para>
    /// <para>
    /// <see cref="XRadioButton"/> and <see cref="XCheckBox"/> controls have a
    /// similar function: they offer choices a user can select or clear.
    /// The difference is that multiple <see cref="XCheckBox"/> controls can be
    /// selected at the same time, but option buttons are mutually exclusive.
    /// </para>
    /// </remarks>
    [ControlCategory(KnownControlCategory.Common)]
    public partial class XRadioButton : XCheckBox
    {
        private static int suppressSiblingNotifyCounter;

        /// <summary>
        /// Initializes a new instance of the <see cref="XRadioButton"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public XRadioButton(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XRadioButton"/> class with the specified text.
        /// </summary>
        /// <param name="text"></param>
        public XRadioButton(string text)
            : this()
        {
            Text = text;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XRadioButton"/> class.
        /// </summary>
        public XRadioButton()
        {
            Item.IsRadioButton = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether sibling controls
        /// should be unchecked when this control's checked state changes.
        /// </summary>
        public virtual bool AutoUncheckSiblings { get; set; } = true;

        /// <summary>
        /// Gets or sets the unique identifier of the radio button group to which this control belongs.
        /// This property allows you to specify a custom group for the radio button,
        /// enabling you to have multiple groups of radio buttons within the same container.
        /// If this property is not set, the radio button will be part of the default group within its container.
        /// </summary>
        [Browsable(false)]
        public virtual ObjectUniqueId? RadioGroupId { get; set; }

        /// <summary>
        /// Gets or sets the collection of sibling <see cref="XRadioButton"/> controls that belong to the same group.
        /// If this property is not set, the control will automatically find its siblings in the same container.
        /// </summary>
        [Browsable(false)]
        public virtual IEnumerable<XRadioButton>? RadioSiblings { get; set; }

        /// <inheritdoc/>
        public override void RaiseCheckedChanged()
        {
            if (DisposingOrDisposed)
                return;

            base.RaiseCheckedChanged();

            if (!AutoUncheckSiblings || suppressSiblingNotifyCounter > 0)
                return;

            suppressSiblingNotifyCounter++;

            try
            {
                var siblings = GetSiblingButtons();

                foreach (var sibling in siblings)
                {
                    sibling.IsChecked = false;
                }
            }
            finally
            {
                suppressSiblingNotifyCounter--;
            }
        }

        /// <summary>
        /// Gets the sibling <see cref="XRadioButton"/> controls in the same container.
        /// </summary>
        /// <returns>A collection of sibling <see cref="XRadioButton"/> controls.</returns>
        protected virtual IEnumerable<XRadioButton> GetSiblingButtons()
        {
            if (RadioSiblings is not null)
                return Select(RadioSiblings);

            return Select(Siblings);

            IEnumerable<XRadioButton> Select(IEnumerable<AbstractControl> siblings)
            {
                foreach (var sibling in siblings)
                {
                    if (sibling == this || sibling is not XRadioButton radioButton)
                        continue;
                    if (radioButton.RadioGroupId != RadioGroupId)
                        continue;
                    yield return radioButton;
                }
            }
        }
    }
}