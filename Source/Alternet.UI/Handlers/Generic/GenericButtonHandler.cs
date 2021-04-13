namespace Alternet.UI
{
    internal class GenericButtonHandler : GenericControlHandler<Button>
    {
        protected override void OnAttach()
        {
            base.OnAttach();

            Control.TextChanged += Control_TextChanged;

            var border = new Border();
            Control.Controls.Add(border);
        }

        protected override void OnDetach()
        {
            base.OnDetach();

            Control.TextChanged -= Control_TextChanged;
        }

        private void Control_TextChanged(object? sender, System.EventArgs? e)
        {
            if (e is null)
                throw new System.ArgumentNullException(nameof(e));

            Update();
        }
    }
}