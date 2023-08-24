using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    // property.h wxPGPropertyFlags

    /// <summary>
    /// Flags of the <see cref="PropertyGrid"/> items.
    /// </summary>
    public enum PropertyGridItemFlags
    {
        /// <summary>
        /// Indicates bold font.
        /// </summary>
        Modified = 0x0001,

        /// <summary>
        /// Disables ('greyed' text and editor does not activate) property.
        /// </summary>
        Disabled = 0x0002,

        /// <summary>
        /// Hider button will hide this property.
        /// </summary>
        Hidden = 0x0004,

        /// <summary>
        /// This property has custom paint image just in front of its value.
        /// If property only draws custom images into a popup list, then this
        /// flag should not be set.
        /// </summary>
        CustomImage = 0x0008,

        /// <summary>
        /// Do not create text based editor for this property (but button-triggered
        /// dialog and choice are ok).
        /// </summary>
        NoEditor = 0x0010,

        /// <summary>
        /// Property is collapsed, ie. it's children are hidden.
        /// </summary>
        Collapsed = 0x0020,

        /// <summary>
        /// If property is selected, then indicates that validation failed for pending value.
        /// If property is not selected, that indicates that the actual property
        /// value has failed validation (NB: this behaviour is not currently supported,
        /// but may be used in future).
        /// </summary>
        InvalidValue = 0x0040,

        /// <summary>
        /// Switched via SetWasModified(). Temporary flag - only used when
        /// setting/changing property value.
        /// </summary>
        WasModified = 0x0200,

        /// <summary>
        /// If set, then child properties (if any) are private, and should be
        /// "invisible" to the application.
        /// </summary>
        Aggregate = 0x0400,

        /// <summary>
        /// If set, then child properties (if any) are copies and should not
        /// be deleted in dtor.
        /// </summary>
        ChildrenAreCopies = 0x0800,

        /// <summary>
        /// Classifies this item as a non-category.
        /// Used for faster item type identification.
        /// </summary>
        Property = 0x1000,

        /// <summary>
        /// Classifies this item as a category.
        /// Used for faster item type identification.
        /// </summary>
        Category = 0x2000,

        /// <summary>
        /// Classifies this item as a property that has children,
        /// but is not aggregate (i.e. children are not private).
        /// </summary>
        MiscParent = 0x4000,

        /// <summary>
        /// Property is read-only. Editor is still created for TextBox-based
        /// property editors. For others, editor is not usually created because
        /// they do implement Readonly style or equivalent.
        /// </summary>
        ReadOnly = 0x8000,

        // NB: FLAGS ABOVE 0x8000 CANNOT BE USED WITH PROPERTY ITERATORS

        /// <summary>
        /// Property's value is composed from values of child properties.
        /// This flag cannot be used with property iterators.
        /// </summary>
        ComposedValue = 0x00010000,

        /// <summary>
        /// Common value of property is selectable in editor.
        /// This flag cannot be used with property iterators.
        /// </summary>
        UsesCommonValue = 0x00020000,

        /// <summary>
        /// Property can be set to unspecified value via editor.
        /// Currently, this applies to following properties:
        /// - IntProperty, UIntProperty, FloatProperty, EditEnumProperty:
        /// Clear the text field
        /// This flag cannot be used with property iterators.
        /// See IPropertyGridItem.SetAutoUnspecified().
        /// </summary>
        AutoUnspecified = 0x00040000,

        /// <summary>
        /// Indicates the bit usable by derived properties.
        /// </summary>
        ClassSpecific1 = 0x00080000,

        /// <summary>
        /// Indicates the bit usable by derived properties.
        /// </summary>
        ClassSpecific2 = 0x00100000,

        /// <summary>
        /// Indicates that the property is being deleted and should be ignored.
        /// </summary>
        BeingDeleted = 0x00200000,

        /// <summary>
        /// Indicates the bit usable by derived properties.
        /// </summary>
        ClassSpecific3 = 0x00400000,
    }
}
