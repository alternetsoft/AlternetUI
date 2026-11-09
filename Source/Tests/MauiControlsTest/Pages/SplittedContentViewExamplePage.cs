using Alternet.Maui;
using Alternet.Maui.Extensions;

using Microsoft.Maui.Controls;

using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AllQuickStarts
{
    public partial class SplittedContentViewExamplePage : ContentPage
    {
        private readonly SplittedContentView sampleView;
        private readonly BoxView firstChild = new ();
        private readonly BoxView secondChild = new ();

        public SplittedContentViewExamplePage()
        {
            sampleView = new SplittedContentView();
            sampleView.FirstContent = firstChild;
            sampleView.SecondContent = secondChild;
            var isDark = Alternet.UI.App.IsDark;
            firstChild.Color = Alternet.Drawing.ThemedColors.Blue.GetColor(isDark).ToMaui();
            secondChild.Color = Alternet.Drawing.ThemedColors.Green.GetColor(isDark).ToMaui();

            var toolbar = new SimpleToolBarView();

            toolbar.AddButton(
                "Toggle orientation",
                null,
                null,
                () =>
                {
                    sampleView.IsVertical = !sampleView.IsVertical;
                });

            toolbar.AddButton(
                "Toggle visibility",
                null,
                null,
                () =>
                {
                    sampleView.IsFirstPaneVisible = !sampleView.IsFirstPaneVisible;
                });

            toolbar.AddExpandingSpace();
            toolbar.IsTopBorderVisible = true;
            toolbar.IsBottomBorderVisible = true;

            var grid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Star }
                }
            };

            grid.Add(toolbar, 0, 0);
            grid.Add(sampleView, 0, 1);

            Content = grid;
        }
    }
}
