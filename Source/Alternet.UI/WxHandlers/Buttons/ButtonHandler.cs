using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class ButtonHandler : WxControlHandler, IButtonHandler
    {
        private ControlStateImages? stateImages;

        public ButtonHandler()
        {
        }

        /// <summary>
        /// Gets a <see cref="Button"/> this handler provides the implementation for.
        /// </summary>
        public new Button Control => (Button)base.Control;

        public bool HasBorder
        {
            get
            {
                return NativeControl.HasBorder;
            }

            set
            {
                NativeControl.HasBorder = value;
            }
        }

        public new Native.Button NativeControl => (Native.Button)base.NativeControl!;

        public bool IsDefault
        {
            get => NativeControl.IsDefault;
            set => NativeControl.IsDefault = value;
        }

        public bool ExactFit
        {
            get => NativeControl.ExactFit;
            set => NativeControl.ExactFit = value;
        }

        public bool IsCancel
        {
            get => NativeControl.IsCancel;
            set => NativeControl.IsCancel = value;
        }

        public ControlStateImages StateImages
        {
            get
            {
                if (stateImages == null)
                {
                    stateImages = new ControlStateImages();
                    stateImages.PropertyChanged += StateImages_PropertyChanged;
                }

                return stateImages;
            }

            set
            {
                if (value is null)
                    throw new ArgumentNullException(nameof(StateImages));
                if (stateImages != null)
                    stateImages.PropertyChanged -= StateImages_PropertyChanged;
                stateImages = value;
                stateImages.PropertyChanged += StateImages_PropertyChanged;
                ApplyStateImages();
                OnImageChanged();
            }
        }

        public bool TextVisible
        {
            get => NativeControl.TextVisible;
            set => NativeControl.TextVisible = value;
        }

        public GenericDirection TextAlign
        {
            get => (GenericDirection)NativeControl.TextAlign;
            set => NativeControl.TextAlign = (int)value;
        }

        public void SetImagePosition(GenericDirection dir)
        {
            NativeControl.SetImagePosition((int)dir);
        }

        public void SetImageMargins(double x, double y)
        {
            NativeControl.SetImageMargins(x, y);
        }

        internal override Native.Control CreateNativeControl()
        {
            return new Native.Button();
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            if (BaseApplication.IsWindowsOS)
                Control.UserPaint = true;

            NativeControl.Text = Control.Text;

            InitializeStateImages();

            Control.TextChanged += Control_TextChanged;
            NativeControl.Click = NativeControl_Click;
        }

        protected override void OnDetach()
        {
            base.OnDetach();

            Control.TextChanged -= Control_TextChanged;
            NativeControl.Click = null;
        }

        private void ApplyStateImages()
        {
            var nativeControl = NativeControl;
            var images = StateImages;
            nativeControl.NormalImage = (UI.Native.Image?)images.Normal?.Handler;
            nativeControl.HoveredImage = (UI.Native.Image?)images.Hovered?.Handler;
            nativeControl.PressedImage = (UI.Native.Image?)images.Pressed?.Handler;
            nativeControl.DisabledImage = (UI.Native.Image?)images.Disabled?.Handler;
            nativeControl.FocusedImage = (UI.Native.Image?)images.Focused?.Handler;
        }

        private void OnImageChanged()
        {
            Control.PerformLayout();
        }

        private void StateImages_PropertyChanged(
            object? sender,
            PropertyChangedEventArgs e)
        {
            var nativeControl = NativeControl;
            if (nativeControl == null)
                return;

            var images = StateImages;

            switch (e.PropertyName)
            {
                case nameof(ControlStateImages.Normal):
                    nativeControl.NormalImage = (UI.Native.Image?)images.Normal?.Handler;
                    break;
                case nameof(ControlStateImages.Hovered):
                    nativeControl.HoveredImage = (UI.Native.Image?)images.Hovered?.Handler;
                    break;
                case nameof(ControlStateImages.Pressed):
                    nativeControl.PressedImage = (UI.Native.Image?)images.Pressed?.Handler;
                    break;
                case nameof(ControlStateImages.Disabled):
                    nativeControl.DisabledImage = (UI.Native.Image?)images.Disabled?.Handler;
                    break;
                default:
                    throw new Exception();
            }

            OnImageChanged();
        }

        private void InitializeStateImages()
        {
            var nativeControl = NativeControl;

            var images = new ControlStateImages();

            var normalImage = nativeControl.NormalImage;
            if (normalImage != null)
                images.Normal = new Bitmap(normalImage);

            var hoveredImage = nativeControl.HoveredImage;
            if (hoveredImage != null)
                images.Hovered = new Bitmap(hoveredImage);

            var pressedImage = nativeControl.PressedImage;
            if (pressedImage != null)
                images.Pressed = new Bitmap(pressedImage);

            var disabledImage = nativeControl.DisabledImage;
            if (disabledImage != null)
                images.Disabled = new Bitmap(disabledImage);

            StateImages = images;
        }

        private void NativeControl_Click()
        {
            Control.RaiseClick(EventArgs.Empty);
        }

        private void Control_TextChanged(object? sender, EventArgs e)
        {
            NativeControl.Text = Control.Text;
        }
    }
}