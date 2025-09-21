using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Alternet.Common
{
    public class DynamicObjectExtender : IDynamicObjectExtender
    {
        public virtual bool BeforeTryInvoke(
            DynamicObject sender,
            InvokeBinder binder,
            object[] args,
            out object? result,
            out bool handled)
        {
            result = null;
            handled = false;
            return false;
        }

        public virtual IEnumerable<string> BeforeGetDynamicMemberNames(DynamicObject sender, out bool handled)
        {
            handled = false;
            return Array.Empty<string>();
        }

        public virtual bool BeforeTryBinaryOperation(
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

        public virtual bool BeforeTryConvert(
            DynamicObject sender,
            ConvertBinder binder,
            out object? result,
            out bool handled)
        {
            result = null;
            handled = false;
            return false;
        }

        public virtual bool BeforeTryCreateInstance(
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

        public virtual bool BeforeTryDeleteIndex(
            DynamicObject sender,
            DeleteIndexBinder binder,
            object[] indexes,
            out bool handled)
        {
            handled = false;
            return false;
        }

        public virtual bool BeforeTryDeleteMember(
            DynamicObject sender,
            DeleteMemberBinder binder,
            out bool handled)
        {
            handled = false;
            return false;
        }

        public virtual bool BeforeTryGetIndex(
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

        public virtual bool BeforeTryGetMember(
            DynamicObject sender,
            GetMemberBinder binder,
            out object? result,
            out bool handled)
        {
            result = null;
            handled = false;
            return false;
        }

        public virtual bool BeforeTryInvokeMember(
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

        public virtual bool BeforeTrySetIndex(
            DynamicObject sender,
            SetIndexBinder binder,
            object[] indexes,
            object value,
            out bool handled)
        {
            handled = false;
            return false;
        }

        public virtual bool BeforeTrySetMember(
            DynamicObject sender,
            SetMemberBinder binder,
            object value,
            out bool handled)
        {
            handled = false;
            return false;
        }

        public virtual bool BeforeTryUnaryOperation(
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
