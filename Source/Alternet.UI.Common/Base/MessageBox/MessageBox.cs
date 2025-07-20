using System;
using System.Collections.Generic;

using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Displays a message window, also known as a dialog box, which presents a message to the user.
    /// It is a modal window, blocking other actions in the application until the user closes it.
    /// A <see cref="MessageBox"/> can contain text, buttons, and symbols that inform and instruct
    /// the user.
    /// </summary>
    /// <remarks>
    /// To display a message box, call one of the 'Show' static methods.
    /// The title, message, buttons, and icons
    /// displayed in the message box are determined by parameters that you pass to these methods.
    /// </remarks>
    public static class MessageBox
    {
        /// <summary>
        /// Gets or sets a value indicating whether the internal dialog should be used.
        /// </summary>
        public static bool UseInternalDialog = true && DebugUtils.IsDebuggerAttached;

        private static Stack<HelpInfo>? helpInfo;

        /// <summary>
        /// Fired when message box is show.
        /// </summary>
        /// <remarks>
        /// If an event handler is added to this event,
        /// default message box show handler is not used.
        /// </remarks>
        public static event EventHandler<BaseEventArgs<MessageBoxInfo>>? ShowDialog;

        /// <summary>
        /// Displays a message box in front of the specified object and with the specified
        /// text, caption, buttons, icon, and default button.
        /// </summary>
        /// <param name="text">The text to display in the message box.</param>
        /// <param name="caption">The text to display in the title bar of the message box.</param>
        /// <param name="buttons">One of the <see cref="MessageBoxButtons"/> values that
        /// specifies which buttons to display in the message box.</param>
        /// <param name="defaultButton">One of the <see cref="MessageBoxDefaultButton" /> values that
        /// specifies the default button for the message box.</param>
        /// <param name="onClose">The action which is called when dialog is closed.</param>
        public static void Show(
            string? text,
            string? caption,
            MessageBoxButtons buttons,
            MessageBoxDefaultButton defaultButton,
            MessageBoxResultDelegate? onClose = null)
        {
            ShowCore(
                null,
                text,
                caption,
                buttons,
                MessageBoxIcon.None,
                MessageBoxDefaultButton.Button1,
                (MessageBoxOptions)0,
                false,
                onClose);
        }

        /// <summary>
        /// Displays a message box with the specified text, caption, buttons, icon, default button,
        /// options, and Help button.</summary>
        /// <param name="text">The text to display in the message box.</param>
        /// <param name="caption">The text to display in the title bar of the message box.</param>
        /// <param name="buttons">One of the <see cref="MessageBoxButtons" /> values that specifies
        /// which buttons to display in the message box.</param>
        /// <param name="icon">One of the <see cref="MessageBoxIcon" /> values that specifies which icon
        /// to display in the message box.</param>
        /// <param name="defaultButton">One of the <see cref="MessageBoxDefaultButton" /> values that
        /// specifies the default button for the message box.</param>
        /// <param name="options">One of the <see cref="MessageBoxOptions" /> values that specifies
        /// which display and association options will be used for the message box. You may pass in 0
        /// if you wish to use the defaults.</param>
        /// <param name="displayHelpButton">
        /// <see langword="true" /> to show the Help button; otherwise, <see langword="false" />.
        /// The default is <see langword="false" />.</param>
        /// <param name="onClose">The action which is called when dialog is closed.</param>
        public static void Show(
            string? text,
            string? caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton,
            MessageBoxOptions options,
            bool displayHelpButton,
            MessageBoxResultDelegate? onClose = null)
        {
            ShowCore(null, text, caption, buttons, icon, defaultButton, options, displayHelpButton, onClose);
        }

        /// <summary>
        /// Displays a message box with the specified text, caption, buttons, icon, default button,
        /// options, and Help button, using the specified Help file.
        /// </summary>
        /// <param name="text">The text to display in the message box.</param>
        /// <param name="caption">The text to display in the title bar of the message box.</param>
        /// <param name="buttons">One of the <see cref="MessageBoxButtons" /> values
        /// that specifies which
        /// buttons to display in the message box.</param>
        /// <param name="icon">One of the <see cref="MessageBoxIcon" /> values that specifies which
        /// icon to display in the message box.</param>
        /// <param name="defaultButton">One of the <see cref="MessageBoxDefaultButton" /> values that
        /// specifies the default button for the message box.</param>
        /// <param name="options">One of the <see cref="MessageBoxOptions" />
        /// values that specifies which
        /// display and association options will be used for the message box.
        /// You may pass in 0 if you wish
        /// to use the defaults.</param>
        /// <param name="helpFilePath">The path and name of the Help file to
        /// display when the user clicks
        /// the Help button.</param>
        /// <param name="onClose">The action which is called when dialog is closed.</param>
        public static void Show(
            string? text,
            string? caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton,
            MessageBoxOptions options,
            string helpFilePath,
            MessageBoxResultDelegate? onClose = null)
        {
            HelpInfo hpi = new(helpFilePath);
            ShowCore(null, text, caption, buttons, icon, defaultButton, options, hpi, onClose);
        }

        /// <summary>Displays a message box with the specified text, caption,
        /// buttons, icon, default button,
        /// options, and Help button, using the specified Help file.</summary>
        /// <param name="owner">A <see cref="Window"/> that will own the modal dialog box.</param>
        /// <param name="text">The text to display in the message box.</param>
        /// <param name="caption">The text to display in the title bar of the message box.</param>
        /// <param name="buttons">One of the <see cref="MessageBoxButtons" /> values that specifies
        /// which buttons to display in the message box.</param>
        /// <param name="icon">One of the <see cref="MessageBoxIcon" /> values that
        /// specifies which
        /// icon to display in the message box.</param>
        /// <param name="defaultButton">One of the <see cref="MessageBoxDefaultButton" />
        /// values that
        /// specifies the default button for the message box.</param>
        /// <param name="options">One of the <see cref="MessageBoxOptions" /> values
        /// that specifies which
        /// display and association options will be used for the message box. You may
        /// pass in 0 if you wish
        /// to use the defaults.</param>
        /// <param name="helpFilePath">The path and name of the Help file to display
        /// when the user clicks
        /// the Help button.</param>
        /// <param name="onClose">The action which is called when dialog is closed.</param>
        public static void Show(
            Window? owner,
            string? text,
            string? caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton,
            MessageBoxOptions options,
            string helpFilePath,
            MessageBoxResultDelegate? onClose = null)
        {
            HelpInfo hpi = new(helpFilePath);
            ShowCore(owner, text, caption, buttons, icon, defaultButton, options, hpi, onClose);
        }

        /// <summary>Displays a message box with the specified text, caption, buttons, icon,
        /// default button,
        /// options, and Help button, using the specified Help file and Help keyword.</summary>
        /// <param name="text">The text to display in the message box.</param>
        /// <param name="caption">The text to display in the title bar of the message box.</param>
        /// <param name="buttons">One of the <see cref="MessageBoxButtons" /> values that
        /// specifies which
        /// buttons to display in the message box.</param>
        /// <param name="icon">One of the <see cref="MessageBoxIcon" /> values that specifies which icon
        /// to display in the message box.</param>
        /// <param name="defaultButton">One of the <see cref="MessageBoxDefaultButton" /> values that
        /// specifies the default button for the message box.</param>
        /// <param name="options">One of the <see cref="MessageBoxOptions" /> values that
        /// specifies which
        /// display and association options will be used for the message box. You may pass
        /// in 0 if you wish
        /// to use the defaults.</param>
        /// <param name="helpFilePath">The path and name of the Help file to display when
        /// the user clicks
        /// the Help button.</param>
        /// <param name="keyword">The Help keyword to display when the user clicks the
        /// Help button.</param>
        /// <param name="onClose">The action which is called when dialog is closed.</param>
        public static void Show(
            string? text,
            string? caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton,
            MessageBoxOptions options,
            string helpFilePath,
            string keyword,
            MessageBoxResultDelegate? onClose = null)
        {
            HelpInfo hpi = new(helpFilePath, keyword);
            ShowCore(null, text, caption, buttons, icon, defaultButton, options, hpi, onClose);
        }

        /// <summary>Displays a message box with the specified text, caption, buttons,
        /// icon, default button,
        /// options, and Help button, using the specified Help file and Help keyword.</summary>
        /// <param name="owner">A <see cref="Window"/> that will own the modal dialog box.</param>
        /// <param name="text">The text to display in the message box.</param>
        /// <param name="caption">The text to display in the title bar of the message box.</param>
        /// <param name="buttons">One of the <see cref="MessageBoxButtons" /> values that
        /// specifies which
        /// buttons to display in the message box.</param>
        /// <param name="icon">One of the <see cref="MessageBoxIcon" /> values that specifies which icon
        /// to display in the message box.</param>
        /// <param name="defaultButton">One of the <see cref="MessageBoxDefaultButton" /> values that
        /// specifies the default button for the message box.</param>
        /// <param name="options">One of the <see cref="MessageBoxOptions" /> values that specifies
        /// which display and association options will be used for the message box.
        /// You may pass in 0 if
        /// you wish to use the defaults.</param>
        /// <param name="helpFilePath">The path and name of the Help file to display when the user
        /// clicks the Help button.</param>
        /// <param name="keyword">The Help keyword to display when the user clicks
        /// the Help button.</param>
        /// <param name="onClose">The action which is called when dialog is closed.</param>
        public static void Show(
            Window? owner,
            string? text,
            string? caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton,
            MessageBoxOptions options,
            string helpFilePath,
            string keyword,
            MessageBoxResultDelegate? onClose = null)
        {
            HelpInfo hpi = new(helpFilePath, keyword);
            ShowCore(owner, text, caption, buttons, icon, defaultButton, options, hpi, onClose);
        }

        /// <summary>Displays a message box with the specified text, caption, buttons,
        /// icon, default button,
        /// options, and Help button, using the specified Help file and
        /// <see langword="HelpNavigator" />.</summary>
        /// <param name="text">The text to display in the message box.</param>
        /// <param name="caption">The text to display in the title bar of the message box.</param>
        /// <param name="buttons">One of the <see cref="MessageBoxButtons" /> values
        /// that specifies which
        /// buttons to display in the message box.</param>
        /// <param name="icon">One of the <see cref="MessageBoxIcon" /> values
        /// that specifies which icon to
        /// display in the message box.</param>
        /// <param name="defaultButton">One of the <see cref="MessageBoxDefaultButton" />
        /// values that specifies
        /// the default button for the message box.</param>
        /// <param name="options">One of the <see cref="MessageBoxOptions" />
        /// values that specifies which display
        /// and association options will be used for the message box.
        /// You may pass in 0 if you wish to use the defaults.</param>
        /// <param name="helpFilePath">The path and name of the Help file to display
        /// when the user clicks the
        /// Help button.</param>
        /// <param name="navigator">One of the <see cref="HelpNavigator" /> values.</param>
        /// <param name="onClose">The action which is called when dialog is closed.</param>
        public static void Show(
            string? text,
            string? caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton,
            MessageBoxOptions options,
            string helpFilePath,
            HelpNavigator navigator,
            MessageBoxResultDelegate? onClose = null)
        {
            HelpInfo hpi = new(helpFilePath, navigator);
            ShowCore(null, text, caption, buttons, icon, defaultButton, options, hpi, onClose);
        }

        /// <summary>Displays a message box with the specified text, caption, buttons,
        /// icon, default button,
        /// options, and Help button, using the specified Help file
        /// and <see langword="HelpNavigator" />.</summary>
        /// <param name="owner">A <see cref="Window"/> that will own the modal dialog box.</param>
        /// <param name="text">The text to display in the message box.</param>
        /// <param name="caption">The text to display in the title bar of the message box.</param>
        /// <param name="buttons">One of the <see cref="MessageBoxButtons" /> values
        /// that specifies which
        /// buttons to display in the message box.</param>
        /// <param name="icon">One of the <see cref="MessageBoxIcon" /> values that
        /// specifies which icon to
        /// display in the message box.</param>
        /// <param name="defaultButton">One of the <see cref="MessageBoxDefaultButton" />
        /// values that specifies
        /// the default button for the message box.</param>
        /// <param name="options">One of the <see cref="MessageBoxOptions" /> values
        /// that specifies which display
        /// and association options will be used for the message box. You may pass in 0
        /// if you wish to use the defaults.</param>
        /// <param name="helpFilePath">The path and name of the Help file to display when
        /// the user clicks the
        /// Help button.</param>
        /// <param name="navigator">One of the <see cref="HelpNavigator" /> values.</param>
        /// <param name="onClose">The action which is called when dialog is closed.</param>
        public static void Show(
            Window? owner,
            string? text,
            string? caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton,
            MessageBoxOptions options,
            string helpFilePath,
            HelpNavigator navigator,
            MessageBoxResultDelegate? onClose = null)
        {
            HelpInfo hpi = new(helpFilePath, navigator);
            ShowCore(owner, text, caption, buttons, icon, defaultButton, options, hpi, onClose);
        }

        /// <summary>Displays a message box with the specified text, caption, buttons, icon,
        /// default button,
        /// options, and Help button, using the specified Help file,
        /// <see langword="HelpNavigator" />, and Help topic.</summary>
        /// <param name="text">The text to display in the message box.</param>
        /// <param name="caption">The text to display in the title bar of the message box.</param>
        /// <param name="buttons">One of the <see cref="MessageBoxButtons" /> values that
        /// specifies which
        /// buttons to display in the message box.</param>
        /// <param name="icon">One of the <see cref="MessageBoxIcon" /> values that specifies
        /// which icon to
        /// display in the message box.</param>
        /// <param name="defaultButton">One of the <see cref="MessageBoxDefaultButton" />
        /// values that
        /// specifies the default button for the message box.</param>
        /// <param name="options">One of the <see cref="MessageBoxOptions" /> values that
        /// specifies which
        /// display and association options will be used for the message box. You may pass
        /// in 0 if you wish to
        /// use the defaults.</param>
        /// <param name="helpFilePath">The path and name of the Help file to display when
        /// the user clicks
        /// the Help button.</param>
        /// <param name="navigator">One of the <see cref="HelpNavigator" /> values.</param>
        /// <param name="param">The numeric ID of the Help topic to display when the user
        /// clicks the Help button.</param>
        /// <param name="onClose">The action which is called when dialog is closed.</param>
        public static void Show(
            string? text,
            string? caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton,
            MessageBoxOptions options,
            string helpFilePath,
            HelpNavigator navigator,
            object param,
            MessageBoxResultDelegate? onClose = null)
        {
            HelpInfo hpi = new(helpFilePath, navigator, param);
            ShowCore(null, text, caption, buttons, icon, defaultButton, options, hpi, onClose);
        }

        /// <summary>Displays a message box with the specified text, caption, buttons, icon,
        /// default button, options, and Help button, using the specified Help file,
        /// <see langword="HelpNavigator" />, and Help topic.</summary>
        /// <param name="owner">A <see cref="Window"/> that will own the modal dialog box.</param>
        /// <param name="text">The text to display in the message box.</param>
        /// <param name="caption">The text to display in the title bar of the message box.</param>
        /// <param name="buttons">One of the <see cref="MessageBoxButtons" /> values that specifies
        /// which buttons to display in the message box.</param>
        /// <param name="icon">One of the <see cref="MessageBoxIcon" /> values that specifies which
        /// icon to display in the message box.</param>
        /// <param name="defaultButton">One of the <see cref="MessageBoxDefaultButton" /> values
        /// that specifies the default button for the message box.</param>
        /// <param name="options">One of the <see cref="MessageBoxOptions" /> values that specifies
        /// which display and association options will be used for the message box. You may pass in 0
        /// if you wish to use the defaults.</param>
        /// <param name="helpFilePath">The path and name of the Help file to display when the user
        /// clicks the Help button.</param>
        /// <param name="navigator">One of the <see cref="HelpNavigator" /> values.</param>
        /// <param name="param">The numeric ID of the Help topic to display when the user
        /// clicks the Help button.</param>
        /// <param name="onClose">The action which is called when dialog is closed.</param>
        public static void Show(
            Window? owner,
            string? text,
            string? caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton,
            MessageBoxOptions options,
            string helpFilePath,
            HelpNavigator navigator,
            object param,
            MessageBoxResultDelegate? onClose = null)
        {
            HelpInfo hpi = new(helpFilePath, navigator, param);
            ShowCore(owner, text, caption, buttons, icon, defaultButton, options, hpi, onClose);
        }

        /// <summary>Displays a message box with the specified text, caption, buttons,
        /// icon, default button,
        /// and options.</summary>
        /// <param name="text">The text to display in the message box.</param>
        /// <param name="caption">The text to display in the title bar of the message box.</param>
        /// <param name="buttons">One of the <see cref="MessageBoxButtons" /> values that specifies
        /// which buttons to display in the message box.</param>
        /// <param name="icon">One of the <see cref="MessageBoxIcon" /> values that specifies which
        /// icon to display in the message box.</param>
        /// <param name="defaultButton">One of the <see cref="MessageBoxDefaultButton" /> values that
        /// specifies the default button for the message box.</param>
        /// <param name="options">One of the <see cref="MessageBoxOptions" /> values that specifies
        /// which display and association options will be used for the message box.
        /// You may pass in 0 if you
        /// wish to use the defaults.</param>
        /// <param name="onClose">The action which is called when dialog is closed.</param>
        public static void Show(
            string? text,
            string? caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton,
            MessageBoxOptions options,
            MessageBoxResultDelegate? onClose = null)
        {
            ShowCore(null, text, caption, buttons, icon, defaultButton, options, showHelp: false, onClose);
        }

        /// <summary>Displays a message box with the specified text, caption, buttons, icon,
        /// and default button.</summary>
        /// <param name="text">The text to display in the message box.</param>
        /// <param name="caption">The text to display in the title bar of the message box.</param>
        /// <param name="buttons">One of the <see cref="MessageBoxButtons" /> values that
        /// specifies which buttons
        /// to display in the message box.</param>
        /// <param name="icon">One of the <see cref="MessageBoxIcon" /> values that specifies
        /// which icon to display
        /// in the message box.</param>
        /// <param name="defaultButton">One of the <see cref="MessageBoxDefaultButton" />
        /// values that specifies the
        /// default button for the message box.</param>
        /// <param name="onClose">The action which is called when dialog is closed.</param>
        public static void Show(
            string? text,
            string? caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton,
            MessageBoxResultDelegate? onClose = null)
        {
            ShowCore(
                null,
                text,
                caption,
                buttons,
                icon,
                defaultButton,
                (MessageBoxOptions)0,
                showHelp: false,
                onClose);
        }

        /// <summary>Displays a message box with specified text, caption, buttons, and icon.</summary>
        /// <param name="text">The text to display in the message box.</param>
        /// <param name="caption">The text to display in the title bar of the message box.</param>
        /// <param name="buttons">One of the <see cref="MessageBoxButtons" /> values that
        /// specifies which buttons to
        /// display in the message box.</param>
        /// <param name="icon">One of the <see cref="MessageBoxIcon" /> values that specifies
        /// which icon to display
        /// in the message box.</param>
        /// <param name="onClose">The action which is called when dialog is closed.</param>
        public static void Show(
            string? text,
            string? caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxResultDelegate? onClose = null)
        {
            ShowCore(
                null,
                text,
                caption,
                buttons,
                icon,
                MessageBoxDefaultButton.Button1,
                (MessageBoxOptions)0,
                showHelp: false,
                onClose);
        }

        /// <summary>Displays a message box with specified text, caption, and buttons.</summary>
        /// <param name="text">The text to display in the message box.</param>
        /// <param name="caption">The text to display in the title bar of the message box.</param>
        /// <param name="buttons">One of the <see cref="MessageBoxButtons" /> values that specifies
        /// which buttons to display in the message box.</param>
        /// <param name="onClose">The action which is called when dialog is closed.</param>
        public static void Show(
            string? text,
            string? caption,
            MessageBoxButtons buttons,
            MessageBoxResultDelegate? onClose = null)
        {
            ShowCore(
                null,
                text,
                caption,
                buttons,
                MessageBoxIcon.None,
                MessageBoxDefaultButton.Button1,
                (MessageBoxOptions)0,
                showHelp: false,
                onClose);
        }

        /// <summary>Displays a message box with specified text and caption.</summary>
        /// <param name="text">The text to display in the message box.</param>
        /// <param name="caption">The text to display in the title bar of the message box.</param>
        /// <param name="onClose">The action which is called when dialog is closed.</param>
        public static void Show(
            string? text,
            string? caption,
            MessageBoxResultDelegate? onClose = null)
        {
            ShowCore(
                null,
                text,
                caption,
                MessageBoxButtons.OK,
                MessageBoxIcon.None,
                MessageBoxDefaultButton.Button1,
                (MessageBoxOptions)0,
                showHelp: false,
                onClose);
        }

        /// <summary>Displays a message box with specified text.</summary>
        /// <param name="text">The text to display in the message box.</param>
        /// <param name="onClose">The action which is called when dialog is closed.</param>
        public static void Show(string? text, MessageBoxResultDelegate? onClose = null)
        {
            ShowCore(
                null,
                text,
                string.Empty,
                MessageBoxButtons.OK,
                MessageBoxIcon.None,
                MessageBoxDefaultButton.Button1,
                (MessageBoxOptions)0,
                showHelp: false,
                onClose);
        }

        /// <summary>Displays a message box in front of the specified object and with
        /// the specified text,
        /// caption, buttons, icon, default button, and options.</summary>
        /// <param name="owner">A <see cref="Window"/> that will own the modal dialog box.</param>
        /// <param name="text">The text to display in the message box.</param>
        /// <param name="caption">The text to display in the title bar of the message box.</param>
        /// <param name="buttons">One of the <see cref="MessageBoxButtons" /> values
        /// that specifies which
        /// buttons to display in the message box.</param>
        /// <param name="icon">One of the <see cref="MessageBoxIcon" /> values that
        /// specifies which icon to
        /// display in the message box.</param>
        /// <param name="defaultButton">One of the <see cref="MessageBoxDefaultButton" />
        /// values the specifies
        /// the default button for the message box.</param>
        /// <param name="options">One of the <see cref="MessageBoxOptions" /> values
        /// that specifies which display
        /// and association options will be used for the message box. You may pass
        /// in 0 if you wish to use the defaults.</param>
        /// <param name="onClose">The action which is called when dialog is closed.</param>
        public static void Show(
            Window? owner,
            string? text,
            string? caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton,
            MessageBoxOptions options,
            MessageBoxResultDelegate? onClose = null)
        {
            ShowCore(owner, text, caption, buttons, icon, defaultButton, options, showHelp: false, onClose);
        }

        /// <summary>Displays a message box in front of the specified object and with
        /// the specified text, caption,
        /// buttons, icon, and default button.</summary>
        /// <param name="owner">A <see cref="Window"/> that will own the modal dialog box.</param>
        /// <param name="text">The text to display in the message box.</param>
        /// <param name="caption">The text to display in the title bar of the message box.</param>
        /// <param name="buttons">One of the <see cref="MessageBoxButtons" /> values
        /// that specifies which buttons
        /// to display in the message box.</param>
        /// <param name="icon">One of the <see cref="MessageBoxIcon" /> values that
        /// specifies which icon to display
        /// in the message box.</param>
        /// <param name="defaultButton">One of the <see cref="MessageBoxDefaultButton" />
        /// values that specifies the
        /// default button for the message box.</param>
        /// <param name="onClose">The action which is called when dialog is closed.</param>
        public static void Show(
            Window? owner,
            string? text,
            string? caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton,
            MessageBoxResultDelegate? onClose = null)
        {
            ShowCore(
                owner,
                text,
                caption,
                buttons,
                icon,
                defaultButton,
                (MessageBoxOptions)0,
                showHelp: false,
                onClose);
        }

        /// <summary>Displays a message box in front of the specified object and
        /// with the specified text, caption,
        /// buttons, and icon.</summary>
        /// <param name="owner">A <see cref="Window"/> that will own the modal dialog box.</param>
        /// <param name="text">The text to display in the message box.</param>
        /// <param name="caption">The text to display in the title bar of the message box.</param>
        /// <param name="buttons">One of the <see cref="MessageBoxButtons" /> values
        /// that specifies which
        /// buttons to display in the message box.</param>
        /// <param name="icon">One of the <see cref="MessageBoxIcon" /> values that
        /// specifies which icon to
        /// display in the message box.</param>
        /// <param name="onClose">The action which is called when dialog is closed.</param>
        public static void Show(
            Window? owner,
            string? text,
            string? caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxResultDelegate? onClose = null)
        {
            ShowCore(
                owner,
                text,
                caption,
                buttons,
                icon,
                MessageBoxDefaultButton.Button1,
                (MessageBoxOptions)0,
                showHelp: false,
                onClose);
        }

        /// <summary>Displays a message box in front of the specified object and
        /// with the specified text,
        /// caption, and buttons.</summary>
        /// <param name="owner">A <see cref="Window"/> that will own the modal dialog box.</param>
        /// <param name="text">The text to display in the message box.</param>
        /// <param name="caption">The text to display in the title bar of the message box.</param>
        /// <param name="buttons">One of the <see cref="MessageBoxButtons" />
        /// values that specifies which
        /// buttons to display in the message box.</param>
        /// <param name="onClose">The action which is called when dialog is closed.</param>
        public static void Show(
            Window? owner,
            string? text,
            string? caption,
            MessageBoxButtons buttons,
            MessageBoxResultDelegate? onClose = null)
        {
            ShowCore(
                owner,
                text,
                caption,
                buttons,
                MessageBoxIcon.None,
                MessageBoxDefaultButton.Button1,
                (MessageBoxOptions)0,
                showHelp: false,
                onClose);
        }

        /// <summary>Displays a message box in front of the specified object and with the
        /// specified text and caption.</summary>
        /// <param name="owner">A <see cref="Window"/> that will own the modal dialog box.</param>
        /// <param name="text">The text to display in the message box.</param>
        /// <param name="caption">The text to display in the title bar of the message box.</param>
        /// <param name="onClose">The action which is called when dialog is closed.</param>
        public static void Show(
            Window? owner,
            string? text,
            string? caption,
            MessageBoxResultDelegate? onClose = null)
        {
            ShowCore(
                owner,
                text,
                caption,
                MessageBoxButtons.OK,
                MessageBoxIcon.None,
                MessageBoxDefaultButton.Button1,
                (MessageBoxOptions)0,
                showHelp: false,
                onClose);
        }

        /// <summary>Displays a message box in front of the specified object and with
        /// the specified text.</summary>
        /// <param name="owner">A <see cref="Window"/> that will own the modal dialog box.</param>
        /// <param name="text">The text to display in the message box.</param>
        /// <param name="onClose">The action which is called when dialog is closed.</param>
        public static void Show(
            Window? owner,
            string? text,
            MessageBoxResultDelegate? onClose = null)
        {
            ShowCore(
                owner,
                text,
                string.Empty,
                MessageBoxButtons.OK,
                MessageBoxIcon.None,
                MessageBoxDefaultButton.Button1,
                (MessageBoxOptions)0,
                showHelp: false,
                onClose);
        }

        /// <summary>
        /// Shows message box using the specified parameters.
        /// </summary>
        /// <param name="info">Message box parameters.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">If requested feature is not supported.</exception>
        public static void Show(MessageBoxInfo info)
        {
            if (ShowDialog is not null)
            {
                BaseEventArgs<MessageBoxInfo> args = new(info);

                ShowDialog(null, args);
            }
            else
                ShowDefault(info);
        }

        /// <summary>
        /// Default show message box handler.
        /// </summary>
        /// <param name="info">Message box parameters.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">If requested feature is not supported.</exception>
        public static void ShowDefault(MessageBoxInfo info)
        {
            info.Icon = ValidateIcon(info.Icon);

            var useInternalDialog = UseInternalDialog;

            if (info.Buttons == MessageBoxButtons.AbortRetryIgnore
                || info.Buttons == MessageBoxButtons.RetryCancel)
            {
                useInternalDialog = true;
            }

            switch (info.DefaultButton)
            {
                case MessageBoxDefaultButton.Button1:
                    if (info.Buttons == MessageBoxButtons.OK
                        || info.Buttons == MessageBoxButtons.OKCancel)
                        info.DefaultButton = MessageBoxDefaultButton.OK;
                    if (info.Buttons == MessageBoxButtons.YesNo
                        || info.Buttons == MessageBoxButtons.YesNoCancel)
                        info.DefaultButton = MessageBoxDefaultButton.Yes;
                    break;
                case MessageBoxDefaultButton.Button2:
                    if (info.Buttons == MessageBoxButtons.OK)
                        info.DefaultButton = MessageBoxDefaultButton.OK;
                    else
                    if (info.Buttons == MessageBoxButtons.OKCancel)
                        info.DefaultButton = MessageBoxDefaultButton.Cancel;
                    else
                    if (info.Buttons == MessageBoxButtons.YesNo
                        || info.Buttons == MessageBoxButtons.YesNoCancel)
                        info.DefaultButton = MessageBoxDefaultButton.No;
                    break;
                case MessageBoxDefaultButton.Button3:
                    switch (info.Buttons)
                    {
                        case MessageBoxButtons.OK:
                            info.DefaultButton = MessageBoxDefaultButton.OK;
                            break;
                        case MessageBoxButtons.OKCancel:
                            info.DefaultButton = MessageBoxDefaultButton.OK;
                            break;
                        case MessageBoxButtons.YesNoCancel:
                            info.DefaultButton = MessageBoxDefaultButton.Cancel;
                            break;
                        case MessageBoxButtons.YesNo:
                            info.DefaultButton = MessageBoxDefaultButton.Yes;
                            break;
                    }

                    break;
            }

            if (info.Buttons == MessageBoxButtons.YesNo
                || info.Buttons == MessageBoxButtons.YesNoCancel)
            {
                if (info.DefaultButton == MessageBoxDefaultButton.OK)
                    info.DefaultButton = MessageBoxDefaultButton.Yes;
            }

            ValidateButtons(info.Buttons, info.DefaultButton);

            if (useInternalDialog && !App.IsMaui)
            {
                WindowMessageBox.ShowMessageBox(info);
            }
            else
            {
                DialogFactory.Handler.ShowMessageBox(info);
            }
        }

        internal static MessageBoxIcon ValidateIcon(MessageBoxIcon icon)
        {
            switch (icon)
            {
                case MessageBoxIcon.Question:
                case MessageBoxIcon.Hand:
                case MessageBoxIcon.Exclamation:
                case MessageBoxIcon.Asterisk:
                case MessageBoxIcon.Stop:
                    return MessageBoxIcon.None;
                default:
                    return icon;
            }
        }

        private static void PushHelpInfo(HelpInfo hpi)
        {
            helpInfo ??= new();
            helpInfo.Push(hpi);
        }

        private static void PopHelpInfo()
        {
            helpInfo?.Pop();
        }

        private static void ShowCore(
            Window? owner,
            string? text,
            string? caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton,
            MessageBoxOptions options,
            HelpInfo hpi,
            MessageBoxResultDelegate? onClose)
        {
            try
            {
                PushHelpInfo(hpi);
                ShowCore(
                    owner,
                    text,
                    caption,
                    buttons,
                    icon,
                    defaultButton,
                    options,
                    showHelp: true,
                    onClose);
            }
            finally
            {
                PopHelpInfo();
            }
        }

        private static void ShowCore(
            Window? owner,
            string? text,
            string? caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton,
            MessageBoxOptions options,
            bool showHelp,
            MessageBoxResultDelegate? onClose)
        {
            void HandleOnClose(MessageBoxInfo info)
            {
                onClose?.Invoke(info);

                if (!info.Handled)
                    ShowDefault(info);
            }

            var info = new MessageBoxInfo()
            {
                Owner = owner,
                Text = text,
                Caption = caption,
                Buttons = buttons,
                Icon = icon,
                DefaultButton = defaultButton,
                Options = options,
                ShowHelp = showHelp,
                Result = DialogResult.None,
                HelpInfo = helpInfo?.Peek(),
                OnClose = HandleOnClose,
            };

            Show(info);
        }

        private static void ValidateButtons(
            MessageBoxButtons buttons,
            MessageBoxDefaultButton defaultButton)
        {
        }
    }
}