using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods which call standard dialogs.
    /// </summary>
    public static class DialogFactory
    {
        private static IDialogFactoryHandler? handler;

        /// <summary>
        /// Gets or sets <see cref="IDialogFactoryHandler"/> object used to create dialogs.
        /// </summary>
        public static IDialogFactoryHandler Handler
        {
            get => handler ??= App.Handler.CreateDialogFactoryHandler();

            set => handler = value;
        }

        /// <summary>
        /// Shows developer tools window.
        /// </summary>
        public static void ShowDeveloperTools()
        {
            DeveloperToolsPanel.ShowDeveloperTools();
        }

        /// <summary>
        /// Shows dialog which asks to enter the lightness value. Possible values are 0..200.
        /// </summary>
        /// <param name="defaultValue">Default value. Optional. If not specified, uses 100.</param>
        /// <returns></returns>
        public static byte? AskLightness(byte defaultValue = 100)
        {
            return AskByte("Lightness", 100, 200);
        }

        /// <summary>
        /// Shows dialog which asks to enter a <see cref="byte"/> value.
        /// </summary>
        /// <param name="title">Dialog title.</param>
        /// <param name="defaultValue">Default value. Optional. If not specified, uses 0.</param>
        /// <param name="maxValue">Maximal value. Optional. If not specified, uses 255.</param>
        /// <returns></returns>
        public static byte? AskByte(string title, byte defaultValue = 0, byte maxValue = 255)
        {
            var result = DialogFactory.GetNumberFromUser(
                null,
                $"{title} (0..{maxValue})",
                null,
                defaultValue,
                0,
                maxValue);
            if (result is null)
                return null;
            return (byte)result.Value;
        }

        /// <summary>
        /// Shows dialog which asks to enter the transparency value. Possible values are 0..255.
        /// </summary>
        /// <param name="defaultValue">Default value.</param>
        /// <returns></returns>
        public static byte? AskTransparency(byte defaultValue)
        {
            return AskByte("Transparency", defaultValue);
        }

        /// <summary>
        /// Shows dialog which asks to enter the brightness value. Possible values are 0..255.
        /// </summary>
        /// <param name="defaultValue">Default value. Optional. If not specified, uses 255.</param>
        /// <returns></returns>
        public static byte? AskBrightness(byte defaultValue = 255)
        {
            return AskByte("Brightness", defaultValue);
        }

        /// <summary>
        /// Shows a dialog asking the user for numeric input.
        /// </summary>
        /// <remarks>
        /// The dialogs title is set to <paramref name="caption"/>, it contains a (possibly) multiline
        /// <paramref name="message"/> above the single line prompt and the zone for entering
        /// the number. Dialog is centered on its parent unless an explicit position is given
        /// in <paramref name="pos"/>.
        /// </remarks>
        /// <remarks>
        /// If the user cancels the dialog, the function returns <c>null</c>.
        /// </remarks>
        /// <remarks>
        /// The number entered must be in the range <paramref name="min"/> to <paramref name="max"/>
        /// (both of which should be positive) and
        /// value is the initial value of it. If the user enters an invalid value, it is forced to fall
        /// into the specified range.
        /// </remarks>
        /// <param name="message">A (possibly) multiline dialog message above the single line
        /// <paramref name="prompt"/>.</param>
        /// <param name="prompt">Single line dialog prompt.</param>
        /// <param name="caption">Dialog title.</param>
        /// <param name="value">Default value. Optional. Default is 0.</param>
        /// <param name="min">A positive minimal value. Optional. Default is 0.</param>
        /// <param name="max">A positive maximal value. Optional. Default is 100.</param>
        /// <param name="parent">Dialog parent.</param>
        /// <param name="pos"></param>
        public static long? GetNumberFromUser(
            string? message,
            string? prompt = null,
            string? caption = null,
            long value = 0,
            long min = 0,
            long max = 100,
            Control? parent = null,
            PointI? pos = default)
        {
            message ??= string.Empty;
            prompt ??= string.Empty;
            caption ??= CommonStrings.Default.WindowTitleInput;
            pos ??= PointI.MinusOne;

            return Handler.GetNumberFromUser(
                message,
                prompt,
                caption,
                value,
                min,
                max,
                parent,
                pos.Value);
        }

        /// <summary>
        /// Pop up a dialog box with title set to <paramref name="caption"/>,
        /// <paramref name="message"/>, and a <paramref name="defaultValue"/>.
        /// The user may type in text and press OK to return this text, or press Cancel
        /// to return the empty string.
        /// </summary>
        /// <param name="message">Dialog message.</param>
        /// <param name="caption">Dialog title.</param>
        /// <param name="defaultValue">Default value. Optional.</param>
        /// <param name="parent">Parent control. Optional.</param>
        /// <param name="x">X-position on the screen. Optional. By default is -1.</param>
        /// <param name="y">Y-position on the screen. Optional. By default is -1.</param>
        /// <param name="centre">If <c>true</c>, the message text (which may include new line
        /// characters) is centred; if <c>false</c>, the message is left-justified.</param>
        public static string? GetTextFromUser(
            string? message,
            string? caption,
            string? defaultValue = null,
            Control? parent = null,
            int x = -1,
            int y = -1,
            bool centre = true)
        {
            defaultValue ??= string.Empty;
            message ??= string.Empty;
            caption ??= CommonStrings.Default.WindowTitleInput;

            var result = Handler.GetTextFromUser(
                message,
                caption,
                defaultValue,
                parent,
                x,
                y,
                centre);
            return result;
        }

        /// <summary>
        /// Used as event handler.
        /// </summary>
        /// <param name="sender">Must implement <see cref="IPropInfoAndInstance"/>.</param>
        /// <param name="e">Event arguments.</param>
        /// <remarks>
        /// Calls <see cref="DialogFactory.EditPropertyWithListEditor(object,string)"/> for
        /// the <paramref name="sender"/>,
        /// if it implements <see cref="IPropInfoAndInstance"/> interface.
        /// </remarks>
        public static void EditWithListEdit(object? sender, EventArgs e)
        {
            if (sender is not IPropInfoAndInstance prop)
                return;
            var instance = prop.Instance;
            var propInfo = prop.PropInfo;

            DialogFactory.EditPropertyWithListEditor(instance, propInfo);
        }

        /// <summary>
        /// Edits property with list editor.
        /// </summary>
        /// <param name="instance">Object which contains the property.</param>
        /// <param name="propInfo">Property information.</param>
        /// <remarks>
        /// List editor must support editing of the property.
        /// </remarks>
        /// <returns><c>null</c> if property editing is not supported; <c>true</c> if editing
        /// was performed and user pressed 'Ok' button; <c>false</c> if user pressed
        /// 'Cancel' button.</returns>
        public static bool? EditPropertyWithListEditor(object? instance, PropertyInfo? propInfo)
        {
            PropertyGrid.RegisterCollectionEditors();

            var source = ListEditSource.CreateEditSource(instance, propInfo);
            if (source == null)
                return null;

            using ListEditDialogWindow dialog = new(source);
            var result = dialog.ShowModal(Window.ActiveWindow) == ModalResult.Accepted;
            return result;
        }

        /// <summary>
        /// Edits property with list editor.
        /// </summary>
        /// <param name="instance">Object which contains the property.</param>
        /// <param name="propName">Property name.</param>
        /// <remarks>
        /// List editor must support editing of the property.
        /// </remarks>
        /// <returns><c>null</c> if property editing is not supported; <c>true</c> if editing
        /// was performed and user pressed 'Ok' button; <c>false</c> if user pressed
        /// 'Cancel' button.</returns>
        public static bool? EditPropertyWithListEditor(object? instance, string propName)
        {
            var propInfo = AssemblyUtils.GetPropInfo(instance, propName);
            var result = EditPropertyWithListEditor(instance, propInfo);
            return result;
        }

        /// <summary>
        /// Edits <see cref="ListView.Columns"/> with list editor.
        /// </summary>
        /// <param name="control">Control which columns will be edited.</param>
        public static bool? EditColumnsWithListEditor(ListView control) =>
            EditPropertyWithListEditor(control, nameof(ListView.Columns));

        /// <summary>
        /// Edits <see cref="ListView.Items"/> with list editor.
        /// </summary>
        /// <param name="control">Control which items will be edited.</param>
        public static bool? EditItemsWithListEditor(ListView control) =>
            EditPropertyWithListEditor(control, nameof(ListView.Items));

        /// <summary>
        /// Edits <see cref="StatusBar.Panels"/> with list editor.
        /// </summary>
        /// <param name="control">Control which items will be edited.</param>
        public static bool? EditItemsWithListEditor(StatusBar? control)
        {
            return EditPropertyWithListEditor(control, nameof(StatusBar.Panels));
        }

        /// <summary>
        /// Edits <see cref="TreeView.Items"/> with list editor.
        /// </summary>
        /// <param name="control">Control which items will be edited.</param>
        public static bool? EditItemsWithListEditor(TreeView control) =>
            EditPropertyWithListEditor(control, nameof(TreeView.Items));

        /// <summary>
        /// Edits <see cref="ListControl.Items"/> with list editor.
        /// </summary>
        /// <param name="control">Control which items will be edited.</param>
        public static bool? EditItemsWithListEditor(ListControl control) =>
            EditPropertyWithListEditor(control, nameof(ListControl.Items));
    }
}
