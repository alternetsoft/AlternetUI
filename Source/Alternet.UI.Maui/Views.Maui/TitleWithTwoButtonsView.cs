#region Copyright (c) 2025 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2025 Alternet Software

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Alternet.Maui
{
    /// <summary>
    /// Implements title with label and two buttons.
    /// By default buttons are 'Settings' and 'Show keyboard'.
    /// </summary>
    public partial class TitleWithTwoButtonsView : ContentView
    {
        private readonly Label label = new()
        {
            Margin = new Thickness(5, 5, 5, 5),
            FontAttributes = FontAttributes.Bold,
        };

        private readonly ImageButton settingsButton = new()
        {
            Margin = new Thickness(5, 5, 5, 5),
            IsVisible = false,
        };

        private readonly ImageButton keyboardButton = new()
        {
            Margin = new Thickness(5, 5, 5, 5),
            IsVisible = false,
        };

        private readonly Grid grid = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="TitleWithTwoButtonsView"/> class.
        /// </summary>
        public TitleWithTwoButtonsView()
        {
            Grid.SetColumn(label, 0);
            Grid.SetColumn(keyboardButton, 1);
            Grid.SetColumn(settingsButton, 2);

            grid.RowDefinitions = new()
            {
                new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) },
            };

            grid.ColumnDefinitions = new()
            {
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) },
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) },
            };

            int size = SimpleToolBarView.GetDefaultImageSize();

            grid.Children.Add(label);

            label.VerticalOptions = LayoutOptions.Center;

            ToolTipProperties.SetText(settingsButton, "Show settings");

            settingsButton.Aspect = Aspect.Center;
            Alternet.UI.MauiUtils
                .SetButtonImage(settingsButton, Alternet.UI.KnownSvgImages.ImgGear, size);
            settingsButton.HorizontalOptions = LayoutOptions.End;
            settingsButton.VerticalOptions = LayoutOptions.Center;
            grid.Children.Add(settingsButton);

            ToolTipProperties.SetText(keyboardButton, "Toggle keyboard visibility");
            keyboardButton.Aspect = Aspect.Center;
            Alternet.UI.MauiUtils
                .SetButtonImage(keyboardButton, Alternet.UI.KnownSvgImages.ImgKeyboard, size);
            keyboardButton.HorizontalOptions = LayoutOptions.End;
            keyboardButton.VerticalOptions = LayoutOptions.Center;
            grid.Children.Add(keyboardButton);

            Content = grid;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TitleWithTwoButtonsView"/> class
        /// with the specified parameters.
        /// </summary>
        /// <param name="title">Title text.</param>
        /// <param name="parentView">Parent view to which this title is attached.</param>
        public TitleWithTwoButtonsView(string title, VisualElement parentView)
            : this()
        {
            label.Text = title;
            if (Alternet.UI.App.IsWindowsOS)
                SetBinding(WidthRequestProperty, new Binding("Width", source: parentView));
        }

        /// <summary>
        /// Occurs when settings button is clicked.
        /// </summary>
        public event EventHandler? SettingsButtonClick
        {
            add
            {
                settingsButton.Clicked += value;
            }

            remove
            {
                settingsButton.Clicked -= value;
            }
        }

        /// <summary>
        /// Gets 'Show Settings' button.
        /// </summary>
        public ImageButton SettingsButton => settingsButton;

        /// <summary>
        /// Gets 'Show Keyboard' button.
        /// </summary>
        public ImageButton KeyboardButton => keyboardButton;

        /// <summary>
        /// Gets label view which contains title text.
        /// </summary>
        public Label Label => label;
    }
}
