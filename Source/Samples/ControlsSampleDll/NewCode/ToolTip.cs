using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Alternet.UI;

// https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.tooltip?view=windowsdesktop-10.0

[DefaultEvent(nameof(Popup))]
public partial class ToolTip : Component
{
    private bool active;
    private int automaticDelay;
    private int autoPopDelay;
    private Color backColor;
    private Color foreColor;
    private bool isBalloon;
    private int initialDelay;
    private bool ownerDraw;
    private int reshowDelay;
    private bool showAlways;
    private bool stripAmpersands;
    private ToolTipIcon toolTipIcon;
    private string? toolTipTitle;
    private bool useAnimation;
    private bool useFading;

    /// <summary>
    /// Initializes a new instance of the <see cref="ToolTip"/> class in its default state.
    /// </summary>
    public ToolTip()
    {
        backColor = SystemColors.Info;
        foreColor = SystemColors.InfoText;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="ToolTip"/> control is currently active.
    /// </summary>
    [DefaultValue(true)]
    public virtual bool Active
    {
        get => active;
        set => active = value;
    }

    /// <summary>
    /// Gets or sets the time (in milliseconds) that passes before the <see cref="ToolTip"/> appears.
    /// </summary>
    [RefreshProperties(RefreshProperties.All)]
    public virtual int AutomaticDelay
    {
        get => automaticDelay;
        set => automaticDelay = value;
    }

    /// <summary>
    /// Gets or sets the initial delay for the <see cref="ToolTip"/> control.
    /// </summary>
    [RefreshProperties(RefreshProperties.All)]
    public virtual int AutoPopDelay
    {
        get => autoPopDelay;
        set => autoPopDelay = value;
    }

    /// <summary>
    /// Gets or sets the BackColor for the <see cref="ToolTip"/> control.
    /// </summary>
    [DefaultValue(typeof(Color), "Info")]
    public virtual Color BackColor
    {
        get => backColor;
        set => backColor = value;
    }

    /// <summary>
    /// Gets or sets the ForeColor for the <see cref="ToolTip"/> control.
    /// </summary>
    [DefaultValue(typeof(Color), "InfoText")]
    public virtual Color ForeColor
    {
        get => foreColor;
        set => foreColor = value;
    }

    /// <summary>
    /// Gets or sets the IsBalloon for the <see cref="ToolTip"/> control.
    /// </summary>
    [DefaultValue(false)]
    public virtual bool IsBalloon
    {
        get => isBalloon;
        set => isBalloon = value;
    }

    /// <summary>
    /// Gets or sets the initial delay for the <see cref="ToolTip"/> control.
    /// </summary>
    [RefreshProperties(RefreshProperties.All)]
    public virtual int InitialDelay
    {
        get => initialDelay;
        set => initialDelay = value;
    }

    /// <summary>
    /// Indicates whether the ToolTip will be drawn by the system or the user.
    /// </summary>
    [DefaultValue(false)]
    public virtual bool OwnerDraw
    {
        get => ownerDraw;
        set => ownerDraw = value;
    }

    /// <summary>
    /// Gets or sets the length of time (in milliseconds) that it takes subsequent ToolTip
    /// instances to appear as the mouse pointer moves from one ToolTip region to another.
    /// </summary>
    [RefreshProperties(RefreshProperties.All)]
    public virtual int ReshowDelay
    {
        get => reshowDelay;
        set => reshowDelay = value;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="ToolTip"/> appears even when its
    /// parent control is not active.
    /// </summary>
    [DefaultValue(false)]
    public virtual bool ShowAlways
    {
        get => showAlways;
        set => showAlways = value;
    }

    /// <summary>
    /// When set to true, any ampersands in the Text property are not displayed.
    /// </summary>
    [Browsable(true)]
    [DefaultValue(false)]
    public virtual bool StripAmpersands
    {
        get => stripAmpersands;
        set => stripAmpersands = value;
    }

    [Localizable(false)]
    [Bindable(true)]
    [DefaultValue(null)]
    [TypeConverter(typeof(StringConverter))]
    public virtual object? Tag { get; set; }

    /// <summary>
    /// Gets or sets an Icon on the ToolTip.
    /// </summary>
    [DefaultValue(ToolTipIcon.None)]
    public virtual ToolTipIcon ToolTipIcon
    {
        get => toolTipIcon;
        set => toolTipIcon = value;
    }

    /// <summary>
    /// Gets or sets the title of the ToolTip.
    /// </summary>
    [DefaultValue(null)]
    [AllowNull]
    public virtual string? ToolTipTitle
    {
        get => toolTipTitle;
        set => toolTipTitle = value;
    }

    /// <summary>
    /// When set to true, animations are used when tooltip is shown or hidden.
    /// </summary>
    [Browsable(true)]
    [DefaultValue(true)]
    public virtual bool UseAnimation
    {
        get => useAnimation;
        set => useAnimation = value;
    }

    /// <summary>
    /// When set to true, a fade effect is used when tooltips are shown or hidden.
    /// </summary>
    [Browsable(true)]
    [DefaultValue(true)]
    public virtual bool UseFading
    {
        get => useFading;
        set => useFading = value;
    }

    /// <summary>
    /// Fires in OwnerDraw mode when the tooltip needs to be drawn.
    /// </summary>
    public event DrawToolTipEventHandler? Draw;

    /// <summary>
    /// Fires when the tooltip is just about to be shown.
    /// </summary>
    public event PopupToolTipEventHandler? Popup;

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
    public virtual void SetToolTip(AbstractControl control, string? caption)
    {
        control.ToolTip = caption;
    }

    /// <summary>
    /// Associates tooltip with the specified control and displays it.
    /// </summary>
    public virtual void Show(string? text, AbstractControl window)
    {
    }

    /// <summary>
    /// Associates tooltip with the specified control and displays it for the
    /// specified duration.
    /// </summary>
    public virtual void Show(string? text, AbstractControl window, int duration)
    {
    }

    /// <summary>
    /// Associates tooltip with the specified control and displays it.
    /// </summary>
    public virtual void Show(string? text, AbstractControl window, PointD point)
    {
    }

    /// <summary>
    /// Associates tooltip with the specified control and displays it.
    /// </summary>
    public virtual void Show(string? text, AbstractControl window, PointD point, int duration)
    {
    }

    /// <summary>
    /// Associates tooltip with the specified control and displays it.
    /// </summary>
    public virtual void Show(string? text, AbstractControl window, int x, int y)
    {
    }

    /// <summary>
    /// Associates tooltip with the specified control and displays it.
    /// </summary>
    public virtual void Show(string? text, AbstractControl window, int x, int y, int duration)
    {
    }

    /// <summary>
    /// Hides <see cref="ToolTip"/> with the specified control.
    /// </summary>
    public virtual void Hide(AbstractControl win)
    {
    }

    /// <summary>
    /// Returns a string representation for the tooltip.
    /// </summary>
    public override string ToString()
    {
        string s = base.ToString();
        return $"{s} InitialDelay: {InitialDelay}, ShowAlways: {ShowAlways}";
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
