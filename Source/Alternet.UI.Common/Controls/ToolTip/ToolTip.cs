using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using Alternet.Drawing;

namespace Alternet.UI;

// https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.tooltip?view=windowsdesktop-10.0

/// <summary>
/// Represents a tooltip component that displays a small pop-up window with a brief description of a control's purpose
/// when the user hovers the mouse pointer over the control. Currently, this class is not fully implemented and
/// not all features are available.
/// </summary>
[DefaultEvent(nameof(Popup))]
public partial class ToolTip : Component
{
    /// <summary>
    /// Gets or sets default value of the <see cref="AutomaticDelay"/> property.
    /// This is the time (in milliseconds) that passes before the tooltip appears.
    /// </summary>
    public static int DefaultAutomaticDelay = 500;

    /// <summary>    
    /// Gets or sets default value of the <see cref="AutoPopDelay"/> property.
    /// </summary>
    public static int DefaultAutoPopDelay = 5000;

    /// <summary>    
    /// Gets or sets default value of the <see cref="InitialDelay"/> property.
    /// </summary>
    public static int DefaultInitialDelay = 500;

    /// <summary>    
    /// Gets or sets default value of the <see cref="ReshowDelay"/> property.
    /// This is the length of time (in milliseconds) that it takes subsequent tooltip
    /// instances to appear as the mouse pointer moves from one tooltip region to another.
    /// </summary>
    public static int DefaultReshowDelay = 100;

    private Color backColor;
    private Color foreColor;
    private int automaticDelay = DefaultAutomaticDelay;
    private int autoPopDelay = DefaultAutoPopDelay;
    private int initialDelay = DefaultInitialDelay;
    private int reshowDelay = DefaultReshowDelay;

    private ToolTipIcon toolTipIcon;
    private string? toolTipTitle;
    private bool active;
    private bool isBalloon = false;
    private bool ownerDraw = false;
    private bool showAlways = false;
    private bool stripAmpersands = false;
    private bool useAnimation = false;
    private bool useFading = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="ToolTip"/> class in its default state.
    /// </summary>
    public ToolTip()
    {
        backColor = SystemColors.Info;
        foreColor = SystemColors.InfoText;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ToolTip"/> class with the specified container.
    /// </summary>
    /// <param name="cont">The container to add the tooltip to.</param>
    public ToolTip(IContainer cont)
        : this()
    {
        ArgumentNullException.ThrowIfNull(cont);

        cont.Add(this);
    }

    /// <summary>
    /// Gets or sets the time (in milliseconds) that passes before the tooltip appears.
    /// Currently this property doesn't have any effect and is implemented for compatibility.
    /// </summary>
    [RefreshProperties(RefreshProperties.All)]
    public virtual int AutomaticDelay
    {
        get => automaticDelay;
        set
        {
            if (automaticDelay == value)
                return;
            automaticDelay = value;
        }
    }

    /// <summary>
    /// Gets or sets the initial delay for the tooltip.
    /// Currently this property doesn't have any effect and is implemented for compatibility.
    /// </summary>
    [RefreshProperties(RefreshProperties.All)]
    internal virtual int AutoPopDelay
    {
        get => autoPopDelay;
        set
        {
            if (autoPopDelay == value)
                return;
            autoPopDelay = value;
        }
    }

    /// <summary>
    /// Gets or sets the BackColor for the tooltip.
    /// Currently this property doesn't have any effect and is implemented for compatibility.
    /// </summary>
    [DefaultValue(typeof(Color), "Info")]
    internal virtual Color BackColor
    {
        get => backColor;
        set
        {
            if (backColor == value)
                return;
            backColor = value;
        }
    }

    /// <summary>
    /// Gets or sets the ForeColor for the tooltip.
    /// Currently this property doesn't have any effect and is implemented for compatibility.
    /// </summary>
    [DefaultValue(typeof(Color), "InfoText")]
    internal virtual Color ForeColor
    {
        get => foreColor;
        set
        {
            if (foreColor == value)
                return;
            foreColor = value;
        }
    }

    /// <summary>
    /// Gets or sets the IsBalloon for the tooltip.
    /// Currently this property doesn't have any effect and is implemented for compatibility.
    /// </summary>
    [DefaultValue(false)]
    internal virtual bool IsBalloon
    {
        get => isBalloon;
        set
        {
            if (isBalloon == value)
                return;
            isBalloon = value;
        }
    }

    /// <summary>
    /// Gets or sets the initial delay for the tooltip.
    /// </summary>
    [RefreshProperties(RefreshProperties.All)]
    internal virtual int InitialDelay
    {
        get => initialDelay;
        set
        {
            if (initialDelay == value)
                return;
            initialDelay = value;
        }
    }

    /// <summary>
    /// Indicates whether the tooltip will be drawn by the system or the user.
    /// This property doesn't have any effect and is implemented for compatibility.
    /// </summary>
    [DefaultValue(false)]
    internal virtual bool OwnerDraw
    {
        get => ownerDraw;
        set
        {
            ownerDraw = value;
        }
    }

    /// <summary>
    /// Gets or sets the length of time (in milliseconds) that it takes subsequent tooltip
    /// instances to appear as the mouse pointer moves from one tooltip region to another.
    /// </summary>
    [RefreshProperties(RefreshProperties.All)]
    internal virtual int ReshowDelay
    {
        get => reshowDelay;
        set
        {
            if (reshowDelay == value)
                return;
            reshowDelay = value;
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the tooltip appears even when its
    /// parent control is not active.
    /// </summary>
    [DefaultValue(false)]
    internal virtual bool ShowAlways
    {
        get => showAlways;
        set
        {
            if (showAlways == value)
                return;
            showAlways = value;
        }
    }

    /// <summary>
    /// When set to true, any ampersands in the Text property are not displayed.
    /// </summary>
    [Browsable(true)]
    [DefaultValue(false)]
    internal virtual bool StripAmpersands
    {
        get => stripAmpersands;
        set
        {
            if (stripAmpersands == value)
                return;
            stripAmpersands = value;
        }
    }

    /// <summary>
    /// Gets or sets an Icon on the tooltip.
    /// </summary>
    [DefaultValue(ToolTipIcon.None)]
    internal virtual ToolTipIcon ToolTipIcon
    {
        get => toolTipIcon;
        set
        {
            if (toolTipIcon == value)
                return;
            toolTipIcon = value;
        }
    }

    /// <summary>
    /// Gets or sets the title of the tooltip.
    /// </summary>
    /// <remarks>
    /// The title is displayed within the window as a line of bold text above the standard text of a toolTip description.
    /// Typically, titles are used either to differentiate different categories of controls on a form or as an
    /// introduction to a long description.
    /// </remarks>
    [DefaultValue(null)]
    [AllowNull]
    internal virtual string? ToolTipTitle
    {
        get => toolTipTitle;
        set
        {
            if (toolTipTitle == value)
                return;
            toolTipTitle = value;
        }
    }

    /// <summary>
    /// When set to true, animations are used when tooltip is shown or hidden.
    /// This property doesn't have any effect and is implemented for compatibility.
    /// </summary>
    [Browsable(true)]
    [DefaultValue(true)]
    internal virtual bool UseAnimation
    {
        get => useAnimation;
        set => useAnimation = value;
    }

    /// <summary>
    /// When set to true, a fade effect is used when tooltips are shown or hidden.
    /// This property doesn't have any effect and is implemented for compatibility.
    /// </summary>
    [Browsable(true)]
    [DefaultValue(true)]
    internal virtual bool UseFading
    {
        get => useFading;
        set
        {
            if (useFading == value)
                return;
            useFading = value;
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the tooltip is currently active.
    /// </summary>
    [DefaultValue(true)]
    internal virtual bool Active
    {
        get => active;
        set
        {
            active = value;
        }
    }

    /// <summary>
    /// Fires in OwnerDraw mode when the tooltip needs to be drawn.
    /// </summary>
    internal event DrawToolTipEventHandler? Draw;

    /// <summary>
    /// Fires when the tooltip is just about to be shown.
    /// </summary>
    internal event PopupToolTipEventHandler? Popup;

    /// <summary>
    /// Returns true if the tooltip can offer an extender property to the specified target component.
    /// </summary>
    public virtual bool CanExtend(object target) => target is Control;

    /// <summary>
    /// Retrieves the tooltip text associated with the specified control.
    /// </summary>
    [DefaultValue(null)]
    [Localizable(true)]
    public virtual string? GetToolTip(AbstractControl? control)
    {
        return control?.ToolTip;
    }

    /// <summary>
    /// Associates tooltip text with the specified control.
    /// </summary>
    /// <param name="tooltip">The text to display in the tooltip.</param>
    /// <param name="control">The control with which the tooltip is associated.</param>
    public virtual void SetToolTip(AbstractControl control, object? tooltip)
    {
        control.ToolTipObject = tooltip;
    }

    /// <summary>
    /// Associates tooltip with the specified control and displays it for the
    /// specified duration or until tooltip is dismissed.
    /// </summary>
    /// <param name="tooltip">The text to display in the tooltip.</param>
    /// <param name="control">The control with which the tooltip is associated.</param>
    /// <param name="duration">The duration for which the tooltip is displayed (in milliseconds).
    /// If duration is not specified, the tooltip will be displayed until the Hide method is called,
    /// or until the parent form is minimized, hidden, or dismissed.</param>
    public void Show(object? tooltip, AbstractControl control, int? duration = null)
    {
        Show(tooltip, control, null, duration);
    }

    /// <summary>
    /// Associates tooltip with the specified control and displays it at the specified point.
    /// </summary>
    /// <param name="point">The point at which to display the tooltip. If null, the tooltip will be displayed
    /// at the default location.</param>
    /// <param name="tooltip">The text to display in the tooltip.</param>
    /// <param name="control">The control with which the tooltip is associated.</param>
    /// <param name="duration">The duration for which the tooltip is displayed (in milliseconds).
    /// If duration is not specified, the tooltip will be displayed until the Hide method is called,
    /// or until the parent form is minimized, hidden, or dismissed.</param>
    public virtual void Show(object? tooltip, AbstractControl control, PointD? point, int? duration = null)
    {
        SetToolTip(control, tooltip);
        ToolTipWindow.ShowToolTip(control, point);
    }

    /// <summary>
    /// Associates tooltip with the specified control and displays it at the specified coordinates.
    /// </summary>
    /// <param name="tooltip">The text to display in the tooltip. If null, the tooltip will be displayed
    /// at the default location.</param>
    /// <param name="control">The control with which the tooltip is associated.</param>
    /// <param name="x">The x-coordinate at which to display the tooltip.</param>
    /// <param name="y">The y-coordinate at which to display the tooltip.</param>
    /// <param name="duration">The duration for which the tooltip is displayed (in milliseconds).
    /// If duration is not specified, the tooltip will be displayed until the Hide method is called,
    /// or until the parent form is minimized, hidden, or dismissed.</param>
    public void Show(object? tooltip, AbstractControl control, float x, float y, int? duration = null)
    {
        Show(tooltip, control, new(x, y), duration);
    }

    /// <summary>
    /// Hides tooltip shown for the the specified control.
    /// </summary>
    public virtual void Hide(AbstractControl control)
    {
        ToolTipWindow.HideGlobalToolTip();
    }

    /// <summary>
    /// Returns a string representation for the tooltip.
    /// </summary>
    public override string ToString()
    {
        var s = base.ToString();
        return $"{s} InitialDelay: {InitialDelay}, ShowAlways: {ShowAlways}";
    }

    /// <summary>
    /// Logs delay related properties to the debug output.
    /// </summary>
    public virtual void LogDelayProperties()
    {
        Debug.WriteLine($"InitialDelay: {InitialDelay}");
        Debug.WriteLine($"ReshowDelay: {ReshowDelay}");
        Debug.WriteLine($"AutoPopDelay: {AutoPopDelay}");
        Debug.WriteLine($"AutomaticDelay: {AutomaticDelay}");
    }

    /// <summary>
    /// Raises the <see cref="Draw"/> event.
    /// </summary>
    /// <param name="e"></param>
    protected virtual void RaiseDraw(DrawToolTipEventArgs e)
    {
        Draw?.Invoke(this, e);
    }

    /// <summary>
    /// Raises the <see cref="Popup"/> event.
    /// </summary>
    /// <param name="e"></param>
    protected virtual void RaisePopup(PopupToolTipEventArgs e)
    {
        Popup?.Invoke(this, e);
    }
}
