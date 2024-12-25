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
            Add(KnownInputType.String, InitAsString);
            Add(KnownInputType.Boolean, InitAsBoolean);
            Add(KnownInputType.Char, InitAsChar);
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
            Add(KnownInputType.DateTime, InitAsDateTime);
            Add(KnownInputType.Date, InitAsDate);
            Add(KnownInputType.Time, InitAsTime);
            Add(KnownInputType.EMail, InitAsEMail);
            Add(KnownInputType.Url, InitAsUrl);
            Add(KnownInputType.None, InitAsNone);

            void Add(
                KnownInputType type,
                Action<CustomTextBox, TextBoxInitializeEventArgs?> action)
            {
                AddInitializer(type, action);
            }
        }

        public virtual void InitAsNone(CustomTextBox c, TextBoxInitializeEventArgs? e)
        {
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
