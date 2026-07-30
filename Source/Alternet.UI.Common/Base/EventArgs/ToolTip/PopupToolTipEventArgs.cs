using System.ComponentModel;

using Alternet.Drawing;

namespace Alternet.UI;

/// <summary>
/// Handler for the popup event of the tooltip.
/// </summary>
public delegate void PopupEventHandler(object? sender, PopupToolTipEventArgs e);

/// <summary>
/// This class contains the information a user needs to handle the popup event of the tooltip.
/// </summary>
public class PopupToolTipEventArgs : BaseCancelEventArgs
{
    /// <summary>
    /// Creates a new PopupToolTipEventArgs with the given parameters.
    /// </summary>
    public PopupToolTipEventArgs(AbstractControl? associatedWindow, AbstractControl? associatedControl, bool isBalloon, SizeD size)
    {
        AssociatedWindow = associatedWindow;
        AssociatedControl = associatedControl;
        ToolTipSize = size;
        IsBalloon = isBalloon;
    }

    /// <summary>
    /// The associated window for which the tooltip is being painted.
    /// </summary>
    public AbstractControl? AssociatedWindow { get; set; }

    /// <summary>
    /// The control for which the tooltip is being painted.
    /// </summary>
    public AbstractControl? AssociatedControl { get; set; }

    /// <summary>
    /// The rectangle outlining the area in which the painting should be done.
    /// </summary>
    public SizeD ToolTipSize { get; set; }

    /// <summary>
    /// Whether the tooltip is Ballooned.
    /// </summary>
    public bool IsBalloon { get; set; }
}
