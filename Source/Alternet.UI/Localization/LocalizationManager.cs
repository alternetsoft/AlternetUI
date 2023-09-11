using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Defines localizations for all application strings.
    /// </summary>
    public class LocalizationManager
    {
        /// <summary>
        /// Current localizations for all application strings.
        /// </summary>
        public static LocalizationManager Default { get; set; } = new();

        /// <summary>
        /// Gets or sets localizations for common strings.
        /// </summary>
        public CommonStrings CommonStrings
        {
            get
            {
                return CommonStrings.Default;
            }

            set
            {
                CommonStrings.Default = value;
            }
        }

        /// <summary>
        /// Gets or sets localizations for system color names.
        /// </summary>
        public KnownColorStrings KnownColors
        {
            get
            {
                return KnownColorStrings.Default;
            }

            set
            {
                KnownColorStrings.Default = value;
            }
        }

        /// <summary>
        /// Gets or sets localizations for property names.
        /// </summary>
        public PropNameStrings PropNames
        {
            get
            {
                return PropNameStrings.Default;
            }

            set
            {
                PropNameStrings.Default = value;
            }
        }

        /// <summary>
        /// Gets or sets localizations for error messages.
        /// </summary>
        public ErrorMessages ErrorMessages
        {
            get
            {
                return ErrorMessages.Default;
            }

            set
            {
                ErrorMessages.Default = value;
            }
        }
    }
}
