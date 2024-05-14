using System;
using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// The central class of the advanced docking and floating toolbars and
    /// panes framework.
    /// </summary>
    /// <remarks>
    /// Manages the panes associated with it, using a pane's
    /// <see cref="IAuiPaneInfo"/> information to determine each pane's docking
    /// and floating behaviour.
    /// </remarks>
    /// <remarks>
    /// It uses a replaceable dock art class to do all drawing, so all drawing
    /// is localized in one area, and may be customized depending on an
    /// application's specific needs.
    /// </remarks>
    /// <remarks>
    /// Works as follows: the programmer adds panes to the class, or makes changes
    /// to existing pane properties (dock position, floating state, show state, etc.).
    /// To apply these changes, <see cref="Update"/> function is called.
    /// This batch processing
    /// can be used to avoid flicker, by modifying more than one pane at a time,
    /// and then "committing" all of the changes at once by calling <see cref="Update"/>.
    /// </remarks>
    internal class AuiManager : BaseComponent, IDisposable
    {
        private Control? managedControl;
        private IntPtr handle;

        /// <summary>
        /// Creates <see cref="AuiManager"/> instance.
        /// </summary>
        public AuiManager()
        {
            handle = Native.AuiManager.CreateAuiManager();
        }

        /// <summary>
        /// Called when <see cref="AuiManager"/> instance is destroyed.
        /// </summary>
        ~AuiManager()
        {
            Dispose();
        }

        /// <summary>
        /// Gets container in which toolbars and sidebars will be created.
        /// </summary>
        /// <remarks>
        /// Use <see cref="SetManagedWindow"/> to assign this property.
        /// </remarks>
        public Control? ManagedControl
        {
            get => managedControl;
        }

        /// <summary>
        /// Gets or sets the associated art provider.
        /// </summary>
        internal IAuiDockArt ArtProvider
        {
            get
            {
                var result = Native.AuiManager.GetArtProvider(handle);
                return new AuiDockArt(result, false);
            }

            set
            {
                Native.AuiManager.SetArtProvider(handle, ((AuiDockArt)value).Handle);
            }
        }

        /// <summary>
        /// Returns true if live resize is always used on the current platform.
        /// </summary>
        /// <remarks>
        /// If this function returns true, LiveResize flag is ignored and live
        /// resize is always used, whether it's specified or not. Currently this
        /// is the case for MacOs and Linux ports, as live resizing is the
        /// only implemented method there.
        /// </remarks>
        public static bool AlwaysUsesLiveResize()
        {
            return Native.AuiManager.AlwaysUsesLiveResize();
        }

        /// <inheritdoc cref="DisposableObject.Dispose()"/>
        public void Dispose()
        {
            UnInit();
            if (handle != IntPtr.Zero)
            {
                Native.AuiManager.Delete(handle);
                handle = IntPtr.Zero;
            }

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dissociate the managed window from the manager.
        /// </summary>
        /// <remarks>
        /// This function may be called before the managed frame or window is
        /// destroyed, but it's unnecessary to call it explicitly, as it will
        /// be called automatically when this window is destroyed,
        /// as well as when the manager itself is.
        /// </remarks>
        public void UnInit()
        {
            if (managedControl == null)
                return;
            managedControl.Disposed -= ManagedWindow_Disposed;
            managedControl = null;

            Native.AuiManager.UnInit(handle);
        }

        /// <summary>
        /// This method is used to specify option flags.
        /// </summary>
        /// <param name="flags">Specifies options which allow the frame
        /// management behaviour to be modified.</param>
        public void SetFlags(AuiManagerOption flags)
        {
            Native.AuiManager.SetFlags(handle, (uint)flags);
        }

        /// <summary>
        /// Returns the current <see cref="AuiManagerOption"/> flags.
        /// </summary>
        public AuiManagerOption GetFlags()
        {
            return (AuiManagerOption)Native.AuiManager.GetFlags(handle);
        }

        /// <summary>
        /// Returns true if windows are resized live.
        /// </summary>
        /// <remarks>
        /// This function combines the check for <see cref="AlwaysUsesLiveResize"/>
        /// and, for the platforms where live resizing is optional,
        /// the check LiveResize flag.
        /// </remarks>
        /// <remarks>
        /// Using this property allows to verify whether live
        /// resizing is being actually used.
        /// </remarks>
        /// <returns><c>true</c> if live resizing is being actually used,
        /// <c>false</c> otherwise.</returns>
        public bool HasLiveResize()
        {
            return Native.AuiManager.HasLiveResize(handle);
        }

        /// <summary>
        /// Called after any number of changes are made to any of the managed panes.
        /// </summary>
        /// <remarks>
        /// Must be invoked after <see cref="AddPane(Control, IAuiPaneInfo)"/>
        /// or <see cref="InsertPane"/>
        /// are called in order to "realize" or "commit" the changes.
        /// </remarks>
        /// <remarks>
        /// In addition, any number of changes may be made to
        /// <see cref="IAuiPaneInfo"/> structures (retrieved with
        /// <see cref="GetPane(Control)"/>, but to realize the changes,
        /// <see cref="Update"/> must be called. This construction allows pane
        /// flicker to be avoided by updating the whole layout at one time.
        /// </remarks>
        public void Update()
        {
            Native.AuiManager.Update(handle);
        }

        /// <summary>
        /// Saves the entire user interface layout into an encoded string,
        /// which can then be stored by the application.
        /// </summary>
        /// <returns><see cref="string"/> with the entire user interface
        /// layout.</returns>
        public string SavePerspective()
        {
            return Native.AuiManager.SavePerspective(handle);
        }

        /// <summary>
        /// Loads a saved perspective. A perspective is the layout state
        /// of an <see cref="AuiManager"/> managed window.
        /// </summary>
        /// <param name="perspective">Serialized layout information of a
        /// perspective (excl. references to UI elements).</param>
        /// <param name="update">If update is true, <see cref="Update"/> is
        /// automatically invoked, thus realizing the specified perspective
        /// on screen.</param>
        /// <remarks>
        /// All currently existing panes that have an object in "perspective"
        /// with the same name ("equivalent") will receive the layout
        /// parameters of the object in "perspective". Existing panes that
        /// do not have an equivalent in "perspective" remain unchanged,
        /// objects in "perspective" having no equivalent in the manager are ignored.
        /// </remarks>
        /// <returns><c>true</c> if perspective was loaded successfully,
        /// <c>false</c> otherwise.</returns>
        public bool LoadPerspective(string perspective, bool update = true)
        {
            return Native.AuiManager.LoadPerspective(handle, perspective, update);
        }

        /// <summary>
        /// Adjusts limits of any new dock size.
        /// </summary>
        /// <remarks>
        /// When a user creates a new dock by dragging a window into a
        /// docked position, often times the large size of the window
        /// will create a dock that is unwieldy large.
        /// </remarks>
        /// <remarks>
        /// <see cref="AuiManager"/> by default limits the size of any new dock
        /// to 1/3 of the window size. For horizontal docks, this would be
        /// 1/3 of the window height. For vertical docks, 1/3 of the width.
        /// </remarks>
        /// <remarks>
        /// Calling this function will adjust this constraint value. The numbers
        /// must be between 0.0 and 1.0. For instance, calling this function
        /// with 0.5, 0.5 will cause new docks to be limited to half of
        /// the size of the entire managed window.
        /// </remarks>
        /// <param name="widthPct">New width limit.</param>
        /// <param name="heightPct">New height limit.</param>
        public void SetDockSizeConstraint(double widthPct, double heightPct)
        {
            Native.AuiManager.SetDockSizeConstraint(handle, widthPct, heightPct);
        }

        /// <summary>
        /// Restore the previously maximized pane.
        /// </summary>
        public void RestoreMaximizedPane()
        {
            Native.AuiManager.RestoreMaximizedPane(handle);
        }

        /// <summary>
        /// Used to lookup a pane information object either by window or by pane
        /// name, which acts as a unique id for a window pane.
        /// </summary>
        /// <remarks>
        /// The returned <see cref="IAuiPaneInfo"/> object may then be modified to
        /// change a pane's look, state or position. After one or more modifications
        /// to <see cref="IAuiPaneInfo"/>, <see cref="Update"/> should be called
        /// to commit the changes to the user interface. If the lookup failed
        /// (meaning the pane could not be found in the manager), a call to
        /// the returned <see cref="IAuiPaneInfo.IsOk"/> method will return false.
        /// </remarks>
        /// <param name="control">Control for which pane was previously created.</param>
        /// <returns></returns>
        public IAuiPaneInfo GetPane(Control control)
        {
            return ToPaneInfo(Native.AuiManager.GetPane(handle, ToHandle(control)));
        }

        /// <summary>
        /// Used to lookup a pane information object by pane
        /// name, which acts as a unique id for a window pane.
        /// </summary>
        /// <remarks>
        /// The returned <see cref="IAuiPaneInfo"/> object may then be modified to
        /// change a pane's look, state or position. After one or more modifications
        /// to <see cref="IAuiPaneInfo"/>, <see cref="Update"/> should be called
        /// to commit the changes to the user interface. If the lookup failed
        /// (meaning the pane could not be found in the manager), a call to
        /// the returned <see cref="IAuiPaneInfo.IsOk"/> method will return false.
        /// </remarks>
        /// <param name="name">Unique pane name.</param>
        public IAuiPaneInfo GetPane(string name)
        {
            return ToPaneInfo(Native.AuiManager.GetPaneByName(handle, name));
        }

        /// <summary>
        /// Sets splitter sash size using <see cref="PlatformDefaults.MinSplitterSashSize"/>
        /// and sash color to <see cref="SystemColors.ButtonFace"/>.
        /// </summary>
        public void SetDefaultSplitterSashProps()
        {
            ArtProvider.SetColor(AuiDockArtSetting.SashColor, SystemColors.ButtonFace);

            var sashSize = ArtProvider.GetMetric(AuiDockArtSetting.SashSize);
            var newSashSize = PixelFromDip(AllPlatformDefaults.PlatformCurrent.MinSplitterSashSize);
            if (newSashSize > sashSize)
                ArtProvider.SetMetric(AuiDockArtSetting.SashSize, newSashSize);
        }

        /// <summary>
        /// Called to specify the frame or window which is to be managed by
        /// <see cref="AuiManager"/>. Frame management is not restricted to
        /// just frames. Child windows or custom controls are also allowed.
        /// </summary>
        /// <param name="managedWnd">Managed window or control.</param>
        public void SetManagedWindow(LayoutPanel managedWnd)
        {
            if (managedWnd is null)
                throw new ArgumentNullException(nameof(managedWnd));

            if (managedControl != null)
                throw new Exception(ErrorMessages.Default.ParameterIsAlreadySet);

            managedControl = managedWnd;
            managedControl.Disposed += ManagedWindow_Disposed;

            Native.AuiManager.SetManagedWindow(handle, ToHandle(managedWnd));
        }

        /// <summary>
        /// Tells the manager to stop managing the pane specified by control.
        /// </summary>
        /// <remarks>
        /// The window, if in a floated frame, is reparented to the managed
        /// <see cref="Window"/>.
        /// </remarks>
        /// <param name="control">Control which pane will be detached.</param>
        /// <returns></returns>
        public bool DetachPane(Control control)
        {
            return Native.AuiManager.DetachPane(handle, ToHandle(control));
        }

        /// <summary>
        /// Tells the frame manager to start managing a child control.
        /// </summary>
        /// <param name="control">Child control to manage.</param>
        /// <param name="paneInfo">Pane information with settings.</param>
        /// <returns></returns>
        public bool AddPane(Control control, IAuiPaneInfo paneInfo)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));

            CheckManagedControl();

            if (control.Parent == null)
            {
                ManagedControl!.Children.Add(control);
            }

            return Native.AuiManager.AddPane(
                handle,
                ToHandle(control),
                ToHandle(paneInfo));
        }

        /// <summary>
        /// Tells the frame manager to start managing a child control.
        /// </summary>
        /// <param name="control">Child control to manage.</param>
        /// <param name="paneInfo">Pane information with settings.</param>
        /// <param name="dropPos">Allows a drop position to be specified,
        /// which will determine where the pane will be added.</param>
        /// <returns><c>true</c> if new pane was added successfully,
        /// <c>false</c> otherwise.</returns>
        public bool AddPane(
            Control control,
            IAuiPaneInfo paneInfo,
            PointD dropPos)
        {
            return Native.AuiManager.AddPane2(
                handle,
                ToHandle(control),
                ToHandle(paneInfo),
                dropPos.X,
                dropPos.Y);
        }

        /// <summary>
        /// Tells the frame manager to start managing a child control.
        /// </summary>
        /// <param name="control">Child control to manage.</param>
        /// <param name="direction">Position where new pane will be added.</param>
        /// <param name="caption">Pane caption.</param>
        /// <returns></returns>
        public bool AddPane(Control control, GenericDirection direction, string caption)
        {
            return Native.AuiManager.AddPane3(
                handle,
                ToHandle(control),
                (int)direction,
                caption);
        }

        /// <summary>
        /// Used to insert either a previously unmanaged pane window into
        /// the frame manager, or to insert a currently managed pane somewhere else.
        /// </summary>
        /// <param name="control">Pane control.</param>
        /// <param name="insertLoc">Position where to insert.</param>
        /// <param name="insertLevel">Сlarification of the insert position.</param>
        /// <remarks>
        /// This function will push all panes, rows, or docks aside and insert
        /// the window into the position specified by <paramref name="insertLoc"/>.
        /// </remarks>
        /// <remarks>
        /// Because <paramref name="insertLoc"/> can specify either a pane, dock row,
        /// or dock layer, the <paramref name="insertLevel"/> parameter is used
        /// to disambiguate this.
        /// </remarks>
        /// <returns><c>true</c> if pane was inserted successfully,
        /// <c>false</c> otherwise.</returns>
        public bool InsertPane(
            Control control,
            IAuiPaneInfo insertLoc,
            AuiPaneInsertLevel insertLevel)
        {
            return Native.AuiManager.InsertPane(
                handle,
                ToHandle(control),
                ToHandle(insertLoc),
                (int)insertLevel);
        }

        /// <summary>
        /// Saves information about a single pane, similar to
        /// <see cref="SavePerspective"/>.
        /// </summary>
        /// <param name="paneInfo">Pane whose layout parameters should
        /// be serialized.</param>
        /// <returns>The serialized layout parameters of the pane are returned
        /// within the <see cref="string"/>. Information about the references
        /// to UI elements stored in the pane are not serialized.</returns>
        public string SavePaneInfo(IAuiPaneInfo paneInfo)
        {
            return Native.AuiManager.SavePaneInfo(handle, ToHandle(paneInfo));
        }

        /// <summary>
        /// Similar to <see cref="LoadPerspective"/>, with the exception that
        /// it only loads information about a single pane.
        /// </summary>
        /// <remarks>
        /// This method writes the serialized data into the passed pane.
        /// References to UI elements are not modified.
        /// </remarks>
        /// <remarks>
        /// This operation also changes the name in the pane information.
        /// </remarks>
        /// <param name="panePart">Serialized pane data.</param>
        /// <param name="paneInfo">Pane information.</param>
        public void LoadPaneInfo(string panePart, IAuiPaneInfo paneInfo)
        {
            Native.AuiManager.LoadPaneInfo(handle, panePart, ToHandle(paneInfo));
        }

        /// <summary>
        /// Returns the current dock constraint values.
        /// </summary>
        public SizeD GetDockSizeConstraint()
        {
            return Native.AuiManager.GetDockSizeConstraint(handle);
        }

        /// <summary>
        /// Calls <see cref="Control.PixelFromDip(double)"/> of the <see cref="ManagedControl"/>.
        /// </summary>
        /// <param name="value">Value in device-independent units (1/96 inch).</param>
        /// <returns>Value converted to pixels.</returns>
        /// <remarks>
        /// Before calling this method, <see cref="ManagedControl"/> needs to be assigned.
        /// </remarks>
        public virtual int PixelFromDip(double value)
        {
            CheckManagedControl();
            return this.ManagedControl!.PixelFromDip(value);
        }

        /// <summary>
        /// Destroys or hides the given pane depending on its flags.
        /// </summary>
        /// <param name="paneInfo">Pane information.</param>
        public void ClosePane(IAuiPaneInfo paneInfo)
        {
            Native.AuiManager.ClosePane(handle, ToHandle(paneInfo));
        }

        /// <summary>
        /// Maximize the given pane.
        /// </summary>
        /// <param name="paneInfo">Pane information.</param>
        public void MaximizePane(IAuiPaneInfo paneInfo)
        {
            Native.AuiManager.MaximizePane(handle, ToHandle(paneInfo));
        }

        /// <summary>
        /// Restore the last state of the given pane.
        /// </summary>
        /// <param name="paneInfo">Pane information.</param>
        public void RestorePane(IAuiPaneInfo paneInfo)
        {
            Native.AuiManager.RestorePane(handle, ToHandle(paneInfo));
        }

        /// <summary>
        /// Check if a key modifier is pressed (actually Control or Alt) while
        /// dragging the frame to not dock the window.
        /// </summary>
        /// <param name="paneInfo">Panel information.</param>
        /// <returns><c>true</c> if panel can be docked, <c>false</c>
        /// otherwise.</returns>
        public bool CanDockPanel(IAuiPaneInfo paneInfo)
        {
            return Native.AuiManager.CanDockPanel(handle, ToHandle(paneInfo));
        }

        /// <summary>
        /// Creates new pane for use with <see cref="AuiManager"/>.
        /// </summary>
        /// <returns><see cref="IAuiPaneInfo"/> instance with empty settings.</returns>
#pragma warning disable
        public IAuiPaneInfo CreatePaneInfo()
#pragma warning restore
        {
            return new AuiPaneInfo(this);
        }

        internal static IntPtr ToHandle(IAuiPaneInfo paneInfo)
        {
            return paneInfo.Handle;
        }

        internal static IntPtr ToHandle(Control window)
        {
            return WxPlatform.WxWidget(window);
        }

        internal IAuiPaneInfo ToPaneInfo(IntPtr handle)
        {
            return new AuiPaneInfo(this, handle);
        }

        private void CheckManagedControl()
        {
            if (managedControl == null)
            {
                throw new Exception(string.Format(
                    ErrorMessages.Default.PropertyIsNull, nameof(ManagedControl)));
            }
        }

        private void ManagedWindow_Disposed(object? sender, EventArgs e)
        {
            UnInit();
        }
    }
}