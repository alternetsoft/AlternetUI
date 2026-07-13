using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI;

/// <summary>
/// Defines the parameters for the popup entry.
/// </summary>
public struct PopupEntryParams
{
    private RectD itemRect;

    /// <summary>
    /// Initializes a new instance of the <see cref="PopupEntryParams"/> struct.
    /// </summary>
    public PopupEntryParams()
    {
    }

    /// <summary>
    /// Gets or sets the debug identifier of the entry.
    /// </summary>
    public string? DebugIdentifier { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the popup should hide when clicking on the parent control.
    /// </summary>
    public bool HideClickOnParent { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether text should be committed each time the text changes.
    /// If set to true, the text is commited on each text change. If false,
    /// the text is commited only when the user presses Enter.
    /// </summary>
    public bool CommitTextOnKeyPress { get; set; }

    /// <summary>
    /// Gets or sets the action to be performed when the Tab key is pressed while the popup is active.
    /// </summary>
    public Action? TabPressed { get; set; }

    /// <summary>
    /// Gets or sets the action to be performed when the Enter key is pressed while the popup is active.
    /// </summary>
    public Action? EnterPressed { get; set; }

    /// <summary>
    /// Gets or sets the action to be performed when the Escape key is pressed while the popup is active.
    /// </summary>
    public Action? EscapePressed { get; set; }

    /// <summary>
    /// Gets or sets the action to be performed when a key is pressed while the popup is active.
    /// </summary>
    public KeyEventHandler? KeyDown { get; set; }

    /// <summary>
    /// Gets or sets the background color of the popup.
    /// </summary>
    public Color? BackColor { get; set; }

    /// <summary>
    /// Gets or sets the foreground color of the popup.
    /// </summary>
    public Color? ForeColor { get; set; }

    /// <summary>
    /// Gets or sets the font of the popup.
    /// </summary>
    public Font? Font { get; set; }

    /// <summary>
    /// Gets or sets the bounds of the item being edited. This is used to position the popup control.
    /// Rectangle is in client coordinates of the <see cref="ItemContainer"/>.
    /// </summary>
    public RectD ItemRect
    {
        readonly get
        {
            return GetItemRect?.Invoke() ?? itemRect;
        }

        set => itemRect = value;
    }

    /// <summary>
    /// Gets or sets the function that is called to get the bounds of the item being edited.
    /// This can be used to dynamically determine the position of the popup control.
    /// </summary>
    public Func<RectD>? GetItemRect { get; set; }

    /// <summary>
    /// Gets or sets whether popup is closed when Escape key is pressed.
    /// If set to true, the popup is closed when Escape key is pressed.
    /// </summary>
    public bool HideOnEscape { get; set; } = true;

    /// <summary>
    /// Gets or sets whether popup is closed when Enter key is pressed.
    /// If set to true, the popup is closed when Enter key is pressed.
    /// </summary>
    public bool HideOnEnter { get; set; } = true;

    /// <summary>
    /// Gets or sets the container control that hosts the popup.
    /// This is used to determine the parent control for the popup.
    /// </summary>
    public AbstractControl? ItemContainer { get; set; }

    /// <summary>
    /// Gets or sets the target control for which popup is being shown.
    /// This is different from <see cref="ItemContainer"/> in that it could be a generic control.
    /// </summary>
    public AbstractControl? TargetControl { get; set; }

    /// <summary>
    /// Gets or sets the function that is called to get initial text for the <see cref="TextBox"/> control.
    /// </summary>
    public Func<string?>? GetItemText { get; set; }

    /// <summary>
    /// Gets or sets the function that is called to set the text from the <see cref="TextBox"/>
    /// control back to the item being edited. This is only
    /// called if the user confirms the edit (e.g., by pressing Enter).
    /// </summary>
    public Action<string?>? SetItemText { get; set; }

    /// <summary>
    /// Gets or sets the action that is called when the popup is closed.
    /// This can be used to perform any cleanup or additional actions after the popup is closed.
    /// </summary>
    public Action? Closed { get; set; }

    /// <summary>
    /// Gets or sets the action that is called when the popup is closing.
    /// This can be used to perform any actions before the popup is closed.
    /// </summary>
    public Action? Closing { get; set; }

    /// <summary>
    /// Gets or sets the action that is called when the entry height is changed.
    /// </summary>
    public Action<float>? EntryHeightChanged { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the border should be visible.
    /// </summary>
    public bool HasBorder { get; set; } = true;

    /// <summary>
    /// Gets or sets empty text hint which is shown in the editor when text is empty.
    /// </summary>
    public string? EmptyTextHint { get; set; }

    /// <summary>
    /// Sets target control and item container for the popup.
    /// This is a convenience method to set both properties at once.
    /// Item container is determined by the target control's first parent which is a platform control.
    /// </summary>
    /// <param name="targetControl">The target control for which the popup is being shown.</param>
    /// <param name="itemRect">The bounds of the item being edited.</param>
    public void SetTargetControl(AbstractControl? targetControl, RectD itemRect)
    {
        TargetControl = targetControl;
        ItemContainer = targetControl;

        while (ItemContainer != null && !ItemContainer.IsPlatformControl)
        {
            itemRect.Location += ItemContainer.Location;
            ItemContainer = ItemContainer.Parent;
        }

        ItemRect = itemRect;
    }

    /// <summary>
    /// Sets target control and item container for the popup.
    /// This is a convenience method to set both properties at once.
    /// Item container is determined by the target control's first parent which is a platform control.
    /// </summary>
    /// <param name="targetControl">The target control for which the popup is being shown.</param>
    /// <param name="func">The function that returns the bounds of the item being edited.</param>
    public void SetTargetControl(AbstractControl? targetControl, Func<RectD> func)
    {
        TargetControl = targetControl;
        ItemContainer = targetControl;

        GetItemRect = Internal;

        while (ItemContainer != null && !ItemContainer.IsPlatformControl)
        {
            ItemContainer = ItemContainer.Parent;
        }

        RectD Internal()
        {
            var result = func();

            var itemContainer = targetControl;

            while (itemContainer != null && !itemContainer.IsPlatformControl)
            {
                result.Location += itemContainer.Location;
                itemContainer = itemContainer.Parent;
            }

            return result;
        }
    }
}
