using System;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <include file="Interfaces/IWebBrowser.xml" path='doc/WebBrowser/*'/>
    [DefaultProperty("Name")]
    [DefaultEvent("Enter")]
    [ControlCategory("Common")]
    public partial class WebBrowser : Control, IWebBrowser
    {
        private static IWebBrowserFactoryHandler? factory;

        private readonly string defaultUrl = "about:blank";

        private IWebBrowserMemoryFS? fMemoryFS;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebBrowser"/> class.
        /// </summary>
        public WebBrowser()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebBrowser"/> class.
        /// </summary>
        public WebBrowser(string url)
        {
            defaultUrl = url;
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Events/FullScreenChanged/*'/>
        public event EventHandler<WebBrowserEventArgs>? FullScreenChanged;

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Events/ScriptMessageReceived/*'/>
        public event EventHandler<WebBrowserEventArgs>? ScriptMessageReceived;

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Events/ScriptResult/*'/>
        public event EventHandler<WebBrowserEventArgs>? ScriptResult;

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Events/Navigated/*'/>
        public event EventHandler<WebBrowserEventArgs>? Navigated;

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Events/Navigating/*'/>
        public event EventHandler<WebBrowserEventArgs>? Navigating;

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Events/BeforeBrowserCreate/*'/>
        public event EventHandler<WebBrowserEventArgs>? BeforeBrowserCreate;

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Events/Loaded/*'/>
        public event EventHandler<WebBrowserEventArgs>? Loaded;

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Events/Error/*'/>
        public event EventHandler<WebBrowserEventArgs>? Error;

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Events/NewWindow/*'/>
        public event EventHandler<WebBrowserEventArgs>? NewWindow;

        /// <include file="Interfaces/IWebBrowser.xml"
        ///     path='doc/Events/DocumentTitleChanged/*'/>
        public event EventHandler<WebBrowserEventArgs>? DocumentTitleChanged;

        public static IWebBrowserFactoryHandler Factory
            => factory ??= BaseApplication.Handler.CreateWebBrowserFactoryHandler();

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Is64Bit/*'/>
        public static bool Is64Bit
        {
            get
            {
                return Environment.Is64BitOperatingSystem;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/MemoryFS/*'/>
        public IWebBrowserMemoryFS MemoryFS
        {
            get
            {
                fMemoryFS ??= Factory.CreateMemoryFileSystem(this);
                return fMemoryFS;
            }
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.WebBrowser;

        /// <summary>
        /// Gets default url (the first loaded url).
        /// </summary>
        public string DefaultUrl => defaultUrl;

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/HasSelection/*'/>
        public virtual bool HasSelection
        {
            get
            {
                CheckDisposed();
                return Handler.HasSelection;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/ZoomFactor/*'/>
        public virtual float ZoomFactor
        {
            get
            {
                CheckDisposed();
                return Handler.ZoomFactor;
            }

            set
            {
                CheckDisposed();
                Handler.ZoomFactor = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control has a border.
        /// </summary>
        public bool HasBorder
        {
            get
            {
                CheckDisposed();
                return Handler.HasBorder;
            }

            set
            {
                CheckDisposed();
                Handler.HasBorder = value;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/ZoomType/*'/>
        public virtual WebBrowserZoomType ZoomType
        {
            get
            {
                CheckDisposed();
                return Handler.ZoomType;
            }

            set
            {
                CheckDisposed();
                Handler.ZoomType = value;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Source/*'/>
        public virtual Uri Source
        {
            get
            {
                return new Uri(GetCurrentURL());
            }

            set
            {
                Navigate(value);
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/CanZoomIn/*'/>
        public virtual bool CanZoomIn { get => Zoom != WebBrowserZoom.Largest; }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/CanGoBack/*'/>
        public virtual bool CanGoBack
        {
            get
            {
                CheckDisposed();
                return Handler.CanGoBack;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/CanUndo/*'/>
        public bool CanUndo
        {
            get
            {
                CheckDisposed();
                return Handler.CanUndo;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/CanRedo/*'/>
        public virtual bool CanRedo
        {
            get
            {
                CheckDisposed();
                return Handler.CanRedo;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/AccessToDevToolsEnabled/*'/>
        public virtual bool AccessToDevToolsEnabled
        {
            get
            {
                CheckDisposed();
                return Handler.AccessToDevToolsEnabled;
            }

            set
            {
                CheckDisposed();
                Handler.AccessToDevToolsEnabled = value;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/UserAgent/*'/>
        public virtual string UserAgent
        {
            get
            {
                CheckDisposed();
                return Handler.UserAgent;
            }

            set
            {
                CheckDisposed();
                Handler.UserAgent = value;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/ContextMenuEnabled/*'/>
        public virtual bool ContextMenuEnabled
        {
            get
            {
                CheckDisposed();
                return Handler.ContextMenuEnabled;
            }

            set
            {
                CheckDisposed();
                Handler.ContextMenuEnabled = value;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Backend/*'/>
        public virtual WebBrowserBackend Backend
        {
            get
            {
                CheckDisposed();
                return Handler.Backend;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Editable/*'/>
        public virtual bool Editable
        {
            get
            {
                CheckDisposed();
                return Handler.Editable;
            }

            set
            {
                CheckDisposed();
                Handler.Editable = value;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Zoom/*'/>
        public virtual WebBrowserZoom Zoom
        {
            get
            {
                CheckDisposed();
                return Handler.Zoom;
            }

            set
            {
                CheckDisposed();
                Handler.Zoom = value;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/IsBusy/*'/>
        public virtual bool IsBusy
        {
            get
            {
                CheckDisposed();
                return Handler.IsBusy;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/PageSource/*'/>
        public virtual string PageSource
        {
            get
            {
                CheckDisposed();
                return Handler.PageSource;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/PageText/*'/>
        public virtual string PageText
        {
            get
            {
                CheckDisposed();
                return Handler.PageText;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/SelectedText/*'/>
        public virtual string SelectedText
        {
            get
            {
                CheckDisposed();
                return Handler.SelectedText;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/SelectedSource/*'/>
        public virtual string SelectedSource
        {
            get
            {
                CheckDisposed();
                return Handler.SelectedSource;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/CanCut/*'/>
        public virtual bool CanCut
        {
            get
            {
                CheckDisposed();
                return Handler.CanCut;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Url/*'/>
        public string Url
        {
            get
            {
                return GetCurrentURL();
            }

            set
            {
                LoadURL(value);
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml"
        /// path='doc/PreferredColorScheme/*'/>
        public virtual WebBrowserPreferredColorScheme PreferredColorScheme
        {
            get
            {
                CheckDisposed();
                return Handler.PreferredColorScheme;
            }

            set
            {
                CheckDisposed();
                Handler.PreferredColorScheme = value;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/CanCopy/*'/>
        public virtual bool CanCopy
        {
            get
            {
                CheckDisposed();
                return Handler.CanCopy;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/CanPaste/*'/>
        public virtual bool CanPaste
        {
            get
            {
                CheckDisposed();
                return Handler.CanPaste;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/CanGoForward/*'/>
        public virtual bool CanGoForward
        {
            get
            {
                CheckDisposed();
                return Handler.CanGoForward;
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/CanZoomOut/*'/>
        public virtual bool CanZoomOut { get => Zoom != WebBrowserZoom.Tiny; }

        /*internal static Brush UixmlPreviewerBrush
        {
            get
            {
                if (uixmlPreviewerBrush == null)
                    uixmlPreviewerBrush = Brushes.White;
                return uixmlPreviewerBrush;
            }
        }*/

        internal new IWebBrowserHandler Handler => (IWebBrowserHandler)base.Handler;

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/GetBackendOS/*'/>
        public static WebBrowserBackendOS GetBackendOS()
        {
            return Factory.GetBackendOS();
        }

        /// <summary>
        /// Prepends filename with "file" url protocol prefix.
        /// </summary>
        /// <param name="filename">Path to the file.</param>
        /// <returns><see cref="string"/> containing filename with "file" url
        /// protocol prefix.</returns>
        /// <remarks>
        /// Under Windows file path starts with drive letter, so we need
        /// to add additional "/" separator between protocol and file path.
        /// This functions checks for OS and adds "file" url protocol correctly.
        /// Also spaces are replaced with %20, other non url chars are replaced
        /// with corresponding html codes.
        /// </remarks>
        public static string PrepareFileUrl(string filename) => CommonUtils.PrepareFileUrl(filename);

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/SetBackendPath/*'/>
        public static void SetBackendPath(string path, bool isRelative = false)
        {
            if (isRelative)
            {
                string location = Assembly.GetExecutingAssembly().Location;
                string s = Path.GetDirectoryName(location)!;
                string s2 = Path.Combine(s, path);
                path = s2;
            }

            if (!Directory.Exists(path))
                return;

            var files = Directory.EnumerateFileSystemEntries(path, "*.dll");

#pragma warning disable
            foreach (string file in files)
            {
                Factory.SetEdgePath(path);
                break;
            }
#pragma warning restore
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/DoCommandGlobal/*'/>
        public static string? DoCommandGlobal(string cmdName, params object?[] args)
        {
            if(cmdName == "CppThrow")
            {
                Factory.ThrowError(1);
                return string.Empty;
            }

            if (cmdName == "Screenshot")
            {
                string location = Assembly.GetExecutingAssembly().Location;
                string s = Path.GetDirectoryName(location)!;

                var screenshotFilename = s + @"\screenshot.bmp";
                if (args.Length == 0)
                    return string.Empty;
                Control? control = args[0] as Control;
                control?.SaveScreenshot(screenshotFilename);
                return screenshotFilename;
            }

            if (cmdName == "UIVersion")
            {
                Assembly thisAssembly = typeof(BaseApplication).Assembly;
                AssemblyName thisAssemblyName = thisAssembly.GetName();
                Version? ver = thisAssemblyName?.Version;
                return ver?.ToString();
            }

            return Factory.DoCommand(cmdName, args);
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/CrtSetDbgFlag/*'/>
        public static void CrtSetDbgFlag(int value)
        {
            Factory.CrtSetDbgFlag(value);
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/IsBackendAvailable/*'/>
        public static bool IsBackendAvailable(WebBrowserBackend value)
        {
            return Factory.IsBackendAvailable(value);
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/SetBackend/*'/>
        public static void SetBackend(WebBrowserBackend value)
        {
            if (value == WebBrowserBackend.IE || value == WebBrowserBackend.IELatest
                || value == WebBrowserBackend.Edge)
            {
                if (WebBrowser.GetBackendOS() != WebBrowserBackendOS.Windows)
                    value = WebBrowserBackend.Default;
            }

            Factory.SetBackend(value);
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/GetLibraryVersionString/*'/>
        public static string GetLibraryVersionString()
        {
            return Factory.GetLibraryVersionString();
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/GetBackendVersionString/*'/>
        public static string GetBackendVersionString(WebBrowserBackend value)
        {
            return Factory.GetBackendVersionString(value);
        }

        /// <include file="Interfaces/IWebBrowser.xml"
        ///     path='doc/SetDefaultUserAgent/*'/>
        public static void SetDefaultUserAgent(string value)
        {
            Factory.SetDefaultUserAgent(value);
        }

        /// <include file="Interfaces/IWebBrowser.xml"
        ///     path='doc/SetDefaultScriptMesageName/*'/>
        public static void SetDefaultScriptMesageName(string value)
        {
            Factory.SetDefaultScriptMesageName(value);
        }

        /// <include file="Interfaces/IWebBrowser.xml"
        ///     path='doc/SetDefaultFSNameMemory/*'/>
        public static void SetDefaultFSNameMemory(string value)
        {
            Factory.SetDefaultFSNameMemory(value);
        }

        /// <include file="Interfaces/IWebBrowser.xml"
        ///     path='doc/SetDefaultFSNameArchive/*'/>
        public static void SetDefaultFSNameArchive(string value)
        {
            Factory.SetDefaultFSNameArchive(value);
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/SetLatestBackend/*'/>
        public static void SetLatestBackend()
        {
            if (IsBackendAvailable(WebBrowserBackend.Edge))
            {
                SetBackend(WebBrowserBackend.Edge);
                return;
            }

            if (IsBackendAvailable(WebBrowserBackend.IELatest))
            {
                SetBackend(WebBrowserBackend.IELatest);
                return;
            }

            if (IsBackendAvailable(WebBrowserBackend.WebKit))
            {
                SetBackend(WebBrowserBackend.WebKit);
                return;
            }

            SetBackend(WebBrowserBackend.Default);
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/LoadUrlOrSearch/*'/>
        public void LoadUrlOrSearch(string? url)
        {
            if (url == null)
            {
                LoadURL();
                return;
            }

            Uri uri;

            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
                uri = new Uri(url);
            else
#pragma warning disable IDE0079
#pragma warning disable CA1847 // Use char literal for a single character lookup
            if (!url.Contains(" ") && url.Contains("."))
            {
                // An invalid URI contains a dot and no spaces,
                // try tacking http:// on the front.
                uri = new Uri("http://" + url);
            }
            else
            {
                // Otherwise treat it as a web search.
#pragma warning disable CA1861 // Avoid constant arrays as arguments
                uri = new Uri("https://google.com/search?q=" +
                    string.Join("+", Uri.EscapeDataString(url).Split(
                        new string[] { "%20" },
                        StringSplitOptions.RemoveEmptyEntries)));
#pragma warning restore CA1861 // Avoid constant arrays as arguments
            }
#pragma warning restore CA1847 // Use char literal for a single character lookup
#pragma warning restore IDE0079

            LoadURL(uri.ToString());
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/DoCommand/*'/>
        public string? DoCommand(string cmdName, params object?[] args)
        {
            CheckDisposed();
            return Handler.DoCommand(cmdName, args);
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/ZoomIn/*'/>
        public virtual void ZoomIn()
        {
            if (CanZoomIn)
                ZoomInOut(1);
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/ZoomOut/*'/>
        public virtual void ZoomOut()
        {
            if (CanZoomOut)
                ZoomInOut(-1);
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Navigate_string/*'/>
        public void Navigate(string urlString)
        {
            LoadURL(urlString);
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Navigate_uri/*'/>
        public void Navigate(Uri source)
        {
            if (source == null)
                LoadURL();
            else
                LoadURL(source.ToString());
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/GoBack/*'/>
        public virtual bool GoBack()
        {
            if (!CanGoBack)
                return false;
            CheckDisposed();
            return Handler.GoBack();
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/GoForward/*'/>
        public virtual bool GoForward()
        {
            if (!CanGoForward)
                return false;
            CheckDisposed();
            return Handler.GoForward();
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Stop/*'/>
        public virtual void Stop()
        {
            CheckDisposed();
            Handler.Stop();
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/ClearHistory/*'/>
        public virtual void ClearHistory()
        {
            CheckDisposed();
            Handler.ClearHistory();
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/EnableHistory/*'/>
        public virtual void EnableHistory(bool enable = true)
        {
            CheckDisposed();
            Handler.EnableHistory(enable);
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Reload/*'/>
        public virtual void Reload()
        {
            CheckDisposed();
            Handler.Reload();
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Reload_noCache/*'/>
        public virtual void Reload(bool noCache)
        {
            CheckDisposed();
            Handler.Reload(noCache);
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/NavigateToString/*'/>
        public virtual void NavigateToString(string html, string? baseUrl = null)
        {
            CheckDisposed();
            if (string.IsNullOrEmpty(html))
            {
                LoadURL();
                return;
            }

            Handler.NavigateToString(html, baseUrl ?? string.Empty);
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/NavigateToStream/*'/>
        public virtual void NavigateToStream(Stream stream)
        {
            if (stream == null)
            {
                this.LoadURL("about:blank");
                return;
            }

            var s = StringFromStream(stream);
            NavigateToString(s);

            static string StringFromStream(Stream stream)
            {
                return new StreamReader(stream, Encoding.UTF8).ReadToEnd();
            }
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/CanSetZoomType/*'/>
        public virtual bool CanSetZoomType(WebBrowserZoomType zoomType)
        {
            CheckDisposed();
            return Handler.CanSetZoomType(zoomType);
        }

        /// <summary>
        /// Raises the <see cref="ScriptMessageReceived"/> event and
        /// <see cref="OnScriptMessageReceived"/> method.
        /// </summary>
        /// <param name="e">
        /// An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        public void RaiseScriptMessageReceived(WebBrowserEventArgs e)
        {
            ScriptMessageReceived?.Invoke(this, e);
            OnScriptMessageReceived(e);
        }

        /// <summary>
        /// Raises the <see cref="FullScreenChanged"/> event and
        /// <see cref="OnFullScreenChanged"/> method.
        /// </summary>
        /// <param name="e">
        /// An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        public void RaiseFullScreenChanged(WebBrowserEventArgs e)
        {
            FullScreenChanged?.Invoke(this, e);
            OnFullScreenChanged(e);
        }

        /// <summary>
        /// Raises the <see cref="ScriptResult"/> event and
        /// <see cref="OnScriptResult"/> method.
        /// </summary>
        /// <param name="e">
        /// An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        public void RaiseScriptResult(WebBrowserEventArgs e)
        {
            ScriptResult?.Invoke(this, e);
            OnScriptResult(e);
        }

        /// <summary>
        /// Raises the <see cref="Navigated"/> event and
        /// <see cref="OnNavigated"/> method.
        /// </summary>
        /// <param name="e">
        /// An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        public void RaiseNavigated(WebBrowserEventArgs e)
        {
            Navigated?.Invoke(this, e);
            OnNavigated(e);
        }

        /// <summary>
        /// Raises the <see cref="DocumentTitleChanged"/> event and
        /// <see cref="OnDocumentTitleChanged"/> method.
        /// </summary>
        /// <param name="e">
        /// An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        public void RaiseDocumentTitleChanged(WebBrowserEventArgs e)
        {
            DocumentTitleChanged?.Invoke(this, e);
            OnDocumentTitleChanged(e);
        }

        /// <summary>
        /// Raises the <see cref="BeforeBrowserCreate"/> event and
        /// <see cref="OnBeforeBrowserCreate"/> method.
        /// </summary>
        /// <param name="e">
        /// An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        public void RaiseBeforeBrowserCreate(WebBrowserEventArgs e)
        {
            BeforeBrowserCreate?.Invoke(this, e);
            OnBeforeBrowserCreate(e);
        }

        /// <summary>
        /// Raises the <see cref="Loaded"/> event and
        /// <see cref="OnLoaded"/> method.
        /// </summary>
        /// <param name="e">
        /// An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        public void RaiseLoaded(WebBrowserEventArgs e)
        {
            Loaded?.Invoke(this, e);
            OnLoaded(e);
        }

        /// <summary>
        /// Raises the <see cref="Error"/> event and
        /// <see cref="OnError"/> method.
        /// </summary>
        /// <param name="e">
        /// An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        public void RaiseError(WebBrowserEventArgs e)
        {
            Error?.Invoke(this, e);
            OnError(e);
        }

        /// <summary>
        /// Raises the <see cref="Navigating"/> event and
        /// <see cref="OnNavigating"/> method.
        /// </summary>
        /// <param name="e">
        /// An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        public void RaiseNavigating(WebBrowserEventArgs e)
        {
            Navigating?.Invoke(this, e);
            OnNavigating(e);
        }

        /// <summary>
        /// Raises the <see cref="NewWindow"/> event and
        /// <see cref="OnNewWindow"/> method.
        /// </summary>
        /// <param name="e">
        /// An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        public void RaiseNewWindow(WebBrowserEventArgs e)
        {
            NewWindow?.Invoke(this, e);
            OnNewWindow(e);
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/SelectAll/*'/>
        public virtual void SelectAll()
        {
            CheckDisposed();
            Handler.SelectAll();
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/DeleteSelection/*'/>
        public virtual void DeleteSelection()
        {
            CheckDisposed();
            Handler.DeleteSelection();
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Undo/*'/>
        public virtual void Undo()
        {
            if (!CanUndo)
                return;
            CheckDisposed();
            Handler.Undo();
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Redo/*'/>
        public virtual void Redo()
        {
            if (!CanRedo)
                return;
            CheckDisposed();
            Handler.Redo();
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/ClearSelection/*'/>
        public virtual void ClearSelection()
        {
            CheckDisposed();
            Handler.ClearSelection();
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Cut/*'/>
        public virtual void Cut()
        {
            if (!CanCut)
                return;
            CheckDisposed();
            Handler.Cut();
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Copy/*'/>
        public virtual void Copy()
        {
            if (!CanCopy)
                return;
            CheckDisposed();
            Handler.Copy();
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Paste/*'/>
        public virtual void Paste()
        {
            if (!CanPaste)
                return;
            CheckDisposed();
            Handler.Paste();
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Find/*'/>
        public virtual int Find(string text, WebBrowserFindParams? prm = null)
        {
            CheckDisposed();
            return Handler.Find(text, prm);
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/FindClearResult/*'/>
        public virtual void FindClearResult()
        {
            CheckDisposed();
            Handler.FindClearResult();
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/Print/*'/>
        public virtual void Print()
        {
            CheckDisposed();
            Handler.Print();
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/RemoveAllUserScripts/*'/>
        public virtual void RemoveAllUserScripts()
        {
            CheckDisposed();
            Handler.RemoveAllUserScripts();
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/AddScriptMessageHandler/*'/>
        public virtual bool AddScriptMessageHandler(string name)
        {
            CheckDisposed();
            return Handler.AddScriptMessageHandler(name);
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/RemoveScriptMessageHandler/*'/>
        public virtual bool RemoveScriptMessageHandler(string name)
        {
            CheckDisposed();
            return Handler.RemoveScriptMessageHandler(name);
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/AddUserScript/*'/>
        public virtual bool AddUserScript(
            string javascript,
            bool injectDocStart = true)
        {
            CheckDisposed();
            return Handler.AddUserScript(javascript, injectDocStart);
        }

        /// <summary>
        ///     Converts the value into a JSON string. It is a simple convertor.
        ///     It is better to use System.Text.Json.JsonSerializer.Serialize method.
        /// </summary>
        /// <param name="v">
        ///     The value to convert.
        /// </param>
        /// <returns>
        ///     The JSON string representation of the value.
        /// </returns>
        /// <remarks>
        ///     Do not call it for primitive values. It supposed to convert only
        ///     TypeCode.Object values. Outputs result like this:
        ///     {firstName:"John", lastName:"Doe", age:50, eyeColor:"blue"}
        /// </remarks>
        public virtual string ObjectToJSON(object? v)
        {
            if (v == null)
                return "null";

            string result = "{";

            var t = v.GetType();
            PropertyInfo[] props = t.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var prop in props)
            {
                if (result.Length > 1)
                    result += ", ";
                var propValue = ToInvokeScriptArg(prop.GetValue(v));
                result += prop.Name + ": " + propValue;
            }

            result += "}";
            return result;
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/ToInvokeScriptArgs/*'/>
        public string ToInvokeScriptArgs(object?[] args)
        {
            if (args == null || args.Length == 0)
                return string.Empty;

            string? s = ToInvokeScriptArg(args[0]);

            for (int i = 1; i < args.Length; i++)
            {
                var arg = ToInvokeScriptArg(args[i]);
                s += ',' + arg;
            }

            return s!;
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/InvokeScriptAsync/*'/>
        public virtual void InvokeScriptAsync(
            string scriptName,
            IntPtr clientData,
            params object?[] args)
        {
            if (scriptName == null)
                return;
            if (args == null || args.Length == 0)
            {
                RunScriptAsync(scriptName + "()");
                return;
            }

            string? s = ToInvokeScriptArgs(args!);

            RunScriptAsync(scriptName + "(" + s + ")", clientData);
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/ToInvokeScriptArg/*'/>
        public virtual string? ToInvokeScriptArg(object? arg)
        {
            if (arg == null)
                return "null";

            TypeCode tcode = Type.GetTypeCode(arg.GetType());

            switch (tcode)
            {
                case TypeCode.Empty:
                case TypeCode.DBNull:
                    return "null";
                case TypeCode.Object:
                    return ObjectToJSON(arg);
                case TypeCode.Boolean:
                    return ((bool)arg).ToString().ToLower();
                case TypeCode.Char:
                case TypeCode.String:
                    return "'" + arg.ToString() + "'";
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    return arg.ToString();
                case TypeCode.Single:
                    return ((float)arg).ToString(
                        "G",
                        CultureInfo.InvariantCulture);
                case TypeCode.Double:
                    return ((double)arg).ToString(
                        "G",
                        CultureInfo.InvariantCulture);
                case TypeCode.Decimal:
                    return ((decimal)arg).ToString(CultureInfo.InvariantCulture);
                case TypeCode.DateTime:
                    // https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings
                    // https://javascript.info/date
                    return "'" + ((System.DateTime)arg).ToString(
                        DateUtils.DateFormatJs,
                        CultureInfo.InvariantCulture) + "'";
                default:
                    break;
            }

            return arg.ToString();
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/LoadURL/*'/>
        public virtual void LoadURL(string? url = null)
        {
            url ??= "about:blank";
            CheckDisposed();
            Handler.LoadURL(url);
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/GetCurrentTitle/*'/>
        public virtual string GetCurrentTitle()
        {
            CheckDisposed();
            return Handler.GetCurrentTitle();
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/GetCurrentURL/*'/>
        public virtual string GetCurrentURL()
        {
            CheckDisposed();
            return Handler.GetCurrentURL();
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/SetVirtualHostNameToFolderMapping/*'/>
        public virtual void SetVirtualHostNameToFolderMapping(
            string hostName,
            string folderPath,
            WebBrowserHostResourceAccessKind accessKind)
        {
            CheckDisposed();
            Handler.SetVirtualHostNameToFolderMapping(hostName, folderPath, accessKind);
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/GetNativeBackend/*'/>
        public IntPtr GetNativeBackend()
        {
            CheckDisposed();
            return Handler.GetNativeBackend();
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/RunScriptAsync/*'/>
        public virtual void RunScriptAsync(
            string javascript,
            IntPtr? clientData = null)
        {
            CheckDisposed();
            Handler.RunScriptAsync(javascript, clientData);
        }

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/SetDefaultPage/*'/>
        internal static void SetDefaultPage(string url)
        {
            Factory.SetDefaultPage(url);
        }

        /*internal virtual bool RunScript(string javascript)
        {
            return RunScript(javascript, out _);
        }

        internal virtual bool RunScript(string javascript, out string result)
        {
            CheckDisposed();
            return Handler.RunScript(javascript, out result);
        }*/

        /*internal virtual string? InvokeScript(string scriptName)
        {
            if (scriptName == null)
                return null;
            bool ok = RunScript(scriptName + "()", out string result);
            if (!ok)
                return null;
            return result;
        }

        internal virtual string? InvokeScript(string scriptName, params object?[] args)
        {
            if (scriptName == null)
                return null;
            if (args == null || args.Length == 0)
                return InvokeScript(scriptName);

            string? s = ToInvokeScriptArgs(args);

            bool ok = RunScript(scriptName + "(" + s + ")", out string result);
            if (!ok)
                return null;
            return result;
        }*/

        /// <include file="Interfaces/IWebBrowser.xml" path='doc/CreateHandler/*'/>
        protected override IControlHandler CreateHandler()
        {
            return BaseApplication.Handler.CreateWebBrowserHandler(this);
        }

        /// <summary>
        /// Called when <see cref="DocumentTitleChanged"/> event is raised.
        /// </summary>
        /// <param name="e">
        /// An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnDocumentTitleChanged(WebBrowserEventArgs e)
        {
        }

        /// <summary>
        /// Called when <see cref="ScriptMessageReceived"/> event is raised.
        /// </summary>
        /// <param name="e">
        /// An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnScriptMessageReceived(WebBrowserEventArgs e)
        {
        }

        /// <summary>
        /// Called when <see cref="Navigated"/> event is raised.
        /// </summary>
        /// <param name="e">
        /// An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnNavigated(WebBrowserEventArgs e)
        {
        }

        /// <summary>
        /// Called when <see cref="Navigating"/> event is raised.
        /// </summary>
        /// <param name="e">
        /// An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnNavigating(WebBrowserEventArgs e)
        {
        }

        /// <summary>
        /// Called when <see cref="Loaded"/> event is raised.
        /// </summary>
        /// <param name="e">
        /// An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnLoaded(WebBrowserEventArgs e)
        {
        }

        /// <summary>
        /// Called when <see cref="NewWindow"/> event is raised.
        /// </summary>
        /// <param name="e">
        /// An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnNewWindow(WebBrowserEventArgs e)
        {
        }

        /// <summary>
        /// Called when <see cref="BeforeBrowserCreate"/> event is raised.
        /// </summary>
        /// <param name="e">
        /// An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnBeforeBrowserCreate(WebBrowserEventArgs e)
        {
        }

        /// <summary>
        /// Called when <see cref="FullScreenChanged"/> event is raised.
        /// </summary>
        /// <param name="e">
        /// An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnFullScreenChanged(WebBrowserEventArgs e)
        {
        }

        /// <summary>
        /// Called when <see cref="ScriptResult"/> event is raised.
        /// </summary>
        /// <param name="e">
        /// An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnScriptResult(WebBrowserEventArgs e)
        {
        }

        /// <summary>
        /// Called when <see cref="Error"/> event is raised.
        /// </summary>
        /// <param name="e">
        /// An <see cref="WebBrowserEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnError(WebBrowserEventArgs e)
        {
        }

        private void ZoomInOut(int delta)
        {
            var currentZoom = Zoom;
            int zoom = (int)currentZoom;
            zoom += delta;
            currentZoom = (WebBrowserZoom)Enum.ToObject(typeof(WebBrowserZoom), zoom);
            Zoom = currentZoom;
        }
    }
}