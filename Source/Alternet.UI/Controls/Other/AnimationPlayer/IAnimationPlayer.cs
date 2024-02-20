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
    /// Provides methods and properties to control animation player.
    /// </summary>
    public interface IAnimationPlayer
    {
        /// <summary>
        /// Gets associated control.
        /// </summary>
        Control Control { get; }

        /// <inheritdoc cref="AnimationPlayer.FrameCount"/>
        uint FrameCount { get; }

        /// <inheritdoc cref="AnimationPlayer.AnimationSize"/>
        SizeI AnimationSize { get; }

        /// <inheritdoc cref="AnimationPlayer.IsOk"/>
        bool IsOk { get; }

        /// <inheritdoc cref="AnimationPlayer.Play"/>
        bool Play();

        /// <inheritdoc cref="AnimationPlayer.Stop"/>
        void Stop();

        /// <inheritdoc cref="AnimationPlayer.IsPlaying"/>
        bool IsPlaying();

        /// <inheritdoc cref="AnimationPlayer.LoadFile"/>
        bool LoadFile(string filename, AnimationType type = AnimationType.Any);

        /// <inheritdoc cref="AnimationPlayer.Load"/>
        bool Load(Stream stream, AnimationType type = AnimationType.Any);

        /// <inheritdoc cref="AnimationPlayer.GetDelay"/>
        int GetDelay(uint i);

        /// <inheritdoc cref="AnimationPlayer.GetFrame"/>
        GenericImage GetFrame(uint i);

        /// <inheritdoc cref="AnimationPlayer.SetInactiveBitmap"/>
        void SetInactiveBitmap(ImageSet? bitmap);
    }
}