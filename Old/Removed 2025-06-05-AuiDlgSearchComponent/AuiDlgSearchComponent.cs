using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Alternet.Common;
using Alternet.Editor.Dialogs.VsControls.Painting;
using Alternet.Editor.TextSource;
using Alternet.UI;
using Alternet.UI.Localization;

namespace Alternet.Editor.Dialogs
{
    internal class AuiDlgSearchComponent : FrameworkElement, FindReplaceControl.IFindReplaceConnect
    {
        private readonly IFindReplaceControlHandler findReplace;
        private bool firstSearch = true;
        private ISearch search;
        private SearchOptions options;
        private IDisposable painter;
        private Form owner;
        private string lastText = string.Empty;
        private SearchOptions saveOptions;
        private string saveText = string.Empty;
        private int updateCount;

        public AuiDlgSearchComponent(IFindReplaceControlHandler findReplace)
        {
            this.findReplace = findReplace;

            FindReplaceControl.DefaultNotFoundBorderLight
                = EditConsts.DefaultNotFoundBorderLightColor;
            FindReplaceControl.DefaultNotFoundBorderDark
                = EditConsts.DefaultNotFoundBorderDarkColor;

            var sDlgSearchCaption = RemoveExtra(StringConsts.DlgSearchCaption);
            var sDlgReplaceCaption = RemoveExtra(StringConsts.DlgReplaceCaption);
            var sDlgSearchReplaceCaption = RemoveExtra(StringConsts.DlgSearchReplaceCaption);

            var sFindWhatCaption = RemoveExtra(StringConsts.FindWhatCaption);
            var sReplaceWithCaption = RemoveExtra(StringConsts.ReplaceWithCaption);
            var sSearchHiddenTextCaption = RemoveExtra(StringConsts.SearchHiddenTextCaption);
            var sPromptOnReplaceCaption = RemoveExtra(StringConsts.PromptOnReplaceCaption);
            var sShowNotFoundWarningCaption
                = RemoveExtra(StringConsts.ShowNotFoundWarningCaption);

            var sSearchUpCaption = RemoveExtra(StringConsts.SearchUpCaption);
            var sSearchCaption = RemoveExtra(StringConsts.SearchCaption);

            var sFromCursorCaption = RemoveExtra(StringConsts.FromCursorCaption);
            var sEntireScopeCaption = RemoveExtra(StringConsts.EntireScopeCaption);

            var sMatchCaseCaption = RemoveExtra(StringConsts.MatchCaseCaption);
            var sMatchWholeWordCaption = RemoveExtra(StringConsts.MatchWholeWordCaption);
            var sUseRegularExpressionsCaption
                = RemoveExtra(StringConsts.UseRegularExpressionsCaption);

            var sCurrentDocumentScope = StringConsts.CurrentDocumentCaption;
            var sAllOpenDocumentsScope = StringConsts.AllOpenDocumentsScope;
            var sCurrentProjectScope = StringConsts.CurrentProjectScope;
            var sSelectionOnlyScope = RemoveExtra(StringConsts.SelectionOnlyCaption);

            var sFindNextCaption = RemoveExtra(StringConsts.FindNextCaption);
            var sFindPreviousCaption = RemoveExtra(StringConsts.FindPreviousCaption);
            var sFindAllCaption = RemoveExtra(StringConsts.FindAllCaption);
            var sReplaceCaption = RemoveExtra(StringConsts.ReplaceCaption);
            var sReplaceAllCaption = RemoveExtra(StringConsts.ReplaceAllCaption);
            var sCloseCaption = RemoveExtra(StringConsts.CloseCaption);
            var sToggleReplaceCaption = RemoveExtra(StringConsts.ToggleReplaceCaption);

            static string RemoveExtra(string s)
            {
                var result = s.Replace("&", string.Empty).Trim(':');
                return result;
            }

            CommonStrings.Default.FindScopeCurrentDocument = sCurrentDocumentScope;
            CommonStrings.Default.FindScopeAllOpenDocuments = sAllOpenDocumentsScope;
            CommonStrings.Default.FindScopeCurrentProject = sCurrentProjectScope;
            CommonStrings.Default.FindScopeSelectionOnly = sSelectionOnlyScope;

            CommonStrings.Default.WindowTitleSearchAndReplace = sDlgSearchReplaceCaption;

            CommonStrings.Default.FindOptionMatchCase = sMatchCaseCaption;
            CommonStrings.Default.FindOptionMatchWholeWord = sMatchWholeWordCaption;
            CommonStrings.Default.FindOptionUseRegularExpressions = sUseRegularExpressionsCaption;

            CommonStrings.Default.ButtonFindNext = sFindNextCaption;
            CommonStrings.Default.ButtonFindPrevious = sFindPreviousCaption;
            CommonStrings.Default.ButtonClose = sCloseCaption;
            CommonStrings.Default.ButtonReplace = sReplaceCaption;
            CommonStrings.Default.ButtonReplaceAll = sReplaceAllCaption;

            CommonStrings.Default.ToggleToSwitchBetweenFindReplace = sToggleReplaceCaption;

            findReplace.FindEditEmptyTextHint = sFindWhatCaption + "...";
            findReplace.ReplaceEditEmptyTextHint = sReplaceWithCaption + "...";

            App.DebugLogIf("Search Created", false);

            Disposed += AlternetUIDlgSearch_Disposed;

            Title = CommonStrings.Default.WindowTitleSearchAndReplace;

            findReplace.Manager = this;
            findReplace.CloseButtonVisible = true;

            findReplace.ClickClose += (_, _) =>
            {
                if (Search != null && Search.InIncrementalSearch)
                    Search.FinishIncrementalSearch();
                /*
                PopupResult = ModalResult.Canceled;
                Close();
                */
            };

            /*
            Closed += (_, _) =>
            {
                App.DebugLogIf($"Search Popup Closed: {PopupResult}", false);
            };
            */

            UpdateFormSize();
            findReplace.IsScopeEditEnabled = true;

            /*
            HideOnEnter = false;
            HideOnEscape = true;
            */
        }

        public virtual string Title { get; set; }

        /// <summary>
        /// Gets or sets <c>ISearch</c> interface owning the dialog.
        /// </summary>
        public virtual ISearch Search
        {
            get
            {
                return SearchShared ? SearchManager.SharedSearch.Search : search;
            }

            set
            {
                if (search == value)
                    return;

                lastText = string.Empty;
                Search?.UnhighlightAll();
                UpdateSearch(value, true);
                HighlightAll();

                App.DebugLogIf("Sets Search", false);
                search = value;
            }
        }

        /// <summary>
        /// When implemented by a class, gets or sets painter object associated with this dialog
        /// </summary>
        public virtual IDisposable Painter
        {
            get
            {
                return painter;
            }

            set
            {
                painter = value;

                App.DebugLogIf("Sets Painter", false);
            }
        }

        /// <summary>
        /// Gets or sets options that define search and
        /// replace behavior.
        /// </summary>
        public virtual SearchOptions Options
        {
            get
            {
                bool chbSearchUpChecked = findReplace.OptionSearchUp;
                bool chbMatchCaseChecked = findReplace.OptionMatchCase;
                bool chbUseRegularExpressionsChecked = findReplace.OptionUseRegularExpressions;
                bool chbMatchWholeWordChecked = findReplace.OptionMatchWholeWord;
                bool chbSearchHiddenTextChecked = findReplace.OptionHiddenText;
                bool chbPromptOnReplaceChecked = findReplace.OptionPromptOnReplace;
                bool findTextAtCursor = findReplace.OptionFindTextAtCursor;

                SearchOptions result = (chbMatchCaseChecked ? SearchOptions.CaseSensitive : 0) |
                    (chbMatchWholeWordChecked ? SearchOptions.WholeWordsOnly : 0) |
                    (chbSearchHiddenTextChecked ? SearchOptions.SearchHiddenText : 0) |
                    (chbSearchUpChecked ? SearchOptions.BackwardSearch : 0) |
                    (chbUseRegularExpressionsChecked ? SearchOptions.RegularExpressions : 0) |
                    (findTextAtCursor ? SearchOptions.FindTextAtCursor : 0) |
                    (chbPromptOnReplaceChecked ? SearchOptions.PromptOnReplace : 0) |
                    (((options & SearchOptions.FindSelectedText) != 0)
                        ? SearchOptions.FindSelectedText : 0) |
                    (((options & SearchOptions.CycledSearch) != 0)
                        ? SearchOptions.CycledSearch : 0) |
                    (((options & SearchOptions.SilentSearch) != 0)
                        ? SearchOptions.SilentSearch : 0);

                if ((search != null) && search.SearchGlobal)
                {
                    result |= (findReplace.IsScopeAllOpenDocuments
                        ? SearchOptions.AllDocuments : 0) |
                    (findReplace.IsScopeCurrentProject ? SearchOptions.CurrentProject : 0) |
                    (findReplace.IsScopeSelectionOnly ? SearchOptions.SelectionOnly : 0);
                }
                else
                {
                    result |= findReplace.IsScopeSelectionOnly ? SearchOptions.SelectionOnly : 0;
                }

                if ((result & SearchOptions.SelectionOnly) != 0)
                {
                    if ((options & SearchOptions.AllDocuments) != 0)
                        result |= SearchOptions.AllDocuments;

                    if ((options & SearchOptions.CurrentProject) != 0)
                        result |= SearchOptions.CurrentProject;

                    if ((options & SearchOptions.EntireScope) != 0)
                        result |= SearchOptions.EntireScope;
                }

                if (Search != null)
                {
                    if ((Search.SearchOptions & SearchOptions.DisplayIncrementalSearchDiaog) != 0)
                        result |= SearchOptions.DisplayIncrementalSearchDiaog;
                }
                else
                {
                    result |= EditConsts.DefaultIncrementalSearchOptions;
                }

                return result;
            }

            set
            {
                options = value;
                UpdateSearch();
                findReplace.OptionMatchCase = (value & SearchOptions.CaseSensitive) != 0;
                findReplace.OptionMatchWholeWord = (value & SearchOptions.WholeWordsOnly) != 0;
                findReplace.OptionHiddenText = (value & SearchOptions.SearchHiddenText) != 0;
                findReplace.OptionSearchUp = (value & SearchOptions.BackwardSearch) != 0;
                findReplace.OptionUseRegularExpressions
                    = (value & SearchOptions.RegularExpressions) != 0;

                if ((search != null) && search.SearchGlobal)
                {
                    findReplace.CanFindInAllOpenDocuments = true;
                    findReplace.CanFindInCurrentProject = true;

                    if ((value & SearchOptions.AllDocuments) != 0)
                    {
                        findReplace.IsScopeAllOpenDocuments = true;
                    }
                    else
                    {
                        if ((value & SearchOptions.CurrentProject) != 0)
                            findReplace.IsScopeCurrentProject = true;
                        else
                            findReplace.IsScopeCurrentDocument = true;
                    }

                    if ((value & SearchOptions.SelectionOnly) != 0)
                        findReplace.IsScopeSelectionOnly = true;
                }
                else
                {
                    findReplace.CanFindInAllOpenDocuments = false;
                    findReplace.CanFindInCurrentProject = false;

                    if ((value & SearchOptions.SelectionOnly) != 0)
                    {
                        findReplace.IsScopeSelectionOnly = true;
                    }
                    else
                    {
                        findReplace.IsScopeCurrentDocument = true;
                    }
                }

                findReplace.OptionFindTextAtCursor = (value & SearchOptions.FindTextAtCursor) != 0;
                findReplace.OptionPromptOnReplace = (value & SearchOptions.PromptOnReplace) != 0;
            }
        }

        /// <summary>
        /// Gets or sets a boolean value that indicates
        /// whether the search or replace dialog should be executed.
        /// </summary>
        public virtual bool IsReplace
        {
            get => findReplace.ReplaceVisible;
            set
            {
                if (IsReplace == value)
                    return;
                findReplace.ReplaceVisible = value;
                UpdateFormSize();
            }
        }

        /// <summary>
        /// Indicates whether the options group box should be visible.
        /// </summary>
        public virtual bool OptionsVisible
        {
            get => findReplace.OptionsVisible;
            set
            {
                if (OptionsVisible == value)
                    return;
                findReplace.OptionsVisible = true;
                UpdateFormSize();
            }
        }

        /// <summary>
        /// Indicates whether all unnumbered bookmarks
        /// should be removed from the bookmark collection.
        /// </summary>
        public virtual bool ClearBookmarks
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a boolean value that indicates
        /// whether search can be executed through selected text.
        /// </summary>
        public virtual bool SelectionEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the search history for the search dialog.
        /// </summary>
        public virtual IList SearchList
        {
            get => findReplace.SearchList;
        }

        /// <summary>
        /// Represents the replace history for the replace dialog.
        /// </summary>
        public virtual IList ReplaceList
        {
            get => findReplace.ReplaceList;
        }

        public string TextToFind
        {
            get => findReplace.TextToFind;
            set => findReplace.TextToFind = value;
        }

        public string TextToReplace
        {
            get => findReplace.TextToReplace;
            set => findReplace.TextToReplace = value;
        }

        bool FindReplaceControl.IFindReplaceConnect.CanMatchCase
        {
            get => true;
        }

        bool FindReplaceControl.IFindReplaceConnect.CanMatchWholeWord
        {
            get => true;
        }

        bool FindReplaceControl.IFindReplaceConnect.CanUseRegularExpressions
        {
            get => true;
        }

        public Control Container
        {
            get
            {
                return search as Control;
            }

            set
            {
            }
        }

        internal SyntaxEdit Editor => search as SyntaxEdit;

        protected virtual bool FirstSearch
        {
            get
            {
                return SearchShared ? SearchManager.SharedSearch.FirstSearch : search.FirstSearch;
            }
        }

        protected virtual bool SearchShared
        {
            get
            {
                return SearchManager.SharedSearch.Shared;
            }

            set
            {
                if (SearchManager.SharedSearch.Shared != value)
                {
                    SearchManager.SharedSearch.Shared = value;
                    SearchManager.SharedSearch.FirstSearch = true;
                    SearchManager.SharedSearch.Init(ref search, Options);
                }
            }
        }

        public static VsControlsPainter GetPainter(Control control)
        {
            return VsControlsTheme.Painter;
        }

        /// <summary>
        /// Toggles searching for whole words on/off.
        /// </summary>
        public virtual void ToggleWholeWord()
        {
            findReplace.ToggleWholeWord();
        }

        /// <summary>
        /// Toggles using regular expressions on/off.
        /// </summary>
        public virtual void ToggleRegularExpressions()
        {
            findReplace.ToggleRegularExpressions();
        }

        /// <summary>
        /// Toggles case sensitive searching on/off.
        /// </summary>
        public virtual void ToggleMatchCase()
        {
            findReplace.ToggleMatchCase();
        }

        /// <summary>
        /// Toggles prompting before replacing on/off.
        /// </summary>
        public virtual void TogglePromptOnReplace()
        {
            findReplace.OptionPromptOnReplace = !findReplace.OptionPromptOnReplace;
        }

        /// <summary>
        /// Toggles searching direction towards/backwards.
        /// </summary>
        public virtual void ToggleSearchUp()
        {
            findReplace.ToggleSearchUp();
        }

        /// <summary>
        /// Toggles searching through hidden text on/off.
        /// </summary>
        public virtual void ToggleHiddenText()
        {
            findReplace.ToggleHiddenText();
        }

        /// <summary>
        /// Enables/disables regular expressions.
        /// </summary>
        /// <param name="enable">Specifies whether regular expressions checkbox
        /// should be enabled.</param>
        public virtual void EnableRegularExpressions(bool enable)
        {
            findReplace.OptionUseRegularExpressionsEnabled = enable;
        }

        /// <summary>
        /// Shows/hides regular expressions.
        /// </summary>
        /// <param name="show">Specifies whether regular expressions checkbox should
        /// be visible.</param>
        public virtual void ShowRegularExpressions(bool show)
        {
            findReplace.OptionUseRegularExpressionsVisible = show;
        }

        /// <summary>
        /// Resets <c>IDlgSearch</c> to the start of search.
        /// </summary>
        public void Init()
        {
            App.DebugLogIf($"Init()", false);
            firstSearch = true;
            saveOptions = SearchOptions.None;
            saveText = string.Empty;
            UpdateSearch();
        }

        public void Init(ISearch search, SearchOptions options)
        {
            App.DebugLogIf($"Init(ISearch search, SearchOptions options)", false);
            updateCount++;
            try
            {
                Search = search;
                Init();
                if (options != SearchOptions.None)
                    Options = options;
            }
            finally
            {
                updateCount--;
            }
        }

        /// <summary>
        /// Updates search engine.
        /// </summary>
        /// <param name="newSearch">New <c>ISearch</c> object performs search.</param>
        /// <param name="update">True if given search engine should be set as current
        /// search.</param>
        public void UpdateSearch(ISearch newSearch, bool update)
        {
            App.DebugLogIf($"UpdateSearch(ISearch newSearch, bool update)", false);

            if (search == newSearch)
                return;

            search = newSearch;

            if (SearchShared)
                SearchManager.SharedSearch.UpdateSearch(newSearch, update);

            if (search != null)
            {
            }
        }

        /// <summary>
        /// Updates text to find.
        /// </summary>
        /// <param name="text">New text to search.</param>
        public void UpdateFindText(string text)
        {
            App.DebugLogIf($"UpdateFindText({text})", false);
            TextToFind = text;
        }

        public void HighlightAll()
        {
            DoHighlightAll();
        }

        /// <summary>
        /// Highlights all occurrences of the search text.
        /// </summary>
        public int DoHighlightAll()
        {
            int result = 0;

            App.DebugLogIf($"HighlightAll()", false);

            if (Search != null && Search.HighlightSearchResults && !Search.InIncrementalSearch)
            {
                result = HighlightAll(TextToFind, Options, GetExpression());
                findReplace.ShowErrorBorder = result == 0;
            }
            else
            {
                findReplace.ShowErrorBorder = TextToFind == string.Empty;
            }

            return result;
        }

        /// <summary>
        /// Shows the dialog as a modal dialog box with
        /// the specified owner.
        /// </summary>
        /// <param name="owner">Any object that implements
        /// IWin32Window that represents the top-level window that will own the
        /// modal dialog box.</param>
        /// <returns>One of the DialogResult values.</returns>
        public DialogResult ShowDialog(IWin32Window owner)
        {
            (this as IDlgSearch).Show(owner as Control);
            return DialogResult.Cancel;
        }

        /// <summary>
        /// Shows the dialog as a modal dialog box.
        /// </summary>
        /// <returns>One of the DialogResult values.</returns>
        public DialogResult ShowDialog()
        {
            (this as IDlgSearch).Show();
            return DialogResult.Cancel;
        }

        void FindReplaceControl.IFindReplaceConnect.SetMatchCase(bool value)
        {
            App.DebugLogIf($"IFindReplaceConnect.SetMatchCase({value})", false);
            HighlightAll();
        }

        public void Show()
        {
            App.DebugLogIf($"IDlgSearch.Show()", false);

            if (search is not SyntaxEdit editor)
                return;

            /*
            Parent = editor;
            AlignInParent(HorizontalAlignment.Right, VerticalAlignment.Top);
            SetSizeToContent();
            BackgroundColor = SystemColors.Control;
            ForegroundColor = SystemColors.ControlText;
            base.Show();
            */

            App.DoEvents();
            findReplace.FindEditor?.SetFocusIfPossible();
            findReplace.SelectAllTextInFindEditor();
        }

        void FindReplaceControl.IFindReplaceConnect.SetMatchWholeWord(bool value)
        {
            HighlightAll();
            App.DebugLogIf($"IFindReplaceConnect.SetMatchWholeWord({value})", false);
        }

        void FindReplaceControl.IFindReplaceConnect.SetUseRegularExpressions(bool value)
        {
            HighlightAll();
            App.DebugLogIf($"IFindReplaceConnect.SetUseRegularExpressions({value})", false);
        }

        void FindReplaceControl.IFindReplaceConnect.SetReplaceVisible(bool value)
        {
            App.DebugLogIf(
                $"IFindReplaceConnect.SetReplaceVisible({value})", false);
            UpdateFormSize();
        }

        void FindReplaceControl.IFindReplaceConnect.Replace()
        {
            HandleReplace();
        }

        void FindReplaceControl.IFindReplaceConnect.ReplaceAll()
        {
            App.DebugLogIf($"IFindReplaceConnect.ReplaceAll()", false);

            AddToHistory();
            UpdateSearchMode();

            if (Search != null)
            {
                SearchOptions opt = Options;
                int count;

                var result = ReplaceAll(
                    TextToFind,
                    findReplace.TextToReplace,
                    Options,
                    GetExpression(),
                    out count);

                if (!result)
                    ShowNotFound();
                else
                {
                    var msg = string.Format(StringConsts.OccurrencesReplaced, count);
                    MessageBoxHandler.Show(msg, Title);
                }
            }
        }

        void FindReplaceControl.IFindReplaceConnect.FindNext()
        {
            App.DebugLogIf($"IFindReplaceConnect.FindNext()", false);
            FindNextOrPrevious(true);
        }

        void FindReplaceControl.IFindReplaceConnect.FindPrevious()
        {
            App.DebugLogIf($"IFindReplaceConnect.FindPrevious()", false);
            FindNextOrPrevious(false);
        }

        void FindReplaceControl.IFindReplaceConnect.SetFindText(string text)
        {
            App.DebugLogIf($"IFindReplaceConnect.SetFindText('{text})'", false);

            if (updateCount > 0)
                return;

            var s = text;

            if ((Search != null) && Search.InIncrementalSearch)
            {
                findReplace.ShowErrorBorder = s == string.Empty;

                string str = string.IsNullOrEmpty(s)
                    ? string.Empty : s[s.Length - 1].ToString();
                Search.IncrementalSearch(str, s.Length < lastText.Length);
                lastText = s;
            }
            else
            {
                DoHighlightAll();
            }
        }

        void FindReplaceControl.IFindReplaceConnect.SetReplaceText(string text)
        {
            App.DebugLogIf($"IFindReplaceConnect.SetReplaceText('{text}')", false);
        }

        internal bool IsEscapeChar(string s, int index)
        {
            bool isEscape = false;
            while ((index > 0) && (s[index - 1] == '\\'))
            {
                if (s[index - 1] == '\\')
                    isEscape = !isEscape;
                index--;
            }

            return isEscape;
        }

        internal void RemoveChar(ref string s, char ch)
        {
            int i = s.Length - 1;
            while (i > 0)
            {
                if ((s[i] == ch) && IsEscapeChar(s, i))
                {
                    s = s.Remove(i - 1, 2);
                }

                i--;
            }
        }

        internal void ReplaceChar(ref string s, char ch, string replace)
        {
            int i = s.Length - 1;
            while (i > 0)
            {
                if ((s[i] == ch) && IsEscapeChar(s, i))
                {
                    s = s.Remove(i - 1, 2);

                    if (replace != string.Empty)
                        s = s.Insert(i - 1, replace);
                }

                i--;
            }
        }

        internal void FixCarriageReturn(ref string s)
        {
            ReplaceChar(ref s, 'r', string.Empty);
            ReplaceChar(ref s, 'n', @"\r\n");
        }

        internal void AddToHistory(IList<object> list, string s)
        {
            if (s != string.Empty)
            {
                int idx = list.IndexOf(s);
                while (idx >= 0)
                {
                    list.RemoveAt(idx);
                    idx = list.IndexOf(s);
                }

                list.Insert(0, s);
            }
        }

        protected void ShowNotFound()
        {
            ShowNotFound(Title);
            findReplace.FindEditor?.SetFocusIfPossible();
        }

        protected virtual bool ReplaceAll(
            string text,
            string replaceWith,
            SearchOptions options,
            Regex expression,
            out int count)
        {
            return SearchShared
                ? SearchManager.SharedSearch.ReplaceAll(
                    Search,
                    text,
                    replaceWith,
                    options,
                    expression,
                    out count,
                    out _)
                : Search.ReplaceAll(text, replaceWith, options, expression, out count, out _);
        }

        protected virtual bool NeedReplaceCurrent(out Match match)
        {
            return SearchShared
                ? SearchManager.SharedSearch.NeedReplaceCurrent(out match)
                : Search.NeedReplaceCurrent(out match);
        }

        protected virtual bool ReplaceCurrent(
            string replaceWith,
            SearchOptions options,
            Match match)
        {
            return SearchShared
                ? SearchManager.SharedSearch.ReplaceCurrent(replaceWith, options, match)
                : Search.ReplaceCurrent(replaceWith, options, match);
        }

        protected virtual bool FindPrevious()
        {
            return SearchShared
                ? SearchManager.SharedSearch.FindPrevious() : Search.FindPrevious();
        }

        protected virtual bool FindNext()
        {
            return SearchShared ? SearchManager.SharedSearch.FindNext() : Search.FindNext();
        }

        protected virtual bool Find(string text, SearchOptions options, Regex expression)
        {
            return SearchShared
                ? SearchManager.SharedSearch.Find(Search, text, options, expression)
                : Search.Find(text, options, expression);
        }

        protected virtual int MarkAll(
            string text,
            SearchOptions options,
            Regex expression,
            bool clearPrevious)
        {
            return SearchShared
                ? SearchManager.SharedSearch.MarkAll(
                    Search,
                    text,
                    options,
                    expression,
                    clearPrevious)
                : Search.MarkAll(text, options, expression, clearPrevious);
        }

        protected /*override*/ void OnBeforeChildKeyDown(object sender, KeyEventArgs e)
        {
            if (e.IsEnter)
            {
                if (findReplace.ReplaceVisible)
                    HandleReplace();
                else
                    FindNextOrPrevious(true);
                e.Suppressed();
                return;
            }

            if (e.Key == Key.Tab)
            {
                if (sender == findReplace.FindEditor)
                {
                    if (findReplace.ReplaceVisible)
                        findReplace.ReplaceEditor?.SetFocusIfPossible();
                }
                else
                    findReplace.FindEditor?.SetFocusIfPossible();
                e.Suppressed();
            }

            /*base.OnBeforeChildKeyDown(sender, e);*/
        }

        protected virtual void ShowNotFound(string caption)
        {
            if (SearchShared)
                SearchManager.SharedSearch.ShowNotFound(Search, caption);
            else
                Search.ShowNotFound(caption);
        }

        protected virtual int HighlightAll(string text, SearchOptions options, Regex expression)
        {
            return Search.HighlightAll(text, options, expression);
        }

        private void UpdateSearch()
        {
            var scope = findReplace.Scope;

            try
            {
                findReplace.CanFindInCurrentDocument = true;

                if ((search != null) && search.SearchGlobal)
                {
                    findReplace.CanFindInAllOpenDocuments = true;
                    findReplace.CanFindInCurrentProject = true;
                }
                else
                {
                    findReplace.CanFindInAllOpenDocuments = false;
                    findReplace.CanFindInCurrentProject = false;
                }

                findReplace.CanFindInSelectionOnly = SelectionEnabled;
            }
            finally
            {
                findReplace.Scope = scope;

                if (findReplace.Scope is null)
                {
                    if (SelectionEnabled)
                        findReplace.Scope = FindReplaceControl.SearchScope.SelectionOnly;
                    else
                        findReplace.Scope = FindReplaceControl.SearchScope.CurrentDocument;
                }
            }
        }

        private Regex GetExpression()
        {
            Regex result = null;

            if (findReplace.OptionUseRegularExpressions)
            {
                try
                {
                    string s = TextToFind;
                    FixCarriageReturn(ref s);
                    result = new Regex(s, GetRegexOptions(TextToFind));
                }
                catch (Exception e)
                {
                    result = null;
                    ErrorHandler.Error(e);
                }
            }

            return result;
        }

        private RegexOptions GetRegexOptions(string text)
        {
            return (findReplace.OptionSearchUp ? RegexOptions.RightToLeft : 0) |
                (!findReplace.OptionMatchCase ? RegexOptions.IgnoreCase : 0) |
                ((text != null) && ((text.IndexOf(@"\r") >= 0) || (text.IndexOf(@"\n") >= 0))
                ? RegexOptions.Multiline : 0);
        }

        private void TextFound()
        {
            firstSearch = false;
            saveOptions = Options;
            saveText = TextToFind;
        }

        private void UpdateSearchMode()
        {
            var isGlobal
                = findReplace.IsScopeAllOpenDocuments || findReplace.IsScopeCurrentProject;

            if (isGlobal)
                SearchShared = (search != null) && search.SearchGlobal;
            else
                SearchShared = false;
        }

        private void MarkAll_Click(object sender, System.EventArgs e)
        {
            AddToHistory();
            UpdateSearchMode();

            if (Search != null)
                MarkAll(TextToFind, Options, GetExpression(), ClearBookmarks);
        }

        private void BeginWord_Click(object sender, System.EventArgs e)
        {
            string s = ((ToolStripMenuItem)sender).Text.Trim();
            int idx = s.IndexOf(Consts.Space);

            if (idx > 0)
                s = s.Substring(0, idx);
            TextToFind = TextToFind + s;
        }

        private void AddToHistory()
        {
            updateCount++;
            try
            {
                var s = TextToFind;

                if (s != string.Empty)
                {
                    AddToHistory(findReplace.SearchList, s);
                    TextToFind = s;
                }

                s = TextToReplace;

                if (findReplace.ReplaceVisible && (s != string.Empty))
                {
                    AddToHistory(findReplace.ReplaceList, s);
                    TextToReplace = s;
                }
            }
            finally
            {
                updateCount--;
            }
        }

        private void FindNextOrPrevious(bool findNext)
        {
            findReplace.OptionSearchUp = !findNext;

            AddToHistory();
            UpdateSearchMode();

            if (Search != null)
            {
                SearchOptions opt = Options;
                bool res;

                if (!(FirstSearch || firstSearch) && (TextToFind == saveText)
                    && OptionsEqual(opt, saveOptions))
                {
                    if ((opt & SearchOptions.BackwardSearch) != 0)
                        res = FindPrevious();
                    else
                        res = FindNext();
                }
                else
                    res = Find(TextToFind, opt, GetExpression());

                if (res)
                    TextFound();
                else
                    ShowNotFound();
            }

            static bool OptionsEqual(SearchOptions opt1, SearchOptions opt2)
            {
                return opt1 == opt2;
            }
        }

        private void HandleReplace()
        {
            App.DebugLogIf($"IFindReplaceConnect.Replace()", false);

            if (!IsReplace)
                IsReplace = true;
            else
            {
                AddToHistory();
                UpdateSearchMode();

                if (Search != null)
                {
                    Match match = null;
                    SearchOptions opt = Options;
                    bool res;

                    if (!FirstSearch && (TextToFind == saveText)
                        && NeedReplaceCurrent(out match))
                    {
                        res = true;
                    }
                    else
                    {
                        res = Find(TextToFind, opt, GetExpression());
                    }

                    if (res)
                    {
                        if (ReplaceCurrent(TextToReplace, opt, match))
                        {
                            if ((Options & SearchOptions.BackwardSearch) != 0)
                                res = FindPrevious();
                            else
                                res = FindNext();
                        }
                        else
                            return;
                    }
                    else
                        res = Find(TextToFind, opt, GetExpression());

                    if (res)
                        TextFound();
                    else
                        ShowNotFound();
                }
            }
        }

        private void AlternetUIDlgSearch_Disposed(object sender, EventArgs e)
        {
            App.DebugLogIf("AlternetUIDlgSearch Disposed", false);
        }

        private void UpdateFormSize()
        {
            /*SetSizeToContent(WindowSizeToContentMode.WidthAndHeight);*/
        }
    }
}