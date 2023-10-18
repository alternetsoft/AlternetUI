#pragma warning disable
using ApiCommon;
using System;

namespace NativeApi.Api
{
    // https://docs.wxwidgets.org/3.2/classwx_check_box.html
    public class CheckBox : Control
    {
        public string Text { get; set; }

        public bool IsChecked { get; set; }

        public int CheckState { get; set; }

        public bool ThreeState { get; set; }

        public bool AlignRight { get; set; }

        public bool AllowAllStatesForUser { get; set; }

        public event EventHandler? CheckedChanged;
    }
}