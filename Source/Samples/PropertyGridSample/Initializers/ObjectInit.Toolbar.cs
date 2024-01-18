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
    internal partial class ObjectInit
    {
        public static void InitFindReplaceControl(object control)
        {
            if (control is not FindReplaceControl findReplace)
                return;

            findReplace.ReplaceVisible = true;
            findReplace.Manager = findReplace.CreateLogger();
        }

        public static void InitGenericToolBar(object control)
        {
            if (control is not GenericToolBar toolbar)
                return;

            toolbar.Margin = (0, 0, 0, 4);

            var buttonIdNew = toolbar.AddSpeedBtn(
                CommonStrings.Default.ButtonNew,
                toolbar.GetNormalSvgImages().ImgFileNew,
                toolbar.GetDisabledSvgImages().ImgFileNew);
            toolbar.AddToolAction(buttonIdNew, ButtonClick);
            toolbar.SetToolShortcut(buttonIdNew, Keys.Control | Keys.N);

            var buttonIdOpen = toolbar.AddSpeedBtn(
                CommonStrings.Default.ButtonOpen,
                toolbar.GetNormalSvgImages().ImgFileOpen,
                toolbar.GetDisabledSvgImages().ImgFileOpen);
            toolbar.AddToolAction(buttonIdOpen, ButtonClick);
            toolbar.SetToolShortcut(buttonIdOpen, Keys.Control | Keys.O);

            ContextMenu? menu = new();

            menu.Add("Item 1").ClickAction = ()=> { Application.Log("Item 1 clicked"); };
            menu.Add("Item 2").ClickAction = () => { Application.Log("Item 2 clicked"); };

            toolbar.SetToolDropDownMenu(buttonIdOpen, menu);

            var separatorId = toolbar.AddSeparator();

            var buttonIdSave = toolbar.AddSpeedBtn(
                CommonStrings.Default.ButtonSave,
                toolbar.GetNormalSvgImages().ImgFileSave,
                toolbar.GetDisabledSvgImages().ImgFileSave);
            toolbar.AddToolAction(buttonIdSave, ButtonClick);

            toolbar.AddSpacer();

            var idText = toolbar.AddText("text");
            toolbar.AddToolAction(idText, ButtonClick);

            var textBox = new TextBox();
            textBox.VerticalAlignment = VerticalAlignment.Center;
            textBox.SuggestedWidth = 100;

            var idEdit = toolbar.AddControl(textBox);

            var itemPicture = toolbar.AddPicture(
                toolbar.GetNormalSvgImages().ImgMessageBoxWarning,
                toolbar.GetDisabledSvgImages().ImgMessageBoxWarning,
                "Picture");
            toolbar.AddToolAction(itemPicture, ButtonClick);
            toolbar.SetToolAlignRight(itemPicture, true);

            var buttonIdMoreItems = toolbar.AddSpeedBtn(KnownButton.MoreItems);
            toolbar.AddToolAction(buttonIdMoreItems, ButtonClick);
            toolbar.SetToolAlignRight(buttonIdMoreItems, true);

            static void ButtonClick(object? sender, EventArgs e)
            {
                if (sender is not Control button)
                    return;
                Application.Log($"Button click: {button.ToolTip}");
            }
        }
    }
}
