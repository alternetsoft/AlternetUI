// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;

#pragma warning disable 1634, 1691  // suppressing PreSharp warnings

namespace Alternet.UI
{
    /// <summary>
    /// This item supports the framework infrastructure and is not intended to be used directly from your code.
    /// </summary>
    public class ClrPropertyInfo : IPropertyInfo
    {
        private readonly Func<object, object?>? _getter;
        private readonly Action<object, object?>? _setter;

        /// <summary>
        /// This item supports the framework infrastructure and is not intended to be used directly from your code.
        /// </summary>
        public ClrPropertyInfo(string name, Func<object, object?>? getter, Action<object, object?>? setter, Type propertyType)
        {
            _getter = getter;
            _setter = setter;
            PropertyType = propertyType;
            Name = name;
        }

        /// <summary>
        /// This item supports the framework infrastructure and is not intended to be used directly from your code.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// This item supports the framework infrastructure and is not intended to be used directly from your code.
        /// </summary>
        public Type PropertyType { get; }

        /// <summary>
        /// This item supports the framework infrastructure and is not intended to be used directly from your code.
        /// </summary>
        public object? Get(object target)
        {
            if (_getter == null)
                throw new NotSupportedException("Property " + Name + " doesn't have a getter");
            return _getter(target);
        }

        /// <summary>
        /// This item supports the framework infrastructure and is not intended to be used directly from your code.
        /// </summary>
        public void Set(object target, object? value)
        {
            if (_setter == null)
                throw new NotSupportedException("Property " + Name + " doesn't have a setter");
            _setter(target, value);
        }

        /// <summary>
        /// This item supports the framework infrastructure and is not intended to be used directly from your code.
        /// </summary>
        public bool CanSet => _setter != null;

        /// <summary>
        /// This item supports the framework infrastructure and is not intended to be used directly from your code.
        /// </summary>
        public bool CanGet => _getter != null;
    }

    //public class ReflectionClrPropertyInfo : ClrPropertyInfo
    //{
    //    static Action<object, object?>? CreateSetter(PropertyInfo info)
    //    {
    //        if (info.SetMethod == null)
    //            return null;
    //        var target = Expression.Parameter(typeof(object), "target");
    //        var value = Expression.Parameter(typeof(object), "value");
    //        return Expression.Lambda<Action<object, object?>>(
    //                Expression.Call(Expression.Convert(target, info.DeclaringType!), info.SetMethod,
    //                    Expression.Convert(value, info.SetMethod.GetParameters()[0].ParameterType)),
    //                target, value)
    //            .Compile();
    //    }

    //    static Func<object, object>? CreateGetter(PropertyInfo info)
    //    {
    //        if (info.GetMethod == null)
    //            return null;
    //        var target = Expression.Parameter(typeof(object), "target");
    //        return Expression.Lambda<Func<object, object>>(
    //                Expression.Convert(Expression.Call(Expression.Convert(target, info.DeclaringType!), info.GetMethod),
    //                    typeof(object)))
    //            .Compile();
    //    }

    //    public ReflectionClrPropertyInfo(PropertyInfo info) : base(info.Name,
    //        CreateGetter(info), CreateSetter(info), info.PropertyType)
    //    {

    //    }
    //}
}

