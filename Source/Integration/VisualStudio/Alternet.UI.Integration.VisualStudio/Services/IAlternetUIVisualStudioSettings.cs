using System.ComponentModel;
using System.Windows.Controls;
using Alternet.UI.Integration.VisualStudio.Views;

namespace Alternet.UI.Integration.VisualStudio.Services
{
    public interface IAlternetUIVisualStudioSettings : INotifyPropertyChanged
    {
        Orientation DesignerSplitOrientation { get; set; }
        AlternetUIDesignerView DesignerView { get; set; }

        void Save();
        void Load();
    }
}
