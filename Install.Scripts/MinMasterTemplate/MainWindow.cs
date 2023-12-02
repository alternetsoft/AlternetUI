using Alternet.UI;

namespace MinMaster
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            var control = new ListBox();
	    control.Add("Hello");
            control.Parent = this;
            control.Width = this.Width;
            control.Height = this.Height;
        }
    }
}
