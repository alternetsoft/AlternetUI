using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI.Localization
{
    /// <summary>
    /// Defines localizations for common strings.
    /// </summary>
    public class CommonStrings
    {
        /// <summary>
        /// Current localizations for common strings.
        /// </summary>
        public static CommonStrings Default { get; set; } = new();

        /// <summary>
        /// Gets or sets common string localization.
        /// </summary>
        public string ButtonOk { get; set; } = "Ok";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonFilter { get; set; } = "Filter";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonQuit { get; set; } = "Quit";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonContinue { get; set; } = "Continue";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonDetails { get; set; } = "Details";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonCancel { get; set; } = "Cancel";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonReset { get; set; } = "Reset";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonFind { get; set; } = "Find";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonFindNext { get; set; } = "Find Next";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonOptions { get; set; } = "Options";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonFindPrevious { get; set; } = "Find Previous";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonReplace { get; set; } = "Replace";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonReplaceAll { get; set; } = "Replace All";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonPrint { get; set; } = "Print";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonApply { get; set; } = "Apply";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonAdd { get; set; } = "Add";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonBack { get; set; } = "Back";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonNew { get; set; } = "New";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonOpen { get; set; } = "Open";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonSave { get; set; } = "Save";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonForward { get; set; } = "Forward";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonGo { get; set; } = "Go";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonZoomIn { get; set; } = "Zoom In";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonZoomOut { get; set; } = "Zoom Out";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonAddChild { get; set; } = "Add Child";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonRemove { get; set; } = "Remove";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonRemoveAll { get; set; } = "Remove All";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonClear { get; set; } = "Clear";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonClearAll { get; set; } = "Clear All";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonClose { get; set; } = "Close";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonCopy { get; set; } = "Copy";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonPassword { get; set; } = "Password";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonPaste { get; set; } = "Paste";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonCut { get; set; } = "Cut";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonUndo { get; set; } = "Undo";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonRedo { get; set; } = "Redo";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonBold { get; set; } = "Bold";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonSearch { get; set; } = "Search";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonItalic { get; set; } = "Italic";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonUnderline { get; set; } = "Underline";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonYes { get; set; } = "Yes";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonNo { get; set; } = "No";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonAbort { get; set; } = "Abort";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonRetry { get; set; } = "Retry";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonIgnore { get; set; } = "Ignore";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonMoreActions { get; set; } = "More Actions";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonHelp { get; set; } = "Help";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonAll { get; set; } = "All";

        // ========================

        /// <inheritdoc cref="ButtonOk"/>
        public string ToolBarPreviousTabToolTip { get; set; } = "Previous Tab";

        /// <inheritdoc cref="ButtonOk"/>
        public string ToolBarNextTabToolTip { get; set; } = "Next Tab";

        // ========================

        /// <inheritdoc cref="ButtonOk"/>
        public string WindowTitleExceptionDetails { get; set; } = "Exception Details";

        /// <inheritdoc cref="ButtonOk"/>
        public string WindowTitleSearchAndReplace { get; set; } = "Search and Replace";

        /// <inheritdoc cref="ButtonOk"/>
        public string WindowTitleSearch { get; set; } = "Search";

        /// <inheritdoc cref="ButtonOk"/>
        public string WindowTitleReplace { get; set; } = "Replace";

        /// <inheritdoc cref="ButtonOk"/>
        public string WindowTitleListEdit { get; set; } = "List Editor";

        /// <inheritdoc cref="ButtonOk"/>
        public string WindowTitleGoToLine { get; set; } = "Go To Line";

        /// <inheritdoc cref="ButtonOk"/>
        public string WindowTitleInput { get; set; } = "Input";

        /// <inheritdoc cref="ButtonOk"/>
        public string WindowTitleProperties { get; set; } = "Properties";

        /// <inheritdoc cref="ButtonOk"/>
        public string WindowTitleApplicationAlert { get; set; } = "Application Says";

        /// <inheritdoc cref="ButtonOk"/>
        public string WindowTitleSelectColor { get; set; } = "Select a Color";

        /// <inheritdoc cref="ButtonOk"/>
        public string WindowTitleSelectValue { get; set; } = "Select a Value";

        /// <inheritdoc cref="ButtonOk"/>
        public string WindowTitleSelectValues { get; set; } = "Select Values";

        /// <inheritdoc cref="ButtonOk"/>
        public string WindowTitleSelectItem { get; set; } = "Select an Item";

        /// <inheritdoc cref="ButtonOk"/>
        public string WindowTitleSelectItems { get; set; } = "Select Items";

        /// <inheritdoc cref="ButtonOk"/>
        public string WindowTitleSelectFontName { get; set; } = "Select a Font Name";

        /// <inheritdoc cref="ButtonOk"/>
        public string WindowTitleSelectFontSize { get; set; } = "Select a Font Size";

        /// <inheritdoc cref="ButtonOk"/>
        public string WindowTitleSelectDate { get; set; } = "Select a Date";

        // ========================

        /// <inheritdoc cref="ButtonOk"/>
        public string NotebookTabTitleBrowser { get; set; } = "Browser";

        /// <inheritdoc cref="ButtonOk"/>
        public string NotebookTabTitleEvents { get; set; } = "Events";

        /// <inheritdoc cref="ButtonOk"/>
        public string NotebookTabTitleOutput { get; set; } = "Output";

        /// <inheritdoc cref="ButtonOk"/>
        public string NotebookTabTitleActivity { get; set; } = "Activity";

        /// <inheritdoc cref="ButtonOk"/>
        public string NotebookTabTitleActions { get; set; } = "Actions";

        // ========================

        /// <inheritdoc cref="ButtonOk"/>
        public string ListEditDefaultItemTitle { get; set; } = "Item";

        // ========================

        /// <inheritdoc cref="ButtonOk"/>
        public string SearchFor { get; set; } = "Search for";

        /// <inheritdoc cref="ButtonOk"/>
        public string ReplaceWith { get; set; } = "Replace with";

        /// <inheritdoc cref="ButtonOk"/>
        public string FindOptionWrap { get; set; } = "Wrap around";

        /// <inheritdoc cref="ButtonOk"/>
        public string FindOptionHiddenText { get; set; } = "Search hidden text";

        /// <inheritdoc cref="ButtonOk"/>
        public string FindOptionPromptOnReplace { get; set; } = "Prompt on replace";

        /// <inheritdoc cref="ButtonOk"/>
        public string FindOptionMatchCase { get; set; } = "Match case";

        /// <inheritdoc cref="ButtonOk"/>
        public string FindOptionUseRegularExpressions { get; set; } = "Regular expressions";

        /// <inheritdoc cref="ButtonOk"/>
        public string FindOptionMatchWholeWord { get; set; } = "Match whole word";

        /// <inheritdoc cref="ButtonOk"/>
        public string FindOptionHighlight { get; set; } = "Highlight";

        /// <inheritdoc cref="ButtonOk"/>
        public string FindOptionSelectionOnly { get; set; } = "Selection Only";

        /// <inheritdoc cref="ButtonOk"/>
        public string FindOptionBackwards { get; set; } = "Backwards";

        /// <inheritdoc cref="ButtonOk"/>
        public string FindScopeCurrentDocument { get; set; } = "Current Document";

        /// <inheritdoc cref="ButtonOk"/>
        public string FindScopeAllOpenDocuments { get; set; } = "All Open Documents";

        /// <inheritdoc cref="ButtonOk"/>
        public string FindScopeCurrentProject { get; set; } = "Current Project";

        /// <inheritdoc cref="ButtonOk"/>
        public string FindScopeSelectionOnly { get; set; } = "Selection Only";

        /// <inheritdoc cref="ButtonOk"/>
        public string ToggleToSwitchBetweenFindReplace { get; set; } =
            "Toggle to switch between find and replace modes";

        // ========================

        /// <inheritdoc cref="ButtonOk"/>
        public string LineNumber { get; set; } = "Line Number";

        /// <inheritdoc cref="ButtonOk"/>
        public string LoadingPleaseWait { get; set; } = "Please wait...";

        /// <inheritdoc cref="ButtonOk"/>
        public string ToolbarSeeMore { get; set; } = "See more";

        /// <inheritdoc cref="ButtonOk"/>
        public string NoPreviewAvailable { get; set; } = "No preview available.";

        /// <inheritdoc cref="ButtonOk"/>
        public string SelectFileToPreview { get; set; } = "Select a file to preview.";

        // ========================

        /// <inheritdoc cref="ButtonOk"/>
        public string FileListBoxColumnName { get; set; } = "Name";

        /// <inheritdoc cref="ButtonOk"/>
        public string FileListBoxColumnDateModified { get; set; } = "Date modified";

        /// <inheritdoc cref="ButtonOk"/>
        public string FileListBoxColumnSize { get; set; } = "Size";

        // ========================

        /// <inheritdoc cref="ButtonOk"/>
        public string EnterValue { get; set; } = "Enter value";

        /// <inheritdoc cref="ButtonOk"/>
        public string DoubleClick { get; set; } = "Double click";
    }
}