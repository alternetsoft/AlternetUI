using ApiCommon;

namespace NativeApi.Api
{
    public class QueryContinueDragEventData : NativeEventData
    {
        public DragInputState inputState;

        public bool escapePressed;

        public DragAction action;
    }
}