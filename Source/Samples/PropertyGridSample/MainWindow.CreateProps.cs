using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using Alternet.Drawing;
using Alternet.UI;
using Alternet.Base.Collections;
using Alternet.UI.Localization;

namespace PropertyGridSample
{
    public partial class MainWindow : Window
    {
        private void InitDefaultPropertyGrid()
        {
            PropGrid.BeginUpdate();
            try
            {
                PropGrid.Clear();

                PropGrid.AddPropCategory("Properties");
                PropGrid.AddProps(WelcomeProps.Default, null, true);

                CreateCollectionProperties(false);
                CreateStringProperties();
                CreateColorProperties();
                CreateEnumProperties();
                CreateOtherProperties();

                PropGrid.AddPropCategory("Nullable");
                PropGrid.AddProps(NullableProps.Default);

                if(!SystemSettings.AppearanceIsDark)
                    PropGrid.SetCategoriesBackgroundColor(Color.LightGray);
            }
            finally
            {
                PropGrid.EndUpdate();
                PropGrid.Refresh();
            }
        }

        void CreateOtherProperties(bool add = true)
        {
            if (!add)
                return;
            PropGrid.AddPropCategory("Other");

            // Date
            var prop = PropGrid.CreateDateItem("Date");
            PropGrid.Add(prop);
            // If none Date is selected (checkbox next to date editor is unchecked)
            // DateTime.MinValue is returned.
            PropGrid.SetPropertyKnownAttribute(
                prop,
                PropertyGridItemAttrId.PickerStyle,
                (int)(DatePickerStyleFlags.DropDown | DatePickerStyleFlags.ShowCentury
                | DatePickerStyleFlags.AllowNone));

            prop = PropGrid.CreateBoolItem("Bool 2");
            PropGrid.Add(prop);
            PropGrid.SetPropertyKnownAttribute(
                prop,
                PropertyGridItemAttrId.UseCheckbox,
                false);

            prop = PropGrid.CreateBoolItem("Bool 3");
            PropGrid.Add(prop);
            PropGrid.SetPropertyKnownAttribute(
                prop,
                PropertyGridItemAttrId.UseCheckbox,
                true);
        }

        void CreateCollectionProperties(bool add)
        {
            if (!add)
                return;

            PropGrid.AddPropCategory("Collection");

            void AddCollectionProp(string label, string propName)
            {
                var prop = PropGrid.CreateProperty(label, null, this, propName);
                if (prop != null)
                {
                    PropGrid.SetPropertyReadOnly(prop, false, false);
                    PropGrid.SetPropertyEditorByKnownName(
                        prop,
                        PropertyGridKnownEditors.TextCtrlAndButton);
                    PropGrid.Add(prop);
                }
            }

            AddCollectionProp("Collection<string>", "ItemsString");
            AddCollectionProp("Collection<object>", "ItemsObject");
        }

        void CreateColorProperties()
        {
            var prm = PropertyGrid.CreateNewItemParams();
            PropGrid.AddPropCategory("Color");

            // Default color editor
            prm.EditKindColor = PropertyGridEditKindColor.Default;
            var prop = PropGrid.CreateColorItemWithKind(
                "Color (Default)",
                null,
                Color.Green,
                prm);
            PropGrid.Add(prop);

            // System color editor
            prm.EditKindColor = PropertyGridEditKindColor.SystemColors;
            prop = PropGrid.CreateColorItemWithKind(
                "Color (System)",
                null,
                Color.FromKnownColor(KnownColor.ButtonFace),
                prm);
            PropGrid.Add(prop);

            // Color with ComboBox
            prm.EditKindColor = PropertyGridEditKindColor.ComboBox;
            prop = PropGrid.CreateColorItemWithKind(
                "Color (ComboBox)",
                null,
                Color.Yellow,
                prm);
            PropGrid.Add(prop);

            // Color with Dialog
            prm.EditKindColor = PropertyGridEditKindColor.TextBoxAndButton;
            prop = PropGrid.CreateColorItemWithKind(
                "Color (Dialog)",
                null,
                Color.Red,
                prm);
            PropGrid.Add(prop);

            // Color with Choice
            prm.EditKindColor = PropertyGridEditKindColor.Choice;
            prop = PropGrid.CreateColorItemWithKind(
                "Color (Choice)",
                null,
                Color.Navy,
                prm);
            PropGrid.Add(prop);

            // Color with Choice and Button
            prm.EditKindColor = PropertyGridEditKindColor.ChoiceAndButton;
            prop = PropGrid.CreateColorItemWithKind(
                "Color (Choice and btn)",
                null,
                Color.Navy,
                prm);
            PropGrid.Add(prop);
        }

        void CreateEnumProperties()
        {
            PropGrid.AddPropCategory("Flags and Enum");

            var choices1 = PropertyGrid.CreateChoicesOnce(typeof(PropertyGridCreateStyle));
            var prop = PropGrid.CreateFlagsItem("Flags", null, choices1,
                PropertyGrid.DefaultCreateStyle);
            PropGrid.Add(prop);

            var choices2 = PropertyGrid.CreateChoicesOnce(typeof(HorizontalAlignment));
            prop = PropGrid.CreateChoicesItem("Enum", null, choices2,
                HorizontalAlignment.Center);
            PropGrid.Add(prop);

            // Editable enum. Can have values which are not in choices.
            var choices = PropertyGrid.CreateChoices();
            choices.Add("Item 1", 1);
            choices.Add("Item 2", 2);
            choices.Add("Item 3", 3);
            prop = PropGrid.CreateEditEnumItem("Enum (editable)", null, choices, "Item 2");
            PropGrid.Add(prop);

            //Font Name
            choices = PropertyGridAdapterFont.FontNameChoices;
            prop = PropGrid.CreateChoicesItem(
                "Font name",
                null,
                choices,
                choices.GetValueFromLabel(Font.Default.Name));
            PropGrid.Add(prop);
        }

        void CreateStringProperties()
        {
            PropGrid.AddPropCategory("String");
            var prm = PropertyGrid.CreateNewItemParams();

            var prop = PropGrid.CreateStringItem("Str");
            PropGrid.Add(prop);

            prm.EditKindString = PropertyGridEditKindString.Ellipsis;
            prop = PropGrid.CreateStringItemWithKind("Str (Ellipsis)", null, null, prm);
            PropGrid.Add(prop);

            prm.EditKindString = PropertyGridEditKindString.Long;
            prop = PropGrid.CreateStringItemWithKind("Str (Long Edit)", null, null, prm);
            PropGrid.Add(prop);

            // Password
            prop = PropGrid.CreateStringItem("Str (Password)");
            PropGrid.Add(prop);
            // Password attribute must be set after adding property to PropertyGrid
            PropGrid.SetPropertyKnownAttribute(prop, PropertyGridItemAttrId.Password, true);

            prop = PropGrid.CreateStringItem(
                "Str (Readonly)",
                null,
                "Some Text");
            PropGrid.SetPropertyReadOnly(prop, true, false);
            PropGrid.Add(prop);

            prop = PropGrid.CreateStringItem(
                                "Error",
                                null,
                                "Error if changed and Enter pressed");
            PropGrid.Add(prop);

            // Filename
            prm.EditKindString = PropertyGridEditKindString.FileName;
            prop = PropGrid.CreateStringItemWithKind(
                "Str (Filename)",
                null,
                PathUtils.GetAppFolder(),
                prm);
            PropGrid.SetPropertyKnownAttribute(
                prop,
                PropertyGridItemAttrId.Wildcard,
                "Text Files (*.txt)|*.txt");
            PropGrid.SetPropertyKnownAttribute(
                prop,
                PropertyGridItemAttrId.DialogTitle,
                "Custom File Dialog Title");
            PropGrid.SetPropertyKnownAttribute(
                prop,
                PropertyGridItemAttrId.ShowFullPath,
                false);
            PropGrid.Add(prop);

            // Dir
            prm.EditKindString = PropertyGridEditKindString.Directory;
            prop = PropGrid.CreateStringItemWithKind(
                "Str (Directory)",
                null,
                PathUtils.GetAppFolder(),
                prm);
            PropGrid.SetPropertyKnownAttribute(
                prop,
                PropertyGridItemAttrId.DialogTitle,
                "This is a custom dir dialog title");
            PropGrid.Add(prop);

            // Image filename
            prm.EditKindString = PropertyGridEditKindString.ImageFileName;
            prop = PropGrid.CreateStringItemWithKind(
                "Str (Image Filename)",
                null,
                PathUtils.GetAppFolder(),
                prm);
            PropGrid.Add(prop);

            // Readonly text and ellipsis
            prm.EditKindString = PropertyGridEditKindString.Ellipsis;
            prm.TextReadOnly = true;
            prop = PropGrid.CreateStringItemWithKind(
                "Str (Readonly + Ellipsis)",
                null,
                null,
                prm);
            PropGrid.Add(prop);
        }
    }
}