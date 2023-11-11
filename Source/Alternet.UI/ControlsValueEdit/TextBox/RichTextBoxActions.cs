using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class RichTextBox
    {
        /// <summary>
        /// Toggles <see cref="FontStyle.Bold"/> style of the selection.
        /// </summary>
        public void SelectionToggleBold()
        {
            ApplyBoldToSelection();
        }

        /// <summary>
        /// Toggles <see cref="FontStyle.Italic"/> style of the selection.
        /// </summary>
        public void SelectionToggleItalic()
        {
            ApplyItalicToSelection();
        }

        /// <summary>
        /// Toggles <see cref="FontStyle.Underlined"/> style of the selection.
        /// </summary>
        public void SelectionToggleUnderlined()
        {
            ApplyUnderlineToSelection();
        }

        /// <summary>
        /// Toggles <see cref="FontStyle.Strikethrough"/> style of the selection.
        /// </summary>
        public void SelectionToggleStrikethrough()
        {
            ApplyTextEffectToSelection(TextBoxTextAttrEffects.Strikethrough);
        }

        /// <summary>
        /// Sets text alignment in the current position to <see cref="TextBoxTextAttrAlignment.Left"/>
        /// </summary>
        public void SelectionAlignLeft()
        {
            ApplyAlignmentToSelection(TextBoxTextAttrAlignment.Left);
        }

        /// <summary>
        /// Sets text alignment in the current position to <see cref="TextBoxTextAttrAlignment.Center"/>
        /// </summary>
        public void SelectionAlignCenter()
        {
            ApplyAlignmentToSelection(TextBoxTextAttrAlignment.Center);
        }

        /// <summary>
        /// Sets text alignment in the current position to <see cref="TextBoxTextAttrAlignment.Right"/>
        /// </summary>
        public void SelectionAlignRight()
        {
            ApplyAlignmentToSelection(TextBoxTextAttrAlignment.Right);
        }

        /// <summary>
        /// Sets text alignment in the current position to <see cref="TextBoxTextAttrAlignment.Justified"/>
        /// </summary>
        public void SelectionAlignJustified()
        {
            ApplyAlignmentToSelection(TextBoxTextAttrAlignment.Justified);
        }

        public void SelectionClearFormatting()
        {

        }

        /// <summary>
        /// Handles default rich text editor keys.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        /// <remarks>
        /// You can use this method in the <see cref="UIElement.KeyDown"/> event handlers.
        /// </remarks>
        public virtual bool HandleAdditionalKeys(KeyEventArgs e)
        {
            if (KeyInfo.Run(KnownKeys.RichEditKeys.ToggleBold, e, SelectionToggleBold))
                return true;
            if (KeyInfo.Run(KnownKeys.RichEditKeys.ToggleItalic, e, SelectionToggleItalic))
                return true;
            if (KeyInfo.Run(KnownKeys.RichEditKeys.ToggleUnderline, e, SelectionToggleUnderlined))
                return true;
            if (KeyInfo.Run(KnownKeys.RichEditKeys.ToggleStrikethrough, e, SelectionToggleStrikethrough))
                return true;
            if (KeyInfo.Run(KnownKeys.RichEditKeys.LeftAlign, e, SelectionAlignLeft))
                return true;
            if (KeyInfo.Run(KnownKeys.RichEditKeys.CenterAlign, e, SelectionAlignCenter))
                return true;
            if (KeyInfo.Run(KnownKeys.RichEditKeys.RightAlign, e, SelectionAlignRight))
                return true;
            if (KeyInfo.Run(KnownKeys.RichEditKeys.Justify, e, SelectionAlignJustified))
                return true;
            if (KeyInfo.Run(KnownKeys.RichEditKeys.ClearTextFormatting, e, SelectionClearFormatting))
                return true;
            return false;
        }
    }
}
