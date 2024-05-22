using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Alternet.Base.Collections;

using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    internal class PropertyGridHandler : ControlHandler<PropertyGrid>, IPropertyGridHandler
    {
        private readonly PropertyGridVariant variant = new();

        static PropertyGridHandler()
        {
            PropertyGrid.EditWithListEdit += DialogFactory.EditWithListEdit;

            KnownColorStrings.CustomChanged += (s, e) =>
            {
                Native.PropertyGrid.KnownColorsSetCustomColorTitle(KnownColorStrings.Default.Custom);
            };
        }

        public PropertyGridHandler()
        {
        }

        /// <summary>
        /// Gets property used in the event handler.
        /// </summary>
        [Browsable(false)]
        public IPropertyGridItem? EventProperty
        {
            get
            {
                return PtrToItem(NativeControl.EventProperty);
            }
        }

        /// <summary>
        /// Gets property value used in the event handler as <see cref="IPropertyGridVariant"/>.
        /// </summary>
        [Browsable(false)]
        public virtual IPropertyGridVariant EventPropValueAsVariant
        {
            get
            {
                IntPtr handle = NativeControl.EventPropValue;
                PropertyGridVariant propValue = new(handle);
                return propValue;
            }
        }

        public new Native.PropertyGrid NativeControl =>
            (Native.PropertyGrid)base.NativeControl!;

        int IPropertyGridHandler.EventValidationFailureBehavior
        {
            get => NativeControl.EventValidationFailureBehavior;
            set => NativeControl.EventValidationFailureBehavior = value;
        }

        IPropertyGridItem? IPropertyGridHandler.EventProperty
        {
            get => PtrToItem(NativeControl.EventProperty);
        }

        string IPropertyGridHandler.EventValidationFailureMessage
        {
            get => NativeControl.EventValidationFailureMessage;
            set => NativeControl.EventValidationFailureMessage = value;
        }

        bool IPropertyGridHandler.HasBorder
        {
            get => NativeControl.HasBorder;
            set => NativeControl.HasBorder = value;
        }

        PropertyGridCreateStyle IPropertyGridHandler.CreateStyle
        {
            get => (PropertyGridCreateStyle)NativeControl.CreateStyle;
            set => NativeControl.CreateStyle = (int)value;
        }

        PropertyGridCreateStyleEx IPropertyGridHandler.CreateStyleEx
        {
            get => (PropertyGridCreateStyleEx)NativeControl.CreateStyleEx;
            set => NativeControl.CreateStyleEx = (int)value;
        }

        int IPropertyGridHandler.EventColumn
        {
            get => NativeControl.EventColumn;
        }

        string IPropertyGridHandler.EventPropertyName
        {
            get => NativeControl.EventPropertyName;
        }

        /// <summary>
        /// Creates new variant instance for use with <see cref="PropertyGrid"/>
        /// </summary>
        public IPropertyGridVariant CreateVariant()
        {
            return new PropertyGridVariant();
        }

        public PropertyGridItemHandle CreateColorProperty(string label, string name, Color value)
        {
            KnownColorsAdd();
            return CreateHandle(NativeControl.CreateColorProperty(label, name, value));
        }

        public PropertyGridItemHandle CreateHandle(IntPtr ptr)
        {
            return new WxPropertyGridItemHandle(ptr);
        }

        public PropertyGridItemHandle CreateSystemColorProperty(
            string label,
            string name,
            Color value)
        {
            KnownColorsAdd();
            uint kind = GetColorKind(value);
            return CreateHandle(NativeControl.CreateSystemColorProperty(label, name, value, kind));
        }

        /// <summary>
        /// Enables or disables automatic translation for enum list labels and
        /// flags child property labels.
        /// </summary>
        /// <param name="enable"><c>true</c> enables automatic translation, <c>false</c>
        /// disables it.</param>
        public void AutoGetTranslation(bool enable)
        {
            Native.PropertyGrid.AutoGetTranslation(enable);
        }

        /// <summary>
        /// Registers all type handlers for use in <see cref="PropertyGrid"/>.
        /// </summary>
        public void InitAllTypeHandlers()
        {
            Native.PropertyGrid.InitAllTypeHandlers();
        }

        /// <summary>
        /// Returns last item which could be iterated using given flags.
        /// </summary>
        /// <param name="flags">Flags to limit returned properties.</param>
        /// <returns></returns>
        public virtual IPropertyGridItem? GetLastItem(PropertyGridIteratorFlags flags)
        {
            return PtrToItem(NativeControl.GetLastItem((int)flags));
        }

        /// <summary>
        /// Gets category of the property item.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual IPropertyGridItem? GetPropertyCategory(IPropertyGridItem? prop)
        {
            if (prop is null)
                return null;
            var result = NativeControl.GetPropertyCategory(ItemToPtr(prop));
            return PtrToItem(result);
        }

        /// <summary>
        /// Gets first property item which satisfies search criteria specified by
        /// <paramref name="flags"/>.
        /// </summary>
        /// <param name="flags">Filter flags.</param>
        public virtual IPropertyGridItem? GetFirst(PropertyGridIteratorFlags flags)
        {
            var result = NativeControl.GetFirst((int)flags);
            return PtrToItem(result);
        }

        /// <summary>
        /// Gets parent of the property item.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual IPropertyGridItem? GetPropertyParent(IPropertyGridItem? prop)
        {
            if (prop is null)
                return null;
            var result = NativeControl.GetPropertyParent(ItemToPtr(prop));
            return PtrToItem(result);
        }

        /// <summary>
        /// Gets first child of the property item.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual IPropertyGridItem? GetFirstChild(IPropertyGridItem? prop)
        {
            if (prop is null)
                return null;
            var result = NativeControl.GetFirstChild(ItemToPtr(prop));
            return PtrToItem(result);
        }

        /// <summary>
        /// Gets selected property item.
        /// </summary>
        /// <returns></returns>
        public virtual IPropertyGridItem? GetSelection()
        {
            var result = NativeControl.GetSelection();
            return PtrToItem(result);
        }

        /// <summary>
        /// Gets property item with the specified <paramref name="name"/>.
        /// </summary>
        /// <param name="name">Name of the property item.</param>
        /// <returns></returns>
        public virtual IPropertyGridItem? GetProperty(string? name)
        {
            if (string.IsNullOrEmpty(name))
                return null;
            var result = NativeControl.GetProperty(name!);
            return PtrToItem(result);
        }

        /// <summary>
        /// Gets property item with the specified <paramref name="label"/>.
        /// </summary>
        /// <param name="label">label of the property item.</param>
        /// <returns></returns>
        public virtual IPropertyGridItem? GetPropertyByLabel(string? label)
        {
            if (string.IsNullOrEmpty(label))
                return null;
            var result = NativeControl.GetPropertyByLabel(label!);
            return PtrToItem(result);
        }

        /// <summary>
        /// Gets property item with the specified <paramref name="name"/>.
        /// </summary>
        /// <param name="name">Name of the property item.</param>
        /// <returns></returns>
        public virtual IPropertyGridItem? GetPropertyByName(string? name)
        {
            if (string.IsNullOrEmpty(name))
                return null;
            var result = NativeControl.GetPropertyByName(name!);
            return PtrToItem(result);
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
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(subname))
                return null;
            var result = NativeControl.GetPropertyByNameAndSubName(name!, subname!);
            return PtrToItem(result);
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
            return PtrToItem(NativeControl.GetRoot());
        }

        /// <summary>
        /// Gets currently selected property.
        /// </summary>
        public virtual IPropertyGridItem? GetSelectedProperty()
        {
            return PtrToItem(NativeControl.GetSelectedProperty());
        }

        public IPropertyGridVariant GetTempVariant()
        {
            return variant;
        }

        public virtual IPropertyGridItem? GetHitTestProp(PointD point)
        {
            var pointI = Control.PixelFromDip(point);
            var ptr = NativeControl.GetHitTestProp(pointI);
            var item = PtrToItem(ptr);
            return item;
        }

        public IPropertyGridVariant ToVariant(object? value)
        {
            variant.AsObject = value;
            return variant;
        }

        /// <summary>
        /// Gets property value as <see cref="IPropertyGridVariant"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual IPropertyGridVariant GetPropertyValueAsVariant(IPropertyGridItem prop)
        {
            var handle = NativeControl.GetPropertyValueAsVariant(ItemToPtr(prop));
            PropertyGridVariant propValue = new(handle);
            return propValue;
        }

        /// <summary>
        /// Sets validator of a property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="validator">Value validator.</param>
        public virtual void SetPropertyValidator(IPropertyGridItem prop, IValueValidator validator)
        {
            IntPtr ptr = default;
            if (validator != null)
                ptr = validator.Handle;
            NativeControl.SetPropertyValidator(ItemToPtr(prop), ptr);
        }

        /// <summary>
        /// Registers additional editors for use in <see cref="PropertyGrid"/>.
        /// </summary>
        public void RegisterAdditionalEditors()
        {
            Native.PropertyGrid.RegisterAdditionalEditors();
        }

        /// <summary>
        /// Sets string constants for <c>true</c> and <c>false</c> words
        /// used in <see cref="bool"/> properties.
        /// </summary>
        /// <param name="trueChoice"></param>
        /// <param name="falseChoice"></param>
        public void SetBoolChoices(string trueChoice, string falseChoice)
        {
            Native.PropertyGrid.SetBoolChoices(trueChoice, falseChoice);
        }

        /// <summary>
        /// Sets image associated with the property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="bmp">Image.</param>
        public void SetPropertyImage(IPropertyGridItem prop, ImageSet? bmp)
        {
            NativeControl.SetPropertyImage(ItemToPtr(prop), (UI.Native.ImageSet?)bmp?.Handler);
        }

        public bool IsSmallScreen()
        {
            return Native.PropertyGrid.IsSmallScreen();
        }

        internal static Color SetColorKind(Color value, uint kind)
        {
            Color Fn()
            {
                const uint PG_COLOUR_CUSTOM = 0xFFFFFF;

                if (kind == PG_COLOUR_CUSTOM)
                    return value;
                else
                {
                    KnownColor knownColor = WxColorUtils.Convert((SystemSettingsColor)kind);
                    if (knownColor == 0)
                        return value;
                    Color result = Color.FromKnownColor(knownColor);
                    return result;
                }
            }

            var result = Fn();
            if (result.IsKnownColor)
                return result;
            if (!result.IsOpaque)
                return result;
            var knownResult = KnownColorTable.ArgbToKnownColor(result.AsUInt());
            return knownResult;
        }

        internal static uint GetColorKind(Color value)
        {
            const uint PG_COLOUR_CUSTOM = 0xFFFFFF;
            uint kind = PG_COLOUR_CUSTOM;

            if (value.IsKnownColor)
            {
                KnownColor knownColor = value.ToKnownColor();
                SystemSettingsColor converted = WxColorUtils.Convert(knownColor);
                if (converted != SystemSettingsColor.Max)
                    kind = (uint)converted;
            }

            return kind;
        }

        internal static void KnownColorsClear()
        {
            Native.PropertyGrid.KnownColorsClear();
        }

        internal static void KnownColorsAdd()
        {
            if (BasePropertyGrid.StaticFlags.HasFlag(BasePropertyGrid.StaticStateFlags.KnownColorsAdded))
                return;
            BasePropertyGrid.StaticFlags |= BasePropertyGrid.StaticStateFlags.KnownColorsAdded;

            var items = ColorUtils.GetColorInfos();

            KnownColorsClear();

            foreach (var item in items)
            {
                if (!item.Visible)
                    continue;
                KnownColorsAdd(
                            item.Label,
                            item.LabelLocalized,
                            item.Value,
                            item.KnownColor);
            }

            KnownColorsApply();
        }

        internal static void KnownColorsAdd(
            string name,
            string title,
            Color value,
            KnownColor knownColor)
        {
            Native.PropertyGrid.KnownColorsAdd(name, title, value, (int)knownColor);
        }

        internal static void KnownColorsApply()
        {
            Native.PropertyGrid.KnownColorsApply();
        }

        internal static IntPtr GetEditorByName(string editorName)
        {
            return Native.PropertyGrid.GetEditorByName(editorName);
        }

        internal override Native.Control CreateNativeControl()
        {
            return new NativePropertyGrid(PropertyGrid.DefaultCreateStyle);
        }

        internal IntPtr GetPropertyValidator(IPropertyGridItem prop)
        {
            return NativeControl.GetPropertyValidator(ItemToPtr(prop));
        }

        internal bool CommitChangesFromEditor()
        {
            return NativeControl.CommitChangesFromEditor(0);
        }

        internal IntPtr GetPropertyImage(IPropertyGridItem prop)
        {
            return NativeControl.GetPropertyImage(ItemToPtr(prop));
        }

        internal void SetPropertyEditor(IPropertyGridItem prop, IntPtr editor)
        {
            NativeControl.SetPropertyEditor(ItemToPtr(prop), editor);
        }

        internal IntPtr ItemToPtr(IPropertyGridItem? prop)
        {
            if (prop is null)
                return default;
            var handle = ((WxPropertyGridItemHandle)prop.Handle).Handle;
            return handle;
        }

        internal void DeleteProperty(IPropertyGridItem prop)
        {
            NativeControl.DeleteProperty(ItemToPtr(prop));
        }

        internal void SetupTextCtrlValue(string text)
        {
            NativeControl.SetupTextCtrlValue(text);
        }

        internal void EndAddChildren(IPropertyGridItem prop)
        {
            NativeControl.EndAddChildren(ItemToPtr(prop));
        }

        protected override void OnDetach()
        {
            base.OnDetach();
            NativeControl.ButtonClick = null;
            NativeControl.Selected = null;
            NativeControl.Changed = null;
            NativeControl.Changing -= NativeControl_Changing;
            NativeControl.Highlighted = null;
            NativeControl.RightClick = null;
            NativeControl.DoubleClick = null;
            NativeControl.ItemCollapsed = null;
            NativeControl.ItemExpanded = null;
            NativeControl.LabelEditBegin -= NativeControl_LabelEditBegin;
            NativeControl.LabelEditEnding -= NativeControl_LabelEditEnding;
            NativeControl.ColBeginDrag -= NativeControl_ColBeginDrag;
            NativeControl.ColDragging = null;
            NativeControl.ColEndDrag = null;
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            NativeControl.ButtonClick += NativeControl_ButtonClick;
            NativeControl.Selected += NativeControl_Selected;
            NativeControl.Changed += NativeControl_Changed;
            NativeControl.Changing += NativeControl_Changing;
            NativeControl.Highlighted += NativeControl_Highlighted;
            NativeControl.RightClick += NativeControl_RightClick;
            NativeControl.DoubleClick += NativeControl_DoubleClick;
            NativeControl.ItemCollapsed += NativeControl_ItemCollapsed;
            NativeControl.ItemExpanded += NativeControl_ItemExpanded;
            NativeControl.LabelEditBegin += NativeControl_LabelEditBegin;
            NativeControl.LabelEditEnding += NativeControl_LabelEditEnding;
            NativeControl.ColBeginDrag += NativeControl_ColBeginDrag;
            NativeControl.ColDragging += NativeControl_ColDragging;
            NativeControl.ColEndDrag += NativeControl_ColEndDrag;
        }

        private void NativeControl_ButtonClick()
        {
            Control.RaiseButtonClick(EventArgs.Empty);
        }

        private void NativeControl_ColEndDrag()
        {
            Control.RaiseColEndDrag(EventArgs.Empty);
        }

        private void NativeControl_ColDragging()
        {
            Control.RaiseColDragging(EventArgs.Empty);
        }

        private void NativeControl_ColBeginDrag(object? sender, CancelEventArgs e)
        {
            Control.RaiseColBeginDrag(e);
        }

        private void NativeControl_LabelEditEnding(object? sender, CancelEventArgs e)
        {
            Control.RaiseLabelEditEnding(e);
        }

        private void NativeControl_LabelEditBegin(object? sender, CancelEventArgs e)
        {
            Control.RaiseLabelEditBegin(e);
        }

        private void NativeControl_ItemExpanded()
        {
            Control.RaiseItemExpanded(EventArgs.Empty);
        }

        private void NativeControl_ItemCollapsed()
        {
            Control.RaiseItemCollapsed(EventArgs.Empty);
        }

        private void NativeControl_DoubleClick()
        {
            Control.RaisePropertyDoubleClick(EventArgs.Empty);
        }

        private void NativeControl_RightClick()
        {
            Control.RaisePropertyRightClick(EventArgs.Empty);
        }

        private void NativeControl_Highlighted()
        {
            Control.RaisePropertyHighlighted(EventArgs.Empty);
        }

        private void NativeControl_Changing(object? sender, CancelEventArgs e)
        {
            Control.RaisePropertyChanging(e);
        }

        private void NativeControl_Changed()
        {
            Control.RaisePropertyChanged(EventArgs.Empty);
        }

        private IPropertyGridItem? PtrToItem(IntPtr ptr)
        {
            return Control.HandleToItem(new WxPropertyGridItemHandle(ptr));
        }

        private void NativeControl_Selected()
        {
            Control.RaisePropertySelected(EventArgs.Empty);
        }

        void IPropertyGridHandler.RefreshProperty(IPropertyGridItem p)
        {
            NativeControl.;
        }

        void IPropertyGridHandler.SetPropertyReadOnly(IPropertyGridItem id, bool set, int flags)
        {
            NativeControl.;
        }

        void IPropertyGridHandler.SetPropertyValueUnspecified(IPropertyGridItem id)
        {
            NativeControl.;
        }

        IPropertyGridItem IPropertyGridHandler.AppendIn(
            IPropertyGridItem id,
            IPropertyGridItem newproperty)
        {
            return NativeControl.;
        }

        void IPropertyGridHandler.BeginAddChildren(IPropertyGridItem id)
        {
            NativeControl.;
        }

        bool IPropertyGridHandler.Collapse(IPropertyGridItem id)
        {
            return NativeControl.;
        }

        void IPropertyGridHandler.DeleteProperty(IPropertyGridItem id)
        {
            return NativeControl.;
        }

        IPropertyGridItem IPropertyGridHandler.RemoveProperty(IPropertyGridItem id)
        {
            return NativeControl.;
        }

        bool IPropertyGridHandler.DisableProperty(IPropertyGridItem id)
        {
            return NativeControl.;
        }

        bool IPropertyGridHandler.EnableProperty(IPropertyGridItem id, bool enable)
        {
            return NativeControl.;
        }

        void IPropertyGridHandler.EndAddChildren(IPropertyGridItem id)
        {
            return NativeControl.;
        }

        bool IPropertyGridHandler.Expand(IPropertyGridItem id)
        {
            return NativeControl.;
        }

        nint IPropertyGridHandler.GetPropertyClientData(IPropertyGridItem id)
        {
            return NativeControl.;
        }

        string IPropertyGridHandler.GetPropertyHelpString(IPropertyGridItem id)
        {
            return NativeControl.;
        }

        string IPropertyGridHandler.GetPropertyLabel(IPropertyGridItem id)
        {
            return NativeControl.;
        }

        string IPropertyGridHandler.GetPropertyValueAsString(IPropertyGridItem id)
        {
            return NativeControl.;
        }

        long IPropertyGridHandler.GetPropertyValueAsLong(IPropertyGridItem id)
        {
            return NativeControl.;
        }

        string IPropertyGridHandler.GetPropNameAsLabel()
        {
            return NativeControl.;
        }

        ulong IPropertyGridHandler.GetPropertyValueAsULong(IPropertyGridItem id)
        {
            return NativeControl.;
        }

        int IPropertyGridHandler.GetPropertyValueAsInt(IPropertyGridItem id)
        {
            return NativeControl.;
        }

        bool IPropertyGridHandler.GetPropertyValueAsBool(IPropertyGridItem id)
        {
            return NativeControl.;
        }

        double IPropertyGridHandler.GetPropertyValueAsDouble(IPropertyGridItem id)
        {
            return NativeControl.;
        }

        DateTime IPropertyGridHandler.GetPropertyValueAsDateTime(IPropertyGridItem id)
        {
            return NativeControl.;
        }

        bool IPropertyGridHandler.HideProperty(IPropertyGridItem id, bool hide, int flags)
        {
            return NativeControl.;
        }

        IPropertyGridItem IPropertyGridHandler.Insert(IPropertyGridItem priorThis, IPropertyGridItem newproperty)
        {
            return NativeControl.;
        }

        IPropertyGridItem IPropertyGridHandler.InsertByIndex(IPropertyGridItem parent, int index, IPropertyGridItem newproperty)
        {
            return NativeControl.;
        }

        bool IPropertyGridHandler.IsPropertyCategory(IPropertyGridItem id)
        {
            return NativeControl.;
        }

        bool IPropertyGridHandler.IsPropertyEnabled(IPropertyGridItem id)
        {
            return NativeControl.;
        }

        bool IPropertyGridHandler.IsPropertyExpanded(IPropertyGridItem id)
        {
            return NativeControl.;
        }

        bool IPropertyGridHandler.IsPropertyModified(IPropertyGridItem id)
        {
            return NativeControl.;
        }

        bool IPropertyGridHandler.IsPropertySelected(IPropertyGridItem id)
        {
            return NativeControl.;
        }

        bool IPropertyGridHandler.IsPropertyShown(IPropertyGridItem id)
        {
            return NativeControl.;
        }

        bool IPropertyGridHandler.IsPropertyValueUnspecified(IPropertyGridItem id)
        {
            return NativeControl.;
        }

        void IPropertyGridHandler.LimitPropertyEditing(IPropertyGridItem id, bool limit)
        {
            NativeControl.;
        }

        IPropertyGridItem IPropertyGridHandler.ReplaceProperty(IPropertyGridItem id, IPropertyGridItem property)
        {
            return NativeControl.;
        }

        void IPropertyGridHandler.SetPropertyBackgroundColor(IPropertyGridItem id, Color color, int flags)
        {
            NativeControl.;
        }

        void IPropertyGridHandler.SetPropertyColorsToDefault(IPropertyGridItem id, int flags)
        {
            NativeControl.;
        }

        void IPropertyGridHandler.SetPropertyTextColor(IPropertyGridItem id, Color col, int flags)
        {
            NativeControl.;
        }

        Color IPropertyGridHandler.GetPropertyBackgroundColor(IPropertyGridItem id)
        {
            return NativeControl.;
        }

        Color IPropertyGridHandler.GetPropertyTextColor(IPropertyGridItem id)
        {
            return NativeControl.;
        }

        void IPropertyGridHandler.SetPropertyEditorByName(IPropertyGridItem id, string editorName)
        {
            NativeControl.;
        }

        void IPropertyGridHandler.SetPropertyLabel(IPropertyGridItem id, string newproplabel)
        {
            NativeControl.;
        }

        void IPropertyGridHandler.SetPropertyName(IPropertyGridItem id, string newName)
        {
            NativeControl.;
        }

        void IPropertyGridHandler.SetPropertyHelpString(IPropertyGridItem id, string helpString)
        {
            NativeControl.;
        }

        bool IPropertyGridHandler.SetPropertyMaxLength(IPropertyGridItem id, int maxLen)
        {
            return NativeControl.;
        }

        void IPropertyGridHandler.SetPropertyValueAsLong(IPropertyGridItem id, long value)
        {
            NativeControl.;
        }

        void IPropertyGridHandler.SetPropertyValueAsInt(IPropertyGridItem id, int value)
        {
            NativeControl.;
        }

        void IPropertyGridHandler.SetPropertyValueAsDouble(IPropertyGridItem id, double value)
        {
            NativeControl.;
        }

        void IPropertyGridHandler.SetPropertyValueAsBool(IPropertyGridItem id, bool value)
        {
            NativeControl.;
        }

        void IPropertyGridHandler.SetPropertyValueAsStr(IPropertyGridItem id, string value)
        {
            NativeControl.;
        }

        void IPropertyGridHandler.SetPropertyValueAsVariant(IPropertyGridItem id, IPropertyGridVariant variant)
        {
            NativeControl.;
        }

        void IPropertyGridHandler.SetPropertyValueAsDateTime(IPropertyGridItem id, DateTime value)
        {
            NativeControl.;
        }

        void IPropertyGridHandler.SetValidationFailureBehavior(int vfbFlags)
        {
            NativeControl.;
        }

        void IPropertyGridHandler.SortChildren(IPropertyGridItem id, int flags)
        {
            NativeControl.;
        }

        bool IPropertyGridHandler.ChangePropertyValue(IPropertyGridItem id, IPropertyGridVariant variant)
        {
            return NativeControl.;
        }

        void IPropertyGridHandler.SetPropertyClientData(IPropertyGridItem prop, nint clientData)
        {
            NativeControl.;
        }

        void IPropertyGridHandler.SetPropertyAttribute(IPropertyGridItem id, string attrName, IPropertyGridVariant variant, PropertyGridItemValueFlags argFlags)
        {
            NativeControl.;
        }

        void IPropertyGridHandler.SetPropertyAttributeAll(string attrName, IPropertyGridVariant variant)
        {
            NativeControl.;
        }

        int IPropertyGridHandler.GetSplitterPosition(uint splitterIndex)
        {
            return NativeControl.;
        }

        int IPropertyGridHandler.GetVerticalSpacing()
        {
            return NativeControl.;
        }

        bool IPropertyGridHandler.IsEditorFocused()
        {
            return NativeControl.;
        }

        bool IPropertyGridHandler.IsEditorsValueModified()
        {
            NativeControl.;
        }

        bool IPropertyGridHandler.IsAnyModified()
        {
            return NativeControl.;
        }

        void IPropertyGridHandler.ResetColors()
        {
            NativeControl.;
        }

        void IPropertyGridHandler.ResetColumnSizes(bool enableAutoResizing)
        {
            NativeControl.;
        }

        void IPropertyGridHandler.MakeColumnEditable(uint column, bool editable)
        {
            NativeControl.;
        }

        void IPropertyGridHandler.BeginLabelEdit(uint column)
        {
            NativeControl.;
        }

        void IPropertyGridHandler.EndLabelEdit(bool commit)
        {
            NativeControl.;
        }

        void IPropertyGridHandler.SetCaptionBackgroundColor(Color col)
        {
            NativeControl.;
        }

        void IPropertyGridHandler.SetCaptionTextColor(Color col)
        {
            NativeControl.;
        }

        void IPropertyGridHandler.SetCellBackgroundColor(Color col)
        {
            NativeControl.;
        }

        void IPropertyGridHandler.SetCellDisabledTextColor(Color col)
        {
            NativeControl.;
        }

        void IPropertyGridHandler.SetCellTextColor(Color col)
        {
            NativeControl.;
        }

        void IPropertyGridHandler.SetColumnCount(int colCount)
        {
            NativeControl.;
        }

        void IPropertyGridHandler.SetEmptySpaceColor(Color col)
        {
            NativeControl.;
        }

        void IPropertyGridHandler.SetLineColor(Color col)
        {
            NativeControl.;
        }

        void IPropertyGridHandler.SetMarginColor(Color col)
        {
            NativeControl.;
        }

        void IPropertyGridHandler.SetSelectionBackgroundColor(Color col)
        {
            NativeControl.;
        }

        void IPropertyGridHandler.SetSelectionTextColor(Color col)
        {
            NativeControl.;
        }

        void IPropertyGridHandler.SetSplitterPosition(int newXPos, int col)
        {
            NativeControl.;
        }

        string IPropertyGridHandler.GetUnspecifiedValueText(int argFlags)
        {
            return NativeControl.;
        }

        void IPropertyGridHandler.SetVirtualWidth(int width)
        {
            NativeControl.;
        }

        void IPropertyGridHandler.SetSplitterLeft(bool privateChildrenToo)
        {
            NativeControl.;
        }

        void IPropertyGridHandler.SetVerticalSpacing(int vspacing)
        {
            NativeControl.;
        }

        bool IPropertyGridHandler.HasVirtualWidth()
        {
            return NativeControl.;
        }

        uint IPropertyGridHandler.GetCommonValueCount()
        {
            return NativeControl.;
        }

        string IPropertyGridHandler.GetCommonValueLabel(uint i)
        {
            return NativeControl.;
        }

        int IPropertyGridHandler.GetUnspecifiedCommonValue()
        {
            return NativeControl.;
        }

        void IPropertyGridHandler.SetUnspecifiedCommonValue(int index)
        {
            NativeControl.;
        }

        void IPropertyGridHandler.RefreshEditor()
        {
            NativeControl.;
        }

        bool IPropertyGridHandler.WasValueChangedInEvent()
        {
            return NativeControl.;
        }

        int IPropertyGridHandler.GetSpacingY()
        {
            return NativeControl.;
        }

        void IPropertyGridHandler.SetupTextCtrlValue(string text)
        {
            NativeControl.;
        }

        bool IPropertyGridHandler.UnfocusEditor()
        {
            return NativeControl.;
        }

        bool IPropertyGridHandler.EnsureVisible(IPropertyGridItem propArg)
        {
            return NativeControl.;
        }

        bool IPropertyGridHandler.SelectProperty(IPropertyGridItem propArg, bool focus)
        {
            return NativeControl.;
        }

        bool IPropertyGridHandler.AddToSelection(IPropertyGridItem propArg)
        {
            return NativeControl.;
        }

        bool IPropertyGridHandler.RemoveFromSelection(IPropertyGridItem propArg)
        {
            return NativeControl.;
        }

        void IPropertyGridHandler.SetCurrentCategory(IPropertyGridItem propArg)
        {
            NativeControl.;
        }

        RectI IPropertyGridHandler.GetImageRect(IPropertyGridItem p, int item)
        {
            return NativeControl.;
        }

        SizeI IPropertyGridHandler.GetImageSize(IPropertyGridItem? p, int item)
        {
            return NativeControl.;
        }

        PropertyGridItemHandle IPropertyGridHandler.CreateStringProperty(string label, string name, string value)
        {
            return NativeControl.;
        }

        PropertyGridItemHandle IPropertyGridHandler.CreateFilenameProperty(string label, string name, string value)
        {
            return NativeControl.;
        }

        PropertyGridItemHandle IPropertyGridHandler.CreateDirProperty(string label, string name, string value)
        {
            return NativeControl.;
        }

        PropertyGridItemHandle IPropertyGridHandler.CreateImageFilenameProperty(string label, string name, string value)
        {
            return NativeControl.;
        }

        PropertyGridItemHandle IPropertyGridHandler.CreateCursorProperty(string label, string name, int value)
        {
            return NativeControl.;
        }

        PropertyGridItemHandle IPropertyGridHandler.CreateBoolProperty(string label, string name, bool value)
        {
            return NativeControl.;
        }

        PropertyGridItemHandle IPropertyGridHandler.CreateIntProperty(string label, string name, long value)
        {
            return NativeControl.;
        }

        PropertyGridItemHandle IPropertyGridHandler.CreateFloatProperty(string label, string name, double value)
        {
            NativeControl.;
        }

        PropertyGridItemHandle IPropertyGridHandler.CreateUIntProperty(string label, string name, ulong value)
        {
            return NativeControl.;
        }

        PropertyGridItemHandle IPropertyGridHandler.CreateLongStringProperty(string label, string name, string value)
        {
            return NativeControl.;
        }

        PropertyGridItemHandle IPropertyGridHandler.CreateDateProperty(string label, string name, DateTime value)
        {
            return NativeControl.;
        }

        PropertyGridItemHandle IPropertyGridHandler.CreateEditEnumProperty(string label, string name, IPropertyGridChoices choices, string value)
        {
            return NativeControl.;
        }

        PropertyGridItemHandle IPropertyGridHandler.CreateEnumProperty(string label, string name, IPropertyGridChoices choices, int value)
        {
            return NativeControl.;
        }

        PropertyGridItemHandle IPropertyGridHandler.CreateFlagsProperty(string label, string name, IPropertyGridChoices choices, int value)
        {
            return NativeControl.;
        }

        PropertyGridItemHandle IPropertyGridHandler.CreatePropCategory(string label, string name)
        {
            return NativeControl.;
        }

        void IPropertyGridHandler.Clear()
        {
            NativeControl.;
        }

        IPropertyGridItem IPropertyGridHandler.Append(IPropertyGridItem property)
        {
            return NativeControl.;
        }

        bool IPropertyGridHandler.ClearSelection(bool validation)
        {
            return NativeControl.;
        }

        void IPropertyGridHandler.ClearModifiedStatus()
        {
            NativeControl.;
        }

        bool IPropertyGridHandler.CollapseAll()
        {
            return NativeControl.;
        }

        bool IPropertyGridHandler.EditorValidate()
        {
            return NativeControl.;
        }

        bool IPropertyGridHandler.ExpandAll(bool expand)
        {
            return NativeControl.;
        }

        string IPropertyGridHandler.GetPropertyName(IPropertyGridItem property)
        {
            return NativeControl.;
        }

        bool IPropertyGridHandler.RestoreEditableState(string src, int restoreStates)
        {
            return NativeControl.;
        }

        string IPropertyGridHandler.SaveEditableState(int includedStates)
        {
            return NativeControl.;
        }

        bool IPropertyGridHandler.SetColumnProportion(uint column, int proportion)
        {
            return NativeControl.;
        }

        int IPropertyGridHandler.GetColumnProportion(uint column)
        {
            return NativeControl.;
        }

        void IPropertyGridHandler.Sort(int flags)
        {
            NativeControl.;
        }

        PointI IPropertyGridHandler.CalcScrolledPosition(PointI point)
        {
            return NativeControl.;
        }

        PointI IPropertyGridHandler.CalcUnscrolledPosition(PointI point)
        {
            return NativeControl.;
        }

        int IPropertyGridHandler.GetHitTestColumn(PointI point)
        {
            NativeControl.;
        }

        void IPropertyGridHandler.SetPropertyFlag(IPropertyGridItem prop, int flag, bool value)
        {
            NativeControl.;
        }

        void IPropertyGridHandler.AddActionTrigger(int action, int keycode, int modifiers)
        {
            NativeControl.;
        }

        void IPropertyGridHandler.DedicateKey(int keycode)
        {
            NativeControl.;
        }

        void IPropertyGridHandler.CenterSplitter(bool enableAutoResizing)
        {
            NativeControl.;
        }

        void IPropertyGridHandler.ClearActionTriggers(int action)
        {
            NativeControl.;
        }

        bool IPropertyGridHandler.CommitChangesFromEditor(uint flags)
        {
            return NativeControl.;
        }

        void IPropertyGridHandler.EditorsValueWasModified()
        {
            NativeControl.;
        }

        void IPropertyGridHandler.EditorsValueWasNotModified()
        {
            NativeControl.;
        }

        bool IPropertyGridHandler.EnableCategories(bool enable)
        {
            return NativeControl.;
        }

        SizeD IPropertyGridHandler.FitColumns()
        {
            return NativeControl.;
        }

        Color IPropertyGridHandler.GetCaptionBackgroundColor()
        {
            return NativeControl.;
        }

        Color IPropertyGridHandler.GetCaptionForegroundColor()
        {
            return NativeControl.;
        }

        Color IPropertyGridHandler.GetCellBackgroundColor()
        {
            return NativeControl.;
        }

        Color IPropertyGridHandler.GetCellDisabledTextColor()
        {
            return NativeControl.;
        }

        Color IPropertyGridHandler.GetCellTextColor()
        {
            return NativeControl.;
        }

        uint IPropertyGridHandler.GetColumnCount()
        {
            return NativeControl.;
        }

        Color IPropertyGridHandler.GetEmptySpaceColor()
        {
            return NativeControl.;
        }

        int IPropertyGridHandler.GetFontHeight()
        {
            return NativeControl.;
        }

        Color IPropertyGridHandler.GetLineColor()
        {
            return NativeControl.;
        }

        Color IPropertyGridHandler.GetMarginColor()
        {
            return NativeControl.;
        }

        int IPropertyGridHandler.GetMarginWidth()
        {
            return NativeControl.;
        }

        int IPropertyGridHandler.GetRowHeight()
        {
            return NativeControl.;
        }

        Color IPropertyGridHandler.GetSelectionBackgroundColor()
        {
            return NativeControl.;
        }

        Color IPropertyGridHandler.GetSelectionForegroundColor()
        {
            return NativeControl.;
        }

        public class WxPropertyGridItemHandle : PropertyGridItemHandle
        {
            private IntPtr handle;

            public WxPropertyGridItemHandle(IntPtr handle)
            {
                this.handle = handle;
            }

            public IntPtr Handle => handle;

            public override int GetHashCode()
            {
                return handle.GetHashCode();
            }

            public override bool Equals(object? obj)
            {
                if (obj is not WxPropertyGridItemHandle wx)
                    return false;
                return handle == wx.handle;
            }
        }

        public class NativePropertyGrid : Native.PropertyGrid
        {
            public NativePropertyGrid(PropertyGridCreateStyle style)
                : base()
            {
                SetNativePointer(NativeApi.PropertyGrid_CreateEx_((int)style));
            }
        }
    }
}