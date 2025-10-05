using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Base.Collections;

namespace Alternet.UI
{
    public partial class Grid
    {
        /// <summary>
        /// Gets or sets <see cref="RowCount"/> and <see cref="ColumnCount"/>
        /// in the single property call.
        /// </summary>
        [Browsable(false)]
        public virtual RowColumnIndex RowColumnCount
        {
            get
            {
                return (RowCount, ColumnCount);
            }

            set
            {
                RowCount = value.Row;
                ColumnCount = value.Column;
            }
        }

        /// <summary>
        /// Gets or sets count of <see cref="RowDefinitions"/>.
        /// </summary>
        /// <remarks>
        /// If row count grows, <see cref="RowDefinition.CreateAuto"/> is used to add new rows.
        /// </remarks>
        public virtual int RowCount
        {
            get
            {
                return RowDefinitions.Count;
            }

            set
            {
                ListUtils.SetCount(RowDefinitions, value, RowDefinition.CreateAuto);
            }
        }

        /// <summary>
        /// Gets or sets count of <see cref="ColumnDefinitions"/>.
        /// </summary>
        /// <remarks>
        /// If column count grows, <see cref="ColumnDefinition.CreateAuto"/> is used to add new rows.
        /// </remarks>
        public virtual int ColumnCount
        {
            get
            {
                return ColumnDefinitions.Count;
            }

            set
            {
                ListUtils.SetCount(ColumnDefinitions, value, ColumnDefinition.CreateAuto);
            }
        }

        /// <summary>
        /// Sets a value that indicates which column child control within a <see cref="Grid"/>
        /// should appear in.
        /// </summary>
        /// <param name="control">The control on which to set the column index.</param>
        /// <param name="value">The 0-based column index to set.</param>
        public static void SetColumn(AbstractControl control, int value)
        {
            if (control is not null)
                control.ColumnIndex = value;
        }

        /// <summary>
        /// Sets a value that indicates which row and column child control within
        /// a <see cref="Grid"/> should appear in.
        /// </summary>
        /// <param name="control">The control on which to set the column index.</param>
        /// <param name="row">The 0-based row index to set.</param>
        /// <param name="col">The 0-based column index to set.</param>
        public static void SetRowColumn(AbstractControl control, int row, int col)
        {
            control?.SetRowColumn(row, col);
        }

        /// <summary>
        /// Gets a value that indicates which column child control within a <see cref="Grid"/>
        /// should appear in.
        /// </summary>
        /// <param name="control">The control for which to get the column index.</param>
        /// <remarks>The 0-based column index.</remarks>
        public static int GetColumn(AbstractControl control)
        {
            return control?.ColumnIndex ?? 0;
        }

        /// <summary>
        /// Sets a value that indicates which row child control within a <see cref="Grid"/>
        /// should appear in.
        /// </summary>
        /// <param name="control">The control on which to set the row index.</param>
        /// <param name="value">The 0-based row index to set.</param>
        public static void SetRow(AbstractControl control, int value)
        {
            if (control is not null)
                control.RowIndex = value;
        }

        /// <summary>
        /// Gets a value that indicates which row child control within
        /// a <see cref="Grid"/> should appear in.
        /// </summary>
        /// <param name="control">The control for which to get the row index.</param>
        /// <remarks>The 0-based row index.</remarks>
        public static int GetRow(AbstractControl control)
        {
            return control?.RowIndex ?? 0;
        }

        /// <summary>
        /// Gets a value that indicates the total number of rows that child content spans
        /// within a <see cref="Grid"/>.
        /// </summary>
        /// <param name="control">The control for which to get the row span.</param>
        /// <returns>The total number of rows that child content spans within a
        /// <see cref="Grid"/>.</returns>
        public static int GetRowSpan(AbstractControl control)
        {
            return control?.RowSpan ?? 1;
        }

        /// <summary>
        /// Sets a value that indicates the total number of rows that child content spans
        /// within a <see cref="Grid"/>.
        /// </summary>
        /// <param name="control">The control for which to set the row span.</param>
        /// <param name="value">The total number of rows that child content spans within
        /// a <see cref="Grid"/>.</param>
        public static void SetRowSpan(AbstractControl control, int value)
        {
            if (control is not null)
                control.RowSpan = value;
        }

        /// <summary>
        /// Gets a value that indicates the total number of columns that child content
        /// spans within a <see cref="Grid"/>.
        /// </summary>
        /// <param name="control">The control for which to get the column span.</param>
        /// <returns>The total number of columns that child content spans within a
        /// <see cref="Grid"/>.</returns>
        public static int GetColumnSpan(AbstractControl control)
        {
            return control?.ColumnSpan ?? 1;
        }

        /// <summary>
        /// Sets a value that indicates the total number of columns that child content
        /// spans within a <see cref="Grid"/>.
        /// </summary>
        /// <param name="control">The control for which to set the column span.</param>
        /// <param name="value">The total number of columns that child content spans
        /// within a <see cref="Grid"/>.</param>
        public static void SetColumnSpan(AbstractControl control, int value)
        {
            if (control is not null)
                control.ColumnSpan = value;
        }

        /// <summary>
        /// Initializes <see cref="Grid"/> rows and columns for the specified controls.
        /// </summary>
        /// <param name="controls">Grid controls.</param>
        /// <remarks>
        /// Dimensions for all rows and columns are set to <see cref="GridLength.Auto"/>.
        /// <see cref="AbstractControl.SetRowColumn"/> is called for the each control
        /// with row and column
        /// indexes equal to position in the <paramref name="controls"/> array.
        /// </remarks>
        public void Setup(AbstractControl[,] controls)
        {
            var rowCount = controls.GetLength(0);
            var colCount = controls.GetLength(1);

            this.DoInsideLayout(() =>
            {
                for (int i = 0; i < rowCount; i++)
                    this.AddAutoRow();
                for (int i = 0; i < colCount; i++)
                    this.AddAutoColumn();

                for (int i = 0; i < rowCount; i++)
                {
                    for (int j = 0; j < colCount; j++)
                    {
                        var control = controls[i, j];
                        control.Parent ??= this;
                        Grid.SetRowColumn(control, i, j);
                    }
                }
            });
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

        /// <inheritdoc/>
        public override void DefaultPaint(PaintEventArgs e)
        {
            base.DefaultPaint(e);
            DefaultPaintDebug(e);
        }

        /// <inheritdoc/>
        protected override void DefaultPaintDebug(PaintEventArgs e)
        {
            if (ShowDebugCorners)
                BorderSettings.DrawDesignCorners(e.Graphics, e.ClientRectangle);
        }
    }
}
