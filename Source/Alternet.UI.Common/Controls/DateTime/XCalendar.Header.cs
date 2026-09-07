using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.UI.Localization;

namespace Alternet.UI
{
    public partial class XCalendar
    {
        /// <summary>
        /// Represents the header of the calendar control, which includes month and year pickers.
        /// </summary>
        public class CalendarHeader : TransparentPanel
        {
            /// <summary>
            /// Gets or sets a value indicating whether the year dropdown in the calendar header should be displayed by default
            /// when the year picker is clicked, allowing users to select a specific year.
            /// </summary>
            public bool DefaultShowYearDropDown = true;

            /// <summary>
            /// Gets or sets a value indicating whether the month dropdown in the calendar header should be displayed by default
            /// when the month picker is clicked, allowing users to select a specific month.
            /// </summary>
            public bool DefaultShowMonthDropDown = true;

            /// <summary>
            /// Gets or sets the default horizontal alignment for the left button in the calendar header.
            /// </summary>
            public static HorizontalAlignment DefaultLeftButtonHorzAlignment = HorizontalAlignment.Right;

            /// <summary>
            /// Gets or sets the default horizontal alignment for the month picker in the calendar header.
            /// </summary>
            public static HorizontalAlignment DefaultMonthHorzAlignment = HorizontalAlignment.Left;

            /// <summary>
            /// Gets or sets the default horizontal alignment for the year picker in the calendar header.
            /// </summary>
            public static HorizontalAlignment DefaultYearHorzAlignment = HorizontalAlignment.Left;

            private readonly SpeedTextButton monthPicker = new();
            private readonly SpeedTextButton yearPicker = new();
            private readonly SpeedButton actionsButton = new();
            private readonly SpeedButton prevButton = new();
            private readonly SpeedButton nextButton = new();
            private readonly YearPicker popupYearPicker;
            private readonly TransparentPanel popupYearPickerPanel;
            private readonly MonthPickerPanel popupMonthPicker;
            private readonly TransparentPanel firstRowPanel = new();
            private readonly GenericControlAndButton<TextPicker> popupDateTextPicker = new();
            private readonly SpeedButton popupDateTextOkButton;
            private readonly SpeedButton popupDateTextCancelButton;

            private DateOnly date = DateOnly.FromDateTime(DateTime.Now);
            private int suspendCounter;
            private MonthNamesKind kind = MonthNamesKind.Full;
            private IFormatProvider? formatProvider;
            string newPopupDateTextPickerText = string.Empty;

            /// <summary>
            /// Initializes a new instance of the <see cref="CalendarHeader"/> class.
            /// </summary>
            public CalendarHeader()
            {
                popupDateTextPicker.HasBtnComboBox = false;

                popupDateTextOkButton = popupDateTextPicker.Buttons.AddSpeedBtnCore(
                    null,
                    KnownSvgImages.ImgOk,
                    null,
                    OnPopupTextPickerEnterApplyButtonClick);
                popupDateTextCancelButton = popupDateTextPicker.Buttons.AddSpeedBtnCore(
                    null,
                    KnownSvgImages.ImgCancel,
                    null,
                    (s, e) =>
                    {
                        OnPopupTextPickerEscapePressed(s, e);
                    });

                popupYearPickerPanel = new();
                popupYearPickerPanel.Visible = false;
                popupYearPickerPanel.MarginTop = 5;

                popupYearPicker = new();
                popupYearPicker.Parent = popupYearPickerPanel;

                popupMonthPicker = new();
                popupMonthPicker.MarginTop = 5;
                popupMonthPicker.Visible = false;

                firstRowPanel.Layout = LayoutStyle.Horizontal;

                Layout = LayoutStyle.Vertical;
                RoundCorners();
                HasBorder = true;
                Padding = (5, 2, 5, 2);

                OnValueChanged();

                actionsButton.Visible = false;
                actionsButton.VerticalAlignment = VerticalAlignment.Center;
                actionsButton.SvgImage = KnownSvgImages.ImgMoreActionsHorz;
                actionsButton.HorizontalAlignment = HorizontalAlignment.Right;
                actionsButton.Parent = firstRowPanel;

                prevButton.VerticalAlignment = VerticalAlignment.Center;
                prevButton.SvgImage = KnownSvgImages.ImgAngleLeft;
                prevButton.HorizontalAlignment = DefaultLeftButtonHorzAlignment;
                prevButton.Parent = firstRowPanel;
                prevButton.Click += OnPrevButtonClick;

                nextButton.VerticalAlignment = VerticalAlignment.Center;
                nextButton.HorizontalAlignment = HorizontalAlignment.Right;
                nextButton.SvgImage = KnownSvgImages.ImgAngleRight;
                nextButton.Parent = firstRowPanel;
                nextButton.Click += OnNextButtonClick;

                monthPicker.MarginRight = 0;
                monthPicker.ImageVisible = false;
                monthPicker.VerticalAlignment = VerticalAlignment.Stretch;
                monthPicker.HorizontalAlignment = DefaultMonthHorzAlignment;
                monthPicker.Parent = firstRowPanel;

                yearPicker.HorizontalAlignment = DefaultYearHorzAlignment;
                yearPicker.VerticalAlignment = VerticalAlignment.Stretch;
                yearPicker.Parent = firstRowPanel;

                popupDateTextPicker.Visible = false;
                popupDateTextPicker.MarginTop = 5;
                popupDateTextPicker.MainControl.CommitOnKeyPress = true;
                popupDateTextPicker.MainControl.CommitOnEnter = true;
                popupDateTextPicker.MainControl.PopupLostFocusBehavior = ModalResult.Accepted;

                firstRowPanel.Parent = this;
                popupDateTextPicker.Parent = this;
                popupYearPickerPanel.Parent = this;
                popupMonthPicker.Parent = this;

                popupYearPickerPanel.VisibleChanged += OnPopupYearPickerVisibleChanged;
                popupMonthPicker.VisibleChanged += OnPopupMonthPickerVisibleChanged;

                popupYearPicker.TextPicker.EnterPressed += OnPopupYearPickerEnterPressed;
                popupYearPicker.TextPicker.EscapePressed += OnPopupYearPickerEscapePressed;
                popupYearPicker.ValueChanged += OnPopupYearPickerValueChanged;

                popupMonthPicker.MonthClick += OnPopupMonthPickerMonthClick;

                ShowMonthDropDown = DefaultShowMonthDropDown;
                ShowYearDropDown = DefaultShowYearDropDown;

                YearClick += OnHeaderYearClick;
                MonthClick += OnHeaderMonthClick;

                popupDateTextPicker.MainControl.EnterPressed += OnPopupTextPickerEnterPressed;
                popupDateTextPicker.MainControl.EscapePressed += OnPopupTextPickerEscapePressed;
                popupDateTextPicker.VisibleChanged += OnPopupTextPickerVisibleChanged;

                PopupDateTextPicker.MainControl.TextEditing += (s, e) =>
                {
                    newPopupDateTextPickerText = e.Value;
                    popupDateTextPicker.Text = newPopupDateTextPickerText; 
                };

                PopupDateTextPicker.MainControl.TextEdited += (s, e) =>
                {
                };
            }

            /// <summary>
            /// Occurs when the year picker in the calendar header is clicked, 
            /// allowing subscribers to handle the event and perform actions based on the year selection.
            /// </summary>
            public event EventHandler? YearClick
            {
                add
                {
                    YearPicker.Click += value;
                }

                remove
                {
                    YearPicker.Click -= value;
                }
            }

            /// <summary>
            /// Occurs when the month picker in the calendar header is clicked,
            /// allowing subscribers to handle the event and perform actions based on the month selection.
            /// </summary>
            public event EventHandler? MonthClick
            {
                add
                {
                    MonthPicker.Click += value;
                }

                remove
                {
                    MonthPicker.Click -= value;
                }
            }

            /// <summary>
            /// Gets or sets the current value of the calendar header, representing the selected month and year.
            /// </summary>
            public event EventHandler? ValueChanged;

            /// <summary>
            /// Gets or sets the kind of month names displayed in the month picker,
            /// allowing customization of the month name format.
            /// </summary>
            public virtual MonthNamesKind Kind
            {
                get => kind;
                set
                {
                    if (value == kind) return;
                    kind = value;
                    UpdatePickerValues();
                }
            }

            /// <summary>
            /// Gets the date text picker.
            /// </summary>
            [Browsable(false)]
            public GenericControlAndButton<TextPicker> PopupDateTextPicker => popupDateTextPicker;

            /// <summary>
            /// Gets the popup panel that contains the year picker.
            /// </summary>
            [Browsable(false)]
            public TransparentPanel PopupYearPickerPanel => popupYearPickerPanel;

            /// <summary>
            /// Gets or sets a value indicating whether the month dropdown in the calendar header should be displayed,
            /// when the month picker is clicked, allowing users to select a specific month.
            /// </summary>
            public virtual bool ShowMonthDropDown { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether the year dropdown in the calendar header should be displayed
            /// when the year picker is clicked, allowing users to select a specific year.
            /// </summary>
            public virtual bool ShowYearDropDown { get; set; }

            /// <summary>
            /// Gets or sets the overlay provider for the calendar header,
            /// which can be used to display additional information on top of the header.
            /// </summary>
            [Browsable(false)]
            public virtual AbstractControl? OverlayProvider { get; set; }

            /// <summary>
            /// Gets the text picker used in the popup panel, allowing users to enter a date as text.
            /// </summary>
            [Browsable(false)]
            public SpeedButton PopupDateTextOkButton => popupDateTextOkButton;

            /// <summary>
            /// Gets the cancel button in the popup date text picker, allowing users to cancel the date entry.
            /// </summary>
            [Browsable(false)]
            public SpeedButton PopupDateTextCancelButton => popupDateTextCancelButton;

            /// <summary>
            /// Gets the year picker used in the popup panel.
            /// </summary>
            [Browsable(false)]
            public YearPicker PopupYearPicker => popupYearPicker;

            /// <summary>
            /// Gets or sets a value indicating whether to disable the month
            /// (and, implicitly, the year) changing.
            /// </summary>
            public virtual bool NoMonthChange { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether to disable the year changing.
            /// </summary>
            public virtual bool NoYearChange { get; set; }

            /// <summary>
            /// Gets the first row panel in the calendar header, which contains the month and year pickers.
            /// </summary>
            [Browsable(false)]
            public TransparentPanel FirstRowPanel => firstRowPanel;

            /// <summary>
            /// Gets the month picker panel used in the popup panel, allowing users to select a month.
            /// </summary>
            [Browsable(false)]
            public MonthPickerPanel PopupMonthPicker => popupMonthPicker;

            /// <summary>
            /// Gets or sets the format provider used for formatting month and year values in the calendar header.
            /// </summary>
            [Browsable(false)]
            public virtual IFormatProvider? FormatProvider
            {
                get => formatProvider;
                set
                {
                    if (value == formatProvider) return;
                    formatProvider = value;
                    UpdatePickerValues();
                }
            }

            /// <inheritdoc/>
            public override bool? IsDarkBackgroundOverride
            {
                get => base.IsDarkBackgroundOverride;
                set
                {
                    monthPicker.IsDarkBackgroundOverride = value;
                    yearPicker.IsDarkBackgroundOverride = value;
                    prevButton.IsDarkBackgroundOverride = value;
                    actionsButton.IsDarkBackgroundOverride = value;
                    nextButton.IsDarkBackgroundOverride = value;
                    popupYearPicker.IsDarkBackgroundOverride = value;
                    popupYearPickerPanel.IsDarkBackgroundOverride = value;
                    popupMonthPicker.IsDarkBackgroundOverride = value;
                    firstRowPanel.IsDarkBackgroundOverride = value;

                    base.IsDarkBackgroundOverride = value;
                }
            }

            /// <summary>
            /// Gets or sets the current value of the calendar header, representing the selected month and year.
            /// </summary>
            public virtual DateOnly Value
            {
                get
                {
                    return date;
                }

                set
                {
                    value = new(value.Year, value.Month, 1);
                    if (this.date == value)
                        return;
                    this.date = value;
                    OnValueChanged();
                    Invalidate();
                }
            }

            /// <summary>
            /// Gets the previous button in the calendar header, which allows users to navigate to the previous month.
            /// </summary>
            [Browsable(false)]
            public SpeedButton PrevButton => prevButton;

            /// <summary>
            /// Gets the actions button in the calendar header, which allows users to perform additional actions.
            /// </summary>
            [Browsable(false)]
            public SpeedButton ActionsButton => actionsButton;

            /// <summary>
            /// Gets the next button in the calendar header, which allows users to navigate to the next month.
            /// </summary>
            [Browsable(false)]
            public SpeedButton NextButton => nextButton;

            /// <summary>
            /// Gets the month picker in the calendar header, which allows users to select a month from a dropdown list.
            /// </summary>
            [Browsable(false)]
            public SpeedButton MonthPicker => monthPicker;

            /// <summary>
            /// Gets the year picker in the calendar header, which allows users to select a year from a dropdown list.
            /// </summary>
            [Browsable(false)]
            public SpeedButton YearPicker => yearPicker;

            /// <summary>
            /// Shows or hides all popups in the calendar header, including the year picker, month picker, and date text box.
            /// </summary>
            /// <param name="show">A value indicating whether to show or hide the popups.</param>
            public virtual void ShowAllPopups(bool show = true)
            {
                ShowPopupYearPicker(show);
                ShowPopupMonthPicker(show);
                ShowPopupDateTextBox(show);
            }

            /// <summary>
            /// Opens a popup for the user to select a year using a year picker.
            /// </summary>
            /// <param name="show">A value indicating whether to show or hide the popup year picker.</param>
            public virtual void ShowPopupYearPicker(bool show = true)
            {
                PopupYearPickerPanel.Visible = show;
            }

            /// <summary>
            /// Opens a popup for the user to select a month using a month picker.
            /// </summary>
            /// <param name="show">A value indicating whether to show or hide the popup month picker.</param>
            public virtual void ShowPopupMonthPicker(bool show = true)
            {
                PopupMonthPicker.Visible = show;
            }

            /// <summary>
            /// Opens a popup for the user to select a date using a text box.
            /// </summary>
            /// <param name="show">A value indicating whether to show or hide the popup date text box.</param>
            public virtual void ShowPopupDateTextBox(bool show = true)
            {
                PopupDateTextPicker.Visible = show;
            }

            /// <summary>
            /// Converts the specified string representation of a date to a <see cref="DateOnly"/> value
            /// and updates the <see cref="Value"/> property.
            /// </summary>
            /// <param name="s">The string representation of the date.</param>
            /// <param name="showError">Indicates whether to show an error if the conversion fails.</param>
            /// <returns><c>true</c> if the conversion was successful; otherwise, <c>false</c>.</returns>
            protected virtual bool StringToValue(string s, bool showError)
            {
                var result = DateOnly.TryParse(s, FormatProvider, out var dt);

                if (result)
                {
                    Value = dt;
                    return true;
                }
                else
                {
                    if (showError)
                    {
                        OverlayProvider?.ShowOverlayToolTipWithError(
                            null,
                            CommonStrings.Default.ErrInvalidDateFormat,
                            HVAlignment.BottomLeft);
                    }
                    return false;
                }
            }

            /// <summary>
            /// Called when the month picker in the calendar header is clicked,
            /// toggling the visibility of the month dropdown panel.
            /// </summary>
            /// <param name="sender">The source of the event.</param>
            /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
            protected virtual void OnHeaderMonthClick(object? sender, EventArgs e)
            {
                if (!ShowMonthDropDown || NoMonthChange)
                    return;
                popupMonthPicker.Visible = !popupMonthPicker.Visible;
            }

            /// <summary>
            /// Called when the enter key is pressed in the year picker,
            /// canceling the edit and hiding the year dropdown panel.
            /// </summary>
            /// <param name="sender">The source of the event.</param>
            /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
            protected virtual void OnPopupYearPickerEnterPressed(object? sender, EventArgs e)
            {
                popupYearPicker.TextPicker.CancelEdit();
                popupYearPickerPanel.Visible = false;
            }

            /// <summary>
            /// Called when the escape key is pressed in the year picker,
            /// canceling the edit and hiding the year dropdown panel.
            /// </summary>
            /// <param name="sender">The source of the event.</param>
            /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
            protected virtual void OnPopupYearPickerEscapePressed(object? sender, EventArgs e)
            {
                popupYearPicker.TextPicker.CancelEdit();
                popupYearPickerPanel.Visible = false;
            }

            /// <summary>
            /// Called when the visibility of the text picker popup panel changes.
            /// </summary>
            /// <param name="sender">The source of the event.</param>
            /// <param name="e">An EventArgs that contains the event data.</param>
            protected virtual void OnPopupTextPickerVisibleChanged(object? sender, EventArgs e)
            {
                if (!popupDateTextPicker.Visible)
                    return;
                popupMonthPicker.Visible = false;
                popupYearPickerPanel.Visible = false;

                var s = Value.ToString(DefaultDateFormatForTextBox, FormatProvider);
                popupDateTextPicker.Text = s;
                newPopupDateTextPickerText = s;

                popupDateTextPicker.MainControl.BeginEdit();
            }

            /// <summary>
            /// Called when the month item in the popup month panel is clicked,
            /// allowing subscribers to handle the event and perform actions based on the month selection.
            /// </summary>
            /// <param name="sender">The source of the event.</param>
            /// <param name="e">The event data.</param>
            protected virtual void OnPopupMonthPickerMonthClick(object? sender, BaseEventArgs<CalendarMonth> e)
            {
                popupMonthPicker.Visible = false;
                Value = new DateOnly(Value.Year, (int)e.Value, Value.Day);
            }

            /// <summary>
            /// Called when the enter key is pressed in the text picker, canceling the edit and hiding the text dropdown panel.
            /// </summary>
            /// <param name="sender">The source of the event.</param>
            /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
            protected virtual void OnPopupTextPickerEnterPressed(object? sender, EventArgs e)
            {
                popupDateTextPicker.MainControl.ApplyEdit();
                popupDateTextPicker.Visible = false;
                StringToValue(newPopupDateTextPickerText, showError: true);
            }

            /// <summary>
            /// Called when the apply button in the text picker is clicked,
            /// applying the edited value and hiding the text dropdown panel.
            /// </summary>
            /// <param name="sender">The source of the event.</param>
            /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
            protected virtual void OnPopupTextPickerEnterApplyButtonClick(object? sender, EventArgs e)
            {
                popupDateTextPicker.Visible = false;
                StringToValue(newPopupDateTextPickerText, showError: true);
            }

            /// <summary>
            /// Called when the escape key is pressed in the text picker, canceling the edit and hiding the text dropdown panel.
            /// </summary>
            /// <param name="sender">The source of the event.</param>
            /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
            protected virtual void OnPopupTextPickerEscapePressed(object? sender, EventArgs e)
            {
                popupDateTextPicker.MainControl.CancelEdit();
                popupDateTextPicker.Visible = false;
            }

            /// <summary>
            /// Called when the value of the year picker in the calendar header changes, updating the calendar's value accordingly.
            /// </summary>
            /// <param name="sender">The source of the event.</param>
            /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
            protected virtual void OnPopupYearPickerValueChanged(object? sender, EventArgs e)
            {
                if (suspendCounter > 0)
                    return;
                Value = new DateOnly(popupYearPicker.Value, Value.Month, Value.Day);
            }

            /// <summary>
            /// Called when the visibility of the year picker popup panel changes,
            /// updating the sticky state of the year picker in the calendar header accordingly.
            /// </summary>
            /// <param name="sender">The source of the event.</param>
            /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
            protected virtual void OnPopupYearPickerVisibleChanged(object? sender, EventArgs e)
            {
                YearPicker.Sticky = popupYearPickerPanel.Visible;

                if (popupYearPickerPanel.Visible)
                {
                    popupMonthPicker.Visible = false;
                    popupDateTextPicker.Visible = false;
                }
            }

            /// <summary>
            /// Called when the year picker in the calendar header is clicked, toggling the visibility of the year dropdown panel.
            /// </summary>
            /// <param name="sender">The source of the event.</param>
            /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
            protected virtual void OnHeaderYearClick(object? sender, EventArgs e)
            {
                if (!ShowYearDropDown || NoMonthChange || NoYearChange)
                    return;
                popupYearPickerPanel.Visible = !popupYearPickerPanel.Visible;
            }

            /// <summary>
            /// Called when the visibility of the month picker popup panel changes,
            /// updating the sticky state of the month picker in the calendar header accordingly.
            /// </summary>
            /// <param name="sender">The source of the event.</param>
            /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
            protected virtual void OnPopupMonthPickerVisibleChanged(object? sender, EventArgs e)
            {
                MonthPicker.Sticky = popupMonthPicker.Visible;

                if (popupMonthPicker.Visible)
                {
                    popupYearPickerPanel.Visible = false;
                    popupDateTextPicker.Visible = false;
                }
            }

            /// <summary>
            /// Updates the values of the month and year pickers based on the current value of the calendar header.
            /// </summary>
            protected virtual void UpdatePickerValues()
            {
                monthPicker.Text = DateUtils.GetMonthName((CalendarMonth)date.Month, Kind, FormatProvider);
                yearPicker.Text = date.Year.ToString();
                popupYearPicker.Value = date.Year;
                popupMonthPicker.Value = (CalendarMonth)date.Month;
            }

            /// <summary>
            /// Handles the event when the previous button is clicked, updating the calendar's value to the previous month.
            /// </summary>
            /// <param name="sender">The source of the event.</param>
            /// <param name="e">The event arguments.</param>
            protected virtual void OnPrevButtonClick(object? sender, EventArgs e)
            {
                if (NoMonthChange)
                    return;

                var newValue = Value.AddMonths(-1);

                if (NoYearChange)
                {
                    if (!DateUtils.IsThisYear(newValue))
                        return;
                }

                Value = newValue;
            }

            /// <summary>
            /// Handles the event when the next button is clicked, updating the calendar's value to the next month.
            /// </summary>
            /// <param name="sender">The source of the event.</param>
            /// <param name="e">The event arguments.</param>
            protected virtual void OnNextButtonClick(object? sender, EventArgs e)
            {
                if (NoMonthChange)
                    return;

                var newValue = Value.AddMonths(1);

                if (NoYearChange)
                {
                    if (!DateUtils.IsThisYear(newValue))
                        return;
                }

                Value = newValue;
            }

            /// <summary>
            /// Called when the value of the calendar header changes, updating the month and year pickers
            /// accordingly and raising the <see cref="ValueChanged"/> event.
            /// </summary>
            protected virtual void OnValueChanged()
            {
                suspendCounter++;
                try
                {
                    UpdatePickerValues();
                }
                finally
                {
                    suspendCounter--;
                }

                ValueChanged?.Invoke(this, EventArgs.Empty);
            }

            /// <summary>
            /// Handles the event when the value of the year picker changes, updating the calendar's value accordingly.
            /// </summary>
            /// <param name="sender">The source of the event.</param>
            /// <param name="e">The event arguments.</param>
            private void OnYearPickerValueChanged(object? sender, EventArgs e)
            {
                if (suspendCounter > 0)
                    return;
            }

            /// <summary>
            /// Handles the event when the value of the month picker changes, updating the calendar's value accordingly.
            /// </summary>
            /// <param name="sender">The source of the event.</param>
            /// <param name="e">The event arguments.</param>
            private void OnMonthPickerValueChanged(object? sender, EventArgs e)
            {
                if (suspendCounter > 0)
                    return;
            }
        }
    }
}
