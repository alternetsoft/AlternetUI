namespace Alternet.UI
{
    public partial class DateTimePicker
    {
        [Browsable(false)]
        internal new string Text
        {
            get => base.Text;
            set => base.Text = value;
        }
    }
}
