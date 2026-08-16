namespace Alternet.Drawing;

/// <summary>
/// Specifies the type of display for hotkey prefixes in the text.
/// </summary>
public enum HotkeyPrefix
{
    /// <summary>
    /// No hotkey prefix is displayed. No underline is applied.
    /// The text is drawn literally, without interpreting &amp; as a hotkey marker.
    /// Example: "E&amp;xit" will render as E&amp;xit (ampersand is visible).
    /// </summary>
    None = 0,

    /// <summary>
    /// Displays the hotkey prefix.
    /// The &amp; is interpreted as a hotkey prefix.
    /// Example: "E&amp;xit" will render as Exit with the x underlined.
    /// This is the standard behavior for menu items and buttons.
    /// </summary>
    Show = 1,

    /// <summary>
    /// Do not display the hotkey prefix.
    /// The &amp; is interpreted as a hotkey marker, but the underline is suppressed.
    /// Example: "E&amp;xit" will render as Exit with no underline.
    /// Useful if you want to keep the hotkey functionality but not visually show the underline.
    /// </summary>
    Hide = 2,
}


