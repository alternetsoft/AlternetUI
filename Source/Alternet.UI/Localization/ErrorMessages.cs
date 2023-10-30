namespace Alternet.UI.Localization
{
    /// <summary>
    /// Defines localizations for error messages.
    /// </summary>
    public class ErrorMessages
    {
        /// <summary>
        /// Current localizations for error messages.
        /// </summary>
        public static ErrorMessages Default { get; set; } = new();

        /// <summary>
        /// Gets or sets error message localization.
        /// </summary>
        public string CurrentApplicationIsNotSet { get; set; } =
            "Current application instance is not set.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string CannotDisposeImmutableObject { get; set; } =
            "Cannot dispose an immutable object.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string CannotChangeReadOnlyObject { get; set; } =
            "Cannot change a readonly object.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string OnlySolidBrushInstancesSupported { get; set; } =
            "Only SolidBrush instances are supported.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string CannotChangeImmutableObject { get; set; } =
            "Cannot change an immutable object.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string InvalidParameter { get; set; } = "Invalid Parameter.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string ParameterIsAlreadySet { get; set; } = "Parameter is already set.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string EventHasAlreadyBeenHandled { get; set; } =
            "Event has already been handled.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string PropertyIsNull { get; set; } = "Property {0} is null.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string PropertyCannotBeNull { get; set; } = "Property {0} value cannot be null.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string InvalidStringFormat { get; set; } = "String format is not valid.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string ValidationNumberIsExpected { get; set; } = "A number is expected.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string ValidationFloatIsExpected { get; set; } = "A floating point number is expected.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string ValidationHexNumberIsExpected { get; set; } = "A hexadecimal number is expected.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string ValidationRangeFormat { get; set; } = "Range is [{0}..{1}].";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string ValidationInvalidFormat { get; set; } = "Invalid format.";
    }
}