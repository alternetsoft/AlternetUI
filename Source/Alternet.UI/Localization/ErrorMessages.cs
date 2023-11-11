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
        public string ErrorTitle { get; set; } = "Error";

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
        public string ValidationNumberIsExpected { get; set; } = "Number is expected.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string ValidationUnsignedNumberIsExpected { get; set; } = "Unsigned number is expected.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string ValidationUnsignedFloatIsExpected { get; set; } = "Unsigned floating point number is expected.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string ValidationFloatIsExpected { get; set; } = "Floating point number is expected.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string ValidationHexNumberIsExpected { get; set; } = "Hexadecimal number is expected.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string ValidationRangeFormat { get; set; } = "Range is [{0}..{1}].";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string ValidationInvalidFormat { get; set; } = "Invalid format.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string ValidationMinimumLength { get; set; } = "Minimum length is {0} characters.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string ValidationMaximumLength { get; set; } = "Maximum length is {0} characters.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string ValidationMinMaxLength { get; set; } = "Valid length is {0} to {1} characters.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string ValidationMinimumValue { get; set; } = "Minimum value is {0}.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string ValidationMaximumValue { get; set; } = "Maximum value is {0}.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string ValidationValueIsRequired { get; set; } = "Value is required.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string ValidationEMailIsExpected { get; set; } = "E-mail is expected.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string ValidationUrlIsExpected { get; set; } = "Url is expected.";
    }
}