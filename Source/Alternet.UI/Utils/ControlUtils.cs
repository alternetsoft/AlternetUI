using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods related to the control drawing.
    /// </summary>
    public static class ControlUtils
    {
        /// <summary>
        /// Shows popup menu.
        /// </summary>
        public static void ShowPopupMenu(
            IControl control,
            ContextMenu? menu,
            double x = -1,
            double y = -1)
        {
            if (menu is null || menu.Items.Count == 0)
                return;
            var e = new CancelEventArgs();
            menu.RaiseOpening(e);
            if (e.Cancel)
                return;
            ((UI.Native.Control)control.NativeControl).ShowPopupMenu(
                MenuItemHandler.GetMenuHandle(menu),
                x,
                y);
            menu.RaiseClosing(e);
        }

        /// <summary>
        /// Sets <see cref="ComboBox.IsEditable"/> property for all the controls in the set.
        /// </summary>
        /// <param name="value"><c>true</c> enables editing of the text;
        /// <c>false</c> disables it.</param>
        /// <param name="controlSet">Set of controls.</param>
        public static ControlSet IsEditable(this ControlSet controlSet, bool value)
        {
            foreach (var item in controlSet.Items)
            {
                if (item is ComboBox comboBox)
                    comboBox.IsEditable = value;
            }

            return controlSet;
        }

        /// <summary>
        /// Creates new <see cref="HorizontalStackPanel"/> and adds
        /// it to the <see cref="Control.Children"/>.
        /// </summary>
        public static HorizontalStackPanel AddHorizontalStackPanel(this Control control)
        {
            var result = new HorizontalStackPanel
            {
                Parent = control,
            };
            return result;
        }

        /// <summary>
        /// Creates new <see cref="TabControl"/> and adds it to the <see cref="Control.Children"/>.
        /// </summary>
        public static TabControl AddTabControl(this Control control)
        {
            var result = new TabControl
            {
                Parent = control,
            };
            return result;
        }

        /// <summary>
        /// Creates new <see cref="Button"/> and adds it to the <see cref="Control.Children"/>.
        /// </summary>
        public static Button AddButton(this Control control, string text, Action? action = null)
        {
            var result = new Button(text)
            {
                Parent = control,
            };

            if (action is not null)
                result.Click += Result_Click;

            return result;

            void Result_Click(object? sender, EventArgs e)
            {
                action.Invoke();
            }
        }

        /// <summary>
        /// Adds multiple buttons.
        /// </summary>
        /// <param name="buttons">Array of title and action.</param>
        /// <returns><see cref="ControlSet"/> with list of created buttons.</returns>
        /// <param name="control">Parent control.</param>
        public static ControlSet AddButtons(this Control control, params (string, Action?)[] buttons)
        {
            List<Control> result = new();

            control.DoInsideLayout(() =>
            {
                foreach (var item in buttons)
                {
                    var button = AddButton(control, item.Item1, item.Item2);
                    result.Add(button);
                }
            });

            return new(result);
        }

        /// <summary>
        /// Adds multiple labels.
        /// </summary>
        /// <param name="control">Parent control.</param>
        /// <param name="text">Array of label text.</param>
        /// <returns><see cref="ControlSet"/> with list of created labels.</returns>
        public static ControlSet AddLabels(this Control control, params string[] text)
        {
            List<Control> result = new();

            control.DoInsideLayout(() =>
            {
                foreach (var item in text)
                {
                    var label = AddLabel(control, item);
                    result.Add(label);
                }
            });

            return new(result);
        }

        /// <summary>
        /// Creates new <see cref="CheckBox"/> and adds it to the <see cref="Control.Children"/>.
        /// </summary>
        public static CheckBox AddCheckBox(this Control control, string text, Action? action = null)
        {
            var result = new CheckBox(text)
            {
                Parent = control,
            };

            if (action is not null)
                result.CheckedChanged += Result_CheckedChanged;

            return result;

            void Result_CheckedChanged(object? sender, EventArgs e)
            {
                action.Invoke();
            }
        }

        /// <summary>
        /// Creates new <see cref="Label"/> and adds it to the <see cref="Control.Children"/>.
        /// </summary>
        public static Label AddLabel(this Control control, string text)
        {
            var result = new Label(text)
            {
                Parent = control,
            };

            return result;
        }

        /// <summary>
        /// Creates new <see cref="VerticalStackPanel"/> and adds it to the <see cref="Control.Children"/>.
        /// </summary>
        public static VerticalStackPanel AddVerticalStackPanel(this Control control)
        {
            var result = new VerticalStackPanel
            {
                Parent = control,
            };
            return result;
        }

        /// <summary>
        /// Creates new <see cref="GroupBox"/> and adds it to the <see cref="Control.Children"/>.
        /// </summary>
        public static GroupBox AddGroupBox(this Control control, string? title = default)
        {
            var result = new GroupBox
            {
                Parent = control,
            };

            if (title is not null)
                result.Title = title;
            return result;
        }

        /// <summary>
        /// Creates new <see cref="GroupBox"/> with vertical layout and adds
        /// it to the <see cref="Control.Children"/>.
        /// </summary>
        public static GroupBox AddVerticalGroupBox(this Control control, string? title = default)
        {
            var result = new GroupBox
            {
                Layout = LayoutStyle.Vertical,
                Parent = control,
            };

            if (title is not null)
                result.Title = title;
            return result;
        }

        /// <summary>
        /// Creates new <see cref="GroupBox"/> with horizontal layout and adds
        /// it to the <see cref="Control.Children"/>.
        /// </summary>
        public static GroupBox AddHorizontalGroupBox(this Control control, string? title = default)
        {
            var result = new GroupBox
            {
                Layout = LayoutStyle.Horizontal,
                Parent = control,
            };

            if (title is not null)
                result.Title = title;
            return result;
        }

        /// <summary>
        /// Creates new <see cref="StackPanel"/> and adds it to the <see cref="Control.Children"/>.
        /// </summary>
        public static StackPanel AddStackPanel(this Control control, bool isVertical = true)
        {
            StackPanelOrientation orientation;

            if (isVertical)
                orientation = StackPanelOrientation.Vertical;
            else
                orientation = StackPanelOrientation.Horizontal;

            var result = new StackPanel
            {
                Parent = control,
                Orientation = orientation,
            };
            return result;
        }

        /// <inheritdoc cref="CustomControlPainter.GetCheckBoxSize"/>
        public static SizeD GetCheckBoxSize(
            Control control,
            CheckState checkState,
            GenericControlState controlState)
        {
            return CustomControlPainter.Current.GetCheckBoxSize(control, checkState, controlState);
        }

        /// <inheritdoc cref="CustomControlPainter.DrawCheckBox"/>
        public static void DrawCheckBox(
            this Graphics canvas,
            Control control,
            RectD rect,
            CheckState checkState,
            GenericControlState controlState)
        {
            CustomControlPainter.Current.DrawCheckBox(
                control,
                canvas,
                rect,
                checkState,
                controlState);
        }

        /// <summary>
        /// Initializes a tuple with two instances of the <see cref="ImageSet"/> class
        /// from the specified <see cref="Stream"/> which contains svg data. Images are loaded
        /// for the normal and disabled states using <see cref="Control.GetSvgColor"/>.
        /// </summary>
        /// <param name="stream">Stream with svg data.</param>
        /// <param name="size">Image size in pixels.</param>
        /// <param name="control">Control which <see cref="Control.GetSvgColor"/>
        /// method is called to get color information.</param>
        /// <returns></returns>
        public static (ImageSet Normal, ImageSet Disabled) GetNormalAndDisabledSvg(
            Stream stream,
            SizeI size,
            Control control)
        {
            var image = ImageSet.FromSvgStream(
                stream,
                size,
                control.GetSvgColor(KnownSvgColor.Normal),
                control.GetSvgColor(KnownSvgColor.Disabled));
            return image;
        }

        /// <summary>
        /// Sets background of the control's parents to Red, Green, Blue and
        /// Yellow colors.
        /// </summary>
        /// <param name="control">Specifies the control which parent's background
        /// is changed</param>
        public static void SetDebugBackgroundToParents(Control? control)
        {
            static Control? SetParentBackground(Control? control, Brush brush)
            {
                if (control == null)
                    return null;
                Control? parent = control?.Parent;
                if (parent != null)
                    parent.Background = brush;
                return parent;
            }

            control = SetParentBackground(control, Brushes.Red);
            control = SetParentBackground(control, Brushes.Green);
            control = SetParentBackground(control, Brushes.Blue);
            SetParentBackground(control, Brushes.Yellow);
        }

        /// <summary>
        /// Initializes a tuple with two instances of the <see cref="ImageSet"/> class
        /// from the specified url which contains svg data. Images are loaded
        /// for the normal and disabled states using <see cref="Control.GetSvgColor"/>.
        /// </summary>
        /// <param name="size">Image size in pixels. If it is not specified,
        /// <see cref="ToolBar.GetDefaultImageSize(Control)"/> is used to get image size.</param>
        /// <param name="control">Control which <see cref="Control.GetSvgColor"/>
        /// method is called to get color information.</param>
        /// <returns></returns>
        /// <param name="url">"embres" or "file" url with svg image data.</param>
        /// <returns></returns>
        public static (ImageSet Normal, ImageSet Disabled) GetNormalAndDisabledSvg(
            string url,
            Control control,
            SizeI? size = null)
        {
            size ??= ToolBar.GetDefaultImageSize(control);

            using var stream = ResourceLoader.StreamFromUrl(url);
            var image = ImageSet.FromSvgStream(
                stream,
                size.Value,
                control.GetSvgColor(KnownSvgColor.Normal),
                control.GetSvgColor(KnownSvgColor.Disabled));
            return image;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSet"/> class
        /// from the specified url which points to svg file or resource.
        /// </summary>
        /// <remarks>
        /// This is similar to <see cref="Image.FromSvgUrl"/> but uses
        /// <see cref="Control.GetDPI"/> and <see cref="ToolbarUtils.GetDefaultImageSize(double)"/>
        /// to get appropriate image size which is best suitable for toolbars.
        /// </remarks>
        /// <param name="url">The file or embedded resource url with Svg data used
        /// to load the image.</param>
        /// <param name="control">Control which <see cref="Control.GetDPI"/> method
        /// is used to get DPI.</param>
        /// <returns><see cref="ImageSet"/> instance loaded from Svg data for use
        /// on the toolbars.</returns>
        /// <param name="color">Svg fill color. Optional.
        /// If provided, svg fill color is changed to the specified value.</param>
        public static ImageSet FromSvgUrlForToolbar(string url, Control control, Color? color = null)
        {
            var imageSize = ToolBar.GetDefaultImageSize(control);
            var result = ImageSet.FromSvgUrl(url, imageSize.Width, imageSize.Height, color);
            return result;
        }
    }
}
