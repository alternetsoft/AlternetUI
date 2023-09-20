using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Defines properties and methods for use in the <see cref="UIDialogListEditWindow"/>
    /// collection editor.
    /// </summary>
    public interface IListEditSource
    {
        /// <summary>
        /// Gets or sets whether collection editor is allowed to add children to the root items.
        /// </summary>
        bool AllowSubItems { get; }

        /// <summary>
        /// Gets or sets whether collection editor is allowed to add items.
        /// </summary>
        bool AllowAdd { get; }

        /// <summary>
        /// Gets or sets whether collection editor is allowed to delete items.
        /// </summary>
        bool AllowDelete { get; }

        /// <summary>
        /// Gets or sets whether collection editor is allowed to apply data back to the property.
        /// </summary>
        bool AllowApplyData { get; }

        /// <summary>
        /// Gets or sets object which property will be edited with the collection editor.
        /// </summary>
        object? Instance { get; set; }

        /// <summary>
        /// Gets or sets property will be edited with the collection editor.
        /// </summary>
        public PropertyInfo? PropInfo { get; set; }

        /// <summary>
        /// Enumerates root items.
        /// </summary>
        IEnumerable? RootItems { get; }

        /// <summary>
        /// Enumerates children items of the specific item.
        /// </summary>
        /// <param name="item">Item id.</param>
        IEnumerable? GetChildren(object item);

        /// <summary>
        /// Creates item clone.
        /// </summary>
        /// <param name="item">Item id.</param>
        /// <returns>Cloned item.</returns>
        object CloneItem(object item);

        /// <summary>
        /// Gets title of the item.
        /// </summary>
        /// <param name="item">Item id.</param>
        string? GetItemTitle(object item);

        /// <summary>
        /// Gets object which will be shown in the <see cref="PropertyGrid"/>.
        /// </summary>
        /// <param name="item">Item id.</param>
        object? GetProperties(object item);

        /// <summary>
        /// Gets <see cref="ImageList"/> which contains item images.
        /// </summary>
        ImageList? ImageList { get; }

        /// <summary>
        /// Gets index of the image associated with the item.
        /// </summary>
        /// <param name="item">Item id.</param>
        /// <remarks>If item has no image, returns <c>null</c>.</remarks>
        int? GetItemImageIndex(object item);

        /// <summary>
        /// Creates new item.
        /// </summary>
        object? CreateNewItem();

        /// <summary>
        /// Applies data back to the property specified with <see cref="Instance"/>
        /// and <see cref="PropInfo"/>.
        /// </summary>
        /// <param name="tree">Data source with edited collection.</param>
        void ApplyData(IEnumerableTree tree);
    }
}
