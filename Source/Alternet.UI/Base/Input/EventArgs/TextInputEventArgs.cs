#nullable disable
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


using System;
using System.Security; 

namespace Alternet.UI
{
    /// <summary>
    /// This class is used in the <see cref="UIElement.TextInput"/> event as EventArgs.
    /// </summary>
    public class TextInputEventArgs : KeyboardEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextInputEventArgs"/> class.
        /// </summary>
        public TextInputEventArgs(KeyboardDevice keyboard, /*PresentationSource inputSource,*/ long timestamp, char keyChar) : base(keyboard, timestamp)
        {
            //if (inputSource == null)
            //    throw new ArgumentNullException("inputSource");

            //_inputSource = inputSource;

            //_realKey = key;
            _keyChar = keyChar;

            // Start out assuming that this is just a normal key.
            //MarkNormal();
        }

        ///// <summary>
        /////     The input source that provided this input.
        ///// </summary>
        //public PresentationSource InputSource
        //{
        //    get 
        //    {
                
        //        return UnsafeInputSource;
        //    }
        //}

        /// <summary>
        ///     The Key referenced by the event, if the key is not being 
        ///     handled specially.
        /// </summary>
        public char KeyChar
        {
            get {return _keyChar;}
        }

        ///// <summary>
        /////     The original key, as opposed to <see cref="Key"/>, which might
        /////     have been changed (e.g. by MarkTextInput).
        ///// </summary>
        ///// <remarks>
        ///// Note:  This should remain internal.  When a processor obfuscates the key,
        ///// such as changing characters to Key.TextInput, we're deliberately trying to
        ///// hide it and make it hard to find.  But internally we'd like an easy way to find
        ///// it.  So we have this internal, but it must remain internal.
        ///// </remarks>
        //internal Key RealKey
        //{
        //    get { return _realKey; }
        //}

        ///// <summary>
        /////     The Key referenced by the event, if the key is going to be
        /////     processed by an IME.
        ///// </summary>
        //public Key ImeProcessedKey
        //{
        //    get { return (_key == Key.ImeProcessed) ? _realKey : Key.None;}
        //}

        ///// <summary>
        /////     The Key referenced by the event, if the key is going to be
        /////     processed by an system.
        ///// </summary>
        //public Key SystemKey
        //{
        //    get { return (_key == Key.System) ? _realKey : Key.None;}
        //}

        ///// <summary>
        /////     The Key referenced by the event, if the the key is going to 
        /////     be processed by Win32 Dead Char System.
        ///// </summary>
        //public Key DeadCharProcessedKey
        //{
        //    get { return (_key == Key.DeadCharProcessed) ? _realKey : Key.None; }
        //}

        /// <summary>
        ///     The mechanism used to call the type-specific handler on the
        ///     target.
        /// </summary>
        /// <param name="genericHandler">
        ///     The generic handler to call in a type-specific way.
        /// </param>
        /// <param name="genericTarget">
        ///     The target to call the handler on.
        /// </param>
        /// <ExternalAPI/> 
        protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
        {
            TextInputEventHandler handler = (TextInputEventHandler) genericHandler;
            
            handler(genericTarget, this);
        }

        //internal void SetRepeat( bool newRepeatState )
        //{
        //    _isRepeat = newRepeatState;
        //}

        //internal void MarkNormal()
        //{
        //    _key = _realKey;
        //}

        //internal void MarkSystem()
        //{
        //    _key = Key.System;
        //}
        
        //internal void MarkImeProcessed()
        //{
        //    _key = Key.ImeProcessed;
        //}

        //internal void MarkDeadCharProcessed()
        //{
        //    _key = Key.DeadCharProcessed;
        //}
        //internal PresentationSource UnsafeInputSource
        //{
        //    get 
        //    {
        //        return _inputSource;
        //    }
        //}

        //internal int ScanCode
        //{
        //    get {return _scanCode;}
        //    set {_scanCode = value;}
        //}

        //internal bool IsExtendedKey
        //{
        //    get {return _isExtendedKey;}
        //    set {_isExtendedKey = value;}
        //}


        //private Key _realKey;
        private char _keyChar;

        //private PresentationSource _inputSource;
        //private int _scanCode;
        //private bool _isExtendedKey;
}
}

