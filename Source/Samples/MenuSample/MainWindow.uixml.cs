using Alternet.Drawing;
using Alternet.UI;
using System;
using System.Linq;
using System.Security.Policy;

namespace MenuSample
{
    public partial class MainWindow : Window
    {
        Toolbar toolbar;
        ToolbarItem dynamicToolbarItemsSeparator;
        ToolbarItem checkableToolbarItem;
        
        public MainWindow()
        {
            InitToolbar();

            InitializeComponent();
            
            SaveCommand = new Command(o => MessageBox.Show("Save"), o => saveEnabledMenuItem.Checked);
            ExportToPngCommand = new Command(o => MessageBox.Show("Export to PNG"));
            DataContext = this;

            PlatformSpecificInitialize();
            UpdateControls();

            dynamicToolbarItemsSeparatorIndex = toolbar.Items.IndexOf(dynamicToolbarItemsSeparator);
            AddDynamicToolbarItem();

            clockStatusBarPanelIndex = statusBar.Panels.IndexOf(clockStatusBarPanel);
            AddDynamicStatusBarPanel();

            foreach (var value in Enum.GetValues(typeof(ToolbarItemImageToTextDisplayMode)))
                imageToTextDisplayModeComboBox.Items.Add(value!);
            imageToTextDisplayModeComboBox.SelectedItem = ToolbarItemImageToTextDisplayMode.Horizontal;

            clockTimer = new Timer(TimeSpan.FromMilliseconds(200), (o, e) => clockStatusBarPanel.Text = System.DateTime.Now.ToString("HH:mm:ss"));
            clockTimer.Start();
        }

        Timer clockTimer;

        int dynamicToolbarItemsSeparatorIndex;
        int clockStatusBarPanelIndex;

        private void InitToolbar()
        {
            toolbar = new ();

            toolbar.BeginUpdate();

            var calendarToolbarItem = new ToolbarItem("Calendar", ToolbarItem_Click);
            calendarToolbarItem.ToolTip = "Calendar Toolbar Item";
            calendarToolbarItem.Image = ImageSet.FromUrl("embres:MenuSample.Resources.Icons.Small.Calendar16.png");
            toolbar.Items.Add(calendarToolbarItem);

            var photoToolbarItem = new ToolbarItem("Photo");
            photoToolbarItem.ToolTip = "Photo Toolbar Item";
            photoToolbarItem.Image = ImageSet.FromUrl(
                             "embres:MenuSample.Resources.Icons.Small.Photo16.png");
            photoToolbarItem.Click += ToolbarItem_Click;
            toolbar.Items.Add(photoToolbarItem);

            toolbar.Items.Add(new ToolbarItem("-"));

            checkableToolbarItem = new ToolbarItem("Pencil Toggle", ToggleToolbarItem_Click);
            checkableToolbarItem.ToolTip = "Pencil Toolbar Item";
            checkableToolbarItem.IsCheckable = true;
            checkableToolbarItem.Image = ImageSet.FromUrl(
                             "embres:MenuSample.Resources.Icons.Small.Pencil16.png");
            toolbar.Items.Add(checkableToolbarItem);

            toolbar.Items.Add(new ToolbarItem("-"));

            var graphDropDownToolbarItem = new ToolbarItem("Graph Drop Down", ToolbarItem_Click);
            graphDropDownToolbarItem.ToolTip = "Graph Toolbar Item";
            graphDropDownToolbarItem.Image = ImageSet.FromUrl(
                             "embres:MenuSample.Resources.Icons.Small.LineGraph16.png");

            var contextMenu = new ContextMenu();

            MenuItem openToolbarMenuItem = new("_Open...", ToolbarDropDownMenuItem_Click);
            MenuItem saveToolbarMenuItem = new("_Save...", ToolbarDropDownMenuItem_Click);
            MenuItem exportToolbarMenuItem = new("E_xport...", ToolbarDropDownMenuItem_Click);
            contextMenu.Items.Add(openToolbarMenuItem);
            contextMenu.Items.Add(saveToolbarMenuItem);
            contextMenu.Items.Add(exportToolbarMenuItem);
            graphDropDownToolbarItem.DropDownMenu = contextMenu;

            toolbar.Items.Add(graphDropDownToolbarItem);

            dynamicToolbarItemsSeparator = new ToolbarItem("-");
            toolbar.Items.Add(dynamicToolbarItemsSeparator);

            toolbar.EndUpdate();

            Toolbar = toolbar;


        }

        private void PlatformSpecificInitialize()
        {
            bool runningUnderMacOS = WebBrowser.GetBackendOS() == WebBrowserBackendOS.MacOS;
            roleControlsPanel.Visible = runningUnderMacOS;
        }

        public Command? SaveCommand { get; private set; }
        public Command? ExportToPngCommand { get; private set; }

        private void OpenMenuItem_Click(object sender, EventArgs e) => MessageBox.Show("Open");

        private void SaveEnabledMenuItem_Click(object sender, EventArgs e) => SaveCommand!.RaiseCanExecuteChanged();

        private void ExportToPdfMenuItem_Click(object sender, EventArgs e) => MessageBox.Show("Export to PDF");

        private void ExportToPngMenuItem_Click(object sender, EventArgs e) => MessageBox.Show("Export to PNG");

        private void OptionsMenuItem_Click(object sender, EventArgs e) => MessageBox.Show("Options");

        private void ExitMenuItem_Click(object sender, EventArgs e) => Close();

        private void AboutMenuItem_Click(object sender, EventArgs e) => MessageBox.Show("AlterNET UI Menu Sample Application.", "About");

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

        private void AddDynamicMenuItemMenuItem_Click(object sender, EventArgs e)
        {
            var parent = EnsureDynamicItemsMenuItem();

            int GetNextItemNumber()
            {
                if (parent.Items.Count == 0)
                    return 1;

                return (int)parent.Items.Last().Tag! + 1;
            }

            var number = GetNextItemNumber();
            parent.Items.Add(new MenuItem("Item " + number, DynamicMenuItem_Click) { Tag = number });
        }

        private void DynamicMenuItem_Click(object? sender, EventArgs e)
        {
            var item = (MenuItem)sender!;
            MessageBox.Show("Dynamic item clicked: " + item.Text);
        }

        private void RemoveLastDynamicMenuItemMenuItem_Click(object sender, EventArgs e)
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

        private void ToggleExitEnabledMenuItem_Click(object sender, EventArgs e)
        {
            exitMenuItem.Enabled = !exitMenuItem.Enabled;
            UpdateControls();
        }

        private void UpdateControls()
        {
            toggleExitEnabledMenuItem.Text = (exitMenuItem.Enabled ? "Disable" : "Enable") + " Exit Menu Item";
            toggleSeparatorMenuItem.Text = separatorMenuItem.Text == "-" ? "Make separator item to be normal" : "Make normal item to be separator";
        }

        private void ContinousScrollingMenuItem_Click(object sender, EventArgs e)
        {
            continousScrollingMenuItem.Checked = true;
            pageScrollingMenuItem.Checked = false;
        }

        private void PageScrollingMenuItem_Click(object sender, EventArgs e)
        {
            continousScrollingMenuItem.Checked = false;
            pageScrollingMenuItem.Checked = true;
        }

        private void GridMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Grid item is checked: " + ((MenuItem)sender).Checked);
        }

        private void ToggleSeparatorMenuItem_Click(object sender, System.EventArgs e)
        {
            if (separatorMenuItem.Text == "-")
                separatorMenuItem.Text = "[Text instead of separator]";
            else
                separatorMenuItem.Text = "-";

            UpdateControls();
        }

        private void SetAboutMenuItemRoleToNone_Click(object sender, EventArgs e)
        {
            aboutMenuItem.Role = MenuItemRoles.None;
        }

        private void SetOptionsMenuItemRoleToNone_Click(object sender, EventArgs e)
        {
            optionsMenuItem.Role = MenuItemRoles.None;
        }

        private void SetOptionsMenuItemRoleToPreferences_Click(object sender, EventArgs e)
        {
            optionsMenuItem.Role = MenuItemRoles.Preferences;
        }

        private void ContextMenuBorder_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            new ExampleContextMenu().Show(contextMenuBorder, e.GetPosition(contextMenuBorder));
        }

        private void ToolbarItem_Click(object? sender, EventArgs e)
        {
            LogEvent("Toolbar item clicked: " + ((ToolbarItem)sender!).Text);
        }

        private void ToggleToolbarItem_Click(object sender, EventArgs e)
        {
            var item = (ToolbarItem)sender;
            LogEvent($"Toggle toolbar item clicked: {item.Text}. Is checked: {item.Checked}");
        }

        private int lastEventNumber = 1;

        void LogEvent(string message)
        {
            eventsListBox.Items.Add($"{lastEventNumber++}. {message}");
            eventsListBox.SelectedIndex = eventsListBox.Items.Count - 1;
        }

        private void ToggleToolbarItemCheckButton_Click(object sender, EventArgs e)
        {
            checkableToolbarItem.Checked = !checkableToolbarItem.Checked;
        }

        private void ToggleFirstToolbarEnabledButton_Click(object sender, EventArgs e)
        {
            var item = toolbar.Items[0];
            item.Enabled = !item.Enabled;
        }

        private void AddDynamicToolbarItem()
        {
            int number = toolbar.Items.Count - dynamicToolbarItemsSeparatorIndex;

            string text = "Dynamic Item " + number;
            var item = new ToolbarItem(text)
            {
                Image = toolbar.Items[0].Image,
                ToolTip = text + " Description"
            };
            
            item.Click += ToolbarItem_Click;
            toolbar.Items.Add(item);
        }

        private void AddDynamicToolbarItemButton_Click(object sender, EventArgs e)
        {
            AddDynamicToolbarItem();
        }

        private void RemoveLastDynamicToolbarItemButton_Click(object sender, EventArgs e)
        {
            if (toolbar.Items.Count == dynamicToolbarItemsSeparatorIndex + 1)
                return;
            toolbar.Items.RemoveAt(toolbar.Items.Count - 1);
        }

        private void ShowToolbarTextCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if(toolbar != null)
                toolbar.ItemTextVisible = showToolbarTextCheckBox.IsChecked;
        }

        private void ShowToolbarImagesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (toolbar != null)
                toolbar.ItemImagesVisible = showToolbarImagesCheckBox.IsChecked;
        }

        private void ToolbarDropDownMenuItem_Click(object sender, EventArgs e)
        {
            var item = (MenuItem)sender;
            LogEvent($"Toolbar drop down menu item clicked: {item.Text.Replace("_", "")}.");
        }

        private void ImageToTextDisplayModeComboBox_SelectedItemChanged(object sender, EventArgs e)
        {
            toolbar.ImageToTextDisplayMode = (ToolbarItemImageToTextDisplayMode)imageToTextDisplayModeComboBox.SelectedItem!;
        }

        private void AddDynamicStatusBarPanelButton_Click(object sender, System.EventArgs e)
        {
            AddDynamicStatusBarPanel();
        }

        private void RemoveLastDynamicStatusBarPanelButton_Click(object sender, System.EventArgs e)
        {
            if (statusBar.Panels.Count == clockStatusBarPanelIndex + 1)
                return;
            statusBar.Panels.RemoveAt(statusBar.Panels.Count - 1);
        }

        private void AddDynamicStatusBarPanel()
        {
            int number = statusBar.Panels.Count - clockStatusBarPanelIndex;
            string text = "Dynamic Panel " + number;
            statusBar.Panels.Add(new StatusBarPanel(text));
        }

        private void ShowSizingGripCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            if(statusBar!=null)
                statusBar.SizingGripVisible = showSizingGripCheckBox.IsChecked;
        }
    }
}