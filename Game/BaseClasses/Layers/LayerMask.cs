using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase;

public static class LayerMask
{
    public static int GetMask(List<Layer> layers)
    {
        int mask = 0;
        foreach (var layer in layers)
        {
            mask |= 1 << (int)layer;
        }
        return mask;
    }
    public static bool IsInLayerMask(int layerMask, Layer layer)
    {
        return (layerMask & GetMask(layer)) != 0;
    }

    private static int GetMask(Layer layer)
    {
        return 1 << (int)layer;
    }
    public static string LayerToName(Layer layer)
    {
        return layer.ToString();
    }
    public static Layer NameToLayer(string name)
    {
        return Enum.Parse<Layer>(name);
        //parses through the enum Layer for name, and returns the int associated. 
    }
}
