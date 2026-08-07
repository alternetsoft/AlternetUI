using Alternet.Drawing;

namespace Alternet.UI;

/// <summary>
/// Handler for the Draw event of the ToolTip control.
/// </summary>
public delegate void DrawToolTipEventHandler(object? sender, DrawToolTipEventArgs e);

/// <summary>
/// This class contains the information a user needs to paint the tooltip.
/// </summary>
public class DrawToolTipEventArgs : BaseEventArgs
{
    private readonly Color backColor;
    private readonly Color foreColor;

    /// <summary>
    /// Creates a new DrawToolTipEventArgs class with the given parameters.
    /// </summary>
    public DrawToolTipEventArgs(
        Graphics graphics,
        AbstractControl? associatedWindow,
        AbstractControl? associatedControl,
        RectD bounds,
        string? toolTipText,
        Color backColor,
        Color foreColor,
        Font? font)
    {
        Graphics = graphics;
        AssociatedWindow = associatedWindow;
        AssociatedControl = associatedControl;
        Bounds = bounds;
        ToolTipText = toolTipText;
        this.backColor = backColor;
        this.foreColor = foreColor;
        Font = font;
    }

    /// <summary>
    ///  Graphics object with which painting should be done.
    /// </summary>
    public Graphics Graphics { get; set; }

    /// <summary>
    /// The window for which the tooltip is being painted.
    /// </summary>
    public AbstractControl? AssociatedWindow { get; set; }

    /// <summary>
    /// The control for which the tooltip is being painted.
    /// </summary>
    public AbstractControl? AssociatedControl { get; set; }

    /// <summary>
    /// The rectangle outlining the area in which the painting should be done.
    /// </summary>
    public RectD Bounds { get; set; }

    /// <summary>
    /// The text that should be drawn.
    /// </summary>
    public string? ToolTipText { get; set; }

    /// <summary>
    /// The font used to draw tooltip text.
    /// </summary>
    public Font? Font { get; set; }

    /// <summary>
    /// Draws the background of the tooltip.
    /// </summary>
    public virtual void DrawBackground()
    {
        Graphics.FillRectangle(backColor.AsBrush, Bounds);
    }

    /// <summary>
    /// Draws the text using the specified text format flags.
    /// </summary>
    public virtual void DrawText(TextFormatFlags flags)
    {
        if (ToolTipText is null)
            return;
        TextRenderer.DrawText(Graphics, ToolTipText, Font ?? Control.DefaultFont, Bounds, foreColor, flags);
    }

    /// <summary>
    /// Draws a border for the tooltip similar to the default border.
    /// </summary>
    public virtual void DrawBorder()
    {
        ControlPaint.DrawBorder(Graphics, Bounds, SystemColors.WindowFrame, ButtonBorderStyle.Solid);
    }
}
