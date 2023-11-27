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
        /// Adds simple action for the specified <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type for which action is registered.</typeparam>
        /// <param name="name">Action name.</param>
        /// <param name="action">Action.</param>
        /// <returns><see cref="IPropertyGridTypeRegistry"/> of the specified
        /// <typeparamref name="T"/> type so you can chain calls and perform other actions
        /// on it.</returns>
        public static IPropertyGridTypeRegistry AddSimpleAction<T>(string name, Action action)
        {
            var registry = PropertyGrid.GetTypeRegistry(typeof(T));
            registry.AddSimpleAction(name, action);
            return registry;
        }

        /// <summary>
        /// Gets list of simple actions or <c>null</c> if there are no actions.
        /// </summary>
        /// <param name="t">Type for which actions are requested.</param>
        /// <returns></returns>
        public static IEnumerable<(string, Action)>? GetSimpleActions(Type t)
        {
            var registry = PropertyGrid.GetTypeRegistryOrNull(t);
            return registry?.GetSimpleActions();
        }

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