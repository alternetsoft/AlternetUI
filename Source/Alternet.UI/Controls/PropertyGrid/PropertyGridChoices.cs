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
        private List<KeyValuePair<int, string>> items = new();

        public PropertyGridChoices(IntPtr handle)
        {
            this.handle = handle;
        }

        public PropertyGridChoices()
        {
            handle = Native.PropertyGridChoices.CreatePropertyGridChoices();
        }

        public IntPtr Handle => handle;

        public int Count => items.Count;

        public int Add(string text)
        {
            int id = GenItemIndex();
            Add(text, id);
            return id;
        }

        public string? GetText(int value)
        {
            foreach(var item in items)
            {
                if (item.Key == value)
                    return item.Value;
            }

            return null;
        }

        public int? GetValue(string text)
        {
            foreach (var item in items)
            {
                if (item.Value == text)
                    return item.Key;
            }

            return null;
        }

        public void Add(string? text, int value, ImageSet? bitmap = null)
        {
            text ??= string.Empty;
            items.Add(new(value, text));
            Native.PropertyGridChoices.Add(
                handle,
                text,
                value,
                bitmap?.NativeImageSet);
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
                Add(item.ToString());
            }
        }

        private int GenItemIndex()
        {
            newItemId++;
            return newItemId;
        }
    }
}