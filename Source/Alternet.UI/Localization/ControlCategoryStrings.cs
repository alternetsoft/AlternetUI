using System;
using System.Collections;
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
        private static AdvDictionary<string, Func<string, string>>? titles;

        /// <summary>
        /// Current localizations for control categories.
        /// </summary>
        public static ControlCategoryStrings Default { get; set; } = new();

        /// <summary>
        /// Gets <see cref="IDictionary"/> which is used to get
        /// localized title for the control category.
        /// </summary>
        public static IDictionary<string, Func<string, string>>? Titles => titles ?? CreateTitles();

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

        /// <summary>
        /// Gets localized title for the specified category id.
        /// </summary>
        /// <param name="id">Category id.</param>
        public virtual string GetLocalizedTitle(string id)
        {
            CreateTitles();
            var fn = titles!.GetValueOrDefault(id, (s) => s);
            return fn(id);
        }

        private static IDictionary<string, Func<string, string>>? CreateTitles()
        {
            titles ??= new()
                {
                    { nameof(Common), (s) => Default.Common },
                    { nameof(Containers), (s) => Default.Containers },
                    { nameof(MenusAndToolbars), (s) => Default.MenusAndToolbars },
                    { nameof(Data), (s) => Default.Data },
                    { nameof(Components), (s) => Default.Components },
                    { nameof(Printing), (s) => Default.Printing },
                    { nameof(Dialogs), (s) => Default.Dialogs },
                    { nameof(Other), (s) => Default.Other },
                };

            return titles;
        }
    }
}