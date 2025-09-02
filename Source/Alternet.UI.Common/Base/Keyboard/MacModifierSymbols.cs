using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains Unicode symbols used to represent macOS modifier keys.
    /// </summary>
    public static class MacModifierSymbols
    {
        /// <summary>
        /// Specifies how the macOS-specific symbols should be used in the application.
        /// </summary>
        public static UsageKind Usage = UsageKind.PlatformSpecific;

        /// <summary>
        /// Represents the Command key (⌘). Unicode U+2318.
        /// </summary>
        public static string Command = "\u2318";

        /// <summary>
        /// Represents the Option (Alt) key (⌥). Unicode U+2325.
        /// </summary>
        public static string Option = "\u2325";

        /// <summary>
        /// Represents the Control key (⌃). Unicode U+2303.
        /// </summary>
        public static string Control = "\u2303";

        /// <summary>
        /// Represents the Shift key (⇧). Unicode U+21E7.
        /// </summary>
        public static string Shift = "\u21E7";

        /// <summary>
        /// Represents the Caps Lock key (⇪). Unicode U+21EA.
        /// </summary>
        public static string CapsLock = "\u21EA";

        /// <summary>
        /// Represents the Tab key (⇥). Unicode U+21E5.
        /// </summary>
        public static string Tab = "\u21E5";

        /// <summary>
        /// Represents the Return (Enter) key (⏎). Unicode U+23CE.
        /// </summary>
        public static string Return = "\u23CE";

        /// <summary>
        /// Represents the Delete key (⌫). Unicode U+232B.
        /// </summary>
        public static string Delete = "\u232B";

        /// <summary>
        /// Represents the Escape key (⎋). Unicode U+238B.
        /// </summary>
        public static string Escape = "\u238B";

        /// <summary>
        /// Specifies the usage of macOS-specific symbols in a cross-platform context.
        /// </summary>
        /// <remarks>This enumeration defines the behavior for including macOS-specific
        /// symbols in an application. It allows developers to control whether macOS symbols
        /// are used based on the platform or always excluded.</remarks>
        public enum UsageKind
        {
            /// <summary>
            /// Use macOS symbols only on macOS platform.
            /// </summary>
            PlatformSpecific,

            /// <summary>
            /// Always use macOS symbols regardless of the platform.
            /// </summary>
            Always,

            /// <summary>
            /// Never use macOS symbols.
            /// </summary>
            Never,
        }

        /// <summary>
        /// Indicates whether macOS-specific symbols are used in the application.
        /// This property evaluates depends on the <see cref="Usage"/> setting.
        /// </summary>
        public static bool AreUsed
        {
            get
            {
                return Usage == UsageKind.Always ||
                    (Usage == UsageKind.PlatformSpecific && App.IsMacOS);
            }
        }
    }
}
