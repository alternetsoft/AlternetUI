using System;
using System.ComponentModel;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a generic check box control. This control is implemented inside the library and can be used in the
    /// same way as a regular native <see cref="CheckBox"/> control. <see cref="StdCheckBox"/> is used when you need
    /// to have the same code for all platforms. <see cref="StdCheckBox"/> provides many additional features,
    /// which are not available in the native <see cref="CheckBox"/> control.
    /// </summary>
    /// <remarks>
    /// Use a <see cref="StdCheckBox"/> to give the user an option, such as true/false or yes/no.
    /// The <see cref="StdCheckBox"/> control can display an image or text or both.
    /// </remarks>
    [ControlCategory("Common")]
    public partial class StdCheckBox : GenericItemControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StdCheckBox"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public StdCheckBox(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StdCheckBox"/> class with the specified text.
        /// </summary>
        /// <param name="text"></param>
        public StdCheckBox(string text)
            : this()
        {
            Text = text;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StdCheckBox"/> class.
        /// </summary>
        public StdCheckBox()
        {
            ItemDefaults.CheckBoxVisible = true;
            WantTab = true;
            IsGraphicControl = false;
        }

        /// <summary>
        /// Gets or sets whether to align check box on the right side of the text.
        /// </summary>
        [DefaultValue(false)]
        public virtual bool AlignRight
        {
            get
            {
                return Item.IsCheckRightAligned;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                if (Item.IsCheckRightAligned == value)
                    return;
                Item.IsCheckRightAligned = value;
                Invalidate();
            }
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.CheckBox;
    }
}