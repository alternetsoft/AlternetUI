#pragma warning disable
using ApiCommon;
using System;
using System.Collections.Generic;
using System.Text;
using Alternet.Drawing;

namespace NativeApi.Api
{
    //https://docs.wxwidgets.org/3.2/overview_propgrid.html
    //https://docs.wxwidgets.org/3.2/classwx_property_grid.html
    public partial class PropertyGrid : Control
    {
        public IntPtr CreateStringProperty(string label, string name, string value) =>
            throw new Exception();


        public IntPtr CreateFilenameProperty(string label, string name, string value) =>
            throw new Exception();

        public IntPtr CreateDirProperty(string label, string name, string value) =>
            throw new Exception();

        public IntPtr CreateImageFilenameProperty(string label, string name, string value) =>
            throw new Exception();

        public IntPtr CreateSystemColorProperty(string label, string name, Color value) =>
            throw new Exception();

        public IntPtr CreateCursorProperty(string label, string name, int value) =>
            throw new Exception();

        public IntPtr CreateBoolProperty(string label, string name, bool value = false) =>
            throw new Exception();

        public IntPtr CreateIntProperty(string label, string name, long value = 0) =>
            throw new Exception();

        public IntPtr CreateFloatProperty(string label, string name, double value = 0.0) =>
            throw new Exception();

        public IntPtr CreateUIntProperty(string label, string name, ulong value = 0) =>
            throw new Exception();

        public IntPtr CreateLongStringProperty(string label, string name, string value) =>
            throw new Exception();

        public IntPtr CreateDateProperty(string label, string name, Alternet.UI.DateTime value) =>
            throw new Exception();

        public void Clear() => throw new Exception();

        public IntPtr Append(IntPtr property) => throw new Exception();

        public IntPtr CreateEditEnumProperty(
            string label,
            string name,
            IntPtr choices,
            string value) => throw new Exception();

        public IntPtr CreateEnumProperty(
            string label,
            string name,
            IntPtr choices,
            int value = 0) => throw new Exception();

        public IntPtr CreateFlagsProperty(
            string label,
            string name,
            IntPtr choices,
            int value = 0) => throw new Exception();

        public bool ClearSelection(bool validation = false) => throw new Exception();

        public void ClearModifiedStatus() => throw new Exception();

        public bool CollapseAll() => throw new Exception();

        public bool EditorValidate() => throw new Exception();

        public bool ExpandAll(bool expand = true) => throw new Exception();

        public IntPtr CreatePropCategory(
            string label,
            string name) => throw new Exception();

        public IntPtr GetFirst(int flags) => throw new Exception();

        public IntPtr GetProperty(string name) => throw new Exception();

        public IntPtr GetPropertyByLabel(string label) => throw new Exception();

        public IntPtr GetPropertyByName(string name) => throw new Exception();

        public IntPtr GetPropertyByNameAndSubName(string name, string subname) =>
            throw new Exception();

        public IntPtr GetSelection() => throw new Exception();

        public string GetPropertyName(IntPtr property) => throw new Exception();

        public static void InitAllTypeHandlers() => throw new Exception();

        public static void RegisterAdditionalEditors() => throw new Exception();

        public bool RestoreEditableState(string src, int restoreStates) =>
            throw new Exception();

        public string SaveEditableState(int includedStates) => throw new Exception();

        public static void SetBoolChoices(string trueChoice, string falseChoice) =>
            throw new Exception();

        public bool SetColumnProportion(uint column, int proportion) => throw new Exception();

        public int GetColumnProportion(uint column) => throw new Exception();

        public void Sort(int flags = 0) => throw new Exception();

        public void RefreshProperty(IntPtr p) => throw new Exception();

        public IntPtr CreateColorProperty(string label, string name, Color value) =>
            throw new Exception();

        public void SetPropertyReadOnly(IntPtr id, bool set, int flags) =>
            throw new Exception();

        public void SetPropertyValueUnspecified(IntPtr id) => throw new Exception();

        public IntPtr AppendIn(IntPtr id, IntPtr newproperty) => throw new Exception();

        public void BeginAddChildren(IntPtr id) => throw new Exception();

        public bool Collapse(IntPtr id) => throw new Exception();

        public void DeleteProperty(IntPtr id) => throw new Exception();

        public IntPtr RemoveProperty(IntPtr id) => throw new Exception();

        public bool DisableProperty(IntPtr id) => throw new Exception();

        public bool EnableProperty(IntPtr id, bool enable = true) =>
            throw new Exception();

        public void EndAddChildren(IntPtr id) => throw new Exception();

        public bool Expand(IntPtr id) => throw new Exception();

        public IntPtr GetFirstChild(IntPtr id) => throw new Exception();

        public IntPtr GetPropertyCategory(IntPtr id) => throw new Exception();

        public IntPtr GetPropertyClientData(IntPtr id) => throw new Exception();

        public string GetPropertyHelpString(IntPtr id) => throw new Exception();

        public IntPtr GetPropertyImage(IntPtr id) => throw new Exception();

        public string GetPropertyLabel(IntPtr id) => throw new Exception();

        public IntPtr GetPropertyParent(IntPtr id) => throw new Exception();

        public string GetPropertyValueAsString(IntPtr id) => throw new Exception();

        public long GetPropertyValueAsLong(IntPtr id) => throw new Exception();

        public ulong GetPropertyValueAsULong(IntPtr id) => throw new Exception();

        public int GetPropertyValueAsInt(IntPtr id) => throw new Exception();

        public bool GetPropertyValueAsBool(IntPtr id) => throw new Exception();

        public double GetPropertyValueAsDouble(IntPtr id) => throw new Exception();

        public Alternet.UI.DateTime GetPropertyValueAsDateTime(IntPtr id) =>
            throw new Exception();

        public bool HideProperty(IntPtr id, bool hide, int flags) =>
            throw new Exception();

        public IntPtr Insert(IntPtr priorThis, IntPtr newproperty) =>
            throw new Exception();

        public IntPtr InsertByIndex(IntPtr parent, int index, IntPtr newproperty) =>
            throw new Exception();

        public bool IsPropertyCategory(IntPtr id) => throw new Exception();

        public bool IsPropertyEnabled(IntPtr id) => throw new Exception();

        public bool IsPropertyExpanded(IntPtr id) => throw new Exception();

        public bool IsPropertyModified(IntPtr id) => throw new Exception();

        public bool IsPropertySelected(IntPtr id) => throw new Exception();

        public bool IsPropertyShown(IntPtr id) => throw new Exception();

        public bool IsPropertyValueUnspecified(IntPtr id) => throw new Exception();

        public void LimitPropertyEditing(IntPtr id, bool limit = true) =>
            throw new Exception();

        public IntPtr ReplaceProperty(IntPtr id, IntPtr property) =>
            throw new Exception();

        public void SetPropertyBackgroundColor(IntPtr id, Color color, int flags) =>
            throw new Exception();

        public void SetPropertyColorsToDefault(IntPtr id, int flags) =>
            throw new Exception();

        public void SetPropertyTextColor(IntPtr id, Color col, int flags) =>
            throw new Exception();

        public Color GetPropertyBackgroundColor(IntPtr id) => throw new Exception();

        public Color GetPropertyTextColor(IntPtr id) => throw new Exception();

        public void SetPropertyClientData(IntPtr id, IntPtr clientData) =>
            throw new Exception();

        public void SetPropertyEditor(IntPtr id, IntPtr editor) =>
            throw new Exception();

        public void SetPropertyEditorByName(IntPtr id, string editorName) =>
            throw new Exception();

        public void SetPropertyLabel(IntPtr id, string newproplabel) =>
            throw new Exception();

        public void SetPropertyName(IntPtr id, string newName) =>
            throw new Exception();

        public void SetPropertyHelpString(IntPtr id, string helpString) =>
            throw new Exception();

        public bool SetPropertyMaxLength(IntPtr id, int maxLen) =>
            throw new Exception();

        public void SetPropertyValueAsLong(IntPtr id, long value) =>
            throw new Exception();

        public void SetPropertyValueAsInt(IntPtr id, int value) =>
            throw new Exception();

        public void SetPropertyValueAsDouble(IntPtr id, double value) =>
            throw new Exception();

        public void SetPropertyValueAsBool(IntPtr id, bool value) =>
            throw new Exception();

        public void SetPropertyValueAsStr(IntPtr id, string value) =>
            throw new Exception();

        public void SetPropertyValueAsVariant(IntPtr id, IntPtr variant) =>
            throw new Exception();

        public void SetPropertyValueAsDateTime(IntPtr id, Alternet.UI.DateTime value) =>
            throw new Exception();

        public void SetValidationFailureBehavior(int vfbFlags) => throw new Exception();

        public void SortChildren(IntPtr id, int flags = 0) => throw new Exception();

        public static IntPtr GetEditorByName(string editorName) => throw new Exception();

        public bool ChangePropertyValue(IntPtr id, IntPtr variant) =>
            throw new Exception();

        public void SetPropertyImage(IntPtr id, ImageSet? bmp) =>
            throw new Exception();

        public void SetPropertyAttribute(IntPtr id, string attrName, IntPtr variant,
            long argFlags = 0) => throw new Exception();

        public void SetPropertyAttributeAll(string attrName, IntPtr variant) =>
            throw new Exception();
    }
}