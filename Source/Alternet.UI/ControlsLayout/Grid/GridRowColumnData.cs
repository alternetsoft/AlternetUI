﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Base.Collections;

namespace Alternet.UI
{
    public partial class Grid
    {
        /// <summary>
        /// Gets or sets count of <see cref="RowDefinitions"/>.
        /// </summary>
        /// <remarks>
        /// If row count grows, <see cref="RowDefinition.CreateAuto"/> is used to add new rows.
        /// </remarks>
        public int RowCount
        {
            get
            {
                return RowDefinitions.Count;
            }

            set
            {
                if (value < 0)
                    throw new ArgumentException(nameof(RowCount));
                ListUtils.SetCount(RowDefinitions, value, RowDefinition.CreateAuto);
            }
        }

        /// <summary>
        /// Gets or sets count of <see cref="ColumnDefinitions"/>.
        /// </summary>
        /// <remarks>
        /// If column count grows, <see cref="ColumnDefinition.CreateAuto"/> is used to add new rows.
        /// </remarks>
        public int ColumnCount
        {
            get
            {
                return ColumnDefinitions.Count;
            }

            set
            {
                if (value < 0)
                    throw new ArgumentException(nameof(ColumnCount));
                ListUtils.SetCount(ColumnDefinitions, value, ColumnDefinition.CreateAuto);
            }
        }

        /// <summary>
        /// Sets a value that indicates which column child control within a <see cref="Grid"/>
        /// should appear in.
        /// </summary>
        /// <param name="control">The control on which to set the column index.</param>
        /// <param name="value">The 0-based column index to set.</param>
        public static void SetColumn(Control control, int value)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));

            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            control.columnIndex = value;
            OnCellAttachedPropertyChanged(control);
        }

        /// <summary>
        /// Sets a value that indicates which row and column child control within
        /// a <see cref="Grid"/> should appear in.
        /// </summary>
        /// <param name="control">The control on which to set the column index.</param>
        /// <param name="row">The 0-based row index to set.</param>
        /// <param name="col">The 0-based column index to set.</param>
        public static void SetRowColumn(Control control, int row, int col)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));
            if (row < 0)
                throw new ArgumentOutOfRangeException(nameof(row));
            if (col < 0)
                throw new ArgumentOutOfRangeException(nameof(col));
            control.rowIndex = row;
            control.columnIndex = col;
            OnCellAttachedPropertyChanged(control);
        }

        /// <summary>
        /// Gets a value that indicates which column child control within a <see cref="Grid"/>
        /// should appear in.
        /// </summary>
        /// <param name="control">The control for which to get the column index.</param>
        /// <remarks>The 0-based column index.</remarks>
        public static int GetColumn(Control control)
        {
            if (control is null)
                throw new ArgumentNullException(nameof(control));
            return control.columnIndex;
        }

        /// <summary>
        /// Sets a value that indicates which row child control within a <see cref="Grid"/>
        /// should appear in.
        /// </summary>
        /// <param name="control">The control on which to set the row index.</param>
        /// <param name="value">The 0-based row index to set.</param>
        public static void SetRow(Control control, int value)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));

            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            control.rowIndex = value;
            OnCellAttachedPropertyChanged(control);
        }

        /// <summary>
        /// Gets a value that indicates which row child control within a <see cref="Grid"/> should appear in.
        /// </summary>
        /// <param name="control">The control for which to get the row index.</param>
        /// <remarks>The 0-based row index.</remarks>
        public static int GetRow(Control control)
        {
            if (control is null)
                throw new ArgumentNullException(nameof(control));
            return control.rowIndex;
        }

        /// <summary>
        /// Gets a value that indicates the total number of rows that child content spans
        /// within a <see cref="Grid"/>.
        /// </summary>
        /// <param name="control">The control for which to get the row span.</param>
        /// <returns>The total number of rows that child content spans within a
        /// <see cref="Grid"/>.</returns>
        public static int GetRowSpan(Control control)
        {
            if (control is null)
                throw new ArgumentNullException(nameof(control));
            return control.rowSpan;
        }

        /// <summary>
        /// Sets a value that indicates the total number of rows that child content spans
        /// within a <see cref="Grid"/>.
        /// </summary>
        /// <param name="control">The control for which to set the row span.</param>
        /// <param name="value">The total number of rows that child content spans within
        /// a <see cref="Grid"/>.</param>
        public static void SetRowSpan(Control control, int value)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));
            if (value < 1)
                throw new ArgumentOutOfRangeException(nameof(value));

            control.rowSpan = value;
            OnCellAttachedPropertyChanged(control);
        }

        /// <summary>
        /// Gets a value that indicates the total number of columns that child content
        /// spans within a <see cref="Grid"/>.
        /// </summary>
        /// <param name="control">The control for which to get the column span.</param>
        /// <returns>The total number of columns that child content spans within a
        /// <see cref="Grid"/>.</returns>
        public static int GetColumnSpan(Control control)
        {
            if (control is null)
                throw new ArgumentNullException(nameof(control));
            return control.columnSpan;
        }

        /// <summary>
        /// Sets a value that indicates the total number of columns that child content
        /// spans within a <see cref="Grid"/>.
        /// </summary>
        /// <param name="control">The control for which to set the column span.</param>
        /// <param name="value">The total number of columns that child content spans
        /// within a <see cref="Grid"/>.</param>
        public static void SetColumnSpan(Control control, int value)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));
            if (value < 1)
                throw new ArgumentOutOfRangeException(nameof(value));
            control.columnSpan = value;
            OnCellAttachedPropertyChanged(control);
        }

        /// <summary>
        /// Adds <see cref="RowDefinition"/> instance with
        /// <see cref="RowDefinition.Height"/> equal to
        /// <see cref="GridLength.Auto"/>.
        /// </summary>
        public RowDefinition AddAutoRow()
        {
            var result = RowDefinition.CreateAuto();
            RowDefinitions.Add(result);
            return result;
        }

        /// <summary>
        /// Adds <see cref="ColumnDefinition"/> instance
        /// with <see cref="ColumnDefinition.Width"/>
        /// equal to <see cref="GridLength.Auto"/>.
        /// </summary>
        public ColumnDefinition AddAutoColumn()
        {
            var result = ColumnDefinition.CreateAuto();
            ColumnDefinitions.Add(result);
            return result;
        }

        /// <summary>
        /// Adds <see cref="RowDefinition"/> instance with <see cref="RowDefinition.Height"/>
        /// equal to <see cref="GridLength.Star"/>.
        /// </summary>
        public RowDefinition AddStarRow()
        {
            var result = new RowDefinition { Height = new GridLength(1, GridUnitType.Star) };
            RowDefinitions.Add(result);
            return result;
        }

        /// <summary>
        /// Adds <see cref="ColumnDefinition"/> instance with <see cref="ColumnDefinition.Width"/>
        /// equal to <see cref="GridLength.Star"/>.
        /// </summary>
        public ColumnDefinition AddStarColumn()
        {
            var result = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) };
            ColumnDefinitions.Add(result);
            return result;
        }
    }
}
