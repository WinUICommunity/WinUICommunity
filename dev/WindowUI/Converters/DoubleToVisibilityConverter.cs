﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace WindowUI;

/// <summary>
/// This class converts a double value into a Visibility enumeration.
/// </summary>
internal class DoubleToVisibilityConverter : DoubleToObjectConverter
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DoubleToVisibilityConverter"/> class.
    /// </summary>
    public DoubleToVisibilityConverter()
    {
        TrueValue = Visibility.Visible;
        FalseValue = Visibility.Collapsed;
        NullValue = Visibility.Collapsed;
    }
}
