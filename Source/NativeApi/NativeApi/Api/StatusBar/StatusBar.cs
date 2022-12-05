using System;

namespace NativeApi.Api
{
    public class StatusBar : Control
    {
        public int PanelCount { get; }

        public void InsertPanelAt(int index, StatusBarPanel item) => throw new Exception();

        public void RemovePanelAt(int index) => throw new Exception();

        public bool SizingGripVisible { get; set; }
    }
}