using ApiCommon;
using System;

namespace NativeApi.Api
{
    [Api]
    [NativeName("MessageBox_")]
    public static class MessageBox
    {
        public static void Show(string text, string? caption = null) => throw new Exception();
    }
}