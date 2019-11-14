using System.Collections.Generic;
using Game5.Env;

namespace Game5.Service.Services.Interfaces
{
    public interface ITileRepository
    {
        Tile this[int id] { get; }
        Tile this[string id] { get; }

        IEnumerator<Tile> GetEnumerator();
        void RegisterAllTiles();
        void RegisterJson(string json);
    }
}