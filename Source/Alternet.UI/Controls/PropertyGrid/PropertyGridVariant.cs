using System;
using System.Collections.Generic;
using System.Linq;
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
                var type = GetValueType();

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
                if(value is null)
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
                    case TypeCode.Object:
                        Clear();
                        return;
                    case TypeCode.Boolean:
                        AsBool = (bool)value;
                        break;
                    case TypeCode.SByte:
                        AsLong = (sbyte)value;
                        break;
                    case TypeCode.Int16:
                        AsLong = (short)value;
                        break;
                    case TypeCode.Int32:
                        AsLong = (int)value;
                        break;
                    case TypeCode.Int64:
                        AsLong = (long)value;
                        break;
                    case TypeCode.Byte:
                        AsULong = (byte)value;
                        break;
                    case TypeCode.UInt32:
                        AsLong = (uint)value;
                        break;
                    case TypeCode.UInt16:
                        AsULong = (ushort)value;
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

        public void Clear()
        {
            Native.PropertyGridVariant.Clear(handle);
        }

        public bool IsNull() => Native.PropertyGridVariant.IsNull(handle);

        public bool Unshare() => Native.PropertyGridVariant.Unshare(handle);

        public void MakeNull() => Native.PropertyGridVariant.MakeNull(handle);

        public string GetValueType() => Native.PropertyGridVariant.GetValueType(handle);

        public bool IsType(string type) => Native.PropertyGridVariant.IsType(handle, type);

        public override string ToString() => Native.PropertyGridVariant.MakeString(handle);

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            if (handle != IntPtr.Zero)
            {
                if (ownHandle)
                    Native.PropertyGridVariant.Delete(handle);
                handle = IntPtr.Zero;
            }
        }
    }
}