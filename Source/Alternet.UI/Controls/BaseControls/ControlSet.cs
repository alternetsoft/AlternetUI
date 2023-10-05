using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Allows to perform group operations on the controls.
    /// </summary>
    public class ControlSet
    {
        private readonly IReadOnlyList<Control> items;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlSet"/> class.
        /// </summary>
        /// <param name="controls"></param>
        public ControlSet(params Control[] controls)
        {
            items = controls;
        }

        /// <summary>
        /// Gets all controls which will be affected by the group operations.
        /// </summary>
        public IReadOnlyList<Control> Items => items;

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element
        /// to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        public Control this[int index] => items[index];

        /// <summary>
        /// Creates two dimensional array 'Control[,]' from the specified columns with controls.
        /// </summary>
        /// <param name="columns">Columns with the controls</param>
        public static Control[,] GridFromColumns(params ControlSet[] columns)
        {
            var colCount = columns.Length;
            var rowCount = columns[0].Items.Count;

            var result = new Control[rowCount, colCount];

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    var column = columns[j];
                    var item = column[i];
                    result[i, j] = item;
                }
            }

            return result;
        }

        /// <summary>
        /// Sets vertical alignmnent for all the controls in the set.
        /// </summary>
        /// <param name="value">A vertical alignment setting.</param>
        public ControlSet VerticalAlignment(VerticalAlignment value)
        {
            foreach (var item in items)
                item.VerticalAlignment = value;
            return this;
        }

        /// <summary>
        /// Sets horizontal alignmnent for all the controls in the set.
        /// </summary>
        /// <param name="value">A horizontal alignment setting.</param>
        public ControlSet HorizontalAlignment(HorizontalAlignment value)
        {
            foreach (var item in items)
                item.HorizontalAlignment = value;
            return this;
        }

        /// <summary>
        /// Sets <see cref="Control.Margin"/> property for all the controls in the set.
        /// </summary>
        /// <param name="value">An oouter margin of a control.</param>
        public ControlSet Margin(Thickness value)
        {
            foreach (var item in items)
                item.Margin = value;
            return this;
        }

        /// <summary>
        /// Sets <see cref="ComboBox.IsEditable"/> property for all the controls in the set.
        /// </summary>
        /// <param name="value"><c>true</c> enables editing of the text;
        /// <c>false</c> disables it.</param>
        public ControlSet IsEditable(bool value)
        {
            foreach (var item in items)
            {
                if (item is ComboBox comboBox)
                    comboBox.IsEditable = value;
            }

            return this;
        }
    }
}
