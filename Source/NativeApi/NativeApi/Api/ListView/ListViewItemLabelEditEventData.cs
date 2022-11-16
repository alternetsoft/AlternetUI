#nullable disable

using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class ListViewItemLabelEditEventData : NativeEventData
    {
        public int itemIndex;
        public string label;
        public bool editCancelled;
    }
}