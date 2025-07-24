using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a header interface for managing columns within a list box control.
    /// </summary>
    /// <remarks>This interface provides methods to manipulate columns in a list box,
    /// including adding, deleting, and modifying columns. It allows for the retrieval
    /// of associated controls and setting column
    /// properties such as width. Each column is identified by a unique identifier,
    /// and operations are performed based on this identifier.</remarks>
    public interface IListBoxHeader
    {
        /// <summary>
        /// Inserts a new column at the specified index with the given title and width.
        /// </summary>
        /// <remarks>The method creates a column with a header that includes both text
        /// and an image, and aligns the content to the left. A splitter is also added next
        /// to the column for resizing purposes. The column and splitter are inserted into the
        /// layout at the specified index, or appended if the index is out of range.</remarks>
        /// <param name="index">The zero-based index at which the column should be inserted.
        /// If the index is out of range, the column is added at the end.</param>
        /// <param name="title">The title text to display on the column header.</param>
        /// <param name="width">The width of the column.</param>
        /// <param name="onClick">An optional action to execute when the column header is clicked.
        /// Can be <see langword="null"/>.</param>
        /// <returns>The unique identifier of the newly inserted column.</returns>
        ObjectUniqueId InsertColumn(
            int index,
            string? title,
            Coord width,
            Action? onClick = null);

        /// <summary>
        /// Deletes the specified column and its associated splitter from the control.
        /// </summary>
        /// <remarks>This method attempts to find and remove the column identified by
        /// <paramref
        /// name="columnId"/>. If the column is found, it also disposes of the associated
        /// splitter, if any, before
        /// disposing of the column itself.</remarks>
        /// <param name="columnId">The unique identifier of the column to be deleted.</param>
        /// <returns><see langword="true"/> if the column and its associated splitter
        /// were successfully deleted; otherwise, <see langword="false"/>
        /// if the column was not found.</returns>
        bool DeleteColumn(ObjectUniqueId? columnId);

        /// <summary>
        /// Retrieves a <see cref="SpeedButton"/> associated with the specified column identifier.
        /// </summary>
        /// <remarks>This method searches for a child control matching the specified
        /// <paramref name="columnId"/> and returns it if it is
        /// of type <see cref="SpeedButton"/>. If no matching control is
        /// found, or if the control is not a <see cref="SpeedButton"/>,
        /// the method returns <see langword="null"/>.</remarks>
        /// <param name="columnId">The unique identifier of the column for which to retrieve
        /// the control.</param>
        /// <returns>A <see cref="SpeedButton"/> if the column is associated with
        /// a <see cref="SpeedButton"/>; otherwise,
        /// <see langword="null"/>.</returns>
        SpeedButton? GetColumnControl(ObjectUniqueId? columnId);

        /// <summary>
        /// Sets the title of the specified column.
        /// </summary>
        /// <param name="columnId">The unique identifier of the column whose title is to be set.</param>
        /// <param name="title">The new title for the column. If <see langword="null"/>,
        /// the title is set to an empty string.</param>
        /// <returns><see langword="true"/> if the column title was successfully set;
        /// otherwise, <see langword="false"/> if the
        /// column was not found.</returns>
        bool SetColumnTitle(ObjectUniqueId columnId, string? title);

        /// <summary>
        /// Sets the width of the specified column.
        /// </summary>
        /// <remarks>This method attempts to find the column associated with the given
        /// <paramref name="columnId"/> and set its width to the specified
        /// <paramref name="width"/>. If the column cannot be
        /// found, the method returns <see langword="false"/>.</remarks>
        /// <param name="columnId">The unique identifier of the column whose width is to be set.</param>
        /// <param name="width">The new width to apply to the column.</param>
        /// <returns><see langword="true"/> if the column width was successfully set;
        /// otherwise, <see langword="false"/>.</returns>
        bool SetColumnWidth(ObjectUniqueId columnId, Coord width);

        /// <summary>
        /// Adds a new column with the specified title and width to the control.
        /// </summary>
        /// <remarks>The method creates a new column with a label and an attached splitter.
        /// The label is configured to display both text and an image, and is docked
        /// to the left of the layout. The splitter is also docked to the left and is
        /// used to separate columns visually.</remarks>
        /// <param name="title">The text to display as the title of the column.
        /// This value cannot be null or empty.</param>
        /// <param name="width">The width of the column, specified as a <see cref="Coord"/> value.</param>
        /// <param name="onClick">The action to be executed when the column is clicked.</param>
        /// <returns>The unique identifier of the newly added column, represented
        /// as an <see cref="ObjectUniqueId"/>.</returns>
        ObjectUniqueId AddColumn(string? title, Coord width, Action? onClick = null);
    }
}
