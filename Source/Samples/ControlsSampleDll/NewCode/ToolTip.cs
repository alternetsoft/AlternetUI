using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Alternet.UI;

// https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.tooltip?view=windowsdesktop-10.0

[DefaultEvent(nameof(Popup))]
public partial class ToolTip : Component
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ToolTip"/> class in its default state.
    /// </summary>
    public ToolTip()
    {
        BackColor = SystemColors.Info;
        ForeColor = SystemColors.InfoText;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="ToolTip"/> control is currently active.
    /// </summary>
    [DefaultValue(true)]
    public virtual bool Active
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the time (in milliseconds) that passes before the <see cref="ToolTip"/> appears.
    /// </summary>
    [RefreshProperties(RefreshProperties.All)]
    public virtual int AutomaticDelay
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the initial delay for the <see cref="ToolTip"/> control.
    /// </summary>
    [RefreshProperties(RefreshProperties.All)]
    public virtual int AutoPopDelay
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the BackColor for the <see cref="ToolTip"/> control.
    /// </summary>
    [DefaultValue(typeof(Color), "Info")]
    public virtual Color BackColor
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the ForeColor for the <see cref="ToolTip"/> control.
    /// </summary>
    [DefaultValue(typeof(Color), "InfoText")]
    public virtual Color ForeColor
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the IsBalloon for the <see cref="ToolTip"/> control.
    /// </summary>
    [DefaultValue(false)]
    public virtual bool IsBalloon
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the initial delay for the <see cref="ToolTip"/> control.
    /// </summary>
    [RefreshProperties(RefreshProperties.All)]
    public virtual int InitialDelay
    {
        get;
        set;
    }

    /// <summary>
    /// Indicates whether the ToolTip will be drawn by the system or the user.
    /// </summary>
    [DefaultValue(false)]
    public virtual bool OwnerDraw { get; set; }

    /// <summary>
    /// Gets or sets the length of time (in milliseconds) that it takes subsequent ToolTip
    /// instances to appear as the mouse pointer moves from one ToolTip region to another.
    /// </summary>
    [RefreshProperties(RefreshProperties.All)]
    public virtual int ReshowDelay
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="ToolTip"/> appears even when its
    /// parent control is not active.
    /// </summary>
    [DefaultValue(false)]
    public virtual bool ShowAlways
    {
        get;
        set;
    }

    /// <summary>
    /// When set to true, any ampersands in the Text property are not displayed.
    /// </summary>
    [Browsable(true)]
    [DefaultValue(false)]
    public virtual bool StripAmpersands
    {
        get;
        set;
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
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the title of the ToolTip.
    /// </summary>
    [DefaultValue(null)]
    [AllowNull]
    public virtual string? ToolTipTitle
    {
        get;
        set;
    }

    /// <summary>
    /// When set to true, animations are used when tooltip is shown or hidden.
    /// </summary>
    [Browsable(true)]
    [DefaultValue(true)]
    public virtual bool UseAnimation { get; set; }

    /// <summary>
    /// When set to true, a fade effect is used when tooltips are shown or hidden.
    /// </summary>
    [Browsable(true)]
    [DefaultValue(true)]
    public virtual bool UseFading { get; set; }

    /// <summary>
    /// Fires in OwnerDraw mode when the tooltip needs to be drawn.
    /// </summary>
    public event DrawToolTipEventHandler? Draw;

    /// <summary>
    /// Fires when the tooltip is just about to be shown.
    /// </summary>
    public event PopupEventHandler? Popup;

    /// <summary>
    /// Returns true if the tooltip can offer an extender property to the specified target component.
    /// </summary>
    public bool CanExtend(object target) => target is Control;

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
}
