using System;
using System.ComponentModel;

namespace Alternet.UI
{
    /// <summary>
    /// Implements the basic functionality common to button controls.
    /// </summary>
    public abstract partial class ButtonBase : Control, ICommandSource
    {
        private Action? clickAction;
        private CommandSourceStruct commandSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonBase"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public ButtonBase(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonBase"/> class.
        /// </summary>
        public ButtonBase()
        {
            commandSource = new(this);
            commandSource.Changed = () =>
            {
                Enabled = commandSource.CanExecute;
            };
        }

        /// <inheritdoc/>
        public virtual ICommand? Command
        {
            get
            {
                return commandSource.Command;
            }

            set
            {
                commandSource.Command = value;
            }
        }

        /// <inheritdoc/>
        public virtual object? CommandParameter
        {
            get
            {
                return commandSource.CommandParameter;
            }

            set
            {
                commandSource.CommandParameter = value;
            }
        }

        /// <inheritdoc/>
        [Browsable(false)]
        public virtual object? CommandTarget
        {
            get
            {
                return commandSource.CommandParameter;
            }

            set
            {
                commandSource.CommandParameter = value;
            }
        }

        /// <summary>
        /// Gets or sets the text displayed on this button.
        /// </summary>
        public override string Text
        {
            get
            {
                return base.Text;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                if (Text == value)
                    return;
                base.Text = value;
                PerformLayoutAndInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets <see cref="Action"/> which will be executed when
        /// this control is clicked by the user.
        /// </summary>
        [Browsable(false)]
        public virtual Action? ClickAction
        {
            get => clickAction;
            set
            {
                clickAction = value;
            }
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            commandSource.Changed = null;
            base.DisposeManaged();
        }

        /// <inheritdoc />
        protected override void OnClick(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            base.OnClick(e);
            clickAction?.Invoke();
            commandSource.Execute();
        }
    }
}