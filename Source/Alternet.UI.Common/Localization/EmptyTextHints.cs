using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI.Localization
{
    /// <summary>
    /// Contains static members which define known default values for
    /// the <see cref="TextBox.EmptyTextHint"/> property.
    /// </summary>
    public static class EmptyTextHints
    {
        private static string? filterEdit;
        private static string? replaceEdit;
        private static string? passwordEdit;
        private static string? findEdit;

        /// <summary>
        /// Gets or sets default value for the <see cref="TextBox.EmptyTextHint"/>
        /// property of the find text editor used in <see cref="FindReplaceControl"/>.
        /// </summary>
        public static string FindEdit
        {
            get
            {
                return findEdit ?? CommonStrings.Default.ButtonFind + StringUtils.ThreeDots;
            }

            set
            {
                findEdit = value;
            }
        }

        /// <summary>
        /// Gets or sets default value for the <see cref="TextBox.EmptyTextHint"/>
        /// property of the password editor.
        /// </summary>
        public static string PasswordEdit
        {
            get
            {
                return passwordEdit ?? CommonStrings.Default.ButtonPassword + StringUtils.ThreeDots;
            }

            set
            {
                passwordEdit = value;
            }
        }

        /// <summary>
        /// Gets or sets default value for the <see cref="TextBox.EmptyTextHint"/>
        /// property of the filter editor.
        /// </summary>
        public static string FilterEdit
        {
            get
            {
                return filterEdit ?? CommonStrings.Default.ButtonFilter + StringUtils.ThreeDots;
            }

            set
            {
                filterEdit = value;
            }
        }

        /// <summary>
        /// Gets or sets default value for the <see cref="TextBox.EmptyTextHint"/>
        /// property of the replace text editor used in <see cref="FindReplaceControl"/>.
        /// </summary>
        public static string ReplaceEdit
        {
            get
            {
                return replaceEdit ?? CommonStrings.Default.ButtonReplace + StringUtils.ThreeDots;
            }

            set
            {
                replaceEdit = value;
            }
        }
    }
}
