using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;
using Alternet.UI;
using Alternet.UI.Localization;

namespace PropertyGridSample
{
    public partial class ObjectInit
    {
        public static void InitFindReplaceControl(object control)
        {
            if (control is not FindReplaceControl findReplace)
                return;

            findReplace.HorizontalAlignment = HorizontalAlignment.Left;
            findReplace.HasBorder = true;
            findReplace.ReplaceVisible = true;
            findReplace.Manager = findReplace.CreateLogger();
        }

        public static void InitGenericToolBarSet(object control)
        {
            if (control is not ToolBarSet toolbar)
                return;

            toolbar.ToolBarCount = 3;
            toolbar.SuggestedWidth = 300;

            var toolbar1 = toolbar[0];
            var toolbar2 = toolbar[1];
            var toolbar3 = toolbar[2];

            toolbar1.AddSpeedBtn(KnownButton.New);
            toolbar1.AddSpeedBtn(KnownButton.Open);
            
            var toolSave = toolbar1.AddSpeedBtn(KnownButton.Save);
            toolbar1.SetToolAlignRight(toolSave, true);

            var toolUndo = toolbar2.AddSpeedBtn(KnownButton.Undo);
            toolbar2.SetToolAlignCenter(toolUndo, true);
            var toolRedo = toolbar2.AddSpeedBtn(KnownButton.Redo);
            toolbar2.SetToolAlignCenter(toolRedo, true);
            toolbar2.TextVisible = true;

            toolbar3.AddSpeedBtn(KnownButton.Bold);
            toolbar3.AddSpeedBtn(KnownButton.Italic);
            toolbar3.AddSpeedBtn(KnownButton.Underline);
            toolbar3.ImageToText = ImageToText.Vertical;
            toolbar3.TextVisible = true;
            toolbar3.Visible = false;
        }

        public static void InitGenericToolBar(object control)
        {
            if (control is not ToolBar toolbar)
                return;
            InitGenericToolBar(toolbar);
        }

        public static void InitGenericToolBar(ToolBar toolbar, bool onlyButtons = false)
        {
            toolbar.Margin = (0, 0, 0, 4);

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

            menu.Add("Item 1").ClickAction = ()=> { App.Log("Item 1 clicked"); };
            menu.Add("Item 2").ClickAction = () => { App.Log("Item 2 clicked"); };

            toolbar.SetToolDropDownMenu(buttonIdOpen, menu);

            var separatorId = toolbar.AddSeparator();

            var saveClone = KnownSvgImages.ImgFileSave.Clone();
            saveClone.SetColorOverride(KnownSvgColor.Normal, isDark: true, Color.Yellow);
            saveClone.SetColorOverride(KnownSvgColor.Normal, false, Color.Red);

            var buttonIdSave = toolbar.AddSpeedBtn(
                CommonStrings.Default.ButtonSave,
                saveClone);
            toolbar.AddToolAction(buttonIdSave, ButtonClick);

            toolbar.AddSpacer();

            var idText = toolbar.AddText("text");
            toolbar.AddToolAction(idText, ButtonClick);

            if (!onlyButtons)
            {
                var textBox = new TextBox();
                textBox.VerticalAlignment = VerticalAlignment.Center;
                textBox.Text = "text1";
                textBox.MinWidth = 100;
                textBox.HorizontalAlignment = HorizontalAlignment.Fill;

                var idEdit = toolbar.AddControl(textBox);
            }

            var itemPicture = toolbar.AddPicture(
                KnownSvgImages.ImgMessageBoxWarning,
                "Picture");
            toolbar.AddToolAction(itemPicture, ButtonClick);
            toolbar.SetToolAlignRight(itemPicture, true);

            var buttonIdMoreItems = toolbar.AddSpeedBtn(KnownButton.MoreItems);
            toolbar.AddToolAction(buttonIdMoreItems, ButtonClick);
            toolbar.SetToolAlignRight(buttonIdMoreItems, true);

            static void ButtonClick(object? sender, EventArgs e)
            {
                if (sender is not AbstractControl button)
                    return;
                App.Log($"Button click: {button.ToolTip}");
            }
        }
    }
}
