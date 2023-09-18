using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI.Localization
{
    /// <summary>
    /// Defines localizations for property categories.
    /// </summary>
    public class PropCategoryStrings
    {
        /// <summary>
        /// Current localizations for control categories.
        /// </summary>
        public static PropCategoryStrings Default { get; set; } = new();

        /// <summary>
        /// Gets or sets property category localization.
        /// </summary>
        public string Other { get; set; } = "Other";

        /// <inheritdoc cref="Other"/>
        public string Action { get; set; } = "Action";

        /// <inheritdoc cref="Other"/>
        public string Appearance { get; set; } = "Appearance";

        /// <inheritdoc cref="Other"/>
        public string Asynchronous { get; set; } = "Asynchronous";

        /// <inheritdoc cref="Other"/>
        public string Behavior { get; set; } = "Behavior";

        /// <inheritdoc cref="Other"/>
        public string Data { get; set; } = "Data";

        /// <inheritdoc cref="Other"/>
        public string Design { get; set; } = "Design";

        /// <inheritdoc cref="Other"/>
        public string DragDrop { get; set; } = "DragDrop";

        /// <inheritdoc cref="Other"/>
        public string Focus { get; set; } = "Focus";

        /// <inheritdoc cref="Other"/>
        public string Format { get; set; } = "Format";

        /// <inheritdoc cref="Other"/>
        public string Key { get; set; } = "Key";

        /// <inheritdoc cref="Other"/>
        public string Layout { get; set; } = "Layout";

        /// <inheritdoc cref="Other"/>
        public string Mouse { get; set; } = "Mouse";

        /// <inheritdoc cref="Other"/>
        public string WindowStyle { get; set; } = "WindowStyle";
    }
}