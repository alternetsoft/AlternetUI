#nullable disable

using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class TreeViewItemLabelEditEventData : NativeEventData
    {
        public IntPtr item;
        public string label;
        public bool editCancelled;
    }
}