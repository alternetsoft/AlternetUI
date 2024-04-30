using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// This is a static control which displays an animation.
    /// This control is useful to display a (small) animation while doing
    /// a long task (e.g. a "throbber").
    /// </summary>
    /// <remarks>
    /// <see cref="AnimationPlayer"/> API is as simple as possible and won't give you full
    /// control on the animation; if you need it then use other controls.
    /// </remarks>
    /// <remarks>
    /// For the platforms where this control has a native implementation, it
    /// may have only limited support for the animation types. Set UseGeneric if you need to
    /// support all of them.
    /// </remarks>
    [ControlCategory("Other")]
    public partial class AnimationPlayer : Control, IAnimationPlayer
    {
        /// <summary>
        /// Gets or sets type of the default driver used inside the <see cref="AnimationPlayer"/>.
        /// </summary>
        public static KnownDriver DefaultDriver = KnownDriver.Generic;

        /// <summary>
        /// Gets or sets function which creates animation player driver used in
        /// the <see cref="AnimationPlayer"/>.
        /// </summary>
        /// <remarks>
        /// If this field is <c>null</c>, <see cref="CreateDefaultPlayerDriver"/>
        /// is used to create animation player driver.
        /// </remarks>
        public static Func<IAnimationPlayer>? CreatePlayerDriver;

        private readonly IAnimationPlayer driver;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationPlayer"/> class.
        /// </summary>
        public AnimationPlayer()
        {
            VerticalAlignment = VerticalAlignment.Top;
            HorizontalAlignment = HorizontalAlignment.Left;

            var fn = CreatePlayerDriver ?? CreateDefaultPlayerDriver;

            driver = fn();
            driver.Control.Parent = this;
        }

        /// <summary>
        /// Enumerates known <see cref="AnimationPlayer"/> drivers.
        /// </summary>
        public enum KnownDriver
        {
            /// <summary>
            /// Native control is used inside the <see cref="AnimationPlayer"/>.
            /// </summary>
            Native,

            /// <summary>
            /// Generic control is used inside the <see cref="AnimationPlayer"/>.
            /// </summary>
            Generic,

            /// <summary>
            /// <see cref="WebBrowser"/> control is used inside the <see cref="AnimationPlayer"/>.
            /// </summary>
            WebBrowser,
        }

        Control IAnimationPlayer.Control { get => this; }

        /// <summary>
        /// Gets the number of frames for this animation.
        /// </summary>
        /// <returns></returns>
        [Browsable(false)]
        public virtual uint FrameCount
        {
            get => driver.FrameCount;
        }

        /// <summary>
        /// Gets the size of the animation in pixels.
        /// </summary>
        /// <returns></returns>
        [Browsable(false)]
        public virtual SizeI AnimationSize
        {
            get => driver.AnimationSize;
        }

        /// <summary>
        /// Returns true if animation data is present.
        /// </summary>
        /// <returns></returns>
        [Browsable(false)]
        public virtual bool IsOk
        {
            get => driver.IsOk;
        }

        /// <summary>
        /// Creates default native animation player driver.
        /// </summary>
        /// <returns></returns>
        public static IAnimationPlayer CreateDefaultPlayerDriver()
        {
            switch (DefaultDriver)
            {
                case KnownDriver.Native:
                    return new NativeAnimationPlayer();
                case KnownDriver.Generic:
                    var result = new NativeAnimationPlayer();
                    result.UseGeneric = true;
                    return result;
                case KnownDriver.WebBrowser:
                default:
                    throw new NotImplementedException(
                        "KnownDriver.WebBrowser is not currently supported.");
            }
        }

        /// <summary>
        /// Starts playing the animation.
        /// </summary>
        /// <returns><c>true</c> if the animation started to play; <c>false</c> otherwise.</returns>
        /// <remarks>
        /// The animation is always played in loop mode (unless the last frame of the animation
        /// has an infinite delay time) and always start from the first frame even if
        /// you stopped it while some other frame was displayed.
        /// </remarks>
        public virtual bool Play()
        {
            return driver.Play();
        }

        /// <summary>
        /// Stops playing the animation.
        /// </summary>
        /// <remarks>
        /// The control will show the first frame of the animation, a custom static
        /// image or the window's background color as specified by the last
        /// <see cref="SetInactiveBitmap"/> call.
        /// </remarks>
        public virtual void Stop()
        {
            driver.Stop();
        }

        /// <summary>
        /// Gets whether animation is being played.
        /// </summary>
        /// <returns><c>true</c> if the animation is being played; <c>false</c> otherwise.</returns>
        public virtual bool IsPlaying()
        {
            return driver.IsPlaying();
        }

        /// <summary>
        /// Loads an animation from a file.
        /// </summary>
        /// <param name="filename">A filename.</param>
        /// <param name="type">One of the <see cref="AnimationType"/> values;
        /// <see cref="AnimationType.Any"/>
        /// means that the function should try to autodetect the animation type.
        /// </param>
        /// <returns></returns>
        public virtual bool LoadFile(string filename, AnimationType type = AnimationType.Any)
        {
            return driver.LoadFile(filename, type);
        }

        /// <summary>
        /// Loads the animation from the given stream.
        /// </summary>
        /// <param name="stream">The stream to use to load the animation.
        /// Under Linux may be any kind of stream; under other platforms this must be
        /// a seekable stream.
        /// </param>
        /// <param name="type">One of the <see cref="AnimationType"/> values;
        /// <see cref="AnimationType.Any"/>
        /// means that the function should try to autodetect the animation type.
        /// </param>
        /// <returns><c>true</c> if the operation succeeded, <c>false</c> otherwise.</returns>
        public virtual bool Load(Stream stream, AnimationType type = AnimationType.Any)
        {
            return driver.Load(stream, type);
        }

        /// <summary>
        /// Gets the delay (in milliseconds) for the specified frame.
        /// </summary>
        /// <param name="i">Frame index.</param>
        /// <returns></returns>
        public virtual int GetDelay(uint i)
        {
            return driver.GetDelay(i);
        }

        /// <summary>
        /// Returns the specified frame as a <see cref="GenericImage"/>.
        /// </summary>
        /// <param name="i">Frame index.</param>
        /// <returns></returns>
        public virtual GenericImage GetFrame(uint i)
        {
            return driver.GetFrame(i);
        }

        /// <summary>
        /// Loads the animation from the given file or resource url.
        /// </summary>
        /// <param name="url">Url with the animation.</param>
        /// <param name="type">One of the <see cref="AnimationType"/> values;
        /// <see cref="AnimationType.Any"/>
        /// means that the function should try to autodetect the animation type.
        /// </param>
        /// <returns><c>true</c> if the operation succeeded, <c>false</c> otherwise.</returns>
        /// <example>
        /// <code>
        /// var resPrefix = $"embres:ControlsSample.Resources.Animation.";
        /// var animationPlant = $"{resPrefix}Plant.gif";
        /// animation.LoadFromUrl(animationPlant);
        /// animation.Play();
        /// </code>
        /// </example>
        public virtual bool LoadFromUrl(string url, AnimationType type = AnimationType.Any)
        {
            using var stream = ResourceLoader.StreamFromUrl(url);
            return driver.Load(stream, type);
        }

        /// <summary>
        /// Sets the bitmap to show on the control when it's not playing an animation.
        /// </summary>
        /// <remarks>
        /// If you set as inactive bitmap <c>null</c> (which is the default), then the first
        /// frame of the animation is instead shown when the control is inactive;
        /// in this case, if there's no valid animation associated with the control, then
        /// the background color of the control is shown.
        /// </remarks>
        /// <remarks>
        /// If the control is not playing the animation, the given bitmap will be
        /// immediately shown, otherwise it will be shown as soon as <see cref="Stop"/> is called.
        /// </remarks>
        /// <remarks>
        /// Note that the inactive bitmap, if smaller than the control's size, will be
        /// centered in the control; if bigger, it will be stretched to fit it.
        /// </remarks>
        /// <param name="bitmap"></param>
        public virtual void SetInactiveBitmap(ImageSet? bitmap)
        {
            driver.SetInactiveBitmap(bitmap);
        }

        internal class NativeAnimationPlayer : Control, IAnimationPlayer
        {
            public NativeAnimationPlayer()
            {
            }

            Control IAnimationPlayer.Control { get => this; }

            public override ControlTypeId ControlKind => ControlTypeId.AnimationPlayer;

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

            [Browsable(false)]
            internal new NativeAnimationControlHandler Handler
            {
                get
                {
                    CheckDisposed();
                    return (NativeAnimationControlHandler)base.Handler;
                }
            }

            internal new Native.AnimationControl NativeControl => Handler.NativeControl;

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

            public virtual int GetDelay(uint i)
            {
                return NativeControl.GetDelay(i);
            }

            public virtual GenericImage GetFrame(uint i)
            {
                var ptr = NativeControl.GetFrame(i);
                var result = new GenericImage(ptr);
                return result;
            }

            public override SizeD GetPreferredSize(SizeD availableSize)
            {
                if (IsOk)
                {
                    var size = AnimationSize;
                    var sizeDips = PixelToDip(size);
                    return sizeDips;
                }

                return base.GetPreferredSize(availableSize);
            }

            public virtual void SetInactiveBitmap(ImageSet? imageSet)
            {
                NativeControl.SetInactiveBitmap((UI.Native.ImageSet?)imageSet?.NativeObject);
            }

            internal override ControlHandler CreateHandler()
            {
                return new NativeAnimationControlHandler();
            }
        }
    }
}