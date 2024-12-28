using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class PropertyGrid
    {
        /// <summary>
        /// Gets property value used in the event handler as <see cref="IPropertyGridVariant"/>.
        /// </summary>
        [Browsable(false)]
        public virtual IPropertyGridVariant EventPropValueAsVariant
        {
            get
            {
                return Handler.EventPropValueAsVariant;
            }
        }

        /// <summary>
        /// Gets or sets validation failure behavior flags used in the event handler.
        /// </summary>
        [Browsable(false)]
        public virtual PropertyGridValidationFailure EventValidationFailureBehavior
        {
            get
            {
                return Handler.EventValidationFailureBehavior;
            }

            set
            {
                Handler.EventValidationFailureBehavior = value;
            }
        }

        /// <inheritdoc cref="PropertyGridColors.CaptionBackgroundColor"/>
        public Color CaptionBackgroundColor
        {
            get
            {
                return Handler.GetCaptionBackgroundColor();
            }

            set
            {
                Handler.SetCaptionBackgroundColor(value);
            }
        }

        /// <inheritdoc cref="PropertyGridColors.CaptionForegroundColor"/>
        public Color CaptionForegroundColor
        {
            get
            {
                return Handler.GetCaptionForegroundColor();
            }

            set
            {
                Handler.SetCaptionTextColor(value);
            }
        }

        /// <inheritdoc cref="PropertyGridColors.SelectionForegroundColor"/>
        public Color SelectionForegroundColor
        {
            get
            {
                return Handler.GetSelectionForegroundColor();
            }

            set
            {
                Handler.SetSelectionTextColor(value);
            }
        }

        /// <inheritdoc cref="PropertyGridColors.CellBackgroundColor"/>
        public Color CellBackgroundColor
        {
            get
            {
                return Handler.GetCellBackgroundColor();
            }

            set
            {
                Handler.SetCellBackgroundColor(value);
            }
        }

        /// <inheritdoc cref="PropertyGridColors.CellDisabledTextColor"/>
        public Color CellDisabledTextColor
        {
            get
            {
                return Handler.GetCellDisabledTextColor();
            }

            set
            {
                Handler.SetCellDisabledTextColor(value);
            }
        }

        /// <inheritdoc cref="PropertyGridColors.CellTextColor"/>
        public Color CellTextColor
        {
            get
            {
                return Handler.GetCellTextColor();
            }

            set
            {
                Handler.SetCellTextColor(value);
            }
        }

        /// <inheritdoc cref="PropertyGridColors.EmptySpaceColor"/>
        public Color EmptySpaceColor
        {
            get
            {
                return Handler.GetEmptySpaceColor();
            }

            set
            {
                Handler.SetEmptySpaceColor(value);
            }
        }

        /// <inheritdoc cref="PropertyGridColors.LineColor"/>
        public Color LineColor
        {
            get
            {
                return Handler.GetLineColor();
            }

            set
            {
                Handler.SetLineColor(value);
            }
        }

        /// <inheritdoc cref="PropertyGridColors.MarginColor"/>
        public Color MarginColor
        {
            get
            {
                return Handler.GetMarginColor();
            }

            set
            {
                Handler.SetMarginColor(value);
            }
        }

        /// <inheritdoc cref="PropertyGridColors.SelectionBackgroundColor"/>
        public Color SelectionBackgroundColor
        {
            get
            {
                return Handler.GetSelectionBackgroundColor();
            }

            set
            {
                Handler.SetSelectionBackgroundColor(value);
            }
        }

        /// <summary>
        /// Gets column index on which event is fired.
        /// </summary>
        /// <remarks>
        /// Use it in the event handlers.
        /// </remarks>
        [Browsable(false)]
        public int EventColumn
        {
            get
            {
                return Handler.EventColumn;
            }
        }

        /// <summary>
        /// Gets property used in the event handler.
        /// </summary>
        [Browsable(false)]
        public IPropertyGridItem? EventProperty
        {
            get
            {
                return Handler.EventProperty;
            }
        }

        /// <summary>
        /// Gets property name used in the event handler.
        /// </summary>
        [Browsable(false)]
        public string EventPropName
        {
            get
            {
                return Handler.EventPropertyName;
            }
        }

        /// <summary>
        /// Gets or sets validation failure message used in the event handler.
        /// </summary>
        [Browsable(false)]
        public virtual string EventValidationFailureMessage
        {
            get
            {
                return Handler.EventValidationFailureMessage;
            }

            set
            {
                Handler.EventValidationFailureMessage = value;
            }
        }

        /// <summary>
        /// Defines visual style and behavior of the <see cref="PropertyGrid"/> control.
        /// </summary>
        /// <remarks>
        /// When this property is changed, control is recreated.
        /// </remarks>
        public PropertyGridCreateStyle CreateStyle
        {
            get
            {
                return Handler.CreateStyle;
            }

            set
            {
                Handler.CreateStyle = value;
            }
        }

        /// <summary>
        /// Defines extended style of the <see cref="PropertyGrid"/> control.
        /// </summary>
        /// <remarks>
        /// When this property is changed, control is recreated.
        /// </remarks>
        public PropertyGridCreateStyleEx CreateStyleEx
        {
            get
            {
                return Handler.CreateStyleEx;
            }

            set
            {
                Handler.CreateStyleEx = value;
            }
        }

        /// <summary>
        /// Returns <see cref="IPropertyGridFactory"/> instance.
        /// </summary>
        [Browsable(false)]
        public IPropertyGridFactory Factory => DefaultFactory;

        /// <summary>
        /// Gets or sets a value indicating whether the control has a border.
        /// </summary>
        public bool HasBorder
        {
            get
            {
                return Handler.HasBorder;
            }

            set
            {
                Handler.HasBorder = value;
            }
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.PropertyGrid;

        /// <summary>
        /// Gets control handler.
        /// </summary>
        [Browsable(false)]
        public new IPropertyGridHandler Handler
        {
            get
            {
                CheckDisposed();
                return (IPropertyGridHandler)base.Handler;
            }
        }

        /// <summary>
        /// Creates new <see cref="IPropertyGridVariant"/> instance.
        /// </summary>
        public static IPropertyGridVariant CreateVariant()
        {
            return ControlFactory.Handler.CreateVariant();
        }

        /// <summary>
        /// Checks system screen design used for laying out various dialogs.
        /// </summary>
        public bool IsSmallScreen()
        {
            return Handler.IsSmallScreen();
        }

        /// <summary>
        /// Creates new variant instance for use with <see cref="PropertyGrid"/>
        /// </summary>
        public virtual IPropertyGridVariant CreateVar()
        {
            return Handler.CreateVariant();
        }

        /// <summary>
        /// Enables or disables automatic translation for enum list labels and
        /// flags child property labels.
        /// </summary>
        /// <param name="enable"><c>true</c> enables automatic translation, <c>false</c>
        /// disables it.</param>
        public void AutoGetTranslation(bool enable)
        {
            Handler.AutoGetTranslation(enable);
        }

        /// <summary>
        /// Registers all type handlers for use in <see cref="PropertyGrid"/>.
        /// </summary>
        public void InitAllTypeHandlers()
        {
            Handler.InitAllTypeHandlers();
        }

        /// <summary>
        /// Registers additional editors for use in <see cref="PropertyGrid"/>.
        /// </summary>
        public void RegisterAdditionalEditors()
        {
            Handler.RegisterAdditionalEditors();
        }

        /// <summary>
        /// Sets string constants for <c>true</c> and <c>false</c> words
        /// used in <see cref="bool"/> properties.
        /// </summary>
        /// <param name="trueChoice"></param>
        /// <param name="falseChoice"></param>
        public void SetBoolChoices(string trueChoice, string falseChoice)
        {
            Handler.SetBoolChoices(trueChoice, falseChoice);
        }

        /// <summary>
        /// Creates <see cref="string"/> property with ellipsis button which opens
        /// <see cref="FileDialog"/> when pressed.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <param name="prm">Property item create parameters.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        /// <remarks>
        /// In order to setup filename and attached <see cref="FileDialog"/>, you can use
        /// <see cref="PropertyGrid.SetPropertyKnownAttribute"/> for
        /// <see cref="PropertyGridItemAttrId.DialogTitle"/>,
        /// <see cref="PropertyGridItemAttrId.InitialPath"/>,
        /// <see cref="PropertyGridItemAttrId.ShowFullPath"/>,
        /// <see cref="PropertyGridItemAttrId.Wildcard"/> attributes.
        /// </remarks>
        public virtual IPropertyGridItem CreateFilenameItem(
            string label,
            string? name = null,
            string? value = null,
            IPropertyGridNewItemParams? prm = null)
        {
            value ??= string.Empty;
            label = CorrectPropLabel(label, prm);
            var handle = Handler.CreateFilenameProperty(
                label,
                CorrectPropName(name),
                value!);
            var result = new PropertyGridItem(this, handle, label, name, value, prm);

            result.PropertyEditorKind = PropertyGridEditKindAll.StringFilename;
            result.CanHaveCustomEllipsis = false;
            OnPropertyCreated(result, prm);
            return result;
        }

        /// <summary>
        /// Creates <see cref="string"/> property with ellipsis button which opens
        /// <see cref="SelectDirectoryDialog"/> when pressed.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <param name="prm">Property item create parameters.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        /// <remarks>
        /// In order to setup folder name and attached <see cref="SelectDirectoryDialog"/>,
        /// you can use
        /// <see cref="PropertyGrid.SetPropertyKnownAttribute"/> for
        /// <see cref="PropertyGridItemAttrId.DialogTitle"/>,
        /// <see cref="PropertyGridItemAttrId.InitialPath"/>,
        /// <see cref="PropertyGridItemAttrId.ShowFullPath"/> attributes.
        /// </remarks>
        public virtual IPropertyGridItem CreateDirItem(
            string label,
            string? name = null,
            string? value = null,
            IPropertyGridNewItemParams? prm = null)
        {
            value ??= string.Empty;
            label = CorrectPropLabel(label, prm);
            var handle = Handler.CreateDirProperty(
                label,
                CorrectPropName(name),
                value!);
            var result = new PropertyGridItem(this, handle, label, name, value, prm);

            result.PropertyEditorKind = PropertyGridEditKindAll.StringDirectory;
            result.CanHaveCustomEllipsis = false;
            OnPropertyCreated(result, prm);
            return result;
        }

        /// <summary>
        /// Creates <see cref="string"/> property with ellipsis button which opens
        /// <see cref="FileDialog"/> when pressed.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <param name="prm">Property item create parameters.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        /// <remarks>
        /// In order to setup filename and attached <see cref="FileDialog"/>, you can use
        /// <see cref="PropertyGrid.SetPropertyKnownAttribute"/> for
        /// <see cref="PropertyGridItemAttrId.DialogTitle"/>,
        /// <see cref="PropertyGridItemAttrId.InitialPath"/>,
        /// <see cref="PropertyGridItemAttrId.ShowFullPath"/> attributes.
        /// </remarks>
        /// <remarks>
        /// This function is similar to <see cref="CreateFilenameItem"/> but wildcards
        /// are limited to supported image file extensions.
        /// </remarks>
        public virtual IPropertyGridItem CreateImageFilenameItem(
            string label,
            string? name = null,
            string? value = null,
            IPropertyGridNewItemParams? prm = null)
        {
            value ??= string.Empty;
            label = CorrectPropLabel(label, prm);
            var handle = Handler.CreateImageFilenameProperty(
                label,
                CorrectPropName(name),
                value!);
            var result = new PropertyGridItem(this, handle, label, name, value, prm);

            result.PropertyEditorKind = PropertyGridEditKindAll.StringImageFilename;
            result.CanHaveCustomEllipsis = false;
            OnPropertyCreated(result, prm);
            return result;
        }

        /// <summary>
        /// Creates <see cref="string"/> property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <param name="prm">Property item create parameters.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateStringItem(
            string label,
            string? name = null,
            string? value = null,
            IPropertyGridNewItemParams? prm = null)
        {
            value ??= string.Empty;
            label = CorrectPropLabel(label, prm);
            var handle = Handler.CreateStringProperty(
                label,
                CorrectPropName(name),
                value!);
            var result = new PropertyGridItem(this, handle, label, name, value, prm);

            result.PropertyEditorKind = PropertyGridEditKindAll.String;
            OnPropertyCreated(result, prm);
            return result;
        }

        /// <summary>
        /// Creates <see cref="bool"/> property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <param name="prm">Property item create parameters.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateBoolItem(
            string label,
            string? name = null,
            bool value = false,
            IPropertyGridNewItemParams? prm = null)
        {
            label = CorrectPropLabel(label, prm);
            var handle = Handler.CreateBoolProperty(
                label,
                CorrectPropName(name),
                value);
            var result = new PropertyGridItem(this, handle, label, name, value, prm);

            result.PropertyEditorKind = PropertyGridEditKindAll.Bool;
            OnPropertyCreated(result, prm);
            return result;
        }

        /// <summary>
        /// Creates <see cref="long"/> property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <param name="prm">Property item create parameters.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateLongItem(
            string label,
            string? name = null,
            long value = 0,
            IPropertyGridNewItemParams? prm = null)
        {
            label = CorrectPropLabel(label, prm);
            var handle = Handler.CreateIntProperty(
                label,
                CorrectPropName(name),
                value);
            var result = new PropertyGridItem(this, handle, label, name, value, prm);

            result.PropertyEditorKind = PropertyGridEditKindAll.Int64;
            OnPropertyCreated(result, prm);
            return result;
        }

        /// <summary>
        /// Creates <see cref="double"/> property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <param name="prm">Property item create parameters.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateDoubleItem(
            string label,
            string? name = null,
            double value = default,
            IPropertyGridNewItemParams? prm = null)
        {
            label = CorrectPropLabel(label, prm);
            var handle = Handler.CreateFloatProperty(
                label,
                CorrectPropName(name),
                value);
            var result = new PropertyGridItem(this, handle, label, name, value, prm);

            result.PropertyEditorKind = PropertyGridEditKindAll.Double;
            SetPropertyMinMax(result, TypeCode.Double);
            OnPropertyCreated(result, prm);
            return result;
        }

        /// <summary>
        /// Creates <see cref="float"/> property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <param name="prm">Property item create parameters.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateFloatItem(
            string label,
            string? name = null,
            float value = default,
            IPropertyGridNewItemParams? prm = null)
        {
            label = CorrectPropLabel(label, prm);
            var handle = Handler.CreateFloatProperty(
                label,
                CorrectPropName(name),
                value);
            var result = new PropertyGridItem(this, handle, label, name, value, prm);

            result.PropertyEditorKind = PropertyGridEditKindAll.Single;
            SetPropertyMinMax(result, TypeCode.Single);
            OnPropertyCreated(result, prm);
            return result;
        }

        /// <summary>
        /// Creates <see cref="Color"/> property with system colors.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <param name="prm">Property item create parameters.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateSystemColorItem(
            string label,
            string? name,
            Color value,
            IPropertyGridNewItemParams? prm = null)
        {
            label = CorrectPropLabel(label, prm);
            var handle = Handler.CreateSystemColorProperty(
                label,
                CorrectPropName(name),
                value);
            var result = new PropertyGridItem(this, handle, label, name, value, prm);

            result.PropertyEditorKind = PropertyGridEditKindAll.ColorSystem;
            result.CanHaveCustomEllipsis = false;
            OnPropertyCreated(result, prm);
            return result;
        }

        /// <summary>
        /// Creates <see cref="Color"/> property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <param name="prm">Property item create parameters.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateColorItem(
            string label,
            string? name,
            Color value,
            IPropertyGridNewItemParams? prm = null)
        {
            label = CorrectPropLabel(label, prm);
            var handle = Handler.CreateColorProperty(
                label,
                CorrectPropName(name),
                value);
            var result = new PropertyGridItem(this, handle, label, name, value, prm);

            result.PropertyEditorKind = PropertyGridEditKindAll.Color;
            result.CanHaveCustomEllipsis = false;
            OnPropertyCreated(result, prm);
            return result;
        }

        /// <summary>
        /// Creates <see cref="ulong"/> property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <param name="prm">Property item create parameters.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateULongItem(
            string label,
            string? name = null,
            ulong value = 0,
            IPropertyGridNewItemParams? prm = null)
        {
            label = CorrectPropLabel(label, prm);
            var handle = Handler.CreateUIntProperty(
                label,
                CorrectPropName(name),
                value);
            var result = new PropertyGridItem(this, handle, label, name, value, prm);

            result.PropertyEditorKind = PropertyGridEditKindAll.UInt64;
            OnPropertyCreated(result, prm);
            return result;
        }

        /// <summary>
        /// Creates <see cref="string"/> property with additional edit dialog for
        /// entering long values.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <param name="prm">Property item create parameters.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateLongStringItem(
            string label,
            string? name = null,
            string? value = null,
            IPropertyGridNewItemParams? prm = null)
        {
            value ??= string.Empty;
            label = CorrectPropLabel(label, prm);
            var handle = Handler.CreateLongStringProperty(
                label,
                CorrectPropName(name),
                value);
            var result = new PropertyGridItem(this, handle, label, name, value, prm);

            result.PropertyEditorKind = PropertyGridEditKindAll.StringLong;
            result.CanHaveCustomEllipsis = false;
            OnPropertyCreated(result, prm);
            return result;
        }

        /// <summary>
        /// Creates <see cref="DateTime"/> property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <param name="prm">Property item create parameters.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateDateItem(
            string label,
            string? name = null,
            DateTime? value = null,
            IPropertyGridNewItemParams? prm = null)
        {
            DateTime dt;

            if (value == null)
                dt = DateTime.Now;
            else
                dt = value.Value;

            label = CorrectPropLabel(label, prm);
            var handle = Handler.CreateDateProperty(
                label,
                CorrectPropName(name),
                dt);
            var result = new PropertyGridItem(this, handle, label, name, value, prm);

            result.PropertyEditorKind = PropertyGridEditKindAll.Date;
            result.CanHaveCustomEllipsis = false;
            OnPropertyCreated(result, prm);
            return result;
        }

        /// <summary>
        /// Creates enumeration property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="choices">Enumeration elements.</param>
        /// <param name="value">Default property value.</param>
        /// <param name="prm">Property item create parameters.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateChoicesItem(
            string label,
            string? name,
            IPropertyGridChoices choices,
            object? value = null,
            IPropertyGridNewItemParams? prm = null)
        {
            value ??= 0;
            label = CorrectPropLabel(label, prm);
            var handle = Handler.CreateEnumProperty(
                label,
                CorrectPropName(name),
                choices,
                (int)value);
            var result = new PropertyGridItem(this, handle, label, name, value, prm);

            result.PropertyEditorKind = PropertyGridEditKindAll.Enum;
            result.Choices = choices;
            OnPropertyCreated(result, prm);
            return result;
        }

        /// <summary>
        /// Creates editable enumeration property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="choices">Enumeration elements.</param>
        /// <param name="value">Default property value.</param>
        /// <param name="prm">Property item create parameters.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateEditEnumItem(
            string label,
            string? name,
            IPropertyGridChoices choices,
            string? value = null,
            IPropertyGridNewItemParams? prm = null)
        {
            value ??= string.Empty;
            label = CorrectPropLabel(label, prm);
            var handle = Handler.CreateEditEnumProperty(
                label,
                CorrectPropName(name),
                choices,
                value);
            var result = new PropertyGridItem(this, handle, label, name, value, prm);

            result.PropertyEditorKind = PropertyGridEditKindAll.EnumEditable;
            result.Choices = choices;
            OnPropertyCreated(result, prm);
            return result;
        }

        /// <summary>
        /// Creates flags property (like enumeration with Flags attribute).
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="choices">Elements.</param>
        /// <param name="value">Default property value.</param>
        /// <param name="prm">Property item create parameters.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateFlagsItem(
            string label,
            string? name,
            IPropertyGridChoices choices,
            object? value = null,
            IPropertyGridNewItemParams? prm = null)
        {
            value ??= 0;
            label = CorrectPropLabel(label, prm);
            var handle = Handler.CreateFlagsProperty(
                label,
                CorrectPropName(name),
                choices,
                (int)value);
            var result = new PropertyGridItem(this, handle, label, name, value, prm);

            result.PropertyEditorKind = PropertyGridEditKindAll.EnumFlags;
            result.Choices = choices;
            OnPropertyCreated(result, prm);
            return result;
        }

        /// <summary>
        /// Deletes all items from the property grid.
        /// </summary>
        public virtual void Clear()
        {
            items.Clear();
            Handler.Clear();
        }

        /// <summary>
        /// Adds item to the property grid.
        /// </summary>
        /// <param name="prop">Item to add.</param>
        /// <param name="parent">Parent item or null.</param>
        public virtual void Add(IPropertyGridItem? prop, IPropertyGridItem? parent = null)
        {
            if (prop == null)
                return;
            SetAsCheckBox(prop);

            if (parent == null)
                Handler.Append(prop);
            else
                Handler.AppendIn(parent, prop);

            items.Add(prop.Handle, prop);
            if (prop.HasChildren)
            {
                foreach (IPropertyGridItem child in prop.Children)
                {
                    Add(child, prop);
                }

                Collapse(prop);
            }

            void SetAsCheckBox(IPropertyGridItem p)
            {
                var kind = p.PropertyEditorKind;
                var kindIsOk = kind == PropertyGridEditKindAll.Bool || p.IsFlags;

                if (BoolAsCheckBox && kindIsOk)
                {
                    SetPropertyKnownAttribute(p, PropertyGridItemAttrId.UseCheckbox, true);
                }
            }
        }

        /// <summary>
        /// Creates properties category.
        /// </summary>
        /// <param name="label">Category label.</param>
        /// <param name="name">Category name.</param>
        /// <param name="prm">Item create params.</param>
        /// <returns>Category property for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreatePropCategory(
            string label,
            string? name = null,
            IPropertyGridNewItemParams? prm = null)
        {
            label = CorrectPropLabel(label, prm);
            var handle = Handler.CreatePropCategory(
                label,
                CorrectPropName(name));
            var result = new PropertyGridItem(this, handle, label, name, null, prm);

            result.IsCategory = true;
            OnPropertyCreated(result, prm);
            return result;
        }

        /// <summary>
        /// Gets property name of the <see cref="IPropertyGridItem"/>.
        /// </summary>
        /// <param name="property">Property Item.</param>
        public virtual string GetPropertyName(IPropertyGridItem property)
        {
            return Handler.GetPropertyName(property);
        }

        /// <summary>
        /// Sorts properties.
        /// </summary>
        /// <param name="topLevelOnly"><c>true</c> to sort only top level properties,
        /// <c>false</c> otherwise.</param>
        public virtual void Sort(bool topLevelOnly = false)
        {
            var flags = topLevelOnly ? PropertyGridItemValueFlags.SortTopLevelOnly : 0;
            Handler.Sort(flags);
        }

        /// <summary>
        /// Sets property readonly flag.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="isSet">New Readonly flag value.</param>
        /// <param name="recurse"><c>true</c> to change readonly flag recursively
        /// for child propereties, <c>false</c> otherwise.</param>
        public virtual void SetPropertyReadOnly(
            IPropertyGridItem prop,
            bool isSet,
            bool recurse = true)
        {
            var flags = recurse
                ? PropertyGridItemValueFlags.Recurse : PropertyGridItemValueFlags.DontRecurse;

            Handler.SetPropertyReadOnly(prop, isSet, flags);
        }

        /// <summary>
        /// Allows property value to be unspecified.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual void SetPropertyValueUnspecified(IPropertyGridItem prop)
        {
            Handler.SetPropertyValueUnspecified(prop);
        }

        /// <summary>
        /// Appends property as a child of other property.
        /// </summary>
        /// <param name="prop">Parent property item.</param>
        /// <param name="newproperty">Property item to add as a child.</param>
        /// <remarks>
        /// It is better to fill <see cref="IPropertyGridItem.Children"/> and
        /// to add property using <see cref="Add"/>.
        /// </remarks>
        public virtual void AppendIn(IPropertyGridItem prop, IPropertyGridItem newproperty)
        {
            Handler.AppendIn(prop, newproperty);
        }

        /// <summary>
        /// Collapses (hides) all sub properties of the given property.
        /// </summary>
        /// <param name="prop">Property item to collapse.</param>
        /// <returns><c>true</c> if operation was successful, <c>false</c> otherwise.</returns>
        public virtual bool Collapse(IPropertyGridItem? prop)
        {
            if (prop is null)
                return false;
            return Handler.Collapse(prop);
        }

        /// <summary>
        /// Removes property from the <see cref="PropertyGrid"/>.
        /// </summary>
        /// <param name="prop">Property item to remove.</param>
        public virtual void RemoveProperty(IPropertyGridItem? prop)
        {
            if (prop is null)
                return;
            Handler.RemoveProperty(prop);
            items.Remove(prop.Handle);
        }

        /// <summary>
        /// Disables property.
        /// </summary>
        /// <param name="prop">Property item to disable.</param>
        /// <returns><c>true</c> if operation was successful, <c>false</c> otherwise.</returns>
        public virtual bool DisableProperty(IPropertyGridItem prop)
        {
            return Handler.DisableProperty(prop);
        }

        /// <summary>
        /// Changes enabled state of the property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="enable">New enabled state value.</param>
        /// <returns><c>true</c> if operation was successful, <c>false</c> otherwise.</returns>
        public virtual bool EnableProperty(IPropertyGridItem? prop, bool enable = true)
        {
            if (prop is null)
                return false;
            return Handler.EnableProperty(prop, enable);
        }

        /// <summary>
        /// Expands (shows) all sub properties of the given property.
        /// </summary>
        /// <param name="prop">Property item to expand.</param>
        /// <returns><c>true</c> if operation was successful, <c>false</c> otherwise.</returns>
        public virtual bool Expand(IPropertyGridItem? prop)
        {
            if (prop is null)
                return false;
            return Handler.Expand(prop);
        }

        /// <summary>
        /// Gets client data associated with the property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual IntPtr GetPropertyClientData(IPropertyGridItem prop)
        {
            return Handler.GetPropertyClientData(prop);
        }

        /// <summary>
        /// Gets help string associated with the property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual string GetPropertyHelpString(IPropertyGridItem prop)
        {
            return Handler.GetPropertyHelpString(prop);
        }

        /// <summary>
        /// Gets label associated with the property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual string GetPropertyLabel(IPropertyGridItem prop)
        {
            return Handler.GetPropertyLabel(prop);
        }

        /// <summary>
        /// Gets property value as <see cref="string"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual string GetPropertyValueAsString(IPropertyGridItem prop)
        {
            return Handler.GetPropertyValueAsString(prop);
        }

        /// <summary>
        /// Gets property value as <see cref="long"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual long GetPropertyValueAsLong(IPropertyGridItem prop)
        {
            return Handler.GetPropertyValueAsLong(prop);
        }

        /// <summary>
        /// Gets property value as <see cref="ulong"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual ulong GetPropertyValueAsULong(IPropertyGridItem prop)
        {
            return Handler.GetPropertyValueAsULong(prop);
        }

        /// <summary>
        /// Gets property value as <see cref="int"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual int GetPropertyValueAsInt(IPropertyGridItem prop)
        {
            return Handler.GetPropertyValueAsInt(prop);
        }

        /// <summary>
        /// Gets property value as <see cref="IPropertyGridVariant"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual IPropertyGridVariant GetPropertyValueAsVariant(IPropertyGridItem prop)
        {
            return Handler.GetPropertyValueAsVariant(prop);
        }

        /// <summary>
        /// Gets property value as <see cref="bool"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual bool GetPropertyValueAsBool(IPropertyGridItem prop)
        {
            return Handler.GetPropertyValueAsBool(prop);
        }

        /// <summary>
        /// Gets property value as <see cref="double"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual double GetPropertyValueAsDouble(IPropertyGridItem prop)
        {
            return Handler.GetPropertyValueAsDouble(prop);
        }

        /// <summary>
        /// Gets property value as <see cref="DateTime"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual DateTime GetPropertyValueAsDateTime(IPropertyGridItem prop)
        {
            return Handler.GetPropertyValueAsDateTime(prop);
        }

        /// <summary>
        /// Hides or shows property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="hide"><c>true</c> to hide the property, <c>false</c> to show
        /// the property.</param>
        /// <param name="recurse">Perform operation recursively for the child items.</param>
        /// <returns><c>true</c> if operation was successful, <c>false</c> otherwise.</returns>
        public virtual bool HideProperty(IPropertyGridItem prop, bool hide, bool recurse = true)
        {
            var flags = recurse
                ? PropertyGridItemValueFlags.Recurse : PropertyGridItemValueFlags.DontRecurse;
            return Handler.HideProperty(prop, hide, flags);
        }

        /// <summary>
        /// Inserts property before another property.
        /// </summary>
        /// <param name="priorThis">Property item before which other property
        /// will be inserted.</param>
        /// <param name="newproperty">Property item to insert.</param>
        public virtual void Insert(IPropertyGridItem priorThis, IPropertyGridItem newproperty)
        {
            Handler.Insert(priorThis, newproperty);
        }

        /// <summary>
        /// Inserts property in parent property at specified index.
        /// </summary>
        /// <param name="parent">Parent property item.</param>
        /// <param name="index">Insert position.</param>
        /// <param name="newproperty">Property item to insert.</param>
        public virtual void InsertAt(
            IPropertyGridItem parent,
            int index,
            IPropertyGridItem newproperty)
        {
            Handler.InsertByIndex(parent, index, newproperty);
        }

        /// <summary>
        /// Gets whether property is category.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <returns><c>true</c> if property is category, <c>false</c> otherwise.</returns>
        public virtual bool IsPropertyCategory(IPropertyGridItem prop)
        {
            return Handler.IsPropertyCategory(prop);
        }

        /// <summary>
        /// Gets whether property is enabled.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <returns><c>true</c> if property is enabled, <c>false</c> otherwise.</returns>
        public virtual bool IsPropertyEnabled(IPropertyGridItem prop)
        {
            return Handler.IsPropertyEnabled(prop);
        }

        /// <summary>
        /// Gets whether property is expanded.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <returns><c>true</c> if property is expanded, <c>false</c> otherwise.</returns>
        public virtual bool IsPropertyExpanded(IPropertyGridItem prop)
        {
            return Handler.IsPropertyExpanded(prop);
        }

        /// <summary>
        /// Gets whether property is modified.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <returns><c>true</c> if property is modified, <c>false</c> otherwise.</returns>
        public virtual bool IsPropertyModified(IPropertyGridItem prop)
        {
            return Handler.IsPropertyModified(prop);
        }

        /// <summary>
        /// Gets whether property is selected.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <returns><c>true</c> if property is selected, <c>false</c> otherwise.</returns>
        public virtual bool IsPropertySelected(IPropertyGridItem prop)
        {
            return Handler.IsPropertySelected(prop);
        }

        /// <summary>
        /// Gets whether property is shown.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <returns><c>true</c> if property is shown, <c>false</c> otherwise.</returns>
        public virtual bool IsPropertyShown(IPropertyGridItem prop)
        {
            return Handler.IsPropertyShown(prop);
        }

        /// <summary>
        /// Gets whether property value is unspecified.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <returns><c>true</c> if property value is unspecified, <c>false</c> otherwise.</returns>
        public virtual bool IsPropertyValueUnspecified(IPropertyGridItem prop)
        {
            return Handler.IsPropertyValueUnspecified(prop);
        }

        /// <summary>
        /// Disables (limit = true) or enables (limit = false) text editor of a property,
        /// if it is not the sole mean to edit the value.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="limit"><c>true</c> to disable text editor, <c>false</c> otherwise.</param>
        public virtual void LimitPropertyEditing(IPropertyGridItem prop, bool limit = true)
        {
            Handler.LimitPropertyEditing(prop, limit);
        }

        /// <summary>
        /// Replaces existing property with newly created property.
        /// </summary>
        /// <param name="prop">Property item to be replaced.</param>
        /// <param name="newProp">New property item.</param>
        public virtual void ReplaceProperty(IPropertyGridItem prop, IPropertyGridItem newProp)
        {
            Handler.ReplaceProperty(prop, newProp);
        }

        /// <summary>
        /// Sets background colour of a property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="color">New background color.</param>
        /// <param name="recurse"><c>true</c> causes color to be set recursively,
        /// <c>false</c> only sets color for the property in question and not
        /// any of its children.</param>
        public virtual void SetPropertyBackgroundColor(
            IPropertyGridItem prop,
            Color color,
            bool recurse = true)
        {
            var flags = recurse
                ? PropertyGridItemValueFlags.Recurse : PropertyGridItemValueFlags.DontRecurse;
            Handler.SetPropertyBackgroundColor(prop, color, flags);
        }

        /// <summary>
        /// Resets text and background colors of a property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="recurse"><c>true</c> causes color to be reset recursively,
        /// <c>false</c> only resets color for the property in question and not
        /// any of its children.</param>
        public virtual void SetPropertyColorsToDefault(IPropertyGridItem prop, bool recurse = true)
        {
            var flags = recurse
                ? PropertyGridItemValueFlags.Recurse : PropertyGridItemValueFlags.DontRecurse;
            Handler.SetPropertyColorsToDefault(prop, flags);
        }

        /// <summary>
        /// Sets text colour of a property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="color">New text color.</param>
        /// <param name="recurse"><c>true</c> causes color to be set recursively,
        /// <c>false</c> only sets color for the property in question and not
        /// any of its children.</param>
        public virtual void SetPropertyTextColor(
            IPropertyGridItem prop,
            Color color,
            bool recurse = true)
        {
            var flags = recurse
                ? PropertyGridItemValueFlags.Recurse : PropertyGridItemValueFlags.DontRecurse;
            Handler.SetPropertyTextColor(prop, color, flags);
        }

        /// <summary>
        /// Restores user-editable state.
        /// </summary>
        /// <param name="src">String generated by <see cref="SaveEditableState"/>.</param>
        /// <param name="restoreStates">Which parts to restore from source string.</param>
        /// <returns><c>true</c> if there were no problems during reading the state,
        /// <c>false</c> otherwise.</returns>
        /// <remarks>
        /// If some parts of state (such as scrolled or splitter position) fail to restore
        /// correctly, please make sure that you call this function after
        /// <see cref="PropertyGrid"/> size has been set.
        /// </remarks>
        public virtual bool RestoreEditableState(
            string src,
            PropertyGridEditableState restoreStates = PropertyGridEditableState.AllStates)
        {
            return Handler.RestoreEditableState(src, restoreStates);
        }

        /// <summary>
        /// Redraws given property.
        /// </summary>
        /// <param name="p">Property item.</param>
        /// <remarks>
        /// This function reselects the property and may cause excess flicker.
        /// </remarks>
        public virtual void RefreshProperty(IPropertyGridItem p)
        {
            Handler.RefreshProperty(p);
        }

        /// <summary>
        /// Used to acquire user-editable state (selected property,
        /// expanded properties, scrolled position, splitter positions).
        /// </summary>
        /// <param name="includedStates">Which parts of state to include.</param>
        /// <returns></returns>
        /// <remarks>
        /// Use <see cref="RestoreEditableState"/> to read state back to <see cref="PropertyGrid"/>.
        /// </remarks>
        public virtual string SaveEditableState(
            PropertyGridEditableState includedStates =
                PropertyGridEditableState.AllStates)
        {
            return Handler.SaveEditableState(includedStates);
        }

        /// <summary>
        /// Sets proportion of an auto-stretchable column.
        /// </summary>
        /// <param name="column">Column index.</param>
        /// <param name="proportion"> Column proportion (must be 1 or higher).</param>
        /// <returns><c>true</c> on success, <c>false</c> on failure.</returns>
        /// <remarks>
        /// <see cref="PropertyGridCreateStyle.SplitterAutoCenter"/> style needs to be used
        /// to indicate that columns are auto-resizable.
        /// </remarks>
        public virtual bool SetColumnProportion(int column, int proportion)
        {
            return Handler.SetColumnProportion(column, proportion);
        }

        /// <summary>
        /// Gets auto-resize proportion of the given column.
        /// </summary>
        /// <param name="column">Column index.</param>
        public virtual int GetColumnProportion(int column)
        {
            return Handler.GetColumnProportion(column);
        }

        /// <summary>
        /// Gets background color of first cell of a property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual Color GetPropertyBackgroundColor(IPropertyGridItem prop)
        {
            return Handler.GetPropertyBackgroundColor(prop);
        }

        /// <summary>
        /// Returns text color of first cell of a property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual Color GetPropertyTextColor(IPropertyGridItem prop)
        {
            return Handler.GetPropertyTextColor(prop);
        }

        /// <summary>
        /// Sets client data of a property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="clientData">Client data associated with the property.</param>
        public virtual void SetPropertyClientData(IPropertyGridItem prop, IntPtr clientData)
        {
            Handler.SetPropertyClientData(prop, clientData);
        }

        /// <summary>
        /// Sets label of a property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="newproplabel">New property label.</param>
        /// <remarks>
        /// Properties under same parent may have same labels. However, property
        /// names must still remain unique.
        /// </remarks>
        public virtual void SetPropertyLabel(IPropertyGridItem prop, string newproplabel)
        {
            Handler.SetPropertyLabel(prop, newproplabel);
        }

        /// <summary>
        /// Sets help string of a property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="helpString">Help string associated with the property.</param>
        public virtual void SetPropertyHelpString(IPropertyGridItem prop, string helpString)
        {
            Handler.SetPropertyHelpString(prop, helpString);
        }

        /// <summary>
        /// Sets maximum length of text in property text editor.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="maxLen">Maximum number of characters of the text the user can enter
        /// in the text editor. If it is 0, the length is not limited and the text can be
        /// as long as it is supported by the underlying native text control widget.</param>
        /// <returns><c>true</c> if maximum length was set, <c>false</c> otherwise.</returns>
        public virtual bool SetPropertyMaxLength(IPropertyGridItem prop, int maxLen)
        {
            return Handler.SetPropertyMaxLength(prop, maxLen);
        }

        /// <summary>
        /// Sets property value as <see cref="long"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="value">New property value.</param>
        public virtual void SetPropertyValueAsLong(IPropertyGridItem prop, long value)
        {
            Handler.SetPropertyValueAsLong(prop, value);
        }

        /// <summary>
        /// Sets property value as <see cref="int"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="value">New property value.</param>
        public virtual void SetPropertyValueAsInt(IPropertyGridItem prop, int value)
        {
            Handler.SetPropertyValueAsInt(prop, value);
        }

        /// <summary>
        /// Sets property value as <see cref="double"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="value">New property value.</param>
        public virtual void SetPropertyValueAsDouble(IPropertyGridItem prop, double value)
        {
            Handler.SetPropertyValueAsDouble(prop, value);
        }

        /// <summary>
        /// Sets property value as <see cref="bool"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="value">New property value.</param>
        public virtual void SetPropertyValueAsBool(IPropertyGridItem prop, bool value)
        {
            Handler.SetPropertyValueAsBool(prop, value);
        }

        /// <summary>
        /// Sets property value as <see cref="string"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="value">New property value.</param>
        public virtual void SetPropertyValueAsStr(IPropertyGridItem prop, string value)
        {
            Handler.SetPropertyValueAsStr(prop, value);
        }

        /// <summary>
        /// Sets property value as <see cref="DateTime"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="value">New property value.</param>
        public virtual void SetPropertyValueAsDateTime(IPropertyGridItem prop, DateTime value)
        {
            Handler.SetPropertyValueAsDateTime(prop, value);
        }

        /// <summary>
        /// Adjusts how <see cref="PropertyGrid"/> behaves when invalid value is
        /// entered in a property.
        /// </summary>
        /// <param name="vfbFlags">Validation failure flags.</param>
        public virtual void SetValidationFailureBehavior(PropertyGridValidationFailure vfbFlags)
        {
            Handler.SetValidationFailureBehavior(vfbFlags);
        }

        /// <summary>
        /// Sorts children of a property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="recurse"><c>true</c> to perform recursive sorting, <c>false</c>
        /// otherwise.</param>
        public virtual void SortChildren(IPropertyGridItem prop, bool recurse = false)
        {
            var flags = recurse
                ? PropertyGridItemValueFlags.Recurse : PropertyGridItemValueFlags.DontRecurse;
            Handler.SortChildren(prop, flags);
        }

        /// <summary>
        /// Sets editor control of a property using its name.
        /// </summary>
        /// <param name="prop">Property Item.</param>
        /// <param name="editorName">Name of the editor.</param>
        /// <remarks>
        /// Names of built-in editors are: TextCtrl, Choice, ComboBox, CheckBox,
        /// TextCtrlAndButton, ChoiceAndButton, SpinCtrl and DatePickerCtrl.
        /// </remarks>
        public virtual void SetPropertyEditorByName(IPropertyGridItem prop, string editorName)
        {
            Handler.SetPropertyEditorByName(prop, editorName);
        }

        /// <summary>
        /// Adds new action trigger (keyboard key association).
        /// </summary>
        /// <param name="action">Action for which triggers (keyboard key associations)
        /// will be added.</param>
        /// <param name="keycode">Key code.</param>
        /// <param name="modifiers">Key mnodifiers (Ctrl, Shift, Alt) of the key.</param>
        public virtual void AddActionTrigger(
            PropertyGridKeyboardAction action,
            Key keycode,
            ModifierKeys modifiers = 0)
        {
            Handler.AddActionTrigger(action, keycode, modifiers);
        }

        /// <summary>
        /// Removes added action triggers for the given action.
        /// </summary>
        /// <param name="action">Action for which triggers (keyboard key associations) will
        /// be removed.</param>
        public virtual void ClearActionTriggers(PropertyGridKeyboardAction action)
        {
            Handler.ClearActionTriggers(action);
        }

        /// <summary>
        /// Dedicates a specific keycode to <see cref="PropertyGrid"/>. This means that
        /// such key presses will not be redirected to editor controls.
        /// </summary>
        /// <param name="keycode"></param>
        /// <remarks>
        /// Using this function allows, for example, navigation between properties using
        /// arrow keys even when the focus is in the editor control.
        /// </remarks>
        public virtual void DedicateKey(Key keycode)
        {
            Handler.DedicateKey(keycode);
        }

        /// <summary>
        /// Centers the splitter.
        /// </summary>
        /// <param name="enableAutoResizing">If <c>true</c>, automatic column resizing is
        /// enabled (only applicable if window style
        /// <see cref="PropertyGridCreateStyle.SplitterAutoCenter"/> is used).</param>
        public virtual void CenterSplitter(bool enableAutoResizing = false)
        {
            Handler.CenterSplitter(enableAutoResizing);
        }

        /// <summary>
        /// Call when editor widget's contents is modified.
        /// </summary>
        /// <remarks>
        /// For example, this is called when changes text in <see cref="TextBox"/>
        /// (used in string or int property).
        /// </remarks>
        public virtual void EditorsValueWasModified()
        {
            Handler.EditorsValueWasModified();
        }

        /// <summary>
        /// Reverse of <see cref="EditorsValueWasModified"/>.
        /// </summary>
        /// <remarks>
        /// This function should only be called by custom properties.
        /// </remarks>
        public virtual void EditorsValueWasNotModified()
        {
            Handler.EditorsValueWasNotModified();
        }

        /// <summary>
        /// Enables or disables (shows/hides) categories according to parameter enable.
        /// </summary>
        /// <param name="enable"></param>
        /// <returns><c>true</c> if operation successful, <c>false</c> otherwise.</returns>
        /// <remarks>
        /// This functions deselects selected property, if any. Validation failure
        /// option <see cref="PropertyGridValidationFailure.StayInProperty"/> is not
        /// respected, i.e.selection is cleared even if editor had invalid value.
        /// </remarks>
        public virtual bool EnableCategories(bool enable)
        {
            return Handler.EnableCategories(enable);
        }

        /// <summary>
        /// Reduces column sizes to minimum possible, while still retaining fully
        /// visible grid contents (labels, images).
        /// </summary>
        /// <returns>Minimum size for the grid to still display everything.</returns>
        /// <remarks>
        /// Does not work well with <see cref="PropertyGridCreateStyle.SplitterAutoCenter"/>
        /// window style. This function only works properly if grid size prior to call
        /// was already fairly large.
        /// </remarks>
        public virtual SizeD FitColumns()
        {
            return Handler.FitColumns();
        }

        /// <summary>
        /// Gets number of columns currently on grid.
        /// </summary>
        public virtual int GetColumnCount()
        {
            return Handler.GetColumnCount();
        }

        /// <summary>
        /// Gets height of highest characters of used font.
        /// </summary>
        public virtual int GetFontHeight()
        {
            return Handler.GetFontHeight();
        }

        /// <summary>
        /// Gets margin width.
        /// </summary>
        public virtual int GetMarginWidth()
        {
            return Handler.GetMarginWidth();
        }

        /// <summary>
        /// Gets height of a single grid row (in pixels).
        /// </summary>
        public virtual int GetRowHeight()
        {
            return Handler.GetRowHeight();
        }

        /// <summary>
        /// Gets current splitter x position.
        /// </summary>
        /// <param name="splitterIndex">Splitter index (starting from 0).</param>
        public virtual int GetSplitterPosition(int splitterIndex = 0)
        {
            return Handler.GetSplitterPosition(splitterIndex);
        }

        /// <summary>
        /// Gets current vertical spacing.
        /// </summary>
        public virtual int GetVerticalSpacing()
        {
            return Handler.GetVerticalSpacing();
        }

        /// <summary>
        /// Gets whether a property editor control has focus.
        /// </summary>
        /// <returns><c>true</c> if a property editor control has focus, <c>false</c>
        /// otherwise.</returns>
        public virtual bool IsEditorFocused()
        {
            return Handler.IsEditorFocused();
        }

        /// <summary>
        /// Gets whether editor's value was marked modified.
        /// </summary>
        /// <returns><c>true</c> if editor's value was marked modified, <c>false</c>
        /// otherwise.</returns>
        public virtual bool IsEditorsValueModified()
        {
            return Handler.IsEditorsValueModified();
        }

        /// <summary>
        /// Gets whether any property has been modified by the user.
        /// </summary>
        /// <returns><c>true</c> if any property has been modified by the user, <c>false</c>
        /// otherwise.</returns>
        public virtual bool IsAnyModified()
        {
            return Handler.IsAnyModified();
        }

        /// <summary>
        /// Resets all colors used in <see cref="PropertyGrid"/> to default values.
        /// </summary>
        public virtual void ResetColors()
        {
            Handler.ResetColors();
        }

        /// <summary>
        /// Resets column sizes and splitter positions, based on proportions.
        /// </summary>
        /// <param name="enableAutoResizing">If <c>true</c>, automatic column resizing
        /// is enabled (only applicable if control style
        /// <see cref="PropertyGridCreateStyle.SplitterAutoCenter"/> is used).</param>
        public virtual void ResetColumnSizes(bool enableAutoResizing = false)
        {
            Handler.ResetColumnSizes(enableAutoResizing);
        }

        /// <summary>
        /// Makes given column editable by user.
        /// </summary>
        /// <param name="column">The index of the column to make editable.</param>
        /// <param name="editable">Using <c>false</c> here will disable column
        /// from being editable.</param>
        public virtual void MakeColumnEditable(int column, bool editable = true)
        {
            Handler.MakeColumnEditable(column, editable);
        }

        /// <summary>
        /// Creates label editor for given column, for property that is currently selected.
        /// </summary>
        /// <param name="column">Which column's label to edit. Note that you should not use
        /// value 1, which is reserved for property value column.</param>
        /// <remarks>
        /// When multiple selection is enabled, this applies to all selected properties.
        /// </remarks>
        public virtual void BeginLabelEdit(int column = 0)
        {
            Handler.BeginLabelEdit(column);
        }

        /// <summary>
        /// Ends label editing, if any.
        /// </summary>
        /// <param name="commit">Use <c>true</c> (default) to store edited label text in
        /// property cell data.</param>
        public virtual void EndLabelEdit(bool commit = true)
        {
            Handler.EndLabelEdit(commit);
        }

        /// <summary>
        /// Sets number of columns (2 or more).
        /// </summary>
        /// <param name="colCount">Number of columns.</param>
        public virtual void SetColumnCount(int colCount)
        {
            Handler.SetColumnCount(colCount);
        }

        /// <summary>
        /// Sets x coordinate of the splitter.
        /// </summary>
        /// <param name="newXPos">Splitter position.</param>
        /// <param name="col">Column index.</param>
        /// <remarks>
        /// Splitter position cannot exceed grid size, and therefore setting it during
        /// form creation may fail as initial grid size is often smaller than desired
        /// splitter position, especially when advanced sizers are being used.
        /// </remarks>
        public virtual void SetSplitterPosition(int newXPos, int col = 0)
        {
            Handler.SetSplitterPosition(newXPos, col);
        }

        /// <summary>
        /// Returns (visual) text representation of the unspecified property value.
        /// </summary>
        public virtual string GetUnspecifiedValueText(PropertyGridValueFormatFlags flags = 0)
        {
            return Handler.GetUnspecifiedValueText(flags);
        }

        /// <summary>
        /// Set virtual width for this particular page.
        /// </summary>
        /// <param name="width">New virtual width.</param>
        /// <remarks>
        /// Width -1 indicates that the virtual width should be disabled.
        /// </remarks>
        public virtual void SetVirtualWidth(int width)
        {
            Handler.SetVirtualWidth(width);
        }

        /// <summary>
        /// Moves splitter as left as possible, while still allowing all labels to be shown in full.
        /// </summary>
        /// <param name="privateChildrenToo">If <c>false</c>, will still allow private children
        /// to be cropped.</param>
        public virtual void SetSplitterLeft(bool privateChildrenToo = false)
        {
            Handler.SetSplitterLeft(privateChildrenToo);
        }

        /// <summary>
        /// Sets vertical spacing.
        /// </summary>
        /// <param name="vspacing">Vertical spacing.</param>
        /// <remarks>
        /// Can be 1, 2, or 3 - a value relative to font height. Value of 2 should be
        /// default on most platforms.
        /// </remarks>
        /// <remarks>
        /// If <paramref name="vspacing"/> is null,
        /// <see cref="PlatformDefaults.PropertyGridVerticalSpacing"/> is used.
        /// </remarks>
        public virtual void SetVerticalSpacing(int? vspacing = null)
        {
            int v;
            if (vspacing is null)
                v = AllPlatformDefaults.PlatformCurrent.PropertyGridVerticalSpacing;
            else
                v = (int)vspacing;

            v = Math.Min(v, 3);
            v = Math.Max(v, 1);

            Handler.SetVerticalSpacing(v);
        }

        /// <summary>
        /// Gets whether control has virtual width specified with
        /// <see cref="SetVirtualWidth"/>.
        /// </summary>
        public virtual bool HasVirtualWidth()
        {
            return Handler.HasVirtualWidth();
        }

        /// <summary>
        /// Gets number of common values.
        /// </summary>
        public virtual int GetCommonValueCount()
        {
            return Handler.GetCommonValueCount();
        }

        /// <summary>
        /// Gets label of given common value.
        /// </summary>
        /// <param name="i">Index of the commo nvalue.</param>
        public virtual string GetCommonValueLabel(int i)
        {
            return Handler.GetCommonValueLabel(i);
        }

        /// <summary>
        /// Gets index of common value that will truly change value to unspecified.
        /// </summary>
        public virtual int GetUnspecifiedCommonValue()
        {
            return Handler.GetUnspecifiedCommonValue();
        }

        /// <summary>
        /// Sets index of common value that will truly change value to unspecified.
        /// </summary>
        /// <param name="index">Index of the common value.</param>
        /// <remarks>
        /// Using -1 will set none to have such effect. Default is 0.
        /// </remarks>
        public virtual void SetUnspecifiedCommonValue(int index)
        {
            Handler.SetUnspecifiedCommonValue(index);
        }

        /// <summary>
        /// Refreshes any active editor control.
        /// </summary>
        public virtual void RefreshEditor()
        {
            Handler.RefreshEditor();
        }

        /// <summary>
        /// You can use this function, for instance, to detect in events if property's
        /// SetValueInEvent was already called in editor's event handler.
        /// </summary>
        /// <remarks>
        /// It really only detects if was value was changed using property's SetValueInEvent(),
        /// which is usually used when a 'picker' dialog is displayed. If value was written by
        /// "normal means" in property's StringToValue() or IntToValue(), then this function
        /// will return false (on the other hand, property's event handler is not even
        /// called in those cases).
        /// </remarks>
        public virtual bool WasValueChangedInEvent()
        {
            return Handler.WasValueChangedInEvent();
        }

        /// <summary>
        /// Gets current Y spacing.
        /// </summary>
        public virtual int GetSpacingY()
        {
            return Handler.GetSpacingY();
        }

        /// <summary>
        /// Unfocuses or closes editor if one was open, but does not deselect property.
        /// </summary>
        public virtual bool UnfocusEditor()
        {
            return Handler.UnfocusEditor();
        }

        /// <summary>
        /// Returns last item which could be iterated using given flags.
        /// </summary>
        /// <param name="flags">Flags to limit returned properties.</param>
        /// <returns></returns>
        public virtual IPropertyGridItem? GetLastItem(PropertyGridIteratorFlags flags)
        {
            return Handler.GetLastItem(flags);
        }

        /// <summary>
        /// Returns "root property".
        /// </summary>
        /// <remarks>
        /// Root property does not have name, etc. and it is not visible.
        /// It is only useful for accessing its children.
        /// </remarks>
        public virtual IPropertyGridItem? GetRoot()
        {
            return Handler.GetRoot();
        }

        /// <summary>
        /// Gets currently selected property.
        /// </summary>
        public virtual IPropertyGridItem? GetSelectedProperty()
        {
            return Handler.GetSelectedProperty();
        }

        /// <summary>
        /// Changes value of a property, as if from an editor.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="value">Property value.</param>
        /// <remarks>
        /// Use this instead of <see cref="SetPropertyValueAsVariant"/> if you need the value to
        /// run through validation process, and also send <see cref="PropertyChanged"/>
        /// event.
        /// </remarks>
        /// <remarks>
        /// Since this function sends <see cref = "PropertyChanged"/> event, it should not
        /// be called from <see cref = "PropertyChanged"/> event handler.
        /// </remarks>
        /// <returns>
        /// <c>true</c> if value was successfully changed.
        /// </returns>
        public virtual bool ChangePropertyValue(IPropertyGridItem prop, object value)
        {
            return Handler.ChangePropertyValue(prop, Handler.ToVariant(value));
        }

        /// <summary>
        /// Changes value of a property, as if from an editor.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="value">Property value.</param>
        /// <remarks>See <see cref="ChangePropertyValue"/> for more details.</remarks>
        public virtual bool ChangePropertyValueAsVariant(
            IPropertyGridItem prop,
            IPropertyGridVariant value)
        {
            return Handler.ChangePropertyValue(prop, value);
        }

        /// <summary>
        /// Sets property value as variant.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="value">New property value.</param>
        public virtual void SetPropertyValueAsVariant(
            IPropertyGridItem prop,
            IPropertyGridVariant value)
        {
            Handler.SetPropertyValueAsVariant(prop, value);
        }

        /// <summary>
        /// Sets image associated with the property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="bmp">Image.</param>
        public virtual void SetPropertyImage(IPropertyGridItem prop, ImageSet? bmp)
        {
            Handler.SetPropertyImage(prop, bmp);
        }

        /// <summary>
        /// Sets an attribute for the property.
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="attrName">Text identifier of attribute. See
        /// <see cref="PropertyGridItemAttrId"/> for the known attribute names.</param>
        /// <param name="value">Value of attribute.</param>
        /// <param name="argFlags">Optional. Use
        /// <see cref="PropertyGridItemValueFlags.Recurse"/> to set the attribute to child
        /// properties recursively.</param>
        /// <remarks>
        /// Setting attribute's value to <c>null</c> will simply remove it from property's
        /// set of attributes.
        /// </remarks>
        /// <remarks>
        /// Property is refreshed with new settings after calling this method.
        /// </remarks>
        public virtual void SetPropertyAttribute(
            IPropertyGridItem prop,
            string attrName,
            object? value = null,
            PropertyGridItemValueFlags argFlags = 0)
        {
            Handler.SetPropertyAttribute(
                prop,
                attrName,
                Handler.ToVariant(value),
                argFlags);
        }

        /// <summary>
        /// Sets an attribute for the property.
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="attrName">Text identifier of attribute. See
        /// <see cref="PropertyGridItemAttrId"/> for the known attribute names.</param>
        /// <param name="value">Value of attribute.</param>
        /// <param name="argFlags">Optional. Use
        /// <see cref="PropertyGridItemValueFlags.Recurse"/> to set the attribute to child
        /// properties recursively.</param>
        /// <remarks>
        /// Property is refreshed with new settings after calling this method.
        /// </remarks>
        public virtual void SetPropertyAttributeAsVariant(
            IPropertyGridItem prop,
            string attrName,
            IPropertyGridVariant value,
            PropertyGridItemValueFlags argFlags = 0)
        {
            Handler.SetPropertyAttribute(
                prop,
                attrName,
                value,
                argFlags);
        }

        /// <summary>
        /// Sets an attribute for all the properties.
        /// </summary>
        /// <param name="attrName">Text identifier of attribute. See
        /// <see cref="PropertyGridItemAttrId"/> for the known attribute names.</param>
        /// <param name="value">Value of attribute.</param>
        public virtual void SetPropertyAttributeAll(string attrName, object value)
        {
            Handler.SetPropertyAttributeAll(attrName, Handler.ToVariant(value));
        }

        /// <summary>
        /// Sets an attribute for all the properties.
        /// </summary>
        /// <param name="attrName">Text identifier of attribute. See
        /// <see cref="PropertyGridItemAttrId"/> for the known attribute names.</param>
        /// <param name="value">Value of attribute.</param>
        public virtual void SetPropertyAttributeAll(string attrName, IPropertyGridVariant value)
        {
            Handler.SetPropertyAttributeAll(attrName, value);
        }

        /// <summary>
        /// Scrolls and/or expands items to ensure that the given item is visible.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <returns><c>true</c> if something was actually done.</returns>
        public virtual bool EnsureVisible(IPropertyGridItem prop)
        {
            return Handler.EnsureVisible(prop);
        }

        /// <summary>
        /// Selects a property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="focus">If <c>true</c>, move keyboard focus to the created
        /// editor right away.</param>
        /// <returns><c>true</c> if selection finished successfully. Usually only fails
        /// if current value in editor is not valid.</returns>
        /// <remarks>
        /// Editor widget is automatically created, but not focused unless focus is true.
        /// </remarks>
        /// <remarks>
        /// This function doesn't send <see cref="PropertySelected"/> event. It also clears
        /// any previous selection.
        /// </remarks>
        public virtual bool SelectProperty(IPropertyGridItem prop, bool focus = false)
        {
            if (prop == null)
                return false;
            else
                return Handler.SelectProperty(prop, focus);
        }

        /// <summary>
        /// Adds given property into selection.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <returns></returns>
        /// <remarks>
        /// Multiple selection is not supported for categories. This means that if you have
        /// properties selected, you cannot add category to selection, and also if
        /// you have category selected, you cannot add other properties to
        /// selection. This function will fail silently in these cases, even returning true.
        /// </remarks>
        /// <remarks>
        /// If <see cref="PropertyGridCreateStyleEx.MultipleSelection"/> extra style is not used,
        /// then this has same effect as calling <see cref="SelectProperty"/>.
        /// </remarks>
        public virtual bool AddToSelection(IPropertyGridItem prop)
        {
            return Handler.AddToSelection(prop);
        }

        /// <summary>
        /// Removes given property from selection.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <remarks>
        /// If property is not selected, an assertion failure will occur.
        /// </remarks>
        public virtual bool RemoveFromSelection(IPropertyGridItem prop)
        {
            return Handler.RemoveFromSelection(prop);
        }

        /// <summary>
        /// Sets the 'current' category - <see cref="Add"/> will add non-category
        /// properties under it.
        /// </summary>
        /// <param name="prop">Property category item.</param>
        public virtual void SetCurrentCategory(IPropertyGridItem prop)
        {
            Handler.SetCurrentCategory(prop);
        }

        /// <summary>
        /// Returns rectangle of custom paint image.
        /// </summary>
        /// <param name="prop">Return image rectangle for this property.</param>
        /// <param name="item">Which choice of property to use (each choice may
        /// have different image).</param>
        public virtual RectI GetImageRect(IPropertyGridItem prop, int item)
        {
            return Handler.GetImageRect(prop, item);
        }

        /// <summary>
        /// Sets flag value for the specified property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="flag">Flag to set.</param>
        /// <param name="value">New value of the flag.</param>
        public virtual void SetPropertyFlag(
            IPropertyGridItem prop,
            PropertyGridItemFlags flag,
            bool value)
        {
            Handler.SetPropertyFlag(prop, flag, value);
        }

        /// <summary>
        /// Returns size of the custom paint image in front of property.
        /// </summary>
        /// <param name="prop">Return image rectangle for this property. If this argument
        /// is <c>null</c>, then preferred size is returned.</param>
        /// <param name="item">Which choice of property to use (each choice may have
        /// different image).</param>
        public virtual SizeI GetImageSize(IPropertyGridItem? prop, int item)
        {
            return Handler.GetImageSize(prop, item);
        }

        /// <summary>
        /// Gets parent of the property item.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual IPropertyGridItem? GetPropertyParent(IPropertyGridItem? prop)
        {
            return Handler.GetPropertyParent(prop);
        }

        /// <summary>
        /// Gets first child of the property item.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual IPropertyGridItem? GetFirstChild(IPropertyGridItem? prop)
        {
            return Handler.GetFirstChild(prop);
        }

        /// <summary>
        /// Gets special property name which means to use label as property name.
        /// </summary>
        /// <returns></returns>
        public string GetPropNameAsLabel()
        {
            return Handler.GetPropNameAsLabel();
        }

        /// <summary>
        /// Gets category of the property item.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual IPropertyGridItem? GetPropertyCategory(IPropertyGridItem? prop)
        {
            return Handler.GetPropertyCategory(prop);
        }

        /// <summary>
        /// Gets first property item which satisfies search criteria specified by
        /// <paramref name="flags"/>.
        /// </summary>
        /// <param name="flags">Filter flags.</param>
        public virtual IPropertyGridItem? GetFirst(PropertyGridIteratorFlags flags)
        {
            return Handler.GetFirst(flags);
        }

        /// <summary>
        /// Gets property item with the specified <paramref name="name"/>.
        /// </summary>
        /// <param name="name">Name of the property item.</param>
        /// <returns></returns>
        public virtual IPropertyGridItem? GetProperty(string? name)
        {
            return Handler.GetProperty(name);
        }

        /// <summary>
        /// Gets property item with the specified <paramref name="label"/>.
        /// </summary>
        /// <param name="label">label of the property item.</param>
        /// <returns></returns>
        public virtual IPropertyGridItem? GetPropertyByLabel(string? label)
        {
            return Handler.GetPropertyByLabel(label);
        }

        /// <summary>
        /// Gets property item with the specified <paramref name="name"/>.
        /// </summary>
        /// <param name="name">Name of the property item.</param>
        /// <returns></returns>
        public virtual IPropertyGridItem? GetPropertyByName(string? name)
        {
            return Handler.GetPropertyByName(name);
        }

        /// <summary>
        /// Gets selected property item.
        /// </summary>
        /// <returns></returns>
        public virtual IPropertyGridItem? GetSelection()
        {
            return Handler.GetSelection();
        }

        /// <summary>
        /// Gets property item with the specified <paramref name="name"/>
        /// and <paramref name="subname"/>.
        /// </summary>
        /// <param name="name">Name of the property item.</param>
        /// <param name="subname">Subname of the property item.</param>
        /// <returns></returns>
        public virtual IPropertyGridItem? GetPropertyByNameAndSubName(string? name, string? subname)
        {
            return Handler.GetPropertyByNameAndSubName(name, subname);
        }

        /// <summary>
        /// Reloads value of the <see cref="IPropertyGridItem"/> item if it is attached
        /// to the external object (<see cref="IPropInfoAndInstance.Instance"/> and
        /// <see cref="IPropInfoAndInstance.PropInfo"/>) are not null.
        /// </summary>
        public virtual void ReloadPropertyValue(IPropertyGridItem item)
        {
            var p = item.PropInfo;
            var instance = item.Instance;
            if (instance == null || p == null)
                return;

            AvoidException(() =>
            {
                var variant = Handler.GetTempVariant();

                object? propValue;
                var reloadFunc = item.GetValueFuncForReload;
                var realInstance = GetRealInstance(item);
                realInstance ??= instance;
                if (reloadFunc == null)
                {
                    propValue = p.GetValue(realInstance);
                    variant.SetCompatibleValue(propValue, p);
                }
                else
                {
                    propValue = reloadFunc(item, realInstance, p);
                    variant.AsObject = propValue;
                }

                SetPropertyValueAsVariant(item, variant);
            });

            if (item.Parent != null)
                ReloadPropertyValue(item.Parent);
        }

        /// <summary>
        /// Creates cursor property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <param name="prm">Property item create parameters.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        internal IPropertyGridItem CreateCursorItem(
           string label,
           string? name = null,
           int value = 0,
           IPropertyGridNewItemParams? prm = null)
        {
            label = CorrectPropLabel(label, prm);
            var handle = Handler.CreateCursorProperty(
                label,
                CorrectPropName(name),
                value);
            var result = new PropertyGridItem(this, handle, label, name, value, prm);
            OnPropertyCreated(result, prm);
            return result;
        }

        internal void DeleteProperty(IPropertyGridItem prop)
        {
            Handler.DeleteProperty(prop);
            items.Remove(prop.Handle);
        }

        /// <inheritdoc/>
        protected override IControlHandler CreateHandler()
        {
            return ControlFactory.Handler.CreatePropertyGridHandler(this);
        }
    }
}
