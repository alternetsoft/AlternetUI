using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Alternet.UI.Integration.VisualStudio.Services;
using Microsoft.VisualStudio.Shell;

using WpfMouseButton = System.Windows.Input.MouseButton;
using WpfModifierKeys = System.Windows.Input.ModifierKeys;
using Alternet.UI.Integration.Remoting;
using Alternet.UI.Integration.VisualStudio.Models;
using PInvoke;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;

using Alternet.UI.Integration;

#pragma warning disable VSTHRD100, VSTHRD010, VSTHRD110

namespace Alternet.UI.Integration.VisualStudio.Views
{
    public partial class AlternetUIPreviewer : UserControl, IDisposable
    {
        private PreviewerProcess _process;
        private bool _centerPreviewer;
        //private Size _lastBitmapSize;

        private BitmapImage windowImageSource;

        public AlternetUIPreviewer()
        {
            InitializeComponent();

            InitializePreviewHost();

            Update(null);

            Loaded += AlternetUIPreviewer_Loaded;

            previewScroller.ScrollChanged += PreviewScroller_ScrollChanged;
            previewScroller.SizeChanged += PreviewScroller_SizeChanged;
        }

        private void InitializePreviewHost()
        {
            //hostPanel = new System.Windows.Forms.Panel();
            //windowsFormsHost.Child = hostPanel;
        }

        private void AlternetUIPreviewer_Loaded(object sender, RoutedEventArgs e)
        {
            // Debugging will cause Loaded/Unloaded events to fire, we only want to do this
            // the first time the designer is loaded, so unsub
            Loaded -= AlternetUIPreviewer_Loaded;
            //_centerPreviewer = true;
        }

        public PreviewerProcess Process
        {
            get => _process;
            set
            {
                if (_process != null)
                {
                    _process.ErrorChanged -= Update;
                    _process.PreviewDataReceived -= Update;
                }

                _process = value;

                if (_process != null)
                {
                    _process.ErrorChanged += Update;
                    _process.PreviewDataReceived += Update;
                }

                Update(_process?.PreviewData);
            }
        }

        public void Dispose()
        {
            Process = null;
            Update(null);
        }

        protected override void OnDpiChanged(DpiScale oldDpi, DpiScale newDpi)
            => Update(_process?.PreviewData);

        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            if ((Keyboard.Modifiers & WpfModifierKeys.Shift) == WpfModifierKeys.Shift)
            {
                previewScroller.ScrollToHorizontalOffset(
                       previewScroller.HorizontalOffset - (2 * e.Delta) / 120 * 48);

                e.Handled = true;
            }
            //else if (Keyboard.Modifiers == WpfModifierKeys.Control)
            //{
            //    var designer = FindParent<AlternetUIDesigner>(this);

            //    if (designer.TryProcessZoomLevelValue(out var currentZoomLevel))
            //    {
            //        currentZoomLevel += e.Delta > 0 ? 0.25 : -0.25;

            //        if (currentZoomLevel < 0.125)
            //        {
            //            currentZoomLevel = 0.125;
            //        }
            //        else if (currentZoomLevel > 8)
            //        {
            //            currentZoomLevel = 8;
            //        }

            //        designer.ZoomLevel = AlternetUIDesigner.FmtZoomLevel(currentZoomLevel * 100);

            //        e.Handled = true;
            //    }
            //}

            base.OnPreviewMouseWheel(e);
        }

        private double GetScaling()
        {
            var result = Process?.Scaling ?? 1;
            return result > 0 ? result : 1;
        }

        private async void Update(object sender, EventArgs e)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            try
            {
                Update(_process.PreviewData);
            }
            catch (Exception ex)
            {
                Log.Error($"Error updating previewer: {ex}");
            }
        }

        double GetDpiScale()
        {
            var source = PresentationSource.FromVisual(this);

            if (source != null)
                return 96.0 * source.CompositionTarget.TransformToDevice.M11;

            return 1;
        }

        static double? cachedDpiScale;

        static double DpiScale
        {
            get
            {
                if (cachedDpiScale == null)
                {
                    var dpiXProperty = typeof(SystemParameters).GetProperty(
                        "DpiX",
                        BindingFlags.NonPublic | BindingFlags.Static);
                    var dpiX = (int)dpiXProperty.GetValue(null, null);
                    cachedDpiScale = dpiX / 96.0;
                }

                return cachedDpiScale.Value;
            }
        }

        static int ScaleWithDpi(int value) => (int)(DpiScale * value);
        static System.Drawing.Size ScaleWithDpi(System.Drawing.Size size) =>
            new System.Drawing.Size(ScaleWithDpi(size.Width), ScaleWithDpi(size.Height));


        private void PreviewScroller_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //if (desiredPreviewSize.Width == 0 || desiredPreviewSize.Height == 0)
            //    return;

            //var newSize = desiredPreviewSize;
            //newSize = ConstrainPreviewSizeToFit(newSize);

            //var actualSize = GetPreviewActualSize();
            //if (actualSize == newSize)
            //    return;

            //windowsFormsHost.Width = newSize.Width;
            //windowsFormsHost.Height = newSize.Height;
            //hostPanel.Size = ScaleWithDpi(newSize);
            //User32.MoveWindow(hostedWindowHandle, 0, 0, newSize.Width, newSize.Height, true);
            //Window.GetWindow(this)?.InvalidateVisual();
        }

        private System.Drawing.Size ConstrainPreviewSizeToFit(System.Drawing.Size newSize)
        {
            if ((int)previewScroller.ActualHeight < newSize.Height)
                newSize.Height = (int)previewScroller.ActualHeight;
            if ((int)previewScroller.ActualWidth < newSize.Width)
                newSize.Width = (int)previewScroller.ActualWidth;
            return newSize;
        }

        private void Update(PreviewData preview)
        {
            if (preview != null && preview.ImageFileName != null)
            {
                loading.Visibility = Visibility.Collapsed;
                previewScroller.Visibility = Visibility.Visible;

                windowImageSource = new BitmapImage();
                windowImageSource.BeginInit();
                windowImageSource.CacheOption = BitmapCacheOption.OnLoad;
                windowImageSource.UriSource = new Uri(preview.ImageFileName);
                windowImageSource.EndInit();

                try
                {
                    File.Delete(preview.ImageFileName);
                }
                catch
                {
                }

                windowImage.Source = windowImageSource;
                windowImage.Width = windowImageSource.Width;
                windowImage.Height = windowImageSource.Height;
            }
            else
            {
                loading.Visibility = Visibility.Visible;
                previewScroller.Visibility = Visibility.Collapsed;
            }

            Window.GetWindow(this)?.Activate();
            Window.GetWindow(this)?.InvalidateVisual();
        }

        private void PreviewScroller_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (_centerPreviewer)
            {
                // We can't do this in Update because the Scroll info may not be updated 
                // yet and the scrollable size may still be old
                previewScroller.ScrollToHorizontalOffset(previewScroller.ScrollableWidth / 2);
                previewScroller.ScrollToVerticalOffset(previewScroller.ScrollableHeight / 2);
                _centerPreviewer = false;
            }
            else
            {

            }
        }
        private static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            //get parent item
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            //we've reached the end of the tree
            if (parentObject == null)
                return null;

            //check if the parent matches the type we're looking for
            T parent = parentObject as T;
            if (parent != null)
                return parent;
            else
                return FindParent<T>(parentObject);
        }

        /*private static UIMouseButton GetButton(WpfMouseButton button)
        {
            switch (button)
            {
                case WpfMouseButton.Left: return UIMouseButton.Left;
                case WpfMouseButton.Middle: return UIMouseButton.Middle;
                case WpfMouseButton.Right: return UIMouseButton.Right;
                default: throw new Exception();
            }
        }*/

        /*private static UIModifierKeys[] GetModifiers(MouseEventArgs e)
        {
            var result = new List<UIModifierKeys>();

            if ((Keyboard.Modifiers & WpfModifierKeys.Alt) != 0)
            {
                result.Add(UIModifierKeys.Alt);
            }

            if ((Keyboard.Modifiers & WpfModifierKeys.Control) != 0)
            {
                result.Add(UIModifierKeys.Control);
            }

            if ((Keyboard.Modifiers & WpfModifierKeys.Shift) != 0)
            {
                result.Add(UIModifierKeys.Shift);
            }

            if ((Keyboard.Modifiers & WpfModifierKeys.Windows) != 0)
            {
                result.Add(UIModifierKeys.Windows);
            }

            //if (e.LeftButton == MouseButtonState.Pressed)
            //{
            //    result.Add(UIModifierKeys.LeftMouseButton);
            //}

            //if (e.RightButton == MouseButtonState.Pressed)
            //{
            //    result.Add(UIModifierKeys.RightMouseButton);
            //}

            //if (e.MiddleButton == MouseButtonState.Pressed)
            //{
            //    result.Add(UIModifierKeys.MiddleMouseButton);
            //}

            return result.ToArray();
        }*/
    }
}
#pragma warning restore VSTHRD100, VSTHRD010, VSTHRD110