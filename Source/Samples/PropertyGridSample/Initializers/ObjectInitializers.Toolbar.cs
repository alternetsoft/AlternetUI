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
    internal partial class ObjectInitializers
    {
        public static void InitGenericToolBar(object control)
        {
            if (control is not GenericToolBar toolbar)
                return;

            var imageSize = Toolbar.GetDefaultImageSize(toolbar);

            var normalColor = toolbar.GetSvgColor(KnownSvgColor.Normal);
            var disabledColor = toolbar.GetSvgColor(KnownSvgColor.Disabled);

            var images = KnownSvgImages.GetForSize(normalColor, imageSize);
            var imagesDisabled = KnownSvgImages.GetForSize(disabledColor, imageSize);

            var buttonIdNew = toolbar.AddSpeedBtn(
                CommonStrings.Default.ButtonNew,
                images.ImgFileNew,
                imagesDisabled.ImgFileNew);
            toolbar.AddToolAction(buttonIdNew, ButtonClick);
            toolbar.SetToolShortcut(buttonIdNew, Keys.Control | Keys.N);

            var buttonIdOpen = toolbar.AddSpeedBtn(
                CommonStrings.Default.ButtonOpen,
                images.ImgFileOpen,
                imagesDisabled.ImgFileOpen);
            toolbar.AddToolAction(buttonIdOpen, ButtonClick);
            toolbar.SetToolShortcut(buttonIdOpen, Keys.Control | Keys.O);

            ContextMenu? menu = new();

            menu.Add("Item 1").ClickAction = ()=> { Application.Log("Item 1 clicked"); };
            menu.Add("Item 2").ClickAction = () => { Application.Log("Item 2 clicked"); };

            toolbar.SetToolDropDownMenu(buttonIdOpen, menu);

            var separatorId = toolbar.AddSeparator();

            var buttonIdSave = toolbar.AddSpeedBtn(
                CommonStrings.Default.ButtonSave,
                images.ImgFileSave,
                imagesDisabled.ImgFileSave);
            toolbar.AddToolAction(buttonIdSave, ButtonClick);

            toolbar.AddSpacer();

            var idText = toolbar.AddText("text");
            toolbar.AddToolAction(idText, ButtonClick);

            var textBox = new TextBox();
            textBox.SuggestedWidth = 100;

            var idEdit = toolbar.AddControl(textBox);

            var itemPicture = toolbar.AddPicture(
                images.ImgMessageBoxWarning,
                imagesDisabled.ImgMessageBoxWarning,
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
