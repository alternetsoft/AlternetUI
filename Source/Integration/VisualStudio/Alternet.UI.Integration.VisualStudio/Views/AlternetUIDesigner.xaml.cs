using Alternet.UI.Integration.IntelliSense;
using Alternet.UI.Integration.IntelliSense.AssemblyMetadata;
using Alternet.UI.Integration.VisualStudio.Models;
using Alternet.UI.Integration.VisualStudio.Services;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Task = System.Threading.Tasks.Task;

namespace Alternet.UI.Integration.VisualStudio.Views
{
    internal partial class AlternetUIDesigner : UserControl, IDisposable
    {
        public static readonly DependencyProperty SelectedTargetProperty =
            DependencyProperty.Register(
                nameof(SelectedTarget),
                typeof(DesignerRunTarget),
                typeof(AlternetUIDesigner),
                new PropertyMetadata(HandleSelectedTargetChanged));

        private static readonly DependencyPropertyKey TargetsPropertyKey =
            DependencyProperty.RegisterReadOnly(
                nameof(Targets),
                typeof(IReadOnlyList<DesignerRunTarget>),
                typeof(AlternetUIDesigner),
                new PropertyMetadata());

        public static readonly DependencyProperty TargetsProperty = TargetsPropertyKey.DependencyProperty;

        private static Dictionary<string, Task<Metadata>> _metadataCache;
        private readonly Throttle<string> _throttle;
        private bool _isDisposed;

        private bool _isPaused;

        private bool _isStarted;

        private bool _loadingTargets;

        private Project _project;

        private IWpfTextViewHost _editor;

        private string _xamlPath;

        public AlternetUIDesigner()
        {
            InitializeComponent();
            _throttle = new Throttle<string>(TimeSpan.FromMilliseconds(300), UpdateXaml);
            //Process = new PreviewerProcess();
            //Process.ErrorChanged += ErrorChanged;
            //Process.FrameReceived += FrameReceived;
            //Process.ProcessExited += ProcessExited;
            //previewer.Process = Process;
            //pausedMessage.Visibility = Visibility.Collapsed;
            //UpdateLayoutForView();

            Loaded += (s, e) =>
            {
                StartStopProcessAsync().FireAndForget();
            };
        }

        /// <summary>
        /// Gets or sets the paused state of the designer.
        /// </summary>
        public bool IsPaused
        {
            get => _isPaused;
            set
            {
                if (_isPaused != value)
                {
                    Log.Logger.Debug("Setting pause state to {State}", value);

                    _isPaused = value;
                    StartStopProcessAsync().FireAndForget();
                }
            }
        }

        /// <summary>
        /// Gets or sets the selected target.
        /// </summary>
        public DesignerRunTarget SelectedTarget
        {
            get => (DesignerRunTarget)GetValue(SelectedTargetProperty);
            set => SetValue(SelectedTargetProperty, value);
        }

        /// <summary>
        /// Gets the list of targets that the designer can use to preview the XAML.
        /// </summary>
        public IReadOnlyList<DesignerRunTarget> Targets
        {
            get => (IReadOnlyList<DesignerRunTarget>)GetValue(TargetsProperty);
            private set => SetValue(TargetsPropertyKey, value);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Starts the designer.
        /// </summary>
        /// <param name="project">The project containing the XAML file.</param>
        /// <param name="xamlPath">The path to the XAML file.</param>
        /// <param name="editor">The VS text editor control host.</param>
        public void Start(Project project, string xamlPath, IWpfTextViewHost editor)
        {
            Log.Logger.Verbose("Started AvaloniaDesigner.Start()");

            if (_isStarted)
            {
                throw new InvalidOperationException("The designer has already been started.");
            }

            _project = project ?? throw new ArgumentNullException(nameof(project));
            _xamlPath = xamlPath ?? throw new ArgumentNullException(nameof(xamlPath));
            _editor = editor ?? throw new ArgumentNullException(nameof(editor));

            InitializeEditor();
            LoadTargetsAndStartProcessAsync().FireAndForget();

            Log.Logger.Verbose("Finished AvaloniaDesigner.Start()");
        }

        /// <summary>
        /// Invalidates the intellisense completion metadata.
        /// </summary>
        /// <remarks>
        /// Should be called when the designer is paused; when unpaused the completion metadata
        /// will be updated.
        /// </remarks>
        public void InvalidateCompletionMetadata()
        {
            var buffer = _editor.TextView.TextBuffer;

            if (buffer.Properties.TryGetProperty<XamlBufferMetadata>(
                    typeof(XamlBufferMetadata),
                    out var metadata))
            {
                metadata.NeedInvalidation = true;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    if (_editor?.TextView.TextBuffer is ITextBuffer2 oldBuffer)
                    {
                        oldBuffer.ChangedOnBackground -= TextChanged;
                    }

                    if (_editor?.IsClosed == false)
                    {
                        _editor.Close();
                    }

                    //Process.FrameReceived -= FrameReceived;

                    _throttle.Dispose();
                    //previewer.Dispose();
                    //Process.Dispose();
                }

                _isDisposed = true;
            }
        }

        private static void HandleSelectedTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AlternetUIDesigner designer && !designer._loadingTargets)
            {
                designer.SelectedTargetChangedAsync(d, e).FireAndForget();
            }
        }

        private static async Task CreateCompletionMetadataAsync(
            string executablePath,
            XamlBufferMetadata target)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            if (_metadataCache == null)
            {
                _metadataCache = new Dictionary<string, Task<Metadata>>();
                var dte = (DTE)Package.GetGlobalService(typeof(DTE));

                dte.Events.BuildEvents.OnBuildBegin += (s, e) => _metadataCache.Clear();
            }

            Log.Logger.Verbose("Started AvaloniaDesigner.CreateCompletionMetadataAsync() for {ExecutablePath}", executablePath);

            try
            {
                var sw = Stopwatch.StartNew();

                Task<Metadata> metadataLoad;

                if (!_metadataCache.TryGetValue(executablePath, out metadataLoad))
                {
                    metadataLoad = Task.Run(() =>
                    {
                        var metadataReader = new MetadataReader(new Alternet.UI.Integration.IntelliSense.Dnlib.DnlibMetadataProvider());
                        return metadataReader.GetForTargetAssembly(executablePath);
                    });
                    _metadataCache[executablePath] = metadataLoad;
                }

                target.CompletionMetadata = await metadataLoad;

                target.NeedInvalidation = false;

                sw.Stop();

                Log.Logger.Verbose("Finished AvaloniaDesigner.CreateCompletionMetadataAsync() took {Time} for {ExecutablePath}", sw.Elapsed, executablePath);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Error creating XAML completion metadata");
            }
            finally
            {
                Log.Logger.Verbose("Finished AvaloniaDesigner.CreateCompletionMetadataAsync()");
            }
        }

        private void UpdateXaml(string xaml)
        {
            //if (Process.IsReady)
            //{
            //    Process.UpdateXamlAsync(xaml).FireAndForget();
            //}
        }

        private void TextChanged(object sender, TextContentChangedEventArgs e)
        {
            _throttle.Queue(e.After.GetText());
        }

        private async Task StartStopProcessAsync()
        {
            if (!_isStarted)
            {
                return;
            }

            /*if (View != AvaloniaDesignerView.Source)
            {
                if (IsPaused)
                {
                    pausedMessage.Visibility = Visibility.Visible;
                    Process.Stop();
                }
                else if (!Process.IsRunning && IsLoaded)
                {
                    pausedMessage.Visibility = Visibility.Collapsed;

                    if (SelectedTarget == null)
                    {
                        await LoadTargetsAsync();
                    }

                    await StartProcessAsync();
                }
            }
            else */
            if (!IsPaused && IsLoaded)
            {
                RebuildMetadata(null, null);
                //                Process.Stop();
            }
        }

        private async Task SelectedTargetChangedAsync(object sender, DependencyPropertyChangedEventArgs e)
        {
            var oldValue = (DesignerRunTarget)e.OldValue;
            var newValue = (DesignerRunTarget)e.NewValue;

            Log.Logger.Debug(
                "AvaloniaDesigner.SelectedTarget changed from {OldTarget} to {NewTarget}",
                oldValue?.ExecutableAssembly,
                newValue?.ExecutableAssembly);

            if (oldValue?.ExecutableAssembly != newValue?.ExecutableAssembly)
            {
                if (_isStarted)
                {
                    try
                    {
                        Log.Logger.Debug("Waiting for StartProcessAsync to finish");
                        //await _startingProcess.WaitAsync();
                        //Process.Stop();
                        StartProcessAsync().FireAndForget();
                    }
                    finally
                    {
                        //_startingProcess.Release();
                    }
                }
            }
        }

        private async Task StartProcessAsync()
        {
            //Log.Logger.Verbose("Started AvaloniaDesigner.StartProcessAsync()");

            //ShowPreview();

            //var assemblyPath = SelectedTarget?.XamlAssembly;
            //var executablePath = SelectedTarget?.ExecutableAssembly;
            //var hostAppPath = SelectedTarget?.HostApp;

            //if (assemblyPath != null && executablePath != null && hostAppPath != null)
            //{
            //    RebuildMetadata(assemblyPath, executablePath);

            //    try
            //    {
            //        await _startingProcess.WaitAsync();

            //        if (!IsPaused)
            //        {
            //            await Process.SetScalingAsync(VisualTreeHelper.GetDpi(this).DpiScaleX * _scaling);
            //            await Process.StartAsync(assemblyPath, executablePath, hostAppPath);
            //            await Process.UpdateXamlAsync(await ReadAllTextAsync(_xamlPath));
            //        }
            //    }
            //    catch (ApplicationException ex)
            //    {
            //        // Don't display an error here: ProcessExited should handle that.
            //        Log.Logger.Debug(ex, "Process.StartAsync exited with error");
            //    }
            //    catch (FileNotFoundException ex)
            //    {
            //        ShowError("Build Required", ex.Message);
            //        Log.Logger.Debug(ex, "StartAsync could not find executable");
            //    }
            //    catch (Exception ex)
            //    {
            //        ShowError("Error", ex.Message);
            //        Log.Logger.Debug(ex, "StartAsync exception");
            //    }
            //    finally
            //    {
            //        _startingProcess.Release();
            //    }
            //}
            //else
            //{
            //    Log.Logger.Error("No executable found");

            //    // This message is unfortunate but I can't work out how to tell when all references
            //    // have finished loading for all projects in the solution.
            //    ShowError(
            //        "No Executable",
            //        "Reference the library from an executable or wait for the solution to finish loading.");
            //}

            //Log.Logger.Verbose("Finished AvaloniaDesigner.StartProcessAsync()");
        }

        private async Task LoadTargetsAndStartProcessAsync()
        {
            Log.Logger.Verbose("Started AvaloniaDesigner.LoadTargetsAndStartProcessAsync()");

            await LoadTargetsAsync();

            if (!_isDisposed)
            {
                _isStarted = true;
                await StartStopProcessAsync();
            }

            Log.Logger.Verbose("Finished AvaloniaDesigner.LoadTargetsAndStartProcessAsync()");
        }

        private async Task LoadTargetsAsync()
        {
            Log.Logger.Verbose("Started AvaloniaDesigner.LoadTargetsAsync()");

            _loadingTargets = true;

            try
            {
                var projects = await AlternetUIPackage.SolutionService.GetProjectsAsync();

                bool IsValidTarget(ProjectInfo project)
                {
                    return (project.Project == _project || project.ProjectReferences.Contains(_project)) &&
                        project.IsExecutable &&
                        project.References.Contains("Alternet.UI");
                }

                bool IsValidOutput(ProjectOutputInfo output)
                {
                    return output.IsNetCore;
                }

                string GetXamlAssembly(ProjectOutputInfo output)
                {
                    var project = projects.FirstOrDefault(x => x.Project == _project);

                    // Ideally we'd have the path to the `project` assembly that `output` uses, but
                    // I'm not sure how to get that information, so instead look for a netcore output
                    // or failing that a netstandard output, and pray.
                    return project?.Outputs
                        .OrderBy(x => !x.IsNetCore)
                        .ThenBy(x => !x.IsNetStandard)
                        .FirstOrDefault()?
                        .TargetAssembly;
                }

                var oldSelectedTarget = SelectedTarget;

                Targets = (from project in projects
                           where IsValidTarget(project)
                           orderby project.Project != project, !project.IsStartupProject, project.Name
                           from output in project.Outputs
                           where IsValidOutput(output)
                           select new DesignerRunTarget
                           {
                               Name = $"{project.Name} [{output.TargetFramework}]",
                               ExecutableAssembly = output.TargetAssembly,
                               XamlAssembly = GetXamlAssembly(output),
                               HostApp = output.HostApp,
                           }).ToList();

                SelectedTarget = Targets.FirstOrDefault(t => t.Name == oldSelectedTarget?.Name) ?? Targets.FirstOrDefault();
            }
            finally
            {
                _loadingTargets = false;
            }

            Log.Logger.Verbose("Finished AvaloniaDesigner.LoadTargetsAsync()");
        }

        private void InitializeEditor()
        {
            editorHost.Child = _editor.HostControl;

            //_editor.TextView.TextBuffer.Properties.RemoveProperty(typeof(PreviewerProcess));
            //_editor.TextView.TextBuffer.Properties.AddProperty(typeof(PreviewerProcess), Process);

            if (_editor.TextView.TextBuffer is ITextBuffer2 newBuffer)
            {
                newBuffer.ChangedOnBackground += TextChanged;
            }
        }

        private void RebuildMetadata(string assemblyPath, string executablePath)
        {
            assemblyPath = assemblyPath ?? SelectedTarget?.XamlAssembly;
            executablePath = executablePath ?? SelectedTarget?.ExecutableAssembly;

            if (assemblyPath != null && executablePath != null)
            {
                var buffer = _editor.TextView.TextBuffer;
                var metadata = buffer.Properties.GetOrCreateSingletonProperty(
                    typeof(XamlBufferMetadata),
                    () => new XamlBufferMetadata());
                buffer.Properties["AssemblyName"] = Path.GetFileNameWithoutExtension(assemblyPath);

                if (metadata.CompletionMetadata == null || metadata.NeedInvalidation)
                {
                    CreateCompletionMetadataAsync(executablePath, metadata).FireAndForget();
                }
            }
        }
    }
}