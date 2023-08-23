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
        private IntPtr handle;

        public PropertyGridChoices(IntPtr handle)
        {
            this.handle = handle;
        }

        public PropertyGridChoices()
        {
            handle = Native.PropertyGridChoices.CreatePropertyGridChoices();
        }

        public IntPtr Handle => handle;

        public void Add(string text, int value, ImageSet? bitmap = null)
        {
            Native.PropertyGridChoices.Add(
                handle,
                text,
                value,
                bitmap?.NativeImageSet);
        }
    }
}