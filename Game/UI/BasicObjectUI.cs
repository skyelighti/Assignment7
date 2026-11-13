using Gum.Forms.Controls;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase;

public class BasicObjectUI
{
    //owner reference: this deosnt have to be a gameobject: ex. if healthbar, use idamagable
    //Init UI properties here
    Panel UIBar;
    //this is where the ui will live inside, mini canvas
    public BasicObjectUI() {
        //pass an owner reference
        //subscribe to owner state
        //intialize UI here
        UIBar = new Panel();
    }
    public void SetActive(bool state) {
        if (state)
        {
            UIBar.AddToRoot();
        }
        else {
            UIBar.RemoveFromRoot();
        }
    }
    public void UpdateUIBar() { 
        //updaating functions
    }
    public void Destroy() {
        SetActive(false);
        //unsubscribe from owners here.
    }
    public void UpdatePosition(Vector2 objectPosition, Vector2 objectSize)
    {
        UIBar.X = objectPosition.X + (objectSize.X - UIBar.Width) * 0.5f;
        UIBar.Y = objectPosition.Y - UIBar.Height + 5f;
    }
    public void UpdatePosition(GameObject obj, Vector2 objectSize)
    {
        UIBar.X = obj.transform.position.X + (objectSize.X - UIBar.Width) * 0.5f;
        UIBar.Y = obj.transform.position.Y - UIBar.Height + 5f;
    }
}
