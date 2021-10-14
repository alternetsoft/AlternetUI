using Alternet.Base.Collections;
using System;

namespace Alternet.UI
{
    /// <summary>
    /// TODO
    /// </summary>
    public class Grid : Control
    {
        public static void SetColumn(Control control, int column)
        {
            //MessageBox.Show("Column: " + column);
        }

        public Collection<ColumnDefinition> ColumnDefinitions { get; } = new Collection<ColumnDefinition>();
    }
}