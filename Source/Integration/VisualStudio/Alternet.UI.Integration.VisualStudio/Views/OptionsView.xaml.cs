using System.Windows.Controls;
using Alternet.UI.Integration.VisualStudio.Services;

namespace Alternet.UI.Integration.VisualStudio.Views
{
    public partial class OptionsView : UserControl
    {
        public OptionsView()
        {
            InitializeComponent();
        }

        public IAlternetUIVisualStudioSettings Settings
        {
            get => DataContext as IAlternetUIVisualStudioSettings;
            set => DataContext = value;
        }
    }
}
