﻿using System;
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
        public virtual void SelectionToggleBold()
        {
            ApplyBoldToSelection();
        }

        /// <summary>
        /// Toggles <see cref="FontStyle.Italic"/> style of the selection.
        /// </summary>
        public virtual void SelectionToggleItalic()
        {
            ApplyItalicToSelection();
        }

        /// <summary>
        /// Toggles <see cref="FontStyle.Underline"/> style of the selection.
        /// </summary>
        public virtual void SelectionToggleUnderlined()
        {
            ApplyUnderlineToSelection();
        }

        /// <summary>
        /// Toggles <see cref="FontStyle.Strikeout"/> style of the selection.
        /// </summary>
        public virtual void SelectionToggleStrikethrough()
        {
            ApplyTextEffectToSelection(TextBoxTextAttrEffects.Strikethrough);
        }

        /// <summary>
        /// Sets text alignment in the current position
        /// to <see cref="TextBoxTextAttrAlignment.Left"/>
        /// </summary>
        public virtual void SelectionAlignLeft()
        {
            ApplyAlignmentToSelection(TextBoxTextAttrAlignment.Left);
        }

        /// <summary>
        /// Sets text alignment in the current position
        /// to <see cref="TextBoxTextAttrAlignment.Center"/>
        /// </summary>
        public virtual void SelectionAlignCenter()
        {
            ApplyAlignmentToSelection(TextBoxTextAttrAlignment.Center);
        }

        /// <summary>
        /// Sets text alignment in the current
        /// position to <see cref="TextBoxTextAttrAlignment.Right"/>
        /// </summary>
        public virtual void SelectionAlignRight()
        {
            ApplyAlignmentToSelection(TextBoxTextAttrAlignment.Right);
        }

        /// <summary>
        /// Sets text alignment in the current
        /// position to <see cref="TextBoxTextAttrAlignment.Justified"/>
        /// </summary>
        public virtual void SelectionAlignJustified()
        {
            ApplyAlignmentToSelection(TextBoxTextAttrAlignment.Justified);
        }

        /// <summary>
        /// Sets specified style to the selection.
        /// </summary>
        public virtual bool ApplyStyleToSelection(
            ITextBoxRichAttr style,
            RichTextSetStyleFlags flags)
        {
            return Handler.ApplyStyleToSelection(style, flags);
        }

        /// <summary>
        /// Clears formatting of the selection.
        /// </summary>
        public virtual void SelectionClearFormatting()
        {
            var style = CreateRichAttr();
            var flags = TextBoxTextAttrFlags.Character | TextBoxTextAttrFlags.Alignment
                | TextBoxTextAttrFlags.ParaSpacingAfter | TextBoxTextAttrFlags.ParaSpacingBefore
                | TextBoxTextAttrFlags.LineSpacing | TextBoxTextAttrFlags.CharacterStyleName;
            flags &= ~TextBoxTextAttrFlags.Url;
            style.SetFlags(flags);
            ApplyStyleToSelection(style, RichTextSetStyleFlags.Remove);
        }

        /// <summary>
        /// Handles additional rich text editor keys like Ctrl+B, Ctrl+I, Ctrl+U and other.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        /// <remarks>
        /// You can use this method in the <see cref="AbstractControl.KeyDown"/> event handlers.
        /// <see cref="AllowAdditionalKeys"/> specifies whether <see cref="HandleAdditionalKeys"/>
        /// is called automatically.
        /// </remarks>
        public virtual bool HandleAdditionalKeys(KeyEventArgs e)
        {
            if (KeyInfo.Run(KnownShortcuts.RichEditKeys.ToggleBold, e, SelectionToggleBold))
                return true;
            if (KeyInfo.Run(KnownShortcuts.RichEditKeys.ToggleItalic, e, SelectionToggleItalic))
                return true;
            if (KeyInfo.Run(
                KnownShortcuts.RichEditKeys.ToggleUnderline,
                e,
                SelectionToggleUnderlined))
                return true;
            if (KeyInfo.Run(
                KnownShortcuts.RichEditKeys.ToggleStrikethrough,
                e,
                SelectionToggleStrikethrough))
                return true;
            if (KeyInfo.Run(KnownShortcuts.RichEditKeys.LeftAlign, e, SelectionAlignLeft))
                return true;
            if (KeyInfo.Run(KnownShortcuts.RichEditKeys.CenterAlign, e, SelectionAlignCenter))
                return true;
            if (KeyInfo.Run(KnownShortcuts.RichEditKeys.RightAlign, e, SelectionAlignRight))
                return true;
            if (KeyInfo.Run(KnownShortcuts.RichEditKeys.Justify, e, SelectionAlignJustified))
                return true;
            if (KeyInfo.Run(
                KnownShortcuts.RichEditKeys.ClearTextFormatting,
                e,
                SelectionClearFormatting))
                return true;
            if (KeyInfo.Run(KnownShortcuts.RichEditKeys.ShowGoToLineDialog, e, ShowDialogGoToLine))
                return true;
            return false;
        }
    }
}
