using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Popup window with <see cref="XCalendar"/> control.
    /// </summary>
    public partial class PopupCalendar : PopupWindow<XCalendar>
    {
        /// <summary>
        /// Gets or sets a value indicating whether the popup window
        /// should be hidden when the user clicks outside of it by default.
        /// </summary>
        public static bool DefaultHideOnClick = true;

        /// <summary>
        /// Gets or sets a value indicating whether the popup window
        /// should be hidden when the "Today" button is clicked by default.
        /// </summary>
        public static bool DefaultHidePopupOnTodayClick = true;

        /// <summary>
        /// Gets or sets a value indicating whether the "Today" button is visible by default.
        /// </summary>
        public static bool DefaultIsTodayButtonVisible = true;

        private static PopupCalendar? defaultCalendar;
        
        private readonly SpeedButton todayButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="PopupCalendar"/> class.
        /// </summary>
        public PopupCalendar()
        {
            Title = CommonStrings.Default.WindowTitleSelectDate;
            HideOnClick = DefaultHideOnClick;
            HideOnDoubleClick = false;

            todayButton = LeftBottomToolBar.AddSpeedBtnCore(
                CommonStrings.Default.Today,
                KnownSvgImages.ImgCalendarCheck,
                CommonStrings.Default.Today,
                OnTodayButtonClick);
            todayButton.UseTheme = ButtonOk.UseTheme;
            todayButton.Visible = DefaultIsTodayButtonVisible;

            MainControl.MouseLeftButtonUp -= OnMainControlMouseLeftButtonUp;

            MainControl.ListBox.MouseLeftButtonUp += (s, e) =>
            {
                var itemIndex = MainControl.ListBox.HitTest();

                if (itemIndex is null || itemIndex == 0)
                    return;

                OnMainControlMouseLeftButtonUp(s, e);
            };
        }

        /// <summary>
        /// Gets or sets default instance of the <see cref="PopupCalendar"/>.
        /// </summary>
        public static new PopupCalendar Default
        {
            get
            {
                if (defaultCalendar == null)
                {
                    defaultCalendar = new PopupCalendar();
                }

                return defaultCalendar;
            }

            set
            {
                defaultCalendar = value;
            }
        }

        /// <summary>
        /// Gets the "Today" button of the <see cref="PopupCalendar"/> control.
        /// </summary>
        public SpeedButton TodayButton => todayButton;

        /// <inheritdoc/>
        protected virtual void OnTodayButtonClick(object? sender, EventArgs e)
        {
            MainControl.AsDateTime = DateTime.Now;
            if (DefaultHidePopupOnTodayClick)
                HidePopup(ModalResult.Accepted);
        }

        /// <inheritdoc/>
        protected override XCalendar CreateMainControl()
        {
            var result = new XCalendar()
            {
                HasBorder = false,
            };

            return result;
        }

        /// <inheritdoc/>
        protected override void OnVisibleChanging(EventArgs e)
        {
            base.OnVisibleChanging(e);
            if (!Visible)
            {
                SetSizeToContent();
            }
        }

        /// <inheritdoc/>
        protected override bool HideOnClickPoint(PointD point)
        {
            return true;
        }
    }
}
