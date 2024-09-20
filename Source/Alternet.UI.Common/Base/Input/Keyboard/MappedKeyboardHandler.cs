using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Abstract implementation of the <see cref="IKeyboardHandler"/> interface with
    /// mappings registration between platfrom keys and <see cref="Key"/>.
    /// </summary>
    /// <typeparam name="T">Type of the platform key.</typeparam>
    public abstract class MappedKeyboardHandler<T> : DisposableObject, IKeyboardHandler
        where T : struct, Enum
    {
        private readonly T maxPlatformKey;
        private readonly Key maxKey;

        private AbstractTwoWayEnumMapping<T, Key>? mapping;

        /// <summary>
        /// Initializes a new instance of the <see cref="MappedKeyboardHandler{T}"/> class.
        /// </summary>
        public MappedKeyboardHandler(T maxPlatformKey, Key maxKey)
        {
            this.maxPlatformKey = maxPlatformKey;
            this.maxKey = maxKey;
        }

        /// <summary>
        /// Gets key mapping manager.
        /// </summary>
        public AbstractTwoWayEnumMapping<T, Key> Mapping
        {
            get
            {
                if (mapping is null)
                {
                    mapping = new TwoWayEnumMapping<T, Key>(maxPlatformKey, maxKey);
                    RegisterKeyMappings();
                }

                return mapping;
            }

            set
            {
                mapping = value;
            }
        }

        /// <summary>
        /// Converts platform key to <see cref="Alternet.UI.Key"/>.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <returns></returns>
        public virtual Key Convert(T key)
        {
            return Mapping.SourceToDest.Convert(key);
        }

        /// <summary>
        /// Converts <see cref="Alternet.UI.Key"/> to platform key.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <returns></returns>
        public virtual T Convert(Alternet.UI.Key key)
        {
            return Mapping.DestToSource.Convert(key);
        }

        /// <summary>
        /// Registers two way key mapping between platform key and <see cref="Key"/>.
        /// </summary>
        public virtual void AddMapping(T source, Key? dest)
        {
            if (dest is null)
                return;
            Mapping.Add(source, dest.Value);
        }

        /// <summary>
        /// Registers one way key mapping from platform key to <see cref="Key"/>.
        /// </summary>
        public virtual void AddOneWayMapping(T from, Key to)
        {
            Mapping.SourceToDest.Add(from, to);
        }

        /// <summary>
        /// Registers one way key mapping from <see cref="Key"/> to platform key.
        /// </summary>
        public virtual void AddOneWayMapping(Key from, T to)
        {
            Mapping.DestToSource.Add(from, to);
        }

        /// <summary>
        /// Registers default key mappings between platform keys and <see cref="Key"/>.
        /// </summary>
        public virtual void RegisterKeyMappings()
        {
        }

        /// <inheritdoc/>
        public abstract KeyStates GetKeyStatesFromSystem(Key key);

        /// <inheritdoc/>
        public virtual bool HideKeyboard(Control? control)
        {
            return false;
        }

        /// <inheritdoc/>
        public virtual bool IsSoftKeyboardShowing(Control? control)
        {
            return false;
        }

        /// <inheritdoc/>
        public virtual bool ShowKeyboard(Control? control)
        {
            return false;
        }

        /// <inheritdoc/>
        public virtual bool IsValidKey(Key key)
        {
            return (int)key >= (int)Key.None && (int)key <= (int)maxKey;
        }
    }
}
