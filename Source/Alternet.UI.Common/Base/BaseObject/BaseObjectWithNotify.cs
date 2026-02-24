using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a base object with notification capabilities.
    /// </summary>
    public partial class BaseObjectWithNotify : BaseObjectWithAttr, INotifyPropertyChanged
    {
        private int suspendCounter;

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets or sets an action which is called when a property value changes.
        /// </summary>
        [Browsable(false)]
        public Action<PropertyChangedEventArgs>? PropertyChangedAction { get; set; }

        /// <summary>
        /// Gets whether <see cref="PropertyChanged"/> event is suspended.
        /// </summary>
        [Browsable(false)]
        public bool IsPropertyChangedSuspended
        {
            get
            {
                return suspendCounter > 0;
            }
        }

        /// <summary>
        /// Suspends <see cref="PropertyChanged"/> event and <see cref="OnPropertyChanged"/>
        /// method so they will not be called when properties are changed.
        /// </summary>
        public virtual void SuspendPropertyChanged()
        {
            suspendCounter++;
        }

        /// <summary>
        /// Resumes <see cref="PropertyChanged"/> event and <see cref="OnPropertyChanged"/>
        /// method so they will be called when properties are changed.
        /// </summary>
        /// <param name="callChangedOnResume">Whether to call <see cref="PropertyChanged"/> event
        /// after action is executed and events are resumed.
        /// Optional. Default is True.</param>
        public virtual void ResumePropertyChanged(bool callChangedOnResume = true)
        {
            if (suspendCounter <= 0)
            {
                throw new InvalidOperationException(
                    "ResumePropertyChanged is called without previous call to SuspendPropertyChanged");
            }

            suspendCounter--;

            if (suspendCounter == 0)
            {
                if (callChangedOnResume)
                    RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Calls the specified action inside block with suspended
        /// <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="action">Action to call.</param>
        /// <param name="callChangedOnResume">Whether to call <see cref="PropertyChanged"/> event
        /// after action is executed and events are resumed.
        /// Optional. Default is True.</param>
        public void DoInsideSuspendedPropertyChanged(Action action, bool callChangedOnResume = true)
        {
            SuspendPropertyChanged();
            try
            {
                action();
            }
            finally
            {
                ResumePropertyChanged(callChangedOnResume);
            }
        }

        /// <summary>
        /// Calls <see cref="PropertyChanged"/> event. If events are suspended
        /// with previous call to <see cref="SuspendPropertyChanged"/>, does nothing.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public virtual void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            if (DisposingOrDisposed || suspendCounter > 0)
                return;
            OnPropertyChanged(propertyName);
            var e = EventArgsUtils.GetPropertyChangedEventArgs(propertyName);
            PropertyChanged?.Invoke(this, e);
            PropertyChangedAction?.Invoke(e);
        }

        /// <summary>
        /// Called when object property is changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
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
        protected virtual bool SetProperty<T>(
            ref T? storage,
            T? value,
            [CallerMemberName] string? propertyName = null,
            Action? changedAction = null)
        {
            if (Immutable || Equals(storage, value))
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
        protected virtual bool SetProperty<T>(ref T? storage, T? value)
        {
            if (Immutable || Equals(storage, value))
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
        protected virtual T GetNewFieldValue<T>(T storage, T value)
        {
            if (Immutable || Equals(storage, value))
                return storage;
            RaisePropertyChanged();
            return value;
        }
    }
}
