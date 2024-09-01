using System;
using System.Collections.Generic;
using System.Text;

namespace ControlsSample
{
    public static class GenericStrings
    {
        public static string One = "One";
        public static string Two = "Two";
        public static string Three = "Three";
        public static string Four = "Four";
        public static string Five = "Five";
        public static string Six = "Six";
        public static string Seven = "Seven";
        public static string Eight = "Eight";
        public static string Nine = "Nine";
        public static string Ten = "Ten";

        public static string NoneInsideLessGreater = "<none>";
        public static string HitTestResult = "HitTest Result";
        public static string Item = "Item";
        public static string SelectedItem = "Selected Item";
        public static string TooManyIndexesToDisplay = "too many indexes to display";
        public static string RemoveItems = "Remove items";
        public static string TextVisible = "Text Visible";
        public static string SetColor = "Set Color";
        public static string BackColor = "Back Color";
        public static string Color = "Color";
        public static string ImageMargin = "Image Margin";
        public static string SomeText = "Some Text";
        public static string IndeterminateStateIsNotSet = "Indeterminate state is not set";

        public static string TabTitleActions = "Actions";
        public static string TabTitleFonts = "Fonts";
        public static string TabTitleProperties = "Properties";
        public static string TabTitlePages = "Pages";
        public static string TabTitleRange = "Range";

        public static string Options = "Options";
        public static string ShowHolidays = "Show Holidays";
        public static string NoMonthChange = "No Month Change";
        public static string UseGeneric = "Use Generic";
        public static string SequentalMonthSelect = "Sequental Month Select";
        public static string ShowSurroundWeeks = "Show Surround Weeks";
        public static string WeekNumbers = "Week Numbers";
        public static string Mark = "Mark";
        public static string DaysStyle = "Days Style";
        public static string MarkDays = "Mark Days";
        public static string Today = "Today";
        public static string Allow = "Allow";
        public static string Tomorrow = "Tomorrow";
        public static string Yesterday = "Yesterday";
        public static string AnyDate = "AnyDate";

        public static string ShowPopupWith = "Show Popup With";
        public static string ModalPopups = "Modal Popups";
        public static string ThisIsLongItemWhichOccupiesMoreSpaceThanOtherItems
            = "This is long item which occupies more space than other items";

        public static string BoolFalse = "false";
        public static string BoolTrue = "true";

        public static string Default = "Default";

        public static string GenericDirectionLeft = "Left";
        public static string GenericDirectionTop = "Top";
        public static string GenericDirectionRight = "Right";
        public static string GenericDirectionBottom = "Bottom";

        public static void AddTenRows(Action<string> action)
        {
            action(One);
            action(Two);
            action(Three);
            action(Four);
            action(Five);
            action(Six);
            action(Seven);
            action(Eight);
            action(Nine);
            action(Ten);
        }
    }
}
