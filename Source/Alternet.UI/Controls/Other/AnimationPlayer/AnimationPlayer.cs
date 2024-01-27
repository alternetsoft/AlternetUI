﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public partial class AnimationPlayer : Control
    {
        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.AnimationPlayer;

        /// <summary>
        /// Gets or sets a value indicating whether to use generic calendar or native calendar.
        /// </summary>
        /// <remarks>
        /// Default value under Linux is <c>true</c> (native version is not working),
        /// on other platfroms - <c>false</c>.
        /// </remarks>
        public bool UseGeneric
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
        internal new NativeAnimationControlHandler Handler
        {
            get
            {
                CheckDisposed();
                return (NativeAnimationControlHandler)base.Handler;
            }
        }

        internal new Native.AnimationControl NativeControl => Handler.NativeControl;

        /// <summary>
        /// Starts playing the animation.
        /// </summary>
        /// <returns><c>true</c> if the animation started to play; <c>false</c> otherwise.</returns>
        /// <remarks>
        /// The animation is always played in loop mode (unless the last frame of the animation
        /// has an infinite delay time) and always start from the first frame even if
        /// you stopped it while some other frame was displayed.
        /// </remarks>
        public virtual bool PlayFn()
        {
            return NativeControl.Play();
        }

        /// <summary>
        /// Starts playing the animation.
        /// </summary>
        /// <remarks>
        /// The animation is always played in loop mode (unless the last frame of the animation
        /// has an infinite delay time) and always start from the first frame even if
        /// you stopped it while some other frame was displayed.
        /// </remarks>
        public virtual void Play()
        {
            NativeControl.Play();
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
            NativeControl.Stop();
        }

        /// <summary>
        /// Gets whether animation is being played.
        /// </summary>
        /// <returns><c>true</c> if the animation is being played; <c>false</c> otherwise.</returns>
        public virtual bool IsPlaying()
        {
            return NativeControl.IsPlaying();
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
            return NativeControl.LoadFile(filename, (int)type);
        }

        /// <summary>
        /// Loads the animation from the given stream.
        /// </summary>
        /// <param name="stream">The stream to use to load the animation.
        /// Under Linux may be any kind of stream; under other platforms this must be a seekable stream.
        /// </param>
        /// <param name="type">One of the <see cref="AnimationType"/> values;
        /// <see cref="AnimationType.Any"/>
        /// means that the function should try to autodetect the animation type.
        /// </param>
        /// <returns><c>true</c> if the operation succeeded, <c>false</c> otherwise.</returns>
        public virtual bool Load(Stream stream, AnimationType type = AnimationType.Any)
        {
            using var inputStream = new UI.Native.InputStream(stream);
            return NativeControl.Load(inputStream, (int)type);
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
            return Load(stream, type);
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
            NativeControl.SetInactiveBitmap(bitmap?.NativeImageSet);
        }

        /// <inheritdoc/>
        internal override ControlHandler CreateHandler()
        {
            return new NativeAnimationControlHandler();
        }
    }
}