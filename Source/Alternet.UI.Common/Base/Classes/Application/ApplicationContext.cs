using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the contextual information about an application thread.
    /// </summary>
    public class ApplicationContext : FrameworkElement
    {
        private Window? mainForm;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationContext" /> class
        /// with no context.</summary>
        public ApplicationContext()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationContext" /> class with the
        /// specified <see cref="Window" />.</summary>
        /// <param name="mainForm">
        /// The main <see cref="Window" /> of the application to use for context.
        /// </param>
        public ApplicationContext(Window? mainForm)
        {
            MainForm = mainForm;
        }

        /// <summary>
        /// Occurs when the message loop of the thread should be terminated, by calling
        /// <see cref="ApplicationContext.ExitThread" />.
        /// </summary>
        public event EventHandler? ThreadExit;

        /// <summary>
        /// Gets or sets the <see cref="Window" /> to use as context.
        /// </summary>
        /// <returns>
        /// The <see cref="Window" /> to use as context.
        /// </returns>
        public Window? MainForm
        {
            get
            {
                return mainForm;
            }

            set
            {
                EventHandler value2 = OnMainFormDestroy;
                if (mainForm != null)
                {
                    mainForm.HandleDestroyed -= value2;
                }

                mainForm = value;
                if (mainForm != null)
                {
                    mainForm.HandleDestroyed += value2;
                }
            }
        }

        /// <summary>
        /// Terminates the message loop of the thread.
        /// </summary>
        public void ExitThread()
        {
            ExitThreadCore();
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            SafeDisposeObject(ref mainForm);
        }

        /// <summary>
        /// Terminates the message loop of the thread.
        /// </summary>
        protected virtual void ExitThreadCore()
        {
            ThreadExit?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Calls <see cref="ExitThreadCore" />, which raises
        /// the <see cref="ThreadExit" /> event.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> that contains the event data.</param>
        protected virtual void OnMainFormClosed(object? sender, EventArgs e)
        {
            ExitThreadCore();
        }

        private void OnMainFormDestroy(object? sender, EventArgs e)
        {
            if (sender is not Window form)
                return;
            if (!form.RecreatingHandle)
            {
                form.HandleDestroyed -= OnMainFormDestroy;
                OnMainFormClosed(sender, e);
            }
        }
    }
}
