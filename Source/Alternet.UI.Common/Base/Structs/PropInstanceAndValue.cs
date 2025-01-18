using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using Alternet.UI.Extensions;

namespace Alternet.UI
{
    /// <summary>
    /// Contains object instance, property information and property value.
    /// </summary>
    public readonly struct PropInstanceAndValue
    {
        /// <summary>
        /// The object instance which contains the property.
        /// </summary>
        public readonly object? Instance;

        /// <summary>
        /// The property value.
        /// </summary>
        public readonly object? Value;

        /// <summary>
        /// The property information.
        /// </summary>
        public readonly PropertyInfo PropInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropInstanceAndValue"/> struct
        /// with the specified information.
        /// </summary>
        /// <param name="instance">The object instance which contains the property.</param>
        /// <param name="value">The property value.</param>
        /// <param name="propInfo">The property information.</param>
        public PropInstanceAndValue(object? instance, PropertyInfo propInfo, object? value)
        {
            Instance = instance;
            PropInfo = propInfo;
            Value = value;
        }

        /// <summary>
        /// Iterates collection of the objects and changes the specified property value
        /// for the each item. Old property values are pushed to the
        /// <paramref name="oldValues"/> stack.
        /// </summary>
        /// <param name="oldValues">The stack to save old property values.</param>
        /// <param name="objects">The collection of objects.</param>
        /// <param name="propInfo">Property information.</param>
        /// <param name="newValue">New value for the property.</param>
        /// <returns></returns>
        /// <remarks>
        /// You can use <see cref="PopProperties"/> for restoring property values
        /// after saved them using <see cref="PushProperties"/>.
        /// </remarks>
        public static void PushProperties(
            IEnumerable objects,
            PropertyInfo? propInfo,
            object? newValue,
            ref ConcurrentStack<PropInstanceAndValue>? oldValues)
        {
            if (propInfo is null)
                return;

            foreach (var obj in objects)
            {
                var propValue = propInfo.GetValue(obj);
                if (propValue == newValue)
                    continue;
                oldValues ??= new();
                oldValues.Push(new(obj, propInfo, propValue));
                propInfo.SetValue(obj, newValue);
            }
        }

        /// <summary>
        /// Restores property values which were previously saved using
        /// <see cref="PushProperties"/>.
        /// </summary>
        /// <param name="savedProperties">Collection of the saved properties.</param>
        public static void PopProperties(ConcurrentStack<PropInstanceAndValue>? savedProperties)
        {
            if (savedProperties is null)
                return;
            while (savedProperties.TryPop(out var item))
            {
                item.PropInfo.SetValue(item.Instance, item.Value);
            }
        }

        /// <summary>
        /// Iterates children controls and changes the specified property value
        /// for the each child. Old property values are pushed to the
        /// <paramref name="oldValues"/> stack.
        /// </summary>
        /// <param name="control">The container control which childs are processed.</param>
        /// <param name="propInfo">Property information.</param>
        /// <param name="oldValues">The stack to save old property values.</param>
        /// <param name="newValue">New value for the property.</param>
        /// <returns></returns>
        /// <remarks>
        /// You can use <see cref="PopProperties"/> for restoring property values
        /// after saved them using this method.
        /// </remarks>
        public static void PushChildrenProperties(
            AbstractControl control,
            PropertyInfo? propInfo,
            object? newValue,
            ref ConcurrentStack<PropInstanceAndValue>? oldValues)
        {
            if (!control.HasChildren)
                return;
            PushProperties(control.Children, propInfo, newValue, ref oldValues);
        }

        /// <summary>
        /// Iterates children controls and changes 'Enabled' property value
        /// for the each child. Old property values are pushed to the
        /// <paramref name="oldValues"/> stack.
        /// </summary>
        /// <param name="oldValues">The stack to save old property values.</param>
        /// <param name="control">The container control which childs are processed.</param>
        /// <param name="newValue">New value for the property.</param>
        /// <returns></returns>
        /// <remarks>
        /// You can use <see cref="PopProperties"/> for restoring property values
        /// after saved them using this method.
        /// </remarks>
        public static void PushChildrenEnabled(
            AbstractControl control,
            bool newValue,
            ref ConcurrentStack<PropInstanceAndValue>? oldValues)
        {
            PushChildrenProperties(
                        control,
                        KnownProperties.AbstractControl.Enabled,
                        BoolBoxes.Box(newValue),
                        ref oldValues);
        }

        /// <summary>
        /// Iterates children controls and changes 'Visible' property value
        /// for the each child. Old property values are pushed to the
        /// <paramref name="oldValues"/> stack.
        /// </summary>
        /// <param name="oldValues">The stack to save old property values.</param>
        /// <param name="control">The container control which childs are processed.</param>
        /// <param name="newValue">New value for the property.</param>
        /// <returns></returns>
        /// <remarks>
        /// You can use <see cref="PopProperties"/> for restoring property values
        /// after saved them using this method.
        /// </remarks>
        public static void PushChildrenVisible(
            AbstractControl control,
            bool newValue,
            ref ConcurrentStack<PropInstanceAndValue>? oldValues)
        {
            PushChildrenProperties(
                        control,
                        KnownProperties.AbstractControl.Visible,
                        BoolBoxes.Box(newValue),
                        ref oldValues);
        }

        /// <summary>
        /// Iterates collection of the container controls and changes 'Enabled' property value
        /// of their childs. Old property values are returned.
        /// </summary>
        /// <param name="containers">The collection of the container controls
        /// which childs are processed.</param>
        /// <param name="newValue">New value for the property.</param>
        /// <returns>Old values of the properties, it allows to restore them later.</returns>
        public static ConcurrentStack<SavedPropertiesItem>? PushChildrenEnabledMultiple(
            IEnumerable<AbstractControl> containers,
            bool newValue)
        {
            ConcurrentStack<SavedPropertiesItem>? result = null;

            foreach (var container in containers)
            {
                ConcurrentStack<PropInstanceAndValue>? oldValues = null;

                PushChildrenEnabled(
                            container,
                            newValue,
                            ref oldValues);

                if(oldValues is not null)
                {
                    SavedPropertiesItem item = new(container, oldValues);
                    result ??= new();
                    result.Push(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Restores property values which were previously saved using
        /// <see cref="PushChildrenEnabledMultiple"/> or similar methods.
        /// </summary>
        /// <param name="saved">The collection of the saved properties.</param>
        /// <param name="doInsideUpdate">Whether to call
        /// <see cref="AbstractControl.DoInsideUpdate"/> when properties are restored.</param>
        public static void PopPropertiesMultiple(
            ConcurrentStack<SavedPropertiesItem>? saved,
            bool doInsideUpdate = true)
        {
            if (saved is null)
                return;
            foreach(var item in saved)
            {
                if (doInsideUpdate)
                    item.Container.DoInsideUpdate(Pop);
                else
                    Pop();

                void Pop()
                {
                    PopProperties(item.SavedProperties);
                }
            }
        }

        /// <summary>
        /// Item which is used in <see cref="PushChildrenEnabledMultiple"/>
        /// and <see cref="PopPropertiesMultiple"/>.
        /// </summary>
        public readonly struct SavedPropertiesItem
        {
            /// <summary>
            /// Container control which properties are saved.
            /// </summary>
            public readonly AbstractControl Container;

            /// <summary>
            /// Information about the saved properties.
            /// </summary>
            public readonly ConcurrentStack<PropInstanceAndValue> SavedProperties;

            /// <summary>
            /// Initializes a new instance of the <see cref="SavedPropertiesItem"/> struct
            /// with the specified parameters.
            /// </summary>
            /// <param name="container">Container control which properties are saved.</param>
            /// <param name="savedProps">Information about the saved properties.</param>
            public SavedPropertiesItem(
                AbstractControl container,
                ConcurrentStack<PropInstanceAndValue> savedProps)
            {
                Container = container;
                SavedProperties = savedProps;
            }
        }
    }
}
