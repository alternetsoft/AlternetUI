using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Defines an interface for managing the selection state of a specific year,
    /// allowing for selection of individual days within that year.
    /// </summary>
    public interface IYearSelectionManager
    {
        /// <summary>
        /// Gets the year for which this selection state is managed.
        /// </summary>
        int Year { get; }

        /// <summary>
        /// Determines whether any days are selected within the year.
        /// </summary>
        /// <returns><c>true</c> if any days are selected; otherwise, <c>false</c>.</returns>
        bool HasAnySelected();

        /// <summary>
        /// Selects or deselects a specific day within the year based on the provided selection state.
        /// </summary>
        /// <param name="day">The day of the year to select or deselect. Day starts from 1.</param>
        /// <param name="isSelected"><c>true</c> to select the day; <c>false</c> to deselect it.</param>
        void Select(int day, bool isSelected);

        /// <summary>
        /// Determines whether the specified day is selected.
        /// </summary>
        /// <param name="day">The day of the year to check. Day starts from 1.</param>
        /// <returns><c>true</c> if the specified day is selected; otherwise, <c>false</c>.</returns>
        bool IsSelected(int day);

        /// <summary>
        /// Selects or deselects a range of days within the year based on the provided selection state.
        /// </summary>
        /// <param name="startDay">The starting day of the range to select or deselect. Day starts from 1.</param>
        /// <param name="endDay">The ending day of the range to select or deselect. Day starts from 1.</param>
        /// <param name="isSelected"><c>true</c> to select the range; <c>false</c> to deselect it.</param>
        void SelectRange(int startDay, int endDay, bool isSelected = true);
    }

    /// <summary>
    /// Defines an interface for managing the selection of dates, allowing for selection of individual dates or ranges of dates.
    /// </summary>
    public interface IDateSelectionManager
    {
        /// <summary>
        /// Occurs when the selection of dates has changed, allowing subscribers to respond to changes in the selection state.
        /// </summary>
        public event EventHandler? SelectionChanged;

        /// <summary>
        /// Begins a batch update operation, suspending change notifications until <see cref="EndUpdate"/> is called.
        /// </summary>
        void BeginUpdate();

        /// <summary>
        /// Ends a batch update operation, resuming change notifications and optionally raising
        /// a change event if any changes occurred during the update.
        /// </summary>
        /// <param name="callChanged">Indicates whether to raise a change event
        /// if any changes occurred during the update.</param>
        void EndUpdate(bool callChanged);

        /// <summary>
        /// Called when selected dates have changed, raising the appropriate events.
        /// </summary>
        void Changed();

        /// <summary>
        /// Clears all selected dates, resetting the selection state.
        /// </summary>
        void Clear();

        /// <summary>
        /// Gets the maximum year for which a selection exists, or <c>null</c> if no selections exist.
        /// </summary>
        /// <returns>The maximum year selection, or <c>null</c> if no selections exist.</returns>
        int? GetMaxYear();

        /// <summary>
        /// Gets the minimum year for which a selection exists, or <c>null</c> if no selections exist.
        /// </summary>
        /// <returns>The minimum year selection, or <c>null</c> if no selections exist.</returns>
        int? GetMinYear();

        /// <summary>
        /// Determines whether the specified date is selected.
        /// </summary>
        /// <param name="date">The date to check.</param>
        /// <returns><c>true</c> if the specified date is selected; otherwise, <c>false</c>.</returns>
        bool IsSelected(DateOnly date);

        /// <summary>
        /// Selects only the specified date, clearing any previously selected dates.
        /// </summary>
        /// <param name="date">The date to select.</param>
        void SelectOnlyDate(DateOnly date);

        /// <summary>
        /// Selects only the specified range of dates, clearing any previously selected dates.
        /// </summary>
        /// <param name="startDate">The start date of the range to select.</param>
        /// <param name="endDate">The end date of the range to select.</param>
        void SelectOnlyRange(DateOnly startDate, DateOnly endDate);

        /// <summary>
        /// Selects or deselects the specified date based on the provided selection state.
        /// </summary>
        /// <param name="date">The date to select or deselect.</param>
        /// <param name="isSelected"><c>true</c> to select the date; <c>false</c> to deselect it.</param>
        void Select(DateOnly date, bool isSelected = true);

        /// <summary>
        /// Selects or deselects a range of dates based on the provided selection state.
        /// Range is inclusive of both start and end dates.
        /// </summary>
        /// <param name="startDate">The start date of the range.</param>
        /// <param name="endDate">The end date of the range.</param>
        /// <param name="isSelected"><c>true</c> to select the dates; <c>false</c> to deselect them.</param>    
        void SelectRange(DateOnly startDate, DateOnly endDate, bool isSelected = true);
    }

    /// <summary>
    /// Represents a manager for selecting dates, allowing for selection of individual dates or ranges of dates.
    /// </summary>
    public partial class DateSelectionManager : BaseObjectWithNotify, IDateSelectionManager
    {
        private readonly BaseDictionary<int, IYearSelectionManager> yearSelections = new();
        private IYearSelectionManager? cachedYearSelection;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateSelectionManager"/> class.
        /// </summary>
        public DateSelectionManager()
        {
        }

        /// <summary>
        /// Occurs when the selection of dates has changed, allowing subscribers to respond to changes in the selection state.
        /// </summary>
        public event EventHandler? SelectionChanged;

        /// <summary>
        /// Begins a batch update operation, suspending change notifications until <see cref="EndUpdate"/> is called.
        /// </summary>
        public virtual void BeginUpdate()
        {
            SuspendPropertyChanged();
        }

        /// <summary>
        /// Ends a batch update operation, resuming change notifications and optionally raising
        /// a change event if any changes occurred during the update.
        /// </summary>
        /// <param name="callChanged">Indicates whether to raise a change event
        /// if any changes occurred during the update.</param>
        public virtual void EndUpdate(bool callChanged)
        {
            ResumePropertyChanged(callChanged);
        }

        /// <summary>
        /// Called when selected dates have changed, raising the appropriate events.
        /// </summary>
        public virtual void Changed()
        {
            RaisePropertyChanged();
        }

        /// <summary>
        /// Clears all selected dates, resetting the selection state.
        /// </summary>
        public virtual void Clear()
        {
            yearSelections.Clear();
            cachedYearSelection = null;
            Changed();
        }

        /// <summary>
        /// Gets the maximum year for which a selection exists, or <c>null</c> if no selections exist.
        /// </summary>
        /// <returns>The maximum year selection, or <c>null</c> if no selections exist.</returns>
        public int? GetMaxYear()
        {
            var maxYearSelection = GetMaxYearSelection();
            return maxYearSelection?.Year;
        }

        /// <summary>
        /// Gets the minimum year for which a selection exists, or <c>null</c> if no selections exist.
        /// </summary>
        /// <returns>The minimum year selection, or <c>null</c> if no selections exist.</returns>
        public int? GetMinYear()
        {
            var minYearSelection = GetMinYearSelection();
            return minYearSelection?.Year;
        }

        /// <summary>
        /// Determines whether the specified date is selected.
        /// </summary>
        /// <param name="date">The date to check.</param>
        /// <returns><c>true</c> if the specified date is selected; otherwise, <c>false</c>.</returns>
        public virtual bool IsSelected(DateOnly date)
        {
            var selection = GetYearSelection(date.Year);

            if (selection is null)
                return false;

            return selection.IsSelected(date.DayOfYear);
        }

        /// <summary>
        /// Selects only the specified date, clearing any previously selected dates.
        /// </summary>
        /// <param name="date">The date to select.</param>
        public virtual void SelectOnlyDate(DateOnly date)
        {
            DoInsideSuspendedPropertyChanged(() =>
            {
                Clear();
                Select(date, true);
            });
        }

        /// <summary>
        /// Selects only the specified range of dates, clearing any previously selected dates.
        /// </summary>
        /// <param name="startDate">The start date of the range to select.</param>
        /// <param name="endDate">The end date of the range to select.</param>
        public virtual void SelectOnlyRange(DateOnly startDate, DateOnly endDate)
        {
            DoInsideSuspendedPropertyChanged(() =>
            {
                Clear();
                SelectRange(startDate, endDate, true);
            });
        }

        /// <summary>
        /// Clears the selection for the specified year, removing any selected days within that year.
        /// </summary>
        /// <param name="year">The year for which to clear the selection.</param>
        /// <returns><c>true</c> if there were any selections for the specified year; otherwise, <c>false</c>.</returns>
        public virtual bool ClearYearSelection(int year)
        {
            return yearSelections.Remove(year);
        }

        /// <summary>
        /// Selects or deselects the specified date based on the provided selection state.
        /// </summary>
        /// <param name="date">The date to select or deselect.</param>
        /// <param name="isSelected"><c>true</c> to select the date; <c>false</c> to deselect it.</param>
        public virtual void Select(DateOnly date, bool isSelected = true)
        {
            if (isSelected)
            {
                var selection = GetOrCreateYearSelection(date.Year);
                selection.Select(date.DayOfYear, true);
            }
            else
            {
                var selection = GetYearSelection(date.Year);
                if (selection != null)
                {
                    selection.Select(date.DayOfYear, false);
                    if (!selection.HasAnySelected())
                        ClearYearSelection(date.Year);
                }
            }
        }

        /// <summary>
        /// Selects or deselects a range of dates based on the provided selection state.
        /// Range is inclusive of both start and end dates.
        /// </summary>
        /// <param name="startDate">The start date of the range.</param>
        /// <param name="endDate">The end date of the range.</param>
        /// <param name="isSelected"><c>true</c> to select the dates; <c>false</c> to deselect them.</param>    
        public virtual void SelectRange(DateOnly startDate, DateOnly endDate, bool isSelected = true)
        {
            if (startDate == endDate)
            {
                Select(startDate, isSelected);
                return;
            }

            if (startDate > endDate)
            {
                (startDate, endDate) = (endDate, startDate);
            }

            DoInsideSuspendedPropertyChanged(() =>
            {
                if (isSelected)
                {
                    while (startDate <= endDate)
                    {
                        var selection = GetOrCreateYearSelection(startDate.Year);
                        selection.Select(startDate.DayOfYear, true);
                        startDate = startDate.AddDays(1);
                    }
                }
                else
                {
                    while (startDate <= endDate)
                    {
                        var selection = GetYearSelection(startDate.Year);
                        if (selection != null)
                        {
                            selection.Select(startDate.DayOfYear, false);
                            if (!selection.HasAnySelected())
                                ClearYearSelection(startDate.Year);
                        }

                        startDate = startDate.AddDays(1);
                    }
                }
            });
        }

        /// <summary>
        /// Gets the selection with the maximum year, or <c>null</c> if no selections exist.
        /// </summary>
        /// <returns>The maximum year selection, or <c>null</c> if no selections exist.</returns>
        protected virtual IYearSelectionManager? GetMaxYearSelection()
        {
            IYearSelectionManager? maxYearSelection = null;
            foreach (var yearSelection in yearSelections)
            {
                if (maxYearSelection is null || yearSelection.Key > maxYearSelection.Year)
                {
                    maxYearSelection = yearSelection.Value;
                }
            }

            return maxYearSelection;
        }

        /// <summary>
        /// Gets the selection with the minimum year, or <c>null</c> if no selections exist.
        /// </summary>
        /// <returns>The minimum year selection, or <c>null</c> if no selections exist.</returns>
        protected virtual IYearSelectionManager? GetMinYearSelection()
        {
            IYearSelectionManager? minYearSelection = null;
            foreach (var yearSelection in yearSelections)
            {
                if (minYearSelection is null || yearSelection.Key < minYearSelection.Year)
                {
                    minYearSelection = yearSelection.Value;
                }
            }

            return minYearSelection;
        }

        /// <summary>
        /// Gets the selection state for a specific year, allowing for selection
        /// of individual days within that year.
        /// </summary>
        /// <param name="year">The year for which to get the selection state.</param>
        /// <returns>The selection state for the specified year, or <c>null</c> if no selection exists for that year.</returns>
        protected virtual IYearSelectionManager? GetYearSelection(int year)
        {
            if (cachedYearSelection?.Year == year)
                return cachedYearSelection;

            if (yearSelections.TryGetValue(year, out var yearSelection))
            {
                cachedYearSelection = yearSelection;
                return yearSelection;
            }

            return null;
        }

        /// <summary>
        /// Gets or creates the selection state for a specific year, allowing
        /// for selection of individual days within that year.
        /// </summary>
        /// <param name="year">The year for which to get or create the selection state.</param>
        /// <returns>The selection state for the specified year.</returns>
        protected virtual IYearSelectionManager GetOrCreateYearSelection(int year)
        {
            if (cachedYearSelection?.Year == year)
                return cachedYearSelection;

            var selection = yearSelections.GetOrCreate(year, () => CreateYearSelection(year));
            cachedYearSelection = selection;
            return selection;
        }

        /// <summary>
        /// Creates a new instance of <see cref="IYearSelectionManager"/> to manage
        /// the selection state for a specific year.
        /// </summary>
        /// <param name="year">The year for which to create the selection state.</param>
        /// <returns>The newly created selection state for a year.</returns>
        protected virtual IYearSelectionManager CreateYearSelection(int year)
        {
            var selection = new YearSelection(year);
            return selection;
        }

        /// <inheritdoc/>
        protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Represents the selection state of a specific year, allowing for selection of individual days within that year.
        /// </summary>
        private class YearSelection : IYearSelectionManager
        {
            private readonly BitArray selectedDays = new(366);

            /// <summary>
            /// Initializes a new instance of the <see cref="YearSelection"/> class.
            /// </summary>
            /// <param name="year">The year for which to manage the selection state.</param>
            public YearSelection(int year)
            {
                Year = year;
            }

            /// <summary>
            /// Gets the year for which this selection state is managed.
            /// </summary>
            public int Year { get; }

            public bool HasAnySelected()
            {
                return selectedDays.HasAnySet();
            }

            /// <inheritdoc/>
            public void SelectRange(int startDay, int endDay, bool isSelected = true)
            {
                for (int day = startDay; day <= endDay; day++)
                {
                    selectedDays[day] = isSelected;
                }
            }

            /// <inheritdoc/>
            public void Select(int day, bool isSelected)
            {
                selectedDays[day] = isSelected;
            }

            /// <inheritdoc/>
            public bool IsSelected(int day)
            {
                return selectedDays[day];
            }
        }
    }
}
