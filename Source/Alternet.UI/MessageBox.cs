using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Alternet.UI
{
    public static class MessageBox
    {
        public static void Show(string text, string? caption = null)
        {
            if (text is null)
                throw new ArgumentNullException(nameof(text));

            Native.MessageBox.Show(text, caption);
        }
    }
}