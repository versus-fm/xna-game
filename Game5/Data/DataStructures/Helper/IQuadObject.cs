using System;
using Microsoft.Xna.Framework;

namespace Game5.Data.DataStructures.Helper
{
    /// <summary>
    ///     This is an item that can be added to a quadtree.
    /// </summary>
    public interface IQuadObject
    {
        /// <summary>
        ///     Event that gets called when the location or size of the bounding rect changes.
        ///     Used to update the quadtree
        /// </summary>
        event EventHandler BoundsChanged;

        Rectangle GetBounds();
    }
}