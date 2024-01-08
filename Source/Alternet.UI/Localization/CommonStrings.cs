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
        public string ButtonCancel { get; set; } = "Cancel";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonFind { get; set; } = "Find";

        /// <inheritdoc cref="ButtonOk"/>
        public string ButtonReplace { get; set; } = "Replace";

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
        public string ButtonCopy { get; set; } = "Copy";

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
        public string ButtonHelp { get; set; } = "Help";

        // ========================

        /// <inheritdoc cref="ButtonOk"/>
        public string WindowTitleListEdit { get; set; } = "List Editor";

        /// <inheritdoc cref="ButtonOk"/>
        public string WindowTitleGoToLine { get; set; } = "Go To Line";

        /// <inheritdoc cref="ButtonOk"/>
        public string WindowTitleInput { get; set; } = "Input";

        /// <inheritdoc cref="ButtonOk"/>
        public string WindowTitleProperties { get; set; } = "Properties";

        /// <inheritdoc cref="ButtonOk"/>
        public string WindowTitleApplicationAlert { get; set; } = "Aplication says";

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

        /// <inheritdoc cref="ButtonOk"/>
        public string NotebookTabTitleSearch { get; set; } = "Search";

        // ========================

        /// <inheritdoc cref="ButtonOk"/>
        public string ListEditDefaultItemTitle { get; set; } = "Item";

        // ========================

        /// <inheritdoc cref="ButtonOk"/>
        public string FindOptionWrap { get; set; } = "Wrap";

        /// <inheritdoc cref="ButtonOk"/>
        public string FindOptionEntireWord { get; set; } = "Entire Word";

        /// <inheritdoc cref="ButtonOk"/>
        public string FindOptionMatchCase { get; set; } = "Match Case";

        /// <inheritdoc cref="ButtonOk"/>
        public string FindOptionHighlight { get; set; } = "Highlight";

        /// <inheritdoc cref="ButtonOk"/>
        public string FindOptionBackwards { get; set; } = "Backwards";

        // ========================

        /// <inheritdoc cref="ButtonOk"/>
        public string LineNumberTemplate { get; set; } = "Line Number ({0} - {1})";

        /// <inheritdoc cref="ButtonOk"/>
        public string LoadingPleaseWait { get; set; } = "Loading. Please wait...";

        /// <inheritdoc cref="ButtonOk"/>
        public string ToolbarSeeMore { get; set; } = "See more";
    }
}