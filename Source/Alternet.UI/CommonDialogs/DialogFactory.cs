using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods which call standard dialogs.
    /// </summary>
    public static class DialogFactory
    {
        /// <summary>
        /// Edits property with list editor.
        /// </summary>
        /// <param name="instance">Object which contains the property.</param>
        /// <param name="propInfo">Property information.</param>
        /// <remarks>
        /// List editor must support editing of the property.
        /// </remarks>
        /// <returns><c>null</c> if property editing is not supported; <c>true</c> if editing
        /// was performed and user pressed 'Ok' button; <c>false</c> if user pressed
        /// 'Cancel' button.</returns>
        public static bool? EditPropertyWithListEditor(object? instance, PropertyInfo? propInfo)
        {
            PropertyGrid.RegisterCollectionEditors();

            var source = ListEditSource.CreateEditSource(instance, propInfo);
            if (source == null)
                return null;

            using UIDialogListEditWindow dialog = new()
            {
                DataSource = source,
            };
            dialog.ShowModal();
            if (dialog.ModalResult == ModalResult.Accepted)
            {
                dialog.Save();
                dialog.Designer?.RaisePropertyChanged(instance, propInfo?.Name);
                return true;
            }

            dialog.Clear();
            return false;
        }

        /// <summary>
        /// Edits property with list editor.
        /// </summary>
        /// <param name="instance">Object which contains the property.</param>
        /// <param name="propName">Property name.</param>
        /// <remarks>
        /// List editor must support editing of the property.
        /// </remarks>
        /// <returns><c>null</c> if property editing is not supported; <c>true</c> if editing
        /// was performed and user pressed 'Ok' button; <c>false</c> if user pressed
        /// 'Cancel' button.</returns>
        public static bool? EditPropertyWithListEditor(object? instance, string propName)
        {
            var propInfo = AssemblyUtils.GetPropInfo(instance, propName);
            var result = EditPropertyWithListEditor(instance, propInfo);
            return result;
        }

        /// <summary>
        /// Edits <see cref="ListView.Columns"/> with list editor.
        /// </summary>
        /// <param name="control">Control which columns will be edited.</param>
        public static bool? EditColumnsWithListEditor(ListView control) =>
            EditPropertyWithListEditor(control, "Columns");

        /// <summary>
        /// Edits <see cref="ListView.Items"/> with list editor.
        /// </summary>
        /// <param name="control">Control which items will be edited.</param>
        public static bool? EditItemsWithListEditor(ListView control) =>
            EditPropertyWithListEditor(control, "Items");

        /// <summary>
        /// Edits <see cref="TreeView.Items"/> with list editor.
        /// </summary>
        /// <param name="control">Control which items will be edited.</param>
        public static bool? EditItemsWithListEditor(TreeView control) =>
            EditPropertyWithListEditor(control, "Items");

        /// <summary>
        /// Edits <see cref="ListControl.Items"/> with list editor.
        /// </summary>
        /// <param name="control">Control which items will be edited.</param>
        public static bool? EditItemsWithListEditor(ListControl control) =>
            EditPropertyWithListEditor(control, "Items");
    }
}
