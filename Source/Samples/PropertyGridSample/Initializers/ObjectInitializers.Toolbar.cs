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

            var buttonIdNew = toolbar.Add(
                CommonStrings.Default.ButtonNew,
                images.ImgFileNew,
                imagesDisabled.ImgFileNew,
                CommonStrings.Default.ButtonNew,
                ButtonClick);

            var buttonIdOpen = toolbar.Add(
                CommonStrings.Default.ButtonOpen,
                images.ImgFileOpen,
                imagesDisabled.ImgFileOpen,
                CommonStrings.Default.ButtonOpen,
                ButtonClick);

            ContextMenu? menu = new();

            menu.Add("Item 1").ClickAction = ()=> { Application.Log("Item 1 clicked"); };
            menu.Add("Item 2").ClickAction = () => { Application.Log("Item 2 clicked"); };

            toolbar.SetToolDropDownMenu(buttonIdOpen, menu);

            var separatorId = toolbar.AddSeparator();

            var buttonIdSave = toolbar.Add(
                CommonStrings.Default.ButtonSave,
                images.ImgFileSave,
                imagesDisabled.ImgFileSave,
                CommonStrings.Default.ButtonSave,
                ButtonClick);

            toolbar.AddSpacer();

            var idText = toolbar.AddText("text");

            var textBox = new TextBox();

            var idEdit = toolbar.AddControl(textBox);

            var buttonIdUndo = toolbar.Add(
                CommonStrings.Default.ButtonUndo,
                images.ImgUndo,
                imagesDisabled.ImgUndo,
                CommonStrings.Default.ButtonUndo,
                ButtonClick);

            /*var buttonIdRedo = toolbar.Add(
                CommonStrings.Default.ButtonRedo,
                images.ImgRedo,
                imagesDisabled.ImgRedo,
                CommonStrings.Default.ButtonRedo,
                ButtonClick);

            var buttonIdBold = toolbar.Add(
                CommonStrings.Default.ButtonBold,
                images.ImgBold,
                imagesDisabled.ImgBold,
                CommonStrings.Default.ButtonBold,
                ButtonClick);

            var buttonIdItalic = toolbar.Add(
                CommonStrings.Default.ButtonItalic,
                images.ImgItalic,
                imagesDisabled.ImgItalic,
                CommonStrings.Default.ButtonItalic,
                ButtonClick);

            var buttonIdUnderline = toolbar.Add(
                CommonStrings.Default.ButtonUnderline,
                images.ImgUnderline,
                imagesDisabled.ImgUnderline,
                CommonStrings.Default.ButtonUnderline,
                ButtonClick);*/

            void ButtonClick(object? sender, EventArgs e)
            {
                if (sender is not SpeedButton button)
                    return;
                Application.Log($"Button click: {button.ToolTip}");
            }
        }
    }
}
