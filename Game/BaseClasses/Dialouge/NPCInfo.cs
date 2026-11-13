
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GameBase;
/// <summary>
/// holds all the information for each npc
/// this is mainly for loading in npcs
/// </summary>
public class NPCInfo
{
    public Character character;
    public string spriteName { get; }
    public string sheetName { get; }
    public string animationName { get; }
    public List<string> dialogueIDOrder = new();



    public NPCInfo(Character c, List<string> order, string spriteName = null, string sheetName = null, string animationName = null)
    {
        character = c;
        this.spriteName = spriteName;
        this.sheetName = sheetName;
        this.animationName = animationName;
        dialogueIDOrder = order;
    }

    public static List<NPCInfo> FromFile(ContentManager content, string filename)
    {
        List<NPCInfo> npcDefs = new();
        string filePath = Path.Combine(content.RootDirectory, filename);
        using (Stream stream = TitleContainer.OpenStream(filePath))
        {
            XDocument document = XDocument.Load(stream);
            XElement root = document.Root;
            foreach (XElement npcElement in root.Elements("NPC"))
            {
                string c = (string)npcElement.Element("Character");
                Character ch = (Character)Enum.Parse(typeof(Character), c);
                string spriteName = ((string)npcElement.Element("Sprite"))?.Trim();
                string sheetName = ((string)npcElement.Element("SpriteSheet"))?.Trim();
                string animationName = ((string)npcElement.Element("Animation"))?.Trim();
                List<string> idList = new();
                if (npcElement.Element("DialogueIDs") is XElement dialogueIDs)
                {
                    foreach (XElement ID in dialogueIDs.Elements("ID"))
                    {
                        idList.Add(((string)ID).Trim());
                    }
                }

                npcDefs.Add(new NPCInfo(ch, idList, spriteName, sheetName, animationName));
            }
        }

        return npcDefs;
    }
}
