using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using Alternet.UI.Integration.VisualStudio.Services;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;

namespace Alternet.UI.Integration.VisualStudio.Views
{
    /// <summary>
    /// A <see cref="WindowPane"/> used to host an <see cref="AvaloniaDesigner"/>.
    /// </summary>
    internal class DesignerPane : EditorHostPane
    {
        private readonly Project _project;
        private readonly string _xamlPath;
        private readonly IWpfTextViewHost _editorHost;
        private readonly DTEEvents _dteEvents;
        private readonly BuildEvents _buildEvents;
        private AlternetUIDesigner _content;
        private bool _isPaused;
        private AlternetUIPackage _package;
        private bool _isDummy;
        private Control _rootControl;
        private bool _reopenScheduled;

        /// <summary>
        /// Initializes a new instance of the <see cref="DesignerPane"/> class.
        /// </summary>
        /// <param name="project">The project containing the XAML file to edit.</param>
        /// <param name="xamlPath">The path to the XAML file to edit.</param>
        /// <param name="editorWindow">The editor window to be used by the designer.</param>
        /// <param name="editorHost">The editor control to be used by the designer.</param>
        public DesignerPane(
            AlternetUIPackage package,
            Project project,
            string xamlPath,
            IVsCodeWindow editorWindow,
            IWpfTextViewHost editorHost,
            bool dummy = false)
            : base(editorWindow)
        {
            _package = package;
            _isDummy = dummy;
            _rootControl = editorHost.HostControl;

            ThreadHelper.ThrowIfNotOnUIThread();

            _project = project;
            _xamlPath = xamlPath;
            _editorHost = editorHost;

            var dte = (DTE)Package.GetGlobalService(typeof(DTE));
            _buildEvents = dte.Events.BuildEvents;
            _dteEvents = dte.Events.DTEEvents;
            _isPaused = dte.Mode == vsIDEMode.vsIDEModeDebug;

            _buildEvents.OnBuildBegin += HandleBuildBegin;
            _buildEvents.OnBuildDone += HandleBuildDone;
            _dteEvents.ModeChanged += HandleModeChanged;

            if (_isDummy)
            {
                _rootControl.Loaded += OnRootControlLoaded;
            }
        }

        private async Task CloseThenOpenRealAsync()
        {
            await Task.Delay(1000);
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            CloseThisFrame();
        }

        private void OnRootControlLoaded(object sender, RoutedEventArgs e)
        {
            _rootControl.Loaded -= OnRootControlLoaded;

            ThreadHelper.JoinableTaskFactory.RunAsync(CloseThenOpenRealAsync);
        }

        private async Task RetryReopenAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var oldFrame = GetWindowFrame();

            for (int attempt = 1; attempt <= 5; attempt++)
            {
                await Task.Delay(300);
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                if (_package.OpenWithMyEditor(_xamlPath))
                {
                    var newFrame = _package.FindDocumentFrame(_xamlPath);

                    if (newFrame != null && !ReferenceEquals(newFrame, oldFrame))
                    {
                        oldFrame?.CloseFrame((uint)__FRAMECLOSE.FRAMECLOSE_NoSave);
                        return;
                    }
                }
            }
        }

        private async Task RetryReopenAsyncOld()
        {
            _package.ScheduleReopen(_xamlPath, 5, () =>
            {
                CloseThisFrame();

                _package.ScheduleReopen(_xamlPath, 5, () =>
                {
                });
            });            
        }

        private void ScheduleReopenOnce()
        {
            if (_reopenScheduled)
                return;

            _reopenScheduled = true;

            ThreadHelper.JoinableTaskFactory.RunAsync(async delegate
            {
                await Task.Delay(300);
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                TryReopenAndCloseSelf();
            });
        }

        private void TryReopenAndCloseSelf()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (_package.OpenWithMyEditor(_xamlPath))
            {
            }
        }

        private IVsWindowFrame GetWindowFrame()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            return GetService(typeof(SVsWindowFrame)) as IVsWindowFrame;
        }

        public void CloseThisFrame()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var frame = GetWindowFrame();
            frame?.CloseFrame((uint)__FRAMECLOSE.FRAMECLOSE_NoSave);
        }

        /// <summary>
        /// Gets the content of the window pane.
        /// </summary>
        public override object Content => _content;

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _content?.Dispose();
            _content = null;
        }

        protected override void Initialize()
        {
            Log.Verbose("Started DesignerPane.Initialize()");

            base.Initialize();

            var settings = this.GetMefService<IAlternetUIVisualStudioSettings>();
            var xamlEditorView = new AlternetUIDesigner();
            xamlEditorView.IsPaused = _isPaused;

            xamlEditorView.Settings = settings;
            xamlEditorView.SplitOrientation = settings.DesignerSplitOrientation;

            if (_isDummy)
            {
                xamlEditorView.View = AlternetUIDesignerView.Source;
            }
            else
            {
                xamlEditorView.View = settings.DesignerView;
            }

            xamlEditorView.Start(_project, _xamlPath, _editorHost);
            _content = xamlEditorView;

            Log.Verbose("Finished DesignerPane.Initialize()");
        }

        private void HandleModeChanged(vsIDEMode lastMode)
        {
            if (_content != null)
            {
                _content.IsPaused = _isPaused = lastMode == vsIDEMode.vsIDEModeDesign;
            }
        }

        private void HandleBuildBegin(vsBuildScope Scope, vsBuildAction Action)
        {
            Log.Debug("Build started");

            _isPaused = true;

            if (_content != null)
            {
                _content.IsPaused = _isPaused;
            }
        }

        private void HandleBuildDone(vsBuildScope Scope, vsBuildAction Action)
        {
            Log.Debug("Build finished");

            _isPaused = false;

            if (_content != null)
            {
                _content.InvalidateCompletionMetadata();
                _content.IsPaused = _isPaused;
            }
        }
    }
}
