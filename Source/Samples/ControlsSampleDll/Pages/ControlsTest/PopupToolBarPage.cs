using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace ControlsSample
{
    internal class PopupToolBarPage : Panel
    {
        public DropDownAlignment? DropDownVerticalAlignment = DropDownAlignment.AfterEnd;
        public DropDownAlignment? DropDownHorizontalAlignment = DropDownAlignment.AfterStart;

        public PopupToolBarPage()
        {
            var contextMenu = new SampleContextMenu();
            var popupToolBar = new PopupToolBar();
            var syncedPopupToolBar = new PopupToolBar();

            InitToolBar(popupToolBar.MainControl, true);

            popupToolBar.MainControl.ConfigureAsContextMenu();

            Layout = LayoutStyle.Vertical;

            var button = new Button
            {
                Text = "Show Popup ToolBar",
                Margin = 10,
                Parent = this,
            };

            button.Click += (s, e) =>
            {
                popupToolBar.ShowPopup(button);
            };

            var button2 = new Button
            {
                Text = "Show Context Menu",
                Margin = 10,
                Parent = this,
                DropDownMenu = contextMenu,
            };

            var button3 = new Button
            {
                Text = "Show Context Menu in Popup ToolBar",
                Margin = 10,
                Parent = this,
            };

            button3.Click += (s, e) =>
            {
                syncedPopupToolBar.MainControl.DataContext = contextMenu;
                syncedPopupToolBar.MainControl.ConfigureAsContextMenu();

                var position = new HVDropDownAlignment(
                    DropDownHorizontalAlignment ?? DropDownAlignment.BeforeStart,
                    DropDownVerticalAlignment ?? DropDownAlignment.AfterEnd);

                syncedPopupToolBar.ShowPopup(button3, position);
            };

            Label label1 = new()
            {
                Text = "Vertical Alignment:",
                Margin = 10,
                Parent = this,
            };

            EnumPicker pickerVerticalAlignment = new()
            {
                Margin = 10,
                Parent = this,
            };

            pickerVerticalAlignment.EnumType = typeof(DropDownAlignment);
            pickerVerticalAlignment.Value = DropDownVerticalAlignment
                ?? DropDownAlignment.AfterEnd;
            pickerVerticalAlignment.ValueChanged += (s, e) =>
            {
                DropDownVerticalAlignment = (DropDownAlignment?)pickerVerticalAlignment.Value;
            };

            Label label2 = new()
            {
                Text = "Horizontal Alignment:",
                Margin = 10,
                Parent = this,
            };

            EnumPicker pickerHorizontalAlignment = new()
            {
                Margin = 10,
                Parent = this,
            };

            pickerHorizontalAlignment.EnumType = typeof(DropDownAlignment);
            pickerHorizontalAlignment.Value = DropDownHorizontalAlignment
                ?? DropDownAlignment.BeforeStart;
            pickerHorizontalAlignment.ValueChanged += (s, e) =>
            {
                DropDownHorizontalAlignment = (DropDownAlignment?)pickerHorizontalAlignment.Value;
            };
        }

        public static void InitToolBar(ToolBar toolbar, bool onlyButtons = false)
        {
            var buttonIdNew = toolbar.AddSpeedBtn(
                CommonStrings.Default.ButtonNew,
                KnownSvgImages.ImgFileNew);
            toolbar.AddToolAction(buttonIdNew, ButtonClick);
            toolbar.SetToolShortcut(buttonIdNew, Keys.Control | Keys.N);

            var buttonOpen = toolbar.AddSpeedBtnCore(
                CommonStrings.Default.ButtonOpen,
                KnownSvgImages.ImgFileOpen);
            toolbar.AddToolAction(buttonOpen.UniqueId, ButtonClick);
            toolbar.SetToolShortcut(buttonOpen.UniqueId, Keys.Control | Keys.O | Keys.Shift | Keys.Alt);

            ContextMenu? menu = new();

            menu.Add("Item 1").ClickAction = () => { App.Log("Item 1 clicked"); };
            menu.Add("Item 2").ClickAction = () => { App.Log("Item 2 clicked"); };

            toolbar.SetToolDropDownMenu(buttonOpen.UniqueId, menu, MenuItem.DefaultMenuArrowImage);

            var separatorId = toolbar.AddSeparator();

            var saveClone = KnownSvgImages.ImgFileSave.Clone();
            saveClone.SetColorOverride(KnownSvgColor.Normal, isDark: true, Color.Yellow);
            saveClone.SetColorOverride(KnownSvgColor.Normal, false, Color.Red);

            var buttonIdSave = toolbar.AddSpeedBtn(
                CommonStrings.Default.ButtonSave,
                saveClone);
            toolbar.AddToolAction(buttonIdSave, ButtonClick);

            toolbar.AddSeparator();

            var idText = toolbar.AddText("Category:");
            toolbar.AddToolAction(idText, ButtonClick);
            toolbar.LastChild!.IsBold = true;

            if (!onlyButtons)
            {
                var textBox = new TextBox();
                textBox.VerticalAlignment = VerticalAlignment.Center;
                textBox.Text = "text1";
                textBox.MinWidth = 100;
                textBox.HorizontalAlignment = HorizontalAlignment.Fill;

                var idEdit = toolbar.AddControl(textBox);
            }

            static void ButtonClick(object? sender, EventArgs e)
            {
                if (sender is not AbstractControl button)
                    return;
                App.Log($"Button click: {button.ToolTip}");
            }

            toolbar.AddSpeedBtn("Some item 1", null);
            toolbar.AddSpeedBtn("Some item 2", KnownSvgImages.ImgBold);
        }
    }
}
