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
    public class FindReplaceControl : VerticalStackPanel
    {
        private readonly GenericToolBar findToolBar = new()
        {
        };

        private readonly GenericToolBar replaceToolBar = new()
        {
        };

        private readonly TextBox findEdit = new()
        {
            Margin = (2, 0, 2, 0),
        };

        private readonly TextBox replaceEdit = new()
        {
            Margin = (2, 0, 2, 0),
        };

        private readonly Panel separatorPanel = new()
        {
            SuggestedHeight = 4,
        };

        private readonly ContextMenu optionsMenu = new();
        private readonly MenuItem menuItemMatchCase = new(CommonStrings.Default.FindOptionMatchCase);
        private readonly MenuItem menuItemMatchWholeWord = new(CommonStrings.Default.FindOptionMatchWholeWord);
        private readonly MenuItem menuItemUseRegularExpressions = new(CommonStrings.Default.FindOptionUseRegularExpressions);

        public FindReplaceControl()
        {
            menuItemMatchCase.Click += MenuItemMatchCase_Click;
            menuItemMatchCase.Image = findToolBar.GetUnscaledNormalSvgImages().ImgFindMatchCase;
            menuItemMatchCase.DisabledImage = findToolBar.GetUnscaledDisabledSvgImages().ImgFindMatchCase;

            menuItemMatchWholeWord.Click += MenuItemMatchWholeWord_Click;
            menuItemMatchWholeWord.Image = findToolBar.GetUnscaledNormalSvgImages().ImgFindMatchFullWord;
            menuItemMatchWholeWord.DisabledImage = findToolBar.GetUnscaledDisabledSvgImages().ImgFindMatchFullWord;

            menuItemUseRegularExpressions.Click += MenuItemUseRegularExpressions_Click;
            menuItemUseRegularExpressions.Image = findToolBar.GetUnscaledNormalSvgImages().ImgRegularExpr;
            menuItemUseRegularExpressions.DisabledImage = findToolBar.GetUnscaledDisabledSvgImages().ImgRegularExpr;

            optionsMenu.Add(menuItemMatchCase);
            optionsMenu.Add(menuItemMatchWholeWord);
            optionsMenu.Add(menuItemUseRegularExpressions);

            const string ToggleReplaceOptions = "Toggle replace options";

            IdToggleReplaceOptions = findToolBar.AddSpeedBtn(
                ToggleReplaceOptions,
                findToolBar.GetNormalSvgImages().ImgAngleDown,
                findToolBar.GetDisabledSvgImages().ImgAngleDown);

            findEdit.SuggestedWidth = 150;
            findEdit.EmptyTextHint = CommonStrings.Default.ButtonFind;
            replaceEdit.EmptyTextHint = CommonStrings.Default.ButtonReplace;
            replaceEdit.SuggestedWidth = 150;
            IdFindEdit = findToolBar.AddControl(findEdit);

            IdFindNext = findToolBar.AddSpeedBtn(
                CommonStrings.Default.ButtonFindNext,
                findToolBar.GetNormalSvgImages().ImgArrowDown,
                findToolBar.GetDisabledSvgImages().ImgArrowDown);
            findToolBar.SetToolShortcut(IdFindNext, Keys.F3);

            IdFindPrevious = findToolBar.AddSpeedBtn(
                CommonStrings.Default.ButtonFindPrevious,
                findToolBar.GetNormalSvgImages().ImgArrowUp,
                findToolBar.GetDisabledSvgImages().ImgArrowUp);
            findToolBar.SetToolShortcut(IdFindPrevious, Keys.F3 | Keys.Shift);

            IdFindOptions = findToolBar.AddSpeedBtn(
                CommonStrings.Default.ButtonOptions,
                findToolBar.GetNormalSvgImages().ImgGear,
                findToolBar.GetDisabledSvgImages().ImgGear);
            findToolBar.SetToolAlignRight(IdFindOptions, true);
            findToolBar.SetToolDropDownMenu(IdFindOptions, optionsMenu);            

            IdFindClose = findToolBar.AddSpeedBtn(
                CommonStrings.Default.ButtonClose,
                findToolBar.GetNormalSvgImages().ImgCancel,
                findToolBar.GetDisabledSvgImages().ImgCancel);
            findToolBar.SetToolAlignRight(IdFindClose, true);

            findToolBar.Parent = this;
            separatorPanel.Visible = false;
            separatorPanel.Parent = this;

            var idDummyImage = replaceToolBar.AddPicture(null);

            IdReplaceEdit = replaceToolBar.AddControl(replaceEdit);

            IdReplace = replaceToolBar.AddTextBtn(
                CommonStrings.Default.ButtonReplace,
                CommonStrings.Default.ButtonReplace);
            replaceToolBar.SetToolShortcut(IdReplace, Keys.R | Keys.Alt);
            replaceToolBar.SetToolAlignRight(IdReplace, true);

            var idSpacer = replaceToolBar.AddSpacer(4);
            replaceToolBar.SetToolAlignRight(idSpacer, true);
            IdReplaceAll = replaceToolBar.AddTextBtn(
                CommonStrings.Default.ButtonReplaceAll,
                CommonStrings.Default.ButtonReplaceAll);
            replaceToolBar.SetToolShortcut(IdReplaceAll, Keys.A | Keys.Alt);
            replaceToolBar.SetToolAlignRight(IdReplaceAll, true);

            replaceToolBar.Visible = false;
            replaceToolBar.Parent = this;

            findToolBar.AddToolAction(IdFindNext, OnClickFindNext);
            findToolBar.AddToolAction(IdFindPrevious, OnClickFindPrevious);
            findToolBar.AddToolAction(IdFindClose, OnClickClose);
            findToolBar.AddToolAction(IdToggleReplaceOptions, OnClickToggleReplace);

            replaceToolBar.AddToolAction(IdReplace, OnClickReplace);
            replaceToolBar.AddToolAction(IdReplaceAll, OnClickReplaceAll);
        }

        private void MenuItemUseRegularExpressions_Click(object? sender, EventArgs e)
        {
            menuItemUseRegularExpressions.Checked = !menuItemUseRegularExpressions.Checked;
        }

        private void MenuItemMatchWholeWord_Click(object? sender, EventArgs e)
        {
            menuItemMatchWholeWord.Checked = !menuItemMatchWholeWord.Checked;
        }

        private void MenuItemMatchCase_Click(object? sender, EventArgs e)
        {
            menuItemMatchCase.Checked = !menuItemMatchCase.Checked;
        }

        public EventHandler? ClickFindNext;

        public EventHandler? ClickFindPrevious;

        public EventHandler? ClickReplace;

        public EventHandler? ClickReplaceAll;

        public EventHandler? ClickClose;

        [Browsable(false)]
        public MenuItem MenuItemMatchCase => menuItemMatchCase;

        [Browsable(false)]
        public MenuItem MenuItemMatchWholeWord => menuItemMatchWholeWord;

        [Browsable(false)]
        public MenuItem MenuItemUseRegularExpressions => menuItemUseRegularExpressions;

        public bool OptionMatchCase
        {
            get
            {
                return MenuItemMatchCase.Checked;
            }

            set
            {
                MenuItemMatchCase.Checked = value;
            }
        }

        public bool OptionMatchWholeWord
        {
            get
            {
                return MenuItemMatchWholeWord.Checked;
            }

            set
            {
                MenuItemMatchWholeWord.Checked = value;
            }
        }

        public bool OptionUseRegularExpressions
        {
            get
            {
                return MenuItemUseRegularExpressions.Checked;
            }

            set
            {
                MenuItemUseRegularExpressions.Checked = value;
            }
        }

        [Browsable(false)]
        public ContextMenu OptionsMenu => optionsMenu;

        [Browsable(false)]
        public ObjectUniqueId IdToggleReplaceOptions { get; set; }

        [Browsable(false)]
        public ObjectUniqueId IdFindEdit { get; set; }

        [Browsable(false)]
        public ObjectUniqueId IdReplaceEdit { get; set; }

        [Browsable(false)]
        public ObjectUniqueId IdFindNext { get; set; }

        [Browsable(false)]
        public ObjectUniqueId IdFindPrevious { get; set; }

        [Browsable(false)]
        public ObjectUniqueId IdFindOptions { get; set; }

        [Browsable(false)]
        public ObjectUniqueId IdFindClose { get; set; }

        [Browsable(false)]
        public ObjectUniqueId IdReplace { get; set; }

        [Browsable(false)]
        public ObjectUniqueId IdReplaceAll { get; set; }

        [Browsable(false)]
        public TextBox FindEdit => findEdit;

        [Browsable(false)]
        public TextBox ReplaceEdit => replaceEdit;

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
                return replaceToolBar.Visible;
            }

            set
            {
                if (value)
                {
                    separatorPanel.Visible = true;
                    replaceToolBar.Visible = true;
                    findToolBar.SetToolDisabledImage(
                        IdToggleReplaceOptions,
                        findToolBar.GetDisabledSvgImages().ImgAngleUp);
                    findToolBar.SetToolImage(
                        IdToggleReplaceOptions,
                        findToolBar.GetNormalSvgImages().ImgAngleUp);
                }
                else
                {
                    separatorPanel.Visible = false;
                    replaceToolBar.Visible = false;
                    findToolBar.SetToolDisabledImage(
                        IdToggleReplaceOptions,
                        findToolBar.GetDisabledSvgImages().ImgAngleDown);
                    findToolBar.SetToolImage(
                        IdToggleReplaceOptions,
                        findToolBar.GetNormalSvgImages().ImgAngleDown);
                }
            }
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
