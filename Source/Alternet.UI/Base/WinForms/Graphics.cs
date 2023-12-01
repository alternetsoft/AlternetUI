namespace Alternet.Drawing
{
    /// <summary>
    /// Defined in order to make library more compatible with the legacy code.
    /// </summary>
    public class Graphics : DrawingContext
    {
        internal Graphics(UI.Native.DrawingContext dc)
            : base(dc)
        {
        }
    }
}
