using BaseTemplate.Game.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase.Dialogue;
/// <summary>
/// handles the showing of dialouge on the game screen
/// connects the UI to the text
/// </summary>
public class DialogueManager
{
    public static DialogueUI dialogueUI;
    private DialogueInfo dinfo;
    bool isTyping;
    string temptxt;
    float SecPerChar = 0.05f;
    float typingTimer;
    int visChars;
    readonly InputManager input;


    public DialogueManager(InputManager inputManager)
    {
        input = inputManager;
        dialogueUI = new();
        inputManager.ActionPressed += SkipDialogue;
        dialogueUI.SetActive(false);
    }
    public void DialogueSetup(DialogueInfo info)
    {
        dinfo = info;
        dinfo.ResetIndex();
        dialogueUI.SetActive(true);
        input.UpdateActionMap(InputActionMap.Dialogue);
        UpdateDialogue();

    }

    public void UpdateDialogue()
    {
        if (!dialogueUI.activeSelf || dinfo == null)
        {
            return;
        }
        temptxt = dinfo.GetDialogue();
        if (temptxt == null)
        {
            dialogueUI.SetActive(false);
            EventSystem.OnDialogueComplete(dinfo);
            input.UpdateActionMap(InputActionMap.Gameplay);
            return;
        }
        else
        {
            StartTypewriter(dinfo.GetDialogue());
            dialogueUI.UpdateDialogueLine(dinfo.GetDialogueLine());
        }
    }
    void StartTypewriter(string text)
    {
        temptxt = text ?? "";
        typingTimer = 0;
        visChars = 0;

        dialogueUI.SetDialogue(temptxt);
        dialogueUI.SetVisibleCharacters(0);

        isTyping = temptxt.Length > 0;
    }
    public void Update(GameTime gameTime)
    {
        if (!dialogueUI.activeSelf || !isTyping)
        {
            return;
        }

        typingTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        int previousCount = visChars;

        if (typingTimer >= SecPerChar && visChars < temptxt.Length)
        {
            typingTimer -= SecPerChar;
            visChars++;
        }

        if (visChars != previousCount)
            dialogueUI.SetVisibleCharacters(visChars);

        isTyping = visChars < temptxt.Length;
    }
    public void FinishTyping()
    {
        //end type early
        if (!isTyping)
            return;

        visChars = temptxt.Length;
        dialogueUI.SetVisibleCharacters(visChars);

        typingTimer = 0;
        isTyping = false;
    }
    public void SkipDialogue(GameAction action)
    {
        if (!dialogueUI.activeSelf || input.inputActionMap != InputActionMap.Dialogue)
        {
            return;
        }
        if (action == GameAction.ProgressDialogue)
        {
            if (isTyping)
            {
                FinishTyping();
            }
            else
            {
                dinfo.IncrementIndex();
                UpdateDialogue();
            }
        }
    }
}
