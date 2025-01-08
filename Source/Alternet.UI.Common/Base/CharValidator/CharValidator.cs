using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace Alternet.UI
{
    /*

    NumberFormatInfo
    https://learn.microsoft.com/en-us/dotnet/api/system.globalization.numberformatinfo?view=net-8.0

    Decimal.Parse
    https://learn.microsoft.com/en-us/dotnet/api/system.decimal.parse?view=net-8.0

    Double Parse
    https://learn.microsoft.com/en-us/dotnet/api/system.double.parse?view=net-8.0

    */

    /// <summary>
    /// Default implementation of the <see cref="ICharValidator"/> interface.
    /// </summary>
    public class CharValidator : AbstractCharValidator
    {
        /// <summary>
        /// Gets or sets array of characters that are always valid
        /// and allowed in the input. By default it equals [127] (Delete key is allowed).
        /// </summary>
        public static char[] AlwaysValidChars = { (char)127 };

        /// <summary>
        /// Gets or set whether control characters (chars with codes 0..31) are
        /// always valid and allowed in the input. Default is True.
        /// </summary>
        public static bool AlwaysValidControlChars = true;

        /// <summary>
        /// Gets or sets override which is used instead of
        /// <see cref="CultureInfo.CurrentCulture"/> when char validators are created.
        /// </summary>
        public static CultureInfo? CultureInfoOverride;

        /// <summary>
        /// Gets or sets whether plus character is allowed in signed numeric validators.
        /// </summary>
        public static bool AllowPlusInSignedNumbers = true;

        /// <summary>
        /// Gets or sets whether to allow native digits in the numeric validators.
        /// </summary>
        public static bool AllowNativeDigitsInNumbers = true;

        private static ICustomCharValidator? allowAllChars;
        private static ICustomCharValidator? digitsValidator;
        private static ICustomCharValidator? digitsAndNegativeSignValidator;
        private static ICustomCharValidator? digitsAndSignValidator;
        private static ICustomCharValidator? unsignedHexValidator;
        private static ICustomCharValidator? signedFloatValidator;
        private static ICustomCharValidator? unsignedFloatValidator;

        private BitArray? charInfo;
        private BitArray64 catInfo = new(true);

        /// <summary>
        /// Occurs when <see cref="CreateValidator(Type)"/> is called.
        /// </summary>
        public static event EventHandler<HandledEventArgsWithResult<Type, ICustomCharValidator>>?
            QueryCharValidatorForType;

        /// <summary>
        /// Gets <see cref="ICustomCharValidator"/> instance which allows any input.
        /// </summary>
        public static ICustomCharValidator Default
        {
            get
            {
                if (allowAllChars is null)
                {
                    var validator = new CharValidator();
                    validator.SetImmutable();
                    allowAllChars = validator;
                }

                return allowAllChars;
            }
        }

        /// <summary>
        /// Gets or sets <see cref="ICustomCharValidator"/> instance
        /// which allows only hex digits.
        /// </summary>
        public static ICustomCharValidator UnsignedHexValidator
        {
            get
            {
                if (unsignedHexValidator is null)
                {
                    var validator = new CharValidator();
                    validator.AllCharsBad().AllowHexDigits();
                    validator.SetImmutable();
                    unsignedHexValidator = validator;
                }

                return unsignedHexValidator;
            }

            set
            {
                if (value is null)
                    return;
                unsignedHexValidator = value;
            }
        }

        /// <summary>
        /// Gets used <see cref="CultureInfo"/>.
        /// </summary>
        public static CultureInfo Culture
        {
            get
            {
                return CultureInfoOverride ?? CultureInfo.CurrentCulture;
            }
        }

        /// <summary>
        /// Gets negative sign from the <see cref="Culture"/> settings.
        /// </summary>
        public static string NegativeSign
        {
            get
            {
                return Culture.NumberFormat.NegativeSign;
            }
        }

        /// <summary>
        /// Gets positive sign from the <see cref="Culture"/> settings.
        /// </summary>
        public static string PositiveSign
        {
            get
            {
                return Culture.NumberFormat.PositiveSign;
            }
        }

        /// <summary>
        /// Gets native digits from the <see cref="Culture"/> settings.
        /// </summary>
        public static string[] NativeDigits
        {
            get
            {
                return Culture.NumberFormat.NativeDigits;
            }
        }

        /// <summary>
        /// Gets number decimal separator from the <see cref="Culture"/> settings.
        /// </summary>
        public static string NumberDecimalSeparator
        {
            get
            {
                return Culture.NumberFormat.NumberDecimalSeparator;
            }
        }

        /// <summary>
        /// Gets number group separator from the <see cref="Culture"/> settings.
        /// </summary>
        public static string NumberGroupSeparator
        {
            get
            {
                return Culture.NumberFormat.NumberGroupSeparator;
            }
        }

        /// <summary>
        /// Gets or sets <see cref="ICustomCharValidator"/> instance
        /// for the signed float validation.
        /// </summary>
        public static ICustomCharValidator SignedFloatValidator
        {
            get
            {
                if (signedFloatValidator is null)
                {
                    var validator = new CharValidator();
                    InitUnsignedFloat(validator);
                    validator.AllowPositiveSign().AllowNegativeSign().SetImmutable();
                    signedFloatValidator = validator;
                }

                return signedFloatValidator;
            }

            set
            {
                if (value is null)
                    return;
                signedFloatValidator = value;
            }
        }

        /// <summary>
        /// Gets or sets <see cref="ICustomCharValidator"/> instance
        /// for the unsigned float validation.
        /// </summary>
        public static ICustomCharValidator UnsignedFloatValidator
        {
            get
            {
                if (unsignedFloatValidator is null)
                {
                    var validator = new CharValidator();
                    InitUnsignedFloat(validator);
                    validator.SetImmutable();
                    unsignedFloatValidator = validator;
                }

                return unsignedFloatValidator;
            }

            set
            {
                if (value is null)
                    return;
                unsignedFloatValidator = value;
            }
        }

        /// <summary>
        /// Gets or sets <see cref="ICustomCharValidator"/> instance
        /// which allows only digits and minus sign.
        /// </summary>
        public static ICustomCharValidator DigitsAndNegativeSignValidator
        {
            get
            {
                if (digitsAndNegativeSignValidator is null)
                {
                    var validator = new CharValidator();
                    validator.AllCharsBad().AllowDigits().AllowNegativeSign();
                    if (AllowNativeDigitsInNumbers)
                        validator.AllowNativeDigits();
                    validator.SetImmutable();
                    digitsAndNegativeSignValidator = validator;
                }

                return digitsAndNegativeSignValidator;
            }

            set
            {
                if (value is null)
                    return;
                digitsAndNegativeSignValidator = value;
            }
        }

        /// <summary>
        /// Gets or sets <see cref="ICustomCharValidator"/> instance
        /// which allows only digits, plus and minus sign.
        /// </summary>
        public static ICustomCharValidator DigitsAndSignValidator
        {
            get
            {
                if (digitsAndSignValidator is null)
                {
                    var validator = new CharValidator();
                    validator.AllCharsBad().AllowDigits().AllowPositiveSign()
                        .AllowNegativeSign().SetImmutable();
                    if (AllowNativeDigitsInNumbers)
                        validator.AllowNativeDigits();
                    digitsAndSignValidator = validator;
                }

                return digitsAndSignValidator;
            }

            set
            {
                if (value is null)
                    return;
                digitsAndSignValidator = value;
            }
        }

        /// <summary>
        /// Gets or sets <see cref="ICustomCharValidator"/> instance
        /// which allows only digits.
        /// </summary>
        public static ICustomCharValidator DigitsValidator
        {
            get
            {
                if (digitsValidator is null)
                {
                    var validator = new CharValidator();
                    validator.AllCharsBad().AllowDigits();
                    validator.SetImmutable();
                    digitsValidator = validator;
                }

                return digitsValidator;
            }

            set
            {
                if (value is null)
                    return;
                digitsValidator = value;
            }
        }

        internal BitArray CharInfo
        {
            get
            {
                return charInfo ??= new(char.MaxValue + 1);
            }
        }

        /// <summary>
        /// Creates <see cref="ICustomCharValidator"/> instance for the specified type code.
        /// Only numeric type codes are supported.
        /// </summary>
        /// <param name="typeCode">Type code.</param>
        public static ICustomCharValidator? CreateValidator(TypeCode typeCode)
        {
            switch (typeCode)
            {
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Byte:
                    return DigitsValidator;
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                    if(AllowPlusInSignedNumbers)
                        return DigitsAndSignValidator;
                    else
                        return DigitsAndNegativeSignValidator;
                case TypeCode.Double:
                case TypeCode.Single:
                    return SignedFloatValidator;
                case TypeCode.Decimal:
                default:
                    return null;
            }
        }

        /// <summary>
        /// Creates <see cref="ICustomCharValidator"/> instance for the specified type.
        /// Only numeric types are supported. Returns Null if type is Null.
        /// </summary>
        /// <param name="type">Type.</param>
        public static ICustomCharValidator? CreateValidator(Type? type)
        {
            if (type is null)
                return null;

            if(QueryCharValidatorForType is not null)
            {
                HandledEventArgsWithResult<Type, ICustomCharValidator?> e = new(type);
                e.Value = type;
                if (e.Handled)
                {
                    return e.Result;
                }
            }

            var typeCode = AssemblyUtils.GetRealTypeCode(type);
            return CreateValidator(typeCode);
        }

        /// <inheritdoc/>
        public override ICharValidator AllCategoriesValid(bool valid = true)
        {
            if (Immutable)
                return this;
            catInfo.SetAllBits(valid);
            return this;
        }

        /// <inheritdoc/>
        public override ICharValidator Reset()
        {
            if (Immutable)
                return this;
            charInfo = null;
            catInfo.SetAllBits();
            return this;
        }

        /// <inheritdoc/>
        public override ICharValidator ValidCategory(UnicodeCategory cat, bool isValid = true)
        {
            if (Immutable)
                return this;
            catInfo[(int)cat] = isValid;
            return this;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool IsValidCategory(UnicodeCategory cat)
        {
            var result = catInfo[(int)cat];
            return result;
        }

        /// <inheritdoc/>
        public override bool IsValidChar(char ch)
        {
            if (charInfo is null)
                return true;
            return charInfo[ch];
        }

        /// <inheritdoc/>
        public override ICharValidator ValidChar(char ch, bool isValid = true)
        {
            if (Immutable)
                return this;
            charInfo?.Set(ch, isValid);
            return this;
        }

        /// <inheritdoc/>
        public override ICharValidator AllCharsValid(bool valid = true)
        {
            if (Immutable)
                return this;
            if (valid)
            {
                charInfo = null;
            }
            else
            {
                CharInfo.SetAll(false);
            }

            return this;
        }

        internal static void InitUnsignedFloat(ICharValidator validator)
        {
            validator.AllCharsBad().ValidChars('e', 'E').ValidChars('.', ',')
                .ValidChars(NumberDecimalSeparator).ValidChars(NumberGroupSeparator);
            if (AllowNativeDigitsInNumbers)
                validator.AllowNativeDigits();
            validator.AllowDigits();
        }
    }
}