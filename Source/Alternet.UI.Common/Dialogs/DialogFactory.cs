using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            get
            {
                return handler ??= App.Handler.CreateDialogFactoryHandler();
            }

            set
            {
                handler = value;
            }
        }

        /// <summary>
        /// Shows developer tools window.
        /// </summary>
        public static void ShowDeveloperTools()
        {
            PanelDevTools.ShowDeveloperTools();
        }

        /// <summary>
        /// Shows "Run terminal command" dialog.
        /// </summary>
        /// <param name="defaultValue"></param>
        public static void ShowRunTerminalCommandDlg(string? defaultValue = default)
        {
            TextFromUserParams prm = new()
            {
                Title = "Run terminal command",
                DefaultValue = defaultValue,
                OnApply = (s) =>
                {
                    if (s is null)
                        return;
                    AppUtils.OpenTerminalAndRunCommand(s);
                },
            };

            GetTextFromUserAsync(prm);
        }

        /// <summary>
        /// Shows critical message on the screen using any possible way.
        /// </summary>
        /// <param name="s">Message to show.</param>
        /// <param name="e">Exception information.</param>
        /// <returns><c>true</c> on success, <c>false</c> on failure.</returns>
        public static void ShowCriticalMessage(string s, Exception? e = null)
        {
            try
            {
                LogUtils.DeleteLog();
                LogUtils.LogToFile(s);
                if (e is not null)
                    LogUtils.LogExceptionToFile(e);
                AppUtils.OpenLogFile();

                if (App.IsWindowsOS)
                {
                    try
                    {
                        var console = CustomWindowsConsole.Default;

                        console.BackColor = ConsoleColor.Black;
                        console.TextColor = ConsoleColor.White;
                        console.Clear();
                        console.WriteLine(s);
                        Console.ReadLine();
                    }
                    catch
                    {
                        AppUtils.OpenTerminalAndRunEcho(s);
                    }
                }
                else
                if (App.IsLinuxOS)
                {
                    AppUtils.OpenTerminalAndRunEcho(s);
                }
                else
                if (App.IsMacOS)
                {
                    AppUtils.OpenTerminalAndRunEcho(s);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Shows dialog which asks to enter the lightness value. Possible values are 0..200.
        /// </summary>
        /// <param name="defaultValue">Default value. Optional. If not specified, uses 100.</param>
        /// <param name="onApply">Action to call when 'Ok' button is pressed in the dialog.</param>
        public static void AskLightnessAsync(Action<byte> onApply, byte defaultValue = 100)
        {
            AskByteAsync("Lightness", onApply, 100, 200);
        }

        /// <summary>
        /// Shows dialog which asks to enter a <see cref="byte"/> value.
        /// </summary>
        /// <param name="title">Dialog title.</param>
        /// <param name="onApply">Action to call when 'Ok' button is pressed in the dialog.</param>
        /// <param name="defaultValue">Default value. Optional. If not specified, uses 0.</param>
        /// <param name="maxValue">Maximal value. Optional. If not specified, uses 255.</param>
        public static void AskByteAsync(
            string? title,
            Action<byte> onApply,
            byte defaultValue = 0,
            byte maxValue = 255)
        {
            ByteFromUserParams prm = new()
            {
                MaxValue = maxValue,
                Title = title,
                Message = $"{title} (0..{maxValue})",
                DefaultValue = defaultValue,
                OnApply = (v) =>
                {
                    if (v is not null)
                        onApply(Convert.ToByte(v.Value));
                },
            };

            GetNumberFromUserAsync(prm);
        }

        /// <summary>
        /// Shows dialog which asks to enter a <see cref="byte"/> value.
        /// </summary>
        /// <param name="title">Dialog title.</param>
        /// <param name="onApply">Action to call when 'Ok' button is pressed in the dialog.</param>
        /// <param name="defaultValue">Default value. Optional. If not specified, uses 0.</param>
        /// <param name="minValue">Minimal value. Optional. If not specified, uses 0.</param>
        /// <param name="maxValue">Maximal value. Optional. If not specified, uses 255.</param>
        public static void AskIntAsync(
            string? title,
            Action<int> onApply,
            int defaultValue = 0,
            int minValue = 0,
            int maxValue = 255)
        {
            ByteFromUserParams prm = new()
            {
                MaxValue = maxValue,
                MinValue = minValue,
                Title = title,
                Message = $"{title} (0..{maxValue})",
                DefaultValue = defaultValue,
                OnApply = (v) =>
                {
                    if (v is not null)
                        onApply(Convert.ToInt32(v.Value));
                },
            };

            GetNumberFromUserAsync(prm);
        }

        /// <summary>
        /// Shows dialog which asks to enter the transparency value. Possible values are 0..255.
        /// </summary>
        /// <param name="onApply">Action to call when 'Ok' button is pressed in the dialog.</param>
        /// <param name="defaultValue">Default value.</param>
        public static void AskTransparencyAsync(Action<byte> onApply, byte defaultValue)
        {
            AskByteAsync("Transparency", onApply, defaultValue);
        }

        /// <summary>
        /// Shows dialog which asks to enter the brightness value. Possible values are 0..255.
        /// </summary>
        /// <param name="defaultValue">Default value. Optional. If not specified, uses 255.</param>
        /// <param name="onApply">Action to call when 'Ok' button is pressed in the dialog.</param>
        public static void AskBrightnessAsync(Action<byte> onApply, byte defaultValue = 255)
        {
            AskByteAsync("Brightness", onApply, defaultValue);
        }

        /// <summary>
        /// Shows a dialog asking the user for numeric input.
        /// </summary>
        /// <remarks>
        /// Minimal, maximal and default values specified in the dialog
        /// parameters must be positive.
        /// </remarks>
        /// <param name="prm">Dialog parameters.</param>
        public static void GetNumberFromUserAsync(LongFromUserParams prm)
        {
            Handler.GetNumberFromUserAsync(prm);
        }

        /// <summary>
        /// Popups a dialog box with a title, message and input box.
        /// The user may type in text and press 'OK' to return this text, or press 'Cancel'
        /// to return the empty string.
        /// </summary>
        /// <param name="prm">Dialog parameters.</param>
        public static void GetTextFromUserAsync(TextFromUserParams prm)
        {
            Handler.GetTextFromUserAsync(prm);
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
        public static void EditPropertyWithListEditor(object? instance, PropertyInfo? propInfo)
        {
            PropertyGrid.RegisterCollectionEditors();

            var source = ListEditSource.CreateEditSource(instance, propInfo);
            if (source == null)
                return;

            var existing = App.FindVisibleWindow<WindowListEdit>();

            if(existing is not null)
            {
                existing.ShowAndFocus();
                App.LogWarning("List Editor is already shown. Please close it first.");
            }
            else
            {
                WindowListEdit dialog = new(source);
                dialog.Show();
            }
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
        public static void EditPropertyWithListEditor(object? instance, string propName)
        {
            var propInfo = AssemblyUtils.GetPropInfo(instance, propName);
            EditPropertyWithListEditor(instance, propInfo);
        }

        /// <summary>
        /// Edits <see cref="ListView.Columns"/> with list editor.
        /// </summary>
        /// <param name="control">Control which columns will be edited.</param>
        public static void EditColumnsWithListEditor(ListView control) =>
            EditPropertyWithListEditor(control, nameof(ListView.Columns));

        /// <summary>
        /// Edits <see cref="ListView.Items"/> with list editor.
        /// </summary>
        /// <param name="control">Control which items will be edited.</param>
        public static void EditItemsWithListEditor(ListView control) =>
            EditPropertyWithListEditor(control, nameof(ListView.Items));

        /// <summary>
        /// Edits <see cref="StatusBar.Panels"/> with list editor.
        /// </summary>
        /// <param name="control">Control which items will be edited.</param>
        public static void EditItemsWithListEditor(StatusBar? control)
        {
            EditPropertyWithListEditor(control, nameof(StatusBar.Panels));
        }

        /// <summary>
        /// Edits <see cref="ListControl{T}.Items"/> with list editor.
        /// </summary>
        /// <param name="control">Control which items will be edited.</param>
        public static void EditItemsWithListEditor<T>(ListControl<T> control)
            where T : class, new()
            => EditPropertyWithListEditor(control, "Items");

        /// <summary>
        /// Converts a <see cref="MessageBoxButtons"/> enumeration value
        /// to an array of <see cref="KnownButton"/>
        /// values.
        /// </summary>
        /// <param name="buttons">The <see cref="MessageBoxButtons"/> value to convert.</param>
        /// <returns>An array of <see cref="KnownButton"/> values that correspond
        /// to the specified <paramref name="buttons"/>
        /// value.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if
        /// <paramref name="buttons"/> is not a valid
        /// <see cref="MessageBoxButtons"/> value.</exception>
        public static KnownButton[] ConvertButtons(MessageBoxButtons buttons)
        {
            switch (buttons)
            {
                case MessageBoxButtons.OK:
                    return new[] { KnownButton.OK };
                case MessageBoxButtons.OKCancel:
                    return new[] { KnownButton.OK, KnownButton.Cancel };
                case MessageBoxButtons.YesNoCancel:
                    return new[] { KnownButton.Yes, KnownButton.No, KnownButton.Cancel };
                case MessageBoxButtons.YesNo:
                    return new[] { KnownButton.Yes, KnownButton.No };
                case MessageBoxButtons.AbortRetryIgnore:
                    return new[] { KnownButton.Abort, KnownButton.Retry, KnownButton.Ignore };
                case MessageBoxButtons.RetryCancel:
                    return new[] { KnownButton.Retry, KnownButton.Cancel };
                default:
                    throw new ArgumentOutOfRangeException(nameof(buttons), buttons, null);
            }
        }

        /// <summary>
        /// Converts a <see cref="KnownButton"/> value to its corresponding <see cref="DialogResult"/>.
        /// </summary>
        /// <param name="button">The <see cref="KnownButton"/> value to convert.</param>
        /// <returns>A <see cref="DialogResult"/> that corresponds to the
        /// specified <paramref name="button"/>. If the button is
        /// not recognized, returns <see cref="DialogResult.None"/>.</returns>
        public static DialogResult ToDialogResult(KnownButton? button)
        {
            if (button == null)
                return DialogResult.None;

            return button.Value switch
            {
                KnownButton.OK => DialogResult.OK,
                KnownButton.Cancel => DialogResult.Cancel,
                KnownButton.Yes => DialogResult.Yes,
                KnownButton.No => DialogResult.No,
                KnownButton.Abort => DialogResult.Abort,
                KnownButton.Retry => DialogResult.Retry,
                KnownButton.Ignore => DialogResult.Ignore,
                _ => DialogResult.None,
            };
        }
    }
}
