using Alternet.Base.Collections;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class DateTimePicker : Control
    {
        /// <inheritdoc/>
        public new DateTimePickerHandler Handler
        {
            get
            {
                CheckDisposed();
                return (DateTimePickerHandler)base.Handler;
            }
        }

        /// <summary>
        /// Gets or sets the text displayed on this picker.
        /// </summary>
        //[DefaultValue("")]
        //[Localizability(LocalizationCategory.Text)]
        //public string Text
        //{
        //    get { return (string)GetValue(TextProperty); }
        //    set { SetValue(TextProperty, value); }
        //}

        private DateTime currentValue;

        public virtual DateTime GetValue()
        {
            return currentValue;
        }

        //public virtual bool GetRange(DateTime dt1, DateTime dt2)
        //{

        //}

        /// <summary>
        /// Event for "Text has changed"
        /// </summary>
        //public static readonly RoutedEvent TextChangedEvent = EventManager.RegisterRoutedEvent(
        //    "TextChanged", // Event name
        //    RoutingStrategy.Bubble, //
        //    typeof(TextChangedEventHandler), //
        //    typeof(DateTimePicker)); //

        /// <summary>
        /// Identifies the <see cref="Text"/> dependency property.
        /// </summary>
        //public static readonly DependencyProperty TextProperty =
        //DependencyProperty.Register(
        //        "Text", // Property name
        //        typeof(string), // Property type
        //        typeof(DateTimePicker), // Property owner
        //        new FrameworkPropertyMetadata( // Property metadata
        //                string.Empty, // default value
        //                FrameworkPropertyMetadataOptions.AffectsLayout | FrameworkPropertyMetadataOptions.AffectsPaint,// Flags
        //                new PropertyChangedCallback(OnTextPropertyChanged),    // property changed callback
        //                new CoerceValueCallback(CoerceText)
        //                ));


        /// <summary>
        /// Called when content in this Control changes.
        /// Raises the TextChanged event.
        /// </summary>
        /// <param name="e"></param>
        //protected virtual void OnTextChanged(TextChangedEventArgs e)
        //{
        //    RaiseEvent(e);
        //}

        /// <summary>
        /// Callback for changes to the Text property
        /// </summary>
        //private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    DateTimePicker picker = (DateTimePicker)d;
        //    picker.OnTextPropertyChanged((string)e.OldValue, (string)e.NewValue);
        //}

        //private void OnTextPropertyChanged(string oldText, string newText)
        //{
        //    OnTextChanged(new TextChangedEventArgs(TextChangedEvent));
        //}

        //private static object CoerceText(DependencyObject d, object value) => value == null ? string.Empty : value;
    }
}
