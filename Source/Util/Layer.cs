using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Util;

/// <summary>
/// Layer Script.
/// </summary>
public static class Layer
{
    /// <summary>
    /// Gets the layer mask for the specified mask index.
    /// </summary>
    /// <param name="maskIndex">The index of the mask.</param>
    /// <returns>The layer mask for the specified index.</returns>
    public static int GetLayer(int maskIndex)
    {
        return 1 << maskIndex;
    }
}
