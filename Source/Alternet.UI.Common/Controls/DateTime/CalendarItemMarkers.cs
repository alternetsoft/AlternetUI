using System;
using System.Collections.ObjectModel;

using Alternet.Drawing;
using Alternet.UI;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a marker for a calendar item, providing properties such as name, title, visibility,
    /// and brush getters for light and dark themes.
    /// </summary>
    public abstract class CalendarItemMarker : ImmutableObject
    {
        private readonly Func<Brush>? lightBrushGetter;
        private readonly Func<Brush>? darkBrushGetter;
        private readonly string? name;
        private string? title;
        private bool isVisible = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarItemMarker"/> class.
        /// </summary>
        public CalendarItemMarker()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarItemMarker"/>
        /// class with the specified name, title, and brush getters for light and dark themes.
        /// </summary>
        /// <param name="name">The name of the calendar item marker.</param>
        /// <param name="title">The title of the calendar item marker.</param>
        /// <param name="lightGetter">A function that returns the brush for the light theme.</param>
        /// <param name="darkGetter">A function that returns the brush for the dark theme.</param>
        public CalendarItemMarker(string? name, string? title, Func<Brush> lightGetter, Func<Brush> darkGetter)
        {
            this.name = name;
            this.title = title;
            this.lightBrushGetter = lightGetter;
            this.darkBrushGetter = darkGetter;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarItemMarker"/> class with the specified name,
        /// title, and color getters for light and dark themes.
        /// </summary>
        /// <param name="name">The name of the item.</param>
        /// <param name="title">The title of the item.</param>
        /// <param name="lightColorGetter">A function that returns the color for the light theme.</param>
        /// <param name="darkColorGetter">A function that returns the color for the dark theme.</param>
        public CalendarItemMarker(string name, string title, Func<Color> lightColorGetter, Func<Color> darkColorGetter)
        {
            this.name = name;
            this.title = title;
            this.lightBrushGetter = () => lightColorGetter().AsBrush;
            this.darkBrushGetter = () => darkColorGetter().AsBrush;
        }

        /// <summary>
        /// Gets the name of the item.
        /// </summary>
        public virtual string? Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the item is visible.
        /// </summary>
        public virtual bool IsVisible
        {
            get => isVisible;
            set
            {
                isVisible = value;
            }
        }

        /// <summary>
        /// Gets or sets the title of the item.
        /// </summary>
        public virtual string? Title
        {
            get => title;
            set
            {
                SetProperty(ref title, value);
            }
        }

        /// <summary>
        /// Represents a picker for calendar item markers, allowing users to select a marker.
        /// </summary>
        public abstract class MarkerPicker : DrawingResourcePicker
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="MarkerPicker"/> class.
            /// </summary>
            public MarkerPicker()
                : base()
            {
            }
        }
    }

    /// <summary>
    /// Represents a category of calendar items, such as "Anniversary", "Birthday", "Business", etc.
    /// </summary>
    public class CalendarItemCategory : CalendarItemMarker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarItemCategory"/> class.
        /// </summary>
        public CalendarItemCategory()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarItemCategory"/>
        /// class with the specified name, title, and brush getters for light and dark themes.
        /// </summary>
        /// <param name="name">The name of the calendar item category.</param>
        /// <param name="title">The title of the calendar item category.</param>
        /// <param name="lightGetter">A function that returns the brush for the light theme.</param>
        /// <param name="darkGetter">A function that returns the brush for the dark theme.</param>
        public CalendarItemCategory(string? name, string? title, Func<Brush> lightGetter, Func<Brush> darkGetter)
            : base(name, title, lightGetter, darkGetter)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarItemCategory"/> class with the specified name,
        /// title, and color getters for light and dark themes.
        /// </summary>
        /// <param name="name">The name of the item.</param>
        /// <param name="title">The title of the item.</param>
        /// <param name="lightColorGetter">A function that returns the color for the light theme.</param>
        /// <param name="darkColorGetter">A function that returns the color for the dark theme.</param>
        public CalendarItemCategory(string name, string title, Func<Color> lightColorGetter, Func<Color> darkColorGetter)
            : base(name, title, lightColorGetter, darkColorGetter)
        {
        }

        /// <summary>
        /// Represents a picker for calendar item categories, allowing users to select a category.
        /// </summary>
        public class CategoryPicker : MarkerPicker
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CategoryPicker"/> class.
            /// </summary>
            public CategoryPicker()
                : base()
            {
            }
        }
    }

    /// <summary>
    /// Represents a status of a calendar item, such as "Busy", "Free", "Tentative", etc.
    /// </summary>
    public class CalendarItemStatus : CalendarItemMarker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarItemStatus"/> class.
        /// </summary>
        public CalendarItemStatus()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarItemStatus"/>
        /// class with the specified name, title, and brush getters for light and dark themes.
        /// </summary>
        /// <param name="name">The name of the calendar item status.</param>
        /// <param name="title">The title of the calendar item status.</param>
        /// <param name="lightGetter">A function that returns the brush for the light theme.</param>
        /// <param name="darkGetter">A function that returns the brush for the dark theme.</param>
        public CalendarItemStatus(string? name, string? title, Func<Brush> lightGetter, Func<Brush> darkGetter)
            : base(name, title, lightGetter, darkGetter)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarItemStatus"/> class with the specified name,
        /// title, and color getters for light and dark themes.
        /// </summary>
        /// <param name="name">The name of the calendar item status.</param>
        /// <param name="title">The title of the calendar item status.</param>
        /// <param name="lightColorGetter">A function that returns the color for the light theme.</param>
        /// <param name="darkColorGetter">A function that returns the color for the dark theme.</param>
        public CalendarItemStatus(string name, string title, Func<Color> lightColorGetter, Func<Color> darkColorGetter)
            : base(name, title, lightColorGetter, darkColorGetter)
        {
        }

        /// <summary>
        /// Represents a picker for calendar item statuses, allowing users to select a status.
        /// </summary>
        public class StatusPicker : MarkerPicker
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="StatusPicker"/> class.
            /// </summary>
            public StatusPicker()
                : base()
            {
            }
        }
    }

    /// <summary>
    /// Represents a collection of known calendar item markers (such as categories and statuses).
    /// </summary>
    public static class KnownCalendarItemMarkers
    {

        /// <summary>
        /// Initializes static members of the <see cref="KnownCalendarItemMarkers"/> class.
        /// </summary>
        static KnownCalendarItemMarkers()
        {
            Anniversary = Create(
            "Anniversary",
            CommonStrings.Default.CalendarItemCategoryAnniversary,
            () => ItemColors.AnniversaryLight,
            () => ItemColors.AnniversaryDark);

            Birthday = Create(
            "Birthday",
            CommonStrings.Default.CalendarItemCategoryBirthday,
            () => ItemColors.BirthdayLight,
            () => ItemColors.BirthdayDark);

            Business = Create(
            "Business",
            CommonStrings.Default.CalendarItemCategoryBusiness,
            () => ItemColors.BusinessLight,
            () => ItemColors.BusinessDark);

            Important = Create(
                "Important",
                CommonStrings.Default.CalendarItemCategoryImportant,
                () => ItemColors.ImportantLight,
                () => ItemColors.ImportantDark);

            MustAttend = Create(
                "MustAttend",
                CommonStrings.Default.CalendarItemCategoryMustAttend,
                () => ItemColors.MustAttendLight,
                () => ItemColors.MustAttendDark);

            NeedPreparation = Create(
                "NeedPreparation",
                CommonStrings.Default.CalendarItemCategoryNeedPreparation,
                () => ItemColors.NeedPreparationLight,
                () => ItemColors.NeedPreparationDark);

            None = Create(
                "None",
                CommonStrings.Default.CalendarItemCategoryNone,
                () => ItemColors.NoneLight,
                () => ItemColors.NoneDark);
            None.IsVisible = false;

            Other = Create(
                "Other",
                CommonStrings.Default.CalendarItemCategoryOther,
                () => ItemColors.OtherLight,
                () => ItemColors.OtherDark);

            Personal = Create(
                "Personal",
                CommonStrings.Default.CalendarItemCategoryPersonal,
                () => ItemColors.PersonalLight,
                () => ItemColors.PersonalDark);

            PhoneCall = Create(
                "PhoneCall",
                CommonStrings.Default.CalendarItemCategoryPhoneCall,
                () => ItemColors.PhoneCallLight,
                () => ItemColors.PhoneCallDark);

            TravelRequired = Create(
                "TravelRequired",
                CommonStrings.Default.CalendarItemCategoryTravelRequired,
                () => ItemColors.TravelRequiredLight,
                () => ItemColors.TravelRequiredDark);

            Vacation = Create(
                "Vacation",
                CommonStrings.Default.CalendarItemCategoryVacation,
                () => ItemColors.VacationLight,
                () => ItemColors.VacationDark);

            static CalendarItemCategory Create(
                string name,
                string title,
                Func<Color> lightColorGetter,
                Func<Color> darkColorGetter)
            {
                var label = new CalendarItemCategory(name, title, lightColorGetter, darkColorGetter);
                return label;
            }

            Categories = new BaseCollection<CalendarItemCategory>
            {
                Anniversary,
                Birthday,
                Business,
                Important,
                MustAttend,
                NeedPreparation,
                None,
                Other,
                Personal,
                PhoneCall,
                TravelRequired,
                Vacation,
            };
        }

        /// <summary>
        /// Gets the "Anniversary" calendar item category.
        /// </summary>
        public static CalendarItemCategory Anniversary { get; }

        /// <summary>
        /// Gets the "Birthday" calendar item category.
        /// </summary>
        public static CalendarItemCategory Birthday { get; }

        /// <summary>
        /// Gets the "Business" calendar item category.
        /// </summary>
        public static CalendarItemCategory Business { get; }

        /// <summary>
        /// Gets the "Important" calendar item category.
        /// </summary>
        public static CalendarItemCategory Important { get; }

        /// <summary>
        /// Gets the "MustAttend" calendar item category.
        /// </summary>
        public static CalendarItemCategory MustAttend { get; }

        /// <summary>
        /// Gets the "NeedPreparation" calendar item category.
        /// </summary>
        public static CalendarItemCategory NeedPreparation { get; }

        /// <summary>
        /// Gets the "None" calendar item category.
        /// </summary>
        public static CalendarItemCategory None { get; }

        /// <summary>
        /// Gets the "Other" calendar item category.
        /// </summary>
        public static CalendarItemCategory Other { get; }

        /// <summary>
        /// Gets the "Personal" calendar item category.
        /// </summary>
        public static CalendarItemCategory Personal { get; }

        /// <summary>
        /// Gets the "PhoneCall" calendar item category.
        /// </summary>
        public static CalendarItemCategory PhoneCall { get; }

        /// <summary>
        /// Gets the "TravelRequired" calendar item category.
        /// </summary>
        public static CalendarItemCategory TravelRequired { get; }

        /// <summary>
        /// Gets the "Vacation" calendar item category.
        /// </summary>
        public static CalendarItemCategory Vacation { get; }

        /// <summary>
        /// Gets the collection of all calendar item categories.
        /// </summary>
        public static BaseCollection<CalendarItemCategory> Categories { get; set; } = new();

        /// <summary>
        /// Finds a calendar item category by its name.
        /// </summary>
        /// <param name="name">The name of the calendar item category.</param>
        /// <returns>The matching <see cref="CalendarItemCategory"/> if found; otherwise, <see cref="None"/>.</returns>
        public static CalendarItemCategory FindCategory(string name)
        {
            foreach (var item in Categories)
            {
                if (item.Name?.Equals(name) == true)
                    return item;
            }

            return None;
        }

        /// <summary>
        /// Gets the "Absent" calendar item status, which is used to indicate that a person is not present or unavailable.
        /// </summary>
        public static CalendarItemStatus Absent { get; set; } = new CalendarItemStatus(
            "Absent",
            CommonStrings.Default.CalendarItemStatusAbsent,
            () => Color.FromArgb(179, 21, 166),
            () => Color.FromArgb(225, 9, 208));

        /// <summary>
        /// Gets the "Busy" calendar item status, which is used to indicate that
        /// a person is occupied or unavailable due to other commitments.
        /// </summary>
        public static CalendarItemStatus Busy { get; set; } = new CalendarItemStatus(
            "Busy",
            CommonStrings.Default.CalendarItemStatusBusy,
            () => Color.FromArgb(31, 118, 193),
            () => Color.FromArgb(68, 152, 234));

        /// <summary>
        /// Gets the "Free" calendar item status, which is used to indicate that a person
        /// is available or not occupied with any other commitments.
        /// </summary>
        public static CalendarItemStatus Free { get; set; } = new CalendarItemStatus(
            "Free",
            CommonStrings.Default.CalendarItemStatusFree,
            () => Color.FromArgb(78, 201, 176),
            () => Color.FromArgb(43, 145, 175));

        /// <summary>
        /// Gets the "Tentative" calendar item status, which is used to indicate
        /// that a person's availability is uncertain or subject to change.
        /// </summary>
        public static CalendarItemStatus Tentative { get; set; } = new CalendarItemStatus(
            "Tentative",
            CommonStrings.Default.CalendarItemStatusTentative,
            () => new HatchBrush(HatchStyle.BackwardDiagonal, Color.FromArgb(31, 118, 193), Color.White),
            () => new HatchBrush(HatchStyle.BackwardDiagonal, Color.FromArgb(68, 152, 234), Color.FromArgb(41, 41, 41)));

        /// <summary>
        /// Gets the "Unknown" calendar item status, which is used when a status cannot be determined.
        /// </summary>
        public static CalendarItemStatus Unknown { get; set; } = new CalendarItemStatus(
            "Unknown",
            CommonStrings.Default.CalendarItemStatusUnknown,
            () => SystemColors.Window,
            () => SystemColors.Window) { IsVisible = false };

        /// <summary>
        /// Gets the collection of all calendar item statuses.
        /// </summary>
        public static Collection<CalendarItemStatus> Statuses { get; set; } =
            [Absent, Busy, Free, Tentative, Unknown];

        /// <summary>
        /// Finds a calendar item status by its name.
        /// </summary>
        /// <param name="name">The name of the calendar item status.</param>
        /// <returns>The matching <see cref="CalendarItemStatus"/> if found; otherwise, <see cref="Unknown"/>.</returns>
        public static CalendarItemStatus FindStatus(string name)
        {
            foreach (var item in Statuses)
            {
                if (item.Name?.Equals(name) == true)
                    return item;
            }

            return Unknown;
        }

        internal static class ItemColors
        {
            public static Color AnniversaryLight { get; set; } = LightDarkBackColors.LightTheme.Red;

            public static Color BirthdayLight { get; set; } = LightDarkBackColors.LightTheme.Orange;

            public static Color BusinessLight { get; set; } = LightDarkBackColors.LightTheme.Yellow;

            public static Color ImportantLight { get; set; } = LightDarkBackColors.LightTheme.Green;

            public static Color MustAttendLight { get; set; } = LightDarkBackColors.LightTheme.Teal;

            public static Color NeedPreparationLight { get; set; } = LightDarkBackColors.LightTheme.Cyan;

            public static Color NoneLight { get; set; } = LightDarkBackColors.LightTheme.Blue;

            public static Color OtherLight { get; set; } = LightDarkBackColors.LightTheme.Indigo;

            public static Color PersonalLight { get; set; } = LightDarkBackColors.LightTheme.Violet;

            public static Color TravelRequiredLight { get; set; } = LightDarkBackColors.LightTheme.Brown;

            public static Color PhoneCallLight { get; set; } = LightDarkBackColors.LightTheme.Pink;

            public static Color VacationLight { get; set; } = LightDarkBackColors.LightTheme.Gray;

            /* Dark colors */

            public static Color AnniversaryDark { get; set; } = LightDarkBackColors.DarkTheme.Red;

            public static Color BirthdayDark { get; set; } = LightDarkBackColors.DarkTheme.Orange;

            public static Color BusinessDark { get; set; } = LightDarkBackColors.DarkTheme.Yellow;

            public static Color ImportantDark { get; set; } = LightDarkBackColors.DarkTheme.Green;

            public static Color MustAttendDark { get; set; } = LightDarkBackColors.DarkTheme.Teal;

            public static Color NeedPreparationDark { get; set; } = LightDarkBackColors.DarkTheme.Cyan;

            public static Color NoneDark { get; set; } = LightDarkBackColors.DarkTheme.Blue;

            public static Color OtherDark { get; set; } = LightDarkBackColors.DarkTheme.Indigo;

            public static Color PersonalDark { get; set; } = LightDarkBackColors.DarkTheme.Violet;

            public static Color PhoneCallDark { get; set; } = LightDarkBackColors.DarkTheme.Pink;

            public static Color TravelRequiredDark { get; set; } = LightDarkBackColors.DarkTheme.Brown;

            public static Color VacationDark { get; set; } = LightDarkBackColors.DarkTheme.Gray;
        }
    }
}
