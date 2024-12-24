using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties related to the initialization
    /// of the <see cref="TextBox"/> and other controls with defaults which are best
    /// for editing different value types.
    /// </summary>
    internal static class TextBoxInitializers
    {
        private static EnumArray<KnownTextValueType, Item?> register;
        private static bool initialized;

        static TextBoxInitializers()
        {
        }

        /// <summary>
        /// Initializes control properties
        /// with defaults which are best for editing the specified
        /// value type.
        /// </summary>
        /// <param name="control">Control to initialize.</param>
        /// <param name="type">Type of the input value.</param>
        public static void Initialize(AbstractControl control, KnownTextValueType type)
        {
            var initializer = GetInitializer(type);
            initializer(control);
        }

        /// <summary>
        /// Gets an action which can initialize control for editing the specified
        /// value type. Result is not null.
        /// </summary>
        /// <param name="type">Type of the input value.</param>
        /// <returns></returns>
        public static Action<AbstractControl> GetInitializer(KnownTextValueType type)
        {
            var item = GetItem(type);
            return item.RaiseInitializer;
        }

        /// <summary>
        /// Removes initialization action for the specified value type.
        /// </summary>
        /// <param name="type">Type of the input value.</param>
        /// <param name="action">Action which is called when control is initialized
        /// for editing the specified value type.</param>
        /// <returns></returns>
        public static void RemoveInitializer(KnownTextValueType type, Action<AbstractControl> action)
        {
            var item = GetItem(type);
            item.RemoveInitializer(action);
        }

        /// <summary>
        /// Adds action used when control is initialized
        /// with defaults which are best for editing the specified
        /// value type.
        /// </summary>
        /// <param name="type">Type of the input value.</param>
        /// <param name="action">Action to call when control is initialized for editing
        /// the specified value type.</param>
        public static void AddInitializer(KnownTextValueType type, Action<AbstractControl> action)
        {
            var item = GetItem(type);
            item.AddInitializer(action);
        }

        private static Item GetItem(KnownTextValueType type)
        {
            Require();
            var item = register[type];
            if (item is null)
            {
                item = new Item();
                register[type] = item;
            }

            return item;
        }

        private static void Require()
        {
            if (initialized)
                return;
            initialized = true;
            register = new();

            AddInitializer(KnownTextValueType.String, InitAsString);
            AddInitializer(KnownTextValueType.Boolean, InitAsBoolean);
            AddInitializer(KnownTextValueType.Char, InitAsChar);
            AddInitializer(KnownTextValueType.SByte, InitAsSByte);
            AddInitializer(KnownTextValueType.Byte, InitAsByte);
            AddInitializer(KnownTextValueType.Int16, InitAsInt16);
            AddInitializer(KnownTextValueType.UInt16, InitAsUInt16);
            AddInitializer(KnownTextValueType.Int32, InitAsInt32);
            AddInitializer(KnownTextValueType.UInt32, InitAsUInt32);
            AddInitializer(KnownTextValueType.Int64, InitAsInt64);
            AddInitializer(KnownTextValueType.UInt64, InitAsUInt64);
            AddInitializer(KnownTextValueType.Single, InitAsSingle);
            AddInitializer(KnownTextValueType.USingle, InitAsUSingle);
            AddInitializer(KnownTextValueType.Double, InitAsDouble);
            AddInitializer(KnownTextValueType.UDouble, InitAsUDouble);
            AddInitializer(KnownTextValueType.Decimal, InitAsDecimal);
            AddInitializer(KnownTextValueType.UDecimal, InitAsUDecimal);
            AddInitializer(KnownTextValueType.DateTime, InitAsDateTime);
            AddInitializer(KnownTextValueType.Date, InitAsDate);
            AddInitializer(KnownTextValueType.Time, InitAsTime);
            AddInitializer(KnownTextValueType.EMail, InitAsEMail);
            AddInitializer(KnownTextValueType.Url, InitAsUrl);
        }

        private static void InitAsString(AbstractControl c)
        {
        }

        private static void InitAsBoolean(AbstractControl c)
        {
        }

        private static void InitAsChar(AbstractControl c)
        {
        }

        private static void InitAsSByte(AbstractControl c)
        {
        }

        private static void InitAsByte(AbstractControl c)
        {
        }

        private static void InitAsInt16(AbstractControl c)
        {
        }

        private static void InitAsUInt16(AbstractControl c)
        {
        }

        private static void InitAsInt32(AbstractControl c)
        {
        }

        private static void InitAsUInt32(AbstractControl c)
        {
        }

        private static void InitAsInt64(AbstractControl c)
        {
        }

        private static void InitAsUInt64(AbstractControl c)
        {
        }

        private static void InitAsSingle(AbstractControl c)
        {
        }

        private static void InitAsUSingle(AbstractControl c)
        {
        }

        private static void InitAsDouble(AbstractControl c)
        {
        }

        private static void InitAsUDouble(AbstractControl c)
        {
        }

        private static void InitAsDecimal(AbstractControl c)
        {
        }

        private static void InitAsUDecimal(AbstractControl c)
        {
        }

        private static void InitAsDateTime(AbstractControl c)
        {
        }

        private static void InitAsDate(AbstractControl c)
        {
        }

        private static void InitAsTime(AbstractControl c)
        {
        }

        private static void InitAsEMail(AbstractControl c)
        {
        }

        private static void InitAsUrl(AbstractControl c)
        {
        }

        private class Item
        {
            public event Action<AbstractControl>? Initializer;

            public void RaiseInitializer(AbstractControl control)
            {
                Initializer?.Invoke(control);
            }

            public void RemoveInitializer(Action<AbstractControl> action)
            {
                Initializer -= action;
            }

            public void AddInitializer(Action<AbstractControl> action)
            {
                Initializer += action;
            }
        }
    }
}
