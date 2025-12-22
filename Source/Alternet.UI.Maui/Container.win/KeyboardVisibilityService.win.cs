using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

#if WINDOWS
using Windows.UI.ViewManagement;

namespace Alternet.Maui
{
    /// <summary>
    /// Provides functionality to monitor the visibility of the on-screen keyboard on Windows platforms.
    /// </summary>
    public partial class KeyboardVisibilityService : DisposableObject, IKeyboardVisibilityService, IDisposable
    {
        /// <inheritdoc/>
        public event EventHandler<KeyboardVisibleChangedEventArgs>? KeyboardVisibleChanged;

        /// <inheritdoc/>
        public bool IsVisible { get; private set; }

        /// <inheritdoc/>
        public double Height { get; private set; }

        private readonly InputPane? pane;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardVisibilityService"/> class.
        /// </summary>
        public KeyboardVisibilityService()
        {
            try
            {
                pane = InputPane.GetForCurrentView();
                pane.Showing += Pane_Showing;
                pane.Hiding += Pane_Hiding;
            }
            catch
            {
            }
        }

        /// <summary>
        /// Raises the <see cref="KeyboardVisibleChanged"/> event with the specified event arguments.
        /// </summary>
        /// <param name="e">The event arguments. Optional. If not specified,
        /// defaults to the current keyboard visibility state.</param>
        public virtual void RaiseKeyboardVisibleChanged(KeyboardVisibleChangedEventArgs? e)
        {
            KeyboardVisibleChanged?.Invoke(this, e ?? new KeyboardVisibleChangedEventArgs(IsVisible, Height));
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            if (pane != null)
            {
                pane.Showing -= Pane_Showing;
                pane.Hiding -= Pane_Hiding;
            }

            base.DisposeManaged();
        }

        private void Pane_Showing(InputPane sender, InputPaneVisibilityEventArgs args)
        {
            IsVisible = true;
            // OccludedRect.Height is in effective pixels; convert to DIPs if necessary
            var height = sender.OccludedRect.Height;
            Height = height;
            KeyboardVisibleChanged?.Invoke(this, new KeyboardVisibleChangedEventArgs(true, height));
        }

        private void Pane_Hiding(InputPane sender, InputPaneVisibilityEventArgs args)
        {
            IsVisible = false;
            Height = 0;
            KeyboardVisibleChanged?.Invoke(this, new KeyboardVisibleChangedEventArgs(false, 0));
        }
    }
}

#endif
