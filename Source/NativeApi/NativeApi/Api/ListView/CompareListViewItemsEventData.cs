
using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class CompareListViewItemsEventData : NativeEventData
    {
        public int item1Index;
        public int item2Index;
    }
}