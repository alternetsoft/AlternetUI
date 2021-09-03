using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using Alternet.UI.Integration.VisualStudio.Services;
using Microsoft.VisualStudio.Shell;

namespace Alternet.UI.Integration.VisualStudio.Views
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    [Guid("aaa3ca7c-c764-4547-a7ae-12055b139bdf")]
    public class OptionsDialogPage : UIElementDialogPage
    {
        private OptionsView _options;

        protected override UIElement Child => _options ?? (_options = new OptionsView());

        protected override void OnActivate(CancelEventArgs e)
        {
            base.OnActivate(e);

            _options.Settings = Site.GetMefService<IAlternetUIVisualStudioSettings>();
        }

        public override void SaveSettingsToStorage()
        {
            base.SaveSettingsToStorage();

            _options?.Settings?.Save();
        }

        public override void LoadSettingsFromStorage()
        {
            base.LoadSettingsFromStorage();

            _options?.Settings?.Load();
        }
    }
}
