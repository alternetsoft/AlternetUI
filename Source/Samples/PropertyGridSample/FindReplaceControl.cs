using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Implements main control of the Find and Replace dialogs.
    /// </summary>
    public class FindReplaceControl : GenericToolBarSet
    {
        private readonly TextBox findEdit = new()
        {
            Margin = (2, 0, 2, 0),
        };

        private readonly TextBox replaceEdit = new()
        {
            Margin = (2, 0, 2, 0),
        };

        public FindReplaceControl()
        {
            ToolBarCount = 3;

            OptionsToolBar.AddPicture(null);

            IdMatchCase = OptionsToolBar.AddStickyBtn(
                CommonStrings.Default.FindOptionMatchCase,
                OptionsToolBar.GetNormalSvgImages().ImgFindMatchCase,
                OptionsToolBar.GetDisabledSvgImages().ImgFindMatchCase,
                null,
                OnClickMatchCase);
            OptionsToolBar.SetToolShortcut(IdMatchCase, KnownKeys.FindReplaceControlKeys.MatchCase);

            IdMatchWholeWord = OptionsToolBar.AddStickyBtn(
                CommonStrings.Default.FindOptionMatchWholeWord,
                OptionsToolBar.GetNormalSvgImages().ImgFindMatchFullWord,
                OptionsToolBar.GetDisabledSvgImages().ImgFindMatchFullWord,
                null,
                OnClickMatchWholeWord);
            OptionsToolBar.SetToolShortcut(IdMatchWholeWord, KnownKeys.FindReplaceControlKeys.MatchWholeWord);

            IdUseRegularExpressions = OptionsToolBar.AddStickyBtn(
                CommonStrings.Default.FindOptionUseRegularExpressions,
                OptionsToolBar.GetNormalSvgImages().ImgRegularExpr,
                OptionsToolBar.GetDisabledSvgImages().ImgRegularExpr,
                null,
                OnClickUseRegularExpressions);
            OptionsToolBar.SetToolShortcut(IdUseRegularExpressions, KnownKeys.FindReplaceControlKeys.UseRegularExpressions);

            IdToggleReplaceOptions = FindToolBar.AddSpeedBtn(
                CommonStrings.Default.ToggleToSwitchBetweenFindReplace,
                FindToolBar.GetNormalSvgImages().ImgAngleDown,
                FindToolBar.GetDisabledSvgImages().ImgAngleDown);

            findEdit.SuggestedWidth = 150;
            findEdit.EmptyTextHint = CommonStrings.Default.ButtonFind;
            replaceEdit.EmptyTextHint = CommonStrings.Default.ButtonReplace;
            replaceEdit.SuggestedWidth = 150;
            IdFindEdit = FindToolBar.AddControl(findEdit);

            IdFindNext = FindToolBar.AddSpeedBtn(
                CommonStrings.Default.ButtonFindNext,
                FindToolBar.GetNormalSvgImages().ImgArrowDown,
                FindToolBar.GetDisabledSvgImages().ImgArrowDown);
            FindToolBar.SetToolShortcut(IdFindNext, KnownKeys.FindReplaceControlKeys.FindNext);

            IdFindPrevious = FindToolBar.AddSpeedBtn(
                CommonStrings.Default.ButtonFindPrevious,
                FindToolBar.GetNormalSvgImages().ImgArrowUp,
                FindToolBar.GetDisabledSvgImages().ImgArrowUp);
            FindToolBar.SetToolShortcut(IdFindPrevious, KnownKeys.FindReplaceControlKeys.FindPrevious);

            IdFindClose = FindToolBar.AddSpeedBtn(
                CommonStrings.Default.ButtonClose,
                FindToolBar.GetNormalSvgImages().ImgCancel,
                FindToolBar.GetDisabledSvgImages().ImgCancel);

            FindToolBar.Parent = this;

            ReplaceToolBar.AddPicture(null);

            IdReplaceEdit = ReplaceToolBar.AddControl(replaceEdit);

            IdReplace = ReplaceToolBar.AddSpeedBtn(
                CommonStrings.Default.ButtonReplace,
                FindToolBar.GetNormalSvgImages().ImgReplace,
                FindToolBar.GetDisabledSvgImages().ImgReplace);
            ReplaceToolBar.SetToolShortcut(IdReplace, KnownKeys.FindReplaceControlKeys.Replace); 

            IdReplaceAll = ReplaceToolBar.AddSpeedBtn(
                CommonStrings.Default.ButtonReplaceAll,
                FindToolBar.GetNormalSvgImages().ImgReplaceAll,
                FindToolBar.GetDisabledSvgImages().ImgReplaceAll);
            ReplaceToolBar.SetToolShortcut(IdReplaceAll, KnownKeys.FindReplaceControlKeys.ReplaceAll); 

            ReplaceToolBar.Visible = false;
            ReplaceToolBar.Parent = this;

            FindToolBar.AddToolAction(IdFindNext, OnClickFindNext);
            FindToolBar.AddToolAction(IdFindPrevious, OnClickFindPrevious);
            FindToolBar.AddToolAction(IdFindClose, OnClickClose);
            FindToolBar.AddToolAction(IdToggleReplaceOptions, OnClickToggleReplace);

            ReplaceToolBar.AddToolAction(IdReplace, OnClickReplace);
            ReplaceToolBar.AddToolAction(IdReplaceAll, OnClickReplaceAll);
        }

        public EventHandler? ClickFindNext;

        public EventHandler? ClickFindPrevious;

        public EventHandler? ClickReplace;

        public EventHandler? ClickReplaceAll;

        public EventHandler? ClickClose;

        public EventHandler? OptionMatchCaseChanged;

        public EventHandler? OptionMatchWholeWordChanged;

        public EventHandler? OptionUseRegularExpressionsChanged;

        public bool OptionMatchCase
        {
            get
            {
                return OptionsToolBar.GetToolSticky(IdMatchCase);
            }

            set
            {
                if (OptionMatchCase == value)
                    return;
                OptionsToolBar.SetToolSticky(IdMatchCase, value);
                OptionMatchCaseChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public bool OptionMatchWholeWord
        {
            get
            {
                return OptionsToolBar.GetToolSticky(IdMatchWholeWord);
            }

            set
            {
                if (OptionMatchWholeWord == value)
                    return;
                OptionsToolBar.SetToolSticky(IdMatchWholeWord, value);
                OptionMatchWholeWordChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public bool OptionUseRegularExpressions
        {
            get
            {
                return OptionsToolBar.GetToolSticky(IdUseRegularExpressions);
            }

            set
            {
                if (OptionUseRegularExpressions == value)
                    return;
                OptionsToolBar.SetToolSticky(IdUseRegularExpressions, value);
                OptionUseRegularExpressionsChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        [Browsable(false)]
        public ObjectUniqueId IdMatchCase { get; }

        [Browsable(false)]
        public ObjectUniqueId IdMatchWholeWord { get; }

        [Browsable(false)]
        public ObjectUniqueId IdUseRegularExpressions { get; }

        [Browsable(false)]
        public ObjectUniqueId IdToggleReplaceOptions { get; }

        [Browsable(false)]
        public ObjectUniqueId IdFindEdit { get; }

        [Browsable(false)]
        public ObjectUniqueId IdReplaceEdit { get; }

        [Browsable(false)]
        public ObjectUniqueId IdFindNext { get; }

        [Browsable(false)]
        public ObjectUniqueId IdFindPrevious { get; }

        [Browsable(false)]
        public ObjectUniqueId IdFindClose { get; }

        [Browsable(false)]
        public ObjectUniqueId IdReplace { get; }

        [Browsable(false)]
        public ObjectUniqueId IdReplaceAll { get; }

        [Browsable(false)]
        public TextBox FindEdit => findEdit;

        [Browsable(false)]
        public TextBox ReplaceEdit => replaceEdit;

        [Browsable(false)]
        public GenericToolBar FindToolBar => GetToolBar(0);

        [Browsable(false)]
        public GenericToolBar ReplaceToolBar => GetToolBar(1);

        [Browsable(false)]
        public GenericToolBar OptionsToolBar => GetToolBar(2);

        public double TextBoxWidth
        {
            get
            {
                return findEdit.SuggestedWidth;
            }

            set
            {
                if (TextBoxWidth == value)
                    return;
                findEdit.SuggestedWidth = value;
                replaceEdit.SuggestedWidth = value;
            }
        }

        public bool ReplaceVisible
        {
            get
            {
                return ReplaceToolBar.Visible;
            }

            set
            {
                if (value)
                {
                    ReplaceToolBar.Visible = true;
                    FindToolBar.SetToolDisabledImage(
                        IdToggleReplaceOptions,
                        FindToolBar.GetDisabledSvgImages().ImgAngleUp);
                    FindToolBar.SetToolImage(
                        IdToggleReplaceOptions,
                        FindToolBar.GetNormalSvgImages().ImgAngleUp);
                }
                else
                {
                    ReplaceToolBar.Visible = false;
                    FindToolBar.SetToolDisabledImage(
                        IdToggleReplaceOptions,
                        FindToolBar.GetDisabledSvgImages().ImgAngleDown);
                    FindToolBar.SetToolImage(
                        IdToggleReplaceOptions,
                        FindToolBar.GetNormalSvgImages().ImgAngleDown);
                }
            }
        }

        private void OnClickUseRegularExpressions(object? sender, EventArgs e)
        {
            OptionUseRegularExpressions = !OptionUseRegularExpressions;
        }

        private void OnClickMatchWholeWord(object? sender, EventArgs e)
        {
            OptionMatchWholeWord = !OptionMatchWholeWord;
        }

        private void OnClickMatchCase(object? sender, EventArgs e)
        {
            OptionMatchCase = !OptionMatchCase;
        }

        private void OnClickFindNext(object? sender, EventArgs e)
        {
            ClickFindNext?.Invoke(this, e);
        }

        private void OnClickToggleReplace(object? sender, EventArgs e)
        {
            ReplaceVisible = !ReplaceVisible;
        }

        private void OnClickFindPrevious(object? sender, EventArgs e)
        {
            ClickFindPrevious?.Invoke(this, e);
        }

        private void OnClickReplace(object? sender, EventArgs e)
        {
            ClickReplace?.Invoke(this, e);
        }

        private void OnClickReplaceAll(object? sender, EventArgs e)
        {
            ClickReplaceAll?.Invoke(this, e);
        }

        private void OnClickClose(object? sender, EventArgs e)
        {
            ClickClose?.Invoke(this, e);
        }
    }
}
