using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a generic panel with inner radio button and suffix controls. 
    /// </summary>
    public partial class XRadioButtonAndSuffix : ControlAndSuffix<XRadioButton, GenericControl>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XRadioButtonAndSuffix"/> class
        /// </summary>
        /// <param name="suffix">The suffix control.</param>
        public XRadioButtonAndSuffix(GenericControl? suffix)
            : base(null, suffix)
        {
            MainControl.TextVisible = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XRadioButtonAndSuffix"/> class.
        /// </summary>
        public XRadioButtonAndSuffix()
            : this(null)
        {
        }

        /// <inheritdoc/>
        protected override GenericControl CreateSuffixControl()
        {
            var result = new Label();

            result.Click += (s, e) =>
            {
                MainControl.IsChecked = true;
            };

            return result;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="XRadioButton"/> is checked.
        /// </summary>
        public bool IsChecked
        {
            get => MainControl.IsChecked;
            set => MainControl.IsChecked = value;
        }
    }
}
