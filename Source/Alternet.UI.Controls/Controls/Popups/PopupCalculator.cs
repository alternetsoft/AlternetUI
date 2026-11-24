using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Popup window with <see cref="Calculator"/> control.
    /// </summary>
    public partial class PopupCalculator : PopupWindow<Calculator>
    {
        private static PopupCalculator? defaultCalculator;

        /// <summary>
        /// Initializes a new instance of the <see cref="PopupCalculator"/> class.
        /// </summary>
        public PopupCalculator()
        {
            Title = CommonStrings.Default.WindowTitleCalculator;
            HideOnClick = false;
            HideOnDoubleClick = false;
        }

        /// <summary>
        /// Gets or sets default instance of the <see cref="PopupCalculator"/>.
        /// </summary>
        public static new PopupCalculator Default
        {
            get
            {
                if (defaultCalculator == null)
                {
                    defaultCalculator = new PopupCalculator();
                }

                return defaultCalculator;
            }

            set
            {
                defaultCalculator = value;
            }
        }

        /// <inheritdoc/>
        protected override Calculator CreateMainControl()
        {
            var result = new Calculator()
            {
                HasBorder = false,
            };

            return result;
        }

        /// <inheritdoc/>
        protected override void OnVisibleChanging(EventArgs e)
        {
            base.OnVisibleChanging(e);
            if (!Visible)
            {
                SetSizeToContent();
            }
        }
    }

    /// <summary>
    /// Popup window with <see cref="PinCodePicker"/> control.
    /// </summary>
    public partial class PopupPinCodePicker : PopupWindow<PinCodePicker>
    {
        private static PopupPinCodePicker? defaultPinCodePicker;

        /// <summary>
        /// Initializes a new instance of the <see cref="PopupPinCodePicker"/> class.
        /// </summary>
        public PopupPinCodePicker()
        {
            Title = CommonStrings.Default.WindowTitleEnterPinCode;
            HideOnClick = false;
            HideOnDoubleClick = false;
        }

        /// <summary>
        /// Gets or sets default instance of the <see cref="PopupPinCodePicker"/>.
        /// </summary>
        public static new PopupPinCodePicker Default
        {
            get
            {
                if (defaultPinCodePicker == null)
                {
                    defaultPinCodePicker = new PopupPinCodePicker();
                }

                return defaultPinCodePicker;
            }

            set
            {
                defaultPinCodePicker = value;
            }
        }

        /// <inheritdoc/>
        protected override PinCodePicker CreateMainControl()
        {
            var result = new PinCodePicker()
            {
                HasBorder = false,
            };

            return result;
        }

        /// <inheritdoc/>
        protected override void OnVisibleChanging(EventArgs e)
        {
            base.OnVisibleChanging(e);
            if (!Visible)
            {
                SetSizeToContent();
            }
        }
    }
}
