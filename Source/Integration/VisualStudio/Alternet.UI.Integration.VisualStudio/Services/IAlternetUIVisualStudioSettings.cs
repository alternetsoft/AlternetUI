using System.ComponentModel;
using System.Windows.Controls;
using Alternet.UI.Integration.VisualStudio.Views;
using Serilog.Events;

namespace Alternet.UI.Integration.VisualStudio.Services
{
    public interface IAlternetUIVisualStudioSettings : INotifyPropertyChanged
    {
        Orientation DesignerSplitOrientation { get; set; }
        AlternetUIDesignerView DesignerView { get; set; }
        LogEventLevel MinimumLogVerbosity { get; set; }

        void Save();
        void Load();
    }
}
