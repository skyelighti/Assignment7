using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GameBase.Tiles;

public class LevelData
{
    public Tilemap Map { get; }
    public List<SpawnInfo> spawns { get; }

    public LevelData(Tilemap map, IEnumerable<SpawnInfo> spawn)
    {
        Map = map;
        spawns = new List<SpawnInfo>(spawn);
    }
}
