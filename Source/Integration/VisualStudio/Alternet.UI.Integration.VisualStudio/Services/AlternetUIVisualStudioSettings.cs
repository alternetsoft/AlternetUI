using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using Alternet.UI.Integration.VisualStudio.Views;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Settings;

namespace Alternet.UI.Integration.VisualStudio.Services
{
    [Export(typeof(IAlternetUIVisualStudioSettings))]
    public class AlternetUIVisualStudioSettings : IAlternetUIVisualStudioSettings, INotifyPropertyChanged
    {
        private const string SettingsKey = nameof(AlternetUIVisualStudioSettings);
        private readonly WritableSettingsStore _settings;
        private Orientation _designerSplitOrientation = Orientation.Vertical;
        private AlternetUIDesignerView _designerView = AlternetUIDesignerView.Split;
        
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
                    (int)AlternetUIDesignerView.Split);
            }
            catch (Exception ex)
            {
                Log.Error($"Failed to load settings: {ex}");
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
            }
            catch (Exception ex)
            {
                Log.Error($"Failed to save settings: {ex}");
            }
        }

        private void RaisePropertyChanged([CallerMemberName] string propName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
