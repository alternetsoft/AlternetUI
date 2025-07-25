using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Implements main control of the Find and Replace dialogs.
    /// </summary>
    [ControlCategory("MenusAndToolbars")]
    public partial class FindReplaceControl : ToolBarSet, IFindReplaceControlHandler
    {
        /// <summary>
        /// Indicates whether the gear button is enabled by default.
        /// </summary>
        public static bool DefaultShowGearButton = false;

        /// <summary>
        /// Gets or sets the default minimum width of the edit controls in the
        /// Find and Replace dialogs.
        /// </summary>
        public static int DefaultMinEditWidth = 150;

        /// <summary>
        /// Gets or sets default border color of the find text editor
        /// in the case when search string is not found.
        /// This property contains default value for the light color theme.
        /// </summary>
        public static Color DefaultNotFoundBorderLight = (229, 20, 0);

        /// <summary>
        /// Gets or sets default border color of the find text editor
        /// in the case when search string is not found.
        /// This property contains default value for the dark color theme.
        /// </summary>
        public static Color DefaultNotFoundBorderDark = (255, 153, 164);

        private static Color? defaultFindEditBorderColorLight;
        private static Color? defaultFindEditBorderColorDark;

        private readonly ListPicker scopeEdit = new()
        {
            Margin = (2, 0, 2, 0),
            UseContextMenuAsPopup = true,
            VerticalAlignment = VerticalAlignment.Center,
        };

        private readonly TextBoxWithListPopup findEdit = new()
        {
            Margin = (2, 0, 2, 0),
            UseContextMenuAsPopup = true,
            VerticalAlignment = VerticalAlignment.Center,
        };

        private readonly TextBoxWithListPopup replaceEdit = new()
        {
            Margin = (2, 0, 2, 0),
            UseContextMenuAsPopup = true,
            VerticalAlignment = VerticalAlignment.Center,
        };

        private readonly ListControlItem scopeCurrentDocument = new()
        {
            Text = CommonStrings.Default.FindScopeCurrentDocument,
        };

        private readonly ListControlItem scopeAllOpenDocuments = new()
        {
            Text = CommonStrings.Default.FindScopeAllOpenDocuments,
        };

        private readonly ListControlItem scopeCurrentProject = new()
        {
            Text = CommonStrings.Default.FindScopeCurrentProject,
        };

        private readonly ListControlItem scopeSelectionOnly = new()
        {
            Text = CommonStrings.Default.FindScopeSelectionOnly,
        };

        private readonly ContextMenu moreActionsMenu = new();

        private IFindReplaceConnect? manager;
        private bool canFindInCurrentDocument = true;
        private bool canFindInAllOpenDocuments = true;
        private bool canFindInCurrentProject = true;
        private bool canFindInSelectionOnly = true;
        private bool showErrorBorder;

        /// <summary>
        /// Initializes a new instance of the <see cref="FindReplaceControl"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public FindReplaceControl(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FindReplaceControl"/> class.
        /// </summary>
        public FindReplaceControl()
        {
            findEdit.BorderColor = FindEditBorderColor;
            replaceEdit.BorderColor = FindEditBorderColor;
            scopeEdit.BorderColor = FindEditBorderColor;

            DoInsideLayout(Fn);

            void Fn()
            {
                UpdateFindScope();

                replaceEdit.DelayedTextChanged += OnReplaceEditTextChanged;
                findEdit.DelayedTextChanged += OnFindEditTextChanged;

                ToolBarCount = 3;

                /* Options ToolBar */

                IdOptionsEmptyButton1 = OptionsToolBar.AddSpeedBtn();

                IdMatchCase = OptionsToolBar.AddStickyBtn(
                    CommonStrings.Default.FindOptionMatchCase,
                    KnownSvgImages.ImgFindMatchCase,
                    null,
                    OnClickMatchCase);
                OptionsToolBar.SetToolShortcut(
                    IdMatchCase,
                    KnownShortcuts.FindReplaceControlKeys.MatchCase);

                IdMatchWholeWord = OptionsToolBar.AddStickyBtn(
                    CommonStrings.Default.FindOptionMatchWholeWord,
                    KnownSvgImages.ImgFindMatchFullWord,
                    null,
                    OnClickMatchWholeWord);
                OptionsToolBar.SetToolShortcut(
                    IdMatchWholeWord,
                    KnownShortcuts.FindReplaceControlKeys.MatchWholeWord);

                IdUseRegularExpressions = OptionsToolBar.AddStickyBtn(
                    CommonStrings.Default.FindOptionUseRegularExpressions,
                    KnownSvgImages.ImgRegularExpr,
                    null,
                    OnClickUseRegularExpressions);
                OptionsToolBar.SetToolShortcut(
                    IdUseRegularExpressions,
                    KnownShortcuts.FindReplaceControlKeys.UseRegularExpressions);

                scopeEdit.HorizontalAlignment = HorizontalAlignment.Fill;

                IdScopeEdit = OptionsToolBar.AddControl(scopeEdit);

                /* Find ToolBar */

                IdToggleReplaceOptions = FindToolBar.AddSpeedBtn(
                    CommonStrings.Default.ToggleToSwitchBetweenFindReplace,
                    KnownSvgImages.ImgAngleDown);

                findEdit.MainControl.EmptyTextHint = EmptyTextHints.FindEdit;
                findEdit.SyncTextAndComboButton();
                replaceEdit.SyncTextAndComboButton();

                findEdit.MinWidth = DefaultMinEditWidth;
                findEdit.HorizontalAlignment = HorizontalAlignment.Fill;

                IdFindEdit = FindToolBar.AddControl(findEdit);

                IdFindNext = FindToolBar.AddSpeedBtn(
                    CommonStrings.Default.ButtonFindNext,
                    KnownSvgImages.ImgArrowDown);
                FindToolBar.SetToolShortcut(
                    IdFindNext,
                    KnownShortcuts.FindReplaceControlKeys.FindNext);
                FindToolBar.SetToolAlignRight(IdFindNext, true);

                IdFindPrevious = FindToolBar.AddSpeedBtn(
                    CommonStrings.Default.ButtonFindPrevious,
                    KnownSvgImages.ImgArrowUp);
                FindToolBar.SetToolShortcut(
                    IdFindPrevious,
                    KnownShortcuts.FindReplaceControlKeys.FindPrevious);
                FindToolBar.SetToolAlignRight(IdFindPrevious, true);

                IdFindClose = FindToolBar.AddSpeedBtn(
                    CommonStrings.Default.ButtonClose,
                    KnownSvgImages.ImgCancel);
                FindToolBar.SetToolAlignRight(IdFindClose, true);

                /* Replace ToolBar */

                replaceEdit.MinWidth = DefaultMinEditWidth;
                replaceEdit.HorizontalAlignment = HorizontalAlignment.Fill;

                IdReplaceEmptyButton1 = ReplaceToolBar.AddSpeedBtn();

                IdReplaceEdit = ReplaceToolBar.AddControl(replaceEdit);
                replaceEdit.MainControl.EmptyTextHint = EmptyTextHints.ReplaceEdit;

                IdReplace = ReplaceToolBar.AddSpeedBtn(
                    CommonStrings.Default.ButtonReplace,
                    KnownSvgImages.ImgReplace);
                ReplaceToolBar.SetToolShortcut(
                    IdReplace,
                    KnownShortcuts.FindReplaceControlKeys.Replace);
                ReplaceToolBar.SetToolAlignRight(IdReplace, true);

                SpeedButton replaceAllButton = ReplaceToolBar.AddSpeedBtnCore(
                    CommonStrings.Default.ButtonReplaceAll,
                    KnownSvgImages.ImgReplaceAll);
                replaceAllButton.ShortcutKeyInfo = KnownShortcuts.FindReplaceControlKeys.ReplaceAll;
                replaceAllButton.HorizontalAlignment = HorizontalAlignment.Right;
                IdReplaceAll = replaceAllButton.UniqueId;

                if (DefaultShowGearButton)
                {
                    IdReplaceMoreActions = ReplaceToolBar.AddRightSpeedBtn(
                        CommonStrings.Default.ButtonOptions,
                        KnownSvgImages.ImgGear);
                    ReplaceToolBar.SetToolDropDownMenu(IdReplaceMoreActions, moreActionsMenu);
                }
                else
                {
                    IdReplaceMoreActions = ReplaceToolBar.AddRightSpeedBtn();
                }

                ReplaceToolBar.Visible = false;

                /* Specify click actions */

                FindToolBar.AddToolAction(IdFindNext, OnClickFindNext);
                FindToolBar.AddToolAction(IdFindPrevious, OnClickFindPrevious);
                FindToolBar.AddToolAction(IdFindClose, OnClickClose);
                FindToolBar.AddToolAction(IdToggleReplaceOptions, OnClickToggleReplace);

                ReplaceToolBar.AddToolAction(IdReplace, OnClickReplace);
                ReplaceToolBar.AddToolAction(IdReplaceAll, OnClickReplaceAll);

                ItemSize = 32;

                var itemToggleMatchCase = moreActionsMenu.Add(
                    CommonStrings.Default.FindOptionMatchCase,
                    ToggleMatchCase);

                var itemToggleWholeWord = moreActionsMenu.Add(
                    CommonStrings.Default.FindOptionMatchWholeWord,
                    ToggleWholeWord);

                var itemToggleHiddenText = moreActionsMenu.Add(
                    CommonStrings.Default.FindOptionHiddenText,
                    ToggleHiddenText);

                var itemToggleRegularExpressions = moreActionsMenu.Add(
                    CommonStrings.Default.FindOptionUseRegularExpressions,
                    ToggleRegularExpressions);

                var itemTogglePromptOnReplace = moreActionsMenu.Add(
                    CommonStrings.Default.FindOptionPromptOnReplace,
                    TogglePromptOnReplace);

                moreActionsMenu.Opening += (sender, e) =>
                {
                    itemToggleMatchCase.Checked = OptionMatchCase;
                    itemToggleWholeWord.Checked = OptionMatchWholeWord;
                    itemToggleHiddenText.Checked = OptionHiddenText;
                    itemToggleRegularExpressions.Checked = OptionUseRegularExpressions;
                    itemTogglePromptOnReplace.Checked = OptionPromptOnReplace;
                };
            }
        }

        /// <summary>
        /// Occurs when 'Find Next' button is clicked.
        /// </summary>
        public event EventHandler? ClickFindNext;

        /// <summary>
        /// Occurs when 'Find Previous' button is clicked.
        /// </summary>
        public event EventHandler? ClickFindPrevious;

        /// <summary>
        /// Occurs when 'Replace' button is clicked.
        /// </summary>
        public event EventHandler? ClickReplace;

        /// <summary>
        /// Occurs when 'Replace All' button is clicked.
        /// </summary>
        public event EventHandler? ClickReplaceAll;

        /// <summary>
        /// Occurs when 'Close' button is clicked.
        /// </summary>
        public event EventHandler? ClickClose;

        /// <summary>
        /// Occurs when option 'Match Case' is changed.
        /// </summary>
        public event EventHandler? OptionMatchCaseChanged;

        /// <summary>
        /// Occurs when option 'Match Whole Word' is changed.
        /// </summary>
        public event EventHandler? OptionMatchWholeWordChanged;

        /// <summary>
        /// Occurs when option 'Use Regular Expressions' is changed.
        /// </summary>
        public event EventHandler? OptionUseRegularExpressionsChanged;

        /// <summary>
        /// Enumerates all known search scopes.
        /// </summary>
        public enum SearchScope
        {
            /// <summary>
            /// Search scope is 'Current Document'.
            /// </summary>
            CurrentDocument,

            /// <summary>
            /// Search scope is 'All Open Documents'.
            /// </summary>
            AllOpenDocuments,

            /// <summary>
            /// Search scope is 'Current Project'.
            /// </summary>
            CurrentProject,

            /// <summary>
            /// Search scope is 'Selection Only'.
            /// </summary>
            SelectionOnly,
        }

        /// <summary>
        /// Provides methods and properties for connection of the search/replace engine with
        /// the <see cref="FindReplaceControl"/>.
        /// </summary>
        public interface IFindReplaceConnect
        {
            /// <summary>
            /// Gets whether 'Match Case' option is supported.
            /// </summary>
            bool CanMatchCase { get; }

            /// <summary>
            /// Gets whether 'Match Whole Word' option is supported.
            /// </summary>
            bool CanMatchWholeWord { get; }

            /// <summary>
            /// Gets whether 'Use Regular Expressions' option is supported.
            /// </summary>
            bool CanUseRegularExpressions { get; }

            /// <summary>
            /// Updates value of the 'Match Case' option.
            /// </summary>
            /// <param name="value">New option value.</param>
            void SetMatchCase(bool value);

            /// <summary>
            /// Updates value of the 'Match Whole Word' option.
            /// </summary>
            /// <param name="value">New option value.</param>
            void SetMatchWholeWord(bool value);

            /// <summary>
            /// Updates value of the 'Use Regular Expressions' option.
            /// </summary>
            /// <param name="value">New option value.</param>
            void SetUseRegularExpressions(bool value);

            /// <summary>
            /// Notifies when visibility of replace options is changed.
            /// </summary>
            /// <param name="value">New option value.</param>
            void SetReplaceVisible(bool value);

            /// <summary>
            /// Performs 'Replace' operation.
            /// </summary>
            void Replace();

            /// <summary>
            /// Performs 'Replace All' operation.
            /// </summary>
            void ReplaceAll();

            /// <summary>
            /// Performs 'Find Next' operation.
            /// </summary>
            void FindNext();

            /// <summary>
            /// Performs 'Find Previous' operation.
            /// </summary>
            void FindPrevious();

            /// <summary>
            /// Sets text to find.
            /// </summary>
            void SetFindText(string text);

            /// <summary>
            /// Sets text to replace.
            /// </summary>
            /// <param name="text"></param>
            void SetReplaceText(string text);
        }

        /// <summary>
        /// Gets or sets default border color of the find text editor.
        /// This property contains default value for the light color theme.
        /// </summary>
        public static Color DefaultFindEditBorderColorLight
        {
            get
            {
                return defaultFindEditBorderColorLight ?? DefaultColors.GetBorderColor(false);
            }

            set
            {
                defaultFindEditBorderColorLight = value;
            }
        }

        /// <summary>
        /// Gets or sets default border color of the find text editor.
        /// This property contains default value for the light color theme.
        /// </summary>
        public static Color DefaultFindEditBorderColorDark
        {
            get
            {
                return defaultFindEditBorderColorDark ?? DefaultColors.GetBorderColor(true);
            }

            set
            {
                defaultFindEditBorderColorDark = value;
            }
        }

        /// <summary>
        /// Gets the find editor control as <see cref="IControl"/>.
        /// </summary>
        [Browsable(false)]
        public IControl FindEditor
        {
            get
            {
                return FindEdit;
            }
        }

        /// <summary>
        /// Gets the replace editor control as <see cref="IControl"/>.
        /// </summary>
        [Browsable(false)]
        public IControl ReplaceEditor
        {
            get
            {
                return ReplaceEdit;
            }
        }

        /// <summary>
        /// Gets drop down menu for gear button. Use <see cref="DefaultShowGearButton"/>
        /// in order to show the gear button.
        /// </summary>
        [Browsable(false)]
        public ContextMenu MoreActionsMenu
        {
            get
            {
                return moreActionsMenu;
            }
        }

        /// <summary>
        /// Gets search scope as <see cref="SearchScope"/>.
        /// </summary>
        [Browsable(false)]
        public virtual SearchScope? Scope
        {
            get
            {
                if (IsScopeAllOpenDocuments)
                    return FindReplaceControl.SearchScope.AllOpenDocuments;
                if (IsScopeCurrentDocument)
                    return FindReplaceControl.SearchScope.CurrentDocument;
                if (IsScopeCurrentProject)
                    return FindReplaceControl.SearchScope.CurrentProject;
                if (IsScopeSelectionOnly)
                    return FindReplaceControl.SearchScope.SelectionOnly;
                return null;
            }

            set
            {
                if (Scope == value)
                    return;
                if (value is null)
                {
                    scopeEdit.Value = null;
                    return;
                }

                switch (value)
                {
                    case SearchScope.CurrentDocument:
                        IsScopeCurrentDocument = true;
                        break;
                    case SearchScope.AllOpenDocuments:
                        IsScopeAllOpenDocuments = true;
                        break;
                    case SearchScope.CurrentProject:
                        IsScopeCurrentProject = true;
                        break;
                    case SearchScope.SelectionOnly:
                        IsScopeSelectionOnly = true;
                        break;
                    default:
                        scopeEdit.Value = null;
                        break;
                }
            }
        }

        /// <summary>
        /// Gets whether selected scope is 'All open documents'.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsScopeAllOpenDocuments
        {
            get
            {
                return scopeEdit.Value == scopeAllOpenDocuments;
            }

            set
            {
                if (IsScopeAllOpenDocuments == value)
                    return;
                if (value)
                {
                    if (canFindInAllOpenDocuments)
                        scopeEdit.Value = scopeAllOpenDocuments;
                }
                else
                {
                    scopeEdit.Value = null;
                }
            }
        }

        /// <summary>
        /// Represents the search history.
        /// </summary>
        public virtual ListBoxItems SearchList
        {
            get => FindEdit.SimpleItems;
        }

        /// <summary>
        /// Represents the replace history.
        /// </summary>
        public virtual ListBoxItems ReplaceList
        {
            get => ReplaceEdit.SimpleItems;
        }

        /// <summary>
        /// Gets or sets the text to find in the search operation.
        /// </summary>
        public string TextToFind
        {
            get => FindEdit.Text;
            set => FindEdit.Text = value;
        }

        /// <summary>
        /// Gets or sets the text to replace in the replace operation.
        /// </summary>
        public string TextToReplace
        {
            get => ReplaceEdit.Text;
            set => ReplaceEdit.Text = value;
        }

        /// <summary>
        /// Gets whether selected scope is 'Current project'.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsScopeCurrentProject
        {
            get
            {
                return scopeEdit.Value == scopeCurrentProject;
            }

            set
            {
                if (IsScopeCurrentProject == value)
                    return;
                if (value)
                {
                    if (canFindInCurrentProject)
                        scopeEdit.Value = scopeCurrentProject;
                }
                else
                {
                    scopeEdit.Value = null;
                }
            }
        }

        /// <summary>
        /// Gets whether selected scope is 'Selection only'.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsScopeSelectionOnly
        {
            get
            {
                return scopeEdit.Value == scopeSelectionOnly;
            }

            set
            {
                if (IsScopeSelectionOnly == value)
                    return;
                if (value)
                {
                    if (canFindInSelectionOnly)
                        scopeEdit.Value = scopeSelectionOnly;
                }
                else
                {
                    scopeEdit.Value = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets a hint shown in an empty unfocused find text editor.
        /// </summary>
        public virtual string? FindEditEmptyTextHint
        {
            get
            {
                return FindEdit.MainControl.EmptyTextHint;
            }

            set
            {
                FindEdit.MainControl.EmptyTextHint = value;
            }
        }

        /// <summary>
        /// Gets or sets a hint shown in an empty unfocused replace text editor.
        /// </summary>
        public virtual string? ReplaceEditEmptyTextHint
        {
            get
            {
                return ReplaceEdit.MainControl.EmptyTextHint;
            }

            set
            {
                ReplaceEdit.MainControl.EmptyTextHint = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the scope edit control is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if the scope edit control is enabled; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsScopeEditEnabled
        {
            get
            {
                return ScopeEdit.Enabled;
            }

            set
            {
                ScopeEdit.Enabled = value;
            }
        }

        /// <summary>
        /// Gets whether selected scope is 'Current document'.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsScopeCurrentDocument
        {
            get
            {
                return scopeEdit.Value == scopeCurrentDocument;
            }

            set
            {
                if (IsScopeCurrentDocument == value)
                    return;
                if (value)
                {
                    if (canFindInCurrentDocument)
                        scopeEdit.Value = scopeCurrentDocument;
                }
                else
                {
                    scopeEdit.Value = null;
                }
            }
        }

        /// <summary>
        /// Gets border of the <see cref="FindEdit"/>.
        /// </summary>
        [Browsable(false)]
        public Border FindEditBorder => findEdit;

        /// <summary>
        /// Gets border of the <see cref="ReplaceEdit"/>.
        /// </summary>
        [Browsable(false)]
        public Border ReplaceEditBorder => replaceEdit;

        /// <summary>
        /// Gets border color of the find text editor.
        /// </summary>
        [Browsable(false)]
        public virtual Color FindEditBorderColor
        {
            get
            {
                if (IsDarkBackground)
                    return DefaultFindEditBorderColorDark;
                else
                    return DefaultFindEditBorderColorLight;
            }
        }

        /// <summary>
        /// Gets or sets border color of the find text editor
        /// in the case when search string is not found.
        /// </summary>
        [Browsable(false)]
        public virtual Color NotFoundBorderColor
        {
            get
            {
                if (IsDarkBackground)
                    return DefaultNotFoundBorderDark;
                else
                    return DefaultNotFoundBorderLight;
            }
        }

        /// <summary>
        /// Gets or sets whether to show error border around find text editor.
        /// </summary>
        public virtual bool ShowErrorBorder
        {
            get
            {
                return showErrorBorder;
            }

            set
            {
                if (showErrorBorder == value)
                    return;
                showErrorBorder = value;
                if (value)
                {
                    findEdit.BorderColor = NotFoundBorderColor;
                }
                else
                {
                    findEdit.BorderColor = FindEditBorderColor;
                }
            }
        }

        /// <summary>
        /// Get or sets whether 'Current Document' find scope is available.
        /// </summary>
        public virtual bool CanFindInCurrentDocument
        {
            get
            {
                return canFindInCurrentDocument;
            }

            set
            {
                if (canFindInCurrentDocument == value)
                    return;
                canFindInCurrentDocument = value;
                UpdateFindScope();
            }
        }

        /// <summary>
        /// Get or sets whether 'All Open Documents' find scope is available.
        /// </summary>
        public virtual bool CanFindInAllOpenDocuments
        {
            get
            {
                return canFindInAllOpenDocuments;
            }

            set
            {
                if (canFindInAllOpenDocuments == value)
                    return;
                canFindInAllOpenDocuments = value;
                UpdateFindScope();
            }
        }

        /// <summary>
        /// Get or sets whether 'Current Project' find scope is available.
        /// </summary>
        public virtual bool CanFindInCurrentProject
        {
            get
            {
                return canFindInCurrentProject;
            }

            set
            {
                if (canFindInCurrentProject == value)
                    return;
                canFindInCurrentProject = value;
                UpdateFindScope();
            }
        }

        /// <summary>
        /// Get or sets whether 'Current Project' find scope is available.
        /// </summary>
        public virtual bool CanFindInSelectionOnly
        {
            get
            {
                return canFindInSelectionOnly;
            }

            set
            {
                if (canFindInSelectionOnly == value)
                    return;
                canFindInSelectionOnly = value;
                UpdateFindScope();
            }
        }

        /// <summary>
        /// Gets or sets <see cref="IFindReplaceConnect"/> instance.
        /// </summary>
        [Browsable(false)]
        public virtual IFindReplaceConnect? Manager
        {
            get => manager;

            set
            {
                if (manager == value)
                    return;
                manager = value;

                if (manager is null)
                    return;

                OptionMatchCaseEnabled = manager.CanMatchCase;
                OptionMatchWholeWordEnabled = manager.CanMatchWholeWord;
                OptionUseRegularExpressionsEnabled = manager.CanUseRegularExpressions;
            }
        }

        /// <summary>
        /// Gets 'Current Document' item in the <see cref="ScopeEdit"/>.
        /// </summary>
        [Browsable(false)]
        public ListControlItem ScopeItemCurrentDocument => scopeCurrentDocument;

        /// <summary>
        /// Gets 'All Open Documents' item in the <see cref="ScopeEdit"/>.
        /// </summary>
        [Browsable(false)]
        public ListControlItem ScopeItemAllOpenDocuments => scopeAllOpenDocuments;

        /// <summary>
        /// Gets 'Current Project' item in the <see cref="ScopeEdit"/>.
        /// </summary>
        [Browsable(false)]
        public ListControlItem ScopeItemCurrentProject => scopeCurrentProject;

        /// <summary>
        /// Gets 'Selection Only' item in the <see cref="ScopeEdit"/>.
        /// </summary>
        [Browsable(false)]
        public ListControlItem ScopeItemSelectionOnly => scopeSelectionOnly;

        /// <summary>
        /// Gets or sets 'Find Text At Cursor' option.
        /// </summary>
        public virtual bool OptionFindTextAtCursor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets 'Prompt On Replace' option.
        /// </summary>
        public virtual bool OptionPromptOnReplace
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets 'Hidden Text' option.
        /// </summary>
        public virtual bool OptionHiddenText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets 'Search Up' option.
        /// </summary>
        public virtual bool OptionSearchUp
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets 'Match Case' option.
        /// </summary>
        public virtual bool OptionMatchCase
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
                Manager?.SetMatchCase(value);
                OptionMatchCaseChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets whether 'Match Case' option is enabled.
        /// </summary>
        public virtual bool OptionMatchCaseEnabled
        {
            get
            {
                return OptionsToolBar.GetToolEnabled(IdMatchCase);
            }

            set
            {
                if (OptionMatchCaseEnabled == value)
                    return;
                OptionsToolBar.SetToolEnabled(IdMatchCase, value);
            }
        }

        /// <summary>
        /// Gets or sets 'Match Whole Word' option.
        /// </summary>
        public virtual bool OptionMatchWholeWord
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
                Manager?.SetMatchWholeWord(value);
                OptionMatchWholeWordChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets whether 'Match Whole Word' option is enabled.
        /// </summary>
        public virtual bool OptionMatchWholeWordEnabled
        {
            get
            {
                return OptionsToolBar.GetToolEnabled(IdMatchWholeWord);
            }

            set
            {
                if (OptionMatchWholeWordEnabled == value)
                    return;
                OptionsToolBar.SetToolEnabled(IdMatchWholeWord, value);
            }
        }

        /// <summary>
        /// Gets or sets 'Use Regular Expressions' option.
        /// </summary>
        public virtual bool OptionUseRegularExpressions
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
                Manager?.SetUseRegularExpressions(value);
                OptionUseRegularExpressionsChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets whether 'Use Regular Expressions' option is enabled.
        /// </summary>
        public virtual bool OptionUseRegularExpressionsEnabled
        {
            get
            {
                return OptionsToolBar.GetToolEnabled(IdUseRegularExpressions);
            }

            set
            {
                if (OptionUseRegularExpressionsEnabled == value)
                    return;
                OptionsToolBar.SetToolEnabled(IdUseRegularExpressions, value);
            }
        }

        /// <summary>
        /// Gets or sets whether 'Use Regular Expressions' option is visible.
        /// </summary>
        public virtual bool OptionUseRegularExpressionsVisible
        {
            get
            {
                return OptionsToolBar.GetToolVisible(IdUseRegularExpressions);
            }

            set
            {
                if (OptionUseRegularExpressionsVisible == value)
                    return;
                OptionsToolBar.SetToolVisible(IdUseRegularExpressions, value);
            }
        }

        /// <summary>
        /// Gets or sets whether 'Close' button is visible.
        /// </summary>
        public virtual bool CloseButtonVisible
        {
            get
            {
                return FindToolBar.GetToolVisible(IdFindClose);
            }

            set
            {
                FindToolBar.SetToolVisible(IdFindClose, value);
            }
        }

        /// <summary>
        /// Gets id of the 'Match Case' button.
        /// </summary>
        [Browsable(false)]
        public ObjectUniqueId IdMatchCase { get; internal set; }

        /// <summary>
        /// Gets id of the 'Match Whole Word' button.
        /// </summary>
        [Browsable(false)]
        public ObjectUniqueId IdMatchWholeWord { get; internal set; }

        /// <summary>
        /// Gets id of the 'Use Regular Expressions' button.
        /// </summary>
        [Browsable(false)]
        public ObjectUniqueId IdUseRegularExpressions { get; internal set; }

        /// <summary>
        /// Gets id of the 'Toggle Replace Options' button.
        /// </summary>
        [Browsable(false)]
        public ObjectUniqueId IdToggleReplaceOptions { get; internal set; }

        /// <summary>
        /// Gets id of the 'Find' editor.
        /// </summary>
        [Browsable(false)]
        public ObjectUniqueId IdFindEdit { get; internal set; }

        /// <summary>
        /// Gets id of the 'Scope' editor.
        /// </summary>
        [Browsable(false)]
        public ObjectUniqueId IdScopeEdit { get; internal set; }

        /// <summary>
        /// Gets id of the 'Replace' editor.
        /// </summary>
        [Browsable(false)]
        public ObjectUniqueId IdReplaceEdit { get; internal set; }

        /// <summary>
        /// Gets id of the 'Find Next' button.
        /// </summary>
        [Browsable(false)]
        public ObjectUniqueId IdFindNext { get; internal set; }

        /// <summary>
        /// Gets id of the 'Find Previous' button.
        /// </summary>
        [Browsable(false)]
        public ObjectUniqueId IdFindPrevious { get; internal set; }

        /// <summary>
        /// Gets id of the 'Close' button.
        /// </summary>
        [Browsable(false)]
        public ObjectUniqueId IdFindClose { get; internal set; }

        /// <summary>
        /// Gets id of the first empty button on the replace toolbar.
        /// </summary>
        [Browsable(false)]
        public ObjectUniqueId IdReplaceEmptyButton1 { get; internal set; }

        /// <summary>
        /// Gets id of the first empty button on the options toolbar.
        /// </summary>
        [Browsable(false)]
        public ObjectUniqueId IdOptionsEmptyButton1 { get; internal set; }

        /// <summary>
        /// Gets id of the last empty button on the replace toolbar.
        /// </summary>
        [Browsable(false)]
        public ObjectUniqueId IdReplaceMoreActions { get; internal set; }

        /// <summary>
        /// Gets id of the 'Replace' button.
        /// </summary>
        [Browsable(false)]
        public ObjectUniqueId IdReplace { get; internal set; }

        /// <summary>
        /// Gets id of the 'Replace All' button.
        /// </summary>
        [Browsable(false)]
        public ObjectUniqueId IdReplaceAll { get; internal set; }

        /// <summary>
        /// Gets <see cref="ComboBox"/> which allows to specify text to find.
        /// </summary>
        [Browsable(false)]
        public TextBoxWithListPopup FindEdit => findEdit;

        /// <summary>
        /// Gets <see cref="ListPicker"/> which allows to specify text to find.
        /// </summary>
        [Browsable(false)]
        public ListPicker ScopeEdit => scopeEdit;

        /// <summary>
        /// Gets <see cref="ComboBox"/> which allows to specify text to replace.
        /// </summary>
        [Browsable(false)]
        public TextBoxWithListPopup ReplaceEdit => replaceEdit;

        /// <summary>
        /// Gets <see cref="ToolBar"/> with find buttons.
        /// </summary>
        [Browsable(false)]
        public ToolBar FindToolBar => GetToolBar(0);

        /// <summary>
        /// Gets <see cref="ToolBar"/> with replace buttons.
        /// </summary>
        [Browsable(false)]
        public ToolBar ReplaceToolBar => GetToolBar(1);

        /// <summary>
        /// Gets <see cref="ToolBar"/> with option buttons.
        /// </summary>
        [Browsable(false)]
        public ToolBar OptionsToolBar => GetToolBar(2);

        /// <summary>
        /// Gets or sets width of <see cref="FindEdit"/> and <see cref="ReplaceEdit"/>
        /// controls.
        /// </summary>
        [Browsable(false)]
        public virtual Coord TextBoxWidth
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

        /// <summary>
        /// Gets or sets whether keys specified in <see cref="KnownShortcuts.FindReplaceControlKeys"/>
        /// are automatically handled.
        /// </summary>
        public virtual bool WantKeys { get; set; } = true;

        /// <summary>
        /// Gets whether the user can perform 'Find Next' action.
        /// Returns <c>true</c> if <see cref="FindToolBar"/> and 'Find Next' button are visible.
        /// </summary>
        [Browsable(false)]
        public virtual bool CanFindNext =>
            FindToolBar.Visible && FindToolBar.GetToolEnabledAndVisible(IdFindNext);

        /// <summary>
        /// Gets whether the user can perform 'Find Previous' action.
        /// Returns <c>true</c> if <see cref="FindToolBar"/> and 'Find Previous' button are visible.
        /// </summary>
        [Browsable(false)]
        public virtual bool CanFindPrevious =>
            FindToolBar.Visible && FindToolBar.GetToolEnabledAndVisible(IdFindPrevious);

        /// <summary>
        /// Gets whether the user can perform 'Replace' action.
        /// Returns <c>true</c> if <see cref="ReplaceToolBar"/> and 'Replace' button are visible.
        /// </summary>
        [Browsable(false)]
        public virtual bool CanReplace =>
            ReplaceVisible && ReplaceToolBar.GetToolEnabledAndVisible(IdReplace);

        /// <summary>
        /// Gets whether the user can perform 'Replace All' action.
        /// Returns <c>true</c> if <see cref="ReplaceToolBar"/> and 'Replace All' button are visible.
        /// </summary>
        [Browsable(false)]
        public virtual bool CanReplaceAll =>
            ReplaceVisible && ReplaceToolBar.GetToolEnabledAndVisible(IdReplaceAll);

        /// <summary>
        /// Gets whether the user can perform 'Match Case' action.
        /// Returns <c>true</c> if <see cref="OptionsToolBar"/> and 'Match Case' button are visible.
        /// </summary>
        [Browsable(false)]
        public virtual bool CanMatchCase =>
            OptionsToolBar.Visible && OptionsToolBar.GetToolEnabledAndVisible(IdMatchCase);

        /// <summary>
        /// Gets whether the user can perform 'Match Whole Word' action.
        /// Returns <c>true</c> if <see cref="OptionsToolBar"/> and 'Match Whole Word'
        /// button are visible.
        /// </summary>
        [Browsable(false)]
        public virtual bool CanMatchWholeWord =>
            OptionsToolBar.Visible && OptionsToolBar.GetToolEnabledAndVisible(IdMatchWholeWord);

        /// <summary>
        /// Gets whether the user can perform 'Use Regular Expressions' action.
        /// Returns <c>true</c> if <see cref="OptionsToolBar"/> and
        /// 'Use Regular Expressions' button are visible.
        /// </summary>
        [Browsable(false)]
        public virtual bool CanUseRegularExpressions =>
            OptionsToolBar.Visible && OptionsToolBar.GetToolEnabledAndVisible(IdUseRegularExpressions);

        /// <summary>
        /// Gets or sets whether <see cref="OptionsToolBar"/> is visible.
        /// </summary>
        public virtual bool OptionsVisible
        {
            get
            {
                return OptionsToolBar.Visible;
            }

            set
            {
                OptionsToolBar.Visible = value;
            }
        }

        /// <summary>
        /// Gets or sets whether toggle replace options button is visible.
        /// </summary>
        public virtual bool ToggleReplaceVisible
        {
            get
            {
                var result = FindToolBar.GetToolVisible(IdToggleReplaceOptions);
                return result;
            }

            set
            {
                if (ToggleReplaceVisible == value)
                    return;
                FindToolBar.SetToolVisible(IdToggleReplaceOptions, value);
                OptionsToolBar.SetToolVisible(IdOptionsEmptyButton1, value);
                ReplaceToolBar.SetToolVisible(IdReplaceEmptyButton1, value);
            }
        }

        /// <summary>
        /// Gets or sets whether <see cref="ReplaceToolBar"/> is visible.
        /// </summary>
        public virtual bool ReplaceVisible
        {
            get
            {
                return ReplaceToolBar.Visible;
            }

            set
            {
                if (ReplaceVisible == value)
                    return;
                ReplaceToolBar.Visible = value;
                FindToolBar.SetToolDisabledImage(
                    IdToggleReplaceOptions,
                    FindToolBar.ToDisabled(KnownSvgImages.GetImgAngleUpDown(value)));
                FindToolBar.SetToolImage(
                    IdToggleReplaceOptions,
                    FindToolBar.ToNormal(KnownSvgImages.GetImgAngleUpDown(value)));
                Manager?.SetReplaceVisible(value);
            }
        }

        [Browsable(false)]
        internal new Color? ForegroundColor
        {
            get => base.ForegroundColor;
            set => base.ForegroundColor = value;
        }

        [Browsable(false)]
        internal new string Text
        {
            get => base.Text;
            set => base.Text = value;
        }

        /// <summary>
        /// Creates <see cref="FindReplaceControl"/> inside <see cref="DialogWindow"/>.
        /// </summary>
        /// <param name="replace">true if replace options are visible.</param>
        /// <returns></returns>
        public static (DialogWindow Dialog, FindReplaceControl Control) CreateInsideDialog(
            bool replace)
        {
            var parentWindow = new DialogWindow();
            parentWindow.Disposed += ParentWindow_Disposed;
            parentWindow.MinimizeEnabled = false;
            parentWindow.MaximizeEnabled = false;
            parentWindow.EscModalResult = ModalResult.Canceled;
            parentWindow.HasSystemMenu = false;
            parentWindow.Title = CommonStrings.Default.WindowTitleSearchAndReplace;

            FindReplaceControl findReplace = new();
            findReplace.Manager = findReplace.CreateLogger();
            findReplace.CloseButtonVisible = false;
            findReplace.ReplaceVisible = replace;
            findReplace.Parent = parentWindow;

            parentWindow.SetSizeToContent(WindowSizeToContentMode.Height);

            return (parentWindow, findReplace);

            static void ParentWindow_Disposed(object? sender, EventArgs e)
            {
                App.DebugLogIf("FindReplace window disposed", false);
            }
        }

        /// <summary>
        /// Creates <see cref="IFindReplaceConnect"/> instance which logs all method calls.
        /// </summary>
        /// <returns></returns>
        public virtual IFindReplaceConnect CreateLogger()
        {
            return new FindReplaceManagerLogger();
        }

        /// <summary>
        /// Handles keys specified in <see cref="KnownShortcuts.FindReplaceControlKeys"/>.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        /// <remarks>
        /// You can use this method in the <see cref="AbstractControl.KeyDown"/> event handlers.
        /// <see cref="WantKeys"/> specifies whether <see cref="HandleKeys"/>
        /// is called automatically.
        /// </remarks>
        public virtual bool HandleKeys(KeyEventArgs e)
        {
            bool Run(KeyInfo[] keys, Action? action)
            {
                return KeyInfo.Run(keys, e, action);
            }

            if (CanFindNext && Run(KnownShortcuts.FindReplaceControlKeys.FindNext, OnClickFindNext))
                return true;
            if (CanFindPrevious
                && Run(KnownShortcuts.FindReplaceControlKeys.FindPrevious, OnClickFindPrevious))
                return true;
            if (CanReplace && Run(KnownShortcuts.FindReplaceControlKeys.Replace, OnClickReplace))
                return true;
            if (CanReplaceAll
                && Run(KnownShortcuts.FindReplaceControlKeys.ReplaceAll, OnClickReplaceAll))
                return true;
            if (CanMatchCase && Run(KnownShortcuts.FindReplaceControlKeys.MatchCase, OnClickMatchCase))
                return true;
            if (CanMatchWholeWord
                && Run(KnownShortcuts.FindReplaceControlKeys.MatchWholeWord, OnClickMatchWholeWord))
                return true;
            if (CanUseRegularExpressions && Run(
                KnownShortcuts.FindReplaceControlKeys.UseRegularExpressions,
                OnClickUseRegularExpressions))
                return true;
            return false;
        }

        /// <summary>
        /// Selects all the text in the find editor control.
        /// </summary>
        public void SelectAllTextInFindEditor()
        {
            FindEdit.MainControl.SelectAll();
        }

        /// <summary>
        /// Toggles the state of the prompt option for replace operations.
        /// </summary>
        /// <remarks>This method inverts the current setting of the <see cref="OptionPromptOnReplace"/>
        /// property, enabling or disabling the prompt for replace operations.</remarks>
        public void TogglePromptOnReplace()
        {
            OptionPromptOnReplace = !OptionPromptOnReplace;
        }

        /// <summary>
        /// Toggles searching through hidden text on/off.
        /// </summary>
        public void ToggleHiddenText()
        {
            OptionHiddenText = !OptionHiddenText;
        }

        /// <summary>
        /// Toggles case sensitive searching on/off.
        /// </summary>
        public void ToggleMatchCase()
        {
            OptionMatchCase = !OptionMatchCase;
        }

        /// <summary>
        /// Toggles using regular expressions on/off.
        /// </summary>
        public void ToggleRegularExpressions()
        {
            OptionUseRegularExpressions = !OptionUseRegularExpressions;
        }

        /// <summary>
        /// Toggles searching direction towards/backwards.
        /// </summary>
        public void ToggleSearchUp()
        {
            OptionSearchUp = !OptionSearchUp;
        }

        /// <summary>
        /// Toggles searching for whole words on/off.
        /// </summary>
        public void ToggleWholeWord()
        {
            OptionMatchWholeWord = !OptionMatchWholeWord;
        }

        internal void OnClickToggleReplace() => OnClickToggleReplace(this, EventArgs.Empty);

        internal void OnClickClose() => OnClickClose(this, EventArgs.Empty);

        /// <inheritdoc/>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (WantKeys)
                HandleKeys(e);
        }

        /// <summary>
        /// Invokes the event handler for the "Use Regular Expressions" button click event.
        /// </summary>
        /// <remarks>This method raises the event associated with clicking the
        /// "Use Regular Expressions"
        /// button. Derived classes can override this method to provide custom
        /// handling for the event.</remarks>
        protected virtual void OnClickUseRegularExpressions()
            => OnClickUseRegularExpressions(this, EventArgs.Empty);

        /// <summary>
        /// Handles the event when the "Match Whole Word" option is clicked.
        /// </summary>
        /// <remarks>This method raises the event associated with clicking the "Match Whole Word" option.
        /// Override this method in a derived class to provide custom handling for the event.</remarks>
        protected virtual void OnClickMatchWholeWord() => OnClickMatchWholeWord(this, EventArgs.Empty);

        /// <summary>
        /// Handles the event when the "Match Case" option is clicked.
        /// </summary>
        /// <remarks>This method raises the event associated with the "Match Case" option being clicked.
        /// Override this method in a derived class to provide custom handling for the event.</remarks>
        protected virtual void OnClickMatchCase() => OnClickMatchCase(this, EventArgs.Empty);

        /// <summary>
        /// Invokes the Find Next operation when the associated event is triggered.
        /// </summary>
        /// <remarks>This method raises the event by calling the
        /// <see cref="OnClickFindNext(object, EventArgs)"/> method.
        /// Override this method in a derived class to provide
        /// custom handling for the Find Next
        /// operation.</remarks>
        protected virtual void OnClickFindNext() => OnClickFindNext(this, EventArgs.Empty);

        /// <summary>
        /// Handles the event when the "Find Previous" button is clicked.
        /// </summary>
        /// <remarks>This method raises the event associated with finding the previous occurrence in a
        /// search operation. Override this method in a derived class to provide custom
        /// handling for the "Find Previous"
        /// action.</remarks>
        protected virtual void OnClickFindPrevious() => OnClickFindPrevious(this, EventArgs.Empty);

        /// <summary>
        /// Raises the click event for the replace action.
        /// </summary>
        /// <remarks>This method is called to trigger the replace action when a click event occurs.
        /// It can be overridden in a derived class to provide custom
        /// handling for the replace action.</remarks>
        protected virtual void OnClickReplace() => OnClickReplace(this, EventArgs.Empty);

        /// <summary>
        /// Handles the event when the "Replace All" button is clicked.
        /// </summary>
        /// <remarks>This method is invoked to trigger the "Replace All" functionality. It raises the
        /// event associated with replacing all occurrences of a specified item.
        /// Override this method in a derived class to provide custom handling for the "Replace All" action.</remarks>
        protected virtual void OnClickReplaceAll() => OnClickReplaceAll(this, EventArgs.Empty);

        /// <summary>
        /// Handles the event when the "Use Regular Expressions" button is clicked.
        /// Toggles the state of the option to use regular expressions.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnClickUseRegularExpressions(object? sender, EventArgs e)
        {
            OptionUseRegularExpressions = !OptionUseRegularExpressions;
        }

        /// <summary>
        /// Handles the event when the "Match Whole Word" button is clicked.
        /// Toggles the state of the <see cref="OptionMatchWholeWord"/> option.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        protected virtual void OnClickMatchWholeWord(object? sender, EventArgs e)
        {
            OptionMatchWholeWord = !OptionMatchWholeWord;
        }

        /// <summary>
        /// Handles the event when the "Match Case" button is clicked.
        /// Toggles the state of the match case option.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        protected virtual void OnClickMatchCase(object? sender, EventArgs e)
        {
            OptionMatchCase = !OptionMatchCase;
        }

        /// <summary>
        /// Handles the event when the "Find Next" button is clicked.
        /// Invokes the <see cref="ClickFindNext"/> event and calls the
        /// <see cref="IFindReplaceConnect.FindNext"/> method on the <see cref="Manager"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnClickFindNext(object? sender, EventArgs e)
        {
            ClickFindNext?.Invoke(this, e);
            Manager?.FindNext();
        }

        /// <summary>
        /// Handles the event when the "Toggle Replace" button is clicked.
        /// Toggles the visibility of the replace toolbar.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnClickToggleReplace(object? sender, EventArgs e)
        {
            ReplaceVisible = !ReplaceVisible;
        }

        /// <summary>
        /// Handles the event when the "Find Previous" button is clicked.
        /// Invokes the <see cref="ClickFindPrevious"/> event and calls the
        /// <see cref="IFindReplaceConnect.FindPrevious"/> method on the <see cref="Manager"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnClickFindPrevious(object? sender, EventArgs e)
        {
            ClickFindPrevious?.Invoke(this, e);
            Manager?.FindPrevious();
        }

        /// <summary>
        /// Handles the event when the "Replace" button is clicked.
        /// Invokes the <see cref="ClickReplace"/> event and calls the
        /// <see cref="IFindReplaceConnect.Replace"/> method on the <see cref="Manager"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnClickReplace(object? sender, EventArgs e)
        {
            ClickReplace?.Invoke(this, e);
            Manager?.Replace();
        }

        /// <summary>
        /// Handles the event when the "Replace All" button is clicked.
        /// Invokes the <see cref="ClickReplaceAll"/> event and
        /// calls the <see cref="IFindReplaceConnect.ReplaceAll"/> method on the <see cref="Manager"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnClickReplaceAll(object? sender, EventArgs e)
        {
            ClickReplaceAll?.Invoke(this, e);
            Manager?.ReplaceAll();
        }

        /// <summary>
        /// Handles the event when the "Close" button is clicked.
        /// Invokes the <see cref="ClickClose"/> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnClickClose(object? sender, EventArgs e)
        {
            ClickClose?.Invoke(this, e);
        }

        /// <summary>
        /// Handles the event when the text in the find editor is changed.
        /// Updates the find text in the <see cref="Manager"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnFindEditTextChanged(object? sender, EventArgs e)
        {
            Post(() =>
            {
                var s = findEdit.Text;
                Manager?.SetFindText(s);
            });
        }

        /// <summary>
        /// Handles the event when the text in the replace editor is changed.
        /// Updates the replace text in the <see cref="Manager"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnReplaceEditTextChanged(object? sender, EventArgs e)
        {
            Post(() =>
            {
                Manager?.SetReplaceText(replaceEdit.Text);
            });
        }

        private void UpdateFindScope()
        {
            void AddOrRemove(ListControlItem item, bool add)
            {
                if (add)
                {
                    if (scopeEdit.Items.IndexOf(item) < 0)
                        scopeEdit.Items.Add(item);
                }
                else
                    scopeEdit.Items.Remove(item);
            }

            var selected = false;

            void SelectIf(object? item, bool select)
            {
                if (select && !selected)
                {
                    scopeEdit.Value = item;
                    selected = true;
                }
            }

            AddOrRemove(scopeCurrentDocument, CanFindInCurrentDocument);
            AddOrRemove(scopeAllOpenDocuments, CanFindInAllOpenDocuments);
            AddOrRemove(scopeCurrentProject, CanFindInCurrentProject);
            AddOrRemove(scopeSelectionOnly, CanFindInSelectionOnly);

            SelectIf(scopeCurrentDocument, CanFindInCurrentDocument);
            SelectIf(scopeAllOpenDocuments, CanFindInAllOpenDocuments);
            SelectIf(scopeCurrentProject, CanFindInCurrentProject);
            SelectIf(scopeSelectionOnly, CanFindInSelectionOnly);
            SelectIf(null, true);
        }

        internal class FindReplaceManagerLogger : IFindReplaceConnect
        {
            private string? text;

            public bool CanMatchCase => true;

            public bool CanMatchWholeWord => true;

            public bool CanUseRegularExpressions => true;

            public bool ShowErrorBorder
            {
                get => string.IsNullOrEmpty(text);
            }

            public void FindNext()
            {
                App.Log("FindReplaceControl.FindNext");
            }

            public void FindPrevious()
            {
                App.Log("FindReplaceControl.FindPrevious");
            }

            public void Replace()
            {
                App.Log("FindReplaceControl.Replace");
            }

            public void ReplaceAll()
            {
                App.Log("FindReplaceControl.ReplaceAll");
            }

            public void SetFindText(string text)
            {
                var s = "FindReplaceControl.FindText =";
                this.text = text;
                App.LogReplace($"{s} '{text}'", s);
            }

            public void SetMatchCase(bool value)
            {
                App.Log($"FindReplaceControl.MatchCase = {value}");
            }

            public void SetMatchWholeWord(bool value)
            {
                App.Log($"FindReplaceControl.MatchWholeWord = {value}");
            }

            public void SetReplaceText(string text)
            {
                var s = "FindReplaceControl.ReplaceText =";
                App.LogReplace($"{s} '{text}'", s);
            }

            public void SetUseRegularExpressions(bool value)
            {
                App.Log($"FindReplaceControl.UseRegularExpressions = {value}");
            }

            public void SetReplaceVisible(bool value)
            {
                App.Log($"FindReplaceControl.ReplaceVisible = {value}");
            }
        }
    }
}