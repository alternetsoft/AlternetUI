using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies all the parameters for a pane managed by <see cref="AuiManager"/>.
    /// </summary>
    /// <remarks>
    /// These parameters specify where the pane is on the screen, whether it
    /// is docked or floating, or hidden.
    /// In addition, these parameters specify the pane's docked position,
    /// floating position, preferred size, minimum size, caption text among
    /// many other parameters.
    /// </remarks>
    internal interface IAuiPaneInfo : IBaseControlItem, IDisposable
    {
        /// <summary>
        /// Gets owner.
        /// </summary>
        AuiManager Owner { get; }

        /// <summary>
        /// Returns handle of the pane info object.
        /// </summary>
        IntPtr Handle { get; }

        /// <summary>
        /// Writes the safe parts of a <see cref="IAuiPaneInfo"/> object
        /// <paramref name="source"/> into "this".
        /// </summary>
        /// <param name="source">Source from which pane property values
        /// are copied.</param>
        /// <remarks>
        /// "Safe parts" are all non-UI elements(e.g.all layout determining
        /// parameters like the size, position etc.).
        /// "Unsafe parts" (references to button, frame and window)
        /// are not modified by this write operation. This method is used when
        /// loading perspectives.
        /// </remarks>
        void SafeSet(IAuiPaneInfo source);

        /// <summary>
        /// Returns true if the <see cref="IAuiPaneInfo"/> structure is valid.
        /// </summary>
        /// <remarks>
        /// A pane structure is valid if it has an associated window.
        /// </remarks>
        /// <returns></returns>
        bool IsOk();

        /// <summary>
        /// Returns true if the pane cannot be resized.
        /// </summary>
        /// <returns></returns>
        bool IsFixed();

        /// <summary>
        /// Returns true if the pane can be resized.
        /// </summary>
        /// <returns></returns>
        bool IsResizable();

        /// <summary>
        /// Returns true if the pane is currently shown.
        /// </summary>
        /// <returns></returns>
        bool IsShown();

        /// <summary>
        /// Returns true if the pane is floating.
        /// </summary>
        /// <returns></returns>
        bool IsFloating();

        /// <summary>
        /// Returns true if the pane is currently docked.
        /// </summary>
        /// <returns></returns>
        bool IsDocked();

        /// <summary>
        /// Returns true if the pane contains a toolbar.
        /// </summary>
        /// <returns></returns>
        bool IsToolbar();

        /// <summary>
        /// Returns true if the pane can be docked at the top of the managed frame.
        /// </summary>
        /// <returns></returns>
        bool IsTopDockable();

        /// <summary>
        /// Returns true if the pane can be docked at the bottom of the managed frame.
        /// </summary>
        /// <returns></returns>
        bool IsBottomDockable();

        /// <summary>
        /// Returns true if the pane can be docked on the left of the managed frame.
        /// </summary>
        /// <returns></returns>
        bool IsLeftDockable();

        /// <summary>
        /// Returns true if the pane can be docked on the right of the managed frame.
        /// </summary>
        /// <returns></returns>
        bool IsRightDockable();

        /// <summary>
        /// Returns true if the pane can be docked at any side.
        /// </summary>
        /// <returns></returns>
        bool IsDockable();

        /// <summary>
        /// Returns true if the pane can be undocked and displayed
        /// as a floating window.
        /// </summary>
        /// <returns></returns>
        bool IsFloatable();

        /// <summary>
        /// Returns true if the docked frame can be undocked or moved to
        /// another dock position.
        /// </summary>
        /// <returns></returns>
        bool IsMovable();

        internal bool IsDestroyOnClose();

        /// <summary>
        /// Returns true if pane is maximized.
        /// </summary>
        /// <returns></returns>
        bool IsMaximized();

        /// <summary>
        /// Returns true if the pane displays a caption.
        /// </summary>
        /// <returns></returns>
        bool HasCaption();

        /// <summary>
        /// Returns true if the pane displays a gripper.
        /// </summary>
        /// <returns></returns>
        bool HasGripper();

        /// <summary>
        /// Returns true if the pane displays a border.
        /// </summary>
        /// <returns></returns>
        bool HasBorder();

        /// <summary>
        /// Returns true if the pane displays a button to close the pane.
        /// </summary>
        /// <returns></returns>
        bool HasCloseButton();

        /// <summary>
        /// Returns true if the pane displays a button to maximize the pane.
        /// </summary>
        /// <returns></returns>
        bool HasMaximizeButton();

        /// <summary>
        /// Returns true if the pane displays a button to minimize the pane.
        /// </summary>
        /// <returns></returns>
        bool HasMinimizeButton();

        /// <summary>
        /// Returns true if the pane displays a button to float the pane.
        /// </summary>
        /// <returns></returns>
        bool HasPinButton();

        /// <summary>
        /// Returns true if the pane displays a gripper at the top.
        /// </summary>
        /// <returns></returns>
        bool HasGripperTop();

        /// <summary>
        /// Assigns the control reference that the <see cref="IAuiPaneInfo"/> should use.
        /// </summary>
        /// <remarks>
        /// This normally does not need to be specified, as the control reference
        /// is automatically assigned to the <see cref="IAuiPaneInfo"/> structure
        /// as soon as it is added to the manager.
        /// </remarks>
        /// <param name="control">New control reference.</param>
        /// <returns>Returns this pane info instance.</returns>
        IAuiPaneInfo Window(Control control);

        /// <summary>
        /// Sets the name of the pane so it can be referenced in lookup functions.
        /// </summary>
        /// <remarks>
        /// If a name is not specified by the user, a random name is assigned to
        /// the pane when it is added to the manager.
        /// </remarks>
        /// <param name="value">New pane name.</param>
        /// <returns>Returns this pane info instance.</returns>
        IAuiPaneInfo Name(string value);

        /// <summary>
        /// Sets the caption of the pane.
        /// </summary>
        /// <param name="value">New caption value.</param>
        /// <returns>Returns this pane info instance.</returns>
        IAuiPaneInfo Caption(string value);

        /// <summary>
        /// Sets the icon of the pane.
        /// </summary>
        /// <param name="bitmap">Bitmap to show in the title bar.</param>
        /// <returns>Returns this pane info instance.</returns>
        IAuiPaneInfo Image(ImageSet? bitmap);

        /// <summary>
        /// Sets the pane dock position to the left of the frame.
        /// </summary>
        /// <remarks>
        /// This is the same thing as calling <see cref="Direction"/> with appropriate
        /// parameter value.
        /// </remarks>
        /// <returns>Returns this pane info instance.</returns>
        IAuiPaneInfo Left();

        /// <summary>
        /// Sets the pane dock position to the right of the frame.
        /// </summary>
        /// <remarks>
        /// This is the same thing as calling <see cref="Direction"/> with appropriate
        /// parameter value.
        /// </remarks>
        /// <returns>Returns this pane info instance.</returns>
        IAuiPaneInfo Right();

        /// <summary>
        /// Sets the pane dock position to the top of the frame.
        /// </summary>
        /// <remarks>
        /// This is the same thing as calling <see cref="Direction"/> with appropriate
        /// parameter value.
        /// </remarks>
        /// <returns>Returns this pane info instance.</returns>
        IAuiPaneInfo Top();

        /// <summary>
        /// Sets the pane dock position to the bottom side of the frame.
        /// </summary>
        /// <remarks>
        /// This is the same thing as calling <see cref="Direction"/> with appropriate
        /// parameter value.
        /// </remarks>
        /// <returns>Returns this pane info instance.</returns>
        IAuiPaneInfo Bottom();

        /// <summary>
        /// Sets the pane dock position to the center of the frame.
        /// </summary>
        /// <remarks>
        /// The centre pane is the space in the middle after all border
        /// panes (left, top, right, bottom) are subtracted from the layout.
        /// This is the same thing as calling <see cref="Direction"/> with
        /// appropriate parameter value.
        /// </remarks>
        /// <returns>Returns this pane info instance.</returns>
        IAuiPaneInfo Center();

        /// <summary>
        /// Determines the direction of the docked pane.
        /// </summary>
        /// <remarks>
        /// It is functionally the same as calling
        /// <see cref="Left"/>, <see cref="Right"/>, <see cref="Top"/> or
        /// <see cref="Bottom"/>, except that docking direction may be
        /// specified programmatically via the parameter.
        /// </remarks>
        /// <param name="direction">New docked pane direction.</param>
        /// <returns>Returns this pane info instance.</returns>
        IAuiPaneInfo Direction(AuiManagerDock direction);

        /// <summary>
        /// Determines the layer of the docked pane.
        /// </summary>
        /// <remarks>
        /// The dock layer is similar to an onion, the inner-most layer being
        /// layer 0. Each shell moving in the outward direction has a higher
        /// layer number. This allows for more complex docking layout formation.
        /// </remarks>
        /// <param name="layer">New pane layer value.</param>
        /// <returns>Returns this pane info instance.</returns>
        IAuiPaneInfo Layer(int layer);

        /// <summary>
        /// Sets the row of the docked pane.
        /// </summary>
        /// <param name="row">New value of the Row property.</param>
        /// <returns>Returns this pane info instance.</returns>
        IAuiPaneInfo Row(int row);

        /// <summary>
        /// Sets the position of the docked pane.
        /// </summary>
        /// <param name="pos">New panel position.</param>
        /// <returns>Returns this pane info instance.</returns>
        IAuiPaneInfo Position(int pos);

        /// <summary>
        /// Sets the ideal size for the pane.
        /// </summary>
        /// <remarks>
        /// The docking manager will attempt to use this size as much as
        /// possible when docking or floating the pane.
        /// </remarks>
        /// <param name="width">New best width.</param>
        /// <param name="height">New best height.</param>
        /// <returns>Returns this pane info instance.</returns>
        IAuiPaneInfo BestSizePixel(int width, int height);

        /// <inheritdoc cref="BestSizePixel(int,int)"/>
        IAuiPaneInfo BestSizeDip(double width, double height);

        /// <summary>
        /// Sets the minimum size of the pane.
        /// </summary>
        /// <param name="width">New minimal width.</param>
        /// <param name="height">New minimal height.</param>
        /// <returns>Returns this pane info instance.</returns>
        IAuiPaneInfo MinSizePixel(int width, int height);

        /// <inheritdoc cref="MinSizePixel(int,int)"/>
        IAuiPaneInfo MinSizeDip(double width, double height);

        /// <summary>
        /// Sets the maximum size of the pane.
        /// </summary>
        /// <param name="width">New maximal width.</param>
        /// <param name="height">New maximal height.</param>
        /// <returns>Returns this pane info instance.</returns>
        IAuiPaneInfo MaxSizePixel(int width, int height);

        /// <summary>
        /// Sets the maximum size of the pane.
        /// </summary>
        /// <param name="size">New maximal width and height.</param>
        /// <returns></returns>
        IAuiPaneInfo MaxSizeDip(SizeD size);

        /// <summary>
        /// Sets the position of the floating pane.
        /// </summary>
        /// <param name="x">New floating pane horizontal position.</param>
        /// <param name="y">New floating pane vertical position.</param>
        /// <returns>Returns this pane info instance.</returns>
        IAuiPaneInfo FloatingPositionPixel(int x, int y);

        /// <summary>
        /// Sets the size of the floating pane.
        /// </summary>
        /// <param name="width">New floating pane width.</param>
        /// <param name="height">New floating pane height.</param>
        /// <returns>Returns this pane info instance.</returns>
        IAuiPaneInfo FloatingSizePixel(int width, int height);

        /// <summary>
        /// Forces a pane to be fixed size so that it cannot be resized.
        /// </summary>
        /// <remarks>
        /// After calling this method, <see cref="IsFixed"/> will return true.
        /// </remarks>
        /// <returns>Returns this pane info instance.</returns>
        IAuiPaneInfo Fixed();

        /// <summary>
        /// Determines whether pane can be resized.
        /// </summary>
        /// <param name="resizable">Allows a pane to be resized if the parameter
        /// is <c>true</c>,
        /// and forces it to be a fixed size if the parameter is <c>false</c>.</param>
        /// <remarks>
        /// This is simply an antonym for <see cref="Fixed"/>.
        /// </remarks>
        /// <returns>Returns this pane info instance.</returns>
        IAuiPaneInfo Resizable(bool resizable = true);

        /// <summary>
        /// Indicates that a pane should be docked.
        /// It is the opposite of <see cref="Float"/>.
        /// </summary>
        /// <returns>Returns this pane info instance.</returns>
        IAuiPaneInfo Dock();

        /// <summary>
        /// Indicates that a pane should be floated.
        /// It is the opposite of <see cref="Dock"/>.
        /// </summary>
        /// <returns>Returns this pane info instance.</returns>
        IAuiPaneInfo Float();

        /// <summary>
        /// Indicates that a pane should be hidden.
        /// </summary>
        /// <returns>Returns this pane info instance.</returns>
        IAuiPaneInfo Hide();

        /// <summary>
        /// Indicates that a pane should be shown.
        /// </summary>
        /// <param name="show"><c>true</c> if a pane should be shown,
        /// <c>false</c> otherwise.</param>
        /// <returns>Returns this pane info instance.</returns>
        IAuiPaneInfo Show(bool show = true);

        /// <summary>
        /// Indicates that a pane caption should be visible.
        /// </summary>
        /// <param name="visible"> If <c>true</c>, pane caption is visible.
        /// If <c>false</c>, no pane caption is drawn.
        /// </param>
        /// <returns>Returns this pane info instance.</returns>
        IAuiPaneInfo CaptionVisible(bool visible = true);

        /// <summary>
        /// Maximizes pane size.
        /// </summary>
        /// <returns>Returns this pane info instance.</returns>
        IAuiPaneInfo Maximize();

        /// <summary>
        /// Restores pane position and size.
        /// </summary>
        /// <returns>Returns this pane info instance.</returns>
        IAuiPaneInfo Restore();

        /// <summary>
        /// Indicates that a border should be drawn for the pane.
        /// </summary>
        /// <param name="visible"></param>
        /// <returns>Returns this pane info instance.</returns>
        IAuiPaneInfo PaneBorder(bool visible = true);

        /// <summary>
        /// Indicates that a gripper should be drawn for the pane.
        /// </summary>
        /// <param name="visible"></param>
        /// <returns>Returns this pane info instance.</returns>
        IAuiPaneInfo Gripper(bool visible = true);

        /// <summary>
        /// Indicates that a gripper should be drawn at the top of the pane.
        /// </summary>
        /// <param name="attop"></param>
        /// <returns>Returns this pane info instance.</returns>
        IAuiPaneInfo GripperTop(bool attop = true);

        /// <summary>
        /// Indicates that a close button should be drawn for the pane.
        /// </summary>
        /// <param name="visible"></param>
        /// <returns>Returns this pane info instance.</returns>
        IAuiPaneInfo CloseButton(bool visible = true);

        /// <summary>
        /// Indicates that a maximize button should be drawn for the pane.
        /// </summary>
        /// <param name="visible"></param>
        /// <returns>Returns this pane info instance.</returns>
        IAuiPaneInfo MaximizeButton(bool visible = true);

        /// <summary>
        /// Indicates that a minimize button should be drawn for the pane.
        /// </summary>
        /// <param name="visible"></param>
        /// <returns>Returns this pane info instance.</returns>
        IAuiPaneInfo MinimizeButton(bool visible = true);

        /// <summary>
        /// Indicates that a pin button should be drawn for the pane.
        /// </summary>
        /// <param name="visible"><c>true</c> if a pin button should be drawn,
        /// <c>false</c> otherwise.</param>
        /// <returns>Returns this pane info instance.</returns>
        IAuiPaneInfo PinButton(bool visible = true);

        /// <summary>
        /// Indicates whether a pane should be destroyed when it is closed.
        /// </summary>
        /// <remarks>
        /// Normally a pane is simply hidden when the close button is clicked.
        /// Setting this property to true will cause the window to be destroyed
        /// when the user clicks the pane's close button.
        /// </remarks>
        /// <param name="b"></param>
        /// <returns>Returns this pane info instance.</returns>
        internal IAuiPaneInfo DestroyOnClose(bool b = true);

        /// <summary>
        /// Indicates whether a pane can be docked at the top of the frame.
        /// </summary>
        /// <param name="b"><c>true</c> if pane can be docked at the top,
        /// <c>false</c> otherwise.</param>
        /// <returns>Returns this pane info instance.</returns>
        IAuiPaneInfo TopDockable(bool b = true);

        /// <summary>
        /// Indicates whether a pane can be docked at the bottom of the frame.
        /// </summary>
        /// <param name="b"><c>true</c> if pane can be docked at the bottom,
        /// <c>false</c> otherwise.</param>
        /// <returns>Returns this pane info instance.</returns>
        IAuiPaneInfo BottomDockable(bool b = true);

        /// <summary>
        /// Indicates whether a pane can be docked on the left of the frame.
        /// </summary>
        /// <param name="b"><c>true</c> if pane can be docked on the left,
        /// <c>false</c> otherwise.</param>
        /// <returns>Returns this pane info instance.</returns>
        IAuiPaneInfo LeftDockable(bool b = true);

        /// <summary>
        /// Indicates whether a pane can be docked on the right of the frame.
        /// </summary>
        /// <param name="b"><c>true</c> if pane can be docked on the right,
        /// <c>false</c> otherwise.</param>
        /// <returns>Returns this pane info instance.</returns>
        IAuiPaneInfo RightDockable(bool b = true);

        /// <summary>
        /// Sets whether the user will be able to undock a pane and turn
        /// it into a floating window.
        /// </summary>
        /// <param name="b"></param>
        /// <returns>Returns this pane info instance.</returns>
        IAuiPaneInfo Floatable(bool b = true);

        /// <summary>
        /// Indicates whether a frame can be moved.
        /// </summary>
        /// <param name="b"></param>
        /// <returns>Returns this pane info instance.</returns>
        IAuiPaneInfo Movable(bool b = true);

        /// <summary>
        /// Causes the containing dock to have no resize sash.
        /// </summary>
        /// <remarks>
        /// This is useful for creating panes that span the entire width
        /// or height of a dock, but should not be resizable in the other direction.
        /// </remarks>
        /// <param name="b"></param>
        /// <returns>Returns this pane info instance.</returns>
        IAuiPaneInfo DockFixed(bool b = true);

        /// <summary>
        /// Specifies whether a frame can be docked or not.
        /// </summary>
        /// <remarks>
        /// It is the same as specifying <see cref="TopDockable"/>,
        /// <see cref="BottomDockable"/>, <see cref="LeftDockable"/> and
        /// <see cref="RightDockable"/> with the same
        /// parameter value.
        /// </remarks>
        /// <param name="b"></param>
        /// <returns>Returns this pane info instance.</returns>
        IAuiPaneInfo Dockable(bool b = true);

        internal bool IsValid();

        /// <summary>
        /// Specifies that the pane should adopt the default pane settings.
        /// </summary>
        /// <returns>Returns this pane info instance.</returns>
        IAuiPaneInfo DefaultPane();

        /// <summary>
        /// Specifies that the pane should adopt the default center pane settings.
        /// </summary>
        /// <remarks>
        /// Centre panes usually do not have caption bars. This function provides
        /// an easy way of preparing a pane to be displayed in the center
        /// dock position.
        /// </remarks>
        /// <returns>Returns this pane info instance.</returns>
        IAuiPaneInfo CenterPane();

        /// <summary>
        /// Specifies that the pane should adopt the default toolbar pane settings.
        /// </summary>
        /// <returns>Returns this pane info instance.</returns>
        IAuiPaneInfo ToolbarPane();

        /// <summary>
        /// Turns the property given by flag on or off with the
        /// <paramref name="option_state"/> parameter.
        /// </summary>
        /// <param name="flag">Flag identifier.</param>
        /// <param name="option_state"></param>
        void SetFlag(int flag, bool option_state);

        /// <summary>
        /// Returns true if the property specified by flag is active for the pane.
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        bool HasFlag(int flag);

        /// <summary>
        /// Gets size that the layout engine will prefer.
        /// </summary>
        SizeI GetBestSizePixel();

        /// <summary>
        /// Gets minimum size the pane window can tolerate.
        /// </summary>
        SizeI GetMinSizePixel();

        /// <summary>
        /// Gets maximum size the pane window can tolerate.
        /// </summary>
        SizeI GetMaxSizePixel();
    }
}