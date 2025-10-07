using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class WxPropertyGridChoices : IPropertyGridChoices
    {
        public const int DefaultNullableValue = int.MaxValue - 1;
        private readonly IntPtr handle;
        private int newItemId;
        private bool isNullable;
        private IPropertyGridChoices? nullableChoices;
        private int nullableValue = DefaultNullableValue;
        private bool hasCustomFonts = false;
        private bool hasBitmaps = false;
        private bool hasCustomFgColors = false;
        private bool hasCustomBgColors = false;

        public WxPropertyGridChoices(IntPtr handle)
        {
            this.handle = handle;
        }

        public WxPropertyGridChoices()
        {
            handle = Native.PropertyGridChoices.CreatePropertyGridChoices();
        }

        public bool HasCustomFonts => hasCustomFonts;

        public bool HasBitmaps => hasBitmaps;

        public bool HasCustomFgColors => hasCustomFgColors;

        public bool HasCustomBgColors => hasCustomBgColors;

        public int NullableValue
        {
            get
            {
                return nullableValue;
            }

            set
            {
                if (nullableValue == value)
                    return;
                nullableValue = value;
                ChoicesChanged();
            }
        }

        public IPropertyGridChoices NullableChoices
        {
            get
            {
                if (isNullable)
                    return this;
                if(nullableChoices == null)
                {
                    var nullable = new WxPropertyGridChoices
                    {
                        isNullable = true,
                    };
                    nullableChoices = nullable;
                }

                if (nullableChoices.Count == 0)
                {
                    nullableChoices.Add(string.Empty, NullableValue);
                    nullableChoices.AddRange(this);
                }

                return nullableChoices;
            }
        }

        public bool IsNullable { get => isNullable; }

        public int Count => (int)Native.PropertyGridChoices.GetCount(handle);

        public IntPtr Handle => handle;

        public void Add(string text, out int value)
        {
            int id = GenItemIndex();
            Add(text, id);
            value = id;
        }

        public int? GetValueFromLabel(string text)
        {
            int labelIndex = GetLabelIndex(text);
            if (labelIndex < 0)
                return null;
            int result = GetValue(labelIndex);
            return result;
        }

        public string? GetLabelFromValue(int value)
        {
            int valueIndex = GetValueIndex(value);
            if (valueIndex < 0)
                return null;
            string result = GetLabel(valueIndex);
            return result;
        }

        public void SetLabelForValue<T>(T value, string label)
            where T : Enum
        {
            SetLabelForValue(Convert.ToInt32(value), label);
        }

        public void SetLabelForValue(int value, string label)
        {
            int valueIndex = GetValueIndex(value);
            if (valueIndex < 0)
                return;
            SetLabel(valueIndex, label);
        }

        public void SetLabel(int index, string value)
        {
            Native.PropertyGridChoices.SetLabel(handle, (uint)index, value);
            ChoicesChanged();
        }

        public void SetBitmap(int index, ImageSet? bitmap)
        {
            hasBitmaps = true;
            Native.PropertyGridChoices.SetBitmap(
                handle,
                (uint)index,
                (UI.Native.ImageSet?)bitmap?.Handler);
            ChoicesChanged();
        }

        public void SetFgColor(int index, Color color)
        {
            hasCustomFgColors = true;
            Native.PropertyGridChoices.SetFgCol(handle, (uint)index, color);
            ChoicesChanged();
        }

        public void SetBgColor(int index, Color color)
        {
            hasCustomBgColors = true;
            Native.PropertyGridChoices.SetBgCol(handle, (uint)index, color);
            ChoicesChanged();
        }

        public Color GetFgColor(int index)
        {
            return Native.PropertyGridChoices.GetFgCol(handle, (uint)index);
        }

        public Color GetBgColor(int index)
        {
            return Native.PropertyGridChoices.GetBgCol(handle, (uint)index);
        }

        public string GetLabel(int ind)
        {
            return Native.PropertyGridChoices.GetLabel(handle, (uint)ind);
        }

        public int GetValue(int ind)
        {
            return Native.PropertyGridChoices.GetValue(handle, (uint)ind);
        }

        public int GetLabelIndex(string str)
        {
            return Native.PropertyGridChoices.GetLabelIndex(handle, str);
        }

        public int GetValueIndex(int val)
        {
            return Native.PropertyGridChoices.GetValueIndex(handle, val);
        }

        public void RemoveValue<T>(T value)
            where T : Enum
        {
            var index = GetValueIndex(Convert.ToInt32(value));
            if (index >= 0)
                RemoveAt(index);
        }

        public bool IsOk()
        {
            return Native.PropertyGridChoices.IsOk(handle);
        }

        public void RemoveAt(int nIndex, int count = 1)
        {
            Native.PropertyGridChoices.RemoveAt(handle, (uint)nIndex, (uint)count);
            ChoicesChanged();
        }

        public void Clear()
        {
            hasBitmaps = false;
            hasCustomFgColors = false;
            hasCustomBgColors = false;
            hasCustomFonts = false;
            newItemId = 0;
            Native.PropertyGridChoices.Clear(handle);
            ChoicesChanged();
        }

        public int Add(string? text, int value, ImageSet? bitmap = null)
        {
            if(bitmap != null)
                hasBitmaps = true;

            text ??= string.Empty;
            Native.PropertyGridChoices.Add(
                handle,
                text,
                value,
                (UI.Native.ImageSet?)bitmap?.Handler);
            ChoicesChanged();
            return Count - 1;
        }

        public int Add(string text, object value, ImageSet? bitmap = null)
        {
            var intValue = 0;
            if (value is not null)
                intValue = (int)value;
            return Add(text, intValue, bitmap);
        }

        public int Add(object value, ImageSet? bitmap = null)
        {
            var text = value?.ToString();
            return Add(text ?? string.Empty, value ?? 0, bitmap);
        }

        public void Insert(int index, string text, int value, ImageSet? bitmap)
        {
            if (bitmap != null)
                hasBitmaps = true;
            Native.PropertyGridChoices.Insert(
                handle,
                index,
                text,
                value,
                (UI.Native.ImageSet?)bitmap?.Handler);
            ChoicesChanged();
        }

        public void AddRange(IEnumerable<string> items)
        {
            if (items == null)
                return;
            foreach (var item in items)
                Add(item, out _);
        }

        public void AddRange(IEnumerable<object> items)
        {
            if (items == null)
                return;
            foreach (var item in items)
            {
                if (item == null)
                    continue;
                var s = item.ToString();
                if(s != null)
                    Add(s, out _);
            }
        }

        public void AddRange(IPropertyGridChoices choices)
        {
            for (int i = 0; i < choices.Count; i++)
            {
                string label = choices.GetLabel(i);
                int value = choices.GetValue(i);

                int index = Add(label, value);

                if (choices.HasBitmaps)
                {
                    Native.PropertyGridChoices.SetBitmapFromItem(
                        Handle,
                        (uint)index,
                        ((WxPropertyGridChoices)choices).Handle,
                        (uint)i);
                }

                if (choices.HasCustomFgColors)
                {
                    Color fgColor = choices.GetFgColor(i);
                    SetFgColor(index, fgColor);
                }

                if (choices.HasCustomBgColors)
                {
                    Color bgColor = choices.GetBgColor(i);
                    SetBgColor(index, bgColor);
                }
            }
        }

        internal void SetFont(int index, IntPtr font)
        {
            hasCustomFonts = true;
            Native.PropertyGridChoices.SetFont(handle, (uint)index, font);
        }

        private void ChoicesChanged()
        {
            nullableChoices?.Clear();
        }

        private int GenItemIndex()
        {
            newItemId++;
            return newItemId;
        }
    }
}