using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Alternet.Maui
{
    /// <summary>
    /// Implements <see cref="CheckBox"/> with <see cref="Label"/>.
    /// </summary>
    public partial class CheckBoxWithLabelView : HorizontalStackLayout
    {
        private readonly CheckBox checkBox = new();
        private readonly Label label = new();

        static CheckBoxWithLabelView()
        {
#if WINDOWS
            Microsoft.Maui.Handlers.CheckBoxHandler.Mapper.AppendToMapping(
                "ReduceSpacing",
                (handler, view) =>
                {
                    handler.PlatformView.Padding = new Microsoft.UI.Xaml.Thickness(0);
                    handler.PlatformView.Margin = new Microsoft.UI.Xaml.Thickness(0);
                });
#endif
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckBoxWithLabelView"/> class.
        /// </summary>
        public CheckBoxWithLabelView()
        {
            label.Margin = 0;
            checkBox.Margin = 0;
            label.Padding = 0;
            Padding = 0;
            Margin = 0;

            checkBox.HeightRequest = 24;
            checkBox.WidthRequest = 24;

            var ho = checkBox.VerticalOptions;
            ho.Alignment = LayoutAlignment.Center;
            checkBox.VerticalOptions = ho;

            ho = label.VerticalOptions;
            ho.Alignment = LayoutAlignment.Center;
            label.VerticalOptions = ho;

            Children.Add(checkBox);
            Children.Add(label);
        }

        /// <summary>
        /// Occurs when checked state of the inner checkbox is changed.
        /// </summary>
        public event EventHandler<CheckedChangedEventArgs> CheckedChanged
        {
            add => checkBox.CheckedChanged += value;
            remove => checkBox.CheckedChanged -= value;
        }

        /// <summary>
        /// Gets or sets tooltip.
        /// </summary>
        public virtual object ToolTip
        {
            get
            {
                return ToolTipProperties.GetText(checkBox);
            }

            set
            {
                ToolTipProperties.SetText(checkBox, value);
                ToolTipProperties.SetText(label, value);
            }
        }

        /// <summary>
        /// Gets or sets whether inner checkbox is checked.
        /// </summary>
        public virtual bool IsChecked
        {
            get
            {
                return checkBox.IsChecked;
            }

            set
            {
                checkBox.IsChecked = value;
            }
        }

        /// <summary>
        /// Gets inner <see cref="CheckBox"/> control.
        /// </summary>
        public CheckBox CheckBox => checkBox;

        /// <summary>
        /// Gets inner label control.
        /// </summary>
        public Label Label => label;

        /// <summary>
        /// Gets or sets label text.
        /// </summary>
        public virtual string Text
        {
            get
            {
                return label.Text;
            }

            set
            {
                label.Text = value;
            }
        }
    }
}