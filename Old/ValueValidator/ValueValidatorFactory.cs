using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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
    internal class ValueValidatorFactory
    {
        private static bool isSilent;

        private IValueValidatorText? decimalValidator;

        static ValueValidatorFactory()
        {
            IsSilent = true;
        }

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
                return isSilent;
            }

            set
            {
                isSilent = value;
                SystemSettings.Handler.SuppressBellOnError(value);
            }
        }

        /// <summary>
        /// Gets or sets whether to play error sound. An opposite of <see cref="IsSilent"/>.
        /// </summary>
        public static bool BellOnError
        {
            get => !IsSilent;
            set => IsSilent = !value;
        }

        /// <summary>
        /// Gets validator for <see cref="decimal"/> numbers;
        /// </summary>
        /// <remarks>
        /// Do not use it. Consider using <see cref="CreateValidator(ValueValidatorKind)"/> method.
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
        /// Creates <see cref="IValueValidatorText"/> instance with
        /// the specified <see cref="TypeCode"/>.
        /// </summary>
        /// <param name="typeCode">Type code.</param>
        public static IValueValidatorText CreateValidator(TypeCode typeCode)
        {
            if (AssemblyUtils.IsTypeCodeUnsignedInt(typeCode))
                return CreateValidator(ValueValidatorKind.UnsignedInt);
            if (AssemblyUtils.IsTypeCodeSignedInt(typeCode))
                return CreateValidator(ValueValidatorKind.SignedInt);
            if (AssemblyUtils.IsTypeCodeFloat(typeCode))
                return CreateValidator(ValueValidatorKind.SignedFloat);
            return CreateValidator(ValueValidatorKind.Generic);
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
                ValueValidatorKind.SignedFloat =>
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
            return ControlFactory.Handler.CreateValueValidatorText(style);
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
            return ControlFactory.Handler.CreateValueValidatorNum(numericType, valueBase);
        }
    }
}