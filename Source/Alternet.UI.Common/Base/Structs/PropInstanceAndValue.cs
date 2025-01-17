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
        public void PushProperties(
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
        public void PopProperties(ConcurrentStack<PropInstanceAndValue>? savedProperties)
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
        public void PushChildrenProperties(
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
        public void PushChildrenEnabled(
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
        public void PushChildrenVisible(
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
    }
}
