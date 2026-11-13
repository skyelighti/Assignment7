using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RenderingLibrary.Graphics;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Xml;
using System.Xml.Linq;


namespace GameBase.Assets;
/// <summary>
/// stores texture regions on a single sprite
/// important to create when creating a single sprite or needing to seperate a sprite out, but most of the time
/// will not be directly referenced in code
/// </summary>
public class TextureAtlas
{
    private Dictionary<string, TextureRegion> _regions;
    public IReadOnlyDictionary<string, TextureRegion> Regions => _regions;
    List<TextureRegion> _regionsByIndex = new();
    public int RegionCount => _regionsByIndex.Count;

    public Texture2D Texture { get; set; }
    //private Dictionary<string, Animation> _animations;
    bool[] AlphaMask;

    public TextureAtlas()
    {
        _regions = new Dictionary<string, TextureRegion>();
        //_animations = new Dictionary<string, Animation>();
    }

    public TextureAtlas(Texture2D texture)
    {
        Texture = texture;
        _regions = new Dictionary<string, TextureRegion>();
        //_animations = new Dictionary<string, Animation>();
        CheckAlpha();
    }

    public void AddRegion(string name, int x, int y, int width, int height)
    {
        TextureRegion region = new TextureRegion(Texture, x, y, width, height);
        region.alphaMask = GetAlphaMask(x, y, width, height);
        _regions.Add(name, region);
        _regionsByIndex.Add(region);
    }
    public TextureRegion GetTextureRegion(int index) {
        return _regionsByIndex[index];
    }
    public TextureRegion GetRegion(string name)
    {
        return _regions[name];
    }
    public int GetIndexOfRegion(string name) {
        if (!_regions.ContainsKey(name)) { return -1; }
        return _regionsByIndex.IndexOf(_regions[name]);
    }
    public bool RemoveRegion(string name)
    {
        if (!_regions.ContainsKey(name)) { return false; }
        TextureRegion region = _regions[name];
        _regionsByIndex.Remove(region);
        _regions.Remove(name);
        return true;
    }


    public void Clear()
    {
        _regions.Clear();
        _regionsByIndex.Clear();
    }
    public bool[] GetAlphaMask(int x, int y, int width, int height)
    {
        bool[] AlphaArea = new bool[width * height];
        Rectangle rect = new Rectangle(x, y, width, height);
        for (int i = 0; i < rect.Height; i++)
        {
            for (int j = 0; j < rect.Width; j++)
            {
                int sheetX = rect.X + j;
                int sheetY = rect.Y + i;

                int sheetIdx = sheetX + (sheetY * Texture.Width);
                int resultIdx = j + (i * rect.Width);
                AlphaArea[resultIdx] = AlphaMask[sheetIdx];

            }
            //if error occuers here its cuz trying to make area larger than texture
        }
        return AlphaArea;

    }

    public void CheckAlpha()
    {
        Color[] pixels = new Color[Texture.Width * Texture.Height];
        AlphaMask = new bool[Texture.Width * Texture.Height];
        //get data takes a 1d array
        Texture.GetData<Color>(pixels);
        for (int i = 0; i < pixels.Length; i++)
        {
            if (pixels[i].A == 0)
            {
                AlphaMask[i] = false;
                //is transparent
            }
            else
            {
                AlphaMask[i] = true;
            }
        }
    }

    public static TextureAtlas FromFile(ContentManager content, string fileName)
    {
        TextureAtlas atlas = new TextureAtlas();

        string filePath = Path.Combine(content.RootDirectory, fileName);

        using (Stream stream = TitleContainer.OpenStream(filePath))
        {
            using (XmlReader reader = XmlReader.Create(stream))
            {
                XDocument doc = XDocument.Load(reader);
                XElement root = doc.Root;

                // The <Texture> element contains the content path for the Texture2D to load.
                // So we will retrieve that value then use the content manager to load the texture.
                string texturePath = root.Element("Texture").Value;
                atlas.Texture = content.Load<Texture2D>(texturePath);
                atlas.CheckAlpha();

                // The <Regions> element contains individual <Region> elements, each one describing
                // a different texture region within the atlas.  
                //
                // Example:
                // <Regions>
                //      <Region name="spriteOne" x="0" y="0" width="32" height="32" />
                //      <Region name="spriteTwo" x="32" y="0" width="32" height="32" />
                // </Regions>
                //
                // So we retrieve all of the <Region> elements then loop through each one
                // and generate a new TextureRegion instance from it and add it to this atlas.
                var regions = root.Element("Regions")?.Elements("Region");

                if (regions != null)
                {
                    foreach (var region in regions)
                    {
                        string name = region.Attribute("name")?.Value;
                        int x = int.Parse(region.Attribute("x")?.Value ?? "0");
                        int y = int.Parse(region.Attribute("y")?.Value ?? "0");
                        int width = int.Parse(region.Attribute("width")?.Value ?? "0");
                        int height = int.Parse(region.Attribute("height")?.Value ?? "0");

                        if (!string.IsNullOrEmpty(name))
                        {
                            atlas.AddRegion(name, x, y, width, height);
                        }
                    }
                }

                return atlas;
            }
        }
    }

}
