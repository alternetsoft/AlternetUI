using Alternet.UI;
using System;
using System.Linq;

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

            UpdateControls();
        }

        public Command SaveCommand { get; }
        public Command ExportToPngCommand { get; }

        private void OpenMenuItem_Click(object sender, EventArgs e) => MessageBox.Show("Open");

        private void SaveEnabledMenuItem_Click(object sender, EventArgs e) => SaveCommand.RaiseCanExecuteChanged();

        private void ExportToPdfMenuItem_Click(object sender, EventArgs e) => MessageBox.Show("Export to PDF");

        private void ExportToPngMenuItem_Click(object sender, EventArgs e) => MessageBox.Show("Export to PNG");

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

        private void ToggleSeparatorMenuItem_Click(object sender, System.EventArgs e)
        {
            if (separatorMenuItem.Text == "-")
                separatorMenuItem.Text = "[Text instead of separator]";
            else
                separatorMenuItem.Text = "-";

            UpdateControls();
        }

        private void SetAboutMenuItemRoleToNone_Click(object sender, System.EventArgs e)
        {
            aboutMenuItem.Role = MenuItemRoles.None;
        }
    }
}