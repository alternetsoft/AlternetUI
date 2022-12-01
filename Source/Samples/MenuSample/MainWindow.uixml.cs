using Alternet.UI;
using System;
using System.Linq;
using System.Security.Policy;

namespace MenuSample
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            SaveCommand = new Command(o => MessageBox.Show("Save"), o => saveEnabledMenuItem.Checked);
            ExportToPngCommand = new Command(o => MessageBox.Show("Export to PNG"));
            DataContext = this;

            PlatformSpecificInitialize();
            UpdateControls();
        }

        private void PlatformSpecificInitialize()
        {
#if NETCOREAPP
            bool runningUnderMacOS = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(
                System.Runtime.InteropServices.OSPlatform.OSX);
#else
            bool runningUnderMacOS = false;
#endif

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

        private void ToolbarItem_Click(object sender, EventArgs e)
        {
            LogEvent("Toolbar item clicked: " + ((ToolbarItem)sender).Text);
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
    }
}