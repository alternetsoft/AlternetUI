
using Alternet.UI;

namespace Alternet.Drawing;

/// <summary>
/// Provides access to system fonts.
/// </summary>
public static class SystemFonts
{
    private static Font? captionFont;
    private static Font? smcaptionFont;
    private static Font? menuFont;
    private static Font? statusFont;
    private static Font? messageBoxFont;
    private static Font? iconTitleFont;
    private static Font? dialogFont;

    /// <summary>
    /// Gets or sets the font used for window captions.
    /// </summary>
    public static Font CaptionFont
    {
        get
        {
            return captionFont ?? Control.DefaultFont;
        }

        set
        {
            captionFont = value;
        }
    }

    /// <summary>
    /// Gets or sets the font used for small window captions.
    /// </summary>
    public static Font SmallCaptionFont
    {
        get
        {
            return smcaptionFont ?? Control.DefaultFont;
        }

        set
        {
            smcaptionFont = value;
        }
    }

    /// <summary>
    /// Gets or sets the font used for menus.
    /// </summary>
    public static Font MenuFont
    {
        get
        {
            return menuFont ?? Control.DefaultFont;
        }

        set
        {
            menuFont = value;
        }
    }

    /// <summary>
    /// Gets or sets the font used for status bars.
    /// </summary>
    public static Font StatusFont
    {
        get
        {
            return statusFont ?? Control.DefaultFont;
        }

        set
        {
            statusFont = value;
        }
    }

    /// <summary>
    /// Gets or sets the font used for message boxes.
    /// </summary>
    public static Font MessageBoxFont
    {
        get
        {
            return messageBoxFont ?? Control.DefaultFont;
        }

        set
        {
            messageBoxFont = value;
        }
    }

    /// <summary>
    /// Gets or sets the font used for icon titles.
    /// </summary>
    public static Font IconTitleFont
    {
        get
        {

            return iconTitleFont ?? Control.DefaultFont;
        }

        set
        {
            iconTitleFont = value;
        }
    }

    /// <summary>
    /// Gets the default font used for controls.
    /// </summary>
    public static Font DefaultFont
    {
        get
        {
            return Control.DefaultFont;
        }
    }

    /// <summary>
    /// Gets or sets the font used for dialog boxes.
    /// </summary>
    public static Font DialogFont
    {
        get
        {
            return dialogFont ?? Control.DefaultFont;
        }

        set
        {
            dialogFont = value;
        }
    }
}