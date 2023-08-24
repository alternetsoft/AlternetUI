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

        /*

        ====
            public void SetPropertyReadOnly(wxPGPropArg id, bool set = true, int flags) =>
                throw new Exception();

            public void SetPropertyValueUnspecified(wxPGPropArg id) => throw new Exception();


            public IntPtr AppendIn(wxPGPropArg id, IntPtr newproperty) => throw new Exception();

            public void BeginAddChildren(wxPGPropArg id) => throw new Exception();

            public bool Collapse(wxPGPropArg id) => throw new Exception();

            public bool ChangePropertyValue(wxPGPropArg id, wxVariant newValue) =>
                throw new Exception();

            public void DeleteProperty(wxPGPropArg id) => throw new Exception();

            public IntPtr RemoveProperty(wxPGPropArg id) => throw new Exception();

            public bool DisableProperty(wxPGPropArg id) => throw new Exception();

            public bool EnableProperty(wxPGPropArg id, bool enable = true) =>
                throw new Exception();

            public void EndAddChildren(wxPGPropArg id) => throw new Exception();

            public bool Expand(wxPGPropArg id) => throw new Exception();

            public IntPtr GetFirstChild(wxPGPropArg id) => throw new Exception();

            public IntPtr GetPropertyCategory(wxPGPropArg id) => throw new Exception();

            public IntPtr GetPropertyClientData(wxPGPropArg id) => throw new Exception();

            public string GetPropertyHelpString(wxPGPropArg id) => throw new Exception();

            public IntPtr GetPropertyImage(wxPGPropArg id) => throw new Exception();

            public string GetPropertyLabel(wxPGPropArg id) => throw new Exception();

            public IntPtr GetPropertyParent(wxPGPropArg id) => throw new Exception();

            public string GetPropertyValueAsString(wxPGPropArg id) => throw new Exception();

            public long GetPropertyValueAsLong(wxPGPropArg id) => throw new Exception();

            public ulong GetPropertyValueAsULong(wxPGPropArg id) => throw new Exception();

            public int GetPropertyValueAsInt(wxPGPropArg id) => throw new Exception();

            public bool GetPropertyValueAsBool(wxPGPropArg id) => throw new Exception();

            public double GetPropertyValueAsDouble(wxPGPropArg id) => throw new Exception();

            public Alternet.UI.DateTime GetPropertyValueAsDate(wxPGPropArg id) =>
                throw new Exception();

            public bool HideProperty(wxPGPropArg id, bool hide = true, int flags) =>
                throw new Exception();

            public IntPtr Insert(wxPGPropArg priorThis, IntPtr newproperty) =>
                throw new Exception();

            public IntPtr Insert(wxPGPropArg parent, int index, IntPtr newproperty) =>
                throw new Exception();

            public bool IsPropertyCategory(wxPGPropArg id) => throw new Exception();

            public bool IsPropertyEnabled(wxPGPropArg id) => throw new Exception();

            public bool IsPropertyExpanded(wxPGPropArg id) => throw new Exception();

            public bool IsPropertyModified(wxPGPropArg id) => throw new Exception();

            public bool IsPropertySelected(wxPGPropArg id) => throw new Exception();

            public bool IsPropertyShown(wxPGPropArg id) => throw new Exception();

            public bool IsPropertyValueUnspecified(wxPGPropArg id) => throw new Exception();

            public void LimitPropertyEditing(wxPGPropArg id, bool limit = true) =>
                throw new Exception();

            public IntPtr ReplaceProperty(wxPGPropArg id, IntPtr property) =>
                throw new Exception();

            public void SetPropertyBackgroundColor(wxPGPropArg id, Color color, int flags) =>
                throw new Exception();

            public void SetPropertyColorsToDefault(wxPGPropArg id, int flags) =>
                throw new Exception();

            public void SetPropertyTextColor(wxPGPropArg id, Color col, int flags) =>
                throw new Exception();

            public Color GetPropertyBackgroundColor(wxPGPropArg id) => throw new Exception();

            public Color GetPropertyTextColor(wxPGPropArg id) => throw new Exception();

            public void SetPropertyClientData(wxPGPropArg id, IntPtr clientData) =>
                throw new Exception();

            public void SetPropertyEditor(wxPGPropArg id, IntPtr editor) =>
                throw new Exception();

            public void SetPropertyEditor(wxPGPropArg id, string editorName) =>
                throw new Exception();

            public void SetPropertyLabel(wxPGPropArg id, string newproplabel) =>
                throw new Exception();

            public void SetPropertyName(wxPGPropArg id, string newName) =>
                throw new Exception();

            public void SetPropertyHelpString(wxPGPropArg id, string helpString) =>
                throw new Exception();

            public bool SetPropertyMaxLength(wxPGPropArg id, int maxLen) =>
                throw new Exception();

            public void SetPropertyValueAsLong(wxPGPropArg id, long value) =>
                throw new Exception();

            public void SetPropertyValueAsInt(wxPGPropArg id, int value) =>
                throw new Exception();

            public void SetPropertyValueAsDouble(wxPGPropArg id, double value) =>
                throw new Exception();

            public void SetPropertyValueAsBool(wxPGPropArg id, bool value) =>
                throw new Exception();

            public void SetPropertyValueAsStr(wxPGPropArg id, string value) =>
                throw new Exception();

            public void SetPropertyValueAsDateTime(wxPGPropArg id, ALternet.UI.DateTime value) =>
                throw new Exception();

            public void SetPropertyValueAsLong(wxPGPropArg id, long value) =>
                throw new Exception();

            public void SetValidationFailureBehavior(int vfbFlags) => throw new Exception();

            public void SortChildren(wxPGPropArg id, int flags = 0) => throw new Exception();

            public static IntPtr GetEditorByName(string& editorName) => throw new Exception();

            //public void SetPropertyImage(wxPGPropArg id, const wxBitmapBundle& bmp) =>
                throw new Exception();

            //public void SetPropertyAttribute(wxPGPropArg id,
            //                           string attrName,
            //                           wxVariant value,
            //                           long argFlags = 0) =>
            //    throw new Exception();

            //public void SetPropertyAttributeAll(string attrName, wxVariant value) =>
                throw new Exception();

            //const wxArrayPGProperty& GetSelectedProperties() =>
                throw new Exception();

         */
    }
}