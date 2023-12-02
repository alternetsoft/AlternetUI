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
        public static ErrorMessages Default = new();

        /// <summary>
        /// Gets or sets error message localization.
        /// </summary>
        public string CurrentApplicationIsNotSet =
            "Current application instance is not set.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string ErrorTitle = "Error";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string CannotDisposeImmutableObject =
            "Cannot dispose an immutable object.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string CannotChangeReadOnlyObject =
            "Cannot change a readonly object.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string OnlySolidBrushInstancesSupported =
            "Only SolidBrush instances are supported.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string CannotChangeImmutableObject =
            "Cannot change an immutable object.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string InvalidParameter = "Invalid Parameter.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string ParameterIsAlreadySet = "Parameter is already set.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string EventHasAlreadyBeenHandled =
            "Event has already been handled.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string PropertyIsNull = "Property {0} is null.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string PropertyCannotBeNull = "Property {0} value cannot be null.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string InvalidStringFormat = "String format is not valid.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string ValidationNumberIsExpected = "Number is expected.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string ValidationUnsignedNumberIsExpected = "Unsigned number is expected.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string ValidationUnsignedFloatIsExpected = "Unsigned floating point number is expected.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string ValidationFloatIsExpected = "Floating point number is expected.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string ValidationHexNumberIsExpected = "Hexadecimal number is expected.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string ValidationRangeFormat = "Range is [{0}..{1}].";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string ValidationInvalidFormat = "Invalid format.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string ValidationMinimumLength = "Minimum length is {0} characters.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string ValidationMaximumLength = "Maximum length is {0} characters.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string ValidationMinMaxLength = "Valid length is {0} to {1} characters.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string ValidationMinimumValue = "Minimum value is {0}.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string ValidationMaximumValue = "Maximum value is {0}.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string ValidationValueIsRequired = "Value is required.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string ValidationEMailIsExpected = "E-mail is expected.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string ValidationUrlIsExpected = "Url is expected.";

        /// <see cref="CurrentApplicationIsNotSet"/>
        public string InvalidBoundArgument
            = "Value of '{1}' is not valid for '{0}'. '{0}' should be greater than {2} and less than or equal to {3}.";
    }
}