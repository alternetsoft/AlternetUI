using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    internal class TextBoxInitializers
        : ControlInitializers<CustomTextBox, TextBoxInitializeEventArgs>
    {
        private static TextBoxInitializers? provider;

        public TextBoxInitializers()
        {
        }

        /// <summary>
        /// Gets or sets whether to assign and use char validator when control is initialized.
        /// Default is True.
        /// </summary>
        public static bool UseCharValidator { get; set; } = true;

        public static TextBoxInitializers Default
        {
            get
            {
                provider ??= new();
                provider.AddDefaultInitializers();
                return provider;
            }

            set
            {
                provider = value;
            }
        }

        public virtual void AddDefaultInitializers()
        {
            Add(KnownInputType.SByte, InitAsSByte);
            Add(KnownInputType.Byte, InitAsByte);
            Add(KnownInputType.Int16, InitAsInt16);
            Add(KnownInputType.UInt16, InitAsUInt16);
            Add(KnownInputType.Int32, InitAsInt32);
            Add(KnownInputType.UInt32, InitAsUInt32);
            Add(KnownInputType.Int64, InitAsInt64);
            Add(KnownInputType.UInt64, InitAsUInt64);
            Add(KnownInputType.Single, InitAsSingle);
            Add(KnownInputType.USingle, InitAsUSingle);
            Add(KnownInputType.Double, InitAsDouble);
            Add(KnownInputType.UDouble, InitAsUDouble);
            Add(KnownInputType.Decimal, InitAsDecimal);
            Add(KnownInputType.UDecimal, InitAsUDecimal);
/*
            Add(KnownInputType.String, InitAsString);
            Add(KnownInputType.EMail, InitAsEMail);
            Add(KnownInputType.Url, InitAsUrl);
*/
            Add(KnownInputType.None, InitAsNone);

            void Add(
                KnownInputType type,
                Action<CustomTextBox, TextBoxInitializeEventArgs?> action)
            {
                AddInitializer(type, action);
            }
        }

        public virtual void InitAsSByte(CustomTextBox c, TextBoxInitializeEventArgs? e)
        {
            SetNumberValidator(c, e, typeof(sbyte));
        }

        public virtual void InitAsByte(CustomTextBox c, TextBoxInitializeEventArgs? e)
        {
            SetNumberValidator(c, e, typeof(byte));
        }

        public virtual void InitAsInt16(CustomTextBox c, TextBoxInitializeEventArgs? e)
        {
            SetNumberValidator(c, e, typeof(short));
        }

        public virtual void InitAsUInt16(CustomTextBox c, TextBoxInitializeEventArgs? e)
        {
            SetNumberValidator(c, e, typeof(ushort));
        }

        public virtual void InitAsInt32(CustomTextBox c, TextBoxInitializeEventArgs? e)
        {
            SetNumberValidator(c, e, typeof(int));
        }

        public virtual void InitAsUInt32(CustomTextBox c, TextBoxInitializeEventArgs? e)
        {
            SetNumberValidator(c, e, typeof(uint));
        }

        public virtual void InitAsInt64(CustomTextBox c, TextBoxInitializeEventArgs? e)
        {
            SetNumberValidator(c, e, typeof(long));
        }

        public virtual void InitAsUInt64(CustomTextBox c, TextBoxInitializeEventArgs? e)
        {
            SetNumberValidator(c, e, typeof(ulong));
        }

        public virtual void InitAsSingle(CustomTextBox c, TextBoxInitializeEventArgs? e)
        {
            SetNumberValidator(c, e, typeof(float));
        }

        public virtual void InitAsUSingle(CustomTextBox c, TextBoxInitializeEventArgs? e)
        {
            SetNumberValidator(c, e, typeof(float));
            c.MinValue = 0F;
        }

        public virtual void InitAsDouble(CustomTextBox c, TextBoxInitializeEventArgs? e)
        {
            SetNumberValidator(c, e, typeof(double));
        }

        public virtual void InitAsUDouble(CustomTextBox c, TextBoxInitializeEventArgs? e)
        {
            SetNumberValidator(c, e, typeof(double));
            c.MinValue = 0D;
        }

        public virtual void InitAsDecimal(CustomTextBox c, TextBoxInitializeEventArgs? e)
        {
            SetNumberValidator(c, e, typeof(decimal));
        }

        public virtual void InitAsUDecimal(CustomTextBox c, TextBoxInitializeEventArgs? e)
        {
            SetNumberValidator(c, e, typeof(decimal));
            c.MinValue = 0M;
        }

        public virtual void InitAsNone(CustomTextBox c, TextBoxInitializeEventArgs? e)
        {
            Reset(c);
        }

/*
        public virtual void InitAsEMail(CustomTextBox c, TextBoxInitializeEventArgs? e)
        {
        }

        public virtual void InitAsUrl(CustomTextBox c, TextBoxInitializeEventArgs? e)
        {
        }

        public virtual void InitAsString(CustomTextBox c, TextBoxInitializeEventArgs? e)
        {
        }

        public virtual void InitAsChar(CustomTextBox c, TextBoxInitializeEventArgs? e)
        {
        }
*/

        protected virtual void Reset(CustomTextBox c)
        {
            c.ResetInputSettings();
        }

        protected virtual void SetNumberValidator(
            CustomTextBox c,
            TextBoxInitializeEventArgs? e,
            Type valueType)
        {
            Reset(c);
            c.SetValidator(valueType, e?.UseCharValidator ?? UseCharValidator);
            c.Options |= TextBoxOptions.DefaultValidation;
        }
    }
}
