using System;

namespace Game5.Data.Attributes.Tile
{
    public class TileEntryAttribute : Attribute
    {
        public TileEntryAttribute(TileEntryType tileEntryType = TileEntryType.Singular)
        {
            EntryType = tileEntryType;
        }

        public TileEntryType EntryType { get; set; }
    }
}