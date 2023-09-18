using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI.Localization
{
    /// <summary>
    /// Defines localizations for control categories.
    /// </summary>
    public class ControlCategoryStrings
    {
        /// <summary>
        /// Current localizations for control categories.
        /// </summary>
        public static ControlCategoryStrings Default { get; set; } = new();

        /// <summary>
        /// Gets or sets control category localization.
        /// </summary>
        public string Common { get; set; } = "Common";

        /// <inheritdoc cref="Common"/>
        public string Containers { get; set; } = "Containers";

        /// <inheritdoc cref="Common"/>
        public string MenusAndToolbars { get; set; } = "Menus & Toolbars";

        /// <inheritdoc cref="Common"/>
        public string Data { get; set; } = "Data";

        /// <inheritdoc cref="Common"/>
        public string Components { get; set; } = "Components";

        /// <inheritdoc cref="Common"/>
        public string Printing { get; set; } = "Printing";

        /// <inheritdoc cref="Common"/>
        public string Dialogs { get; set; } = "Dialogs";

        /// <inheritdoc cref="Common"/>
        public string Other { get; set; } = "Other";
    }
}
