using ApiCommon;

namespace NativeApi.Api
{
    public class QueryContinueDragEventData : NativeEventData
    {
        public bool escapePressed;

        public DragAction action;
    }
}