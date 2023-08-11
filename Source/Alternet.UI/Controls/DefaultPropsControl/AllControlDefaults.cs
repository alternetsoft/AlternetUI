using System;

namespace Alternet.UI
{
    /// <summary>
    /// Contains default property values for all the controls in the library.
    /// </summary>
    public class AllControlDefaults
    {
        private readonly ControlDefaults[] controlProps =
            new ControlDefaults[(int)ControlId.MaxValue + 1];

        static AllControlDefaults()
        {
        }

        /// <summary>
        /// Contains default property values for the <see cref="Control"/>.
        /// </summary>
        public ControlDefaults Control => GetProps(ControlId.Control);

        /// <summary>
        /// Contains default property values for the <see cref="Button"/>.
        /// </summary>
        public ControlDefaults Button => GetProps(ControlId.Button);

        /// <summary>
        /// Contains default property values for the <see cref="CheckBox"/>.
        /// </summary>
        public ControlDefaults CheckBox => GetProps(ControlId.CheckBox);

        /// <summary>
        /// Contains default property values for the <see cref="RadioButton"/>.
        /// </summary>
        public ControlDefaults RadioButton => GetProps(ControlId.RadioButton);

        /// <summary>
        /// Contains default property values for the <see cref="ColorPicker"/>.
        /// </summary>
        public ControlDefaults ColorPicker => GetProps(ControlId.ColorPicker);

        /// <summary>
        /// Contains default property values for the <see cref="DateTimePicker"/>.
        /// </summary>
        public ControlDefaults DateTimePicker =>
            GetProps(ControlId.DateTimePicker);

        /// <summary>
        /// Contains default property values for the <see cref="Grid"/>.
        /// </summary>
        public ControlDefaults Grid => GetProps(ControlId.Grid);

        /// <summary>
        /// Contains default property values for the <see cref="GroupBox"/>.
        /// </summary>
        public ControlDefaults GroupBox => GetProps(ControlId.GroupBox);

        /// <summary>
        /// Contains default property values for the <see cref="Label"/>.
        /// </summary>
        public ControlDefaults Label => GetProps(ControlId.Label);

        /// <summary>
        /// Contains default property values for the <see cref="LayoutPanel"/>.
        /// </summary>
        public ControlDefaults LayoutPanel => GetProps(ControlId.LayoutPanel);

        /// <summary>
        /// Contains default property values for the <see cref="ComboBox"/>.
        /// </summary>
        public ControlDefaults ComboBox => GetProps(ControlId.ComboBox);

        /// <summary>
        /// Contains default property values for the <see cref="ListBox"/>.
        /// </summary>
        public ControlDefaults ListBox => GetProps(ControlId.ListBox);

        /// <summary>
        /// Contains default property values for the <see cref="CheckListBox"/>.
        /// </summary>
        public ControlDefaults CheckListBox => GetProps(ControlId.CheckListBox);

        /// <summary>
        /// Contains default property values for the <see cref="ListView"/>.
        /// </summary>
        public ControlDefaults ListView => GetProps(ControlId.ListView);

        /// <summary>
        /// Contains default property values for the <see cref="Menu"/>.
        /// </summary>
        public ControlDefaults Menu => GetProps(ControlId.Menu);

        /// <summary>
        /// Contains default property values for the <see cref="NumericUpDown"/>.
        /// </summary>
        public ControlDefaults NumericUpDown => GetProps(ControlId.NumericUpDown);

        /// <summary>
        /// Contains default property values for the <see cref="PictureBox"/>.
        /// </summary>
        public ControlDefaults PictureBox => GetProps(ControlId.PictureBox);

        /// <summary>
        /// Contains default property values for the <see cref="Popup"/>.
        /// </summary>
        public ControlDefaults Popup => GetProps(ControlId.Popup);

        /// <summary>
        /// Contains default property values for the <see cref="ProgressBar"/>.
        /// </summary>
        public ControlDefaults ProgressBar => GetProps(ControlId.ProgressBar);

        /// <summary>
        /// Contains default property values for the <see cref="ScrollViewer"/>.
        /// </summary>
        public ControlDefaults ScrollViewer => GetProps(ControlId.ScrollViewer);

        /// <summary>
        /// Contains default property values for the <see cref="Slider"/>.
        /// </summary>
        public ControlDefaults Slider => GetProps(ControlId.Slider);

        /// <summary>
        /// Contains default property values for the <see cref="SplitterPanel"/>.
        /// </summary>
        public ControlDefaults SplitterPanel => GetProps(ControlId.SplitterPanel);

        /// <summary>
        /// Contains default property values for the <see cref="StackPanel"/>.
        /// </summary>
        public ControlDefaults StackPanel => GetProps(ControlId.StackPanel);

        /// <summary>
        /// Contains default property values for the <see cref="StatusBar"/>.
        /// </summary>
        public ControlDefaults StatusBar => GetProps(ControlId.StatusBar);

        /// <summary>
        /// Contains default property values for the <see cref="StatusBarPanel"/>.
        /// </summary>
        public ControlDefaults StatusBarPanel =>
            GetProps(ControlId.StatusBarPanel);

        /// <summary>
        /// Contains default property values for the <see cref="TabControl"/>.
        /// </summary>
        public ControlDefaults TabControl => GetProps(ControlId.TabControl);

        /// <summary>
        /// Contains default property values for the <see cref="TabPage"/>.
        /// </summary>
        public ControlDefaults TabPage => GetProps(ControlId.TabPage);

        /// <summary>
        /// Contains default property values for the <see cref="TextBox"/>.
        /// </summary>
        public ControlDefaults TextBox => GetProps(ControlId.TextBox);

        /// <summary>
        /// Contains default property values for the <see cref="Toolbar"/>.
        /// </summary>
        public ControlDefaults Toolbar => GetProps(ControlId.Toolbar);

        /// <summary>
        /// Contains default property values for the <see cref="ToolbarItem"/>.
        /// </summary>
        public ControlDefaults ToolbarItem => GetProps(ControlId.ToolbarItem);

        /// <summary>
        /// Contains default property values for the <see cref="TreeView"/>.
        /// </summary>
        public ControlDefaults TreeView => GetProps(ControlId.TreeView);

        /// <summary>
        /// Contains default property values for the <see cref="UserPaintControl"/>.
        /// </summary>
        public ControlDefaults UserPaintControl =>
            GetProps(ControlId.UserPaintControl);

        /// <summary>
        /// Contains default property values for the <see cref="WebBrowser"/>.
        /// </summary>
        public ControlDefaults WebBrowser => GetProps(ControlId.WebBrowser);

        /// <summary>
        /// Contains default property values for the <see cref="Window"/>.
        /// </summary>
        public ControlDefaults Window => GetProps(ControlId.Window);

        /// <summary>
        /// Returns default property values for some control.
        /// </summary>
        /// <param name="control">Control identifier.</param>
        public ControlDefaults GetProps(ControlId control)
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
        public object? GetPropValue(ControlId control, ControlDefaultsId prop)
        {
            ControlDefaults props = GetProps(control);
            object? result = props.GetProp(prop);
            if (result != null || control == ControlId.Control)
                return result;
            props = GetProps(ControlId.Control);
            result = props.GetProp(prop);
            return result;
        }
    }
}