using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class PropertyGridChoices : IPropertyGridChoices
    {
        private readonly IntPtr handle;
        private int newItemId;

        public PropertyGridChoices(IntPtr handle)
        {
            this.handle = handle;
        }

        public PropertyGridChoices()
        {
            handle = Native.PropertyGridChoices.CreatePropertyGridChoices();
        }

        public int Count => (int)Native.PropertyGridChoices.GetCount(handle);

        public IntPtr Handle => handle;

        public int Add(string text)
        {
            int id = GenItemIndex();
            Add(text, id);
            return id;
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

        public bool IsOk()
        {
            return Native.PropertyGridChoices.IsOk(handle);
        }

        public void RemoveAt(int nIndex, int count = 1)
        {
            Native.PropertyGridChoices.RemoveAt(handle, (uint)nIndex, (uint)count);
        }

        public void Clear()
        {
            Native.PropertyGridChoices.Clear(handle);
        }

        public void Add(string? text, int value, ImageSet? bitmap = null)
        {
            text ??= string.Empty;
            /*items.Add(new(value, text));*/
            Native.PropertyGridChoices.Add(
                handle,
                text,
                value,
                bitmap?.NativeImageSet);
        }

        public void Insert(int index, string text, int value, ImageSet? bitmapBundle)
        {
            Native.PropertyGridChoices.Insert(
                handle,
                index,
                text,
                value,
                bitmapBundle?.NativeImageSet);
        }

        public void AddRange(IEnumerable<string> items)
        {
            if (items == null)
                return;
            foreach (var item in items)
                Add(item);
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
                    Add(s);
            }
        }

        private int GenItemIndex()
        {
            newItemId++;
            return newItemId;
        }
    }
}