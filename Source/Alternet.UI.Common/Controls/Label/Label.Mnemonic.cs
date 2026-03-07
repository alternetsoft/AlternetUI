using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Alternet.UI
{
    public partial class Label
    {
        /// <summary>
        /// Gets or sets the mnemonic marker character. Default is '&amp;'.
        /// </summary>
        public static char DefaultMnemonicMarker = '&';

        /// <summary>
        /// Gets or sets whether to show mnemonic markers in the text.
        /// </summary>
        public static bool DefaultMnemonicMarkerEnabled = false;

        private MnemonicMarkerHelper mnemonicMarkerHelper = new ();

        /// <inheritdoc cref="MnemonicMarkerHelper.MnemonicMarker"/>
        public virtual char? MnemonicMarker
        {
            get => mnemonicMarkerHelper.MnemonicMarker;
            set
            {
                if (mnemonicMarkerHelper.MnemonicMarker == value)
                    return;
                mnemonicMarkerHelper.MnemonicMarker = value;
                if (MnemonicMarkerEnabled is true)
                    PerformLayoutAndInvalidate();
            }
        }

        /// <inheritdoc cref="MnemonicMarkerHelper.MnemonicMarkerEnabled"/>
        public virtual bool? MnemonicMarkerEnabled
        {
            get => mnemonicMarkerHelper.MnemonicMarkerEnabled;

            set
            {
                if (mnemonicMarkerHelper.MnemonicMarkerEnabled == value)
                    return;
                mnemonicMarkerHelper.MnemonicMarkerEnabled = value;
                PerformLayoutAndInvalidate();
            }
        }

        /// <inheritdoc cref="MnemonicMarkerHelper.MnemonicCharIndex"/>
        [DefaultValue(null)]
        public virtual int? MnemonicCharIndex
        {
            get
            {
                return mnemonicMarkerHelper.MnemonicCharIndex;
            }

            set
            {
                if (mnemonicMarkerHelper.MnemonicCharIndex == value)
                    return;
                mnemonicMarkerHelper.MnemonicCharIndex = value;
                Invalidate();
            }
        }

        /// <inheritdoc cref="MnemonicMarkerHelper.GetWithoutMnemonicMarkers"/>
        protected virtual string GetWithoutMnemonicMarkers(string s, out int mnemonicCharIndex)
        {
            return mnemonicMarkerHelper.GetWithoutMnemonicMarkers(s, out mnemonicCharIndex);
        }
    }
}
