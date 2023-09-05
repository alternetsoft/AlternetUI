#pragma warning disable
using ApiCommon;
using System;
using System.Collections.Generic;
using System.Text;
using Alternet.Drawing;

namespace NativeApi.Api
{
    //https://docs.wxwidgets.org/3.2/classwx_variant.html
    public class PropertyGridVariant
    {
        public static void Delete(IntPtr handle) { }

        public static IntPtr CreateVariant() => throw new Exception();

        public static bool IsNull(IntPtr handle) => throw new Exception();
        public static bool Unshare(IntPtr handle) => throw new Exception();
        public static void MakeNull(IntPtr handle) => throw new Exception();
        public static void Clear(IntPtr handle) => throw new Exception();
        public static string GetValueType(IntPtr handle) => throw new Exception();
        public static bool IsType(IntPtr handle, string type) => throw new Exception();
        public static string MakeString(IntPtr handle) => throw new Exception();

        public static Color GetColor(IntPtr handle) => throw new Exception();
        public static double GetDouble(IntPtr handle) => throw new Exception();
        public static bool GetBool(IntPtr handle) => throw new Exception();

        public static int GetInt(IntPtr handle) => throw new Exception();
        public static uint GetUInt(IntPtr handle) => throw new Exception();

        public static long GetLong(IntPtr handle) => throw new Exception();
        public static ulong GetULong(IntPtr handle) => throw new Exception();
        public static Alternet.UI.DateTime GetDateTime(IntPtr handle) => throw new Exception();
        public static string GetString(IntPtr handle) => throw new Exception();

        public static void SetColor(IntPtr handle, Color val) => throw new Exception();
        public static void SetDouble(IntPtr handle, double val) => throw new Exception();
        public static void SetBool(IntPtr handle, bool val) => throw new Exception();
        public static void SetLong(IntPtr handle, long val) => throw new Exception();
        public static void SetULong(IntPtr handle, ulong val) => throw new Exception();
        public static void SetInt(IntPtr handle, int val) => throw new Exception();
        public static void SetUInt(IntPtr handle, uint val) => throw new Exception();
        public static void SetShort(IntPtr handle, short val) => throw new Exception();
        public static void SetDateTime(IntPtr handle, Alternet.UI.DateTime val) =>
            throw new Exception();
        public static void SetString(IntPtr handle, string value) => throw new Exception();
    }
}