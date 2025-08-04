using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Panel with Ok, Cancel and Apply buttons.
    /// </summary>
    [ControlCategory("Containers")]
    public partial class PanelOkCancelButtons : StackPanel
    {
        private Button? applyButton;
        private Button? cancelButton;
        private Button? okButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="PanelOkCancelButtons"/> class.
        /// This constructor creates a panel with 'Ok' and 'Cancel' buttons.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public PanelOkCancelButtons(Control parent)
            : this(CreateParameters.Default)
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PanelOkCancelButtons"/> class.
        /// This constructor creates a panel with 'Ok' and 'Cancel' buttons.
        /// </summary>
        public PanelOkCancelButtons()
            : this(CreateParameters.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PanelOkCancelButtons"/>
        /// class with options to automatically
        /// create standard buttons.
        /// </summary>
        /// <param name="autoCreateButtons">A value indicating whether
        /// the 'Ok' and 'Cancel' buttons should be automatically created.
        /// Specify <see langword="true"/> to create the buttons; otherwise,
        /// <see langword="false"/>.</param>
        public PanelOkCancelButtons(bool autoCreateButtons)
            : this(new CreateParameters
            {
                IsOkButtonCreated = autoCreateButtons,
                IsCancelButtonCreated = autoCreateButtons,
                IsApplyButtonCreated = false,
            })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PanelOkCancelButtons"/> class
        /// with specified parameters.
        /// </summary>
        /// <remarks>This constructor sets up a horizontal stack panel with buttons
        /// aligned to the right and centered vertically.
        /// It optionally creates the 'Ok', 'Cancel', and 'Apply' buttons based on
        /// the provided parameters.</remarks>
        /// <param name="prm">The parameters used to determine which buttons
        /// to create and configure.</param>
        public PanelOkCancelButtons(CreateParameters prm)
        {
            SuspendLayout();
            try
            {
                Orientation = StackPanelOrientation.Horizontal;
                HorizontalAlignment = UI.HorizontalAlignment.Right;
                VerticalAlignment = UI.VerticalAlignment.Center;

                if (prm.IsOkButtonCreated)
                    OkButton.Required();
                if (prm.IsCancelButtonCreated)
                    CancelButton.Required();
                if (prm.IsApplyButtonCreated)
                    ApplyButton.Required();
            }
            finally
            {
                ResumeLayout();
            }
        }

        /// <summary>
        /// Occurs when the 'Ok' button is clicked.
        /// </summary>
        /// <remarks>Subscribe to this event to handle the 'Ok' button click action.
        /// Ensure that any event
        /// handlers attached to this event are properly detached when no longer
        /// needed to prevent memory leaks.</remarks>
        public event EventHandler? OkButtonClick;

        /// <summary>
        /// Occurs when the 'Cancel' button is clicked.
        /// </summary>
        /// <remarks>Subscribe to this event to handle the 'Cancel' button click action.
        /// Ensure that any event
        /// handlers attached to this event are properly detached when no longer
        /// needed to prevent memory leaks.</remarks>
        public event EventHandler? CancelButtonClick;

        /// <summary>
        /// Occurs when the 'Apply' button is clicked.
        /// </summary>
        /// <remarks>Subscribe to this event to handle the 'Apply' button click action.
        /// Ensure that any event
        /// handlers attached to this event are properly detached when no longer
        /// needed to prevent memory leaks.</remarks>
        public event EventHandler? ApplyButtonClick;

        /// <summary>
        /// Occurs when any button is clicked.
        /// </summary>
        public event EventHandler? ButtonClicked;

        /// <summary>
        /// Gets or sets default margin for the buttons.
        /// </summary>
        public static Thickness DefaultButtonMargin { get; set; } = 5;

        /// <summary>
        /// Gets or sets an action to be executed when a button is clicked.
        /// </summary>
        [Browsable(false)]
        public Action? ButtonClickedAction { get; set; }

        /// <summary>
        /// Gets 'Ok' button.
        /// </summary>
        [Browsable(false)]
        public Button OkButton
        {
            get
            {
                if (okButton is null)
                {
                    okButton = CreateButton(CommonStrings.Default.ButtonOk);
                    okButton.IsDefault = true;
                    okButton.Parent = this;
                    okButton.Click += HandleOkButtonClick;
                    okButton.CustomAttr.SetAttribute("KnownButton", KnownButton.OK);
                }

                return okButton;
            }
        }

        /// <summary>
        /// Gets 'Cancel' button.
        /// </summary>
        [Browsable(false)]
        public Button CancelButton
        {
            get
            {
                if (cancelButton is null)
                {
                    cancelButton = CreateButton(CommonStrings.Default.ButtonCancel);
                    cancelButton.IsCancel = true;
                    cancelButton.Parent = this;
                    cancelButton.Click += HandleCancelButtonClick;
                    cancelButton.CustomAttr.SetAttribute("KnownButton", KnownButton.Cancel);
                }

                return cancelButton;
            }
        }

        /// <summary>
        /// Gets 'Apply' button.
        /// </summary>
        [Browsable(false)]
        public Button ApplyButton
        {
            get
            {
                if (applyButton is null)
                {
                    applyButton = CreateButton(CommonStrings.Default.ButtonApply);
                    applyButton.Visible = false;
                    applyButton.Parent = this;
                    applyButton.Click += HandleApplyButtonClick;
                    applyButton.CustomAttr.SetAttribute("KnownButton", KnownButton.Apply);
                }

                return applyButton;
            }
        }

        /// <summary>
        /// Gets or sets whether 'Apply' button is visible.
        /// </summary>
        public virtual bool ShowApplyButton
        {
            get
            {
                return applyButton is not null && applyButton.Visible;
            }

            set
            {
                if (ShowApplyButton == value)
                    return;
                ApplyButton.Visible = value;
            }
        }

        /// <summary>
        /// Gets or sets whether 'Cancel' button is visible.
        /// </summary>
        public virtual bool ShowCancelButton
        {
            get
            {
                return cancelButton is not null && cancelButton.Visible;
            }

            set
            {
                if (ShowCancelButton == value)
                    return;
                CancelButton.Visible = value;
            }
        }

        /// <summary>
        /// Gets or sets whether 'Ok' button is visible.
        /// </summary>
        public virtual bool ShowOkButton
        {
            get
            {
                return okButton is not null && okButton.Visible;
            }

            set
            {
                if (ShowOkButton == value)
                    return;
                OkButton.Visible = value;
            }
        }

        /// <summary>
        /// Gets an enumerable collection of buttons that are currently visible.
        /// </summary>
        [Browsable(false)]
        public virtual IEnumerable<Button> VisibleButtons
        {
            get
            {
                foreach (var child in Children)
                {
                    if (child is Button btn && btn.IsVisible)
                        yield return btn;
                }
            }
        }

        /// <summary>
        /// Gets the number of buttons that are currently visible.
        /// </summary>
        [Browsable(false)]
        public virtual int VisibleButtonCount
        {
            get
            {
                return VisibleButtons.Count();
            }
        }

        /// <summary>
        /// Gets or sets whether 'Ok' and 'Cancel' buttons change
        /// <see cref="Window.ModalResult"/> when they are clicked.
        /// Default is <c>false</c>.
        /// </summary>
        public virtual bool UseModalResult { get; set; }

        /// <summary>
        /// Gets the last button that was clicked, if any.
        /// </summary>
        [Browsable(false)]
        public KnownButton? LastButtonClicked { get; private set; }

        /// <summary>
        /// Gets the <see cref="DialogResult"/> corresponding to the last button clicked.
        /// </summary>
        public virtual DialogResult LastClickedAsDialogResult =>
            DialogFactory.ToDialogResult(LastButtonClicked);

        [Browsable(false)]
        internal new string Text
        {
            get => base.Text;
            set => base.Text = value;
        }

        /// <summary>
        /// Creates a <see cref="PanelOkCancelButtons"/> instance with the specified
        /// button configuration.
        /// </summary>
        /// <param name="buttons">The <see cref="MessageBoxButtons"/> enumeration value
        /// that specifies the buttons to include.</param>
        /// <returns>A <see cref="PanelOkCancelButtons"/> configured with
        /// the specified buttons.</returns>
        public static PanelOkCancelButtons CreateWithButtons(MessageBoxButtons buttons)
        {
            var btn = DialogFactory.ConvertButtons(buttons);
            return CreateWithButtons(btn);
        }

        /// <summary>
        /// Creates a <see cref="PanelOkCancelButtons"/> instance with specified buttons.
        /// </summary>
        /// <remarks>The method initializes a panel with OK, Apply, and Cancel buttons based on the
        /// presence of these values in the <paramref name="buttons"/> array.
        /// Additional buttons specified in the array
        /// are also added to the panel.</remarks>
        /// <param name="buttons">An array of <see cref="KnownButton"/> values indicating
        /// which buttons to include in the panel. The panel
        /// will automatically include OK, Apply, and Cancel buttons if specified.</param>
        /// <returns>A <see cref="PanelOkCancelButtons"/> instance containing
        /// the specified buttons.</returns>
        public static PanelOkCancelButtons CreateWithButtons(params KnownButton[] buttons)
        {
            var prm = new CreateParameters();
            prm.IsOkButtonCreated = buttons.Contains(KnownButton.OK);
            prm.IsApplyButtonCreated = buttons.Contains(KnownButton.Apply);
            prm.IsCancelButtonCreated = buttons.Contains(KnownButton.Cancel);

            var panel = new PanelOkCancelButtons(prm);

            panel.DoInsideLayout(() =>
            {
                foreach (var button in buttons)
                {
                    if (button == KnownButton.OK
                        || button == KnownButton.Apply || button == KnownButton.Cancel)
                        continue;
                    panel.AddButton(button);
                }
            });

            return panel;
        }

        /// <summary>
        /// Gets the default button from the collection of child buttons.
        /// </summary>
        /// <returns>The default button if found; otherwise, null.</returns>
        public virtual Button? GetDefaultButton()
        {
            foreach (var child in Children)
            {
                if (child is Button btn && btn.IsDefault && btn.IsVisible)
                    return btn;
            }

            return null;
        }

        /// <summary>
        /// Sets the specified button as the default button, ensuring that no other buttons
        /// are marked as default.
        /// </summary>
        /// <param name="button">The button to set as the default.
        /// If <see langword="null"/>, all buttons will be unset as default.</param>
        public virtual void SetDefaultButtonExclusive(Button? button)
        {
            foreach (var child in Children)
            {
                if (child is Button btn)
                {
                    btn.IsDefault = btn == button;
                }
            }
        }

        /// <summary>
        /// Sets the specified button as the exclusive cancel button within the collection
        /// of child buttons.
        /// </summary>
        /// <remarks>This method iterates through all child elements and sets the
        /// <see cref="Button.IsCancel"/> property to <see langword="true"/> for
        /// the specified button, and <see langword="false"/> for all other buttons.
        /// Only one button can be designated as the cancel button at a time.</remarks>
        /// <param name="button">The button to be set as the exclusive cancel button.
        /// If <see langword="null"/>, no button will be set as the
        /// cancel button.</param>
        public virtual void SetCancelButtonExclusive(Button? button)
        {
            foreach (var child in Children)
            {
                if (child is Button btn)
                {
                    btn.IsCancel = btn == button;
                }
            }
        }

        /// <summary>
        /// Sets the specified button as the exclusive cancel button.
        /// </summary>
        /// <remarks>If the specified button is not found, the method performs no action.</remarks>
        /// <param name="value">The button to be set as the exclusive cancel button.</param>
        public virtual void SetCancelButtonExclusive(KnownButton value)
        {
            var button = GetButton(value);
            if (button is null)
                return;
            SetCancelButtonExclusive(button);
        }

        /// <summary>
        /// Retrieves the button at the specified index from the collection of visible buttons.
        /// </summary>
        /// <remarks>This method returns <see langword="null"/> if the specified index
        /// is out of range of the visible buttons.</remarks>
        /// <param name="index">The zero-based index of the button to retrieve.
        /// Must be within the range of visible buttons.</param>
        /// <returns>The <see cref="Button"/> at the specified index if it exists;
        /// otherwise, <see langword="null"/>.</returns>
        public virtual Button? GetVisibleButton(int index)
        {
            var visibleButtons = VisibleButtons.ToArray();

            if (index < 0 || index >= visibleButtons.Length)
                return null;
            var vis = visibleButtons[index];
            return vis;
        }

        /// <summary>
        /// Sets the specified button as the default button, ensuring it is the only default button.
        /// </summary>
        /// <param name="index">The zero-based index of the button to set as the default.</param>
        /// <returns><see langword="true"/> if the button was successfully
        /// set as the default;  otherwise, <see
        /// langword="false"/> if the button is not visible or does not exist. </returns>
        public virtual bool SetDefaultButtonExclusive(int index)
        {
            var button = GetVisibleButton(index);
            if (button is null)
                return false;
            SetDefaultButtonExclusive(button);
            return true;
        }

        /// <summary>
        /// Sets the specified button as the default button, ensuring it is the only default button.
        /// </summary>
        /// <param name="value">The button to set as the default, identified
        /// by a <see cref="KnownButton"/> value.</param>
        /// <returns><see langword="true"/> if the button was successfully
        /// set as the default; otherwise, <see langword="false"/>
        /// if the button could not be found.</returns>
        public virtual bool SetDefaultButtonExclusive(KnownButton value)
        {
            var button = GetButton(value);
            if (button is null)
                return false;
            SetDefaultButtonExclusive(button);
            return true;
        }

        /// <summary>
        /// Sets the default button for the panel exclusively based on the specified button value.
        /// </summary>
        /// <remarks>This method maps the specified <see cref="MessageBoxDefaultButton"/>
        /// to an internal representation and sets it as the default button.
        /// If the specified button value is not recognized, the
        /// method returns <see langword="false"/>.</remarks>
        /// <param name="value">The <see cref="MessageBoxDefaultButton"/> value representing
        /// the button to set as default.</param>
        /// <returns><see langword="true"/> if the default button was set successfully;
        /// otherwise, <see langword="false"/>.</returns>
        public virtual bool SetDefaultButtonExclusive(MessageBoxDefaultButton value)
        {
            switch(value)
            {
                case MessageBoxDefaultButton.Button1:
                    return SetDefaultButtonExclusive(0);
                case MessageBoxDefaultButton.Button2:
                    return SetDefaultButtonExclusive(1);
                case MessageBoxDefaultButton.Button3:
                    return SetDefaultButtonExclusive(2);
                case MessageBoxDefaultButton.Cancel:
                    return SetDefaultButtonExclusive(KnownButton.Cancel);
                case MessageBoxDefaultButton.Yes:
                    return SetDefaultButtonExclusive(KnownButton.Yes);
                case MessageBoxDefaultButton.No:
                    return SetDefaultButtonExclusive(KnownButton.No);
                default:
                    return false;
            }
        }

        /// <summary>
        /// Sets the cancel button to be exclusive among the specified known buttons.
        /// </summary>
        /// <remarks>This method ensures that only one of the specified buttons
        /// is set as the cancel
        /// button, based on a predefined priority order.</remarks>
        /// <param name="buttons">An array of <see cref="KnownButton"/> values representing
        /// the buttons to consider for exclusivity. The
        /// method prioritizes <see cref="KnownButton.Cancel"/>, followed
        /// by <see cref="KnownButton.No"/>, and then <see cref="KnownButton.Close"/>.</param>
        public virtual void TrySetCancelButtonExclusive(KnownButton[] buttons)
        {
            var btn = Internal();

            if(btn is not null)
            {
                SetCancelButtonExclusive(btn.Value);
            }
            else
            {
                SetCancelButtonExclusive(null);
            }

            KnownButton? Internal()
            {
                if(buttons.Length == 1)
                {
                    return buttons[0];
                }

                if (buttons.Contains(KnownButton.Cancel))
                    return KnownButton.Cancel;
                if (buttons.Contains(KnownButton.No))
                    return KnownButton.No;
                if (buttons.Contains(KnownButton.Close))
                    return KnownButton.Close;
                return null;
            }
        }

        /// <summary>
        /// Resets the text of all button controls within the panel to their default values.
        /// </summary>
        /// <remarks>This method iterates over all child elements and updates the text of
        /// each button based on its associated known button type.
        /// </remarks>
        public virtual void ResetButtonsText()
        {
            DoInsideLayout(() =>
            {
                for (int i = Children.Count - 1; i >= 0; i--)
                {
                    var child = Children[i];
                    if (child is not Button btn)
                        continue;
                    var value = btn.CustomAttr.GetAttribute("KnownButton");

                    if (value is not KnownButton knownButton)
                        continue;
                    btn.Text = KnownButtons.GetText(knownButton) ?? knownButton.ToString();
                }
            });
        }

        /// <summary>
        /// Sets the text of a specified button.
        /// </summary>
        /// <remarks>
        /// This method attempts to set the text of the specified button, identified by
        /// the <paramref name="button"/> parameter.
        /// If the button is not found in the panel, the method returns <see langword="false"/>
        /// and no changes are made.
        /// If the <paramref name="text"/> parameter is <see langword="null"/> or empty,
        /// the button's text is set to the default text
        /// associated with the <see cref="KnownButton"/> value, as provided
        /// by <c>KnownButtons.GetText</c>.
        /// Otherwise, the button's text is set to the specified <paramref name="text"/> value.
        /// </remarks>
        /// <param name="button">The button whose text is to be set.</param>
        /// <param name="text">The new text to display on the button.</param>
        /// <returns><see langword="true"/> if the button text was successfully set;
        /// otherwise, <see langword="false"/>.</returns>
        public virtual bool SetButtonText(KnownButton button, string? text)
        {
            var btn = GetButton(button);
            if (btn is null)
                return false;

            string? s;

            if (string.IsNullOrEmpty(text))
                s = KnownButtons.GetText(button) ?? string.Empty;
            else
                s = text;

            btn.Text = s ?? button.ToString();

            return true;
        }

        /// <summary>
        /// Configures the visibility of buttons based on the specified set of known buttons.
        /// </summary>
        /// <remarks>This method updates the visibility of predefined buttons such
        /// as 'Ok', 'Apply', and 'Cancel', as well as any additional buttons specified in
        /// the <paramref name="buttons"/> array. Buttons not
        /// included in the array will be hidden.</remarks>
        /// <param name="buttons">An array of <see cref="KnownButton"/> values representing
        /// the buttons to be displayed. This parameter cannot
        /// be <see langword="null"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="buttons"/>
        /// is <see langword="null"/>.</exception>
        public virtual void SetButtons(params KnownButton[] buttons)
        {
            if (buttons is null)
                throw new ArgumentNullException(nameof(buttons));

            DoInsideLayout(() =>
            {
                var applyButtonIndex = Array.IndexOf(buttons, KnownButton.Apply);
                var cancelButtonIndex = Array.IndexOf(buttons, KnownButton.Cancel);

                ShowOkButton = buttons.Contains(KnownButton.OK);
                ShowApplyButton = applyButtonIndex >= 0;
                ShowCancelButton = cancelButtonIndex >= 0;

                foreach (var button in buttons)
                {
                    if (button == KnownButton.OK
                        || button == KnownButton.Apply || button == KnownButton.Cancel)
                        continue;

                    var existingButton = GetButton(button);

                    if (existingButton is null)
                        AddButton(button);
                    else
                    {
                        existingButton.Visible = true;
                    }
                }

                for (int i = Children.Count - 1; i >= 0; i--)
                {
                    var child = Children[i];
                    if (child is not Button btn)
                        continue;
                    var knownButton = btn.CustomAttr.GetAttribute<KnownButton>("KnownButton");

                    if (!buttons.Contains(knownButton))
                    {
                        child.Visible = false;
                    }
                }

                SortChildren(Comparison);

                int Comparison(AbstractControl x, AbstractControl y)
                {
                    if (x is Button btnX && y is Button btnY)
                    {
                        var knownButtonX = btnX.CustomAttr.GetAttribute<KnownButton>("KnownButton");
                        var knownButtonY = btnY.CustomAttr.GetAttribute<KnownButton>("KnownButton");

                        var indexX = Array.IndexOf(buttons, knownButtonX);
                        var indexY = Array.IndexOf(buttons, knownButtonY);

                        return indexX.CompareTo(indexY);
                    }

                    return 0;
                }
            });
        }

        /// <summary>
        /// Sets the specified button as the exclusive cancel button for the panel.
        /// </summary>
        /// <remarks>This method converts the provided <see cref="MessageBoxButtons"/>
        /// to a format suitable for setting the cancel button exclusively.
        /// It is intended for use when a specific button should be
        /// designated as the cancel action in a panel.</remarks>
        /// <param name="buttons">The buttons to be converted and set as
        /// the exclusive cancel button.</param>
        public virtual void SetCancelButtonExclusive(MessageBoxButtons buttons)
        {
            var convertedButtons = DialogFactory.ConvertButtons(buttons);
            TrySetCancelButtonExclusive(convertedButtons);
        }

        /// <summary>
        /// Configures the panel with the specified button layout.
        /// </summary>
        /// <remarks>This method converts the specified <paramref name="buttons"/>
        /// to a format suitable
        /// for the panel and applies the configuration.</remarks>
        /// <param name="buttons">The <see cref="MessageBoxButtons"/> enumeration value that
        /// specifies the button layout to display.</param>
        public virtual void SetButtons(MessageBoxButtons buttons)
        {
            var convertedButtons = DialogFactory.ConvertButtons(buttons);
            SetButtons(convertedButtons);
        }

        /// <summary>
        /// Retrieves a button from the collection of children that matches the
        /// specified known button identifier.
        /// </summary>
        /// <param name="button">The identifier of the known button to search for.</param>
        /// <returns>A <see cref="Button"/> object that matches the specified identifier,
        /// or <see langword="null"/> if no matching button is found.</returns>
        public virtual Button? GetButton(KnownButton button)
        {
            foreach (var child in Children)
            {
                if (child is Button btn
                    && btn.CustomAttr.GetAttribute<KnownButton>("KnownButton") == button)
                    return btn;
            }

            return null;
        }

        /// <summary>
        /// Adds a new button with the specified text and click action to the panel.
        /// </summary>
        /// <param name="text">The text to display on the button. Cannot be null or empty.</param>
        /// <param name="clickAction">The action to execute when the button is clicked.
        /// Can be null.</param>
        /// <returns>A <see cref="Button"/> instance representing the newly added button.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="text"/>
        /// is null or empty.</exception>
        public virtual Button AddButton(string text, Action? clickAction = null)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentException("Button text cannot be null or empty.", nameof(text));
            Button button = CreateButton(text);

            if (clickAction is not null)
            {
                button.Click += (sender, e) => clickAction.Invoke();
            }

            button.Parent = this;
            return button;
        }

        /// <summary>
        /// Adds a new button to the current container with the specified behavior.
        /// </summary>
        /// <remarks>The button's text is determined by the specified
        /// <paramref name="button"/> type. The
        /// <paramref name="clickAction"/> is invoked whenever the button is clicked.
        /// The new button is added as a child to the panel.</remarks>
        /// <param name="button">A <see cref="KnownButton"/> value that specifies
        /// the type of button to add.</param>
        /// <param name="clickAction">An <see cref="Action"/> to be executed when
        /// the button is clicked.</param>
        /// <returns>A <see cref="Button"/> instance representing the newly added button.</returns>
        public virtual Button AddButton(KnownButton button, Action? clickAction = null)
        {
            var text = KnownButtons.GetText(button) ?? string.Empty;
            Button newButton = AddButton(text, clickAction);

            newButton.Click += (sender, e) =>
            {
                OnButtonClicked(button);
            };

            newButton.CustomAttr.SetAttribute("KnownButton", button);
            return newButton;
        }

        /// <summary>
        /// Called when 'Cancel' button is clicked.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void HandleCancelButtonClick(object? sender, EventArgs e)
        {
            OnButtonClicked(KnownButton.Cancel);
            CancelButtonClick?.Invoke(this, e);
        }

        /// <summary>
        /// Called when 'Ok' button is clicked.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void HandleOkButtonClick(object? sender, EventArgs e)
        {
            OnButtonClicked(KnownButton.OK);
            OkButtonClick?.Invoke(this, e);
        }

        /// <summary>
        /// Called when 'Apply' button is clicked.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void HandleApplyButtonClick(object? sender, EventArgs e)
        {
            OnButtonClicked(KnownButton.Apply);
            ApplyButtonClick?.Invoke(this, e);
        }

        /// <summary>
        /// Invoked when a button is clicked.
        /// </summary>
        /// <remarks>This method is called to handle button click events. Override this method in a
        /// derived class to provide custom handling for button clicks.</remarks>
        /// <param name="button">The button that was clicked, or <see langword="null"/>
        /// if no specific button is associated with the event.</param>
        protected virtual void OnButtonClicked(KnownButton? button)
        {
            LastButtonClicked = button;

            if(button == KnownButton.Cancel)
                ApplyModalResult(ModalResult.Canceled);
            else
                ApplyModalResult(ModalResult.Accepted);

            ButtonClicked?.Invoke(this, EventArgs.Empty);
            ButtonClickedAction?.Invoke();
        }

        /// <summary>
        /// Creates a button with the specified text.
        /// Override this method to customize button creation.
        /// </summary>
        /// <param name="text">The text for the button.</param>
        /// <returns>A <c>Button</c> instance.</returns>
        protected virtual Button CreateButton(string text)
        {
            Button result = new()
            {
                Text = text,
                Margin = DefaultButtonMargin,
                VerticalAlignment = UI.VerticalAlignment.Center,
                HorizontalAlignment = UI.HorizontalAlignment.Right,
            };

            return result;
        }

        /// <summary>
        /// Applies the modal result to the parent window if it is a dialog.
        /// </summary>
        /// <param name="modalResult">The modal result to apply.</param>
        protected virtual void ApplyModalResult(ModalResult modalResult)
        {
            if (!UseModalResult)
                return;
            if (ParentWindow is not DialogWindow dialog)
                return;
            dialog.ModalResult = modalResult;
        }

        /// <summary>
        /// Represents the parameters used to configure the creation of the panel.
        /// </summary>
        /// <remarks>This structure allows to specify which standard dialog
        /// buttons ("OK", "Cancel", "Apply") should be created.</remarks>
        public struct CreateParameters
        {
            /// <summary>
            /// Gets or sets a value indicating whether an "OK" button should be created.
            /// </summary>
            public bool IsOkButtonCreated = true;

            /// <summary>
            /// Gets or sets a value indicating whether a "Cancel" button should be created.
            /// </summary>
            public bool IsCancelButtonCreated = true;

            /// <summary>
            /// Gets or sets a value indicating whether the "Apply" button should be created.
            /// </summary>
            public bool IsApplyButtonCreated;

            internal static readonly CreateParameters Default = new();

            /// <summary>
            /// Initializes a new instance of the <see cref="CreateParameters"/> class.
            /// </summary>
            public CreateParameters()
            {
            }
        }
    }
}
