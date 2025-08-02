using System;
using Alternet.UI;
using Alternet.UI.Localization;

namespace ApiDoc
{
    public partial class SplittedPanelWindow : Window
    {
        public SplittedPanelWindow()
        {
            InitializeComponent();
            var mainPanel = InitSplittedPanel();
            mainPanel.Parent = this;
        }

        #region CSharpCreation
        public static SplittedPanel InitSplittedPanel()
        {
            SplittedPanel panel = new();

            StdListBox LeftLabel = new()
            {
                Parent = panel.LeftPanel,
                HasBorder = false,
            };
            LeftLabel.Add("Left");

            StdListBox RightLabel = new()
            {
                Parent = panel.RightPanel,
                HasBorder = false,
            };
            RightLabel.Add("Right");

            ToolBar toolbar = new()
            {
                Parent = panel.TopPanel,
            };
            InitGenericToolBar(toolbar);
            panel.TopSplitter.Visible = false;
            panel.TopPanel.Height = toolbar.ItemSize + 6;

            StdListBox BottomLabel = new()
            {
                Parent = panel.BottomPanel,
                HasBorder = false,
            };
            BottomLabel.Add("Bottom");

            LogListBox FillLabel = new()
            {
                Parent = panel.FillPanel,
                HasBorder = false,
            };
            FillLabel.BindApplicationLog();

            panel.RightPanel.Width = 150;
            panel.LeftPanel.Width = 150;
            panel.BottomPanel.Height = 200;

            return panel;
        }
        #endregion

        public static void InitGenericToolBar(object control)
        {
            if (control is not ToolBar toolbar)
                return;

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
                SuggestedWidth = 100
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

            static void ButtonClick(object? sender, EventArgs e)
            {
                if (sender is not Control button)
                    return;
                App.Log($"Button click: {button.ToolTip}");
            }
        }
    }
}