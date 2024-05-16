using Alternet.Drawing;
using Alternet.UI;
using System;
using System.Linq;
using System.Security.Policy;

namespace MenuSample
{
    public partial class MainWindow : Window
    {
        private const string ResPrefix = "embres:ControlsSample.Resources.ToolBarPng.Large.";

        private readonly string Calendar16Url = $"{ResPrefix}Calendar32.png";
        private readonly int dynamicToolbarItemsSeparatorIndex;
        private readonly bool IsDebugBackground = false;
        private readonly ToolBar toolbar = new();

        private int newItemIndex = 0;

        ObjectUniqueId checkableToolbarItem;
        ObjectUniqueId graphDropDownToolbarItem;

        static MainWindow()
        {
        }

        public void ExportToPngCommand_Click(object? sender, EventArgs e)
        {
            ExportToPngCommand?.Execute(null);
        }

        public void SaveCommand_Click(object? sender, EventArgs e)
        {
            SaveCommand?.Execute(null);
        }

        public MainWindow()
        {
            Icon = Application.DefaultIcon;
            Layout = LayoutStyle.Vertical;

            toolbar.TextVisible = true;
            toolbar.ItemSize = 32;
            toolbar.Margin = (0, 0, 0, 4);

            toolbar.Parent = this;

            InitializeComponent();

            noDividerCheckBox.Enabled = false;
            verticalCheckBox.Enabled = false;
            isRightCheckBox.Enabled = false;
            isBottomCheckBox.Enabled = false;
            imageToTextDisplayModeComboBox.Enabled = false;

            InitToolbar();

            SaveCommand = new Command(o => LogEvent("Save"), o => saveEnabledMenuItem.Checked);
            ExportToPngCommand = new Command(o => LogEvent("Export to PNG"));
            DataContext = this;

            PlatformSpecificInitialize();
            UpdateControls();

            dynamicToolbarItemsSeparatorIndex = toolbar.Children.Count;

            AddDynamicToolbarItem();

            foreach (var value in Enum.GetValues(typeof(ImageToText)))
                imageToTextDisplayModeComboBox.Items.Add(value!);
            imageToTextDisplayModeComboBox.SelectedItem =
                ImageToText.Horizontal;

            this.Closing += MainWindow_Closing;

            contextMenuBorder.BorderColor = Color.Red;
            contextMenuBorder.BorderWidth = new Thickness(2, 2, 2, 2);

            contextMenuLabel.Font = Control.DefaultFont.AsBold;
            contextMenuBorder.PerformLayout();

            mainPanel.TabAlignment = TabAlignment.Left;

            statusClearButton.Click += StatusClearButton_Click;
            statusNullButton.Click += StatusNullButton_Click;
            statusRecreateButton.Click += StatusRecreateButton_Click;
            statusAddButton.Click += StatusAddButton_Click;
            statusRemoveButton.Click += StatusRemoveButton_Click;
            statusEditButton.Click += StatusEditButton_Click;

            eventsListBox.ContextMenu.Required();

            openMenuItem.Image = KnownSvgImages.ImgFileOpen.AsNormal(16, IsDarkBackground);
            saveMenuItem.Image = KnownSvgImages.ImgFileSave.AsNormal(16, IsDarkBackground);

            eventsListBox.BindApplicationLog();
        }
        private StatusBar? GetStatusBar() => StatusBar as StatusBar;

        private void StatusAddButton_Click(object? sender, System.EventArgs e)
        {
            StatusBar ??= new();
            GetStatusBar()?.Add($"Panel {GenItemIndex()}");
        }

        private void StatusRemoveButton_Click(object? sender, System.EventArgs e)
        {
            GetStatusBar()?.Panels.RemoveLast();
        }

        private void StatusRecreateButton_Click(object? sender, EventArgs e)
        {
            StatusBar = new StatusBar();
            GetStatusBar()?.Add($"Panel {GenItemIndex()}");
            GetStatusBar()?.Add($"Panel {GenItemIndex()}");
        }

        private void StatusNullButton_Click(object? sender, EventArgs e)
        {
            StatusBar = null;
        }

        private void StatusEditButton_Click(object? sender, EventArgs e)
        {
            StatusBar ??= new();
            DialogFactory.EditItemsWithListEditor(GetStatusBar());
        }

        private void StatusClearButton_Click(object? sender, EventArgs e)
        {
            GetStatusBar()?.Panels.Clear();
        }

        internal void SetDebugBackground(Control control)
        {
            if (!IsDebugBackground)
                return;
            control.Background = new SolidBrush(Color.Purple);
            DrawingUtils.SetDebugBackgroundToParents(control);
        }

        private void MainWindow_Closing(object? sender, WindowClosingEventArgs e)
        {
        }

        private void InitToolbar()
        {
            ObjectUniqueId calendarToolbarItem;
            ObjectUniqueId photoToolbarItem;
            ObjectUniqueId dynamicToolbarItemsSeparator;

            calendarToolbarItem = toolbar.AddSpeedBtn(
                "Calendar",
                ImageSet.FromUrl(Calendar16Url),
                null,
                "Calendar Toolbar Item",
                ToolbarItem_Click);

            photoToolbarItem = toolbar.AddSpeedBtn(
                "Photo",
                ImageSet.FromUrl($"{ResPrefix}Photo32.png"),
                null,
                "Photo Toolbar Item",
                ToolbarItem_Click);

            toolbar.AddSeparator();

            checkableToolbarItem = toolbar.AddStickyBtn(
                "Pencil",
                ImageSet.FromUrl($"{ResPrefix}Pencil32.png"),
                null,
                "Pencil Toolbar Item",
                ToggleToolbarItem_Click);
            toolbar.SetToolSticky(checkableToolbarItem, true);

            toolbar.AddSeparator();

            graphDropDownToolbarItem = toolbar.AddSpeedBtn(
                "Graph",
                ImageSet.FromUrl($"{ResPrefix}LineGraph32.png"),
                null,
                "Graph Toolbar Item",
                ToolbarItem_Click);

            var contextMenu = new ContextMenu();

            MenuItem openToolbarMenuItem =
                new("_Open...", ToolbarDropDownMenuItem_Click);
            MenuItem saveToolbarMenuItem =
                new("_Save...", ToolbarDropDownMenuItem_Click);
            MenuItem exportToolbarMenuItem =
                new("E_xport...", ToolbarDropDownMenuItem_Click);
            contextMenu.Items.Add(openToolbarMenuItem);
            contextMenu.Items.Add(saveToolbarMenuItem);
            contextMenu.Items.Add(exportToolbarMenuItem);

            toolbar.SetToolDropDownMenu(graphDropDownToolbarItem, contextMenu);
            
            dynamicToolbarItemsSeparator = toolbar.AddSeparator();
        }

        private void PlatformSpecificInitialize()
        {
            roleControlsPanel.Visible = Application.IsMacOS;
        }

        public Command? SaveCommand { get; private set; }
        public Command? ExportToPngCommand { get; private set; }

        private void OpenMenuItem_Click(object? sender, EventArgs e) =>
            LogEvent("Open");

        private void SaveEnabledMenuItem_Click(object? sender, EventArgs e) =>
            SaveCommand!.RaiseCanExecuteChanged();

        private void ExportToPdfMenuItem_Click(object? sender, EventArgs e) =>
            LogEvent("Export to PDF");

        private void ExportToPngMenuItem_Click(object? sender, EventArgs e) =>
            LogEvent("Export to PNG");

        private void OptionsMenuItem_Click(object? sender, EventArgs e) => LogEvent("Options");

        private void ExitMenuItem_Click(object? sender, EventArgs e) => Close();

        private void AboutMenuItem_Click(object? sender, EventArgs e) =>
            LogEvent("AlterNET UI Menu Sample Application.");

        MenuItem? dynamicItemsMenuItem;

        MenuItem EnsureDynamicItemsMenuItem()
        {
            if (dynamicItemsMenuItem == null)
            {
                dynamicItemsMenuItem = new MenuItem("Dynamic Items");
                (Menu as Menu)?.Items.Add(dynamicItemsMenuItem);
            }

            return dynamicItemsMenuItem;
        }

        private void AddDynamicMenuItemMenuItem_Click(object? sender, EventArgs e)
        {
            var parent = EnsureDynamicItemsMenuItem();

            var number = GenItemIndex();
            parent.Items.Add(new MenuItem("Item " + number, DynamicMenuItem_Click));
        }

        private void DynamicMenuItem_Click(object? sender, EventArgs e)
        {
            var item = (MenuItem)sender!;
            LogEvent("Dynamic item clicked: " + item.Text);
        }

        private void RemoveLastDynamicMenuItemMenuItem_Click(object? sender, EventArgs e)
        {
            var parent = dynamicItemsMenuItem;
            if (parent == null)
                return;

            if (parent.Items.Count > 0)
            {
                parent.Items.RemoveAt(parent.Items.Count - 1);
            }

            if (parent.Items.Count == 0)
            {
                (Menu as Menu)?.Items.Remove(parent);
                dynamicItemsMenuItem = null;
            }
        }

        private void ToggleExitEnabledMenuItem_Click(object? sender, EventArgs e)
        {
            exitMenuItem.Enabled = !exitMenuItem.Enabled;
            UpdateControls();
        }

        private void UpdateControls()
        {
            toggleExitEnabledMenuItem.Text =
                (exitMenuItem.Enabled ? "Disable" : "Enable") + " Exit Menu Item";
            toggleSeparatorMenuItem.Text = separatorMenuItem.Text == "-" ?
                "Separator item to normal" : "Normal item to separator";
        }

        private void ContinousScrollingMenuItem_Click(object? sender, EventArgs e)
        {
            continousScrollingMenuItem.Checked = true;
            pageScrollingMenuItem.Checked = false;
            LogEvent("Continous Scrolling Clicked");
        }

        private void PageScrollingMenuItem_Click(object? sender, EventArgs e)
        {
            continousScrollingMenuItem.Checked = false;
            pageScrollingMenuItem.Checked = true;
            LogEvent("Page Scrolling Clicked");
        }

        private void GridMenuItem_Click(object sender, EventArgs e)
        {
            LogEvent("Grid item is checked: " + ((MenuItem)sender).Checked);
        }

        private void ToggleSeparatorMenuItem_Click(object? sender, System.EventArgs e)
        {
            if (separatorMenuItem.Text == "-")
                separatorMenuItem.Text = "[Text instead of separator]";
            else
                separatorMenuItem.Text = "-";

            UpdateControls();
        }

        private void SetAboutMenuItemRoleToNone_Click(object? sender, EventArgs e)
        {
            aboutMenuItem.Role = MenuItemRoles.None;
        }

        private void SetOptionsMenuItemRoleToNone_Click(object? sender, EventArgs e)
        {
            optionsMenuItem.Role = MenuItemRoles.None;
        }

        private void SetOptionsMenuItemRoleToPreferences_Click(object? sender, EventArgs e)
        {
            optionsMenuItem.Role = MenuItemRoles.Preferences;
        }

        private void ContextMenuBorder_MouseRightButtonUp(object? sender, MouseEventArgs e)
        {
            new ExampleContextMenu().Show(contextMenuBorder, Mouse.GetPosition(contextMenuBorder));
        }

        private void ToolbarItem_Click(object? sender, EventArgs e)
        {
            if (sender is not Control button)
                return;
            LogEvent("Toolbar item clicked: " + button.ToolTip);
        }

        private void ToggleToolbarItem_Click(object? sender, EventArgs e)
        {
            if (sender is not Control button)
                return;
            toolbar.ToggleToolSticky(button.UniqueId);
            var sticky = toolbar.GetToolSticky(button.UniqueId);
            LogEvent($"Toggle toolbar item clicked: {button.ToolTip}. Is checked: {sticky}");
        }

        void LogEvent(string? message)
        {
            Application.Log(message);
        }

        private void ToggleToolbarItemCheckButton_Click(object? sender, EventArgs e)
        {
            toolbar.ToggleToolSticky(checkableToolbarItem);
        }

        private void ToggleFirstToolbarEnabledButton_Click(object? sender, EventArgs e)
        {
            var control = toolbar.Children[0];
            var enabled = toolbar.GetToolEnabled(control.UniqueId);
            enabled = !enabled;
            toolbar.SetToolEnabled(control.UniqueId, enabled);
        }

        private void AddDynamicToolbarItem()
        {
            int number = GenItemIndex();
            string text = "Item " + number;
            var image = toolbar.GetToolImage(toolbar.Children[0].UniqueId);
            var item = toolbar.AddSpeedBtn(
                text,
                image,
                null,
                text + " Description",
                ToolbarItem_Click);
        }

        private void AddDynamicToolbarItemButton_Click(object? sender, EventArgs e)
        {
            AddDynamicToolbarItem();
        }

        private void RemoveLastDynamicToolbarItemButton_Click(object? sender, EventArgs e)
        {
            var count = toolbar.Children.Count;
            if (count <= dynamicToolbarItemsSeparatorIndex)
                return;
            toolbar.DeleteTool(toolbar.Children[count - 1].UniqueId);
        }

        private void ShowToolbarTextCheckBox_CheckedChanged(
            object? sender,
            EventArgs e)
        {
            toolbar.TextVisible = showToolbarTextCheckBox.IsChecked;
        }

        /*private void ImageTextVertical()
        {
            if (toolbar == null)
                return;
            if (toolbar.IsVertical || toolbar.IsRight)
            {
                imageToTextDisplayModeComboBox.SelectedItem =
                    ImageToText.Vertical;
                toolbar.ImageToText = ImageToText.Vertical;
            }
        }*/

        private void IsRightCheckBox_Changed(object? sender, EventArgs e)
        {
            /*if (toolbar != null)
                toolbar.IsRight = isRightCheckBox.IsChecked;
            ImageTextVertical();*/
        }

        private void IsBottomCheckBox_Changed(object? sender, EventArgs e)
        {
            /*if (toolbar != null)
                toolbar.IsBottom = isBottomCheckBox.IsChecked;*/
        }

        private void NoDividerCheckBox_Changed(object? sender, EventArgs e)
        {
            /*if (toolbar != null)
                toolbar.NoDivider = noDividerCheckBox.IsChecked;*/
        }

        private void VerticalCheckBox_Changed(object? sender, EventArgs e)
        {
            /*if (toolbar != null)
                toolbar.IsVertical = verticalCheckBox.IsChecked;
            ImageTextVertical();*/
        }

        private void ShowToolbarImagesCheckBox_CheckedChanged(
            object? sender,
            EventArgs e)
        {
            toolbar.ImageVisible = showToolbarImagesCheckBox.IsChecked;
        }

        private void ToolbarDropDownMenuItem_Click(object? sender, EventArgs e)
        {
            MenuItem? item = sender as MenuItem;
            LogEvent($"Toolbar drop down menu item clicked: {item?.Text.Replace("_", "")}.");
        }

        private void ImageToTextDisplayModeComboBox_SelectedItemChanged(object sender, EventArgs e)
        {
            toolbar.ImageToText = (ImageToText)imageToTextDisplayModeComboBox.SelectedItem!;
        }

        private int GenItemIndex()
        {
            newItemIndex++;
            return newItemIndex;
        }

        private void ShowSizingGripButton_Click(object? sender, EventArgs e)
        {
            var s = GetStatusBar();

            if (s != null)
                s.SizingGripVisible = !s.SizingGripVisible;
        }
    }
}