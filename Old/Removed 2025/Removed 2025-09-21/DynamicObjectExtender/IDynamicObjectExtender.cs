using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Alternet.Common
{
    /// <summary>
    /// Defines methods to extend the behavior of a <see cref="DynamicObject"/>.
    /// </summary>
    public interface IDynamicObjectExtender
    {
        /// <inheritdoc cref="IDynamicObjectExtender.BeforeTryInvoke"/>
        public delegate bool BeforeTryInvokeDelegate(
                   DynamicObject sender,
                   InvokeBinder binder,
                   object[] args,
                   out object? result,
                   out bool handled);

        /// <inheritdoc cref="IDynamicObjectExtender.BeforeGetDynamicMemberNames"/>
        public delegate IEnumerable<string> BeforeGetDynamicMemberNamesDelegate(
            DynamicObject sender,
            out bool handled);

        /// <inheritdoc cref="IDynamicObjectExtender.BeforeTryBinaryOperation"/>
        public delegate bool BeforeTryBinaryOperationDelegate(
            DynamicObject sender,
            BinaryOperationBinder binder,
            object arg,
            out object? result,
            out bool handled);

        /// <inheritdoc cref="IDynamicObjectExtender.BeforeTryConvert"/>
        public delegate bool BeforeTryConvertDelegate(
            DynamicObject sender,
            ConvertBinder binder,
            out object? result,
            out bool handled);

        /// <inheritdoc cref="IDynamicObjectExtender.BeforeTryCreateInstance"/>
        public delegate bool BeforeTryCreateInstanceDelegate(
            DynamicObject sender,
            CreateInstanceBinder binder,
            object[] args,
            out object? result,
            out bool handled);

        /// <inheritdoc cref="IDynamicObjectExtender.BeforeTryDeleteIndex"/>
        public delegate bool BeforeTryDeleteIndexDelegate(
            DynamicObject sender,
            DeleteIndexBinder binder,
            object[] indexes,
            out bool handled);

        /// <inheritdoc cref="IDynamicObjectExtender.BeforeTryDeleteMember"/>
        public delegate bool BeforeTryDeleteMemberDelegate(
            DynamicObject sender,
            DeleteMemberBinder binder,
            out bool handled);

        /// <inheritdoc cref="IDynamicObjectExtender.BeforeTryGetIndex"/>
        public delegate bool BeforeTryGetIndexDelegate(
            DynamicObject sender,
            GetIndexBinder binder,
            object[] indexes,
            out object? result,
            out bool handled);

        /// <inheritdoc cref="IDynamicObjectExtender.BeforeTryGetMember"/>
        public delegate bool BeforeTryGetMemberDelegate(
            DynamicObject sender,
            GetMemberBinder binder,
            out object? result,
            out bool handled);

        /// <inheritdoc cref="IDynamicObjectExtender.BeforeTryInvokeMember"/>
        public delegate bool BeforeTryInvokeMemberDelegate(
            DynamicObject sender,
            InvokeMemberBinder binder,
            object[] args,
            out object? result,
            out bool handled);

        /// <inheritdoc cref="IDynamicObjectExtender.BeforeTrySetIndex"/>
        public delegate bool BeforeTrySetIndexDelegate(
            DynamicObject sender,
            SetIndexBinder binder,
            object[] indexes,
            object value,
            out bool handled);

        /// <inheritdoc cref="IDynamicObjectExtender.BeforeTrySetMember"/>
        public delegate bool BeforeTrySetMemberDelegate(
            DynamicObject sender,
            SetMemberBinder binder,
            object value,
            out bool handled);

        /// <inheritdoc cref="IDynamicObjectExtender.BeforeTryUnaryOperation"/>
        public delegate bool BeforeTryUnaryOperationDelegate(
            DynamicObject sender,
            UnaryOperationBinder binder,
            out object? result,
            out bool handled);

        /// <summary>
        /// Called before invoking a method on the dynamic object.
        /// </summary>
        /// <param name="sender">The dynamic object instance.</param>
        /// <param name="binder">The binder providing information about the invocation.</param>
        /// <param name="args">The arguments passed to the method.</param>
        /// <param name="result">The result of the invocation, if handled.</param>
        /// <param name="handled">Indicates whether the invocation was handled.</param>
        /// <returns>True if the invocation should proceed; otherwise, false.</returns>
        bool BeforeTryInvoke(
            DynamicObject sender,
            InvokeBinder binder,
            object[] args,
            out object? result,
            out bool handled);

        /// <summary>
        /// Called before retrieving the dynamic member names.
        /// </summary>
        /// <param name="sender">The dynamic object instance.</param>
        /// <returns>A collection of dynamic member names.</returns>
        /// <param name="handled">Indicates whether the operation was handled.</param>
        /// <returns>True if the operation should proceed; otherwise, false.</returns>
        IEnumerable<string> BeforeGetDynamicMemberNames(DynamicObject sender, out bool handled);

        /// <summary>
        /// Called before performing a binary operation on the dynamic object.
        /// </summary>
        /// <param name="sender">The dynamic object instance.</param>
        /// <param name="binder">The binder providing information about the operation.</param>
        /// <param name="arg">The argument for the binary operation.</param>
        /// <param name="result">The result of the operation, if handled.</param>
        /// <param name="handled">Indicates whether the operation was handled.</param>
        /// <returns>True if the operation should proceed; otherwise, false.</returns>
        bool BeforeTryBinaryOperation(
            DynamicObject sender,
            BinaryOperationBinder binder,
            object arg,
            out object? result,
            out bool handled);

        /// <summary>
        /// Called before converting the dynamic object to another type.
        /// </summary>
        /// <param name="sender">The dynamic object instance.</param>
        /// <param name="binder">The binder providing information about the conversion.</param>
        /// <param name="result">The result of the conversion, if handled.</param>
        /// <param name="handled">Indicates whether the conversion was handled.</param>
        /// <returns>True if the conversion should proceed; otherwise, false.</returns>
        bool BeforeTryConvert(
            DynamicObject sender,
            ConvertBinder binder,
            out object? result,
            out bool handled);

        /// <summary>
        /// Called before creating an instance of the dynamic object.
        /// </summary>
        /// <param name="sender">The dynamic object instance.</param>
        /// <param name="binder">The binder providing information about the creation.</param>
        /// <param name="args">The arguments for the creation.</param>
        /// <param name="result">The result of the creation, if handled.</param>
        /// <param name="handled">Indicates whether the creation was handled.</param>
        /// <returns>True if the creation should proceed; otherwise, false.</returns>
        bool BeforeTryCreateInstance(
            DynamicObject sender,
            CreateInstanceBinder binder,
            object[] args,
            out object? result,
            out bool handled);

        /// <summary>
        /// Called before deleting an index from the dynamic object.
        /// </summary>
        /// <param name="sender">The dynamic object instance.</param>
        /// <param name="binder">The binder providing information about the deletion.</param>
        /// <param name="indexes">The indexes to delete.</param>
        /// <param name="handled">Indicates whether the deletion was handled.</param>
        /// <returns>True if the deletion should proceed; otherwise, false.</returns>
        bool BeforeTryDeleteIndex(
            DynamicObject sender,
            DeleteIndexBinder binder,
            object[] indexes,
            out bool handled);

        /// <summary>
        /// Called before deleting a member from the dynamic object.
        /// </summary>
        /// <param name="sender">The dynamic object instance.</param>
        /// <param name="binder">The binder providing information about the deletion.</param>
        /// <param name="handled">Indicates whether the deletion was handled.</param>
        /// <returns>True if the deletion should proceed; otherwise, false.</returns>
        bool BeforeTryDeleteMember(
            DynamicObject sender,
            DeleteMemberBinder binder,
            out bool handled);

        /// <summary>
        /// Called before retrieving an index from the dynamic object.
        /// </summary>
        /// <param name="sender">The dynamic object instance.</param>
        /// <param name="binder">The binder providing information about the retrieval.</param>
        /// <param name="indexes">The indexes to retrieve.</param>
        /// <param name="result">The result of the retrieval, if handled.</param>
        /// <param name="handled">Indicates whether the retrieval was handled.</param>
        /// <returns>True if the retrieval should proceed; otherwise, false.</returns>
        bool BeforeTryGetIndex(
            DynamicObject sender,
            GetIndexBinder binder,
            object[] indexes,
            out object? result,
            out bool handled);

        /// <summary>
        /// Called before retrieving a member from the dynamic object.
        /// </summary>
        /// <param name="sender">The dynamic object instance.</param>
        /// <param name="binder">The binder providing information about the retrieval.</param>
        /// <param name="result">The result of the retrieval, if handled.</param>
        /// <param name="handled">Indicates whether the retrieval was handled.</param>
        /// <returns>True if the retrieval should proceed; otherwise, false.</returns>
        bool BeforeTryGetMember(
            DynamicObject sender,
            GetMemberBinder binder,
            out object? result,
            out bool handled);

        /// <summary>
        /// Called before invoking a member on the dynamic object.
        /// </summary>
        /// <param name="sender">The dynamic object instance.</param>
        /// <param name="binder">The binder providing information about the invocation.</param>
        /// <param name="args">The arguments passed to the member.</param>
        /// <param name="result">The result of the invocation, if handled.</param>
        /// <param name="handled">Indicates whether the invocation was handled.</param>
        /// <returns>True if the invocation should proceed; otherwise, false.</returns>
        bool BeforeTryInvokeMember(
            DynamicObject sender,
            InvokeMemberBinder binder,
            object[] args,
            out object? result,
            out bool handled);

        /// <summary>
        /// Called before setting an index on the dynamic object.
        /// </summary>
        /// <param name="sender">The dynamic object instance.</param>
        /// <param name="binder">The binder providing information about the setting.</param>
        /// <param name="indexes">The indexes to set.</param>
        /// <param name="value">The value to set.</param>
        /// <param name="handled">Indicates whether the setting was handled.</param>
        /// <returns>True if the setting should proceed; otherwise, false.</returns>
        bool BeforeTrySetIndex(
            DynamicObject sender,
            SetIndexBinder binder,
            object[] indexes,
            object value,
            out bool handled);

        /// <summary>
        /// Called before setting a member on the dynamic object.
        /// </summary>
        /// <param name="sender">The dynamic object instance.</param>
        /// <param name="binder">The binder providing information about the setting.</param>
        /// <param name="value">The value to set.</param>
        /// <param name="handled">Indicates whether the setting was handled.</param>
        /// <returns>True if the setting should proceed; otherwise, false.</returns>
        bool BeforeTrySetMember(
            DynamicObject sender,
            SetMemberBinder binder,
            object value,
            out bool handled);

        /// <summary>
        /// Called before performing a unary operation on the dynamic object.
        /// </summary>
        /// <param name="sender">The dynamic object instance.</param>
        /// <param name="binder">The binder providing information about the operation.</param>
        /// <param name="result">The result of the operation, if handled.</param>
        /// <param name="handled">Indicates whether the operation was handled.</param>
        /// <returns>True if the operation should proceed; otherwise, false.</returns>
        bool BeforeTryUnaryOperation(
            DynamicObject sender,
            UnaryOperationBinder binder,
            out object? result,
            out bool handled);
    }
}
