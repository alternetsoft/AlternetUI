using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Defines localizations for common strings.
    /// </summary>
    public class CommonStrings
    {
        /// <summary>
        /// Current localizations for common strings.
        /// </summary>
        public static CommonStrings Default { get; set; } = new();

        /// <summary>
        /// Gets or sets common string localization.
        /// </summary>
        public string ButtonOk { get; set; } = "Ok";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonCancel { get; set; } = "Cancel";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonApply { get; set; } = "Apply";
    }
}
