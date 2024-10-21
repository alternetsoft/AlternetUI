using Alternet.UI;
using System;

namespace ApiDoc
{
    public partial class WindowWindow : Alternet.UI.Window
    {
        public WindowWindow()
        {
            InitializeComponent();
            window.Background = Alternet.Drawing.Brushes.LightGray;
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
            MessageBox.Show(text, string.Empty);
        }
        #endregion    
    }
}