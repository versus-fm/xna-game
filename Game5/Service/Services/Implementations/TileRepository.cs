using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Game5.Data.Attributes.Service;
using Game5.Data.Attributes.Tile;
using Game5.Data.Helper;
using Game5.Env;
using Game5.Service.Services.Interfaces;
using Newtonsoft.Json.Linq;

namespace Game5.Service.Services.Implementations
{
    [Service(typeof(ITileRepository))]
    public class TileRepository : IEnumerable<Tile>, ITileRepository
    {
        private readonly Dictionary<int, Tile> tiles;
        private readonly Dictionary<string, Tile> tilesByName;

        public TileRepository()
        {
            tilesByName = new Dictionary<string, Tile>();
            tiles = new Dictionary<int, Tile>();
        }

        public IEnumerator<Tile> GetEnumerator()
        {
            return tiles.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return tiles.Values.GetEnumerator();
        }

        public void RegisterAllTiles()
        {
            var assembly = Assembly.GetCallingAssembly();
            foreach (var type in assembly.GetTypes())
                if (type.GetCustomAttributes(typeof(TileCollectionAttribute), true).Length > 0)
                {
                    var obj = Activator.CreateInstance(type);
                    var properties = type.GetProperties()
                        .Where(prop => prop.IsDefined(typeof(TileEntryAttribute), false));
                    foreach (var prop in properties)
                    {
                        var attr = (TileEntryAttribute) prop.GetCustomAttribute(typeof(TileEntryAttribute));
                        if (attr.EntryType == TileEntryType.Singular)
                        {
                            var val = (Tile) prop.GetValue(obj);
                            tiles.Add(GetFreeId(val), val);
                            if (val.Name == "") val.Name = val.Id.ToString();
                            tilesByName.Add(val.Name, val);
                        }

                        if (attr.EntryType == TileEntryType.Collection)
                        {
                            var val = (IEnumerable<Tile>) prop.GetValue(obj);
                            foreach (var tile in val)
                            {
                                tiles.Add(GetFreeId(tile), tile);
                                if (tile.Name == "") tile.Name = tile.Id.ToString();
                                tilesByName.Add(tile.Name, tile);
                            }
                        }
                    }
                }
        }

        public Tile this[int id]
        {
            get
            {
                if (!tiles.ContainsKey(id)) return null;
                return tiles[id];
            }
        }

        public Tile this[string id]
        {
            get
            {
                if (!tilesByName.ContainsKey(id)) return null;
                return tilesByName[id];
            }
        }

        public void RegisterJson(string json)
        {
            var obj = JObject.Parse(json);
            foreach (var root in obj.Root)
            {
                var propertyList = root.Children().First();
                foreach (var tileListing in propertyList.Children())
                {
                    var name = tileListing["Type"].Value<string>();
                    var tile = (Tile) Activator.CreateInstance(TileTypeCache.GetTileType(name));
                    var data = tileListing["Data"].Value<JToken>();
                    foreach (JProperty val in data.Children())
                    {
                        var property = tile.GetType().GetProperty(val.Name);
                        property.SetValue(tile,
                            val.Value.ChangeType(property.PropertyType, CultureInfo.CurrentCulture));
                    }

                    tiles.Add(GetFreeId(tile), tile);
                    if (tile.Name == "") tile.Name = tile.Id.ToString();
                    tilesByName.Add(tile.Name, tile);
                }
            }
        }

        private int GetFreeId(Tile t)
        {
            var id = t.Id;
            if (id == -1) return t.Id;
            while (tiles.ContainsKey(id)) id++;
            t.Id = id;
            return id;
        }
    }
}