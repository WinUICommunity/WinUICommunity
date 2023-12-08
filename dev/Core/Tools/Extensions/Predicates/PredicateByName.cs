﻿namespace WindowUI;

/// <summary>
/// An <see cref="IPredicate{T}"/> type matching <see cref="FrameworkElement"/> instances by name.
/// </summary>
internal readonly struct PredicateByName : IPredicate<FrameworkElement>
{
    /// <summary>
    /// The name of the element to look for.
    /// </summary>
    private readonly string name;

    /// <summary>
    /// The comparison type to use to match <see name="name"/>.
    /// </summary>
    private readonly StringComparison comparisonType;

    /// <summary>
    /// Initializes a new instance of the <see cref="PredicateByName"/> struct.
    /// </summary>
    /// <param name="name">The name of the element to look for.</param>
    /// <param name="comparisonType">The comparison type to use to match <paramref name="name"/>.</param>
    public PredicateByName(string name, StringComparison comparisonType)
    {
        this.name = name;
        this.comparisonType = comparisonType;
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Match(FrameworkElement element)
    {
        return element.Name.Equals(this.name, this.comparisonType);
    }
}
