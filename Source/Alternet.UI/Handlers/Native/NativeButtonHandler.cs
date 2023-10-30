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
            NativeControl.Click += NativeControl_Click;
        }

        protected override void OnDetach()
        {
            base.OnDetach();

            Control.TextChanged -= Control_TextChanged;
            NativeControl.Click -= NativeControl_Click;
        }

        private void ApplyStateImages()
        {
            var nativeControl = NativeControl;
            var images = StateImages;
            nativeControl.NormalImage = images.NormalImage?.NativeImage;
            nativeControl.HoveredImage = images.HoveredImage?.NativeImage;
            nativeControl.PressedImage = images.PressedImage?.NativeImage;
            nativeControl.DisabledImage = images.DisabledImage?.NativeImage;
            nativeControl.FocusedImage = images.FocusedImage?.NativeImage;
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
                case nameof(ControlStateImages.NormalImage):
                    nativeControl.NormalImage = images.NormalImage?.NativeImage;
                    break;
                case nameof(ControlStateImages.HoveredImage):
                    nativeControl.HoveredImage = images.HoveredImage?.NativeImage;
                    break;
                case nameof(ControlStateImages.PressedImage):
                    nativeControl.PressedImage = images.PressedImage?.NativeImage;
                    break;
                case nameof(ControlStateImages.DisabledImage):
                    nativeControl.DisabledImage = images.DisabledImage?.NativeImage;
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
                images.NormalImage = new Bitmap(normalImage);

            var hoveredImage = nativeControl.HoveredImage;
            if (hoveredImage != null)
                images.HoveredImage = new Bitmap(hoveredImage);

            var pressedImage = nativeControl.PressedImage;
            if (pressedImage != null)
                images.PressedImage = new Bitmap(pressedImage);

            var disabledImage = nativeControl.DisabledImage;
            if (disabledImage != null)
                images.DisabledImage = new Bitmap(disabledImage);

            StateImages = images;
        }

        private void NativeControl_Click(object? sender, System.EventArgs? e)
        {
            if (e is null)
                throw new ArgumentNullException(nameof(e));

            Control.RaiseClick(e);
        }

        private void Control_TextChanged(object? sender, System.EventArgs? e)
        {
            if (e is null)
                throw new ArgumentNullException(nameof(e));

            NativeControl.Text = Control.Text;
        }
    }
}