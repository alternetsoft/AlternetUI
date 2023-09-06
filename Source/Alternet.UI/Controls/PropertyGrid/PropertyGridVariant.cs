using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class PropertyGridVariant : IDisposable, IPropertyGridVariant
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

        public PropertyGridVariant()
        {
            handle = Native.PropertyGridVariant.CreateVariant();
            ownHandle = true;
        }

        public PropertyGridVariant(IntPtr handle)
        {
            this.handle = handle;
            ownHandle = false;
        }

        /// <summary>
        /// Called when <see cref="PropertyGridVariant"/> instance is destroyed.
        /// </summary>
        ~PropertyGridVariant()
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
                return Native.PropertyGridVariant.GetColor(handle);
            }

            set
            {
                Native.PropertyGridVariant.SetColor(handle, value);
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

        public object? GetCompatibleValue(PropertyInfo p)
        {
            if (IsNull)
                return null;

            var type = AssemblyUtils.GetRealType(p.PropertyType);
            TypeCode typeCode = Type.GetTypeCode(type);
            var nullable = AssemblyUtils.GetNullable(p);

            switch (typeCode)
            {
                case TypeCode.Empty:
                    return null;
                case TypeCode.Object:
                    return AsObject;
                case TypeCode.DBNull:
                    return null;
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
                    return default(decimal);
                case TypeCode.DateTime:
                    return AsDateTime;
                case TypeCode.String:
                    return AsString;
                default:
                    return null;
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
                        AsString = value.ToString();
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
                    AsDouble = Convert.ToDouble((decimal)value);
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