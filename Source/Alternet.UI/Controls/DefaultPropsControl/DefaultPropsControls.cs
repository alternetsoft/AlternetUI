using System;

namespace Alternet.UI
{
    /// <summary>
    /// Contains default property values for all the controls in the library.
    /// </summary>
    public class DefaultPropsControls
    {
        private readonly DefaultPropsControl[] controlProps =
            new DefaultPropsControl[(int)AllControls.MaxValue + 1];

        static DefaultPropsControls()
        {
        }

        /// <summary>
        /// Contains default property values for the <see cref="Control"/>.
        /// </summary>
        public DefaultPropsControl Control => GetProps(AllControls.Control);

        /// <summary>
        /// Contains default property values for the <see cref="Button"/>.
        /// </summary>
        public DefaultPropsControl Button => GetProps(AllControls.Button);

        /// <summary>
        /// Contains default property values for the <see cref="CheckBox"/>.
        /// </summary>
        public DefaultPropsControl CheckBox => GetProps(AllControls.CheckBox);

        /// <summary>
        /// Contains default property values for the <see cref="RadioButton"/>.
        /// </summary>
        public DefaultPropsControl RadioButton => GetProps(AllControls.RadioButton);

        /// <summary>
        /// Contains default property values for the <see cref="ColorPicker"/>.
        /// </summary>
        public DefaultPropsControl ColorPicker => GetProps(AllControls.ColorPicker);

        /// <summary>
        /// Contains default property values for the <see cref="DateTimePicker"/>.
        /// </summary>
        public DefaultPropsControl DateTimePicker =>
            GetProps(AllControls.DateTimePicker);

        /// <summary>
        /// Contains default property values for the <see cref="Grid"/>.
        /// </summary>
        public DefaultPropsControl Grid => GetProps(AllControls.Grid);

        /// <summary>
        /// Contains default property values for the <see cref="Label"/>.
        /// </summary>
        public DefaultPropsControl GroupBox => GetProps(AllControls.GroupBox);

        /// <summary>
        /// Contains default property values for the <see cref=""/>.
        /// </summary>
        public DefaultPropsControl Label => GetProps(AllControls.Label);

        /// <summary>
        /// Contains default property values for the <see cref="LayoutPanel"/>.
        /// </summary>
        public DefaultPropsControl LayoutPanel => GetProps(AllControls.LayoutPanel);

        /// <summary>
        /// Contains default property values for the <see cref="ComboBox"/>.
        /// </summary>
        public DefaultPropsControl ComboBox => GetProps(AllControls.ComboBox);

        /// <summary>
        /// Contains default property values for the <see cref="ListBox"/>.
        /// </summary>
        public DefaultPropsControl ListBox => GetProps(AllControls.ListBox);

        /// <summary>
        /// Contains default property values for the <see cref="CheckListBox"/>.
        /// </summary>
        public DefaultPropsControl CheckListBox => GetProps(AllControls.CheckListBox);

        /// <summary>
        /// Contains default property values for the <see cref="ListView"/>.
        /// </summary>
        public DefaultPropsControl ListView => GetProps(AllControls.ListView);

        /// <summary>
        /// Contains default property values for the <see cref="Menu"/>.
        /// </summary>
        public DefaultPropsControl Menu => GetProps(AllControls.Menu);

        /// <summary>
        /// Contains default property values for the <see cref="NumericUpDown"/>.
        /// </summary>
        public DefaultPropsControl NumericUpDown => GetProps(AllControls.NumericUpDown);

        /// <summary>
        /// Contains default property values for the <see cref="PictureBox"/>.
        /// </summary>
        public DefaultPropsControl PictureBox => GetProps(AllControls.PictureBox);

        /// <summary>
        /// Contains default property values for the <see cref="Popup"/>.
        /// </summary>
        public DefaultPropsControl Popup => GetProps(AllControls.Popup);

        /// <summary>
        /// Contains default property values for the <see cref="ProgressBar"/>.
        /// </summary>
        public DefaultPropsControl ProgressBar => GetProps(AllControls.ProgressBar);

        /// <summary>
        /// Contains default property values for the <see cref="ScrollViewer"/>.
        /// </summary>
        public DefaultPropsControl ScrollViewer => GetProps(AllControls.ScrollViewer);

        /// <summary>
        /// Contains default property values for the <see cref="Slider"/>.
        /// </summary>
        public DefaultPropsControl Slider => GetProps(AllControls.Slider);

        /// <summary>
        /// Contains default property values for the <see cref="SplitterPanel"/>.
        /// </summary>
        public DefaultPropsControl SplitterPanel => GetProps(AllControls.SplitterPanel);

        /// <summary>
        /// Contains default property values for the <see cref="StackPanel"/>.
        /// </summary>
        public DefaultPropsControl StackPanel => GetProps(AllControls.StackPanel);

        /// <summary>
        /// Contains default property values for the <see cref="StatusBar"/>.
        /// </summary>
        public DefaultPropsControl StatusBar => GetProps(AllControls.StatusBar);

        /// <summary>
        /// Contains default property values for the <see cref="StatusBarPanel"/>.
        /// </summary>
        public DefaultPropsControl StatusBarPanel =>
            GetProps(AllControls.StatusBarPanel);

        /// <summary>
        /// Contains default property values for the <see cref="TabControl"/>.
        /// </summary>
        public DefaultPropsControl TabControl => GetProps(AllControls.TabControl);

        /// <summary>
        /// Contains default property values for the <see cref="TabPage"/>.
        /// </summary>
        public DefaultPropsControl TabPage => GetProps(AllControls.TabPage);

        /// <summary>
        /// Contains default property values for the <see cref="TextBox"/>.
        /// </summary>
        public DefaultPropsControl TextBox => GetProps(AllControls.TextBox);

        /// <summary>
        /// Contains default property values for the <see cref="Toolbar"/>.
        /// </summary>
        public DefaultPropsControl Toolbar => GetProps(AllControls.Toolbar);

        /// <summary>
        /// Contains default property values for the <see cref="ToolbarItem"/>.
        /// </summary>
        public DefaultPropsControl ToolbarItem => GetProps(AllControls.ToolbarItem);

        /// <summary>
        /// Contains default property values for the <see cref="TreeView"/>.
        /// </summary>
        public DefaultPropsControl TreeView => GetProps(AllControls.TreeView);

        /// <summary>
        /// Contains default property values for the <see cref="UserPaintControl"/>.
        /// </summary>
        public DefaultPropsControl UserPaintControl =>
            GetProps(AllControls.UserPaintControl);

        /// <summary>
        /// Contains default property values for the <see cref="WebBrowser"/>.
        /// </summary>
        public DefaultPropsControl WebBrowser => GetProps(AllControls.WebBrowser);

        /// <summary>
        /// Contains default property values for the <see cref="Window"/>.
        /// </summary>
        public DefaultPropsControl Window => GetProps(AllControls.Window);

        /// <summary>
        /// Returns default property values for some control.
        /// </summary>
        /// <param name="control">Control identifier.</param>
        public DefaultPropsControl GetProps(AllControls control)
        {
            DefaultPropsControl result = controlProps[(int)control];
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
        public object? GetPropValue(AllControls control, AllControlProps prop)
        {
            DefaultPropsControl props = GetProps(control);
            object? result = props.GetProp(prop);
            if (result != null || control == AllControls.Control)
                return result;
            props = GetProps(AllControls.Control);
            result = props.GetProp(prop);
            return result;
        }
    }
}