#if ALTERNET_UI_INTEGRATION_REMOTING
namespace Alternet.UI.Integration.Remoting
#else
namespace Alternet.UI
#endif
{
    /// <summary>
    ///     The MouseButton enumeration describes the buttons available on
    ///     the mouse device.
    /// </summary>
    public enum MouseButton
    {
        /// <summary>
        ///    The left mouse button.
        /// </summary>
        Left,

        /// <summary>
        ///    The middle mouse button.
        /// </summary>
        Middle,

        /// <summary>
        ///    The right mouse button.
        /// </summary>
        Right,

        /// <summary>
        ///    The fourth mouse button.
        /// </summary>
        XButton1,

        /// <summary>
        ///    The fifth mouse button.
        /// </summary>
        XButton2,
    }
}