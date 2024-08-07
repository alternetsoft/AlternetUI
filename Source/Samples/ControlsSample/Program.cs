using System;
using System.ComponentModel;
using Alternet.Drawing;

using Alternet.UI;

namespace ControlsSample
{
    internal class Program
    {
        public static void InitSamples()
        {
            if(!App.IsLinuxOS)
                InternalSamplesPage.Add("Action Simulator Sample", () => new ActionSimulatorPage());

            PropertyGridSample.MainWindow.LimitedTypesStatic.Add(
                typeof(PropertyGridSample.ControlPainterPreview));

            PropertyGridSample.ObjectInit.Actions.Add(typeof(PropertyGridSample.ControlPainterPreview), (c) =>
            {
                var control = (c as PropertyGridSample.ControlPainterPreview)!;
                control.SuggestedSize = 200;
            });
        }

        [STAThread]
        public static void Main()
        {
            LogUtils.ShowDebugWelcomeMessage = true;

            var testBadFont = false;

            var application = new Application();

            application.SetUnhandledExceptionModeIfDebugger(UnhandledExceptionMode.CatchException);

            InitSamples();

            if (testBadFont)
                Control.DefaultFont = new Font("abrakadabra", 12);

            var window = new MainWindow();

            application.Run(window);

            window.Dispose();
            application.Dispose();
        }
    }
}