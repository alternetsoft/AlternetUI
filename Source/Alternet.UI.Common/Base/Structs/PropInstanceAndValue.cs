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
        /// for the each item. Old property values are returned.
        /// </summary>
        /// <param name="objects">The collection of objects.</param>
        /// <param name="propInfo">Property information.</param>
        /// <param name="newValue">New value for the property.</param>
        /// <returns></returns>
        /// <remarks>
        /// You can use <see cref="PopProperties"/> for restoring property values
        /// after saved them using <see cref="PushProperties"/>.
        /// </remarks>
        public ConcurrentStack<PropInstanceAndValue>? PushProperties(
            IEnumerable objects,
            PropertyInfo? propInfo,
            object? newValue)
        {
            if (propInfo is null)
                return null;

            ConcurrentStack<PropInstanceAndValue>? result = null;

            foreach (var obj in objects)
            {
                var propValue = propInfo.GetValue(obj);
                if (propValue == newValue)
                    continue;
                result ??= new();
                result.Push(new(obj, propInfo, propValue));
                propInfo.SetValue(obj, newValue);
            }

            return result;
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
        /// for the each child. Old property values are returned.
        /// </summary>
        /// <param name="control">The container control which childs are processed.</param>
        /// <param name="propInfo">Property information.</param>
        /// <param name="newValue">New value for the property.</param>
        /// <returns></returns>
        /// <remarks>
        /// You can use <see cref="PopProperties"/> for restoring property values
        /// after saved them using this method.
        /// </remarks>
        public ConcurrentStack<PropInstanceAndValue>? PushChildrenProperties(
            AbstractControl control,
            PropertyInfo? propInfo,
            object? newValue)
        {
            if (!control.HasChildren)
                return null;
            var result = PushProperties(control.Children, propInfo, newValue);
            return result;
        }

        /// <summary>
        /// Iterates children controls and changes 'Enabled' property value
        /// for the each child. Old property values are returned.
        /// </summary>
        /// <param name="control">The container control which childs are processed.</param>
        /// <param name="newValue">New value for the property.</param>
        /// <returns></returns>
        /// <remarks>
        /// You can use <see cref="PopProperties"/> for restoring property values
        /// after saved them using this method.
        /// </remarks>
        public ConcurrentStack<PropInstanceAndValue>? PushChildrenEnabled(
            AbstractControl control,
            bool newValue)
        {
            return PushChildrenProperties(
                        control,
                        KnownProperties.AbstractControl.Enabled,
                        BoolBoxes.Box(newValue));
        }

        /// <summary>
        /// Iterates children controls and changes 'Visible' property value
        /// for the each child. Old property values are returned.
        /// </summary>
        /// <param name="control">The container control which childs are processed.</param>
        /// <param name="newValue">New value for the property.</param>
        /// <returns></returns>
        /// <remarks>
        /// You can use <see cref="PopProperties"/> for restoring property values
        /// after saved them using this method.
        /// </remarks>
        public ConcurrentStack<PropInstanceAndValue>? PushChildrenVisible(
            AbstractControl control,
            bool newValue)
        {
            return PushChildrenProperties(
                        control,
                        KnownProperties.AbstractControl.Visible,
                        BoolBoxes.Box(newValue));
        }
    }
}
