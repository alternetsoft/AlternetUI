using Alternet.Drawing;
using Alternet.UI;
using System;
using System.Linq;

namespace DrawingSample
{
    partial class BrushesAndPensPageSettings : Panel
    {
        private readonly Label dashStyleLabel = new("Dash Style:")
        {
        };

        private readonly Label lineCapLabel = new("Line Cap:")
        {
        };

        private readonly Label lineJoinLabel = new("Line Join:")
        {
        };

        private readonly EnumPicker dashStyleComboBox = new()
        {
        };

        private readonly EnumPicker lineCapComboBox = new()
        {
        };

        private readonly EnumPicker lineJoinComboBox = new()
        {
        };

        private BrushesAndPensPage? page;

        public BrushesAndPensPageSettings()
        {
            DoInsideLayout(() =>
            {
                InitializeComponent();

                Group(dashStyleLabel, lineCapLabel, lineJoinLabel).GroupName("labels");
                Group(dashStyleComboBox, lineCapComboBox, lineJoinComboBox).GroupName("editors");

                Children.Add(new HorizontalStackPanel().WithChildren(dashStyleLabel, dashStyleComboBox));
                Children.Add(new HorizontalStackPanel().WithChildren(lineCapLabel, lineCapComboBox));
                Children.Add(new HorizontalStackPanel().WithChildren(lineJoinLabel, lineJoinComboBox));

                GetNamedGroup("labels", recursive: true)
                .Margin(new(0, 5, 10, 0))
                .VerticalAlignment(VerticalAlignment.Center)
                .MinWidthToMaxPreferred();

                GetNamedGroup("editors", recursive: true)
                .Margin(new(0, 5, 5, 0))
                .HorizontalAlignment(HorizontalAlignment.Fill);

                GetGroup<ColorPicker>(recursive: true).ForEach((c) =>
                {
                    c.ListBox.AddTransparentColor();
                });
            });
        }

        public void Initialize(BrushesAndPensPage page)
        {
            DataContext = page;
            this.page = page;

            brushComboBox.EnumType = typeof(BrushesAndPensPage.BrushType);
            hatchStyleComboBox.EnumType = typeof(BrushHatchStyle);
            dashStyleComboBox.EnumType = typeof(DashStyle);
            lineJoinComboBox.EnumType = typeof(LineJoin);
            lineCapComboBox.EnumType = typeof(LineCap);

            dashStyleComboBox.Value = page.PenDashStyle;
            dashStyleComboBox.ValueChanged += (s, e) =>
            {
                page.PenDashStyle = dashStyleComboBox.ValueAs<DashStyle>();
            };

            lineCapComboBox.Value = page.LineCap;
            lineCapComboBox.ValueChanged += (s, e) =>
            {
                page.LineCap = lineCapComboBox.ValueAs<LineCap>();
            };

            lineJoinComboBox.Value = page.LineJoin;
            lineJoinComboBox.ValueChanged += (s, e) =>
            {
                page.LineJoin = lineJoinComboBox.ValueAs<LineJoin>();
            };

            hatchStyleComboBox.Value = page.HatchStyle;
            hatchStyleComboBox.ValueChanged += (s, e) =>
            {
                page.HatchStyle = hatchStyleComboBox.ValueAs<BrushHatchStyle>();
            };

            brushComboBox.Value = page.Brush;
            brushComboBox.ValueChanged += (s, e) =>
            {
                page.Brush = brushComboBox.ValueAs<BrushesAndPensPage.BrushType>();
            };

            shapeCountSlider.Value = page.ShapeCount;
            shapeCountSlider.ValueChanged += (s, e) =>
            {
                page.ShapeCount = shapeCountSlider.Value;
            };

            brushColor1Picker.Value = page.BrushColor1;
            brushColor1Picker.ValueChanged += (s, e) =>
            {
                page.BrushColor1 = brushColor1Picker.Value;
            };

            brushColor2Picker.Value = page.BrushColor2;
            brushColor2Picker.ValueChanged += (s, e) =>
            {
                page.BrushColor2 = brushColor2Picker.Value;
            };

            penColorPicker.Value = page.PenColor;
            penColorPicker.ValueChanged += (s, e) =>
            {
                page.PenColor = penColorPicker.Value;
            };

            penWidthSlider.Value = page.PenWidth;
            penWidthSlider.ValueChanged += (s, e) =>
            {
                page.PenWidth = penWidthSlider.Value;
            };

            rectanglesIncludedCheckBox.IsChecked = page.RectanglesIncluded;
            rectanglesIncludedCheckBox.CheckedChanged += (s, e) =>
            {
                page.RectanglesIncluded = rectanglesIncludedCheckBox.IsChecked;
            };

            ellipsesIncludedCheckBox.IsChecked = page.EllipsesIncluded;
            ellipsesIncludedCheckBox.CheckedChanged += (s, e) =>
            {
                page.EllipsesIncluded = ellipsesIncludedCheckBox.IsChecked;
            };
        }
    }
}