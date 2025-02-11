using Alternet.UI;
using System;
using System.Diagnostics;

namespace ApiDoc
{
    public partial class WindowWindow : Alternet.UI.Window
    {
        public WindowWindow()
        {
            InitializeComponent();
            window.TitleChanged += Window_TitleChanged;
        }

        public void WindowExample1()
        {
            #region WindowCSharpCreation
            var wnd = new Alternet.UI.Window();
            #endregion
        }

        #region WindowEventHandler
        private void Window_TitleChanged(object? sender, EventArgs e)
        {
            var text = window.Title == "" ? "\"\"" : window.Title;
            Debug.WriteLine(text, string.Empty);
        }
        #endregion    
    }
}