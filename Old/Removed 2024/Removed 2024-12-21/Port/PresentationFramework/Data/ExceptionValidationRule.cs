#pragma warning disable
#nullable disable
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


//
// Description:
//      ExceptionValidationRule is used when a ValidationError is the result of an Exception as
//      there is no actual ValidationRule.
//
//
// See specs at Validation.mht
//

using System.Globalization;

namespace Alternet.UI.Port
{
    /// <summary>
    ///     ExceptionValidationRule can be added to the ValidationRulesCollection of a Binding
    ///     or MultiBinding to indicate that Exceptions that occur during UpdateSource should
    ///     be considered ValidationErrors
    /// </summary>
    internal sealed class ExceptionValidationRule : ValidationRule
    {
        /// <summary>
        /// ExceptionValidationRule ctor.
        /// </summary>
        public ExceptionValidationRule()
        {
        }

        /// <summary>
        /// Validate is called when Data binding is updating
        /// </summary>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return ValidationResult.ValidResult;
        }

        internal static readonly ExceptionValidationRule Instance = new ExceptionValidationRule();
    }
}

