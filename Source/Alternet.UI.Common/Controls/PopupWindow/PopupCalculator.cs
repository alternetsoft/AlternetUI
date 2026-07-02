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
            Title = CommonStrings.Default.WindowTitleSelectValue;
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
}
