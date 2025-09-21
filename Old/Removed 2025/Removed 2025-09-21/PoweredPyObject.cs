using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using System.Text;

using Alternet.Common;

using Python.Runtime;

namespace Alternet.Scripter.Python
{
    public class PoweredPyObject : PyObject
    {
        private PyObject obj;

        public PoweredPyObject(BorrowedReference reference)
            : base(reference)
        {
        }

        public PoweredPyObject(BorrowedReference reference, bool skipCollect)
            : base(reference, skipCollect)
        {
        }

        public PoweredPyObject(in StolenReference reference)
            : base(in reference)
        {
        }

        public static IDynamicObjectExtender GlobalExtender { get; set; }

        /// <summary>
        /// Gets raw Python proxy for this object (bypasses all conversions,
        /// except <c>null</c> &lt;==&gt; <c>None</c>)
        /// </summary>
        /// <remarks>
        /// Given an arbitrary managed object, return a Python instance that
        /// reflects the managed object.
        /// </remarks>
        public static PyObject PoweredFromManagedObject(object ob)
        {
            // Special case: if ob is null, we return None.
            if (ob == null)
            {
                return new PyObject(Runtime.PyNone);
            }

            var reference = CLRObject.GetReference(ob);

            if (reference.IsNull()) throw new NullReferenceException();

            return new PoweredPyObject(reference.StealNullable());
        }

        /// <inheritdoc/>
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            bool invokeResult;

            if(GlobalExtender is not null)
            {
                invokeResult = GlobalExtender.BeforeTryInvokeMember(
                    this,
                    binder,
                    args,
                    out result,
                    out var handled);
                if (handled)
                    return invokeResult;
            }

            invokeResult = base.TryInvokeMember(binder, args, out result);
            return invokeResult;
        }

        /// <inheritdoc/>
        public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
        {
            var invokeResult = base.TryInvoke(binder, args, out result);
            return invokeResult;
        }

        /// <inheritdoc/>
        public override IEnumerable<string> GetDynamicMemberNames()
        {
            var invokeResult = base.GetDynamicMemberNames();
            return invokeResult;
        }

        /// <inheritdoc/>
        public override bool TryBinaryOperation(BinaryOperationBinder binder, object arg, out object result)
        {
            var invokeResult = base.TryBinaryOperation(binder, arg, out result);
            return invokeResult;
        }

        /// <inheritdoc/>
        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            var invokeResult = base.TryConvert(binder, out result);
            return invokeResult;
        }

        /// <inheritdoc/>
        public override bool TryCreateInstance(CreateInstanceBinder binder, object[] args, out object result)
        {
            var invokeResult = base.TryCreateInstance(binder, args, out result);
            return invokeResult;
        }

        /// <inheritdoc/>
        public override bool TryDeleteIndex(DeleteIndexBinder binder, object[] indexes)
        {
            var invokeResult = base.TryDeleteIndex(binder, indexes);
            return invokeResult;
        }

        /// <inheritdoc/>
        public override bool TryDeleteMember(DeleteMemberBinder binder)
        {
            var invokeResult = base.TryDeleteMember(binder);
            return invokeResult;
        }

        /// <inheritdoc/>
        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            var invokeResult = base.TryGetIndex(binder, indexes, out result);
            return invokeResult;
        }

        /// <inheritdoc/>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var invokeResult = base.TryGetMember(binder, out result);
            return invokeResult;
        }

        /// <inheritdoc/>
        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            var invokeResult = base.TrySetIndex(binder, indexes, value);
            return invokeResult;
        }

        /// <inheritdoc/>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var invokeResult = base.TrySetMember(binder, value);
            return invokeResult;
        }

        /// <inheritdoc/>
        public override bool TryUnaryOperation(UnaryOperationBinder binder, out object result)
        {
            var invokeResult = base.TryUnaryOperation(binder, out result);
            return invokeResult;
        }
    }
}