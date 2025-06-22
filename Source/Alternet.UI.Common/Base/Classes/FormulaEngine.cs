using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a method that evaluates the specified C# code
    /// asynchronously and returns the result as the specified type.
    /// </summary>
    /// <remarks>This method uses the Roslyn scripting API to evaluate the provided C# code.
    /// The evaluation is performed in an isolated scripting context,
    /// and the result is returned as the specified type
    /// <typeparamref name="T"/>.</remarks>
    /// <typeparam name="T">The type of the result expected from the evaluation.</typeparam>
    /// <param name="code">The C# code to evaluate. This cannot be null or empty.</param>
    /// <param name="options">Optional script options to configure the evaluation,
    /// such as references and imports. Can be null.</param>
    /// <param name="globalObject">An optional object to provide global variables
    /// or state to the script. Can be null.</param>
    /// <param name="globalType">The type of the global object, if specified. Can be null.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.
    /// Defaults to <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation.
    /// The task result contains the evaluated value of type
    /// <typeparamref name="T"/>.</returns>
    public delegate Task<T> EvaluateAsyncDelegate<T>(
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
        /// Evaluates the specified C# code asynchronously and returns the result
        /// as the specified type.
        /// </summary>
        /// <remarks>This method uses the Roslyn scripting API to evaluate the provided C# code.
        /// The evaluation is performed in an isolated scripting context,
        /// and the result is returned as the specified type
        /// <typeparamref name="T"/>.</remarks>
        /// <typeparam name="T">The type of the result expected from the evaluation.</typeparam>
        /// <param name="code">The C# code to evaluate. This cannot be null or empty.</param>
        /// <param name="options">Optional script options to configure the evaluation,
        /// such as references and imports. Can be null.</param>
        /// <param name="globalObject">An optional object to provide global variables
        /// or state to the script. Can be null.</param>
        /// <param name="globalType">The type of the global object, if specified. Can be null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.
        /// Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains the evaluated value of type
        /// <typeparamref name="T"/>.</returns>
        public static Task<T> EvaluateAsync<T>(
            string code,
            object? options = null,
            object? globalObject = null,
            Type? globalType = null,
            CancellationToken cancellationToken = default)
        {
            var result = CSharpScript.EvaluateAsync<T>(
                code,
                (ScriptOptions?)options,
                globalObject,
                globalType);
            return result;
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
                    EvaluateAsync<object>("2");
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
