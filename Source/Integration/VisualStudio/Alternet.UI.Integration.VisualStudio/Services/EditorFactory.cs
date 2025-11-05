using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Alternet.UI.Integration.VisualStudio.Models;
using Alternet.UI.Integration.VisualStudio.Views;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;

using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

namespace Alternet.UI.Integration.VisualStudio.Services
{
    /// <summary>
    /// Implements <see cref="IVsEditorFactory"/> to create <see cref="DesignerPane"/>s containing
    /// uixml designer.
    /// </summary>
    internal sealed class EditorFactory : IVsEditorFactory, IVsSimpleDocFactory, IDisposable
    {
        private static readonly Guid XmlLanguageServiceGuid = new Guid("f6819a78-a205-47b5-be1c-675b3c7f0b8e");
        private static readonly Guid XamlLanguageServiceGuid = new Guid("cd53c9a1-6bc2-412b-be36-cc715ed8dd41");
        private readonly AlternetUIPackage _package;
        private IOleServiceProvider _oleServiceProvider;
        private ServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorFactory"/> class.
        /// </summary>
        /// <param name="package">The package that the factory belongs to.</param>
        public EditorFactory(AlternetUIPackage package) => _package = package;

        /// <inheritdoc/>
        public int SetSite(IOleServiceProvider psp)
        {
            _oleServiceProvider = psp;
            _serviceProvider = new ServiceProvider(psp);
            return VSConstants.S_OK;
        }

        /// <inheritdoc/>
        public int MapLogicalView(ref Guid rguidLogicalView, out string pbstrPhysicalView)
        {
            pbstrPhysicalView = null;

            if (rguidLogicalView == VSConstants.LOGVIEWID_Primary ||
                rguidLogicalView == VSConstants.LOGVIEWID_Code ||
                rguidLogicalView == VSConstants.LOGVIEWID_Debugging ||
                rguidLogicalView == VSConstants.LOGVIEWID_TextView ||
                rguidLogicalView == VSConstants.LOGVIEWID_Designer)
            {
                return VSConstants.S_OK;
            }

            return VSConstants.E_NOTIMPL;
        }

        // Returns an IVsTextLines for the file content, or null on failure.
        // If the document is already open, returns the existing doc data's IVsTextLines.
        // If not, creates an in-memory IVsTextLines populated with the file text.
        public async Task<IVsTextLines> LoadFromFileAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentNullException(nameof(filePath));

            // 1) Try to get existing doc data from RDT (must run on UI thread)
            await _package.JoinableTaskFactory.SwitchToMainThreadAsync(_package.DisposalToken);

            var sp = _package as IServiceProvider;
            var rdt = sp.GetService(typeof(SVsRunningDocumentTable)) as IVsRunningDocumentTable;
            if (rdt != null)
            {
                // pass flags = 0 (no lock) — or use read-lock if you will hold doc data
                int hr = rdt.FindAndLockDocument(0, filePath, out IVsHierarchy hier, out uint itemid, out IntPtr punkDocData, out uint docCookie);
                if (hr == VSConstants.S_OK && punkDocData != IntPtr.Zero)
                {
                    try
                    {
                        object comObj = Marshal.GetObjectForIUnknown(punkDocData);
                        var vsTextLines = comObj as IVsTextLines;
                        if (vsTextLines != null)
                        {
                            // release the pointer we got from RDT (FindAndLockDocument gives you an extra ref)
                            Marshal.Release(punkDocData);
                            return vsTextLines;
                        }

                        // If docdata is not IVsTextLines, it may be an editor-specific docdata. You can try to adapt it.
                        // e.g. if it's an ITextBuffer -> get IVsTextLines via adapter.GetBufferAdapter(...).
                    }
                    finally
                    {
                        // Make sure to release the COM pointer if not null (we released above when returning)
                        try { if (punkDocData != IntPtr.Zero) Marshal.Release(punkDocData); } catch { }
                    }
                }
            }

            // 2) Fallback: create an in-memory IVsTextLines and populate it with the file contents.
            // Read file off the UI thread to avoid blocking UI.
            string fileText = await Task.Run(() => File.Exists(filePath) ? File.ReadAllText(filePath) : string.Empty);

            // Switch to UI thread for adapter/IVs operations
            await _package.JoinableTaskFactory.SwitchToMainThreadAsync(_package.DisposalToken);

            // Try MEF adapter route first so the IVsTextLines is integrated with the ITextBuffer ecosystem.
            var componentModel = (IComponentModel)_package.GetService<SComponentModel>();
            if (componentModel != null)
            {
                var adapter = componentModel.GetService<IVsEditorAdaptersFactoryService>();
                var textFactory = componentModel.GetService<ITextBufferFactoryService>();
                var contentTypeRegistry = componentModel.GetService<Microsoft.VisualStudio.Utilities.IContentTypeRegistryService>();

                if (adapter != null && textFactory != null && contentTypeRegistry != null)
                {
                    var contentType = textFactory.PlaintextContentType ?? contentTypeRegistry.GetContentType("plaintext") ?? contentTypeRegistry.GetContentType("text");
                    if (contentType != null)
                    {
                        // Create an ITextBuffer with file content and ask adapter for IVsTextLines
                        ITextBuffer tb = textFactory.CreateTextBuffer(fileText, contentType);
                        var vsFromAdapter = adapter.GetBufferAdapter(tb) as IVsTextLines;
                        if (vsFromAdapter != null)
                            return vsFromAdapter;
                    }

                    // If adapter.GetBufferAdapter returns null (environment may not map this content type),
                    // create a VS-com buffer via adapter.CreateVsTextBufferAdapter and populate it.
                    var vsBuf = adapter.CreateVsTextBufferAdapter(_oleServiceProvider) as IVsTextLines;
                    if (vsBuf != null)
                    {
                        ReplaceEntireBufferWithText(vsBuf, fileText);
                        return vsBuf;
                    }
                }
            }

            // 3) Last chance: create a raw COM VsTextBufferClass and fill it
            try
            {
                var comBuf = new Microsoft.VisualStudio.TextManager.Interop.VsTextBufferClass() as IVsTextLines;
                if (comBuf != null)
                {
                    ReplaceEntireBufferWithText(comBuf, fileText);
                    return comBuf;
                }
            }
            catch
            {
                // ignore and return null below
            }

            return null;
        }

        public (IVsCodeWindow codeWindow, IWpfTextViewHost wpfHost) CreateCodeWindowAndHost(
                IServiceProvider serviceProvider,
                IVsTextLines vsTextLines)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
            if (vsTextLines == null) throw new ArgumentNullException(nameof(vsTextLines));

            var componentModel = (IComponentModel)serviceProvider.GetService(typeof(SComponentModel));
            if (componentModel == null) throw new InvalidOperationException("SComponentModel not available.");

            var adapter = componentModel.GetService<IVsEditorAdaptersFactoryService>();
            var textEditorFactory = componentModel.GetService<ITextEditorFactoryService>();
            var textBufferFactory = componentModel.GetService<ITextBufferFactoryService>();
            var contentTypeRegistry = componentModel.GetService<IContentTypeRegistryService>();

            if (adapter == null || textEditorFactory == null || textBufferFactory == null || contentTypeRegistry == null)
                throw new InvalidOperationException("Required editor services are not available.");

            IVsCodeWindow codeWindow = null;
            IWpfTextViewHost host = null;

            // 1) Create IVsCodeWindow and set its buffer
            codeWindow = adapter.CreateVsCodeWindowAdapter(_oleServiceProvider) as IVsCodeWindow;
            if (codeWindow == null)
            {
                // Could not obtain a code window adapter; fall back to creating a WPF view host only.
                host = CreateWpfHostFromVsTextLinesBackingBuffer(adapter, textEditorFactory, textBufferFactory, contentTypeRegistry, vsTextLines);
                return (null, host);
            }

            ErrorHandler.ThrowOnFailure(codeWindow.SetBuffer(vsTextLines));

            // 2) Try to get an existing primary IVsTextView from the code window
            IVsTextView primaryView = null;
            try
            {
                // GetPrimaryView should be present on IVsCodeWindow
                codeWindow.GetPrimaryView(out primaryView);
            }
            catch
            {
                primaryView = null;
            }

            // 3) If no primary view, create one and try to set it as primary.
            if (primaryView == null)
            {
                IVsTextView newView = adapter.CreateVsTextViewAdapter(_oleServiceProvider) as IVsTextView;
                if (newView != null)
                {
                    bool setSucceeded = false;

                    // Try calling SetPrimaryView if the runtime type implements it (use reflection)
                    try
                    {
                        var mi = codeWindow.GetType().GetMethod("SetPrimaryView", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                        if (mi != null)
                        {
                            mi.Invoke(codeWindow, new object[] { newView });
                            setSucceeded = true;

                            // Try to read back primaryView
                            codeWindow.GetPrimaryView(out primaryView);
                        }
                    }
                    catch
                    {
                        setSucceeded = false;
                    }

                    // If reflection failed, try cast to dynamic as last resort (may succeed if runtime type exposes the method)
                    if (!setSucceeded)
                    {
                        try
                        {
                            dynamic dyn = codeWindow;
                            dyn.SetPrimaryView((object)newView);
                            setSucceeded = true;
                            codeWindow.GetPrimaryView(out primaryView);
                        }
                        catch
                        {
                            setSucceeded = false;
                        }
                    }

                    // If we still don't have a primary view, use the created IVsTextView as a candidate
                    if (primaryView == null)
                    {
                        primaryView = newView;
                    }
                }
            }

            // 4) Obtain an IWpfTextView from the IVsTextView (preferred) or create a host from backing ITextBuffer
            IWpfTextView wpfTextView = null;
            if (primaryView != null)
            {
                try
                {
                    wpfTextView = adapter.GetWpfTextView(primaryView);
                }
                catch { wpfTextView = null; }
            }

            if (wpfTextView == null)
            {
                // Try to obtain backing ITextBuffer from IVsTextLines and create WPF view from it
                host = CreateWpfHostFromVsTextLinesBackingBuffer(adapter, textEditorFactory, textBufferFactory, contentTypeRegistry, vsTextLines);
            }
            else
            {
                host = textEditorFactory.CreateTextViewHost(wpfTextView, setFocus: false);
            }

            return (codeWindow, host);
        }

        // Helper: try to get backing ITextBuffer from adapter or read text and create a new ITextBuffer, then create a WPF host.
        private static IWpfTextViewHost CreateWpfHostFromVsTextLinesBackingBuffer(
            IVsEditorAdaptersFactoryService adapter,
            ITextEditorFactoryService textEditorFactory,
            ITextBufferFactoryService textBufferFactory,
            IContentTypeRegistryService contentTypeRegistry,
            IVsTextLines vsTextLines)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            ITextBuffer textBuffer = null;

            try
            {
                // Some adapter implementations expose GetDataBuffer(IVsTextLines)
                textBuffer = adapter.GetDataBuffer(vsTextLines);
            }
            catch
            {
                textBuffer = null;
            }

            if (textBuffer == null)
            {
                // Fallback: read contents from IVsTextLines and create new ITextBuffer (independent buffer)
                string content = ReadAllTextFromVsTextLines(vsTextLines);
                var ctype = textBufferFactory.PlaintextContentType
                            ?? contentTypeRegistry.GetContentType("plaintext")
                            ?? contentTypeRegistry.GetContentType("text")
                            ?? contentTypeRegistry.ContentTypes.FirstOrDefault();
                textBuffer = textBufferFactory.CreateTextBuffer(content ?? string.Empty, ctype);
            }

            var view = textEditorFactory.CreateTextView(textBuffer);
            var host = textEditorFactory.CreateTextViewHost(view, setFocus: false);
            return host;
        }

        // Returns IWpfTextViewHost bound to the same text content as vsTextLines.
        // Must be called on the UI thread.
        public static IWpfTextViewHost CreateWpfHostForVsTextLines(IServiceProvider serviceProvider, IVsTextLines vsTextLines)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
            if (vsTextLines == null) throw new ArgumentNullException(nameof(vsTextLines));

            var componentModel = (IComponentModel)serviceProvider.GetService(typeof(SComponentModel));
            if (componentModel == null) throw new InvalidOperationException("SComponentModel not available.");

            var adapter = componentModel.GetService<IVsEditorAdaptersFactoryService>();
            var textFactory = componentModel.GetService<ITextBufferFactoryService>();
            var textEditorFactory = componentModel.GetService<ITextEditorFactoryService>();
            var contentTypeRegistry = componentModel.GetService<IContentTypeRegistryService>();

            if (adapter == null || textFactory == null || textEditorFactory == null || contentTypeRegistry == null)
                throw new InvalidOperationException("Required editor services are not available.");

            // 1) Try to get the ITextBuffer that backs this IVsTextLines (adapter may support this)
            ITextBuffer textBuffer = null;
            try
            {
                // Some adapter implementations expose GetDataBuffer(IVsTextLines)
                // If your adapter has a different method name, consider reflection or alternative mapping.
                textBuffer = adapter.GetDataBuffer(vsTextLines);
            }
            catch
            {
                textBuffer = null;
            }

            // 2) If no backing ITextBuffer, read the IVsTextLines contents and create a new ITextBuffer
            if (textBuffer == null)
            {
                string content = ReadAllTextFromVsTextLines(vsTextLines);
                var contentType = textFactory.PlaintextContentType
                                  ?? contentTypeRegistry.GetContentType("plaintext")
                                  ?? contentTypeRegistry.GetContentType("text")
                                  ?? contentTypeRegistry.ContentTypes.FirstOrDefault();

                textBuffer = textFactory.CreateTextBuffer(content ?? string.Empty, contentType);
            }

            // 3) Create an IWpfTextView and host
            var wpfView = textEditorFactory.CreateTextView(textBuffer);
            var host = textEditorFactory.CreateTextViewHost(wpfView, setFocus: false);

            return host;
        }

        // Helper: read entire text from IVsTextLines (UI thread)
        private static string ReadAllTextFromVsTextLines(IVsTextLines vsTextLines)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (vsTextLines == null) return string.Empty;

            ErrorHandler.ThrowOnFailure(vsTextLines.GetLineCount(out int lineCount));
            var sb = new StringBuilder();
            for (int i = 0; i < lineCount; i++)
            {
                // get line length safely
                ErrorHandler.ThrowOnFailure(vsTextLines.GetLengthOfLine(i, out int len));
                // try length-based GetLineText, fallback to MaxValue
                try
                {
                    ErrorHandler.ThrowOnFailure(vsTextLines.GetLineText(i, 0, i, len, out string txt));
                    sb.Append(txt);
                }
                catch (COMException)
                {
                    ErrorHandler.ThrowOnFailure(vsTextLines.GetLineText(i, 0, i, int.MaxValue, out string txt));
                    sb.Append(txt);
                }
                if (i != lineCount - 1) sb.Append('\n');
            }
            return sb.ToString();
        }

        public (IVsCodeWindow codeWindow, IWpfTextViewHost textViewHost) CreateCodeWindowAndWpfHost(
               IServiceProvider serviceProvider,
               IVsTextLines vsTextLines)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
            if (vsTextLines == null) throw new ArgumentNullException(nameof(vsTextLines));

            // Get MEF services
            var componentModel = (IComponentModel)serviceProvider.GetService(typeof(SComponentModel));
            if (componentModel == null) throw new InvalidOperationException("SComponentModel not available.");

            var adapter = componentModel.GetService<IVsEditorAdaptersFactoryService>();
            var textEditorFactory = componentModel.GetService<Microsoft.VisualStudio.Text.Editor.ITextEditorFactoryService>();
            if (adapter == null || textEditorFactory == null)
                throw new InvalidOperationException("Required editor services are not available.");

            // 1) Create a VsCodeWindow (COM adapter)
            var codeWindow = adapter.CreateVsCodeWindowAdapter(_oleServiceProvider) as IVsCodeWindow;
            if (codeWindow == null)
                throw new InvalidOperationException("CreateVsCodeWindowAdapter returned null.");

            // 2) Set the buffer (IVsTextLines)
            ErrorHandler.ThrowOnFailure(codeWindow.SetBuffer(vsTextLines));

            // 3) Create an IVsTextView and set it as the primary view of the code window
            var vsTextView = adapter.CreateVsTextViewAdapter(_oleServiceProvider) as IVsTextView;
            if (vsTextView == null)
                throw new InvalidOperationException("CreateVsTextViewAdapter returned null.");

            // 4) Obtain the IWpfTextView corresponding to the IVsTextView
            var wpfTextView = adapter.GetWpfTextView(vsTextView);
            if (wpfTextView == null)
            {
                // In some cases the adapter returns null until the view is properly initialized.
                // Try to get it by asking the adapter to provide a view for the underlying ITextBuffer if possible.
                // Attempt alternative: if we can get ITextBuffer from IVsTextLines, create IWpfTextView from that.
                ITextBuffer textBuffer = null;
                try
                {
                    textBuffer = adapter.GetDataBuffer(vsTextLines); // may return null in some environments
                }
                catch { /* ignore */ }

                if (textBuffer != null)
                {
                    var createdView = textEditorFactory.CreateTextView(textBuffer);
                    if (createdView != null)
                    {
                        // If we created the view directly, get a host from the factory
                        var host = textEditorFactory.CreateTextViewHost(createdView, false);
                        return (codeWindow, host);
                    }
                }

                throw new InvalidOperationException("Could not obtain IWpfTextView from IVsTextView or from data buffer.");
            }

            // 5) Create IWpfTextViewHost from the IWpfTextView
            var wpfHost = textEditorFactory.CreateTextViewHost(wpfTextView, false);
            if (wpfHost == null)
                throw new InvalidOperationException("CreateTextViewHost returned null.");

            // Optionally, you can call codeWindow.GetPrimaryView(out var checkView) and verify adapter.GetWpfTextView(checkView) matches.
            return (codeWindow, wpfHost);
        }

        public int CreateEditorInstance(
            uint grfCreateDoc,
            string pszMkDocument,
            string pszPhysicalView,
            IVsHierarchy pvHier,
            uint itemid,
            IntPtr punkDocDataExisting,
            out IntPtr ppunkDocView,
            out IntPtr ppunkDocData,
            out string pbstrEditorCaption,
            out Guid pguidCmdUI,
            out int pgrfCDW)
        {
            const string textNotReady = "Uixml document is not yet ready. Please wait some time and reload it...";

            ThreadHelper.ThrowIfNotOnUIThread();

            Log.Verbose($"Started EditorFactory.CreateEditorInstance({pszMkDocument})");

            ppunkDocView = IntPtr.Zero;
            ppunkDocData = IntPtr.Zero;
            pguidCmdUI = Guids.AtlernetUIEditorFactory;
            pgrfCDW = 0;
            pbstrEditorCaption = string.Empty;

            if ((grfCreateDoc & (VSConstants.CEF_OPENFILE | VSConstants.CEF_SILENT)) == 0)
            {
                return VSConstants.E_INVALIDARG;
            }

            IVsTextLines textBuffer = null;

            var project = GetProject(pvHier);

            if (project == null)
            {
                return VSConstants.VS_E_BUSY;
            }
            else
            {
                textBuffer = GetTextBuffer(pszMkDocument, punkDocDataExisting);
            }

            if (textBuffer == null)
            {
                /* textBuffer = GetTextBuffer2(pszMkDocument, punkDocDataExisting);*/

                var isDummy = false;

                if (textBuffer == null)
                {
                    textBuffer = CreateVsTextLinesFromString(_oleServiceProvider, _serviceProvider, textNotReady);
                    isDummy = true;
                }

                (IVsCodeWindow codeWindow, IWpfTextViewHost textViewHost) = CreateCodeWindowAndWpfHost(
                   _serviceProvider,
                   textBuffer);

                var pane2 = new DesignerPane(project, pszMkDocument, codeWindow, textViewHost, isDummy);
                ppunkDocView = Marshal.GetIUnknownForObject(pane2);
                ppunkDocData = Marshal.GetIUnknownForObject(textBuffer);
                Log.Verbose($"Finished EditorFactory.CreateEditorInstance({pszMkDocument})");
                return VSConstants.S_OK;
            }

            if (textBuffer == null)
                return VSConstants.VS_E_BUSY;

            var (editorWindow, editorControl) = CreateEditorControl(textBuffer);
            var pane = new DesignerPane(project, pszMkDocument, editorWindow, editorControl);
            ppunkDocView = Marshal.GetIUnknownForObject(pane);
            ppunkDocData = Marshal.GetIUnknownForObject(textBuffer);

            Log.Verbose($"Finished EditorFactory.CreateEditorInstance({pszMkDocument})");
            return VSConstants.S_OK;
        }

        /// <inheritdoc/>
        public int Close() => VSConstants.S_OK;

        /// <inheritdoc/>
        public void Dispose()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            _serviceProvider?.Dispose();
            _serviceProvider = null;
        }

        // Replaces entire contents of vsTextLines with 'text'. Throws on failure.
        public static void ReplaceEntireBufferWithText(IVsTextLines vsTextLines, string text)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (vsTextLines == null) throw new ArgumentNullException(nameof(vsTextLines));
            if (text == null) text = string.Empty;

            // Get current line count and last line length to compute replacement span
            int hr;
            hr = vsTextLines.GetLineCount(out int lineCount);
            ErrorHandler.ThrowOnFailure(hr);

            int startLine = 0;
            int startIndex = 0;

            // If buffer is empty, endLine/endIndex should be 0,0 (ReplaceLines will insert).
            int endLine = Math.Max(0, lineCount - 1);
            int endIndex = 0;
            if (lineCount > 0)
            {
                hr = vsTextLines.GetLengthOfLine(endLine, out endIndex);
                ErrorHandler.ThrowOnFailure(hr);
            }

            // Prepare unmanaged LPCWSTR pointer
            IntPtr pszText = IntPtr.Zero;
            try
            {
                pszText = Marshal.StringToCoTaskMemUni(text);

                var changed = new TextSpan[1];
                // ReplaceLines(startLine, startIndex, endLine, endIndex, pszText, iNewLen, changedSpan)
                hr = vsTextLines.ReplaceLines(
                    startLine,
                    startIndex,
                    endLine,
                    endIndex,
                    pszText,
                    text.Length,
                    changed);
                ErrorHandler.ThrowOnFailure(hr);

                // Optionally inspect changed[0] for debugging
                // changed[0].iStartLine, changed[0].iStartIndex, changed[0].iEndLine, changed[0].iEndIndex

                /*
                // VERIFY: read buffer back and ensure text is present
                var actual = ReadAllTextSafe(vsTextLines);
                if (!string.Equals(actual, text, StringComparison.Ordinal))
                {
                    // For debugging, throw with content snapshot
                    throw new InvalidOperationException($"ReplaceLines completed but buffer content does not match.\nExpected length={text.Length}\nActual length={actual?.Length ?? -1}\nActual preview: {Preview(actual)}");
                }
                */
            }
            finally
            {
                if (pszText != IntPtr.Zero) Marshal.FreeCoTaskMem(pszText);
            }
        }

        public static string ReadAllTextSafe(IVsTextLines vsTextLines)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (vsTextLines == null) throw new ArgumentNullException(nameof(vsTextLines));

            try
            {
                ErrorHandler.ThrowOnFailure(vsTextLines.GetLineCount(out int lineCount));

                var sb = new StringBuilder();

                for (int i = 0; i < lineCount; i++)
                {
                    // Get length of this line first (avoids invalid endIndex)
                    int hr = vsTextLines.GetLengthOfLine(i, out int lineLen);
                    ErrorHandler.ThrowOnFailure(hr);

                    // Try the length-based call first (safer)
                    try
                    {
                        hr = vsTextLines.GetLineText(i, 0, i, lineLen, out string lineText);
                        ErrorHandler.ThrowOnFailure(hr);
                        sb.Append(lineText);
                    }
                    catch (COMException)
                    {
                        // Fallback: try with int.MaxValue (some implementations expect this)
                        hr = vsTextLines.GetLineText(i, 0, i, int.MaxValue, out string fallbackText);
                        ErrorHandler.ThrowOnFailure(hr);
                        sb.Append(fallbackText);
                    }

                    if (i != lineCount - 1) sb.Append('\n');
                }

                return sb.ToString();
            }
            catch (COMException cex)
            {
                // Augment error with runtime type for easier debugging
                string typeName;
                try { typeName = vsTextLines.GetType().FullName; } catch { typeName = "<unknown>"; }

                throw new InvalidOperationException(
                    $"IVsTextLines ({typeName}) GetLineText/Related call failed with HRESULT 0x{cex.ErrorCode:X8}: {cex.Message}",
                    cex);
            }
        }

        // Extra helper to gather diagnostics without throwing for quick inspection
        public static string GetBufferDiagnostics(IVsTextLines vsTextLines)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (vsTextLines == null) return "vsTextLines == null";

            var sb = new StringBuilder();
            sb.AppendLine($"Type: {vsTextLines.GetType().FullName}");
            try
            {
                ErrorHandler.ThrowOnFailure(vsTextLines.GetLineCount(out int lineCount));
                sb.AppendLine($"LineCount: {lineCount}");
                for (int i = 0; i < Math.Min(20, lineCount); i++)
                {
                    ErrorHandler.ThrowOnFailure(vsTextLines.GetLengthOfLine(i, out int len));
                    sb.AppendLine($"Line {i}: length={len}");
                }
            }
            catch (COMException cex)
            {
                sb.AppendLine($"Diagnostic call failed with HRESULT 0x{cex.ErrorCode:X8}: {cex.Message}");
            }
            return sb.ToString();
        }

        static string Preview(string s) => s == null ? "<null>" : s.Length > 200 ? s.Substring(0, 200) + "..." : s;

        public static IVsTextLines CreateVsTextLinesFromString(
            IOleServiceProvider psp,
            IServiceProvider serviceProvider,
            string text)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var componentModel = (IComponentModel)serviceProvider.GetService(typeof(SComponentModel));
            if (componentModel == null) return null;

            var textBufferFactory = componentModel.GetService<ITextBufferFactoryService>();
            var contentTypeRegistry = componentModel.GetService<IContentTypeRegistryService>();
            var adapter = componentModel.GetService<IVsEditorAdaptersFactoryService>();

            if (textBufferFactory == null || contentTypeRegistry == null || adapter == null)
                return null;

            // Try preferred path: create ITextBuffer and get the IVs adapter for it.
            IContentType contentType =
                textBufferFactory.PlaintextContentType
                ?? contentTypeRegistry.GetContentType("plaintext")
                ?? contentTypeRegistry.GetContentType("text")
                ?? contentTypeRegistry.ContentTypes.FirstOrDefault();

            if (contentType != null)
            {
                ITextBuffer tb = textBufferFactory.CreateTextBuffer(text, contentType);
                if (tb != null)
                {
                    var vsFromAdapter = adapter.GetBufferAdapter(tb) as IVsTextLines;
                    if (vsFromAdapter != null)
                    {
                        ReplaceEntireBufferWithText(vsFromAdapter, text);
                        return vsFromAdapter;
                    }
                }
            }

            // Fallback: create a VS COM text buffer and populate it.
            // Pass a valid IServiceProvider (often your package) into CreateVsTextBufferAdapter.
            IVsTextLines vsTextLines = adapter.CreateVsTextBufferAdapter(psp) as IVsTextLines;
            if (vsTextLines != null)
            {
                ReplaceEntireBufferWithText(vsTextLines, text);
                return vsTextLines;
            }

            try
            {
                var comBuffer = new Microsoft.VisualStudio.TextManager.Interop.VsTextBufferClass() as IVsTextLines;
                if (comBuffer != null)
                {
                    ReplaceEntireBufferWithText(comBuffer, text);
                    return comBuffer;
                }
            }
            catch
            {
            }

            return null;
        }

        public static async Task<IVsTextLines> CreateVsTextLinesFromStringAsync(AsyncPackage package, string text)
        {
            // Ensure we're allowed to request MEF services
            await package.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            // Get the MEF component model
            var componentModel = (IComponentModel)await package.GetServiceAsync(typeof(SComponentModel));
            if (componentModel is null)
                return null;

            // Get services
            var textBufferFactory = componentModel.GetService<ITextBufferFactoryService>();
            var contentTypeRegistry = componentModel.GetService<IContentTypeRegistryService>();
            var adapter = componentModel.GetService<IVsEditorAdaptersFactoryService>();

            // Choose a content type. "text" or "plaintext" is a safe default; use "CSharp" etc. if you need language features.
            var contentType = contentTypeRegistry.GetContentType("text");

            // Create an ITextBuffer containing your text
            ITextBuffer textBuffer = textBufferFactory.CreateTextBuffer(text, contentType);

            // Get the Vs adapter (IVsTextLines / IVsTextBuffer)
            var vsTextLines = adapter.GetBufferAdapter(textBuffer) as IVsTextLines;
         
            return vsTextLines;
        }

        // Replace this method in your EditorFactory class
        private IVsTextLines GetTextBuffer2(string fileName, IntPtr punkDocDataExisting)
    {
        ThreadHelper.ThrowIfNotOnUIThread();

        Log.Verbose($"Started EditorFactory.GetTextBuffer({fileName})");

        IVsTextLines result = null;

        // 0) If caller provided docdata, prefer it
        if (punkDocDataExisting != IntPtr.Zero)
        {
            try
            {
                var obj = Marshal.GetObjectForIUnknown(punkDocDataExisting);
                result = obj as IVsTextLines;
                if (result == null)
                {
                    // incompatible docdata passed
                    ErrorHandler.ThrowOnFailure(VSConstants.VS_E_INCOMPATIBLEDOCDATA);
                }

                // success
                Log.Verbose("Using existing punkDocDataExisting as IVsTextLines");
                goto finalize;
            }
            catch (COMException)
            {
                // fall through to other attempts
            }
        }

        // 1) Try RDT (maybe document already open)
        var rdt = (IVsRunningDocumentTable)_serviceProvider.GetService(typeof(SVsRunningDocumentTable));
        if (rdt != null)
        {
            IntPtr punkDocData = IntPtr.Zero;
            try
            {
                int hr = rdt.FindAndLockDocument((uint)_VSRDTFLAGS.RDT_NoLock, fileName, out IVsHierarchy hier, out uint itemid, out punkDocData, out uint docCookie);
                if (hr == VSConstants.S_OK && punkDocData != IntPtr.Zero)
                {
                    try
                    {
                        var comObj = Marshal.GetObjectForIUnknown(punkDocData);
                        result = comObj as IVsTextLines;
                        if (result != null)
                        {
                            Log.Verbose("Found IVsTextLines in RDT");
                            goto finalize;
                        }
                    }
                    finally
                    {
                        // FindAndLockDocument returns an extra ref; release it
                        Marshal.Release(punkDocData);
                        punkDocData = IntPtr.Zero;
                    }
                }
            }
            catch (COMException) { /* ignore and continue to next approach */ }
        }

        // 2) Try Invisible Editor Manager (pass this factory instance as pFactory)
        var iem = (IVsInvisibleEditorManager)_serviceProvider.GetService(typeof(SVsInvisibleEditorManager));
        if (iem != null)
        {
            try
            {
                // IMPORTANT: pass 'this' as IVsEditorFactory so the invisible editor can create the right docdata.
                // If your class is not the IVsEditorFactory instance, pass the correct factory object instance here.
                IVsInvisibleEditor invisibleEditor = null;
                int opResult = iem.RegisterInvisibleEditor(
                    fileName,
                    pProject: null,
                    dwFlags: (uint)_EDITORREGFLAGS.RIEF_ENABLECACHING,
                    pFactory: this,
                    ppEditor: out invisibleEditor);

                if (ErrorHandler.Succeeded(opResult) && invisibleEditor != null)
                {
                    try
                    {
                        var guidIVSTextLines = typeof(IVsTextLines).GUID;
                        IntPtr docDataPtr = IntPtr.Zero;
                        // fEnsureWritable = 1
                        ErrorHandler.ThrowOnFailure(invisibleEditor.GetDocData(1, ref guidIVSTextLines, out docDataPtr));
                        if (docDataPtr != IntPtr.Zero)
                        {
                            try
                            {
                                result = Marshal.GetObjectForIUnknown(docDataPtr) as IVsTextLines;
                                if (result != null)
                                {
                                    Log.Verbose("Obtained IVsTextLines from invisible editor");
                                    goto finalize;
                                }
                            }
                            finally
                            {
                                Marshal.Release(docDataPtr);
                                docDataPtr = IntPtr.Zero;
                            }
                        }
                    }
                    finally
                    {
                        // Keep the invisibleEditor alive if needed. If you want to release immediately:
                        // Marshal.ReleaseComObject(invisibleEditor) (only if you don't need it).
                        // Typically invisible editors are kept around by the manager; don't release blindly.
                        // If you created the invisible editor intentionally, consider storing it for disposal.
                    }
                }
                else
                {
                    Log.Verbose($"RegisterInvisibleEditor returned 0x{opResult:X8}");
                }
            }
            catch (COMException ex)
            {
                Log.Verbose("RegisterInvisibleEditor/GetDocData failed: " + ex);
            }
        }
        else
        {
            Log.Verbose("SVsInvisibleEditorManager not available from service provider");
        }

        // 3) Fallback: create a plain in-memory IVsTextLines and populate it from disk.
        try
        {
            // Try adapter route first (MEF adapter -> IVsTextLines), else raw COM buffer
            var componentModel = (Microsoft.VisualStudio.ComponentModelHost.IComponentModel)_serviceProvider.GetService(typeof(SComponentModel));
            if (componentModel != null)
            {
                var adapter = componentModel.GetService<Microsoft.VisualStudio.Editor.IVsEditorAdaptersFactoryService>();
                if (adapter != null)
                {
                    var vsBuf = adapter.CreateVsTextBufferAdapter(_oleServiceProvider) as IVsTextLines;
                    if (vsBuf != null)
                    {
                        var fileText = File.Exists(fileName) ? File.ReadAllText(fileName) : string.Empty;
                        ReplaceEntireBufferWithText(vsBuf, fileText);
                        result = vsBuf;
                        Log.Verbose("Created IVsTextLines via adapter.CreateVsTextBufferAdapter and loaded file");
                        goto finalize;
                    }
                }
            }

            // Last resort: create VsTextBufferClass directly
            var comBuf = new Microsoft.VisualStudio.TextManager.Interop.VsTextBufferClass() as IVsTextLines;
            if (comBuf != null)
            {
                var fileText = File.Exists(fileName) ? File.ReadAllText(fileName) : string.Empty;
                ReplaceEntireBufferWithText(comBuf, fileText);
                result = comBuf;
                Log.Verbose("Created IVsTextLines via VsTextBufferClass and loaded file");
                goto finalize;
            }
        }
        catch (Exception ex)
        {
            Log.Verbose("Fallback buffer creation failed: " + ex);
        }

    finalize:
        if (result != null)
        {
            // Set XML language service (runs on UI thread)
            ErrorHandler.ThrowOnFailure(result.SetLanguageServiceID(XmlLanguageServiceGuid));
            Log.Verbose($"Finished EditorFactory.GetTextBuffer({fileName})");
            return result;
        }

        Log.Verbose($"EditorFactory.GetTextBuffer({fileName}) returning null");
        return null;
    }

        // IVsSimpleDocFactory.LoadDocument
        public int LoadDocument(string pszMkDocument, ref Guid riid, out IntPtr ppDocData)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            ppDocData = IntPtr.Zero;

            if (string.IsNullOrEmpty(pszMkDocument))
                return VSConstants.E_INVALIDARG;

            try
            {
                var iidTextLines = typeof(IVsTextLines).GUID;

                // Only support IVsTextLines here. Add other IIDs if needed.
                if (riid != iidTextLines)
                    return VSConstants.VS_E_INCOMPATIBLEDOCDATA;

                // 1) If the caller provided an EditorFactory and it exposes a docdata-creation method,
                //    prefer it. You may need to make a public CreateDocData/GetTextBuffer on EditorFactory.
                IVsTextLines vsTextLines = null;
         
                // 2) Try Running Document Table (document already open)
                if (vsTextLines == null)
                {
                    var rdt = (IVsRunningDocumentTable)_serviceProvider.GetService(typeof(SVsRunningDocumentTable));
                    if (rdt != null)
                    {
                        IntPtr punk = IntPtr.Zero;
                        try
                        {
                            int hr = rdt.FindAndLockDocument((uint)_VSRDTFLAGS.RDT_NoLock, pszMkDocument, out IVsHierarchy _, out uint _, out punk, out uint _);
                            if (hr == VSConstants.S_OK && punk != IntPtr.Zero)
                            {
                                object comObj = Marshal.GetObjectForIUnknown(punk);
                                vsTextLines = comObj as IVsTextLines;
                                Marshal.Release(punk); // release the RDT pointer
                            }
                        }
                        catch { if (punk != IntPtr.Zero) Marshal.Release(punk); }
                    }
                }

                // 3) Fallback: create an in-memory IVsTextLines via adapter or VsTextBufferClass and load file contents.
                if (vsTextLines == null)
                {
                    // Get file text off the UI if small; file I/O is OK here for fallback.
                    string text = File.Exists(pszMkDocument) ? File.ReadAllText(pszMkDocument) : string.Empty;

                    var componentModel = (IComponentModel)_serviceProvider.GetService(typeof(SComponentModel));
                    if (componentModel != null)
                    {
                        var adapter = componentModel.GetService<IVsEditorAdaptersFactoryService>();
                        if (adapter != null)
                        {
                            // Try to create a VS COM buffer via the adapter (preferred over raw VsTextBufferClass)
                            vsTextLines = adapter.CreateVsTextBufferAdapter(_oleServiceProvider) as IVsTextLines;
                            if (vsTextLines != null)
                            {
                                ReplaceEntireBufferWithText(vsTextLines, text);
                            }
                        }
                    }

                    if (vsTextLines == null)
                    {
                        // Last-resort raw COM buffer
                        try
                        {
                            var comBuf = new Microsoft.VisualStudio.TextManager.Interop.VsTextBufferClass() as IVsTextLines;
                            if (comBuf != null)
                            {
                                ReplaceEntireBufferWithText(comBuf, text);
                                vsTextLines = comBuf;
                            }
                        }
                        catch { /* ignore */ }
                    }
                }

                if (vsTextLines == null)
                    return VSConstants.E_FAIL;

                // Return a fresh IUnknown pointer for the caller; caller will Release it.
                ppDocData = Marshal.GetIUnknownForObject(vsTextLines);
                return VSConstants.S_OK;
            }
            catch (COMException cex)
            {
                return cex.ErrorCode;
            }
            catch
            {
                return VSConstants.E_FAIL;
            }
        }

        private IVsTextLines GetTextBuffer(string fileName, IntPtr punkDocDataExisting)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            IVsTextLines result;

            Log.Verbose($"Started EditorFactory.GetTextBuffer({fileName})");

            if (punkDocDataExisting == IntPtr.Zero)
            {
                // Get an invisible editor over the file. This is much easier than having to
                // manually figure out the right content type and language service, and it will
                // automatically associate the document with its owning project, meaning we will
                // get intellisense in our editor with no extra work.
                var iem = _serviceProvider.GetService<IVsInvisibleEditorManager, SVsInvisibleEditorManager>();

                var opResult = iem.RegisterInvisibleEditor(
                                    fileName,
                                    pProject: null,
                                    dwFlags: (uint)_EDITORREGFLAGS.RIEF_ENABLECACHING,
                                    pFactory: this,
                                    ppEditor: out var invisibleEditor);

                if (ErrorHandler.Failed(opResult))
                    return null;

                var guidIVSTextLines = typeof(IVsTextLines).GUID;

                ErrorHandler.ThrowOnFailure(invisibleEditor.GetDocData(
                    fEnsureWritable: 1,
                    riid: ref guidIVSTextLines,
                    ppDocData: out var docDataPointer));

                result = (IVsTextLines)Marshal.GetObjectForIUnknown(docDataPointer);
            }
            else
            {
                result = Marshal.GetObjectForIUnknown(punkDocDataExisting) as IVsTextLines;

                if (result == null)
                {
                    ErrorHandler.ThrowOnFailure(VSConstants.VS_E_INCOMPATIBLEDOCDATA);
                }
            }

            // Set buffer content type to XML. The default XAML content type will cause blue
            // squiggly lines to be displayed on the elements, as the XAML language service is
            // hard-coded as to the XAML dialects it supports and Avalonia isn't one of them :(
            ErrorHandler.ThrowOnFailure(result.SetLanguageServiceID(XmlLanguageServiceGuid));

            Log.Verbose($"Finished EditorFactory.GetTextBuffer({fileName})");
            return result;
        }

        private (IVsCodeWindow, IWpfTextViewHost) CreateEditorControl(IVsTextLines bufferAdapter)
        {
            Log.Verbose("Started EditorFactory.CreateEditorControl()");

            var componentModel = _serviceProvider.GetService<IComponentModel, SComponentModel>();
            var eafs = componentModel.GetService<IVsEditorAdaptersFactoryService>();
            var codeWindow = eafs.CreateVsCodeWindowAdapter(_oleServiceProvider);

            // Disable the splitter control on the editor as leaving it enabled causes a crash if the user
            // tries to use it here.
            ((IVsCodeWindowEx)codeWindow).Initialize(
                (uint)_codewindowbehaviorflags.CWB_DISABLESPLITTER,
                VSUSERCONTEXTATTRIBUTEUSAGE.VSUC_Usage_Filter,
                szNameAuxUserContext: "",
                szValueAuxUserContext: "",
                InitViewFlags: 0,
                pInitView: new INITVIEW[1]);

            // Add metadata to the buffer so we can identify it as containing Avalonia XAML.
            var buffer = eafs.GetDataBuffer(bufferAdapter);

            // HACK: VS has given us an uninitialized IVsTextLines in punkDocDataExisting. Not sure what
            // we can do here except tell VS to close the tab and reopen it.
            if (buffer == null)
            {
                ErrorHandler.ThrowOnFailure(VSConstants.VS_E_INCOMPATIBLEDOCDATA);
            }

            buffer.Properties.GetOrCreateSingletonProperty(() => new XamlBufferMetadata());

            ErrorHandler.ThrowOnFailure(codeWindow.SetBuffer(bufferAdapter));
            ErrorHandler.ThrowOnFailure(codeWindow.GetPrimaryView(out var textViewAdapter));

            // In VS2019 preview 3, the IWpfTextViewHost.HostControl comes parented. Remove the
            // control from its parent otherwise we can't reparent it. This is probably a bug
            // in the preview and can probably be removed later.
            var textViewHost = eafs.GetWpfTextViewHost(textViewAdapter);

            if (textViewHost.HostControl.Parent is Decorator parent)
            {
                parent.Child = null;
            }

            Log.Verbose("Finished EditorFactory.CreateEditorControl()");
            return (codeWindow, textViewHost);
        }

        private static Project GetProject(IVsHierarchy hierarchy)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            ErrorHandler.ThrowOnFailure(hierarchy.GetProperty(
                VSConstants.VSITEMID_ROOT,
                (int)__VSHPROPID.VSHPROPID_ExtObject,
                out var objProj));
            return objProj as Project;
        }
    }
}
