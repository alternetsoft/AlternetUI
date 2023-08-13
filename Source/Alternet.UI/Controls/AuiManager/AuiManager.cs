using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    // https://learn.microsoft.com/ru-ru/dotnet/api/system.idisposable?view=net-7.0

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
    public class AuiManager : IDisposable
    {
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

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            if (handle != IntPtr.Zero)
            {
                Native.AuiManager.Delete(handle);
                handle = IntPtr.Zero;
            }
        }

        public void UnInit()
        {
            Native.AuiManager.UnInit(handle);
        }

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
        /// Returns true if live resize is always used on the current platform.
        /// </summary>
        /// <remarks>
        /// If this function returns true, LiveResize flag is ignored and live
        /// resize is always used, whether it's specified or not. Currently this
        /// is the case for MacOs and Linux ports, as live resizing is the
        /// only implemented method there.
        /// </remarks>
        public bool AlwaysUsesLiveResize()
        {
            return Native.AuiManager.AlwaysUsesLiveResize();
        }


        /*
        Returns true if windows are resized live.

        This function combines the check for AlwaysUsesLiveResize() and, for the platforms where live resizing is optional, the check for wxAUI_MGR_LIVE_RESIZE flag.

        Using this accessor allows to verify whether live resizing is being actually used.  
         */
        public bool HasLiveResize()
        {
            return Native.AuiManager.HasLiveResize(handle);
        }

        /*
This method is called after any number of changes are made to any of the managed panes.

Update() must be invoked after AddPane() or InsertPane() are called in order to "realize" or "commit" the changes. In addition, any number of changes may be made to wxAuiPaneInfo structures (retrieved with wxAuiManager::GetPane), but to realize the changes, Update() must be called. This construction allows pane flicker to be avoided by updating the whole layout at one time.
         
         */
        public void Update()
        {
            Native.AuiManager.Update(handle);
        }

        public string SavePerspective()
        {
            return Native.AuiManager.SavePerspective(handle);
        }

        /*
         Loads a saved perspective.

        A perspective is the layout state of an AUI managed window.

        All currently existing panes that have an object in "perspective" with the same name ("equivalent") will receive the layout parameters of the object in "perspective". Existing panes that do not have an equivalent in "perspective" remain unchanged, objects in "perspective" having no equivalent in the manager are ignored.

        Parameters
        perspective	Serialized layout information of a perspective (excl. pointers to UI elements).
        update	If update is true, wxAuiManager::Update() is automatically invoked, thus realizing the specified perspective on screen.
        * 
         */
        public bool LoadPerspective(string perspective, bool update = true)
        {
            return Native.AuiManager.LoadPerspective(handle, perspective, update);
        }

        public void SetDockSizeConstraint(double widthPct, double heightPct)
        {
            Native.AuiManager.SetDockSizeConstraint(handle, widthPct, heightPct);
        }

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

        public void SetManagedWindow(Window managedWnd)
        {
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
        /// <returns><c>true</c> if new pane was added successfully.</returns>
        public bool AddPane(
            Control control,
            IAuiPaneInfo paneInfo,
            Point dropPos)
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

        /*
         This method is used to insert either a previously unmanaged pane window into the frame manager, or to insert a currently managed pane somewhere else.

        InsertPane() will push all panes, rows, or docks aside and insert the window into the position specified by insert_location.

        Because insert_location can specify either a pane, dock row, or dock layer, the insert_level parameter is used to disambiguate this. The parameter insert_level can take a value of wxAUI_INSERT_PANE, wxAUI_INSERT_ROW or wxAUI_INSERT_DOCK.
        * 
         */
        public bool InsertPane(
            Control window,
            IAuiPaneInfo insertLocPaneInfo,
            AuiPaneInsertLevel insertLevel)
        {
            return Native.AuiManager.InsertPane(
                handle,
                ToHandle(window),
                ToHandle(insertLocPaneInfo),
                (int)insertLevel);
        }

        public string SavePaneInfo(IAuiPaneInfo paneInfo)
        {
            return Native.AuiManager.SavePaneInfo(handle, ToHandle(paneInfo));
        }

        /*
        LoadPaneInfo() is similar to LoadPerspective, with the exception that it only loads information about a single pane.

        This method writes the serialized data into the passed pane. Pointers to UI elements are not modified.

        Note
        This operation also changes the name in the pane information!

         */
        public void LoadPaneInfo(string panePart, IAuiPaneInfo paneInfo)
        {
            Native.AuiManager.LoadPaneInfo(handle, panePart, ToHandle(paneInfo));
        }

        /// <summary>
        /// Returns the current dock constraint values.
        /// </summary>
        public Size GetDockSizeConstraint()
        {
            return Native.AuiManager.GetDockSizeConstraint(handle);
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

        public void RestorePane(IAuiPaneInfo paneInfo)
        {
            Native.AuiManager.RestorePane(handle, ToHandle(paneInfo));
        }

        /// <summary>
        /// Check if a key modifier is pressed (actually Control or Alt) while
        /// dragging the frame to not dock the window.
        /// </summary>
        /// <param name="paneInfo">Panel information.</param>
        /// <returns></returns>
        public bool CanDockPanel(IAuiPaneInfo paneInfo)
        {
            return Native.AuiManager.CanDockPanel(handle, ToHandle(paneInfo));
        }

        /// <summary>
        /// Creates new pane for use with <see cref="AuiManager"/>.
        /// </summary>
        /// <returns><see cref="IAuiPaneInfo"/> instance with empty settings.</returns>
        public IAuiPaneInfo CreateAuiPaneInfo()
        {
            return new AuiPaneInfo();
        }

        internal static IAuiPaneInfo ToPaneInfo(IntPtr handle)
        {
            return new AuiPaneInfo(handle);
        }

        internal static IntPtr ToHandle(IAuiPaneInfo paneInfo)
        {
            return paneInfo.Handle;
        }

        internal static IntPtr ToHandle(Control window)
        {
            return window.Handler.NativeControl!.WxWidget;
        }
    }
}

/*
===========

RestoreMaximizedPane()
Restore the previously maximized pane.


RestorePane()
Restore the last state of the given pane.


SavePaneInfo()
SavePaneInfo() is similar to SavePerspective, with the exception that it only saves information about a single pane.
pane	Pane whose layout parameters should be serialized.
Returns
The serialized layout parameters of the pane are returned within the string. Information about the pointers to UI elements stored in the pane are not serialized.

wxString wxAuiManager::SavePerspective
Saves the entire user interface layout into an encoded wxString, which can then be stored by the application (probably using wxConfig).

void wxAuiManager::SetDockSizeConstraint
When a user creates a new dock by dragging a window into a docked position, often times the large size of the window will create a dock that is unwieldy large.

wxAuiManager by default limits the size of any new dock to 1/3 of the window size. For horizontal docks, this would be 1/3 of the window height. For vertical docks, 1/3 of the width.

Calling this function will adjust this constraint value. The numbers must be between 0.0 and 1.0. For instance, calling SetDockSizeContraint with 0.5, 0.5 will cause new docks to be limited to half of the size of the entire managed window.


SetFlags()
This method is used to specify wxAuiManagerOption's flags.
flags specifies options which allow the frame management behaviour to be modified.


SetManagedWindow()
Called to specify the frame or window which is to be managed by wxAuiManager.
Frame management is not restricted to just frames. Child windows or custom controls are also allowed.


ShowHint()
virtual void wxAuiManager::ShowHint	(	const wxRect & 	rect	)	
virtual
This function is used by controls to explicitly show a hint window at the specified rectangle.

It is rarely called, and is mostly used by controls implementing custom pane drag/drop behaviour. The specified rectangle should be in screen coordinates.

UnInit()
Dissociate the managed window from the manager.
This function may be called before the managed frame or window is destroyed, but, since wxWidgets 3.1.4, it's unnecessary to call it explicitly, as it will be called automatically when this window is destroyed, as well as when the manager itself is.


public void SetArtProvider(IntPtr artProvider) { }
public IntPtr GetArtProvider(){};

//public IntPtr[] GetAllPanes(){};


public IntPtr CreateFloatingFrame(,
    IntPtr parentWindow, IAuiPaneInfo paneInfo){};
*/