#nullable disable

using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class TreeViewItemLabelEditEventData : NativeEventData
    {
        public IntPtr item;
        public NativeStringSpan label;
        public bool editCancelled;
    }
}