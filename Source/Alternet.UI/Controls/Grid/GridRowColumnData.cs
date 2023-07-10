using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public partial class Grid
    {
        /*static Dictionary<Control, int> controlColumns = new Dictionary<Control, int>();
        static Dictionary<Control, int> controlRows = new Dictionary<Control, int>();
        static Dictionary<Control, int> controlColumnSpans = new Dictionary<Control, int>();
        static Dictionary<Control, int> controlRowSpans = new Dictionary<Control, int>();*/
        // static Dictionary<Control, bool> controlIsSharedSizeScopes = new Dictionary<Control, bool>();

        /// <summary>
        /// Sets a value that indicates which column child control within a <see cref="Grid"/> should appear in.
        /// </summary>
        /// <param name="control">The control on which to set the column index.</param>
        /// <param name="value">The 0-based column index to set.</param>
        public static void SetColumn(Control control, int value)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));

            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));

            /*controlColumns[control] = value;*/

            control.ExtendedProps.GridColumn = value;
            OnCellAttachedPropertyChanged(control);
        }

        /// <summary>
        /// Sets a value that indicates which row and column child control within a <see cref="Grid"/> should appear in.
        /// </summary>
        /// <param name="control">The control on which to set the column index.</param>
        /// <param name="row">The 0-based row index to set.</param>
        /// <param name="col">The 0-based column index to set.</param>
        public static void SetRowColumn(Control control, int row, int col)
        {
            SetRow(control, row);
            SetColumn(control, col);
        }

        /// <summary>
        /// Gets a value that indicates which column child control within a <see cref="Grid"/> should appear in.
        /// </summary>
        /// <param name="control">The control for which to get the column index.</param>
        /// <remarks>The 0-based column index.</remarks>
        public static int GetColumn(Control control)
        {
            if (control is null)
                throw new ArgumentNullException(nameof(control));

            /*return controlColumns.TryGetValue(control, out var value) ? value : 0;*/

            if (control.HasExtendedProps)
                return control.ExtendedProps.GridColumn;
            else
                return 0;
        }

        /// <summary>
        /// Sets a value that indicates which row child control within a <see cref="Grid"/> should appear in.
        /// </summary>
        /// <param name="control">The control on which to set the row index.</param>
        /// <param name="value">The 0-based row index to set.</param>
        public static void SetRow(Control control, int value)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));

            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));

            /*controlRows[control] = value;*/

            control.ExtendedProps.GridRow = value;
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

            /*return controlRows.TryGetValue(control, out var value) ? value : 0;*/

            if (control.HasExtendedProps)
                return control.ExtendedProps.GridRow;
            else
                return 0;

        }

        /// <summary>
        /// Gets a value that indicates the total number of rows that child content spans within a <see cref="Grid"/>.
        /// </summary>
        /// <param name="control">The control for which to get the row span.</param>
        /// <returns>The total number of rows that child content spans within a <see cref="Grid"/>.</returns>
        public static int GetRowSpan(Control control)
        {
            if (control is null)
                throw new ArgumentNullException(nameof(control));

            /*return controlRowSpans.TryGetValue(control, out var value) ? value : 1;*/

            if (control.HasExtendedProps)
                return control.ExtendedProps.GridRowSpan;
            else
                return 1;
        }

        /// <summary>
        /// Sets a value that indicates the total number of rows that child content spans within a <see cref="Grid"/>.
        /// </summary>
        /// <param name="control">The control for which to set the row span.</param>
        /// <param name="value">The total number of rows that child content spans within a <see cref="Grid"/>.</param>
        public static void SetRowSpan(Control control, int value)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));

            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));

            /*controlRowSpans[control] = value;*/

            control.ExtendedProps.GridRowSpan = value;
            OnCellAttachedPropertyChanged(control);
        }

        /// <summary>
        /// Gets a value that indicates the total number of columns that child content spans within a <see cref="Grid"/>.
        /// </summary>
        /// <param name="control">The control for which to get the column span.</param>
        /// <returns>The total number of columns that child content spans within a <see cref="Grid"/>.</returns>
        public static int GetColumnSpan(Control control)
        {
            if (control is null)
                throw new ArgumentNullException(nameof(control));

            /*return controlColumnSpans.TryGetValue(control, out var value) ? value : 1;*/

            if (control.HasExtendedProps)
                return control.ExtendedProps.GridColumnSpan;
            else
                return 1;
        }

        /// <summary>
        /// Sets a value that indicates the total number of columns that child content spans within a <see cref="Grid"/>.
        /// </summary>
        /// <param name="control">The control for which to set the column span.</param>
        /// <param name="value">The total number of columns that child content spans within a <see cref="Grid"/>.</param>
        public static void SetColumnSpan(Control control, int value)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));

            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));

            /*controlColumnSpans[control] = value;*/

            control.ExtendedProps.GridColumnSpan = value;
            OnCellAttachedPropertyChanged(control);
        }

        // public static bool GetIsSharedSizeScope(Control control)
        // {
        //    if (control is null)
        //    {
        //        throw new ArgumentNullException(nameof(control));
        //    }

        // return controlIsSharedSizeScopes.TryGetValue(control, out var value) ? value : false;
        // }

        // public static void SetIsSharedSizeScope(Control control, bool value)
        // {
        //    if (control == null)
        //    {
        //        throw new ArgumentNullException(nameof(control));
        //    }

        // controlIsSharedSizeScopes[control] = value;
        //    DefinitionBase.OnIsSharedSizeScopePropertyChanged(control, value);
        // }
    }
}
