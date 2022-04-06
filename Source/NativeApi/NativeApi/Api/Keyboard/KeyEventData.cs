
using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class KeyEventData : NativeEventData
    {
        public Key key;
        public long timestamp;
        public bool isRepeat;
    }
}