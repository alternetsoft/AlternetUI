using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Alternet.Common
{
    public class DynamicObjectExtenderWithEvents : IDynamicObjectExtender
    {
        /// <summary>
        /// Occurs before invoking a method on the dynamic object.
        /// </summary>
        public event IDynamicObjectExtender.BeforeTryInvokeDelegate? BeforeTryInvoke;

        /// <summary>
        /// Occurs before retrieving the dynamic member names.
        /// </summary>
        public event IDynamicObjectExtender.BeforeGetDynamicMemberNamesDelegate? BeforeGetDynamicMemberNames;

        /// <summary>
        /// Occurs before performing a binary operation on the dynamic object.
        /// </summary>
        public event IDynamicObjectExtender.BeforeTryBinaryOperationDelegate? BeforeTryBinaryOperation;

        /// <summary>
        /// Occurs before converting the dynamic object to another type.
        /// </summary>
        public event IDynamicObjectExtender.BeforeTryConvertDelegate? BeforeTryConvert;

        /// <summary>
        /// Occurs before creating an instance of the dynamic object.
        /// </summary>
        public event IDynamicObjectExtender.BeforeTryCreateInstanceDelegate? BeforeTryCreateInstance;

        /// <summary>
        /// Occurs before deleting an index from the dynamic object.
        /// </summary>
        public event IDynamicObjectExtender.BeforeTryDeleteIndexDelegate? BeforeTryDeleteIndex;

        /// <summary>
        /// Occurs before deleting a member from the dynamic object.
        /// </summary>
        public event IDynamicObjectExtender.BeforeTryDeleteMemberDelegate? BeforeTryDeleteMember;

        /// <summary>
        /// Occurs before retrieving an index from the dynamic object.
        /// </summary>
        public event IDynamicObjectExtender.BeforeTryGetIndexDelegate? BeforeTryGetIndex;

        /// <summary>
        /// Occurs before retrieving a member from the dynamic object.
        /// </summary>
        public event IDynamicObjectExtender.BeforeTryGetMemberDelegate? BeforeTryGetMember;

        /// <summary>
        /// Occurs before invoking a member on the dynamic object.
        /// </summary>
        public event IDynamicObjectExtender.BeforeTryInvokeMemberDelegate? BeforeTryInvokeMember;

        /// <summary>
        /// Occurs before setting an index on the dynamic object.
        /// </summary>
        public event IDynamicObjectExtender.BeforeTrySetIndexDelegate? BeforeTrySetIndex;

        /// <summary>
        /// Occurs before setting a member on the dynamic object.
        /// </summary>
        public event IDynamicObjectExtender.BeforeTrySetMemberDelegate? BeforeTrySetMember;

        /// <summary>
        /// Occurs before performing a unary operation on the dynamic object.
        /// </summary>
        public event IDynamicObjectExtender.BeforeTryUnaryOperationDelegate? BeforeTryUnaryOperation;

        bool IDynamicObjectExtender.BeforeTryInvoke(
            DynamicObject sender,
            InvokeBinder binder,
            object[] args,
            out object? result,
            out bool handled)
        {
            result = null;
            handled = false;

            var invokeResult = BeforeTryInvoke?.Invoke(sender, binder, args, out result, out handled) ?? false;

            if (handled)
                return invokeResult;

            return false;
        }

        IEnumerable<string> IDynamicObjectExtender.BeforeGetDynamicMemberNames(
            DynamicObject sender,
            out bool handled)
        {
            handled = false;
            return Array.Empty<string>();
        }

        bool IDynamicObjectExtender.BeforeTryBinaryOperation(
            DynamicObject sender,
            BinaryOperationBinder binder,
            object arg,
            out object? result,
            out bool handled)
        {
            result = null;
            handled = false;
            return false;
        }

        bool IDynamicObjectExtender.BeforeTryConvert(
            DynamicObject sender,
            ConvertBinder binder,
            out object? result,
            out bool handled)
        {
            result = null;
            handled = false;
            return false;
        }

        bool IDynamicObjectExtender.BeforeTryCreateInstance(
            DynamicObject sender,
            CreateInstanceBinder binder,
            object[] args,
            out object? result,
            out bool handled)
        {
            result = null;
            handled = false;
            return false;
        }

        bool IDynamicObjectExtender.BeforeTryDeleteIndex(
            DynamicObject sender,
            DeleteIndexBinder binder,
            object[] indexes,
            out bool handled)
        {
            handled = false;
            return false;
        }

        bool IDynamicObjectExtender.BeforeTryDeleteMember(
            DynamicObject sender,
            DeleteMemberBinder binder,
            out bool handled)
        {
            handled = false;
            return false;
        }

        bool IDynamicObjectExtender.BeforeTryGetIndex(
            DynamicObject sender,
            GetIndexBinder binder,
            object[] indexes,
            out object? result,
            out bool handled)
        {
            result = null;
            handled = false;
            return false;
        }

        bool IDynamicObjectExtender.BeforeTryGetMember(
            DynamicObject sender,
            GetMemberBinder binder,
            out object? result,
            out bool handled)
        {
            result = null;
            handled = false;
            return false;
        }

        bool IDynamicObjectExtender.BeforeTryInvokeMember(
            DynamicObject sender,
            InvokeMemberBinder binder,
            object[] args,
            out object? result,
            out bool handled)
        {
            result = null;
            handled = false;
            return false;
        }

        bool IDynamicObjectExtender.BeforeTrySetIndex(
            DynamicObject sender,
            SetIndexBinder binder,
            object[] indexes,
            object value,
            out bool handled)
        {
            handled = false;
            return false;
        }

        bool IDynamicObjectExtender.BeforeTrySetMember(
            DynamicObject sender,
            SetMemberBinder binder,
            object value,
            out bool handled)
        {
            handled = false;
            return false;
        }

        bool IDynamicObjectExtender.BeforeTryUnaryOperation(
            DynamicObject sender,
            UnaryOperationBinder binder,
            out object? result,
            out bool handled)
        {
            result = null;
            handled = false;
            return false;
        }
    }
}
