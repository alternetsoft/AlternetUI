using ApiCommon;
using System;

namespace NativeApi.Api
{
    [NativeName("MessageBox_")]
    public static class MessageBox
    {
        public static MessageBoxResult Show(
            Window? owner,
            string text,
            string? caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton) => throw new Exception();
    }

    public enum MessageBoxResult
    {
        OK,
        Cancel,
        Yes,
        No
    }

    public enum MessageBoxDefaultButton
    {
        OK,
        Cancel,
        Yes,
        No
    }

    public enum MessageBoxButtons
    {
        OK,
        OKCancel,
        YesNoCancel,
        YesNo,
    }

    public enum MessageBoxIcon
    {
        None,
        Information,
        Question,
        Warning,
        Error
    }
}