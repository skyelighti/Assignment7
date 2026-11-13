using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using GameBase.Assets;
using System.Net.Sockets;

namespace GameBase.Tiles;

public class Tilemap
{
    private readonly Dictionary<string, Tileset> _tileset = new();
    private readonly TileReference[] _tiles;
    Dictionary<int, AnimationController> tileAnimations = new();

    /// <summary>
    /// Gets the total number of rows in this tilemap.
    /// </summary>
    public int Rows { get; }

    /// <summary>
    /// Gets the total number of columns in this tilemap.
    /// </summary>
    public int Columns { get; }

    /// <summary>
    /// Gets the total number of tiles in this tilemap.
    /// </summary>
    public int Count { get; }

    /// <summary>
    /// Gets or Sets the scale factor to draw each tile at.
    /// </summary>
    public Vector2 Scale { get; set; }

    public int CellWidth { get; }
    public int CellHeight { get; }

    public float TileWidth => CellWidth * Scale.X;
    public float TileHeight => CellHeight * Scale.Y;

    /// <summary>
    /// Creates a new tilemap.
    /// </summary>
    /// <param name="tileset">The tileset used by this tilemap.</param>
    /// <param name="columns">The total number of columns in this tilemap.</param>
    /// <param name="rows">The total number of rows in this tilemap.</param>
    /// <param name="cellWidth">The width of each tile in pixels.</param>
    /// <param name="cellHeight">The height of each tile in pixels.</param>
    public Tilemap(Dictionary<string, Tileset> tileset, int columns, int rows, int cellWidth, int cellHeight)
    {
        _tileset = tileset;
        Rows = rows;
        Columns = columns;
        Count = Columns * Rows;
        Scale = Vector2.One;
        CellWidth = cellWidth;
        CellHeight = cellHeight;
        _tiles = new TileReference[Count];
        Array.Fill(_tiles, null);
    }

    /// <summary>
    /// Sets the tile at the given index in this tilemap to use the tile from
    /// the tileset at the specified tileset id.
    /// </summary>
    /// <param name="index">The index of the tile in this tilemap.</param>
    /// <param name="tileReference">The tile reference containing the tileset name and ID.</param>
    public void SetTile(int index, TileReference tileReference)
    {
        _tiles[index] = tileReference;
    }

    /// <summary>
    /// Sets the tile at the given column and row in this tilemap to use the tile
    /// from the tileset at the specified tileset id.
    /// </summary>
    /// <param name="column">The column of the tile in this tilemap.</param>
    /// <param name="row">The row of the tile in this tilemap.</param>
    /// <param name="tilesetName">The name of the tileset containing the tile.</param>
    /// <param name="tilesetID">The tileset id of the tile from the tileset to use.</param>
    public void SetTile(int column, int row, string tileset, int tilesetID)
    {
        int index = row * Columns + column;
        SetTile(index, new TileReference(tileset, tilesetID));
    }

    /// <summary>
    /// Gets the texture region of the tile from this tilemap at the specified index.
    /// </summary>
    /// <param name="index">The index of the tile in this tilemap.</param>
    /// <returns>The texture region of the tile from this tilemap at the specified index.</returns>
    public TileInfo GetTile(int index)
    {
        TileReference tileReference = _tiles[index];

        if (tileReference == null)
            return null;
        return _tileset[tileReference.TilesetName].GetTile(tileReference.TileID);
    }

    /// <summary>
    /// Gets the texture region of the tile from this tilemap at the specified
    /// column and row.
    /// </summary>
    /// <param name="column">The column of the tile in this tilemap.</param>
    /// <param name="row">The row of the tile in this tilemap.</param>
    /// <returns>The texture region of the tile from this tilemap at the specified column and row.</returns>
    public TileInfo GetTile(int column, int row)
    {
        int index = row * Columns + column;
        return GetTile(index);
    }

    /// <summary>
    /// Draws this tilemap using the given sprite batch.
    /// </summary>
    /// <param name="spriteBatch">The sprite batch used to draw this tilemap.</param>
    public void Draw(SpriteBatch spriteBatch)
    {
        for (int i = 0; i < Count; i++)
        {
            TileReference tileReference = _tiles[i];
            if (tileReference == null)
                continue;
            TextureRegion tile;
            if (tileAnimations.ContainsKey(i))
            {
                var controller = tileAnimations[i];
                var frame = controller.Animation.Frames[controller.CurrentFrame];
                tile = frame.Region;
            }
            else {
                tile = _tileset[tileReference.TilesetName].GetTile(tileReference.TileID).Region;
            }

            int x = i % Columns;
            int y = i / Columns;

            Vector2 position = new Vector2(x * TileWidth, y * TileHeight);
            tile.Draw(spriteBatch, position, Color.White, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 1.0f);
        }
    }

    public IEnumerable<TileCollider> CreateColliders()
    {
        for (int row = 0; row < Rows; row++)
        {
            for (int column = 0; column < Columns; column++)
            {
                TileInfo tile = GetTile(column, row);
                if (tile == null || (!tile.IsSolid && !tile.IsTrigger))
                    continue;

                var position = new Vector2(column * TileWidth, row * TileHeight);
                yield return new TileCollider(tile, new Transform(position, 0f, Scale));
            }
        }
    }

    public bool IsSolid(int column, int row)
    {
        if (column < 0 || column >= Columns || row < 0 || row >= Rows)
        {
            return false;
        }

        int index = row * Columns + column;
        TileReference tileReference = _tiles[index];
        if (tileReference == null)
            return false;
        return _tileset[tileReference.TilesetName].IsSolid(tileReference.TileID);
    }

    public void SetAnimation(int tileIdx, string animationName)
    {
        Tileset ts = _tileset[_tiles[tileIdx].TilesetName];
        SpriteSheet sheet = ts.sheet;
        if (animationName == null) {
            return;
        }
        IAnimation animation = Services.Get<SpriteLibrary>().GetAnimation($"{sheet.Name}.{animationName}");
        if (animation != null)
        {
            if (!tileAnimations.ContainsKey(tileIdx)) {
                tileAnimations[tileIdx] = new AnimationController();
            }
            tileAnimations[tileIdx].ChangeAnimation(animation);
            tileAnimations[tileIdx].Play();
        }

    }

    public void Update(GameTime gameTime) {
        foreach (AnimationController x in tileAnimations.Values) {
            x.Update(gameTime);
        }
    }

}
