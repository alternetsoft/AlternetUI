
using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class TabPageSelectionEventData : NativeEventData
    {
        public int oldSelectedTabPageIndex;
        public int newSelectedTabPageIndex;
    }
}