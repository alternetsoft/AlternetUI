using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public partial class PropertyGrid
    {
        /// <summary>
        /// Clears current selection, if any.
        /// </summary>
        /// <param name="validation">If set to false, deselecting the property will
        /// always work, even if its editor had invalid value in it.</param>
        /// <returns>Returns true if successful or if there was no selection. May fail if validation
        /// was enabled and active editor had invalid value.</returns>
        public bool ClearSelection(bool validation) => NativeControl.ClearSelection(validation);

        /// <summary>
        /// Resets modified status of all properties.
        /// </summary>
        public void ClearModifiedStatus() => NativeControl.ClearModifiedStatus();

        /// <summary>
        /// Collapses all items that can be collapsed. This functions clears selection.
        /// </summary>
        public bool CollapseAll() => NativeControl.CollapseAll();

        /// <summary>
        /// Returns true if all property grid data changes have been committed.
        /// </summary>
        /// <returns>Usually only returns false if value in active editor has been invalidated
        /// by a validator.</returns>
        public bool EditorValidate() => NativeControl.EditorValidate();

        /// <summary>
        /// Expands all items that can be expanded. This functions clears selection.
        /// </summary>
        /// <param name="expand"></param>
        /// <returns></returns>
        public bool ExpandAll(bool expand) => NativeControl.ExpandAll(expand);
    }
}