using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents information about a calendar alarm, including its advance time,
    /// enabled state, sound settings, and snooze time.
    /// </summary>
    public partial class CalendarAlarmInfo : ImmutableObject, IEquatable<CalendarAlarmInfo>
    {
        /// <summary>
        /// Gets or sets the default advance time for alarms.
        /// </summary>
        public static TimeSpan DefaultAlarmAdvance = TimeSpan.FromMinutes(5);

        /// <summary>
        /// Gets or sets the default value indicating whether the alarm should play a sound.
        /// </summary>
        public static bool DefaultPlaySound = false;

        /// <summary>
        /// Gets or sets the default sound path for alarms.
        /// </summary>
        public static string DefaultSoundPath = string.Empty;

        private TimeSpan advance = DefaultAlarmAdvance;
        private bool enabled = true;
        private bool playSound = DefaultPlaySound;
        private DateTime snoozeTime = DateTime.MinValue;
        private string? soundPath = DefaultSoundPath;

        /// <summary>
        /// Gets or sets an advance time.
        /// </summary>
        public virtual TimeSpan Advance
        {
            get
            {
                return advance;
            }

            set
            {
                SetProperty(ref advance, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the alarm is active.
        /// </summary>
        public virtual bool Enabled
        {
            get
            {
                return enabled;
            }

            set
            {
                SetProperty(ref enabled, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the alarm have to play some sound.
        /// </summary>
        public virtual bool PlaySound
        {
            get
            {
                return playSound;
            }

            set
            {
                SetProperty(ref playSound, value);
            }
        }

        /// <summary>
        /// Gets or sets an <c>CalendarAlarmInfo</c> snooze interval.
        /// </summary>
        public virtual DateTime SnoozeTime
        {
            get
            {
                return snoozeTime;
            }

            set
            {
                SetProperty(ref snoozeTime, value);
            }
        }

        /// <summary>
        /// Gets or sets a path and file name of sound file to play.
        /// </summary>
        public virtual string? SoundPath
        {
            get
            {
                return soundPath;
            }

            set
            {
                SetProperty(ref soundPath, value);
            }
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current instance of <see cref="CalendarAlarmInfo"/>.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><c>true</c> if the specified object is equal to the current instance;
        /// otherwise, <c>false</c>.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is not CalendarAlarmInfo other)
            {
                return false;
            }

            return Equals(other);
        }

        /// <inheritdoc/>
        public bool Equals(CalendarAlarmInfo? other)
        {
            if (other == null)
            {
                return false;
            }

            return Advance == other.Advance &&
                   SnoozeTime == other.SnoozeTime &&
                   Enabled == other.Enabled &&
                   PlaySound == other.PlaySound &&
                   SoundPath == other.SoundPath;
        }

        /// <summary>
        /// Saves the current state of this instance
        /// to the specified <see cref="IPropertyBag"/>.
        /// </summary>
        /// <param name="propertyBag">The property bag to which the state will be saved.</param>
        /// <param name="prefix">An optional prefix to prepend to property names.</param>
        public virtual void SaveTo(IPropertyBag propertyBag, string? prefix = null)
        {
            if (prefix is not null)
                prefix += ".";
            else
                prefix = "";

            propertyBag.SetBoolProperty(prefix + nameof(Enabled), Enabled);
            propertyBag.SetBoolProperty(prefix + nameof(PlaySound), PlaySound);
            propertyBag.SetStringProperty(prefix + nameof(SoundPath), SoundPath ?? string.Empty);
            propertyBag.SetDateTimeProperty(prefix + nameof(SnoozeTime), SnoozeTime);
            propertyBag.SetTimeSpanProperty(prefix + nameof(Advance), Advance);
        }

        /// <summary>
        /// Copies the contents from another <c>CalendarAlarmInfo</c> object.
        /// </summary>
        /// <param name="source">Specifies <c>CalendarAlarmInfo</c> to assign.</param>
        public virtual void Assign(CalendarAlarmInfo? source)
        {
            if (Immutable)
                return;

            if (source == null)
            {
                Reset();
                return;
            }

            DoInsideSuspendedPropertyChanged(() =>
            {
                Advance = source.Advance;
                SnoozeTime = source.SnoozeTime;
                Enabled = source.Enabled;
                PlaySound = source.PlaySound;
                SoundPath = source.SoundPath;
            });
        }

        /// <summary>
        /// Resets the properties of the <c>CalendarAlarmInfo</c> object to their default values.
        /// </summary>
        public virtual void Reset()
        {
            DoInsideSuspendedPropertyChanged(() =>
            {
                ResetAdvance();
                ResetEnabled();
                ResetPlaySound();
                ResetSnoozeTime();
                ResetSoundPath();
            });
        }

        /// <summary>
        /// Creates a new object that is a copy of the current object.
        /// </summary>
        /// <returns>A new instance of <c>CalendarAlarmInfo</c> that is the memberwise
        /// clone of the current <c>CalendarAlarmInfo</c> object.</returns>
        public virtual CalendarAlarmInfo Clone()
        {
            var result = Activator.CreateInstance(GetType()) as CalendarAlarmInfo;
            result ??= new CalendarAlarmInfo();
            result.Assign(this);
            return result;
        }

        /// <summary>
        /// Resets <c>Advance</c> property to the default value.
        /// </summary>
        public virtual void ResetAdvance()
        {
            Advance = DefaultAlarmAdvance;
        }

        /// <summary>
        /// Resets <c>Enabled</c> property to the default value.
        /// </summary>
        public virtual void ResetEnabled()
        {
            Enabled = true;
        }

        /// <summary>
        /// Resets <c>PlaySound</c> property to the default value.
        /// </summary>
        public virtual void ResetPlaySound()
        {
            PlaySound = DefaultPlaySound;
        }

        /// <summary>
        /// Resets <c>SnoozeTime</c> property to the default value.
        /// </summary>
        public virtual void ResetSnoozeTime()
        {
            SnoozeTime = DateTime.MinValue;
        }

        /// <summary>
        /// Resets <c>SoundPath</c> property to the default value.
        /// </summary>
        public virtual void ResetSoundPath()
        {
            SoundPath = string.Empty;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return (Advance, Enabled, PlaySound, SnoozeTime, SoundPath).GetHashCode();
        }
    }
}
