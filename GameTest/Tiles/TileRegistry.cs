using Game5.Data.Attributes;
using Game5.Data.Attributes.Tile;
using Game5.Env;

namespace GameTest.Tiles
{
    [TileCollection]
    public class TileRegistry
    {
        [TileEntry] public Tile dirtTile => new Tile().SetTextureName("test");

        [TileEntry] public Tile slope => new Tile().SetTextureName("test");
    }
}