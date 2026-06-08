using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Controls;


namespace Alternet.UI.Integration.VisualStudio
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using Microsoft.VisualStudio.PlatformUI;

    public partial class MySidebarControl : UserControl
    {
        private readonly TextBlock InfoTextBlock;

        public MySidebarControl()
        {
            // Create a container panel
            var stack = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(8)
            };

            // Create the text block
            InfoTextBlock = new TextBlock
            {
                Text = "Waiting for document change...",
                TextWrapping = TextWrapping.Wrap,
            };

            InfoTextBlock.SetResourceReference(TextBlock.ForegroundProperty, EnvironmentColors.ToolWindowTextBrushKey);
            InfoTextBlock.SetResourceReference(TextBlock.BackgroundProperty, EnvironmentColors.ToolWindowBackgroundBrushKey);

            // Add to container
            stack.Children.Add(InfoTextBlock);

            // Set as content of the control
            this.Content = stack;
        }

        /// <summary>
        /// Updates the info text with document and project paths.
        /// </summary>
        public void UpdateInfo(string docPath, string projectPath)
        {
            InfoTextBlock.Text = $"Document: {docPath}\nProject: {projectPath}";
        }
    }
}
