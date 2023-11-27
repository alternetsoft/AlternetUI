using System;

namespace Alternet.UI
{
    /// <summary>
    /// Contains default property values for all the controls in the library.
    /// </summary>
    public class AllControlDefaults
    {
        private readonly ControlDefaults[] controlProps =
            new ControlDefaults[(int)ControlTypeId.MaxValue + 1];

        static AllControlDefaults()
        {
        }

        /// <summary>
        /// Contains default property values for the <see cref="Control"/>.
        /// </summary>
        public ControlDefaults Control => GetProps(ControlTypeId.Control);

        /// <summary>
        /// Contains default property values for the <see cref="Button"/>.
        /// </summary>
        public ControlDefaults Button => GetProps(ControlTypeId.Button);

        /// <summary>
        /// Contains default property values for the <see cref="CheckBox"/>.
        /// </summary>
        public ControlDefaults CheckBox => GetProps(ControlTypeId.CheckBox);

        /// <summary>
        /// Contains default property values for the <see cref="RadioButton"/>.
        /// </summary>
        public ControlDefaults RadioButton => GetProps(ControlTypeId.RadioButton);

        /// <summary>
        /// Contains default property values for the <see cref="ColorPicker"/>.
        /// </summary>
        public ControlDefaults ColorPicker => GetProps(ControlTypeId.ColorPicker);

        /// <summary>
        /// Contains default property values for the <see cref="DateTimePicker"/>.
        /// </summary>
        public ControlDefaults DateTimePicker =>
            GetProps(ControlTypeId.DateTimePicker);

        /// <summary>
        /// Contains default property values for the <see cref="Grid"/>.
        /// </summary>
        public ControlDefaults Grid => GetProps(ControlTypeId.Grid);

        /// <summary>
        /// Contains default property values for the <see cref="GroupBox"/>.
        /// </summary>
        public ControlDefaults GroupBox => GetProps(ControlTypeId.GroupBox);

        /// <summary>
        /// Contains default property values for the <see cref="Label"/>.
        /// </summary>
        public ControlDefaults Label => GetProps(ControlTypeId.Label);

        /// <summary>
        /// Contains default property values for the <see cref="LayoutPanel"/>.
        /// </summary>
        public ControlDefaults LayoutPanel => GetProps(ControlTypeId.LayoutPanel);

        /// <summary>
        /// Contains default property values for the <see cref="ComboBox"/>.
        /// </summary>
        public ControlDefaults ComboBox => GetProps(ControlTypeId.ComboBox);

        /// <summary>
        /// Contains default property values for the <see cref="MultilineTextBox"/>.
        /// </summary>
        public ControlDefaults MultilineTextBox => GetProps(ControlTypeId.MultilineTextBox);

        /// <summary>
        /// Contains default property values for the <see cref="ListBox"/>.
        /// </summary>
        public ControlDefaults ListBox => GetProps(ControlTypeId.ListBox);

        /// <summary>
        /// Contains default property values for the <see cref="CheckListBox"/>.
        /// </summary>
        public ControlDefaults CheckListBox => GetProps(ControlTypeId.CheckListBox);

        /// <summary>
        /// Contains default property values for the <see cref="ListView"/>.
        /// </summary>
        public ControlDefaults ListView => GetProps(ControlTypeId.ListView);

        /// <summary>
        /// Contains default property values for the <see cref="Menu"/>.
        /// </summary>
        public ControlDefaults Menu => GetProps(ControlTypeId.Menu);

        /// <summary>
        /// Contains default property values for the <see cref="NumericUpDown"/>.
        /// </summary>
        public ControlDefaults NumericUpDown => GetProps(ControlTypeId.NumericUpDown);

        /// <summary>
        /// Contains default property values for the <see cref="PictureBox"/>.
        /// </summary>
        public ControlDefaults PictureBox => GetProps(ControlTypeId.PictureBox);

        /// <summary>
        /// Contains default property values for the <see cref="Popup"/>.
        /// </summary>
        public ControlDefaults Popup => GetProps(ControlTypeId.Popup);

        /// <summary>
        /// Contains default property values for the <see cref="ProgressBar"/>.
        /// </summary>
        public ControlDefaults ProgressBar => GetProps(ControlTypeId.ProgressBar);

        /// <summary>
        /// Contains default property values for the <see cref="ScrollViewer"/>.
        /// </summary>
        public ControlDefaults ScrollViewer => GetProps(ControlTypeId.ScrollViewer);

        /// <summary>
        /// Contains default property values for the <see cref="Slider"/>.
        /// </summary>
        public ControlDefaults Slider => GetProps(ControlTypeId.Slider);

        /// <summary>
        /// Contains default property values for the <see cref="SplitterPanel"/>.
        /// </summary>
        public ControlDefaults SplitterPanel => GetProps(ControlTypeId.SplitterPanel);

        /// <summary>
        /// Contains default property values for the <see cref="StackPanel"/>.
        /// </summary>
        public ControlDefaults StackPanel => GetProps(ControlTypeId.StackPanel);

        /// <summary>
        /// Contains default property values for the <see cref="StatusBar"/>.
        /// </summary>
        public ControlDefaults StatusBar => GetProps(ControlTypeId.StatusBar);

        /// <summary>
        /// Contains default property values for the <see cref="StatusBarPanel"/>.
        /// </summary>
        public ControlDefaults StatusBarPanel =>
            GetProps(ControlTypeId.StatusBarPanel);

        /// <summary>
        /// Contains default property values for the <see cref="TabControl"/>.
        /// </summary>
        public ControlDefaults TabControl => GetProps(ControlTypeId.TabControl);

        /// <summary>
        /// Contains default property values for the <see cref="TabPage"/>.
        /// </summary>
        public ControlDefaults TabPage => GetProps(ControlTypeId.TabPage);

        /// <summary>
        /// Contains default property values for the <see cref="TextBox"/>.
        /// </summary>
        public ControlDefaults TextBox => GetProps(ControlTypeId.TextBox);

        /// <summary>
        /// Contains default property values for the <see cref="Toolbar"/>.
        /// </summary>
        public ControlDefaults Toolbar => GetProps(ControlTypeId.Toolbar);

        /// <summary>
        /// Contains default property values for the <see cref="ToolbarItem"/>.
        /// </summary>
        public ControlDefaults ToolbarItem => GetProps(ControlTypeId.ToolbarItem);

        /// <summary>
        /// Contains default property values for the <see cref="TreeView"/>.
        /// </summary>
        public ControlDefaults TreeView => GetProps(ControlTypeId.TreeView);

        /// <summary>
        /// Contains default property values for the <see cref="UserPaintControl"/>.
        /// </summary>
        public ControlDefaults UserPaintControl =>
            GetProps(ControlTypeId.UserPaintControl);

        /// <summary>
        /// Contains default property values for the <see cref="WebBrowser"/>.
        /// </summary>
        public ControlDefaults WebBrowser => GetProps(ControlTypeId.WebBrowser);

        /// <summary>
        /// Contains default property values for the <see cref="Window"/>.
        /// </summary>
        public ControlDefaults Window => GetProps(ControlTypeId.Window);

        /// <summary>
        /// Gets <see cref="ControlDefaults"/> for the specified <see cref="ControlTypeId"/>.
        /// </summary>
        /// <param name="control">Control identifier.</param>
        public ControlDefaults this[ControlTypeId control] => GetProps(control);

        /// <summary>
        /// Returns default property values for some control.
        /// </summary>
        /// <param name="control">Control identifier.</param>
        public ControlDefaults GetProps(ControlTypeId control)
        {
            ControlDefaults result = controlProps[(int)control];
            if(result == null)
            {
                result = new();
                controlProps[(int)control] = result;
            }

            return result;
        }

        /// <summary>
        /// Returns default property value for some control.
        /// </summary>
        /// <param name="control">Control identifier.</param>
        /// <param name="prop">Property identifier.</param>
        /// <returns></returns>
        public object? GetPropValue(ControlTypeId control, ControlDefaultsId prop)
        {
            ControlDefaults props = GetProps(control);
            object? result = props.GetProp(prop);
            if (result != null || control == ControlTypeId.Control)
                return result;
            props = GetProps(ControlTypeId.Control);
            result = props.GetProp(prop);
            return result;
        }
    }
}