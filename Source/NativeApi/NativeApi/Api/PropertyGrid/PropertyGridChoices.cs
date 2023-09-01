#pragma warning disable
using ApiCommon;
using System;
using System.Collections.Generic;
using System.Text;
using Alternet.Drawing;

namespace NativeApi.Api
{
    public class PropertyGridChoices
    {
        public static IntPtr CreatePropertyGridChoices() => default;

        public static void Delete(IntPtr handle) { }

        public static void Add(IntPtr handle, string text, int value,
            ImageSet? bitmapBundle) {}

        // Returns label of item.
        public static string GetLabel(IntPtr handle, uint ind) => default;

        // Returns number of items.
        public static uint GetCount(IntPtr handle) => default;

        // Returns value of item.
        public static int GetValue(IntPtr handle, uint ind) => default;

        // Returns index of item with given label.
        public static int GetLabelIndex(IntPtr handle, string str) => default;

        // Returns index of item with given value.
        public static int GetValueIndex(IntPtr handle, int val) => default;

        public static void Insert(IntPtr handle, int index, string text, int value,
            ImageSet? bitmapBundle) { }

        // Returns false if this is a constant empty set of choices,
        // which should not be modified.
        public static bool IsOk(IntPtr handle) => default;

        // Removes count items starting at position nIndex.
        public static void RemoveAt(IntPtr handle, ulong nIndex, ulong count = 1) { }

        public static void Clear(IntPtr handle) { }
    }
}


