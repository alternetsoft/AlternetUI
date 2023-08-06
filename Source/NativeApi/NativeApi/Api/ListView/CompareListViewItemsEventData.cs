
using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class CompareListViewItemsEventData : NativeEventData
    {
        public long item1Index;
        public long item2Index;
    }
}