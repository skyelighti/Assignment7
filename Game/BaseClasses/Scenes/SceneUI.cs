using Gum;
using Gum.Forms.Controls;
using Gum.Wireframe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase;

//each scene should hold a reference to it's own UI, and handle loading and unloading
public abstract class SceneUI
{
    protected Panel Root { get; } = new Panel();
    //holds all ui inside the scene, like canvas per scene
    protected Scene scene;
    //it should hold a reference to the scene its drawing ontop of? 
    public SceneUI(Scene s)
    {
        scene = s;
        Root.Dock(Dock.Fill);
        //fills the whole screen w the root panel, so this scene can draw anywhere within the whole scren
        //init UI
        Init();

    }
    protected abstract void Init();
    //init/constructs all scene UI
    public void SetActive(bool state)
    {
        if (state)
        {
            Root.AddToRoot();
        }
        else
        {
            //this should show all the UI objects
            //this should is for scene loading
            Root.RemoveFromRoot();
            //attaches to gum global root, which should show?
        }
    }

    public void ResetUI()
    {
        foreach (var child in Root.Visual.Children.ToArray())
        {
            //doesnt fcuk up arrau? 
            child.Parent = null;
        }

        Init();
    }
}

