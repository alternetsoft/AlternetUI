using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class NativeButtonHandler : ButtonHandler
    {
        private ControlStateImages? stateImages;

        public override bool HasBorder
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

        public override bool IsDefault
        {
            get => NativeControl.IsDefault;
            set => NativeControl.IsDefault = value;
        }

        public override bool ExactFit
        {
            get => NativeControl.ExactFit;
            set => NativeControl.ExactFit = value;
        }

        public override bool IsCancel
        {
            get => NativeControl.IsCancel;
            set => NativeControl.IsCancel = value;
        }

        public override ControlStateImages StateImages
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

        public override bool TextVisible
        {
            get => NativeControl.TextVisible;
            set => NativeControl.TextVisible = value;
        }

        public override GenericDirection TextAlign
        {
            get => (GenericDirection)NativeControl.TextAlign;
            set => NativeControl.TextAlign = (int)value;
        }

        public override void SetImagePosition(GenericDirection dir)
        {
            NativeControl.SetImagePosition((int)dir);
        }

        public override void SetImageMargins(double x, double y)
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
            nativeControl.NormalImage = (UI.Native.Image?)images.Normal?.NativeObject;
            nativeControl.HoveredImage = (UI.Native.Image?)images.Hovered?.NativeObject;
            nativeControl.PressedImage = (UI.Native.Image?)images.Pressed?.NativeObject;
            nativeControl.DisabledImage = (UI.Native.Image?)images.Disabled?.NativeObject;
            nativeControl.FocusedImage = (UI.Native.Image?)images.Focused?.NativeObject;
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
                    nativeControl.NormalImage = (UI.Native.Image?)images.Normal?.NativeObject;
                    break;
                case nameof(ControlStateImages.Hovered):
                    nativeControl.HoveredImage = (UI.Native.Image?)images.Hovered?.NativeObject;
                    break;
                case nameof(ControlStateImages.Pressed):
                    nativeControl.PressedImage = (UI.Native.Image?)images.Pressed?.NativeObject;
                    break;
                case nameof(ControlStateImages.Disabled):
                    nativeControl.DisabledImage = (UI.Native.Image?)images.Disabled?.NativeObject;
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