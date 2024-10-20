using System;
using Alternet.UI;
using Alternet.UI.Localization;

namespace ApiDoc
{
    public partial class ToolBarWindow : Window
    {
        public ToolBarWindow()
        {
            InitializeComponent();
            Padding = 5;
            VerticalStackPanel mainPanel = new()
            {
                Parent = this,
            };
            var control = InitGenericToolBar();
            control.Parent = mainPanel;
            LogListBox listBox = new()
            {
                HasBorder = false,
                VerticalAlignment = VerticalAlignment.Fill,
                Parent = mainPanel,
            };
            listBox.BindApplicationLog();
        }

        #region CSharpCreation
        public static ToolBar InitGenericToolBar()
        {
            ToolBar toolbar = new()
            {
                Margin = (0, 0, 0, 4),
            };

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

            toolbar.SetToolDropDownMenu(buttonIdOpen, menu);

            var separatorId = toolbar.AddSeparator();

            var buttonIdSave = toolbar.AddSpeedBtn(
                CommonStrings.Default.ButtonSave,
                KnownSvgImages.ImgFileSave);
            toolbar.AddToolAction(buttonIdSave, ButtonClick);

            toolbar.AddSpacer();

            var idText = toolbar.AddText("text");
            toolbar.AddToolAction(idText, ButtonClick);

            var textBox = new TextBox
            {
                VerticalAlignment = VerticalAlignment.Center,
                SuggestedWidth = 300,
            };

            var idEdit = toolbar.AddControl(textBox);

            var itemPicture = toolbar.AddPicture(
                KnownSvgImages.ImgMessageBoxWarning,
                "Picture");
            toolbar.AddToolAction(itemPicture, ButtonClick);
            toolbar.SetToolAlignRight(itemPicture, true);

            var buttonIdMoreItems = toolbar.AddSpeedBtn(KnownButton.MoreItems);
            toolbar.AddToolAction(buttonIdMoreItems, ButtonClick);
            toolbar.SetToolAlignRight(buttonIdMoreItems, true);

            return toolbar;

            static void ButtonClick(object? sender, EventArgs e)
            {
                if (sender is not Control button)
                    return;
                App.Log($"Button click: {button.ToolTip}");
            }
        }
        #endregion
    }
}