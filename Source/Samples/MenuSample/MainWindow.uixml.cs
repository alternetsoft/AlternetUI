using Alternet.Drawing;
using Alternet.UI;
using System;
using System.Linq;
using System.Security.Policy;

namespace MenuSample
{
    public partial class MainWindow : Window
    {
        private const string ResPrefix = "embres:MenuSample.Resources.Icons.Small.";
        private int newItemIndex = 0;
        Toolbar? toolbar;
        ToolbarItem? dynamicToolbarItemsSeparator;
        ToolbarItem? checkableToolbarItem;
        private readonly bool IsDebugBackground = false;

        public MainWindow()
        {
            Icon = ImageSet.FromUrlOrNull("embres:MenuSample.Sample.ico");
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

            clockStatusBarPanelIndex = statusBar!.Panels.IndexOf(clockStatusBarPanel!);
            AddDynamicStatusBarPanel();

            foreach (var value in Enum.GetValues(typeof(ToolbarImageToText)))
                imageToTextDisplayModeComboBox.Items.Add(value!);
            imageToTextDisplayModeComboBox.SelectedItem = 
                ToolbarImageToText.Horizontal;

            clockTimer = new Timer(TimeSpan.FromMilliseconds(200), TimerEvent);
            clockTimer.Start();

            this.Closing += MainWindow_Closing;

            contextMenuBorder.BorderColor = Color.Red;
            contextMenuBorder.BorderWidth = new Thickness(0, 1, 0, 1);

            contextMenuLabel.Font = Control.DefaultFont.AsBold;
            contextMenuBorder.PerformLayout();
        }

        private void ImageTextVertical()
        {
            if (toolbar == null)
                return;
            if(toolbar.IsVertical || toolbar.IsRight)
            {
                imageToTextDisplayModeComboBox.SelectedItem = 
                    ToolbarImageToText.Vertical;
                toolbar.ImageToText = ToolbarImageToText.Vertical;
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
            clockTimer.Stop();
        }

        private void TimerEvent(object? sender, EventArgs e)
        {
            if (this.IsDisposed || statusBar!.IsDisposed)
                return;
            clockStatusBarPanel.Text = System.DateTime.Now.ToString("HH:mm:ss");
        }

        readonly Timer clockTimer;
        readonly int dynamicToolbarItemsSeparatorIndex;
        readonly int clockStatusBarPanelIndex;

        private void InitToolbar()
        {
            toolbar = Toolbar;

            var calendarToolbarItem = new ToolbarItem("Calendar", ToolbarItem_Click)
            {
                ToolTip = "Calendar Toolbar Item",
                Image = ImageSet.FromUrl($"{ResPrefix}Calendar16.png")
            };

            var photoToolbarItem = new ToolbarItem("Photo", ToolbarItem_Click)
            {
                ToolTip = "Photo Toolbar Item",
                Image = ImageSet.FromUrl($"{ResPrefix}Photo16.png")
            };

            checkableToolbarItem =
                new ToolbarItem("Pencil", ToggleToolbarItem_Click)
                {
                    ToolTip = "Pencil Toolbar Item",
                    IsCheckable = true,
                    Image = ImageSet.FromUrl($"{ResPrefix}Pencil16.png")
                };

            toolbar?.Items.Add(calendarToolbarItem);
            toolbar?.Items.Add(photoToolbarItem);
            toolbar?.Items.Add(new ToolbarItem("-"));
            toolbar?.Items.Add(checkableToolbarItem);
            toolbar?.Items.Add(new ToolbarItem("-"));

            var graphDropDownToolbarItem =
                new ToolbarItem("Graph", ToolbarItem_Click)
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

            dynamicToolbarItemsSeparator = new ToolbarItem("-");
            toolbar?.Items.Add(dynamicToolbarItemsSeparator);

            Application.Current.LogMessage += Current_LogMessage;
        }

        private void Current_LogMessage(object? sender, LogMessageEventArgs e)
        {
            LogEvent(e.Message);
        }

        private void PlatformSpecificInitialize()
        {
            roleControlsPanel.Visible = Application.IsMacOs;
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
        }

        private void PageScrollingMenuItem_Click(object? sender, EventArgs e)
        {
            continousScrollingMenuItem.Checked = false;
            pageScrollingMenuItem.Checked = true;
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

        private void ContextMenuBorder_MouseRightButtonUp(object? sender, MouseButtonEventArgs e)
        {
            new ExampleContextMenu().Show(contextMenuBorder, e.GetPosition(contextMenuBorder));
        }

        private void ToolbarItem_Click(object? sender, EventArgs e)
        {
            LogEvent("Toolbar item clicked: " + ((ToolbarItem)sender!).Text);
        }

        private void ToggleToolbarItem_Click(object? sender, EventArgs e)
        {
            ToolbarItem? item = sender as ToolbarItem;
            LogEvent($"Toggle toolbar item clicked: {item?.Text}. Is checked: {item?.Checked}");
        }

        private int lastEventNumber = 1;

        void LogEvent(string? message)
        {
            if (message is null)
                return;
            eventsListBox.Items.Add($"{lastEventNumber++}. {message}");
            eventsListBox.SelectedIndex = eventsListBox.Items.Count - 1;
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
            var item = new ToolbarItem(text)
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
            toolbar!.ImageToText = (ToolbarImageToText)imageToTextDisplayModeComboBox.SelectedItem!;
        }

        private void AddDynamicStatusBarPanelButton_Click(object sender, System.EventArgs e)
        {
            AddDynamicStatusBarPanel();
        }

        private void RemoveLastDynamicStatusBarPanelButton_Click(object? sender, System.EventArgs e)
        {
            if (statusBar!.Panels.Count == clockStatusBarPanelIndex + 1)
                return;
            statusBar.Panels.RemoveAt(statusBar.Panels.Count - 1);
        }

        private void AddDynamicStatusBarPanel()
        {
            int number = GenItemIndex();
            string text = "Panel " + number;
            statusBar.Panels.Add(new StatusBarPanel(text));
        }

        private int GenItemIndex()
        {
            newItemIndex++;
            return newItemIndex;
        }

        private void ShowSizingGripCheckBox_CheckedChanged(object? sender, System.EventArgs e)
        {
            if(statusBar!=null)
                statusBar.SizingGripVisible = showSizingGripCheckBox.IsChecked;
        }
    }
}