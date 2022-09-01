using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using Alternet.UI.Integration.VisualStudio.Views;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Settings;
using Serilog;
using Serilog.Events;

namespace Alternet.UI.Integration.VisualStudio.Services
{
    [Export(typeof(IAlternetUIVisualStudioSettings))]
    public class AlternetUIVisualStudioSettings : IAlternetUIVisualStudioSettings, INotifyPropertyChanged
    {
        private const string SettingsKey = nameof(AlternetUIVisualStudioSettings);
        private readonly WritableSettingsStore _settings;
        private Orientation _designerSplitOrientation = Orientation.Vertical;
        private AlternetUIDesignerView _designerView = AlternetUIDesignerView.Source;
        private LogEventLevel _minimumLogVerbosity = LogEventLevel.Warning;

        [ImportingConstructor]
        public AlternetUIVisualStudioSettings(SVsServiceProvider vsServiceProvider)
        {
            var shellSettingsManager = new ShellSettingsManager(vsServiceProvider);
            _settings = shellSettingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);
            Load();
        }

        public Orientation DesignerSplitOrientation
        {
            get => _designerSplitOrientation;
            set
            {
                if (_designerSplitOrientation != value)
                {
                    _designerSplitOrientation = value;
                    RaisePropertyChanged();
                }
            }
        }

        public AlternetUIDesignerView DesignerView
        {
            get => _designerView;
            set
            {
                if (_designerView != value)
                {
                    _designerView = value;
                    RaisePropertyChanged();
                }
            }
        }

        public LogEventLevel MinimumLogVerbosity
        {
            get => _minimumLogVerbosity;
            set
            {
                if (_minimumLogVerbosity != value)
                {
                    _minimumLogVerbosity = value;
                    RaisePropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Load()
        {
            try
            {
                DesignerSplitOrientation = (Orientation)_settings.GetInt32(
                    SettingsKey,
                    nameof(DesignerSplitOrientation),
                    (int)Orientation.Vertical);
                DesignerView = (AlternetUIDesignerView)_settings.GetInt32(
                    SettingsKey,
                    nameof(DesignerView),
                    (int)AlternetUIDesignerView.Source);
                MinimumLogVerbosity = (LogEventLevel)_settings.GetInt32(
                    SettingsKey,
                    nameof(MinimumLogVerbosity),
                    (int)LogEventLevel.Warning);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to load settings");
            }
        }

        public void Save()
        {
            try
            {
                if (!_settings.CollectionExists(SettingsKey))
                {
                    _settings.CreateCollection(SettingsKey);
                }

                _settings.SetInt32(SettingsKey, nameof(DesignerSplitOrientation), (int)DesignerSplitOrientation);
                _settings.SetInt32(SettingsKey, nameof(DesignerView), (int)DesignerView);
                _settings.SetInt32(SettingsKey, nameof(MinimumLogVerbosity), (int)MinimumLogVerbosity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to save settings");
            }
        }

        private void RaisePropertyChanged([CallerMemberName] string propName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
