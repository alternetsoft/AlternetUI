using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties related to the initialization
    /// of the <see cref="TextBox"/> and other controls with defaults which are best
    /// for editing different value types.
    /// </summary>
    internal class ControlInitializers<TControl, TEventArgs> : BaseObject
        where TControl : AbstractControl
        where TEventArgs : EventArgs
    {
        private EnumArray<KnownInputType, Item?> register = new();

        static ControlInitializers()
        {
        }

        public ControlInitializers()
        {
        }

        /// <summary>
        /// Initializes control properties
        /// with defaults which are best for editing the specified
        /// value type.
        /// </summary>
        /// <param name="control">Control to initialize.</param>
        /// <param name="type">Type of the input value.</param>
        /// <param name="e">Initialization arguments.</param>
        public virtual void Initialize(
            TControl control,
            KnownInputType type,
            TEventArgs? e)
        {
            var initializer = GetInitializer(type);
            initializer(control, e);
        }

        /// <summary>
        /// Gets an action which can initialize control for editing the specified
        /// value type. Result is not null.
        /// </summary>
        /// <param name="type">Type of the input value.</param>
        /// <returns></returns>
        public virtual Action<TControl, TEventArgs?> GetInitializer(KnownInputType type)
        {
            var item = GetItem(type);
            return item.RaiseInitializer;
        }

        /// <summary>
        /// Removes initialization action for the specified value type.
        /// </summary>
        /// <param name="type">Type of the input value.</param>
        /// <param name="action">Action which is called when control is initialized
        /// for editing the specified value type.</param>
        /// <returns></returns>
        public virtual void RemoveInitializer(
            KnownInputType type,
            Action<TControl, TEventArgs?> action)
        {
            var item = GetItem(type);
            item.RemoveInitializer(action);
        }

        /// <summary>
        /// Adds action used when control is initialized
        /// with defaults which are best for editing the specified
        /// value type.
        /// </summary>
        /// <param name="type">Type of the input value.</param>
        /// <param name="action">Action to call when control is initialized for editing
        /// the specified value type.</param>
        public virtual void AddInitializer(
            KnownInputType type,
            Action<TControl, TEventArgs?> action)
        {
            var item = GetItem(type);
            item.AddInitializer(action);
        }

        public virtual Item GetItem(KnownInputType type)
        {
            var item = register[type];
            if (item is null)
            {
                item = new Item();
                register[type] = item;
            }

            return item;
        }

        public class Item
        {
            public event Action<TControl, TEventArgs?>? Initializer;

            public void RaiseInitializer(TControl control, TEventArgs? e)
            {
                Initializer?.Invoke(control, e);
            }

            public void RemoveInitializer(Action<TControl, TEventArgs?> action)
            {
                Initializer -= action;
            }

            public void AddInitializer(Action<TControl, TEventArgs?> action)
            {
                Initializer += action;
            }
        }
    }
}
