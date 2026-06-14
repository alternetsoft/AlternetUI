using Alternet.UI.Integration.VisualStudio.Services;
using Alternet.UI.Integration.VisualStudio.Views;
using EnvDTE;

using Microsoft;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Threading;
using Task = System.Threading.Tasks.Task;

namespace Alternet.UI.Integration.VisualStudio
{
    [ProvideAutoLoad(VSConstants.UICONTEXT.NoSolution_string, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionExists_string, PackageAutoLoadFlags.BackgroundLoad)]
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [ProvideToolWindow(typeof(MySidebarToolWindow))]
    [Guid(PackageGuidString)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideOptionPage(typeof(OptionsDialogPage), Name, "General", 113, 0, supportsAutomation: true)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideEditorExtension(
        typeof(EditorFactory),
        ".uixml",
        100,
        NameResourceID = 113,
        EditorFactoryNotify = true,
        ProjectGuid = VSConstants.UICONTEXT.CSharpProject_string,
        DefaultName = Name)]
    [ProvideEditorFactory(typeof(EditorFactory), 113, TrustLevel = __VSEDITORTRUSTLEVEL.ETL_AlwaysTrusted)]
    [ProvideEditorLogicalView(typeof(EditorFactory), LogicalViewID.Designer)]
    public sealed class AlternetUIPackage : AsyncPackage, IVsRunningDocTableEvents, IVsSelectionEvents
    {
        internal static IVsOutputWindow? OutputWindow;        
        internal static IVsOutputWindowPane OutputPane;

        private uint _rdtCookie;
        private static Guid _guid;
        private static ErrorListProvider _errorListProvider;

        static AlternetUIPackage()
        {
            Log.Write = Write;
        }

        public static void InitializeMsg(IServiceProvider serviceProvider)
        {
            _errorListProvider = new ErrorListProvider(serviceProvider);
        }

        public static void AddError(string message, int line = -1, int column = -1)
        {
            AddMsgTask(message, TaskErrorCategory.Error, line, column);
        }

        public static void AddWarning(string message, int line = -1, int column = -1)
        {
            AddMsgTask(message, TaskErrorCategory.Warning, line, column);
        }

        public static void AddMessage(string message, int line = -1, int column = -1)
        {
            AddMsgTask(message, TaskErrorCategory.Message, line, column);
        }

        // Called when a document is saved, opened, closed, etc.
        public int OnAfterDocumentWindowHide(uint docCookie, IVsWindowFrame pFrame)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            // After closing, determine the new active document
            var monitorSelection = (IVsMonitorSelection)GetService(typeof(SVsShellMonitorSelection));
            monitorSelection.GetCurrentElementValue((uint)VSConstants.VSSELELEMID.SEID_DocumentFrame, out object frameObj);

            string docPath = null;
            string projectPath = null;

            if (frameObj is IVsWindowFrame frame)
            {
                frame.GetProperty((int)__VSFPROPID.VSFPROPID_pszMkDocument, out object docPathObj);
                docPath = docPathObj as string;

                frame.GetProperty((int)__VSFPROPID.VSFPROPID_Hierarchy, out object hierarchyObj);
                if (hierarchyObj is IVsHierarchy hierarchy)
                {
                    hierarchy.GetCanonicalName(VSConstants.VSITEMID_ROOT, out projectPath);
                }
            }

            UpdateSidebarInfo(docPath, projectPath);

            return VSConstants.S_OK;
        }

        public int OnAfterFirstDocumentLock(
            uint docCookie,
            uint dwRDTLockType,
            uint dwReadLocksRemaining,
            uint dwEditLocksRemaining) => VSConstants.S_OK;

        public int OnAfterSave(uint docCookie)
        {
            UpdateSidebarInfo(docCookie);

            return VSConstants.S_OK;
        }

        public int OnBeforeDocumentWindowShow(uint docCookie, int fFirstShow, IVsWindowFrame pFrame)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeLastDocumentUnlock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining, uint dwEditLocksRemaining)
        {
            return VSConstants.S_OK;
        }

        // This is the key one: fires when a document is opened or reloaded
        public int OnAfterAttributeChange(uint docCookie, uint grfAttribs)
        {
            UpdateSidebarInfo(docCookie);

            return VSConstants.S_OK;
        }

        private void UpdateSidebarInfo(uint docCookie)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var rdt = new RunningDocumentTable(this);
            var docInfo = rdt.GetDocumentInfo(docCookie);

            string docPath = docInfo.Moniker;
            string projectPath = null;

            if (docInfo.Hierarchy is IVsHierarchy hierarchy)
            {
                hierarchy.GetCanonicalName(VSConstants.VSITEMID_ROOT, out projectPath);
            }

            UpdateSidebarInfo(docPath, projectPath);
        }

        private void UpdateSidebarInfo(string docPath, string projectPath)
        {
            var window = this.FindToolWindow(typeof(MySidebarToolWindow), 0, true) as MySidebarToolWindow;
            if (window?.Content is MySidebarControl control)
            {
                control.UpdateInfo(docPath, projectPath);
            }
        }

        private static void AddMsgTask(
            string message,
            TaskErrorCategory category,
            int line = -1,
            int column = -1)
        {
            _errorListProvider.Tasks.Add(new ErrorTask
            {
                Category = TaskCategory.User,
                ErrorCategory = category,
                Text = message,
                Line = line,
                Column = column,
            });
        }

        public const string PackageGuidString = "aaaba8d5-1180-4bf8-8821-345f72a4cb79";
        public const string Name = "Alternet.UI.Integration.VisualStudio";

        public static SolutionService SolutionService { get; private set; }

        public static JoinableTaskFactory JTF { get; private set; }

        internal IVsWindowFrame FindDocumentFrame(string fileName)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var openDoc = GetService(typeof(SVsUIShellOpenDocument)) as IVsUIShellOpenDocument;
            Assumes.Present(openDoc);

            Guid logicalView = VSConstants.LOGVIEWID_Primary;


            int hr = openDoc.IsDocumentOpen(
                pHierCaller: null,
                itemidCaller: VSConstants.VSITEMID_NIL,
                pszMkDocument: fileName,
                rguidLogicalView: ref logicalView,
                grfIDO: 0,
                ppHierOpen: out _,
                pitemidOpen: null,
                ppWindowFrame: out IVsWindowFrame frame,
                pfOpen: out int isOpen);

            if (hr >= 0 && isOpen != 0)
                return frame;

            return null;
        }

        protected override async Task InitializeAsync(
            CancellationToken cancellationToken,
            IProgress<ServiceProgressData> progress)
        {
            JTF = JoinableTaskFactory;

            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            InitializeLogging();

            if (await GetServiceAsync(typeof(SVsRunningDocumentTable)) is IVsRunningDocumentTable rdt)
            {
                rdt.AdviseRunningDocTableEvents(this, out _rdtCookie);
            }

            if (await GetServiceAsync(typeof(IMenuCommandService)) is OleMenuCommandService commandService)
            {
                // Command ID must match your VSCT Guid + IDSymbol
                var cmdId = new CommandID(new Guid("A1B2C3D4-E5F6-47A8-ABCD-1234567890AB"), 0x0100);


                var menuItem = new OleMenuCommand((s, e) => ShowSidebar(), cmdId);
                menuItem.Enabled = true;
                menuItem.Supported = true;

                menuItem.BeforeQueryStatus += (s, e) =>
                {
                    ThreadHelper.ThrowIfNotOnUIThread();
                    menuItem.Visible = true; // always visible on toolbar
                    menuItem.Enabled = true;
                };

                commandService.AddCommand(menuItem);
            }

            var monitorSelection = await GetServiceAsync(typeof(SVsShellMonitorSelection)) as IVsMonitorSelection;
            if (monitorSelection != null)
            {
                monitorSelection.AdviseSelectionEvents(this, out _selectionCookie);
            }

            RegisterEditorFactory(new EditorFactory(this));

            var dte = (DTE)await GetServiceAsync(typeof(DTE));
            SolutionService = new SolutionService(dte);

            Log.Information("AlterNET UI Package initialized");
        }

        bool outputPaneLoggingEnabled = true;
        private uint _selectionCookie;
        private readonly Dictionary<string, int> _pendingReopens =
            new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        private string GetSelectedFilePath()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var monitorSelection = (IVsMonitorSelection)GetService(typeof(SVsShellMonitorSelection));
            if (monitorSelection == null)
                return null;

            IntPtr hierarchyPtr;
            uint itemid;
            IVsMultiItemSelect multiItemSelect;
            IntPtr selectionContainerPtr;

            monitorSelection.GetCurrentSelection(out hierarchyPtr, out itemid, out multiItemSelect, out selectionContainerPtr);

            if (hierarchyPtr == IntPtr.Zero)
                return null;

            var hierarchy = Marshal.GetObjectForIUnknown(hierarchyPtr) as IVsHierarchy;
            Marshal.Release(hierarchyPtr);

            if (hierarchy == null)
                return null;

            object value;
            hierarchy.GetProperty(itemid, (int)__VSHPROPID.VSHPROPID_Name, out value);
            hierarchy.GetProperty(itemid, (int)__VSHPROPID.VSHPROPID_ExtObject, out value);

            var projItem = value as EnvDTE.ProjectItem;
            return projItem?.FileNames[1];
        }

        private void ShowSidebar()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var window = this.FindToolWindow(typeof(MySidebarToolWindow), 0, true);
            if (window?.Frame is IVsWindowFrame frame)
            {
                Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(frame.Show());
            }
        }

        internal void ScheduleReopen(string fileName, int maxRetries = 5, Action onOpened = null)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (_pendingReopens.TryGetValue(fileName, out var currentRetry))
            {
                if (currentRetry >= maxRetries)
                    return;

                _pendingReopens[fileName] = currentRetry + 1;
            }
            else
            {
                _pendingReopens[fileName] = 1;
            }

            JoinableTaskFactory.RunAsync(async delegate
            {
                try
                {
                    await Task.Delay(400);
                    await JoinableTaskFactory.SwitchToMainThreadAsync();

                    if (OpenWithMyEditor(fileName))
                    {
                        _pendingReopens.Remove(fileName);
                        onOpened?.Invoke();
                    }
                    else
                    {
                        if (_pendingReopens.TryGetValue(fileName, out var retry) && retry < maxRetries)
                        {
                            ScheduleReopen(fileName, maxRetries, onOpened);
                        }
                        else
                        {
                            _pendingReopens.Remove(fileName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Information($"Reopen failed for {fileName}: {ex}");
                    _pendingReopens.Remove(fileName);
                }
            });
        }        

        internal bool OpenWithMyEditorOld(string fileName)
        {
            try
            {
                ThreadHelper.ThrowIfNotOnUIThread();

                var openDoc = GetService(typeof(SVsUIShellOpenDocument)) as IVsUIShellOpenDocument;
                Assumes.Present(openDoc);

                Guid editorGuid = new Guid(EditorFactory.EditorFactoryGuidString);
                Guid logicalView = VSConstants.LOGVIEWID_Primary;

                Microsoft.VisualStudio.OLE.Interop.IServiceProvider sp;
                IVsUIHierarchy hierarchy;
                uint itemId;
                IVsWindowFrame windowFrame;

                int hr = openDoc.OpenDocumentViaProject(
                    pszMkDocument: fileName,
                    rguidLogicalView: ref logicalView,
                    out sp,
                    out hierarchy,
                    out itemId,
                    out windowFrame);

                if (ErrorHandler.Failed(hr))
                {
                    throw new COMException($"OpenDocumentViaProject failed for '{fileName}'", hr);
                }

                ErrorHandler.ThrowOnFailure(openDoc.OpenDocumentViaProjectWithSpecific(
                    pszMkDocument: fileName,
                    grfEditorFlags: (uint)__VSOSEFLAGS.OSE_ChooseBestStdEditor,
                    rguidEditorType: ref editorGuid,
                    pszPhysicalView: null,
                    rguidLogicalView: ref logicalView,
                    ppSP: out sp,
                    ppHier: out hierarchy,
                    pitemid: out itemId,
                    ppWindowFrame: out windowFrame));

                return true;
            }
            catch
            {
                return false;
            }
        }

        internal bool OpenWithMyEditor(string fileName)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            try
            {
                var openDoc = GetService(typeof(SVsUIShellOpenDocument)) as IVsUIShellOpenDocument;
                Assumes.Present(openDoc);

                Guid editorGuid = new Guid(EditorFactory.EditorFactoryGuidString);
                Guid logicalView = VSConstants.LOGVIEWID_Primary;

                Microsoft.VisualStudio.OLE.Interop.IServiceProvider sp;
                IVsUIHierarchy hierarchy;
                uint itemId;
                IVsWindowFrame windowFrame;

                int hr = openDoc.OpenDocumentViaProjectWithSpecific(
                    pszMkDocument: fileName,
                    grfEditorFlags: (uint)__VSOSEFLAGS.OSE_ChooseBestStdEditor,
                    rguidEditorType: ref editorGuid,
                    pszPhysicalView: null,
                    rguidLogicalView: ref logicalView,
                    ppSP: out sp,
                    ppHier: out hierarchy,
                    pitemid: out itemId,
                    ppWindowFrame: out windowFrame);


                if (hr < 0)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                Log.Information($"OpenWithMyEditor failed for {fileName}: {ex}");
                return false;
            }
        }

        private static void Write(string message)
        {
            AlternetUIPackage.JTF.Run(() =>
            {
                return WriteAsync(message);
            });
        }

        private static async Task WriteAsync(string message)
        {
            var jtf = AlternetUIPackage.JTF;

            if (jtf is null)
                return;

            await jtf.SwitchToMainThreadAsync();

            AlternetUIPackage.OutputPane?.OutputStringThreadSafe(
                DateTime.Now + ": " + message + Environment.NewLine);
        }

        private void InitializeLogging()
        {
            if (outputPaneLoggingEnabled)
                InitializeOutputPaneLogging();
        }

        private void InitializeOutputPaneLogging()
        {
            /*const string format = "{Timestamp:HH:mm:ss.fff} [{Level}] {Pid} {Message}{NewLine}{Exception}";*/

            ThreadHelper.ThrowIfNotOnUIThread();
            OutputWindow = this.GetService<IVsOutputWindow, SVsOutputWindow>();

            InitializeMsg(this);

            _guid = Guid.NewGuid();
            OutputWindow?.CreatePane(ref _guid, "Alternet.UI", 1, 1);
            OutputWindow?.GetPane(ref _guid, out OutputPane);

            var settings = this.GetMefService<IAlternetUIVisualStudioSettings>();
        }

        public int OnSelectionChanged(IVsHierarchy pHierOld, uint itemidOld, IVsMultiItemSelect pMISOld, ISelectionContainer pSCOld, IVsHierarchy pHierNew, uint itemidNew, IVsMultiItemSelect pMISNew, ISelectionContainer pSCNew)
        {
            return VSConstants.S_OK;
        }

        public int OnElementValueChanged(uint elementid, object varValueOld, object varValueNew)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (elementid == (uint)VSConstants.VSSELELEMID.SEID_DocumentFrame)
            {
                if (varValueNew is IVsWindowFrame frame)
                {
                    frame.GetProperty((int)__VSFPROPID.VSFPROPID_pszMkDocument, out object docPathObj);
                    string docPath = docPathObj as string;

                    frame.GetProperty((int)__VSFPROPID.VSFPROPID_Hierarchy, out object hierarchyObj);
                    string projectPath = null;
                    if (hierarchyObj is IVsHierarchy hierarchy)
                    {
                        hierarchy.GetCanonicalName(VSConstants.VSITEMID_ROOT, out projectPath);
                    }

                    var window = FindToolWindow(typeof(MySidebarToolWindow), 0, true) as MySidebarToolWindow;
                    if (window?.Content is MySidebarControl control)
                    {
                        control.UpdateInfo(docPath, projectPath);
                    }
                }
            }
            return VSConstants.S_OK;
        }

        public int OnCmdUIContextChanged(uint dwCmdUICookie, int fActive)
        {
            return VSConstants.S_OK;
        }
    }
}
