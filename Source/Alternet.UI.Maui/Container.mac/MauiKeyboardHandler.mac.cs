using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui.Controls.PlatformConfiguration;

#if IOS || MACCATALYST

using UIKit;

namespace Alternet.UI
{
    /// <summary>
    /// Implements <see cref="IKeyboardHandler"/> for MAUI platform under MacOs.
    /// </summary>
    public class MauiKeyboardHandler : MappedKeyboardHandler<UIKeyboardHidUsage>
    {
        /// <summary>
        /// Gets or sets default <see cref="IKeyboardHandler"/> implementation.
        /// </summary>
        public static MauiKeyboardHandler Default = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="MauiKeyboardHandler"/> class.
        /// </summary>
        public MauiKeyboardHandler()
            : base(UIKeyboardHidUsage.KeyboardRightGui, Key.MaxMaui)
        {
        }

        /// <inheritdoc/>
        public override void RegisterKeyMappings()
        {
        }

        /// <inheritdoc/>
        public override KeyStates GetKeyStatesFromSystem(Key key)
        {
            return KeyStates.None;
        }
    }
}
#endif