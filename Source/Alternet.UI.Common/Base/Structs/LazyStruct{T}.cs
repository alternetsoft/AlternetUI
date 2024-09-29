using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.ExceptionServices;
using System.Threading;

namespace Alternet.UI;

/// <summary>
/// Provides support for lazy initialization. This is not thread-safe struct.
/// </summary>
/// <typeparam name="T">The type of object that is being lazily initialized.</typeparam>
public class LazyStruct<T>
{
    private readonly Func<T> factory;

    private T? val;
    private bool isValueCreated;

    /// <summary>
    /// Initializes a new instance of the <see cref="LazyStruct{T}"/> class.
    /// </summary>
    /// <param name="valueFactory">Functions which is called in order to create value. Optional.
    /// If not specified, default constructor for the <typeparamref name="T"/> is used.</param>
    public LazyStruct(Func<T> valueFactory)
    {
        ExceptionUtils.DebugThrowIfNull(valueFactory, nameof(valueFactory));
        factory = valueFactory;
    }

    /// <summary>
    /// Gets whether a value has been created for this object.
    /// </summary>
    public bool IsValueCreated => isValueCreated;

    /// <summary>
    /// Gets the lazily initialized value.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public T Value
    {
        get
        {
            if (isValueCreated)
                return val!;
            isValueCreated = true;
            val = factory();
            return val;
        }

        set
        {
            isValueCreated = true;
            val = value;
        }
    }

    /// <summary>
    /// Converts this object to a human readable string.
    /// </summary>
    public override string? ToString()
    {
        if (!IsValueCreated)
        {
            return "Value is not created.";
        }

        return Value?.ToString();
    }
}
