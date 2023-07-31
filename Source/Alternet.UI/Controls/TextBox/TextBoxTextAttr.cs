using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class TextBoxTextAttr : ITextBoxTextAttr, IDisposable
    {
        private IntPtr handle;

        public TextBoxTextAttr()
        {
            handle = Native.TextBoxTextAttr.CreateTextAttr();
        }

        public TextBoxTextAttr(IntPtr handle)
        {
            this.handle = handle;
        }

        ~TextBoxTextAttr()
        {
            Dispose();
        }

        public IntPtr Handle => handle;

        public void SetTextColor(Color colText)
        {
            Native.TextBoxTextAttr.SetTextColor(handle, colText);
        }

        public void SetBackgroundColor(Color colBack)
        {
            Native.TextBoxTextAttr.SetBackgroundColor(handle, colBack);
        }

        public void Dispose()
        {
            if (handle != IntPtr.Zero)
            {
                Native.TextBoxTextAttr.Delete(handle);
                handle = IntPtr.Zero;
            }
        }
    }
}
