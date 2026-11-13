using GameBase.Dialogue;
using Gum.Forms.Controls;
using Gum.GueDeriving;
using Gum.Wireframe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTemplate.Game.UI
{
    public class DialogueUI
    {
        private Panel Root = new();
        SpriteRuntime backgroundPanel;
        RectangleRuntime dialoguePanel;
        SpriteRuntime spritePanel;
        RectangleRuntime namePanel;
        Label name;
        Label dialogue;
        public bool activeSelf { get; private set; } = false;

        public DialogueUI()
        {
            //init here
            Init();
        }
        //this shouldn't count as scene UI imo, any scene that has dialogue should contain one of these?

        public void Init()
        {
            Root.Dock(Dock.Fill);

            backgroundPanel = new SpriteRuntime();
            backgroundPanel.Dock(Dock.Fill);
            Root.AddChild(backgroundPanel);

            dialoguePanel = new RectangleRuntime();
            dialoguePanel.IsFilled = true;
            dialoguePanel.FillRed = 28;
            dialoguePanel.FillGreen = 32;
            dialoguePanel.FillBlue = 42;
            dialoguePanel.FillAlpha = 230;

            dialoguePanel.HeightUnits = Gum.DataTypes.DimensionUnitType.PercentageOfParent;
            dialoguePanel.Dock(Dock.Bottom);
            dialoguePanel.Height = 30;
            Root.AddChild(dialoguePanel);

            Panel dialogueBox = new Panel();
            dialoguePanel.AddChild(dialogueBox);
            dialogueBox.Dock(Dock.Fill);
            dialogueBox.Height = -16;
            dialogueBox.Width = -16;


            dialogue = new Label();
            dialogueBox.AddChild(dialogue);
            dialogue.Anchor(Anchor.TopLeft);
            dialogue.Visual.WidthUnits = Gum.DataTypes.DimensionUnitType.PercentageOfParent;
            dialogue.Width = 100;
            dialogue.Text = "Dialogue goes here";

            spritePanel = new SpriteRuntime();
            dialoguePanel.AddChild(spritePanel);
            spritePanel.Anchor(Anchor.TopRight);
            spritePanel.YOrigin = RenderingLibrary.Graphics.VerticalAlignment.Bottom;
            spritePanel.X = -16;
            spritePanel.Y = 0;
            spritePanel.WidthUnits = Gum.DataTypes.DimensionUnitType.MaintainFileAspectRatio;
            spritePanel.HeightUnits = Gum.DataTypes.DimensionUnitType.PercentageOfParent;
            spritePanel.Height = 80;
            spritePanel.Width = 100;


            namePanel = new RectangleRuntime();
            dialoguePanel.AddChild(namePanel);
            namePanel.IsFilled = true;
            namePanel.FillRed = 28;
            namePanel.FillGreen = 32;
            namePanel.FillBlue = 42;
            namePanel.Anchor(Anchor.TopLeft);
            namePanel.YOrigin = RenderingLibrary.Graphics.VerticalAlignment.Bottom;
            namePanel.Y = 5;
            namePanel.X = -16;
            namePanel.WidthUnits = Gum.DataTypes.DimensionUnitType.RelativeToChildren;
            namePanel.HeightUnits = Gum.DataTypes.DimensionUnitType.RelativeToChildren;
            namePanel.Width = 30;
            namePanel.Height = 10;

            name = new Label();
            namePanel.AddChild(name);

            name.Visual.WidthUnits = Gum.DataTypes.DimensionUnitType.RelativeToChildren;
            name.Visual.HeightUnits = Gum.DataTypes.DimensionUnitType.RelativeToChildren;
            name.Width = 0;
            name.Height = 0;

            name.Anchor(Anchor.Center);
            name.Text = "Name";


        }
        public void SetName(string n)
        {
            name.Text = n;
        }
        public void SetDialogue(string d)
        {
            dialogue.Text = d;
        }
        public void SetBackground(Sprite bg)
        {
            backgroundPanel.Texture = bg.Region.Texture;
        }
        public void ToggleBackgroundOn(bool state)
        {
            backgroundPanel.Visible = state;
        }

        public void SetSprite(Sprite s)
        {
            if (s == null) { spritePanel.Visible = false; }
            else
            {
                spritePanel.Texture = s.Region.Texture;
                //this only holds unanimated stuff rn work in laater?
                spritePanel.Visible = true;
            }
        }

        public void UpdateDialogueLine(DialogueLine d)
        {
            if (d.background == null)
            {
                ToggleBackgroundOn(false);
            }
            else
            {
                SetBackground(d.background);
                ToggleBackgroundOn(true);
            }
            SetDialogue(d.dialogue);
            SetSprite(d.sprite);
            SetName(d.Name);
        }
        public void SetActive(bool state)
        {
            if (state)
            {
                Root.AddToRoot();
                activeSelf = true;
            }
            else
            {
                Root.RemoveFromRoot();
                activeSelf = false;
            }
        }

        public void SetVisibleCharacters(int count)
        {
            TextRuntime text = (TextRuntime)dialogue.TextComponent;
            text.MaxLettersToShow = count;
        }
    }
}
