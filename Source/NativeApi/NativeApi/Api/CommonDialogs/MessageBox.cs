using ApiCommon;
using System;

namespace NativeApi.Api
{
    [NativeName("MessageBoxObj")]
    public static class MessageBox
    {
        public static DialogResult Show(
            Window? owner,
            NativeStringSpan text,
            NativeStringSpan caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton) => throw new Exception();
    }
}