using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Item of the <see cref="PropertyGrid"/>.
    /// </summary>
    public interface IPropertyGridItem : IPropInfoAndInstance, IBaseControlItem
    {
        /// <summary>
        /// Occurs when property value has been changed.
        /// </summary>
        event EventHandler? PropertyChanged;

        /// <summary>
        /// Occurs when button is clicked in the property editor.
        /// </summary>
        event EventHandler? ButtonClick;

        /// <summary>
        /// Gets whether property editor can have ellipsis button
        /// </summary>
        bool CanHaveCustomEllipsis { get; }

        /// <summary>
        /// Gets <see cref="IPropertyGridNewItemParams"/> used when this item was created.
        /// </summary>
        IPropertyGridNewItemParams? Params { get; }

        /// <summary>
        /// Gets <see cref="PropertyGrid"/> instance which owns this property.
        /// </summary>
        IPropertyGrid Owner { get; }

        /// <summary>
        /// Gets or sets user data associated with this <see cref="IPropertyGridItem"/>.
        /// </summary>
        object? UserData { get; set; }

        /// <summary>
        /// Gets <see cref="IPropertyGridChoices"/> used in the item editor.
        /// </summary>
        /// <remarks>
        /// This property can be null. It is assigned for Enum and Flags item.
        /// </remarks>
        IPropertyGridChoices? Choices { get; }

        /// <summary>
        /// Gets list of children properties.
        /// </summary>
        /// <remarks>
        /// You can change list of children properties before item is
        /// added to <see cref="PropertyGrid"/>.
        /// </remarks>
        IList<IPropertyGridItem> Children { get; }

        /// <summary>
        /// Gets parent property.
        /// </summary>
        IPropertyGridItem? Parent { get; }

        /// <summary>
        /// Gets whether <see cref="Children"/> has items.
        /// </summary>
        bool HasChildren { get; }

        /// <summary>
        /// Gets or sets function used when property is reloaded.
        /// </summary>
        /// <remarks>
        /// If it is not specified, default mechanism is used for reload.
        /// </remarks>
        Func<IPropertyGridItem?, object, PropertyInfo, object?>? GetValueFuncForReload { get; set; }

        /// <summary>
        /// Gets type of the property editor.
        /// </summary>
        PropertyGridEditKindAll PropertyEditorKind { get; set; }

        /// <summary>
        /// Gets whether property editor is <see cref="PropertyGridEditKindAll.EnumFlags"/>.
        /// </summary>
        bool IsFlags { get; }

        /// <summary>
        /// Gets custom flags and attributes provider. You can store any custom data here.
        /// </summary>
        IFlagsAndAttributes FlagsAndAttributes { get; }

        /// <summary>
        /// Item handle.
        /// </summary>
        IntPtr Handle { get; }

        /// <summary>
        /// Item name.
        /// </summary>
        string DefaultName { get; }

        /// <summary>
        /// Item label.
        /// </summary>
        string DefaultLabel { get; }

        /// <summary>
        /// Item default value.
        /// </summary>
        object? DefaultValue { get; }

        /// <summary>
        /// Item is category.
        /// </summary>
        bool IsCategory { get; }

        /// <summary>
        /// Raises <see cref="PropertyChanged"/> event.
        /// </summary>
        void RaisePropertyChanged();

        /// <summary>
        /// Raises <see cref="ButtonClick"/> event.
        /// </summary>
        void RaiseButtonClick();

        /// <summary>
        /// Adds list of <see cref="IPropertyGridItem"/> to <see cref="Children"/>.
        /// </summary>
        /// <param name="children"></param>
        void AddChildren(IEnumerable<IPropertyGridItem> children);
    }
}
