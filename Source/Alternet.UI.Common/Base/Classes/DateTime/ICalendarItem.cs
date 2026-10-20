using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Defines the interface for a calendar item that can be displayed in a calendar or scheduler control.
    /// </summary>
    public interface ICalendarItem
        : INotifyPropertyChanged, IDisposableObject, IImmutableObject, IBaseObjectWithAttr
    {
        /// <inheritdoc cref="CalendarItem.Rule"/>
        RepeatPatternRule Rule { get; set; }

        /// <inheritdoc cref="CalendarItem.AsJsonString"/>
        string AsJsonString { get; set; }

        /// <inheritdoc cref="CalendarItem.Title"/>
        string? Title { get; set; }

        /// <inheritdoc cref="CalendarItem.Description"/>
        string? Description { get; set; }

        /// <inheritdoc cref="CalendarItem.Location"/>
        object? Location { get; set; }

        /// <inheritdoc cref="CalendarItem.Status"/>
        CalendarItemCategory Category { get; set; }

        /// <inheritdoc cref="CalendarItem.Status"/>
        CalendarItemStatus Status { get; set; }

        /// <inheritdoc cref="CalendarItem.UserData"/>
        object? UserData { get; set; }

        /// <inheritdoc cref="CalendarItem.UserName"/>
        string? UserName { get; set; }

        /// <inheritdoc cref="CalendarItem.Visible"/>
        bool Visible { get; set; }

        /// <inheritdoc cref="CalendarItem.LoadFrom"/>
        bool LoadFrom(IPropertyBag? propertyBag);

        /// <inheritdoc cref="CalendarItem.SaveTo"/>
        void SaveTo(IPropertyBag propertyBag);

        /// <inheritdoc cref="CalendarItem.Equals(object)"/>
        bool Equals(object? obj);

        /// <inheritdoc cref="CalendarItem.Reset"/>
        void Reset();

        /// <inheritdoc cref="CalendarItem.Assign"/>
        void Assign(object? other);
    }
}
