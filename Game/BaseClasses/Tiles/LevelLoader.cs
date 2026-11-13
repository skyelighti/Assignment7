using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace GameBase.Tiles;

public static class LevelLoader
{
    public static LevelData LoadLevelData(ContentManager content, string filename) {
        string filePath = Path.Combine(content.RootDirectory, filename);
        
        using (Stream stream = TitleContainer.OpenStream(filePath))
        {
            int nextFirstID = 1;
            using (XmlReader reader = XmlReader.Create(stream))
            {
                XDocument doc = XDocument.Load(reader);
                XElement root = doc.Root;
                Dictionary<string, Tileset> tileSetDict = new(); 
                Dictionary<int, Tileset> tileSetsByID = new();

                foreach (XElement set in root.Elements("Tileset")) {
                    string n = (string)set.Element("name");
                    SpriteSheet s = Services.Get<SpriteLibrary>().GetSpriteSheet((string)set.Element("sheet"));
                    
                    Tileset tileSet = new Tileset(n, s);
                    //creates the base tileset
                    tileSetDict[n] = tileSet;
                    tileSetsByID[nextFirstID] = tileSet;

                    //configs the base tileset, i should maybe seperate this out, so that multiple tilesets can be loaded
                 

                    nextFirstID += tileSet.Count;
                }

                foreach (int solid in ReadTileIds(root.Element("SolidTiles")))
                {
                    TileReference t = GetTile(solid, tileSetsByID);
                    if (t != null)
                    {
                        Tileset ts = tileSetDict[t.TilesetName];
                        ts.SetSolid(t.TileID, true);
                    }
                }
                foreach (int trigger in ReadTileIds(root.Element("TriggerTiles")))
                {
                    TileReference t = GetTile(trigger, tileSetsByID);
                    if (t != null)
                    {
                        Tileset ts = tileSetDict[t.TilesetName];
                        ts.SetTrigger(t.TileID, true);
                    }
                }
                foreach (XElement l in root.Elements("TileLayers").Elements("Layer"))
                {
                    string layerName = (string)l.Attribute("name");
                    Layer layer = Enum.Parse<Layer>(layerName);
                    foreach (int id in ReadTileIds(l))
                    {
                        TileReference t = GetTile(id, tileSetsByID);
                        if (t != null)
                        {
                            Tileset ts = tileSetDict[t.TilesetName];
                            ts.SetLayer(t.TileID, layer);
                        }
                    }
                }
                foreach (XElement customcol in root.Elements("TileColliders").Elements("Tile"))
                {
                    int colnum = (int)customcol.Attribute("id");
                    TileReference t = GetTile(colnum, tileSetsByID);
                    if (t != null)
                    {
                        Tileset ts = tileSetDict[t.TilesetName];
                        Rectangle r = new Rectangle((int)customcol.Attribute("x"), (int)customcol.Attribute("y"), (int)customcol.Attribute("width"), (int)customcol.Attribute("height"));
                        ts.SetCollisionBounds(t.TileID, r);
                    }
                }

                Tilemap map = ReadLayer(tileSetDict, tileSetsByID, root.Element("Tiles"));


                List<SpawnInfo> spawns = ReadSpawns(root, map.Columns, map.Rows);
                LevelData lData = new LevelData(map,spawns);
                return lData;
            }
        }
    }
    public static TileReference GetTile(int globalID, Dictionary<int, Tileset> tileSetsByID)
    {
        //converting from a global ID/number into a local one for tilemap reference
        if (globalID == 0)
            return null;

        int bestFirstID = -1;

        foreach (int firstID in tileSetsByID.Keys)
        {
            if (firstID <= globalID && firstID > bestFirstID)
            {
                bestFirstID = firstID;
            }
        }

        Tileset tileset = tileSetsByID[bestFirstID];

        int localID = globalID - bestFirstID;

        return new TileReference(tileset.Name, localID);
    }
    public static List<SpawnInfo> ReadSpawns(XElement root, int columns, int rows)
    {
        //makes the spawninfos from the xml and puts them all in a list
        List<SpawnInfo> spawns = new();
        foreach (XElement spawn in root.Elements("Spawns").Elements("Spawn"))
        {
            //i should make soemthing for constructing spawns specifically
            string t = (string)spawn.Attribute("type");
            int column = int.Parse((string)spawn.Attribute("column"));
            int row = int.Parse((string)spawn.Attribute("row"));
            string definition = (string)spawn.Attribute("definition");
            string id = (string)spawn.Attribute("id");

            var s = new SpawnInfo(t, column, row, definition, id);
            spawns.Add(s);

        }
        return spawns;
    }
    private static Tilemap ReadLayer(Dictionary<string, Tileset> tileset, Dictionary<int, Tileset> tileSetsByID, XElement element)
    {
        //determines the rows and columns, populating the tileset based off the xml.
        //specifically seperated from load level data if i want to layer multiple tilemaps
        string[] rows = element.Value.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        int rowCount = rows.Length;
        List<int> GlobalIDs = new();
        for (int x = 0; x < rows.Length; x++) { 
           string[] i = rows[x].Split(new[] { ' ', '\t',',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string j in i) {
                GlobalIDs.Add(int.Parse(j));
            }
        }
        int columnCount = rows[0].Split(new[] { ' ', '\t', ',' }, StringSplitOptions.RemoveEmptyEntries).Length;


        Tilemap map = new Tilemap(tileset, columnCount, rowCount, tileset.Values.First().TileWidth, tileset.Values.First().TileHeight);
        //loop through the tilemap and set each tile..
        for (int i = 0; i < rowCount; i++) {
            for (int j = 0; j < columnCount; j++) {
                int index = i * columnCount + j;
                int tglobalID = GlobalIDs[index]; 
                TileReference t = GetTile(tglobalID, tileSetsByID);
                if (t != null) {
                    map.SetTile(index, t);
                }
            }
        }
        return map;
    }


    private static IEnumerable<int> ReadTileIds(XElement element)
    {
        //used for solid, and trigger tiles
        //creates a list when returning
        if (element == null)
            yield break;

        string[] values = element.Value.Split(new[] { ' ', '\t', '\r', '\n' },StringSplitOptions.RemoveEmptyEntries);

        foreach (string value in values)
        {
            int tileID = int.Parse(value);   
            yield return tileID;
        }

    }
}
