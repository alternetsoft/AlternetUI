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
            Add(KnownTextValueType.String, InitAsString);
            Add(KnownTextValueType.Boolean, InitAsBoolean);
            Add(KnownTextValueType.Char, InitAsChar);
            Add(KnownTextValueType.SByte, InitAsSByte);
            Add(KnownTextValueType.Byte, InitAsByte);
            Add(KnownTextValueType.Int16, InitAsInt16);
            Add(KnownTextValueType.UInt16, InitAsUInt16);
            Add(KnownTextValueType.Int32, InitAsInt32);
            Add(KnownTextValueType.UInt32, InitAsUInt32);
            Add(KnownTextValueType.Int64, InitAsInt64);
            Add(KnownTextValueType.UInt64, InitAsUInt64);
            Add(KnownTextValueType.Single, InitAsSingle);
            Add(KnownTextValueType.USingle, InitAsUSingle);
            Add(KnownTextValueType.Double, InitAsDouble);
            Add(KnownTextValueType.UDouble, InitAsUDouble);
            Add(KnownTextValueType.Decimal, InitAsDecimal);
            Add(KnownTextValueType.UDecimal, InitAsUDecimal);
            Add(KnownTextValueType.DateTime, InitAsDateTime);
            Add(KnownTextValueType.Date, InitAsDate);
            Add(KnownTextValueType.Time, InitAsTime);
            Add(KnownTextValueType.EMail, InitAsEMail);
            Add(KnownTextValueType.Url, InitAsUrl);

            void Add(
                KnownTextValueType type,
                Action<CustomTextBox, TextBoxInitializeEventArgs?> action)
            {
                AddInitializer(type, action);
            }
        }

        public virtual void InitAsString(CustomTextBox c, TextBoxInitializeEventArgs? e)
        {
        }

        public virtual void InitAsBoolean(CustomTextBox c, TextBoxInitializeEventArgs? e)
        {
        }

        public virtual void InitAsChar(CustomTextBox c, TextBoxInitializeEventArgs? e)
        {
        }

        public virtual void InitAsSByte(CustomTextBox c, TextBoxInitializeEventArgs? e)
        {
        }

        public virtual void InitAsByte(CustomTextBox c, TextBoxInitializeEventArgs? e)
        {
        }

        public virtual void InitAsInt16(CustomTextBox c, TextBoxInitializeEventArgs? e)
        {
        }

        public virtual void InitAsUInt16(CustomTextBox c, TextBoxInitializeEventArgs? e)
        {
        }

        public virtual void InitAsInt32(CustomTextBox c, TextBoxInitializeEventArgs? e)
        {
        }

        public virtual void InitAsUInt32(CustomTextBox c, TextBoxInitializeEventArgs? e)
        {
        }

        public virtual void InitAsInt64(CustomTextBox c, TextBoxInitializeEventArgs? e)
        {
        }

        public virtual void InitAsUInt64(CustomTextBox c, TextBoxInitializeEventArgs? e)
        {
        }

        public virtual void InitAsSingle(CustomTextBox c, TextBoxInitializeEventArgs? e)
        {
        }

        public virtual void InitAsUSingle(CustomTextBox c, TextBoxInitializeEventArgs? e)
        {
        }

        public virtual void InitAsDouble(CustomTextBox c, TextBoxInitializeEventArgs? e)
        {
        }

        public virtual void InitAsUDouble(CustomTextBox c, TextBoxInitializeEventArgs? e)
        {
        }

        public virtual void InitAsDecimal(CustomTextBox c, TextBoxInitializeEventArgs? e)
        {
        }

        public virtual void InitAsUDecimal(CustomTextBox c, TextBoxInitializeEventArgs? e)
        {
        }

        public virtual void InitAsDateTime(CustomTextBox c, TextBoxInitializeEventArgs? e)
        {
        }

        public virtual void InitAsDate(CustomTextBox c, TextBoxInitializeEventArgs? e)
        {
        }

        public virtual void InitAsTime(CustomTextBox c, TextBoxInitializeEventArgs? e)
        {
        }

        public virtual void InitAsEMail(CustomTextBox c, TextBoxInitializeEventArgs? e)
        {
        }

        public virtual void InitAsUrl(CustomTextBox c, TextBoxInitializeEventArgs? e)
        {
        }
    }
}
