
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GameBase.Dialogue;
/// <summary>
/// storing a bunch of small dialouge lines, into a section of dialouge
/// mainly used in loading dialouge from a file
/// </summary>
public class DialogueInfo
{
    // the specific character that owns this interaction, who do you need to interact with to have this section play?
    //doesnt always require a character, as items and inspecting the enviroment can also allow for this
    public Character? character;

    //the lines of dialouge within this single section
    List<DialogueLine>? dialogue = new();

    //instance ID- eventaully switch towards GUIDs
    public string ID;

    //do we move on to the next portion of dialouge once this section is finished? ex or do we wait for an external trigger to continue the dialouge
    public bool advancesProgress = false;

    //index of lines within this specfic sectiono of dialouge
    int indx = 0;

    public DialogueInfo(List<DialogueLine> d, string id, bool advanceafter, Character? c = null)
    {
        dialogue = d;
        ID = id;
        advancesProgress = advanceafter;
        character = c;

    }
    public DialogueLine GetDialogueLine()
    {
        return dialogue[indx];
    }
    public string GetDialogue()
    {
        string dia = indx < dialogue.Count ? dialogue[indx].dialogue : null;
        return dia;
    }
    //each who says each piece of dialouge and the line
    public string GetName()
    {
        string name = indx < dialogue.Count ? dialogue[indx].Name : null;
        return name;
    }
    //is there an icon for the current person talking?
    public Sprite GetIcon()
    {
        Sprite icon = indx < dialogue.Count ? dialogue[indx].sprite : null;
        return icon;
    }
    //does a line have a specific 2d background we want to render while this line is playing?
    public Sprite GetBackground()
    {
        Sprite bg = indx < dialogue.Count ? dialogue[indx].background : null;
        return bg;
    }
    public void IncrementIndex()
    {
        indx++;
    }
    public void ResetIndex()
    {
        indx = 0;
    }

    /// <summary>
    /// loads the dialouge from an XML file - shift towards jsons eventually
    /// </summary>
    /// <param name="content"></param>
    /// <param name="filename"></param>
    /// <returns></returns>
    public static List<DialogueInfo> FromFile(ContentManager content, string filename)
    {
        List<DialogueInfo> dialogueDefs = new();
        string filePath = Path.Combine(content.RootDirectory, filename);
        using (Stream stream = TitleContainer.OpenStream(filePath))
        {
            XDocument document = XDocument.Load(stream);
            XElement root = document.Root;
            foreach (XElement dInfoElement in root.Elements("DialogueInfo"))
            {
                string character = (string)dInfoElement.Element("Character");
                string a = (string)dInfoElement.Element("Advance");
                string ID = (string)dInfoElement.Element("ID");
                List<DialogueLine> dialogueLine = new();
                foreach (XElement dLine in dInfoElement.Element("DialogueLines").Elements("Line"))
                {
                    string n = (string)dLine.Element("Name");
                    string d = (string)dLine.Element("Dialogue");
                    string s = (string)dLine.Element("Sprite");
                    string b = (string)dLine.Element("Background");

                    Sprite sp = string.IsNullOrWhiteSpace(s) ? null : Services.Get<SpriteLibrary>().CreateSprite(s);
                    Sprite bg = string.IsNullOrWhiteSpace(b) ? null : Services.Get<SpriteLibrary>().CreateSprite(b);
                    dialogueLine.Add(new DialogueLine(n, d, sp, bg));
                }
                //checks if there is a character, if this isnt use null, otherwise gets the character
                Character? c = string.IsNullOrWhiteSpace(character) ? null : Enum.Parse<Character>(character.Trim());
                bool advance = !string.IsNullOrWhiteSpace(a) && bool.Parse(a);
                dialogueDefs.Add(new DialogueInfo(dialogueLine, ID, advance, c));
            }
        }

        return dialogueDefs;
    }
}
