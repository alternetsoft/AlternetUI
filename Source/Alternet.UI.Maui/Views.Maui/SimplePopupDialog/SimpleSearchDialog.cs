using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Alternet.Maui
{
    /// <summary>
    /// Represents a dialog for performing a simple search and replace operations.
    /// </summary>
    /// <remarks>This dialog is designed to collect user input for a search operation and replace
    /// operations. It inherits functionality from <see cref="SimpleTwoFieldInputDialog"/>.</remarks>
    public partial class SimpleSearchDialog : SimpleTwoFieldInputDialog
    {
        private readonly SimpleToolBarView.IToolBarItem firstButton;
        private readonly SimpleToolBarView.IToolBarItem secondButton;
        private readonly SimpleToolBarView.IToolBarItem toggleReplaceButton;

        private CheckBoxWithLabelView? caseSensitiveCheckBox;
        private CheckBoxWithLabelView? wholeWordsCheckBox;
        private CheckBoxWithLabelView? regularExpressionsCheckBox;
        private CheckBoxWithLabelView? selectionOnlyCheckBox;
        private VerticalStackLayout? settingsLayout;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleSearchDialog"/> class.
        /// </summary>
        public SimpleSearchDialog()
        {
            SecondLabel.IsVisible = false;
            SecondEntryBorder.IsVisible = false;

            var strings = UI.Localization.CommonStrings.Default;

            Title = strings.WindowTitleSearch;
            Message = strings.SearchFor;
            SecondMessage = strings.ReplaceWith;
            Placeholder = string.Empty;
            SecondPlaceholder = string.Empty;

            firstButton = Buttons.AddDialogButton(strings.ButtonFindPrevious, null, null, () =>
            {
                OnFirstButtonClicked();
            });

            secondButton = Buttons.AddDialogButton(strings.ButtonFindNext, null, null, () =>
            {
                OnSecondButtonClicked();
            });

            AddCancelButton();

            DialogTitle.AddGearButton(Alternet.UI.KnownSvgImages.ImgMoreActions);
            DialogTitle.GearButtonClicked += (s, e) =>
            {
                if (settingsLayout is null)
                    return;
                settingsLayout.IsVisible = !settingsLayout.IsVisible;
            };

            toggleReplaceButton = DialogTitle.InsertButton(
                0,
                null,
                Alternet.UI.Localization.CommonStrings.Default.ToggleToSwitchBetweenFindReplace,
                KnownSvgImages.ImgAngleDown,
                () =>
                {
                    IsReplaceVisible = !IsReplaceVisible;
                });
        }

        /// <summary>
        /// Gets the layout container for settings controls.
        /// </summary>
        public VerticalStackLayout SettingsLayout => settingsLayout!;

        /// <summary>
        /// Gets a value indicating whether the current operation or comparison is case-sensitive.
        /// </summary>
        public virtual bool IsCaseSensitive
        {
            get
            {
                return caseSensitiveCheckBox!.IsChecked;
            }

            set
            {
                caseSensitiveCheckBox!.IsChecked = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the search is restricted to whole words.
        /// </summary>
        public virtual bool IsWholeWords
        {
            get
            {
                return wholeWordsCheckBox!.IsChecked;
            }

            set
            {
                wholeWordsCheckBox!.IsChecked = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the search string is treated as a regular expression.
        /// </summary>
        public virtual bool IsRegularExpression
        {
            get
            {
                return regularExpressionsCheckBox!.IsChecked;
            }

            set
            {
                regularExpressionsCheckBox!.IsChecked = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current operation is limited to the selected text.
        /// </summary>
        public virtual bool IsSelectionOnly
        {
            get
            {
                return selectionOnlyCheckBox!.IsChecked;
            }

            set
            {
                selectionOnlyCheckBox!.IsChecked = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the selection-only mode is enabled.
        /// </summary>
        public virtual bool IsSelectionOnlyEnabled
        {
            get
            {
                return selectionOnlyCheckBox!.IsEnabled;
            }

            set
            {
                selectionOnlyCheckBox!.IsEnabled = value;
            }
        }

        /// <summary>
        /// Gets the first button in the dialog.
        /// </summary>
        public SimpleToolBarView.IToolBarItem FirstButton => firstButton;

        /// <summary>
        /// Gets the second button in the dialog.
        /// </summary>
        public SimpleToolBarView.IToolBarItem SecondButton => secondButton;

        /// <summary>
        /// Occurs when the "Replace" button is clicked.
        /// </summary>
        /// <remarks>This event is typically used to handle actions triggered by the
        /// "Replace" button in a user interface. Subscribers can use this event to
        /// perform operations such as replacing text or updating
        /// data.</remarks>
        public virtual Action? ReplaceButtonClicked { get; set; }

        /// <summary>
        /// Occurs when the "Replace All" button is clicked.
        /// </summary>
        /// <remarks>This event is typically used to handle actions triggered by the
        /// "Replace All" button in a user interface. Subscribers can use this event to
        /// perform operations such as replacing text or updating
        /// data.</remarks>
        public virtual Action? ReplaceAllButtonClicked { get; set; }

        /// <summary>
        /// Gets or sets the action to be invoked when settings are changed.
        /// </summary>
        public virtual Action? SettingsChanged { get; set; }

        /// <summary>
        /// Occurs when the "Find Next" button is clicked.
        /// </summary>
        /// <remarks>Subscribe to this event to handle the action triggered
        /// by the "Find Next" button. The
        /// event provides no additional data beyond the sender and event arguments.</remarks>
        public virtual Action? FindNextButtonClicked { get; set; }

        /// <summary>
        /// Occurs when the "Find Previous" button is clicked.
        /// </summary>
        /// <remarks>Subscribe to this event to handle the action triggered
        /// by the "Find Previous" button. The
        /// event provides no additional data beyond the sender and event arguments.</remarks>
        public virtual Action? FindPreviousButtonClicked { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the replace functionality is visible.
        /// </summary>
        public virtual bool IsReplaceVisible
        {
            get
            {
                return SecondLabel.IsVisible;
            }

            set
            {
                if (IsReplaceVisible == value)
                    return;
                SecondLabel.IsVisible = value;
                SecondEntryBorder.IsVisible = value;

                var strings = UI.Localization.CommonStrings.Default;

                Title = value ? strings.WindowTitleReplace : strings.WindowTitleSearch;

                firstButton.Text = value ? strings.ButtonReplace : strings.ButtonFindPrevious;
                secondButton.Text = value ? strings.ButtonReplaceAll : strings.ButtonFindNext;

                if (toggleReplaceButton is not null)
                {
                    toggleReplaceButton.SvgImage = IsReplaceVisible
                        ? KnownSvgImages.ImgAngleUp : KnownSvgImages.ImgAngleDown;
                }
            }
        }

        /// <inheritdoc/>
        public override bool NeedOkButton => false;

        /// <inheritdoc/>
        public override bool NeedCancelButton => false;

        /// <inheritdoc/>
        public override void OnOkButtonClicked(DialogCloseAction action)
        {
            base.OnOkButtonClicked(action);
            if (IsReplaceVisible)
                ReplaceButtonClicked?.Invoke();
            else
                FindNextButtonClicked?.Invoke();
        }

        /// <inheritdoc/>
        protected override void CreateOtherContent()
        {
            settingsLayout = new Microsoft.Maui.Controls.VerticalStackLayout();
            settingsLayout.IsVisible = false;

            CheckBoxWithLabelView AddCheckBox(string title)
            {
                CheckBoxWithLabelView result = new();

                var labelMargin = GetLabelMargin();
                result.Margin = new(labelMargin.Left, 0, labelMargin.Right, 0);
                result.Label.Text = title;
                result.Padding = 0;

                settingsLayout.Children.Add(result);

                return result;
            }

            base.CreateOtherContent();

            var strings = UI.Localization.CommonStrings.Default;

            caseSensitiveCheckBox = AddCheckBox(strings.FindOptionMatchCase);
            wholeWordsCheckBox = AddCheckBox(strings.FindOptionMatchWholeWord);
            regularExpressionsCheckBox = AddCheckBox(strings.FindOptionUseRegularExpressions);
            selectionOnlyCheckBox = AddCheckBox(strings.FindOptionSelectionOnly);

            caseSensitiveCheckBox.CheckedChanged += (s, e) =>
            {
                OnSettingsChanged();
            };

            wholeWordsCheckBox.CheckedChanged += (s, e) =>
            {
                OnSettingsChanged();
            };

            regularExpressionsCheckBox.CheckedChanged += (s, e) =>
            {
                OnSettingsChanged();
            };

            selectionOnlyCheckBox.CheckedChanged += (s, e) =>
            {
                OnSettingsChanged();
            };

            ContentLayout.Children.Add(settingsLayout);
        }

        /// <summary>
        /// Invoked when the settings have changed, triggering the <see cref="SettingsChanged"/> event.
        /// </summary>
        /// <remarks>This method is called to notify subscribers that the settings have been updated.
        /// Derived classes can override this method to perform additional
        /// actions when settings change. Ensure that any
        /// overridden implementation calls the base method to maintain event invocation.</remarks>
        protected virtual void OnSettingsChanged()
        {
            SettingsChanged?.Invoke();
        }

        /// <summary>
        /// Raises the appropriate event when the second button is clicked,
        /// based on the current <see cref="IsReplaceVisible"/> state.
        /// </summary>
        /// <remarks>If <see cref="IsReplaceVisible"/> is <see langword="true"/>,
        /// the <see cref="ReplaceAllButtonClicked"/> event is raised.
        /// Otherwise, the <see cref="FindNextButtonClicked"/> event
        /// is raised.</remarks>
        protected virtual void OnSecondButtonClicked()
        {
            if (IsReplaceVisible)
                ReplaceAllButtonClicked?.Invoke();
            else
                FindNextButtonClicked?.Invoke();
        }

        /// <summary>
        /// Raises the appropriate event based on the visibility state of the Replace button.
        /// </summary>
        /// <remarks>If <see cref="IsReplaceVisible"/> is <see langword="true"/>, the
        /// <see cref="ReplaceButtonClicked"/> event is raised. Otherwise, the
        /// <see cref="FindPreviousButtonClicked"/> event
        /// is raised.</remarks>
        protected virtual void OnFirstButtonClicked()
        {
            if (IsReplaceVisible)
                ReplaceButtonClicked?.Invoke();
            else
                FindPreviousButtonClicked?.Invoke();
        }
    }
}
