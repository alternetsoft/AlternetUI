using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a method that evaluates the specified C# code
    /// asynchronously and returns the result.
    /// </summary>
    /// <remarks>This method uses the Roslyn scripting API to evaluate the provided C# code.
    /// The evaluation is performed in an isolated scripting context.</remarks>
    /// <param name="owner">An optional object that can be passed to the evaluation method
    /// in order to determine the context of the evaluation. For example <see cref="Calculator"/>
    /// passes itself.</param>
    /// <param name="code">The C# code to evaluate. This cannot be null or empty.</param>
    /// <param name="options">Optional script options to configure the evaluation,
    /// such as references and imports. Can be null.</param>
    /// <param name="globalObject">An optional object to provide global variables
    /// or state to the script. Can be null.</param>
    /// <param name="globalType">The type of the global object, if specified. Can be null.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.
    /// Defaults to <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation.
    /// The task result contains the evaluated value.</returns>
    public delegate Task<object> EvaluateAsyncDelegate(
            object? owner,
            string code,
            object? options = null,
            object? globalObject = null,
            Type? globalType = null,
            CancellationToken cancellationToken = default);

    /// <summary>
    /// Provides functionality for evaluating C# code dynamically and managing
    /// the formula engine's lifecycle.
    /// </summary>
    /// <remarks>The <see cref="FormulaEngine"/> class allows for the
    /// evaluation of C# code snippets in an
    /// isolated scripting context using the Roslyn scripting API.
    /// It also provides a method to initialize the engine,
    /// which can be used to preload necessary libraries for
    /// improved performance during runtime.</remarks>
    public static class FormulaEngine
    {
        private static bool engineInitialized;

        /// <summary>
        /// An optional override for the evaluation method.
        /// This can be set to provide a custom implementation.
        /// </summary>
        public static EvaluateAsyncDelegate? EvaluateOverride { get; set; }

        /// <summary>
        /// Evaluates the specified C# code asynchronously and returns the result.
        /// If <see cref="EvaluateOverride"/> is set, it will be used instead
        /// of the default implementation.
        /// </summary>
        /// <remarks>This method uses the Roslyn scripting API to evaluate the provided C# code.
        /// The evaluation is performed in an isolated scripting context.</remarks>
        /// <param name="owner">An optional object that can be passed to the evaluation method
        /// in order to determine the context of the evaluation. For example <see cref="Calculator"/>
        /// passes itself.</param>
        /// <param name="code">The C# code to evaluate. This cannot be null or empty.</param>
        /// <param name="options">Optional script options to configure the evaluation,
        /// such as references and imports. Can be null.</param>
        /// <param name="globalObject">An optional object to provide global variables
        /// or state to the script. Can be null.</param>
        /// <param name="globalType">The type of the global object, if specified. Can be null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.
        /// Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains the evaluated value.</returns>
        public static Task<object> EvaluateAsync(
            object? owner,
            string code,
            object? options = null,
            object? globalObject = null,
            Type? globalType = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (EvaluateOverride != null)
                {
                    return EvaluateOverride(
                        owner,
                        code,
                        options,
                        globalObject,
                        globalType,
                        cancellationToken);
                }

                var engine = new SimpleFormulaEvaluator(code);
                var result = engine.Evaluate();
                return Task.FromResult<object>(result);

/*
                MethodInfo? methodInfo = null;

                var result = AssemblyUtils.InvokeMethodWithResult(
                    typeof(Microsoft.CodeAnalysis.CSharp.Scripting.CSharpScript),
                    "EvaluateAsync",
                    ref methodInfo,
                    null,
                    new object?[]
                    {
                        code,
                        options,
                        globalObject,
                        globalType,
                        cancellationToken,
                    },
                    new System.Type[]
                    {
                        typeof(string),
                        typeof(Microsoft.CodeAnalysis.Scripting.ScriptOptions),
                        typeof(object),
                        typeof(Type),
                        typeof(CancellationToken),
                    });

                if (result is not Task<object> taskResult)
                {
                    return Task.FromException<object>(
                        new InvalidOperationException("Evaluation failed."));
                }
                return taskResult;
*/
            }
            catch (Exception e)
            {
                BaseObject.Nop(e);
                return Task.FromException<object>(e);
            }
        }

        /// <summary>
        /// Initializes formula engine. Do not need to call it directly. It
        /// can be called from the application startup in order to preload formula
        /// engine libraries.
        /// </summary>
        public static void Init()
        {
            try
            {
                if (!engineInitialized)
                {
                    EvaluateAsync(null, "2");
                    engineInitialized = true;
                }
            }
            catch (Exception e)
            {
                BaseObject.Nop(e);
            }
        }
    }
}
