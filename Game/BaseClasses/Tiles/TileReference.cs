using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase.Tiles;

public class TileReference
{
    public string TilesetName { get; init; }
    public int TileID { get; init; }
    public TileReference(string name, int ID) {
        TilesetName = name;
        TileID = ID;
    }
}

