using System;
using Alternet.UI;
using Alternet.UI.Localization;

namespace Alternet.UI.Documentation.Examples
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var mainPanel = InitSplittedPanel();
            mainPanel.Parent = this;
        }

        #region CSharpCreation
        public static SplittedPanel InitSplittedPanel()
        {
            SplittedPanel panel = new();

            ListBox LeftLabel = new()
            {
                Parent = panel.LeftPanel,
                HasBorder = false,
            };
            LeftLabel.Add("Left");

            ListBox RightLabel = new()
            {
                Parent = panel.RightPanel,
                HasBorder = false,
            };
            RightLabel.Add("Right");

            GenericToolBar toolbar = new();
            toolbar.Parent = panel.TopPanel;
            InitGenericToolBar(toolbar);
            panel.TopSplitter.Visible = false;
            panel.TopPanel.Height = toolbar.ItemSize + 6;

            ListBox BottomLabel = new()
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

            menu.Add("Item 1").ClickAction = () => { Application.Log("Item 1 clicked"); };
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