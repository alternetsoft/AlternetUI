using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Defines methods for serializing and deserializing date and time values to and from string representations.
    /// </summary>
    public interface IDateTimeSerialization
    {
        /// <summary>
        /// Converts a <see cref="DateTime"/> to a string representation.
        /// </summary>
        /// <param name="date">The date and time to convert.</param>
        /// <returns>A string representation of the date and time.</returns>
        string DateTimeToString(DateTime date);

        /// <summary>
        /// Converts a <see cref="DateOnly"/> to a string representation.
        /// </summary>
        /// <param name="date">The date to convert.</param>
        /// <returns>A string representation of the date.</returns>
        string DateToString(DateOnly date);

        /// <summary>
        /// Converts a <see cref="TimeOnly"/> to a string representation.
        /// </summary>
        /// <param name="time">The time to convert.</param>
        /// <returns>A string representation of the time.</returns>
        string TimeToString(TimeOnly time);

        /// <summary>
        /// Converts a string representation of a date and time to a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="s">The string representation of the date and time.</param>
        /// <param name="result">The converted <see cref="DateTime"/>
        /// if the conversion succeeds; otherwise, <c>DateTime.MinValue</c>.</param>
        /// <returns><c>true</c> if the conversion succeeds; otherwise, <c>false</c>.</returns>
        bool TryDateTimeFromString(string s, out DateTime result);

        /// <summary>
        /// Converts a string representation of a date to a <see cref="DateOnly"/>.
        /// </summary>
        /// <param name="s">The string representation of the date.</param>
        /// <param name="result">The converted <see cref="DateOnly"/>
        /// if the conversion succeeds; otherwise, <c>DateOnly.MinValue</c>.</param>
        /// <returns><c>true</c> if the conversion succeeds; otherwise, <c>false</c>.</returns>
        bool TryDateFromString(string s, out DateOnly result);

        /// <summary>
        /// Converts a string representation of a time to a <see cref="TimeOnly"/>.
        /// </summary>
        /// <param name="s">The string representation of the time.</param>
        /// <param name="result">The converted <see cref="TimeOnly"/>
        /// if the conversion succeeds; otherwise, <c>TimeOnly.MinValue</c>.</param>
        /// <returns><c>true</c> if the conversion succeeds; otherwise, <c>false</c>.</returns>
        bool TryTimeFromString(string s, out TimeOnly result);

        /// <summary>
        /// Converts a <see cref="TimeSpan"/> to a string representation.
        /// </summary>
        /// <param name="timeSpan">The time span to convert.</param>
        /// <returns>A string representation of the time span.</returns>
        string TimeSpanToString(TimeSpan timeSpan);

        /// <summary>
        /// Converts a string representation of a time span to a <see cref="TimeSpan"/>.
        /// </summary>
        /// <param name="s">The string representation of the time span.</param>
        /// <param name="result">The converted <see cref="TimeSpan"/>
        /// if the conversion succeeds; otherwise, <c>TimeSpan.Zero</c>.</param>
        /// <returns><c>true</c> if the conversion succeeds; otherwise, <c>false</c>.</returns>
        bool TryTimeSpanFromString(string s, out TimeSpan result);
    }
}
