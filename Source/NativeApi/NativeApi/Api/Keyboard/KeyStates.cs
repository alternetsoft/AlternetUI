﻿
using ApiCommon;
using System;

namespace NativeApi.Api
{
    [Flags]
    public enum KeyStates : byte
    {
        /// <summary>
        ///     No state (same as up).
        /// </summary>
        None = 0,

        /// <summary>
        ///    The key is down.
        /// </summary>
        Down = 1,

        /// <summary>
        ///    The key is toggled on.
        /// </summary>
        Toggled = 2
    }
}