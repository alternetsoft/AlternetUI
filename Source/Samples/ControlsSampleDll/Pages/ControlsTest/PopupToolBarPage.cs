using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace ControlsSample
{
    internal class PopupToolBarPage : Panel
    {
        public PopupToolBarPage()
        {
            var contextMenu = new SampleContextMenu();
            var popupToolBar = new PopupToolBar
            {
            };

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
        }

        public static void InitToolBar(ToolBar toolbar, bool onlyButtons = false)
        {
            var buttonIdNew = toolbar.AddSpeedBtn(
                CommonStrings.Default.ButtonNew,
                KnownSvgImages.ImgFileNew);
            toolbar.AddToolAction(buttonIdNew, ButtonClick);
            toolbar.SetToolShortcut(buttonIdNew, Keys.Control | Keys.N);

            var buttonIdOpen = toolbar.AddSpeedBtn(
                CommonStrings.Default.ButtonOpen,
                KnownSvgImages.ImgFileOpen);
            toolbar.AddToolAction(buttonIdOpen, ButtonClick);
            toolbar.SetToolShortcut(buttonIdOpen, Keys.Control | Keys.O);

            ContextMenu? menu = new();

            menu.Add("Item 1").ClickAction = () => { App.Log("Item 1 clicked"); };
            menu.Add("Item 2").ClickAction = () => { App.Log("Item 2 clicked"); };

            toolbar.SetToolDropDownMenu(buttonIdOpen, menu, MenuItem.DefaultMenuArrowImage);

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
