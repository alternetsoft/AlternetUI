using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Alternet.UI.Localization
{
    /// <summary>
    /// Defines localizations for common strings.
    /// </summary>
    public partial class CommonStrings
    {
        /// <summary>
        /// Current localizations for common strings.
        /// </summary>
        public static CommonStrings Default { get; set; } = new();

        public string NoValue { get; set; } = "no value";

        public string ButtonOk { get; set; } = "Ok";

        public string TransparentColorDisplayName { get; set; } = "Transparent";

        public string EmptyColorDisplayName { get; set; } = "Empty";

        public string DefaultFontDisplayName { get; set; } = "Default";

        public string DefaultMonoFontDisplayName { get; set; } = "Default Monospace";

        public string ButtonFilter { get; set; } = "Filter";

        public string ButtonQuit { get; set; } = "Quit";

        public string ButtonContinue { get; set; } = "Continue";

        public string ButtonThrow { get; set; } = "Throw";

        public string ButtonDetails { get; set; } = "Details";

        public string ButtonCancel { get; set; } = "Cancel";

        public string ButtonReset { get; set; } = "Reset";

        public string ButtonFind { get; set; } = "Find";

        public string ButtonFindNext { get; set; } = "Find Next";

        public string ButtonOptions { get; set; } = "Options";

        public string ButtonFindPrevious { get; set; } = "Find Previous";

        public string ButtonReplace { get; set; } = "Replace";

        public string ButtonReplaceAll { get; set; } = "Replace All";

        public string ButtonPrint { get; set; } = "Print";

        public string ButtonApply { get; set; } = "Apply";

        public string ButtonAdd { get; set; } = "Add";

        public string ButtonBack { get; set; } = "Back";

        public string ButtonNew { get; set; } = "New";

        public string ButtonOpen { get; set; } = "Open";

        public string ButtonSave { get; set; } = "Save";

        public string ButtonForward { get; set; } = "Forward";

        public string ButtonGo { get; set; } = "Go";

        public string ButtonZoomIn { get; set; } = "Zoom In";

        public string ButtonZoomOut { get; set; } = "Zoom Out";

        public string ButtonAddChild { get; set; } = "Add Child";

        public string ButtonRemove { get; set; } = "Remove";

        public string ButtonRemoveAll { get; set; } = "Remove All";

        public string ButtonClear { get; set; } = "Clear";

        public string ButtonClearAll { get; set; } = "Clear All";

        public string ButtonClose { get; set; } = "Close";

        public string ButtonMinimize { get; set; } = "Minimize";

        public string ButtonMaximize { get; set; } = "Maximize";

        public string ButtonCopy { get; set; } = "Copy";

        public string ButtonPassword { get; set; } = "Password";

        public string ButtonPaste { get; set; } = "Paste";

        public string ButtonCut { get; set; } = "Cut";

        public string ButtonUndo { get; set; } = "Undo";

        public string ButtonRedo { get; set; } = "Redo";

        public string ButtonBold { get; set; } = "Bold";

        public string ButtonSearch { get; set; } = "Search";

        public string ButtonItalic { get; set; } = "Italic";

        public string ButtonUnderline { get; set; } = "Underline";

        public string ButtonYes { get; set; } = "Yes";

        public string ButtonNo { get; set; } = "No";

        public string ButtonAbort { get; set; } = "Abort";

        public string ButtonRetry { get; set; } = "Retry";

        public string ButtonIgnore { get; set; } = "Ignore";

        public string ButtonMoreActions { get; set; } = "More Actions";

        public string ButtonHelp { get; set; } = "Help";

        public string ButtonAll { get; set; } = "All";

        // ========================

        public string ToolBarPreviousTabToolTip { get; set; } = "Previous Tab";

        public string ToolBarNextTabToolTip { get; set; } = "Next Tab";

        // ========================

        public string WindowTitleExceptionDetails { get; set; } = "Exception Details";

        public string WindowTitleSearchAndReplace { get; set; } = "Search and Replace";

        public string WindowTitleSearch { get; set; } = "Search";

        public string WindowTitleReplace { get; set; } = "Replace";

        public string WindowTitleListEdit { get; set; } = "List Editor";

        public string WindowTitleGoToLine { get; set; } = "Go To Line";

        public string WindowTitleInput { get; set; } = "Input";

        public string WindowTitleProperties { get; set; } = "Properties";

        public string WindowTitleInfo { get; set; } = "Info";

        public string WindowTitleApplicationAlert { get; set; } = "Application Says";

        public string WindowTitleSelectColor { get; set; } = "Select a Color";

        public string WindowTitleSelectValue { get; set; } = "Select a Value";

        public string WindowTitleSelectValues { get; set; } = "Select Values";

        public string WindowTitleSelectItem { get; set; } = "Select an Item";

        public string WindowTitleSelectItems { get; set; } = "Select Items";

        public string WindowTitleSelectFontName { get; set; } = "Select a Font Name";

        public string WindowTitleSelectFontSize { get; set; } = "Select a Font Size";

        public string WindowTitleSelectDate { get; set; } = "Select a Date";

        // ========================

        public string NotebookTabTitleBrowser { get; set; } = "Browser";

        public string NotebookTabTitleEvents { get; set; } = "Events";

        public string NotebookTabTitleOutput { get; set; } = "Output";

        public string NotebookTabTitleActivity { get; set; } = "Activity";

        public string NotebookTabTitleActions { get; set; } = "Actions";

        // ========================

        public string ListEditDefaultItemTitle { get; set; } = "Item";

        // ========================

        public string SearchFor { get; set; } = "Search for";

        public string ReplaceWith { get; set; } = "Replace with";

        public string FindOptionWrap { get; set; } = "Wrap around";

        public string FindOptionHiddenText { get; set; } = "Search hidden text";

        public string FindOptionPromptOnReplace { get; set; } = "Prompt on replace";

        public string FindOptionMatchCase { get; set; } = "Match case";

        public string FindOptionUseRegularExpressions { get; set; } = "Regular expressions";

        public string FindOptionMatchWholeWord { get; set; } = "Match whole word";

        public string FindOptionHighlight { get; set; } = "Highlight";

        public string FindOptionSelectionOnly { get; set; } = "Selection Only";

        public string FindOptionBackwards { get; set; } = "Backwards";

        public string FindScopeCurrentDocument { get; set; } = "Current Document";

        public string FindScopeAllOpenDocuments { get; set; } = "All Open Documents";

        public string FindScopeCurrentProject { get; set; } = "Current Project";

        public string FindScopeSelectionOnly { get; set; } = "Selection Only";

        public string ToggleToSwitchBetweenFindReplace { get; set; } =
            "Toggle to switch between find and replace modes";

        // ========================

        public string LineNumber { get; set; } = "Line Number";

        public string LoadingPleaseWait { get; set; } = "Please wait...";

        public string ToolbarSeeMore { get; set; } = "See more";

        public string NoPreviewAvailable { get; set; } = "No preview available.";

        public string SelectFileToPreview { get; set; } = "Select a file to preview.";

        public string Starts { get; set; } = "Starts";

        public string Ends { get; set; } = "Ends";

        // ========================

        public string FileListBoxColumnName { get; set; } = "Name";

        public string FileListBoxColumnDateModified { get; set; } = "Date modified";

        public string FileListBoxColumnSize { get; set; } = "Size";

        // ========================

        public string EnterValue { get; set; } = "Enter value";

        public string DoubleClick { get; set; } = "Double click";

        // ========================

        public string TimePeriodUnitYear { get; set; } = "year";

        public string TimePeriodUnitMonth { get; set; } = "month";

        public string TimePeriodUnitWeek { get; set; } = "week";

        public string TimePeriodUnitDay { get; set; } = "day";

        public string TimePeriodUnitHour { get; set; } = "hour";

        public string TimePeriodUnitMinute { get; set; } = "minute";

        public string TimePeriodUnitSecond { get; set; } = "second";

        public string TimePeriodUnitMillisecond { get; set; } = "millisecond";

        public string TimePeriodUnitYears { get; set; } = "years";

        public string TimePeriodUnitMonths { get; set; } = "months";

        public string TimePeriodUnitWeeks { get; set; } = "weeks";

        public string TimePeriodUnitDays { get; set; } = "days";

        public string TimePeriodUnitHours { get; set; } = "hours";

        public string TimePeriodUnitMinutes { get; set; } = "minutes";

        public string TimePeriodUnitSeconds { get; set; } = "seconds";

        public string TimePeriodUnitMilliseconds { get; set; } = "milliseconds";

        // ========================

        public string RelativeWeekdayFirst { get; set; } = "First";

        public string RelativeWeekdaySecond { get; set; } = "Second";

        public string RelativeWeekdayThird { get; set; } = "Third";

        public string RelativeWeekdayFourth { get; set; } = "Fourth";

        public string RelativeWeekdayLast { get; set; } = "Last";

        public string ExtendedDayOfWeekDay { get; set; } = "Day";

        public string ExtendedDayOfWeekWeekday { get; set; } = "Weekday";

        public string ExtendedDayOfWeekWeekend { get; set; } = "Weekend";

        // ========================
        public string ScheduleRepeatPatternNone { get; set; } = "None";

        public string ScheduleRepeatPatternDaily { get; set; } = "Daily";

        public string ScheduleRepeatPatternWeekly { get; set; } = "Weekly";

        public string ScheduleRepeatPatternMonthly { get; set; } = "Monthly";

        public string ScheduleRepeatPatternYearly { get; set; } = "Yearly";

        // ========================

        public string DateRepeatPatternPrefixLabelEvery { get; set; } = "Every";

        public string DailyRepeatPatternRuleKindEveryDay { get; set; } = "Every day";
        
        public string DailyRepeatPatternRuleKindEvenDays { get; set; } = "On even days";
        
        public string DailyRepeatPatternRuleKindOddDays { get; set; } = "On odd days";
        
        public string DailyRepeatPatternRuleKindWeekdays { get; set; } = "On weekdays";
        
        public string DailyRepeatPatternRuleKindWeekends { get; set; } = "On weekends";

        // ========================

        public string DayOfWeekAndMonthSeparator { get; set; } = "of";

        public string OnThePrefix { get; set; } = "On the";

        public string OnPrefix { get; set; } = "On";

        public string OnDayPrefix { get; set; } = "On day";

        public string Never { get; set; } = "Never";

        public string After { get; set; } = "After";

        public string Occurrence { get; set; } = "occurrence";

        public string Occurrences { get; set; } = "occurrences";

        public string Repeat { get; set; } = "Repeat";

        public string Today { get; set; } = "Today";
    }
}