using System;
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
    public partial class FindReplaceControl : ToolBarSet
    {
        /// <summary>
        /// Gets or sets default border color of the find text editor.
        /// This property contains default value for the light color theme.
        /// </summary>
        public static Color DefaultFindEditBorderColorLight = Color.Empty;

        /// <summary>
        /// Gets or sets default border color of the find text editor.
        /// This property contains default value for the light color theme.
        /// </summary>
        public static Color DefaultFindEditBorderColorDark = Color.Empty;

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

        private readonly ComboBox scopeEdit = new()
        {
            HasBorder = false,
            Margin = (2, 0, 2, 0),
            VerticalAlignment = VerticalAlignment.Center,
        };

        private readonly ComboBox findEdit = new()
        {
            HasBorder = false,
            VerticalAlignment = VerticalAlignment.Center,
        };

        private readonly ComboBox replaceEdit = new()
        {
            HasBorder = false,
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

        private readonly Border findEditBorder = new()
        {
            Margin = (2, 0, 2, 0),
            VerticalAlignment = VerticalAlignment.Center,
        };

        private readonly Border replaceEditBorder = new()
        {
            Margin = (2, 0, 2, 0),
            VerticalAlignment = VerticalAlignment.Center,
        };

        private IFindReplaceConnect? manager;
        private bool canFindInCurrentDocument = true;
        private bool canFindInAllOpenDocuments = true;
        private bool canFindInCurrentProject = true;
        private bool canFindInSelectionOnly = true;
        private bool showErrorBorder;

        /// <summary>
        /// Initializes a new instance of the <see cref="FindReplaceControl"/> class.
        /// </summary>
        public FindReplaceControl()
        {
            findEditBorder.BorderColor = FindEditBorderColor;
            replaceEditBorder.BorderColor = FindEditBorderColor;

            DoInsideLayout(Fn);

            void Fn()
            {
                scopeEdit.IsEditable = false;
                UpdateFindScope();

                replaceEdit.TextChanged += ReplaceEdit_TextChanged;
                findEdit.TextChanged += FindEdit_TextChanged;

                ToolBarCount = 3;

                OptionsToolBar.AddSpeedBtn();

                IdMatchCase = OptionsToolBar.AddStickyBtn(
                    CommonStrings.Default.FindOptionMatchCase,
                    OptionsToolBar.GetNormalSvgImages().ImgFindMatchCase,
                    OptionsToolBar.GetDisabledSvgImages().ImgFindMatchCase,
                    null,
                    OnClickMatchCase);
                OptionsToolBar.SetToolShortcut(
                    IdMatchCase,
                    KnownKeys.FindReplaceControlKeys.MatchCase);

                IdMatchWholeWord = OptionsToolBar.AddStickyBtn(
                    CommonStrings.Default.FindOptionMatchWholeWord,
                    OptionsToolBar.GetNormalSvgImages().ImgFindMatchFullWord,
                    OptionsToolBar.GetDisabledSvgImages().ImgFindMatchFullWord,
                    null,
                    OnClickMatchWholeWord);
                OptionsToolBar.SetToolShortcut(
                    IdMatchWholeWord,
                    KnownKeys.FindReplaceControlKeys.MatchWholeWord);

                IdUseRegularExpressions = OptionsToolBar.AddStickyBtn(
                    CommonStrings.Default.FindOptionUseRegularExpressions,
                    OptionsToolBar.GetNormalSvgImages().ImgRegularExpr,
                    OptionsToolBar.GetDisabledSvgImages().ImgRegularExpr,
                    null,
                    OnClickUseRegularExpressions);
                OptionsToolBar.SetToolShortcut(
                    IdUseRegularExpressions,
                    KnownKeys.FindReplaceControlKeys.UseRegularExpressions);

                IdScopeEdit = OptionsToolBar.AddControl(scopeEdit);

                IdToggleReplaceOptions = FindToolBar.AddSpeedBtn(
                    CommonStrings.Default.ToggleToSwitchBetweenFindReplace,
                    FindToolBar.GetNormalSvgImages().ImgAngleDown,
                    FindToolBar.GetDisabledSvgImages().ImgAngleDown);

                findEdit.SuggestedWidth = 150;
                findEdit.EmptyTextHint = CommonStrings.Default.ButtonFind + "...";
                replaceEdit.SuggestedWidth = findEdit.SuggestedWidth;
                findEdit.Parent = findEditBorder;
                IdFindEdit = FindToolBar.AddControl(findEditBorder);

                IdFindNext = FindToolBar.AddSpeedBtn(
                    CommonStrings.Default.ButtonFindNext,
                    FindToolBar.GetNormalSvgImages().ImgArrowDown,
                    FindToolBar.GetDisabledSvgImages().ImgArrowDown);
                FindToolBar.SetToolShortcut(IdFindNext, KnownKeys.FindReplaceControlKeys.FindNext);

                IdFindPrevious = FindToolBar.AddSpeedBtn(
                    CommonStrings.Default.ButtonFindPrevious,
                    FindToolBar.GetNormalSvgImages().ImgArrowUp,
                    FindToolBar.GetDisabledSvgImages().ImgArrowUp);
                FindToolBar.SetToolShortcut(
                    IdFindPrevious,
                    KnownKeys.FindReplaceControlKeys.FindPrevious);

                IdFindClose = FindToolBar.AddSpeedBtn(
                    CommonStrings.Default.ButtonClose,
                    FindToolBar.GetNormalSvgImages().ImgCancel,
                    FindToolBar.GetDisabledSvgImages().ImgCancel);
                FindToolBar.SetToolAlignRight(IdFindClose, true);

                ReplaceToolBar.AddSpeedBtn();

                replaceEdit.Parent = replaceEditBorder;
                IdReplaceEdit = ReplaceToolBar.AddControl(replaceEditBorder);
                replaceEdit.EmptyTextHint = CommonStrings.Default.ButtonReplace + "...";

                IdReplace = ReplaceToolBar.AddSpeedBtn(
                    CommonStrings.Default.ButtonReplace,
                    FindToolBar.GetNormalSvgImages().ImgReplace,
                    FindToolBar.GetDisabledSvgImages().ImgReplace);
                ReplaceToolBar.SetToolShortcut(IdReplace, KnownKeys.FindReplaceControlKeys.Replace);

                IdReplaceAll = ReplaceToolBar.AddSpeedBtn(
                    CommonStrings.Default.ButtonReplaceAll,
                    FindToolBar.GetNormalSvgImages().ImgReplaceAll,
                    FindToolBar.GetDisabledSvgImages().ImgReplaceAll);
                ReplaceToolBar.SetToolShortcut(
                    IdReplaceAll,
                    KnownKeys.FindReplaceControlKeys.ReplaceAll);

                ReplaceToolBar.Visible = false;

                FindToolBar.AddToolAction(IdFindNext, OnClickFindNext);
                FindToolBar.AddToolAction(IdFindPrevious, OnClickFindPrevious);
                FindToolBar.AddToolAction(IdFindClose, OnClickClose);
                FindToolBar.AddToolAction(IdToggleReplaceOptions, OnClickToggleReplace);

                ReplaceToolBar.AddToolAction(IdReplace, OnClickReplace);
                ReplaceToolBar.AddToolAction(IdReplaceAll, OnClickReplaceAll);

                ItemSize = 32;
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
        /// Gets border of the <see cref="FindEdit"/>.
        /// </summary>
        [Browsable(false)]
        public Border FindEditBorder => findEditBorder;

        /// <summary>
        /// Gets border of the <see cref="ReplaceEdit"/>.
        /// </summary>
        [Browsable(false)]
        public Border ReplaceEditBorder => replaceEditBorder;

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
        public bool ShowErrorBorder
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
                    findEditBorder.BorderColor = NotFoundBorderColor;
                }
                else
                {
                    findEditBorder.BorderColor = FindEditBorderColor;
                }
            }
        }

        /// <summary>
        /// Get or sets whether 'Current Document' find scope is available.
        /// </summary>
        public bool CanFindInCurrentDocument
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
        public bool CanFindInAllOpenDocuments
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
        public bool CanFindInCurrentProject
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
        public bool CanFindInSelectionOnly
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
        public IFindReplaceConnect? Manager
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
        /// Gets or sets 'Hidden Text' option.
        /// </summary>
        public bool OptionHiddenText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets 'Search Up' option.
        /// </summary>
        public bool OptionSearchUp
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets 'Match Case' option.
        /// </summary>
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
                Manager?.SetMatchCase(value);
                OptionMatchCaseChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets whether 'Match Case' option is enabled.
        /// </summary>
        public bool OptionMatchCaseEnabled
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
                Manager?.SetMatchWholeWord(value);
                OptionMatchWholeWordChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets whether 'Match Whole Word' option is enabled.
        /// </summary>
        public bool OptionMatchWholeWordEnabled
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
                Manager?.SetUseRegularExpressions(value);
                OptionUseRegularExpressionsChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets whether 'Use Regular Expressions' option is enabled.
        /// </summary>
        public bool OptionUseRegularExpressionsEnabled
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
        public bool OptionUseRegularExpressionsVisible
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
        public bool CloseButtonVisible
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
        public ComboBox FindEdit => findEdit;

        /// <summary>
        /// Gets <see cref="ComboBox"/> which allows to specify text to find.
        /// </summary>
        [Browsable(false)]
        public ComboBox ScopeEdit => scopeEdit;

        /// <summary>
        /// Gets <see cref="ComboBox"/> which allows to specify text to replace.
        /// </summary>
        [Browsable(false)]
        public ComboBox ReplaceEdit => replaceEdit;

        /// <summary>
        /// Gets <see cref="GenericToolBar"/> with find buttons.
        /// </summary>
        [Browsable(false)]
        public GenericToolBar FindToolBar => GetToolBar(0);

        /// <summary>
        /// Gets <see cref="GenericToolBar"/> with replace buttons.
        /// </summary>
        [Browsable(false)]
        public GenericToolBar ReplaceToolBar => GetToolBar(1);

        /// <summary>
        /// Gets <see cref="GenericToolBar"/> with option buttons.
        /// </summary>
        [Browsable(false)]
        public GenericToolBar OptionsToolBar => GetToolBar(2);

        /// <summary>
        /// Gets or sets width of <see cref="FindEdit"/> and <see cref="ReplaceEdit"/>
        /// controls.
        /// </summary>
        [Browsable(false)]
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

        /// <summary>
        /// Gets or sets whether keys specified in <see cref="KnownKeys.FindReplaceControlKeys"/>
        /// are automatically handled.
        /// </summary>
        public bool WantKeys { get; set; } = true;

        /// <summary>
        /// Gets or sets whether <see cref="ReplaceToolBar"/> is visible.
        /// </summary>
        public bool ReplaceVisible
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
                    FindToolBar.GetSvgImages(false).GetImgAngleUpDown(value));
                FindToolBar.SetToolImage(
                    IdToggleReplaceOptions,
                    FindToolBar.GetSvgImages(true).GetImgAngleUpDown(value));
                Manager?.SetReplaceVisible(value);
            }
        }

        private bool CanFindNext =>
            FindToolBar.Visible && FindToolBar.GetToolEnabledAndVisible(IdFindNext);

        private bool CanFindPrevious =>
            FindToolBar.Visible && FindToolBar.GetToolEnabledAndVisible(IdFindPrevious);

        private bool CanReplace =>
            ReplaceVisible && ReplaceToolBar.GetToolEnabledAndVisible(IdReplace);

        private bool CanReplaceAll =>
            ReplaceVisible && ReplaceToolBar.GetToolEnabledAndVisible(IdReplaceAll);

        private bool CanMatchCase =>
            OptionsToolBar.Visible && OptionsToolBar.GetToolEnabledAndVisible(IdMatchCase);

        private bool CanMatchWholeWord =>
            OptionsToolBar.Visible && OptionsToolBar.GetToolEnabledAndVisible(IdMatchWholeWord);

        private bool CanUseRegularExpressions =>
            OptionsToolBar.Visible && OptionsToolBar.GetToolEnabledAndVisible(IdUseRegularExpressions);

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
                Application.DebugLogIf("FindReplace window disposed", false);
            }
        }

        /// <summary>
        /// Creates <see cref="IFindReplaceConnect"/> instance which logs all method calls.
        /// </summary>
        /// <returns></returns>
        public IFindReplaceConnect CreateLogger()
        {
            return new FindReplaceManagerLogger();
        }

        /// <summary>
        /// Handles keys specified in <see cref="KnownKeys.FindReplaceControlKeys"/>.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        /// <remarks>
        /// You can use this method in the <see cref="Control.KeyDown"/> event handlers.
        /// <see cref="WantKeys"/> specifies whether <see cref="HandleKeys"/>
        /// is called automatically.
        /// </remarks>
        public virtual bool HandleKeys(KeyEventArgs e)
        {
            bool Run(KeyInfo[] keys, Action? action)
            {
                return KeyInfo.Run(keys, e, action);
            }

            if (CanFindNext && Run(KnownKeys.FindReplaceControlKeys.FindNext, OnClickFindNext))
                return true;
            if (CanFindPrevious && Run(KnownKeys.FindReplaceControlKeys.FindPrevious, OnClickFindPrevious))
                return true;
            if (CanReplace && Run(KnownKeys.FindReplaceControlKeys.Replace, OnClickReplace))
                return true;
            if (CanReplaceAll && Run(KnownKeys.FindReplaceControlKeys.ReplaceAll, OnClickReplaceAll))
                return true;
            if (CanMatchCase && Run(KnownKeys.FindReplaceControlKeys.MatchCase, OnClickMatchCase))
                return true;
            if (CanMatchWholeWord && Run(KnownKeys.FindReplaceControlKeys.MatchWholeWord, OnClickMatchWholeWord))
                return true;
            if (CanUseRegularExpressions && Run(KnownKeys.FindReplaceControlKeys.UseRegularExpressions, OnClickUseRegularExpressions))
                return true;
            return false;
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

        private void OnClickUseRegularExpressions() => OnClickUseRegularExpressions(this, EventArgs.Empty);

        private void OnClickMatchWholeWord() => OnClickMatchWholeWord(this, EventArgs.Empty);

        private void OnClickMatchCase() => OnClickMatchCase(this, EventArgs.Empty);

        private void OnClickFindNext() => OnClickFindNext(this, EventArgs.Empty);

        private void OnClickFindPrevious() => OnClickFindPrevious(this, EventArgs.Empty);

        private void OnClickReplace() => OnClickReplace(this, EventArgs.Empty);

        private void OnClickReplaceAll() => OnClickReplaceAll(this, EventArgs.Empty);

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
            Manager?.FindNext();
        }

        private void OnClickToggleReplace(object? sender, EventArgs e)
        {
            ReplaceVisible = !ReplaceVisible;
        }

        private void OnClickFindPrevious(object? sender, EventArgs e)
        {
            ClickFindPrevious?.Invoke(this, e);
            Manager?.FindPrevious();
        }

        private void OnClickReplace(object? sender, EventArgs e)
        {
            ClickReplace?.Invoke(this, e);
            Manager?.Replace();
        }

        private void OnClickReplaceAll(object? sender, EventArgs e)
        {
            ClickReplaceAll?.Invoke(this, e);
            Manager?.ReplaceAll();
        }

        private void OnClickClose(object? sender, EventArgs e)
        {
            ClickClose?.Invoke(this, e);
        }

        private void FindEdit_TextChanged(object? sender, EventArgs e)
        {
            Manager?.SetFindText(findEdit.Text);
        }

        private void ReplaceEdit_TextChanged(object? sender, EventArgs e)
        {
            Manager?.SetReplaceText(replaceEdit.Text);
        }

        private void UpdateFindScope()
        {
            void AddOrRemove(object item, bool add)
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
                    scopeEdit.SelectedItem = item;
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
            public bool CanMatchCase => true;

            public bool CanMatchWholeWord => true;

            public bool CanUseRegularExpressions => true;

            public void FindNext()
            {
                Application.Log("FindReplaceControl.FindNext");
            }

            public void FindPrevious()
            {
                Application.Log("FindReplaceControl.FindPrevious");
            }

            public void Replace()
            {
                Application.Log("FindReplaceControl.Replace");
            }

            public void ReplaceAll()
            {
                Application.Log("FindReplaceControl.ReplaceAll");
            }

            public void SetFindText(string text)
            {
                Application.Log($"FindReplaceControl.FindText = '{text}'");
            }

            public void SetMatchCase(bool value)
            {
                Application.Log($"FindReplaceControl.MatchCase = {value}");
            }

            public void SetMatchWholeWord(bool value)
            {
                Application.Log($"FindReplaceControl.MatchWholeWord = {value}");
            }

            public void SetReplaceText(string text)
            {
                Application.Log($"FindReplaceControl.ReplaceText = '{text}'");
            }

            public void SetUseRegularExpressions(bool value)
            {
                Application.Log($"FindReplaceControl.UseRegularExpressions = {value}");
            }

            public void SetReplaceVisible(bool value)
            {
                Application.Log($"FindReplaceControl.ReplaceVisible = {value}");
            }
        }
    }
}