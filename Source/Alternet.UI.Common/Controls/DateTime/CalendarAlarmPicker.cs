using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control that allows the user to select a calendar alarm.
    /// </summary>
    public partial class CalendarAlarmPicker : TransparentPanel
    {
        /// <summary>
        /// Gets or sets the default distance between controls in the <see cref="CalendarAlarmPicker"/> control.
        /// </summary>
        public static float DefaultControlDistance = 5;

        /// <summary>
        /// Gets or sets the default minimum width for the controls in the <see cref="CalendarAlarmPicker"/> control.
        /// </summary>
        public static float DefaultMinControlWidth = 200;

        private readonly CalendarAlarmInfo data;
        private readonly FileNamePicker soundFilePicker = new();
        private readonly TimePeriodPicker timePeriodPicker = new();
        private readonly PictureBox periodImage = new();
        private readonly PictureBox soundImage = new();
        private readonly XCheckBox playSoundCheckBox = new(CommonStrings.Default.PlaySound);

        private readonly TransparentPanel firstRowPanel = new()
        {
            Layout = LayoutStyle.Horizontal,
        };

        private readonly TransparentPanel secondRowPanel = new()
        {
            Layout = LayoutStyle.Horizontal,
        };

        private readonly TransparentPanel thirdRowPanel = new()
        {
            Layout = LayoutStyle.Horizontal,
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarAlarmPicker"/> class
        /// with the specified <see cref="CalendarAlarmInfo"/> data.
        /// </summary>
        /// <param name="data"></param>
        public CalendarAlarmPicker(CalendarAlarmInfo data)
        {
            Layout = LayoutStyle.Vertical;

            this.data = data;
            data.PropertyChanged += (s, e) => OnValueChanged(e);
            soundFilePicker.OpenFileProps.Filter = FileMaskUtils.FileDialogFilterSoundFiles;
            soundFilePicker.OpenFileProps.Title = CommonStrings.Default.WindowTitleSelectSoundFile;

            periodImage.SvgImage = KnownSvgImages.ImgHourglass;
            periodImage.MarginRight = DefaultControlDistance;
            soundImage.SvgImage = KnownSvgImages.ImgBell;
            soundImage.MarginRight = DefaultControlDistance;

            periodImage.Parent = firstRowPanel;
            soundImage.Parent = thirdRowPanel;

            timePeriodPicker.Parent = firstRowPanel;
            soundFilePicker.Parent = thirdRowPanel;

            soundFilePicker.MainControl.EmptyTextHint = CommonStrings.Default.DefaultSound;
            soundFilePicker.MinWidth = DefaultMinControlWidth;
            timePeriodPicker.MinWidth = DefaultMinControlWidth;

            firstRowPanel.Parent = this;
            firstRowPanel.MarginBottom = DefaultControlDistance;

            playSoundCheckBox.Parent = secondRowPanel;
            playSoundCheckBox.PaddingLeft = 0;

            secondRowPanel.Parent = this;

            thirdRowPanel.MarginTop = DefaultControlDistance;
            thirdRowPanel.Parent = this;

            ValueToControls();

            timePeriodPicker.ValueChanged += (s, e) =>
            {
                var value = timePeriodPicker.Value;
                data.Advance = value.AsTimeSpan();
                data.Enabled = value.Value != 0;
            };

            playSoundCheckBox.CheckedChanged += (s, e) =>
            {
                data.PlaySound = playSoundCheckBox.Checked;
            };

            soundFilePicker.MainControl.TextChanged += (s, e) =>
            {
                data.SoundPath = soundFilePicker.MainControl.Text;
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarAlarmPicker"/> class.
        /// </summary>
        public CalendarAlarmPicker()
            : this(new ())
        {
        }

        /// <summary>
        /// Occurs when the <see cref="Value"/> property changes.
        /// </summary>
        public event EventHandler? ValueChanged;

        /// <summary>
        /// Gets the <see cref="FileNamePicker"/> control used for
        /// selecting a sound file in the <see cref="CalendarAlarmPicker"/>.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FileNamePicker SoundFilePicker => soundFilePicker;

        /// <summary>
        /// Gets the <see cref="TimePeriodPicker"/> control used for selecting
        /// a time period in the <see cref="CalendarAlarmPicker"/>.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TimePeriodPicker TimePeriodPicker => timePeriodPicker;

        /// <summary>
        /// Gets the <see cref="PictureBox"/> control that displays the
        /// period image in the <see cref="CalendarAlarmPicker"/>.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PictureBox PeriodImage => periodImage;

        /// <summary>
        /// Gets the <see cref="PictureBox"/> control that displays
        /// the sound image in the <see cref="CalendarAlarmPicker"/>.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PictureBox SoundImage => soundImage;

        /// <summary>
        /// Gets the <see cref="XCheckBox"/> control that allows users to enable or disable sound playback.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public XCheckBox PlaySoundCheckBox => playSoundCheckBox;

        /// <summary>
        /// Gets the first row panel of the <see cref="CalendarAlarmPicker"/>,
        /// which contains the period image and time period picker.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TransparentPanel FirstRowPanel => firstRowPanel;

        /// <summary>
        /// Gets the second row panel of the <see cref="CalendarAlarmPicker"/>, which contains the play sound checkbox.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TransparentPanel SecondRowPanel => secondRowPanel;

        /// <summary>
        /// Gets the third row panel of the <see cref="CalendarAlarmPicker"/>,
        /// which contains the sound image and sound file picker.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TransparentPanel ThirdRowPanel => thirdRowPanel;

        /// <summary>
        /// Gets or sets the <see cref="CalendarAlarmInfo"/> data of the <see cref="CalendarAlarmPicker"/> control.
        /// </summary>
        public virtual CalendarAlarmInfo Value
        {
            get => data;
            set
            {
                data.Assign(value);
            }
        }

        /// <summary>
        /// Updates the controls in the <see cref="CalendarAlarmPicker"/> to reflect the current <see cref="Value"/>.
        /// </summary>
        public virtual void ValueToControls()
        {
            playSoundCheckBox.Checked = data.PlaySound;
            !!!
            soundFilePicker.MainControl.Text = data.SoundPath ?? string.Empty;
        }

        /*
                        var value = timePeriodPicker.Value;
                        data.Advance = value.AsTimeSpan();
                        data.Enabled = value.Value != 0;
        */

        /// <summary>
        /// Raises the <see cref="ValueChanged"/> event when the <see cref="Value"/> property changes.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnValueChanged(PropertyChangedEventArgs e)
        {
            ValueToControls();
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
