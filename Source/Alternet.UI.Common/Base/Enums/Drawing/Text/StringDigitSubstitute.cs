namespace Alternet.Drawing;

/// <summary>
/// Specifies the digit substitution method for a particular language.
/// </summary>
public enum StringDigitSubstitute
{
    /// <summary>
    /// The digit substitution method is determined by the user locale settings.
    /// </summary>
    User = 0,

    /// <summary>
    /// The digit substitution method is determined by the language specified in the <see cref='StringFormat'/> instance.
    /// </summary>
    None = 1,

    /// <summary>
    /// The digit substitution method is determined by the national language settings.
    /// </summary>
    National = 2,

    /// <summary>
    /// The digit substitution method is determined by the traditional digit shapes for the specified language.
    /// </summary>
    Traditional = 3,
}