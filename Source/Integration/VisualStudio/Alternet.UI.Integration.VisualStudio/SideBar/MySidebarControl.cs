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
            var stack = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(8)
            };

            InfoTextBlock = new TextBlock
            {
                TextWrapping = TextWrapping.Wrap,
            };

            InfoTextBlock.SetResourceReference(TextBlock.ForegroundProperty, EnvironmentColors.ToolWindowTextBrushKey);
            InfoTextBlock.SetResourceReference(TextBlock.BackgroundProperty, EnvironmentColors.ToolWindowBackgroundBrushKey);

            stack.Children.Add(InfoTextBlock);

            this.Content = stack;
        }

        public void UpdateInfo(string docPath, string projectPath)
        {
            InfoTextBlock.Text = $"Document: {docPath}\nProject: {projectPath}";
        }
    }
}
