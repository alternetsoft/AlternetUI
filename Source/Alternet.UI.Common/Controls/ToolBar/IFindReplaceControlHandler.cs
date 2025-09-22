#nullable enable
#pragma warning disable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Defines the interface that allows working with the find and replace control.
    /// </summary>
    public interface IFindReplaceControlHandler
    {
        /// <inheritdoc cref="FindReplaceControl.ClickFindNext"/>
        event EventHandler? ClickFindNext;

        /// <inheritdoc cref="FindReplaceControl.ClickFindPrevious"/>
        event EventHandler? ClickFindPrevious;

        /// <inheritdoc cref="FindReplaceControl.ClickReplace"/>
        event EventHandler? ClickReplace;

        /// <inheritdoc cref="FindReplaceControl.ClickReplaceAll"/>
        event EventHandler? ClickReplaceAll;

        /// <inheritdoc cref="FindReplaceControl.ClickClose"/>
        event EventHandler? ClickClose;

        /// <inheritdoc cref="FindReplaceControl.OptionMatchCaseChanged"/>
        event EventHandler? OptionMatchCaseChanged;

        /// <inheritdoc cref="FindReplaceControl.OptionMatchWholeWordChanged"/>
        event EventHandler? OptionMatchWholeWordChanged;

        /// <inheritdoc cref="FindReplaceControl.OptionUseRegularExpressionsChanged"/>
        event EventHandler? OptionUseRegularExpressionsChanged;

        /// <inheritdoc cref="FindReplaceControl.Scope"/>
        FindReplaceControl.SearchScope? Scope { get; set; }

        /// <inheritdoc cref="FindReplaceControl.IsScopeAllOpenDocuments"/>
        bool IsScopeAllOpenDocuments { get; set; }

        /// <inheritdoc cref="FindReplaceControl.IsScopeCurrentProject"/>
        bool IsScopeCurrentProject { get; set; }

        /// <inheritdoc cref="FindReplaceControl.IsScopeSelectionOnly"/>
        bool IsScopeSelectionOnly { get; set; }

        /// <inheritdoc cref="FindReplaceControl.IsScopeCurrentDocument"/>
        bool IsScopeCurrentDocument { get; set; }

        IControl? FindEditor { get; }

        IControl? ReplaceEditor { get; }

        /// <inheritdoc cref="FindReplaceControl.IsScopeEditEnabled"/>
        bool IsScopeEditEnabled { get; set; }

        /// <inheritdoc cref="FindReplaceControl.TextBoxWidth"/>
        Coord TextBoxWidth { get; set; }

        /// <inheritdoc cref="FindReplaceControl.WantKeys"/>
        bool WantKeys { get; set; }

        /// <inheritdoc cref="FindReplaceControl.CanFindNext"/>
        bool CanFindNext { get; }

        /// <inheritdoc cref="FindReplaceControl.CanFindPrevious"/>
        bool CanFindPrevious { get; }

        /// <inheritdoc cref="FindReplaceControl.CanReplace"/>
        bool CanReplace { get; }

        /// <inheritdoc cref="FindReplaceControl.CanReplaceAll"/>
        bool CanReplaceAll { get; }

        /// <inheritdoc cref="FindReplaceControl.CanMatchCase"/>
        bool CanMatchCase { get; }

        /// <inheritdoc cref="FindReplaceControl.CanMatchWholeWord"/>
        bool CanMatchWholeWord { get; }

        /// <inheritdoc cref="FindReplaceControl.CanUseRegularExpressions"/>
        bool CanUseRegularExpressions { get; }

        /// <inheritdoc cref="FindReplaceControl.ShowErrorBorder"/>
        bool ShowErrorBorder { get; set; }

        /// <inheritdoc cref="FindReplaceControl.CanFindInCurrentDocument"/>
        bool CanFindInCurrentDocument { get; set; }

        /// <inheritdoc cref="FindReplaceControl.CanFindInAllOpenDocuments"/>
        bool CanFindInAllOpenDocuments { get; set; }

        /// <inheritdoc cref="FindReplaceControl.CanFindInCurrentProject"/>
        bool CanFindInCurrentProject { get; set; }

        /// <inheritdoc cref="FindReplaceControl.CanFindInSelectionOnly"/>
        bool CanFindInSelectionOnly { get; set; }

        /// <inheritdoc cref="FindReplaceControl.Manager"/>
        FindReplaceControl.IFindReplaceConnect? Manager { get; set; }

        /// <inheritdoc cref="FindReplaceControl.OptionFindTextAtCursor"/>
        bool OptionFindTextAtCursor { get; set; }

        /// <inheritdoc cref="FindReplaceControl.OptionPromptOnReplace"/>
        bool OptionPromptOnReplace { get; set; }

        /// <inheritdoc cref="FindReplaceControl.OptionHiddenText"/>
        bool OptionHiddenText { get; set; }

        /// <inheritdoc cref="FindReplaceControl.OptionSearchUp"/>
        bool OptionSearchUp { get; set; }

        /// <inheritdoc cref="FindReplaceControl.OptionMatchCase"/>
        bool OptionMatchCase { get; set; }

        /// <inheritdoc cref="FindReplaceControl.OptionMatchCaseEnabled"/>
        bool OptionMatchCaseEnabled { get; set; }

        /// <inheritdoc cref="FindReplaceControl.OptionMatchWholeWord"/>
        bool OptionMatchWholeWord { get; set; }

        /// <inheritdoc cref="FindReplaceControl.OptionMatchWholeWordEnabled"/>
        bool OptionMatchWholeWordEnabled { get; set; }

        /// <inheritdoc cref="FindReplaceControl.OptionUseRegularExpressions"/>
        bool OptionUseRegularExpressions { get; set; }

        /// <inheritdoc cref="FindReplaceControl.SearchList"/>
        ListBoxItems SearchList { get; }

        /// <inheritdoc cref="FindReplaceControl.ReplaceList"/>
        ListBoxItems ReplaceList { get; }

        /// <inheritdoc cref="FindReplaceControl.TextToFind"/>
        string TextToFind { get; set; }

        /// <inheritdoc cref="FindReplaceControl.FindEditEmptyTextHint"/>
        string? FindEditEmptyTextHint { get; set; }

        /// <inheritdoc cref="FindReplaceControl.ReplaceEditEmptyTextHint"/>
        string? ReplaceEditEmptyTextHint { get; set; }

        /// <inheritdoc cref="FindReplaceControl.TextToReplace"/>
        string TextToReplace { get; set; }

        /// <inheritdoc cref="FindReplaceControl.OptionUseRegularExpressionsEnabled"/>
        bool OptionUseRegularExpressionsEnabled { get; set; }

        /// <inheritdoc cref="FindReplaceControl.OptionUseRegularExpressionsVisible"/>
        bool OptionUseRegularExpressionsVisible { get; set; }

        /// <inheritdoc cref="FindReplaceControl.CloseButtonVisible"/>
        bool CloseButtonVisible { get; set; }

        /// <inheritdoc cref="FindReplaceControl.OptionsVisible"/>
        bool OptionsVisible { get; set; }

        /// <inheritdoc cref="FindReplaceControl.ToggleReplaceVisible"/>
        bool ToggleReplaceVisible { get; set; }

        /// <inheritdoc cref="FindReplaceControl.ReplaceVisible"/>
        bool ReplaceVisible { get; set; }

        /// <inheritdoc cref="FindReplaceControl.ToggleHiddenText"/>
        void ToggleHiddenText();

        /// <inheritdoc cref="FindReplaceControl.ToggleMatchCase"/>
        void ToggleMatchCase();

        /// <inheritdoc cref="FindReplaceControl.ToggleRegularExpressions"/>
        void ToggleRegularExpressions();

        /// <inheritdoc cref="FindReplaceControl.ToggleSearchUp"/>
        void ToggleSearchUp();

        /// <inheritdoc cref="FindReplaceControl.ToggleWholeWord"/>
        void ToggleWholeWord();

        /// <inheritdoc cref="FindReplaceControl.SelectAllTextInFindEditor"/>
        void SelectAllTextInFindEditor();
    }
}