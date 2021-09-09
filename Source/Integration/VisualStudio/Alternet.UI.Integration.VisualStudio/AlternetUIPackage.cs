using System;
using System.Runtime.InteropServices;
using System.Threading;
using Alternet.UI.Integration.VisualStudio.Services;
using Alternet.UI.Integration.VisualStudio.Views;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Serilog;
using Serilog.Core;
using Task = System.Threading.Tasks.Task;

namespace Alternet.UI.Integration.VisualStudio
{
    [Guid(PackageGuidString)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionExistsAndFullyLoaded_string, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideEditorExtension(
        typeof(EditorFactory),
        ".uixml",
        100,
        NameResourceID = 113,
        EditorFactoryNotify = true,
        ProjectGuid = VSConstants.UICONTEXT.CSharpProject_string,
        DefaultName = Name)]
    //[ProvideEditorExtension(
    //    typeof(EditorFactory),
    //    ".paml",
    //    100,
    //    NameResourceID = 113,
    //    EditorFactoryNotify = true,
    //    ProjectGuid = VSConstants.UICONTEXT.CSharpProject_string,
    //    DefaultName = Name)]
    //[ProvideEditorExtension(
    //    typeof(EditorFactory),
    //    ".xaml",
    //    0x40,
    //    NameResourceID = 113,
    //    EditorFactoryNotify = true,
    //    ProjectGuid = VSConstants.UICONTEXT.CSharpProject_string,
    //    DefaultName = Name)]
    [ProvideEditorFactory(typeof(EditorFactory), 113, TrustLevel = __VSEDITORTRUSTLEVEL.ETL_AlwaysTrusted)]
    [ProvideEditorLogicalView(typeof(EditorFactory), LogicalViewID.Designer)]
    //[ProvideXmlEditorChooserDesignerView(Name,
    //    "xaml",
    //    LogicalViewID.Designer,
    //    10000,
    //    Namespace = "http://schemas.alternetsoft.com/ui/2021",
    //    MatchExtensionAndNamespace = true,
    //    CodeLogicalViewEditor = typeof(EditorFactory),
    //    DesignerLogicalViewEditor = typeof(EditorFactory),
    //    DebuggingLogicalViewEditor = typeof(EditorFactory),
    //    TextLogicalViewEditor = typeof(EditorFactory))]
    [ProvideOptionPage(typeof(OptionsDialogPage), Name, "General", 113, 0, supportsAutomation: true)]
    internal sealed class AlternetUIPackage : AsyncPackage
    {
        public const string PackageGuidString = "aaaba8d5-1180-4bf8-8821-345f72a4cb79";
        public const string Name = "Alternet.UI.Integration.VisualStudio";

        public static SolutionService SolutionService { get; private set; }

        protected override async Task InitializeAsync(
            CancellationToken cancellationToken,
            IProgress<ServiceProgressData> progress)
        {
            await base.InitializeAsync(cancellationToken, progress);

            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            InitializeLogging();
            RegisterEditorFactory(new EditorFactory(this));

            var dte = (DTE)await GetServiceAsync(typeof(DTE));
            SolutionService = new SolutionService(dte);

            //Log.Logger.Information("Alternet UI Package initialized");
        }

        bool outputPaneLoggingEnabled = false;

        private void InitializeLogging()
        {
            if (outputPaneLoggingEnabled)
                InitializeOutputPaneLogging();
        }

        private void InitializeOutputPaneLogging()
        {
            const string format = "{Timestamp:HH:mm:ss.fff} [{Level}] {Pid} {Message}{NewLine}{Exception}";
            var ouput = this.GetService<IVsOutputWindow, SVsOutputWindow>();
            var settings = this.GetMefService<IAlternetUIVisualStudioSettings>();
            var levelSwitch = new LoggingLevelSwitch() { MinimumLevel = settings.MinimumLogVerbosity };

            settings.PropertyChanged += (s, e) => levelSwitch.MinimumLevel = settings.MinimumLogVerbosity;

            var sink = new OutputPaneEventSink(ouput, outputTemplate: format);
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(levelSwitch)
                .WriteTo.Sink(sink, levelSwitch: levelSwitch)
                .WriteTo.Trace(outputTemplate: format)
                .CreateLogger();
        }
    }
}
