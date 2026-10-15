using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control that allows users to edit <see cref="CalendarItem"/>
    /// properties in a user-friendly manner.
    /// </summary>
    [ControlCategory(KnownControlCategory.Other)]
    public partial class CalendarItemPicker : HiddenBorder
    {
        /// <summary>
        /// Gets the default padding for the <see cref="CalendarItemPicker"/> control.
        /// </summary>
        public static readonly Thickness DefaultPadding = 5;

        /// <summary>
        /// Gets or sets the default margin between child controls in the <see cref="CalendarItemPicker"/>.
        /// </summary>
        public static Thickness DefaultMinChildMargin = 5;

        private readonly GenericRoundBorderWithTitle detailsPanel = new(CommonStrings.Default.CalendarItemLabelDetails);
        private readonly GenericRoundBorderWithTitle moreOptionsPanel = new(CommonStrings.Default.CalendarItemLabelMoreOptions);

        private readonly Label titleLabel = new();
        private readonly TextPicker titlePicker = new ();
        
        private readonly Label descriptionLabel = new();
        private readonly MultilineTextPicker descriptionPicker = new();

        private readonly Label locationLabel = new();
        private readonly TextPicker locationPicker = new();

        private readonly Label categoryLabel = new();
        private readonly DrawingResourcePicker categoryPicker = new();

        private readonly Label statusLabel = new();
        private readonly DrawingResourcePicker statusPicker = new();

        private readonly RepeatPatternPicker rulePicker = new();

        private CalendarItem? calendarItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarItemPicker"/> class.
        /// </summary>
        public CalendarItemPicker()
        {
            Layout = LayoutStyle.Vertical;
            Padding = DefaultPadding;

            detailsPanel.Margin = DefaultMinChildMargin;
            moreOptionsPanel.Margin = DefaultMinChildMargin;

            titleLabel.Text = CommonStrings.Default.CalendarItemLabelTitle;
            descriptionLabel.Text = CommonStrings.Default.CalendarItemLabelDescription;
            locationLabel.Text = CommonStrings.Default.CalendarItemLabelLocation;
            categoryLabel.Text = CommonStrings.Default.CalendarItemLabelCategory;
            statusLabel.Text = CommonStrings.Default.CalendarItemLabelStatus;

            foreach (var item in KnownCalendarItemMarkers.Categories)
            {
                categoryPicker.Add(item);
            }

            foreach (var item in KnownCalendarItemMarkers.Statuses)
            {
                statusPicker.Add(item);
            }

            ValueToControls();

            titleLabel.Parent = detailsPanel;
            titlePicker.Parent = detailsPanel;

            categoryLabel.Parent = detailsPanel;
            categoryPicker.Parent = detailsPanel;

            statusLabel.Parent = detailsPanel;
            statusPicker.Parent = detailsPanel;

            detailsPanel.Parent = this;

            rulePicker.Padding = 0;
            rulePicker.Parent = this;

            locationLabel.Parent = moreOptionsPanel;
            locationPicker.Parent = moreOptionsPanel;

            descriptionLabel.Parent = moreOptionsPanel;
            descriptionPicker.Parent = moreOptionsPanel;

            moreOptionsPanel.Parent = this;

            titlePicker.ValueChanged += (s, e) =>
            {
                Value.Title = titlePicker.Text;
            };

            locationPicker.ValueChanged += (s, e) =>
            {
                Value.Location = locationPicker.Text;
            };
            
            descriptionPicker.ValueChanged += (s, e) =>
            {
                Value.Description = descriptionPicker.Text;
            };
            
            categoryPicker.ValueChanged += (s, e) =>
            {
                var newValue = categoryPicker.Value as CalendarItemCategory;
                Value.Category = newValue ?? KnownCalendarItemMarkers.CategoryNone;
            };
            
            statusPicker.ValueChanged += (s, e) =>
            {
                var newValue = statusPicker.Value as CalendarItemStatus;
                Value.Status = newValue ?? KnownCalendarItemMarkers.StatusUnknown;
            };

            rulePicker.ValueChanged += (s, e) =>
            {
                Value.Rule = rulePicker.Value;
            };
        }

        /// <summary>
        /// Occurs when the <see cref="CalendarItem"/> being edited has changed.
        /// </summary>
        public event EventHandler? ValueChanged;

        /// <summary>
        /// Gets the <see cref="GenericRoundBorderWithTitle"/> that contains the controls for editing
        /// the details of the <see cref="CalendarItem"/>.
        /// </summary>
        [Browsable(false)]
        public GenericRoundBorderWithTitle DetailsPanel => detailsPanel;

        /// <summary>
        /// Gets the <see cref="GenericRoundBorderWithTitle"/> that contains the controls for editing
        /// the more options of the <see cref="CalendarItem"/>.
        /// </summary>
        [Browsable(false)]
        public GenericRoundBorderWithTitle MoreOptionsPanel => moreOptionsPanel;

        /// <summary>
        /// Gets the <see cref="RepeatPatternPicker"/> that is used to edit the recurrence rule.
        /// </summary>
        [Browsable(false)]
        public RepeatPatternPicker RulePicker => rulePicker;

        /// <summary>
        /// Gets the <see cref="Label"/> that displays the label which is shown
        /// above the <see cref="TitlePicker"/> control.
        /// </summary>
        [Browsable(false)]
        public Label TitleLabel => titleLabel;

        /// <summary>
        /// Gets the <see cref="TextPicker"/> that is used to edit the title
        /// of the <see cref="CalendarItem"/> being edited.
        /// </summary>
        [Browsable(false)]
        public TextPicker TitlePicker => titlePicker;

        /// <summary>
        /// Gets the <see cref="Label"/> that displays the label which is
        /// shown above the <see cref="DescriptionPicker"/> control.
        /// </summary>
        [Browsable(false)]
        public Label DescriptionLabel => descriptionLabel;

        /// <summary>
        /// Gets the <see cref="MultilineTextPicker"/> that is used to edit
        /// the description of the <see cref="CalendarItem"/> being edited.
        /// </summary>
        [Browsable(false)]
        public MultilineTextPicker DescriptionPicker => descriptionPicker;

        /// <summary>
        /// Gets the <see cref="Label"/> that displays the label which
        /// is shown above the <see cref="LocationPicker"/> control.
        /// </summary>
        [Browsable(false)]
        public Label LocationLabel => locationLabel;

        /// <summary>
        /// Gets the <see cref="TextPicker"/> that is used to edit the location
        /// of the <see cref="CalendarItem"/> being edited.
        /// </summary>
        [Browsable(false)]
        public TextPicker LocationPicker => locationPicker;

        /// <summary>
        /// Gets the <see cref="Label"/> that displays the label which is shown
        /// above the <see cref="CategoryPicker"/> control.
        /// </summary>
        [Browsable(false)]
        public Label CategoryLabel => categoryLabel;

        /// <summary>
        /// Gets the <see cref="DrawingResourcePicker"/> that is used to select
        /// the category of the <see cref="CalendarItem"/> being edited.
        /// </summary>
        [Browsable(false)]
        public DrawingResourcePicker CategoryPicker => categoryPicker;

        /// <summary>
        /// Gets the <see cref="Label"/> that displays the label which is shown
        /// above the <see cref="StatusPicker"/> control.
        /// </summary>
        [Browsable(false)]
        public Label StatusLabel => statusLabel;

        /// <summary>
        /// Gets the <see cref="DrawingResourcePicker"/> used to select the status
        /// of the <see cref="CalendarItem"/> being edited.
        /// </summary>
        [Browsable(false)]
        public DrawingResourcePicker StatusPicker => statusPicker;

        /// <summary>
        /// Gets or sets the <see cref="CalendarItem"/> that is being edited by the picker.
        /// </summary>
        [Browsable(false)]
        public virtual CalendarItem Value
        {
            get
            {
                if (calendarItem == null)
                {
                    calendarItem = CreateCalendarItem();
                    calendarItem.PropertyChanged += OnCalendarItemChanged;
                }

                return calendarItem;
            }

            set
            {
                if (calendarItem == value)
                    return;
                value ??= CreateCalendarItem();

                if (calendarItem != null)
                {
                    calendarItem.PropertyChanged -= OnCalendarItemChanged;
                }

                calendarItem = value;
                calendarItem.PropertyChanged += OnCalendarItemChanged;
                ValueToControls();
            }
        }

        /// <summary>
        /// Updates the controls of the picker to reflect the current values
        /// of the <see cref="CalendarItem"/> being edited.
        /// </summary>
        public virtual void ValueToControls()
        {
            titlePicker.Text = Value.Title ?? string.Empty;
            descriptionPicker.Text = Value.Description ?? string.Empty;
            locationPicker.Text = Value.Location?.ToString() ?? string.Empty;
            statusPicker.Value = Value.Status ?? KnownCalendarItemMarkers.StatusUnknown;
            categoryPicker.Value = Value.Category ?? KnownCalendarItemMarkers.CategoryNone;
            rulePicker.Value = Value.Rule;
        }

        /// <summary>
        /// Called when a property of the <see cref="CalendarItem"/> being edited has changed.
        /// This method can be overridden in derived classes to provide custom behavior when a property changes.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A <see cref="PropertyChangedEventArgs"/> that contains the event data.</param>
        protected virtual void OnCalendarItemChanged(object? sender, PropertyChangedEventArgs e)
        {
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            if (calendarItem != null)
                calendarItem.PropertyChanged -= OnCalendarItemChanged;

            base.DisposeManaged();
        }

        /// <summary>
        /// Creates a new instance of <see cref="CalendarItem"/>. This method can be overridden
        /// in derived classes to provide a custom implementation of <see cref="CalendarItem"/>.
        /// </summary>
        /// <returns>A new instance of <see cref="CalendarItem"/>.</returns>
        protected virtual CalendarItem CreateCalendarItem()
        {
            return new CalendarItem();
        }
    }

    /// <summary>
    /// Represents a scrollable version of the <see cref="CalendarItemPicker"/> control.
    /// </summary>
    public class ScrollableCalendarItemPicker : ScrollViewer<CalendarItemPicker>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollableCalendarItemPicker"/> class.
        /// </summary>
        public ScrollableCalendarItemPicker()
        {
            ScrolledControl.HasBorder = false;
        }

        /// <summary>
        /// Occurs when the selected calendar item changes in the inner <see cref="CalendarItemPicker"/> control.
        /// </summary>
        public event EventHandler? ValueChanged
        {
            add => ScrolledControl.ValueChanged += value;
            remove => ScrolledControl.ValueChanged -= value;
        }

        /// <summary>
        /// Gets the <see cref="CalendarItem"/> instance representing
        /// the selected calendar item and its associated rules.
        /// </summary>
        public CalendarItem Value
        {
            get
            {
                return ScrolledControl.Value;
            }

            set
            {
                ScrolledControl.Value = value;
            }
        }
    }
}
