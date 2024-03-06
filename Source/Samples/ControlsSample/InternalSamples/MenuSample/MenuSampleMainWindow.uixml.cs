using Alternet.Drawing;
using Alternet.UI;
using System;
using System.Linq;
using System.Security.Policy;

namespace MenuSample
{
    public partial class MainWindow : Window
    {
        private const string ResPrefix = "embres:ControlsSample.Resources.ToolBarPng.Small.";
        private readonly int dynamicToolbarItemsSeparatorIndex;
        private int newItemIndex = 0;
        ToolBar? toolbar;
        ToolBarItem? dynamicToolbarItemsSeparator;
        ToolBarItem? checkableToolbarItem;
        private readonly bool IsDebugBackground = false;

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
            InitializeComponent();

            InitToolbar();

            SaveCommand = new Command(o => LogEvent("Save"), o => saveEnabledMenuItem.Checked);
            ExportToPngCommand = new Command(o => LogEvent("Export to PNG"));
            DataContext = this;

            PlatformSpecificInitialize();
            UpdateControls();

            dynamicToolbarItemsSeparatorIndex =
                toolbar!.Items.IndexOf(dynamicToolbarItemsSeparator!);
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

            var images = KnownSvgImages.GetForSize(GetSvgColor(KnownSvgColor.Normal), 16);
            var imagesDisabled = KnownSvgImages.GetForSize(GetSvgColor(KnownSvgColor.Disabled), 16);

            openMenuItem.Image = images.ImgFileOpen;
            openMenuItem.Image = imagesDisabled.ImgFileOpen;

            saveMenuItem.Image = images.ImgFileSave;
            saveMenuItem.Image = imagesDisabled.ImgFileSave;

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

        private void ImageTextVertical()
        {
            if (toolbar == null)
                return;
            if (toolbar.IsVertical || toolbar.IsRight)
            {
                imageToTextDisplayModeComboBox.SelectedItem =
                    ImageToText.Vertical;
                toolbar.ImageToText = ImageToText.Vertical;
            }
        }

        internal void SetDebugBackground(Control control)
        {
            if (!IsDebugBackground)
                return;
            control.Background = new SolidBrush(Color.Purple);
            LayoutFactory.SetDebugBackgroundToParents(control);
        }

        private void MainWindow_Closing(object? sender, WindowClosingEventArgs e)
        {
        }

        private void InitToolbar()
        {
            toolbar = (ToolBar as ToolBar) ?? new();

            var calendarToolbarItem = new ToolBarItem("Calendar", ToolbarItem_Click)
            {
                ToolTip = "Calendar Toolbar Item",
                Image = ImageSet.FromUrl($"{ResPrefix}Calendar16.png")
            };

            var photoToolbarItem = new ToolBarItem("Photo", ToolbarItem_Click)
            {
                ToolTip = "Photo Toolbar Item",
                Image = ImageSet.FromUrl($"{ResPrefix}Photo16.png")
            };

            checkableToolbarItem =
                new ToolBarItem("Pencil", ToggleToolbarItem_Click)
                {
                    ToolTip = "Pencil Toolbar Item",
                    IsCheckable = true,
                    Image = ImageSet.FromUrl($"{ResPrefix}Pencil16.png")
                };

            toolbar?.Items.Add(calendarToolbarItem);
            toolbar?.Items.Add(photoToolbarItem);
            toolbar?.Items.Add(new ToolBarItem("-"));
            toolbar?.Items.Add(checkableToolbarItem);
            toolbar?.Items.Add(new ToolBarItem("-"));

            var graphDropDownToolbarItem =
                new ToolBarItem("Graph", ToolbarItem_Click)
                {
                    ToolTip = "Graph Toolbar Item",
                    Image = ImageSet.FromUrl($"{ResPrefix}LineGraph16.png")
                };

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
            graphDropDownToolbarItem.DropDownMenu = contextMenu;

            toolbar?.Items.Add(graphDropDownToolbarItem);

            dynamicToolbarItemsSeparator = new ToolBarItem("-");
            toolbar?.Items.Add(dynamicToolbarItemsSeparator);
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
                Menu!.Items.Add(dynamicItemsMenuItem);
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
                Menu!.Items.Remove(parent);
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
            new ExampleContextMenu().Show(contextMenuBorder, e.GetPosition(contextMenuBorder));
        }

        private void ToolbarItem_Click(object? sender, EventArgs e)
        {
            LogEvent("Toolbar item clicked: " + ((ToolBarItem)sender!).Text);
        }

        private void ToggleToolbarItem_Click(object? sender, EventArgs e)
        {
            ToolBarItem? item = sender as ToolBarItem;
            LogEvent($"Toggle toolbar item clicked: {item?.Text}. Is checked: {item?.Checked}");
        }

        private int lastEventNumber = 1;

        void LogEvent(string? message)
        {
            Application.Log(message);
        }

        private void ToggleToolbarItemCheckButton_Click(object? sender, EventArgs e)
        {
            checkableToolbarItem!.Checked = !checkableToolbarItem.Checked;
        }

        private void ToggleFirstToolbarEnabledButton_Click(object? sender, EventArgs e)
        {
            var item = toolbar?.Items[0];
            item!.Enabled = !item.Enabled;
        }

        private void AddDynamicToolbarItem()
        {
            int number = GenItemIndex();

            string text = "Item " + number;
            var item = new ToolBarItem(text)
            {
                Image = toolbar!.Items[0].Image,
                ToolTip = text + " Description"
            };

            item.Click += ToolbarItem_Click;
            toolbar.Items.Add(item);
        }

        private void AddDynamicToolbarItemButton_Click(object? sender, EventArgs e)
        {
            AddDynamicToolbarItem();
        }

        private void RemoveLastDynamicToolbarItemButton_Click(object? sender, EventArgs e)
        {
            if (toolbar!.Items.Count == dynamicToolbarItemsSeparatorIndex + 1)
                return;
            toolbar.Items.RemoveAt(toolbar.Items.Count - 1);
        }

        private void ShowToolbarTextCheckBox_CheckedChanged(
            object? sender,
            EventArgs e)
        {
            if (toolbar != null)
                toolbar.ItemTextVisible = showToolbarTextCheckBox.IsChecked;
        }

        private void IsRightCheckBox_Changed(object? sender, EventArgs e)
        {
            if (toolbar != null)
                toolbar.IsRight = isRightCheckBox.IsChecked;
            ImageTextVertical();
        }

        private void IsBottomCheckBox_Changed(object? sender, EventArgs e)
        {
            if (toolbar != null)
                toolbar.IsBottom = isBottomCheckBox.IsChecked;
        }

        private void NoDividerCheckBox_Changed(object? sender, EventArgs e)
        {
            if (toolbar != null)
                toolbar.NoDivider = noDividerCheckBox.IsChecked;
        }

        private void VerticalCheckBox_Changed(object? sender, EventArgs e)
        {
            if (toolbar != null)
                toolbar.IsVertical = verticalCheckBox.IsChecked;
            ImageTextVertical();
        }

        private void ShowToolbarImagesCheckBox_CheckedChanged(
            object? sender,
            EventArgs e)
        {
            if (toolbar != null)
                toolbar.ItemImagesVisible = showToolbarImagesCheckBox.IsChecked;
        }

        private void ToolbarDropDownMenuItem_Click(object? sender, EventArgs e)
        {
            MenuItem? item = sender as MenuItem;
            LogEvent($"Toolbar drop down menu item clicked: {item?.Text.Replace("_", "")}.");
        }

        private void ImageToTextDisplayModeComboBox_SelectedItemChanged(object sender, EventArgs e)
        {
            toolbar!.ImageToText = (ImageToText)imageToTextDisplayModeComboBox.SelectedItem!;
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