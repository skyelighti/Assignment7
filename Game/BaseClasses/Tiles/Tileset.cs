using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameBase.Assets;
using Microsoft.Xna.Framework;
namespace GameBase.Tiles;

public class Tileset
{
    private readonly TileInfo[] _tiles;
    /// <summary>
    /// Gets the width, in pixels, of each tile in this tileset.
    /// </summary>
    public int TileWidth { get; }

    /// <summary>
    /// Gets the height, in pixels, of each tile in this tileset.
    /// </summary>
    public int TileHeight { get; }

    public string Name { get; }
    /// <summary>
    /// Gets the total number of columns in this tileset.
    /// </summary>
    public int Columns { get; }

    /// <summary>
    /// Gets the total number of rows in this tileset.
    /// </summary>
    public int Rows { get; }

    /// <summary>
    /// Gets the total number of tiles in this tileset.
    /// </summary>
    public int Count { get; }
    public SpriteSheet sheet { get; }
    /// <summary>
    /// Creates a new tileset based on the given texture region with the specified
    /// tile width and height.
    /// </summary>
    /// <param name="textureRegion">The texture region that contains the tiles for the tileset.</param>
    /// <param name="tileWidth">The width of each tile in the tileset.</param>
    /// <param name="tileHeight">The height of each tile in the tileset.</param>
    public Tileset(string name, SpriteSheet sheet)
    {
        Name = name;
        TileWidth = sheet.frameWidth;
        TileHeight = sheet.frameHeight;

        Columns = sheet.columns;
        Rows = sheet.rows;
        Count = Columns * Rows;
        this.sheet = sheet;

        // Create the texture regions that make up each individual tile
        _tiles = new TileInfo[Count];

        for (int i = 0; i < Count; i++)
        {
            _tiles[i] = new TileInfo
            {
                Region = sheet.GetTextureRegion(i),
                IsSolid = false
            };
        }
    }

    /// <summary>
    /// Gets the texture region for the tile from this tileset at the given index.
    /// </summary>
    /// <param name="index">The index of the texture region in this tile set.</param>
    /// <returns>The texture region for the tile form this tileset at the given index.</returns>
    public TileInfo GetTile(int index) => _tiles[index];

    /// <summary>
    /// Gets the texture region for the tile from this tileset at the given location.
    /// </summary>
    /// <param name="column">The column in this tileset of the texture region.</param>
    /// <param name="row">The row in this tileset of the texture region.</param>
    /// <returns>The texture region for the tile from this tileset at given location.</returns>
    public TileInfo GetTile(int column, int row)
    {
        int index = row * Columns + column;
        return GetTile(index);
    }

    public void SetSolid(int tileID, bool isSolid)
    {
        _tiles[tileID] = new TileInfo
        {
            Region = _tiles[tileID].Region,
            IsSolid = isSolid,
            IsTrigger = _tiles[tileID].IsTrigger,
            layer = _tiles[tileID].layer,
            CollisionBounds = _tiles[tileID].CollisionBounds
        };
    }
    public void SetCollisionBounds(int tileID, Rectangle rect) {
        _tiles[tileID] = new TileInfo
        {
            Region = _tiles[tileID].Region,
            IsSolid = _tiles[tileID].IsSolid,
            IsTrigger = _tiles[tileID].IsTrigger,
            layer = _tiles[tileID].layer,
            CollisionBounds = rect
        };
    }

    public bool IsSolid(int tileID)
    {
        return _tiles[tileID].IsSolid;
    }

    public void SetTrigger(int tileID, bool isTrigger)
    {
        TileInfo tile = _tiles[tileID];
        _tiles[tileID] = new TileInfo
        {
            Region = tile.Region,
            IsSolid = tile.IsSolid,
            IsTrigger = isTrigger,
            layer = tile.layer,
            CollisionBounds = tile.CollisionBounds
        };
    }

    public void SetLayer(int tileID, Layer layer)
    {
        TileInfo tile = _tiles[tileID];
        _tiles[tileID] = new TileInfo
        {
            Region = tile.Region,
            IsSolid = tile.IsSolid,
            IsTrigger = tile.IsTrigger,
            layer = layer,
            CollisionBounds = tile.CollisionBounds
        };
    }

}
