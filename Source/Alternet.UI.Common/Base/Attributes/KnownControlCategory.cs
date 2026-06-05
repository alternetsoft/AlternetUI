using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Defines known control categories for design-time support.
    /// </summary>
    public static class KnownControlCategory
    {
        /// <summary>
        /// Indicates that the control is hidden and should not be displayed in the designer toolbox.
        /// </summary>
        public const string Hidden = "Hidden";

        /// <summary>
        /// Indicates that the control is for internal use only and should not be displayed in the designer toolbox.
        /// </summary>
        public const string Internal = "Internal";

        /// <summary>
        /// Indicates that the control is related to printing functionality.
        /// </summary>
        public const string Printing = "Printing";

        /// <summary>
        /// Indicates that the control belongs to other categories not specifically defined.
        /// </summary>
        public const string Other = "Other";

        /// <summary>
        /// Indicates that the control belongs to the common category.
        /// </summary>
        public const string Common = "Common";

        /// <summary>
        /// Indicates that the control belongs to the tests category.
        /// </summary>
        public const string Tests = "Tests";

        /// <summary>
        /// Indicates that the control belongs to the containers category.
        /// </summary>
        public const string Containers = "Containers";

        /// <summary>
        /// Indicates that the control belongs to the editors category.
        /// </summary>
        public const string Editors = "Editors";

        /// <summary>
        /// Indicates that the control belongs to the date category.
        /// </summary>
        public const string Date = "Date";

        /// <summary>
        /// Indicates that the control belongs to the panels category.
        /// </summary>
        public const string Panels = "Panels";

        /// <summary>
        /// Indicates that the control belongs to the menus and toolbars category.
        /// </summary>
        public const string MenusAndToolbars = "MenusAndToolbars";

        /// <summary>
        /// Indicates that the control belongs to the dialogs category.
        /// </summary>
        public const string Dialogs = "Dialogs";
        
        /// <summary>
        /// Indicates that the control belongs to the native category.
        /// </summary>
        public const string Native = "Native";

    }
}
