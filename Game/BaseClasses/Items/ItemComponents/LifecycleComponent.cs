using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase;
/// <summary>
/// if things only appear temporarily and despawn after a period. Destroys upon lifecycle end?
/// </summary>
public class LifecycleComponent : Component, IUpdatable, IPersistent
{
    float lifecycle;
    public event Action LifecycleComplete;

    public bool pooledObject { get; }
    //this triggers upon completion. the base gameobject should handle the actual destruction, so like if it should be removed from scene, call remove object, or setactive false if it is pooled
    public LifecycleComponent(float lifecycle, GameObject owner, bool pooled) : base(owner)
    {
        this.lifecycle = lifecycle;
        pooledObject = pooled;

    }

    public void CaptureState(GameObjectSaveData data)
    {
        data.RemainingLifetime = lifecycle;
    }

    public override void Connect()
    {
        
    }

    public override void OnDestroy()
    {
        //owner.ownerScene.RemoveObject(owner);
        //commenting out immediate destruction to allow for more flexibilty, with pooled objects
    }

    public void RestoreState(GameObjectSaveData data)
    {
        lifecycle = (float)data.RemainingLifetime;
    }

    public void Update(GameTime gameTime)
    {
        if (!activeSelf) { return; }
        if (lifecycle >= 0)
        {
            lifecycle -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        else {
            LifecycleComplete?.Invoke();
            if (pooledObject)
            {
                owner.SetActive(false);
            }
            else {
                owner.ownerScene.RemoveObject(owner);
            }
        }
    }

}
