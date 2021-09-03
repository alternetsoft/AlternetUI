using System.ComponentModel;
using System.Windows.Controls;
using Alternet.UI.Integration.VisualStudio.Views;
using Serilog.Events;

namespace Alternet.UI.Integration.VisualStudio.Services
{
    public interface IAlternetUIVisualStudioSettings : INotifyPropertyChanged
    {
        LogEventLevel MinimumLogVerbosity { get; set; }
        void Save();
        void Load();
    }
}
