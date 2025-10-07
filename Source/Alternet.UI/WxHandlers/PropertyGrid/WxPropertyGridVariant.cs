using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class WxPropertyGridVariant : IDisposable, IPropertyGridVariant
    {
        private const string TypeNameNull = "null";
        private const string TypeNameBool = "bool";
        private const string TypeNameLongLong = "longlong";
        private const string TypeNameLong = "long";
        private const string TypeNameULongLong = "ulonglong";
        private const string TypeNameULong = "ulong";
        private const string TypeNameDateTime = "datetime";
        private const string TypeNameDouble = "double";
        private const string TypeNameString = "string";
        private const string TypeNameColor = "wxColour";
        private const string TypeNameColor2 = "wxColourPropertyValue";

        private readonly bool ownHandle;
        private IntPtr handle;

        public WxPropertyGridVariant()
        {
            handle = Native.PropertyGridVariant.CreateVariant();
            ownHandle = true;
        }

        public WxPropertyGridVariant(IntPtr handle)
        {
            this.handle = handle;
            ownHandle = false;
        }

        /// <summary>
        /// Called when <see cref="WxPropertyGridVariant"/> instance is destroyed.
        /// </summary>
        ~WxPropertyGridVariant()
        {
            Dispose();
        }

        public IntPtr Handle => handle;

        public object? AsObject
        {
            get
            {
                var type = ValueType;

                if (type == TypeNameNull)
                    return null;
                if (type == TypeNameBool)
                    return AsBool;
                if (type == TypeNameDateTime)
                    return AsDateTime;
                if (type == TypeNameDouble)
                    return AsDouble;
                if (type == TypeNameString)
                    return AsString;
                if (type == TypeNameLong || type == TypeNameLongLong)
                    return AsLong;
                if (type == TypeNameULong || type == TypeNameULongLong)
                    return AsULong;
                if (type == TypeNameColor || type == TypeNameColor2)
                    return AsColor;
                return null;
            }

            set
            {
                SetAsObject(value);
            }
        }

        public decimal AsDecimal
        {
            get
            {
                try
                {
                    var type = ValueType;

                    if (type == TypeNameString)
                    {
                        if (decimal.TryParse(AsString, out var decimalResult))
                            return decimalResult;
                    }

                    if (type == TypeNameBool)
                        return AsBool ? 1 : 0;
                    if (type == TypeNameDouble)
                        return (decimal)AsDouble;
                    if (type == TypeNameLong || type == TypeNameLongLong)
                        return (decimal)AsLong;
                    if (type == TypeNameULong || type == TypeNameULongLong)
                        return (decimal)AsULong;
                    return default;
                }
                catch
                {
                    return default;
                }
            }
        }

        public double AsDouble
        {
            get
            {
                return Native.PropertyGridVariant.GetDouble(handle);
            }

            set
            {
                Native.PropertyGridVariant.SetDouble(handle, value);
            }
        }

        public uint AsUInt
        {
            get
            {
                return Native.PropertyGridVariant.GetUInt(handle);
            }

            set
            {
                Native.PropertyGridVariant.SetUInt(handle, value);
            }
        }

        public int AsInt
        {
            get
            {
                return Native.PropertyGridVariant.GetInt(handle);
            }

            set
            {
                Native.PropertyGridVariant.SetInt(handle, value);
            }
        }

        public bool AsBool
        {
            get
            {
                return Native.PropertyGridVariant.GetBool(handle);
            }

            set
            {
                Native.PropertyGridVariant.SetBool(handle, value);
            }
        }

        public long AsLong
        {
            get
            {
                return Native.PropertyGridVariant.GetLong(handle);
            }

            set
            {
                Native.PropertyGridVariant.SetLong(handle, value);
            }
        }

        public ulong AsULong
        {
            get
            {
                return Native.PropertyGridVariant.GetULong(handle);
            }

            set
            {
                Native.PropertyGridVariant.SetULong(handle, value);
            }
        }

        public DateTime AsDateTime
        {
            get
            {
                return Native.PropertyGridVariant.GetDateTime(handle);
            }

            set
            {
                Native.PropertyGridVariant.SetDateTime(handle, value);
            }
        }

        public Color AsColor
        {
            get
            {
                var color = Native.PropertyGridVariant.GetColor(handle);
                var kind = Native.PropertyGridVariant.GetLastColorKind();
                var result = WxPropertyGridHandler.SetColorKind(color, kind);
                return result;
            }

            set
            {
                uint kind = WxPropertyGridHandler.GetColorKind(value);
                Native.PropertyGridVariant.SetColor(handle, value, kind);
            }
        }

        public string AsString
        {
            get
            {
                return Native.PropertyGridVariant.GetString(handle);
            }

            set
            {
                Native.PropertyGridVariant.SetString(handle, value);
            }
        }

        public bool IsNull => Native.PropertyGridVariant.IsNull(handle);

        public string ValueType => Native.PropertyGridVariant.GetValueType(handle);

        public void Clear()
        {
            Native.PropertyGridVariant.Clear(handle);
        }

        public bool Unshare() => Native.PropertyGridVariant.Unshare(handle);

        public void MakeNull() => Native.PropertyGridVariant.MakeNull(handle);

        public bool IsType(string type) => Native.PropertyGridVariant.IsType(handle, type);

        public override string ToString() => Native.PropertyGridVariant.MakeString(handle);

        public void SetCompatibleValue(object? value, PropertyInfo p)
        {
            if (value is null)
                Clear();
            SetAsObject(value);
        }

        public object? GetCompatibleValue(IPropertyGridItem item)
        {
            var newValue = Internal();
            if (newValue is not null)
            {
                var typeConverter = item?.TypeConverter;
                
                if(typeConverter is null)
                {
                    var type = item?.PropInfo?.PropertyType;
                    if(type is not null)
                    typeConverter = StringConverters.Default.GetTypeConverter(type);
                }               

                if (typeConverter is not null)
                {
                    if (typeConverter.CanConvertFrom(newValue.GetType()))
                    {
                        var converted = typeConverter.ConvertFrom(newValue);
                        newValue = converted;
                    }
                }
            }

            return newValue;

            object? Internal()
            {
                if (IsNull)
                    return null;

                var p = item.PropInfo;

                if (p == null)
                    return AsObject;

                var type = AssemblyUtils.GetRealType(p.PropertyType);
                TypeCode typeCode = Type.GetTypeCode(type);
                var nullable = AssemblyUtils.GetNullable(p);

                if (type.IsEnum)
                {
                    if (nullable)
                    {
                        var kind = item.PropertyEditorKind;
                        var kindisEnum = kind == PropertyGridEditKindAll.Enum
                            || kind == PropertyGridEditKindAll.EnumEditable;
                        var choices = item.Choices;
                        if (choices != null && kindisEnum)
                        {
                            var value = AsLong;
                            if (value == choices.NullableValue)
                                return null;
                        }
                    }

                    return Enum.ToObject(type, AsLong);
                }

                switch (typeCode)
                {
                    case TypeCode.Empty:
                        return null;
                    case TypeCode.DBNull:
                        return null;
                    case TypeCode.Object:
                        return AsObject;
                    case TypeCode.Boolean:
                        return AsBool;
                    case TypeCode.Char:
                        var s = AsString;
                        if (s.Length < 1)
                        {
                            if (nullable)
                                return null;
                            return 0;
                        }

                        return s[0];
                    case TypeCode.SByte:
                        return (sbyte)AsLong;
                    case TypeCode.Byte:
                        return (byte)AsULong;
                    case TypeCode.Int16:
                        return (short)AsLong;
                    case TypeCode.UInt16:
                        return (ushort)AsULong;
                    case TypeCode.Int32:
                        return (int)AsLong;
                    case TypeCode.UInt32:
                        return (uint)AsULong;
                    case TypeCode.Int64:
                        return AsLong;
                    case TypeCode.UInt64:
                        return AsULong;
                    case TypeCode.Single:
                        return (float)AsDouble;
                    case TypeCode.Double:
                        return AsDouble;
                    case TypeCode.Decimal:
                        return AsDecimal;
                    case TypeCode.DateTime:
                        return AsDateTime;
                    case TypeCode.String:
                        return AsString;
                    default:
                        return null;
                }
            }
        }

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            if (handle != IntPtr.Zero)
            {
                if (ownHandle)
                    Native.PropertyGridVariant.Delete(handle);
                handle = IntPtr.Zero;
            }

            GC.SuppressFinalize(this);
        }

        private void SetAsObject(object? value)
        {
            if (value is null)
            {
                Clear();
                return;
            }

            var type = value.GetType();
            TypeCode typeCode = Type.GetTypeCode(type);

            switch (typeCode)
            {
                case TypeCode.Empty:
                case TypeCode.DBNull:
                default:
                    Clear();
                    return;
                case TypeCode.Object:
                    if (value is Color color)
                        AsColor = color;
                    else
                    {
                        var s = value.ToString();
                        AsString = s!;
                    }

                    return;
                case TypeCode.Boolean:
                    AsBool = (bool)value;
                    break;
                case TypeCode.SByte:
                    AsInt = (sbyte)value;
                    break;
                case TypeCode.Int16:
                    AsInt = (short)value;
                    break;
                case TypeCode.Int32:
                    AsInt = (int)value;
                    break;
                case TypeCode.Int64:
                    AsLong = (long)value;
                    break;
                case TypeCode.Byte:
                    AsUInt = (byte)value;
                    break;
                case TypeCode.UInt32:
                    AsUInt = (uint)value;
                    break;
                case TypeCode.UInt16:
                    AsUInt = (ushort)value;
                    break;
                case TypeCode.UInt64:
                    AsULong = (ulong)value;
                    break;
                case TypeCode.Single:
                    AsDouble = (float)value;
                    break;
                case TypeCode.Double:
                    AsDouble = (double)value;
                    break;
                case TypeCode.Decimal:
                    try
                    {
                        AsDouble = Convert.ToDouble((decimal)value);
                    }
                    catch
                    {
                        AsDouble = 0;
                    }

                    break;
                case TypeCode.DateTime:
                    AsDateTime = (DateTime)value;
                    break;
                case TypeCode.Char:
                case TypeCode.String:
                    AsString = value.ToString()!;
                    break;
            }
        }
    }
}