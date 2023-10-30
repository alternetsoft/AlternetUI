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
        private IValueValidatorText? decimalValidator;

        /// <summary>
        /// Gets or sets default implementation of the <see cref="ValueValidatorFactory"/>.
        /// </summary>
        public static ValueValidatorFactory Default { get; set; } = new();

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
        /// <remarks>
        /// Do not use it. Consider using <see cref="CreateValidator"/> method.
        /// </remarks>
        public virtual IValueValidatorText DecimalValidator
        {
            get
            {
                decimalValidator ??= CreateValueValidatorNum(ValueValidatorNumStyle.Float);
                return decimalValidator;
            }
        }

        /// <summary>
        /// Creates <see cref="IValueValidatorText"/> instance with the specified kind.
        /// </summary>
        /// <param name="kind">Kind ofthe validator.</param>
        public static IValueValidatorText CreateValidator(ValueValidatorKind kind)
        {
            return kind switch
            {
                ValueValidatorKind.SignedInt =>
                    Default.CreateValueValidatorNum(ValueValidatorNumStyle.Signed),
                ValueValidatorKind.UnsignedInt =>
                    Default.CreateValueValidatorNum(ValueValidatorNumStyle.Unsigned),
                ValueValidatorKind.Float =>
                    Default.CreateValueValidatorNum(ValueValidatorNumStyle.Float),
                ValueValidatorKind.SignedHex =>
                    Default.CreateValueValidatorNum(ValueValidatorNumStyle.Signed, 16),
                ValueValidatorKind.UnsignedHex =>
                    Default.CreateValueValidatorNum(ValueValidatorNumStyle.Unsigned, 16),
                _ => Default.CreateValueValidatorText(ValueValidatorTextStyle.None),
            };
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
        /// <param name="numericType">Number kind (signed int, unsigned int, float).</param>
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