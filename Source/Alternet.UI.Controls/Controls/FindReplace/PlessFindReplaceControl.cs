#nullable enable
#pragma warning disable

using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    internal class PlessFindReplaceControl : IFindReplaceControlHandler
    {
        public event EventHandler? ClickFindNext;

        public event EventHandler? ClickFindPrevious;

        public event EventHandler? ClickReplace;

        public event EventHandler? ClickReplaceAll;

        public event EventHandler? ClickClose;

        public event EventHandler? OptionMatchCaseChanged;

        public event EventHandler? OptionMatchWholeWordChanged;

        public event EventHandler? OptionUseRegularExpressionsChanged;

        public virtual FindReplaceControl.SearchScope? Scope { get; set; }

        public virtual bool IsScopeAllOpenDocuments { get; set; }

        public virtual bool IsScopeCurrentProject { get; set; }

        public virtual bool IsScopeSelectionOnly { get; set; }

        public virtual bool IsScopeCurrentDocument { get; set; }

        public virtual IControl? FindEditor { get; }

        public virtual IControl? ReplaceEditor { get; }

        public virtual bool IsScopeEditEnabled { get; set; }

        public virtual Coord TextBoxWidth { get; set; }

        public virtual bool WantKeys { get; set; }

        public virtual bool CanFindNext { get; }

        public virtual bool CanFindPrevious { get; }

        public virtual bool CanReplace { get; }

        public virtual bool CanReplaceAll { get; }

        public virtual bool CanMatchCase { get; }

        public virtual bool CanMatchWholeWord { get; }

        public virtual bool CanUseRegularExpressions { get; }

        public virtual bool ShowErrorBorder { get; set; }

        public virtual bool CanFindInCurrentDocument { get; set; }

        public virtual bool CanFindInAllOpenDocuments { get; set; }

        public virtual bool CanFindInCurrentProject { get; set; }

        public virtual bool CanFindInSelectionOnly { get; set; }

        public virtual FindReplaceControl.IFindReplaceConnect? Manager { get; set; }

        public virtual bool OptionFindTextAtCursor { get; set; }

        public virtual bool OptionPromptOnReplace { get; set; }

        public virtual bool OptionHiddenText { get; set; }

        public virtual bool OptionSearchUp { get; set; }

        public virtual bool OptionMatchCase { get; set; }

        public virtual bool OptionMatchCaseEnabled { get; set; }

        public virtual bool OptionMatchWholeWord { get; set; }

        public virtual bool OptionMatchWholeWordEnabled { get; set; }

        public virtual bool OptionUseRegularExpressions { get; set; }

        public virtual ListBoxItems SearchList { get; } = [];

        public virtual ListBoxItems ReplaceList { get; } = [];

        public string TextToFind { get; set; } = string.Empty;

        public string? FindEditEmptyTextHint { get; set; }

        public string? ReplaceEditEmptyTextHint { get; set; }

        public string TextToReplace { get; set; } = string.Empty;

        public virtual bool OptionUseRegularExpressionsEnabled { get; set; }

        public virtual bool OptionUseRegularExpressionsVisible { get; set; }

        public virtual bool CloseButtonVisible { get; set; }

        public virtual bool OptionsVisible { get; set; }

        public virtual bool ToggleReplaceVisible { get; set; }

        public virtual bool ReplaceVisible { get; set; }

        public virtual void SelectAllTextInFindEditor()
        {
        }

        public virtual void ToggleHiddenText()
        {
        }

        public virtual void ToggleMatchCase()
        {
        }

        public virtual void ToggleRegularExpressions()
        {
        }

        public virtual void ToggleSearchUp()
        {
        }

        public virtual void ToggleWholeWord()
        {
        }
    }
}
