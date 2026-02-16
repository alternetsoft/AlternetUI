using Alternet.UI.Integration.IntelliSense;
using Alternet.UI.Integration.IntelliSense.AssemblyMetadata;
using Alternet.UI.Integration.IntelliSense.Dnlib;
using Alternet.UI.Integration.VisualStudio.Models;
using Alternet.UI.Integration.VisualStudio.Services;
using Alternet.UI.Integration.VisualStudio.Utils;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Task = System.Threading.Tasks.Task;
using Alternet.UI.Integration.VisualStudio;

using Alternet.UI.Integration;

#pragma warning disable VSTHRD100, VSTHRD010

namespace Alternet.UI.Integration.VisualStudio.Views
{
    public enum AlternetUIDesignerView
    {
        Split,
        Design,
        Source,
    }

    /// <summary>
    /// The AlternetUI XAML designer control.
    /// </summary>
    internal partial class AlternetUIDesigner : UserControl, IDisposable
    {
        private static readonly DependencyPropertyKey TargetsPropertyKey =
            DependencyProperty.RegisterReadOnly(
                nameof(Targets),
                typeof(IReadOnlyList<DesignerRunTarget>),
                typeof(AlternetUIDesigner),
                new PropertyMetadata());

        public static readonly DependencyProperty SelectedTargetProperty =
            DependencyProperty.Register(
                nameof(SelectedTarget),
                typeof(DesignerRunTarget),
                typeof(AlternetUIDesigner),
                new PropertyMetadata(HandleSelectedTargetChanged));

        public static readonly DependencyProperty SplitOrientationProperty =
            DependencyProperty.Register(
                nameof(SplitOrientation),
                typeof(Orientation),
                typeof(AlternetUIDesigner),
                new PropertyMetadata(Orientation.Horizontal, HandleSplitOrientationChanged));

        public static readonly DependencyProperty ViewProperty =
            DependencyProperty.Register(
                nameof(View),
                typeof(AlternetUIDesignerView),
                typeof(AlternetUIDesigner),
                new PropertyMetadata(AlternetUIDesignerView.Split, HandleViewChanged));

        public static readonly DependencyProperty TargetsProperty =
            TargetsPropertyKey.DependencyProperty;

        private static readonly GridLength ZeroStar = new GridLength(0, System.Windows.GridUnitType.Star);
        private static readonly GridLength OneStar = new GridLength(1, System.Windows.GridUnitType.Star);
        private readonly Throttle<string> _throttle;
        private readonly ColumnDefinition _previewCol = new ColumnDefinition { Width = OneStar };
        private readonly ColumnDefinition _codeCol = new ColumnDefinition { Width = OneStar };
        private Project _project;
        private IWpfTextViewHost _editor;
        private string _xamlPath;
        private bool _loadingTargets;
        private bool _isStarted;
        private bool _isPaused;
        private SemaphoreSlim _startingProcess = new SemaphoreSlim(1, 1);
        private bool _disposed;
        private double _scaling = 1;
        private AlternetUIDesignerView _unPausedView;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlternetUIDesigner"/> class.
        /// </summary>
        public AlternetUIDesigner()
        {
            InitializeComponent();

            _throttle = new Throttle<string>(TimeSpan.FromMilliseconds(300), UpdateXaml);
            Process = new PreviewerProcess();
            Process.ErrorChanged += ErrorChanged;
            Process.PreviewDataReceived += PreviewDataReceived;
            Process.ProcessExited += ProcessExited;
            previewer.Process = Process;
            pausedMessage.Visibility = Visibility.Collapsed;
            UpdateLayoutForView();

            Loaded += (s, e) =>
            {
                StartStopProcessAsync().FireAndForget();
            };
        }
        
        public IAlternetUIVisualStudioSettings Settings { get; set; }

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
                    Log.Debug($"Setting pause state to {value}");

                    _isPaused = value;
                    StartStopProcessAsync().FireAndForget();

                    if (value)
                    {
                        _unPausedView = View;
                        // Hide the designer and only show the xaml source when debugging
                        // This matches UWP/WPF's designer
                        View = AlternetUIDesignerView.Source;
                    }
                    else
                    {
                        View = _unPausedView;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the previewer process used by the designer.
        /// </summary>
        public PreviewerProcess Process { get; }

        /// <summary>
        /// Gets the list of targets that the designer can use to preview the XAML.
        /// </summary>
        public IReadOnlyList<DesignerRunTarget> Targets
        {
            get => (IReadOnlyList<DesignerRunTarget>)GetValue(TargetsProperty);
            private set => SetValue(TargetsPropertyKey, value);
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
        /// Gets or sets the orientation of the split view.
        /// </summary>
        public Orientation SplitOrientation
        {
            get => (Orientation)GetValue(SplitOrientationProperty);
            set => SetValue(SplitOrientationProperty, value);
        }

        /// <summary>
        /// Gets or sets the type of view to display.
        /// </summary>
        public AlternetUIDesignerView View
        {
            get => (AlternetUIDesignerView)GetValue(ViewProperty);
            set => SetValue(ViewProperty, value);
        }

        /// <summary>
        /// Starts the designer.
        /// </summary>
        /// <param name="project">The project containing the XAML file.</param>
        /// <param name="xamlPath">The path to the XAML file.</param>
        /// <param name="editor">The VS text editor control host.</param>
        public void Start(Project project, string xamlPath, IWpfTextViewHost editor)
        {
            Log.Verbose("Started AlternetUIDesigner.Start()");

            if (_isStarted)
            {
                throw new InvalidOperationException("The designer has already been started.");
            }

            _project = project ?? throw new ArgumentNullException(nameof(project));
            _xamlPath = xamlPath ?? throw new ArgumentNullException(nameof(xamlPath));
            _editor = editor ?? throw new ArgumentNullException(nameof(editor));

            InitializeEditor();
            LoadTargetsAndStartProcessAsync().FireAndForget();

            Log.Verbose("Finished AlternetUIDesigner.Start()");
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

        /// <summary>
        /// Disposes of the designer and all resources.
        /// </summary>
        public void Dispose()
        {
            _disposed = true;

            if (_editor?.TextView.TextBuffer is ITextBuffer2 oldBuffer)
            {
                oldBuffer.ChangedOnBackground -= TextChanged;
            }

            if (_editor?.IsClosed == false)
            {
                _editor.Close();
            }

            Process.PreviewDataReceived -= PreviewDataReceived;

            _throttle.Dispose();
            previewer.Dispose();
            Process.Dispose();
        }

        protected override void OnDpiChanged(DpiScale oldDpi, DpiScale newDpi)
        {
            Process.SetScalingAsync(newDpi.DpiScaleX * _scaling).FireAndForget();
        }

        private void InitializeEditor()
        {
            editorHost.Child = _editor.HostControl;

            _editor.TextView.TextBuffer.Properties.RemoveProperty(typeof(PreviewerProcess));
            _editor.TextView.TextBuffer.Properties.AddProperty(typeof(PreviewerProcess), Process);

            _editor.TextView.Properties.RemoveProperty(typeof(AlternetUIDesigner));
            _editor.TextView.Properties.AddProperty(typeof(AlternetUIDesigner), this);

            if (_editor.TextView.TextBuffer is ITextBuffer2 newBuffer)
            {
                newBuffer.ChangedOnBackground += TextChanged;
            }
        }

        private async Task LoadTargetsAndStartProcessAsync()
        {
            Log.Verbose("Started AlternetUIDesigner.LoadTargetsAndStartProcessAsync()");

            await LoadTargetsAsync();

            if (!_disposed)
            {
                _isStarted = true;
                await StartStopProcessAsync();
            }

            Log.Verbose("Finished AlternetUIDesigner.LoadTargetsAndStartProcessAsync()");
        }

        private async Task LoadTargetsAsync()
        {
            bool IsValidOutput(ProjectOutputInfo output)
            {
                return true;
            }

            string GetXamlAssembly(ProjectOutputInfo output, IReadOnlyList<ProjectInfo> projects)
            {
                var project = projects.FirstOrDefault(x => x.Project == _project);
                return project?.Outputs.FirstOrDefault(x => x.TargetFramework == output.TargetFramework)?.TargetAssembly;
            }

            bool IsValidTarget(ProjectInfo project)
            {
                return (project.Project == _project || project.ProjectReferences.Contains(_project)) &&
                    project.IsExecutable &&
                    project.References.Contains("Alternet.UI") || project.References.Contains("Alternet.UI.Common") ||
                    ProjectReferencesAlternetUISourceProject(project);

                static bool ProjectReferencesAlternetUISourceProject(ProjectInfo project)
                {
                    // For the IntelliSense to show up in the sample projects
                    // when developing AlterNET UI and referencing the project
                    // instead of the NuGet package.
                    return project.ProjectReferences.Any(
                        x => Path.GetFileName(x.FullName)
                        .Equals("Alternet.UI.csproj", StringComparison.OrdinalIgnoreCase));
                }
            }


            Log.Verbose("Started AlternetUIDesigner.LoadTargetsAsync()");

            _loadingTargets = true;

            try
            {
                try
                {
                    var projects = await AlternetUIPackage.SolutionService.GetProjectsAsync();

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
                                   XamlAssembly = GetXamlAssembly(output, projects),
                                   HostApp = output.HostApp,
                               }).ToList();

                    SelectedTarget = Targets.FirstOrDefault(t => t.Name == oldSelectedTarget?.Name)
                        ?? Targets.FirstOrDefault();
                }
                finally
                {
                    _loadingTargets = false;
                }

            }
            catch(Exception ex)
            {
                Log.Verbose($"AlternetUIDesigner.LoadTargetsAsync() exception: {ex}");
            }

            Log.Verbose("Finished AlternetUIDesigner.LoadTargetsAsync()");
        }

        private async Task StartStopProcessAsync()
        {
            if (!_isStarted)
            {
                return;
            }

            if (!IsPaused && IsLoaded)
                RebuildMetadata(null, null);

            if (View != AlternetUIDesignerView.Source)
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
            else if (!IsPaused && IsLoaded)
            {
                Process.Stop();
            }
        }

        private async Task StartProcessAsync()
        {
            Log.Verbose("Started AlternetUIDesigner.StartProcessAsync()");

            ShowPreview();

            var assemblyPath = SelectedTarget?.XamlAssembly;
            var executablePath = SelectedTarget?.ExecutableAssembly;
            var hostAppPath = SelectedTarget?.HostApp;

            if (assemblyPath != null && executablePath != null && hostAppPath != null)
            {
                RebuildMetadata(assemblyPath, executablePath);

                try
                {
                    await _startingProcess.WaitAsync();

                    if (!IsPaused)
                    {
                        await Process.SetScalingAsync(VisualTreeHelper.GetDpi(this).DpiScaleX * _scaling);
                        await Process.StartAsync(assemblyPath, executablePath, hostAppPath);
                        await Process.UpdateXamlAsync(await ReadAllTextAsync(_xamlPath), GetOwnerWindowLocation());
                    }
                }
                catch (ApplicationException ex)
                {
                    // Don't display an error here: ProcessExited should handle that.
                    Log.Debug($"Process.StartAsync exited with error: {ex}");
                }
                catch (FileNotFoundException ex)
                {
                    ShowError("Build Required", ex.Message);
                    Log.Debug($"StartAsync could not find executable: {ex}");
                }
                catch (Exception ex)
                {
                    ShowError("Error", ex.Message);
                    Log.Debug($"StartAsync exception: {ex}");
                }
                finally
                {
                    _startingProcess.Release();
                }
            }
            else
            {
                Log.Error("No executable found");

                // This message is unfortunate but I can't work out how to tell when all references
                // have finished loading for all projects in the solution.
                ShowError(
                    "No Executable",
                    "Reference the library from an executable or wait for the solution to finish loading.");
            }

            Log.Verbose("Finished AlternetUIDesigner.StartProcessAsync()");
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

        private static Dictionary<string, Task<Metadata>> _metadataCache;

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

            Log.Verbose($"Started AlternetUIDesigner.CreateCompletionMetadataAsync() for {executablePath}");

            try
            {
                var sw = Stopwatch.StartNew();

                Task<Metadata> metadataLoad;

                if (!_metadataCache.TryGetValue(executablePath, out metadataLoad))
                {
                    metadataLoad = Task.Run(() =>
                    {
                        var metadataReader = new MetadataReader(new DnlibMetadataProvider());
                        return metadataReader.GetForTargetAssembly(executablePath);
                    });
                    _metadataCache[executablePath] = metadataLoad;
                }

                target.CompletionMetadata = await metadataLoad;

                target.NeedInvalidation = false;

                sw.Stop();

                Log.Verbose($"Finished AlternetUIDesigner.CreateCompletionMetadataAsync() took {sw.Elapsed} for {executablePath}");
            }
            catch (Exception ex)
            {
                Log.Error($"Error creating XAML completion metadata: {ex}");
            }
            finally
            {
                Log.Verbose("Finished AlternetUIDesigner.CreateCompletionMetadataAsync()");
            }
        }

        private async void ErrorChanged(object sender, EventArgs e)
        {
            const string strCheckDetails = "Check \"AlterNET UI\" section in the Output window for more info.";

            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            if (Process.PreviewData == null || Process.Error != null)
            {
                if(Process.Error != null)
                {
                    var errorMessage = Process.Error.Message;

                    if (errorMessage != null && errorMessage.Length > 0)
                    {
                        var ln = Process.Error.UixmlLineNumber;

                        if (ln is not null)
                        {
                            ShowError("Preview failed", $"{errorMessage}({ln}){Environment.NewLine}{strCheckDetails}");
                        }
                        else
                        {
                            ShowError("Preview failed", $"{errorMessage}{Environment.NewLine}{strCheckDetails}");
                        }

                        return;
                    }
                }

                ShowError("Preview failed", strCheckDetails);
            }
            else
            {
                ShowPreview();
            }
        }

        private async void PreviewDataReceived(object sender, EventArgs e)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            if (Process.PreviewData != null && Process.Error == null)
            {
                ShowPreview();
            }
        }

        private async void ProcessExited(object sender, EventArgs e)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            if (!IsPaused && View != AlternetUIDesignerView.Source)
            {
                ShowError(
                    "Process Exited",
                    "The previewer process exited unexpectedly. See the output window for more information.");
            }
        }

        private void ShowError(string heading, string message)
        {
            errorIndicator.Visibility = Visibility.Visible;
            errorHeading.Text = heading;
            errorMessage.Text = message;
        }

        private void ShowPreview()
        {
            errorIndicator.Visibility = Visibility.Collapsed;
        }

        private void TextChanged(object sender, TextContentChangedEventArgs e)
        {
            _throttle.Queue(e.After.GetText());
        }

        private void UpdateLayoutForView()
        {
            void HorizontalGrid()
            {
                if (mainGrid.RowDefinitions.Count == 0)
                {
                    mainGrid.RowDefinitions.Add(previewRow);
                    mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    mainGrid.RowDefinitions.Add(codeRow);
                    mainGrid.ColumnDefinitions.Clear();
                    splitter.Height = 5;
                    splitter.Width = double.NaN;
                }
            }

            void VerticalGrid()
            {
                if (mainGrid.ColumnDefinitions.Count == 0)
                {
                    mainGrid.ColumnDefinitions.Add(_previewCol);
                    mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                    mainGrid.ColumnDefinitions.Add(_codeCol);
                    mainGrid.RowDefinitions.Clear();
                    splitter.Width = 5;
                    splitter.Height = double.NaN;
                }
            }

            orientationListBox.Visibility = View == AlternetUIDesignerView.Split ? Visibility.Visible : Visibility.Collapsed;

            if (View == AlternetUIDesignerView.Split)
            {
                previewRow.Height = OneStar;
                codeRow.Height = OneStar;

                if (SplitOrientation == Orientation.Horizontal)
                {
                    HorizontalGrid();
                }
                else
                {
                    VerticalGrid();
                }

                splitter.Visibility = Visibility.Visible;
            }
            else
            {
                HorizontalGrid();
                previewRow.Height = View == AlternetUIDesignerView.Design ? OneStar : ZeroStar;
                codeRow.Height = View == AlternetUIDesignerView.Source ? OneStar : ZeroStar;
                splitter.Visibility = Visibility.Collapsed;
            }
        }

        private void UpdateXaml(string xaml)
        {
            if (Process.IsReady)
            {
                Process.UpdateXamlAsync(xaml, GetOwnerWindowLocation()).FireAndForget();
            }
        }

        private System.Drawing.Point GetOwnerWindowLocation() => WindowHelper.GetWindowLocation(Application.Current.MainWindow);

        private async Task SelectedTargetChangedAsync(object sender, DependencyPropertyChangedEventArgs e)
        {
            var oldValue = (DesignerRunTarget)e.OldValue;
            var newValue = (DesignerRunTarget)e.NewValue;

            Log.Debug(
                $"AlternetUIDesigner.SelectedTarget changed from {oldValue?.ExecutableAssembly} to {newValue?.ExecutableAssembly}");

            if (oldValue?.ExecutableAssembly != newValue?.ExecutableAssembly)
            {
                if (_isStarted)
                {
                    try
                    {
                        Log.Debug("Waiting for StartProcessAsync to finish");
                        await _startingProcess.WaitAsync();
                        Process.Stop();
                        StartProcessAsync().FireAndForget();
                    }
                    finally
                    {
                        _startingProcess.Release();
                    }
                }
            }
        }

        private static void HandleSelectedTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AlternetUIDesigner designer && !designer._loadingTargets)
            {
                designer.SelectedTargetChangedAsync(d, e).FireAndForget();
            }
        }

        private static void HandleSplitOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AlternetUIDesigner designer)
            {
                designer.UpdateLayoutForView();

                var settings = designer.Settings;
                if (settings != null)
                {
                    settings.DesignerSplitOrientation = (Orientation)e.NewValue;
                    settings.Save();
                }
            }
        }

        private static void HandleViewChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AlternetUIDesigner designer)
            {
                designer.UpdateLayoutForView();
                designer.StartStopProcessAsync().FireAndForget();

                var settings = designer.Settings;
                if (settings != null)
                {
                    settings.DesignerView = (AlternetUIDesignerView)e.NewValue;
                    settings.Save();
                }
            }
        }

        //private static void HandleZoomLevelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    if (d is AlternetUIDesigner designer && designer.TryProcessZoomLevelValue(out double scaling))
        //    {
        //        designer.UpdateScaling(scaling);
        //    }
        //}

        private static async Task<string> ReadAllTextAsync(string fileName)
        {
            using (var reader = File.OpenText(fileName))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }

}

#pragma warning restore VSTHRD100, VSTHRD010