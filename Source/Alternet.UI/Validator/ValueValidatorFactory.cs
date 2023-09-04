using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Provides static methods related to control validators.
    /// </summary>
    /// <remarks>
    /// Creates <see cref="IValueValidator"/> instances.
    /// </remarks>
    public static class ValueValidatorFactory
    {
        private static IValueValidatorText? decimalValidator;

        /// <summary>
        /// Gets validator for <see cref="decimal"/> numbers;
        /// </summary>
        public static IValueValidatorText DecimalValidator
        {
            get
            {
                decimalValidator ??= CreateValueValidatorNum(ValueValidatorNumStyle.Float);
                return decimalValidator;
            }
        }

        /// <summary>
        /// Creates <see cref="IValueValidatorText"/> instance.
        /// </summary>
        /// <param name="style">Text format style.</param>
        public static IValueValidatorText CreateValueValidatorText(ValueValidatorTextStyle style)
        {
            return new ValueValidatorText(style);
        }

        /// <summary>
        /// Creates <see cref="IValueValidatorText"/> instance for the numeric properties.
        /// </summary>
        /// <param name="numericType"></param>
        /// <param name="valueBase">Value base (2, 8, 10 or 16). Optional.
        /// Default value is 10.</param>
        public static IValueValidatorText CreateValueValidatorNum(
            ValueValidatorNumStyle numericType,
            int valueBase = 10)
        {
            return new ValueValidatorNumProp(numericType, valueBase);
        }
    }
}
