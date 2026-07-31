using Alternet.Drawing;

namespace Alternet.UI;

/// <summary>
/// Represents the method that handles the <see cref="AbstractControl.GiveFeedback"/> event of a control.
/// </summary>
/// <param name="sender">The source of the event.</param>
/// <param name="e">A <see cref="GiveFeedbackEventArgs"/> that contains the event data.</param>
public delegate void GiveFeedbackEventHandler(object sender, GiveFeedbackEventArgs e);

/// <summary>
/// Provides data for the <see cref="AbstractControl.GiveFeedback"/> event.
/// </summary>
public class GiveFeedbackEventArgs : BaseEventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GiveFeedbackEventArgs"/>
    /// class with the specified <see cref="DragDropEffects"/> value.
    /// </summary>
    public GiveFeedbackEventArgs(DragDropEffects effect, bool useDefaultCursors)
        : this(effect, useDefaultCursors, dragImage: default!, cursorOffset: default, useDefaultDragImage: false)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GiveFeedbackEventArgs"/> class with the specified
    /// <see cref="DragDropEffects"/> value, drag image, cursor offset, and a value indicating
    /// whether to use the default drag image.
    /// </summary>
    public GiveFeedbackEventArgs(
        DragDropEffects effect,
        bool useDefaultCursors,
        Bitmap? dragImage,
        PointD cursorOffset,
        bool useDefaultDragImage)
    {
        Effect = effect;
        UseDefaultCursors = useDefaultCursors;
        DragImage = dragImage;
        CursorOffset = cursorOffset;
        UseDefaultDragImage = useDefaultDragImage;
    }

    /// <summary>
    /// Gets the type of drag-and-drop operation.
    /// </summary>
    public DragDropEffects Effect { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether a default pointer is used.
    /// </summary>
    public bool UseDefaultCursors { get; set; }

    /// <summary>
    /// Gets or sets the drag image bitmap.
    /// </summary>
    public Bitmap? DragImage { get; set; }

    /// <summary>
    /// Gets or sets the drag image cursor offset.
    /// </summary>
    /// <remarks>
    /// Specifies the location of the cursor within <see cref="DragImage"/>, which is an offset from the upper-left corner.
    /// </remarks>
    public PointD CursorOffset { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether a layered window drag image is used.
    /// </summary>
    /// <remarks>
    /// Specify <see langword="true"/> for <see cref="UseDefaultDragImage"/> to use a layered window drag image with a size of 96x96;
    /// otherwise <see langword="false"/>.
    /// </remarks>
    public bool UseDefaultDragImage { get; set; }
}
