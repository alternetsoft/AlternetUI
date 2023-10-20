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
    public class ValueValidatorFactory
    {
        /// <summary>
        /// Gets or sets default implementation of the <see cref="ValueValidatorFactory"/>.
        /// </summary>
        public static ValueValidatorFactory Default { get; set; } = new();

        private IValueValidatorText? decimalValidator;

        /// <summary>
        /// Gets or sets whether error sound produced by the validators
        /// if an invalid key is pressed is currently disabled.
        /// </summary>
        /// <remarks>
        /// If <c>true</c>, error sound is not played when a validator
        /// detects an error. If <c>false</c>, error sound is enabled.
        /// </remarks>
        public static bool IsSilent
        {
            get
            {
                return Native.Validator.IsSilent();
            }

            set
            {
                Native.Validator.SuppressBellOnError(value);
            }
        }

        /// <summary>
        /// Gets validator for <see cref="decimal"/> numbers;
        /// </summary>
        public virtual IValueValidatorText DecimalValidator
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
        public virtual IValueValidatorText CreateValueValidatorText(ValueValidatorTextStyle style)
        {
            return new ValueValidatorText(style);
        }

        /// <summary>
        /// Creates <see cref="IValueValidatorText"/> instance for the numeric properties.
        /// </summary>
        /// <param name="numericType"></param>
        /// <param name="valueBase">Value base (2, 8, 10 or 16). Optional.
        /// Default value is 10.</param>
        public virtual IValueValidatorText CreateValueValidatorNum(
            ValueValidatorNumStyle numericType,
            int valueBase = 10)
        {
            return new ValueValidatorNumProp(numericType, valueBase);
        }
    }
}