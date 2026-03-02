using System;
using System.ComponentModel;
using System.IO;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal class WxAnimationPlayerHandler : WxControlHandler, IAnimationPlayerHandler
    {
        private readonly bool useGeneric;

        public WxAnimationPlayerHandler(bool useGeneric)
        {
            this.useGeneric = useGeneric;
        }

        public new Native.AnimationControl NativeControl
            => (Native.AnimationControl)base.NativeControl;

        public new AnimationPlayer? Control => (AnimationPlayer?)base.Control;

        [Browsable(false)]
        public virtual uint FrameCount
        {
            get => NativeControl.GetFrameCount();
        }

        public virtual bool UseGeneric
        {
            get
            {
                return NativeControl.UseGeneric;
            }

            set
            {
                NativeControl.UseGeneric = value;
            }
        }

        [Browsable(false)]
        public virtual SizeI AnimationSize
        {
            get => NativeControl.GetSize();
        }

        [Browsable(false)]
        public virtual bool IsOk
        {
            get => NativeControl.IsOk();
        }

        public virtual bool Play()
        {
            return NativeControl.Play();
        }

        public virtual void Stop()
        {
            NativeControl.Stop();
        }

        public virtual bool IsPlaying()
        {
            return NativeControl.IsPlaying();
        }

        public virtual bool LoadFile(string filename, AnimationType type = AnimationType.Any)
        {
            return NativeControl.LoadFile(filename, (int)type);
        }

        public virtual bool Load(Stream stream, AnimationType type = AnimationType.Any)
        {
            using var inputStream = new UI.Native.InputStream(stream);
            return NativeControl.Load(inputStream, (int)type);
        }

        public virtual void SetInactiveBitmap(ImageSet? imageSet)
        {
            NativeControl.SetInactiveBitmap((UI.Native.ImageSet?)imageSet?.Handler);
        }

        public virtual int GetDelay(uint i)
        {
            return NativeControl.GetDelay(i);
        }

        public virtual GenericImage GetFrame(uint i)
        {
            return WxGenericImageHandler.Create(NativeControl.GetFrame(i));
        }

        internal override Native.Control CreateNativeControl()
        {
            return new Native.AnimationControl();
        }

        protected override void OnAttach()
        {
            base.OnAttach();
            if (useGeneric)
                UseGeneric = true;
            Control?.DoInsideLayout(() =>
            {
                Control.VerticalAlignment = VerticalAlignment.Top;
                Control.HorizontalAlignment = HorizontalAlignment.Left;
            });
        }
    }
}