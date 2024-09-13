// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;

namespace WinUICommunity;

/// <summary>
/// A markup extension that returns a collection of values of a specific <see langword="enum"/>
/// </summary>
[MarkupExtensionReturnType(ReturnType = typeof(Array))]
public sealed partial class EnumValuesExtension : MarkupExtension
{
    /// <summary>
    /// Gets or sets the <see cref="global::System.Type"/> of the target <see langword="enum"/>
    /// </summary>
    public Type? Type { get; set; }

    /// <inheritdoc/>
    [UnconditionalSuppressMessage("AOT", "IL3050:Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.", Justification = "<Pending>")]
    protected override object ProvideValue()
    {
        // TODO: We should probably make a throw helper and throw here if type is null?
        return Enum.GetValues(Type!);
    }

}
