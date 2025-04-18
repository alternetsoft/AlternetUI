﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Extends <see cref="DisposableObject"/> with <see cref="Immutable"/> property
    /// and other features.
    /// Allows to implement immutable objects with properties that can not be changed.
    /// </summary>
    public class ImmutableObject : DisposableObject, IImmutableObject
    {
        private bool immutable;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImmutableObject"/> class.
        /// </summary>
        public ImmutableObject()
        {
        }

        /// <summary>
        /// Gets or sets whether object is immutable (properties can not be changed).
        /// </summary>
        [Browsable(false)]
        public virtual bool Immutable
        {
            get
            {
                return immutable;
            }

            protected set
            {
                immutable = value;
            }
        }

        /// <summary>
        /// Marks the object as immutable.
        /// </summary>
        /// <remarks>
        /// Marks this object as immutable, meaning that the contents of its properties
        /// will not change
        /// for the lifetime of the object. This state can be set, but it cannot be cleared once
        /// it is set.
        /// </remarks>
        [Browsable(false)]
        public virtual void SetImmutable()
        {
            immutable = true;
        }

        /// <summary>
        /// Sets field value and calls <see cref="BaseObjectWithNotify.RaisePropertyChanged"/> method.
        /// </summary>
        /// <param name="storage">Field where property is stored.</param>
        /// <param name="value">New property value.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="changedAction">This action is called when property changes</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected bool SetProperty<T>(
            ref T? storage,
            T? value,
            [CallerMemberName] string? propertyName = null,
            Action? changedAction = null)
        {
            if (immutable || Equals(storage, value))
                return false;
            storage = value;
            RaisePropertyChanged(propertyName);
            changedAction?.Invoke();
            return true;
        }

        /// <summary>
        /// Sets field value and calls <see cref="BaseObjectWithNotify.RaisePropertyChanged"/> method.
        /// </summary>
        /// <param name="storage">Field where property is stored.</param>
        /// <param name="value">New property value.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected bool SetProperty<T>(ref T? storage, T? value)
        {
            if (immutable || Equals(storage, value))
                return false;
            storage = value;
            RaisePropertyChanged();
            return true;
        }

        /// <summary>
        /// Gets new field value and calls <see cref="BaseObjectWithNotify.RaisePropertyChanged"/> method.
        /// </summary>
        /// <param name="storage">Field value.</param>
        /// <param name="value">New property value.</param>
        /// <returns></returns>
        protected T GetNewFieldValue<T>(T storage, T value)
        {
            if (immutable || Equals(storage, value))
                return storage;
            RaisePropertyChanged();
            return value;
        }
    }
}
