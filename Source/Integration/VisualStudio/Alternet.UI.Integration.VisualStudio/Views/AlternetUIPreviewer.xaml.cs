using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Alternet.UI.Integration.VisualStudio.Services;
using Microsoft.VisualStudio.Shell;
using Serilog;

using UIMouseButton = Alternet.UI.Integration.Remoting.MouseButton;
using UIModifierKeys = Alternet.UI.Integration.Remoting.ModifierKeys;

using WpfMouseButton = System.Windows.Input.MouseButton;
using WpfModifierKeys = System.Windows.Input.ModifierKeys;
using Alternet.UI.Integration.Remoting;
using Alternet.UI.Integration.VisualStudio.Models;
using PInvoke;
using System.Runtime.InteropServices;

#pragma warning disable VSTHRD100, VSTHRD010, VSTHRD110

namespace Alternet.UI.Integration.VisualStudio.Views
{
    public partial class AlternetUIPreviewer : UserControl, IDisposable
    {
        private PreviewerProcess _process;
        private bool _centerPreviewer;
        //private Size _lastBitmapSize;

        private System.Windows.Forms.Panel hostPanel;

        public AlternetUIPreviewer()
        {
            InitializeComponent();

            InitializePreviewHost();

            Update(null);

            Loaded += AlternetUIPreviewer_Loaded;

            previewScroller.ScrollChanged += PreviewScroller_ScrollChanged;
        }

        private void InitializePreviewHost()
        {
            hostPanel = new System.Windows.Forms.Panel();
            windowsFormsHost.Child = hostPanel;
            hostPanel.Size = new System.Drawing.Size(300, 300);
        }

        private void AlternetUIPreviewer_Loaded(object sender, RoutedEventArgs e)
        {
            // Debugging will cause Loaded/Unloaded events to fire, we only want to do this
            // the first time the designer is loaded, so unsub
            Loaded -= AlternetUIPreviewer_Loaded;
            _centerPreviewer = true;
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

        protected override void OnDpiChanged(DpiScale oldDpi, DpiScale newDpi) => Update(_process?.PreviewData);

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
                Log.Logger.Error(ex, "Error updating previewer");
            }
        }

        private void Update(PreviewData preview)
        {
            if (preview != null && preview.WindowHandle != IntPtr.Zero)
            {
                //var style = GetWindowLong(preview.WindowHandle, WindowLongIndexFlags.GWL_STYLE);

                User32.SetParent(preview.WindowHandle, hostPanel.Handle);
                //SetWindowLong(appWin, WindowLongIndexFlags.GWL_STYLE, (SetWindowLongFlags)(WindowStyles.WS_VISIBLE/* | WindowStyles.WS_OVERLAPPEDWINDOW*/));
                //            SetWindowLong(appWin, WindowLongIndexFlags.GWL_STYLE, (SetWindowLongFlags)style);
                //System.Threading.Thread.Sleep(100);
                User32.MoveWindow(preview.WindowHandle, 0, 0, hostPanel.ClientRectangle.Width, hostPanel.ClientRectangle.Height, true);
                InvalidateRect(preview.WindowHandle, IntPtr.Zero, true);
                User32.UpdateWindow(preview.WindowHandle);
            }
        }

        [DllImport("user32.dll")]
        static extern bool InvalidateRect(IntPtr hWnd, IntPtr lpRect, bool bErase);

        //private void Update(BitmapSource bitmap)
        //{
        //    preview.Source = bitmap;

        //    if (bitmap != null)
        //    {
        //        var scaling = VisualTreeHelper.GetDpi(this).DpiScaleX;

        //        // If an error in the Xaml is present, we get a bitmap with width/height = 1
        //        // Which isn't ideal, but also messes up the scroll location since it will
        //        // trigger it to re-center, so only change the size
        //        // if the process shows we don't have an error
        //        if (Process.Error == null)
        //        {
        //            preview.Width = bitmap.Width / scaling;
        //            preview.Height = bitmap.Height / scaling;
        //        }

        //        loading.Visibility = Visibility.Collapsed;
        //        previewScroller.Visibility = Visibility.Visible;

        //        var fullScaling = scaling * Process.Scaling;
        //        var hScale = preview.Width * 2 / fullScaling;
        //        var vScale = preview.Height * 2 / fullScaling;
        //        previewGrid.Margin = new Thickness(hScale, vScale, hScale, vScale);

        //        // The bitmap size only changes if
        //        // 1- The design size changes
        //        // 2- The scaling changes from zoom factor
        //        // 3- The DPI changes
        //        // To ensure we don't have the ScrollViewer end up in a weird place,
        //        // recenter the content if the size changes
        //        if (preview.Width != _lastBitmapSize.Width ||
        //            preview.Height != _lastBitmapSize.Height)
        //        {
        //            _centerPreviewer = true;
        //            _lastBitmapSize = new Size(preview.Width, preview.Height);
        //        }
        //    }
        //    else
        //    {
        //        loading.Visibility = Visibility.Visible;
        //        previewScroller.Visibility = Visibility.Collapsed;
        //    }
        //}

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
        }

        //private void Preview_MouseMove(object sender, MouseEventArgs e)
        //{
        //    var p = e.GetPosition(preview);
        //    var scaling = GetScaling();

        //    Process?.SendInputAsync(new PointerMovedEventMessage
        //    {
        //        X = p.X / scaling,
        //        Y = p.Y / scaling,
        //        Modifiers = GetModifiers(e),
        //    });
        //}

        //private void Preview_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    var p = e.GetPosition(preview);
        //    var scaling = GetScaling();

        //    Process?.SendInputAsync(new PointerPressedEventMessage
        //    {
        //        X = p.X / scaling,
        //        Y = p.Y / scaling,
        //        Button = GetButton(e.ChangedButton),
        //        Modifiers = GetModifiers(e),
        //    });
        //}

        //private void Preview_MouseUp(object sender, MouseButtonEventArgs e)
        //{
        //    var p = e.GetPosition(preview);
        //    var scaling = GetScaling();

        //    Process?.SendInputAsync(new PointerReleasedEventMessage
        //    {
        //        X = p.X / scaling,
        //        Y = p.Y / scaling,
        //        Button = GetButton(e.ChangedButton),
        //        Modifiers = GetModifiers(e),
        //    });
        //}

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

        private static UIMouseButton GetButton(WpfMouseButton button)
        {
            switch (button)
            {
                case WpfMouseButton.Left: return UIMouseButton.Left;
                case WpfMouseButton.Middle: return UIMouseButton.Middle;
                case WpfMouseButton.Right: return UIMouseButton.Right;
                default: throw new Exception();
            }
        }

        private static UIModifierKeys[] GetModifiers(MouseEventArgs e)
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
        }
    }
}
#pragma warning restore VSTHRD100, VSTHRD010, VSTHRD110