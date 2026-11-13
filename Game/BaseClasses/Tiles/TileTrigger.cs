using FlatRedBall.Glue.StateInterpolation;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase;

public class TileTrigger: IObject
{
    Transform transform;
    public Hitbox hitbox;
    public event Action trigger;
    public bool OnlyActivateOnce;
    public bool entered;
    public bool activeSelf { get; private set; } = true;
    private Func<Rectangle, Rectangle, bool> activationCheck;
    bool activatedThisVisit = false;

    public void SetActive(bool state)
    {
        if (state == activeSelf) { return; }
        activeSelf = state;
        hitbox.SetActive(state);
    }
    public TileTrigger(Vector2 location, Vector2 scale, Action action, Func<Rectangle, Rectangle, bool> actiCheck) {
        transform = new Transform(new Vector2(location.X * scale.X, location.Y * scale.Y), scale);
        //converts tilemap location to world location
        hitbox = new Hitbox(transform);
        hitbox.SetTrigger(true);
        hitbox.CollisionEnter += OnCollisionEnter;
        hitbox.CollisionStay += OnCollisionStay;
        hitbox.CollisionExit += OnCollisionExit;
        trigger += action;
        activationCheck = actiCheck;
    }

    public void OnDestroy()
    {
        hitbox.CollisionEnter -= OnCollisionEnter;
        hitbox.CollisionStay -= OnCollisionStay;
        hitbox.CollisionExit -= OnCollisionExit;
    }
    public void OnCollisionEnter(ICollidable col) {
        if (!activeSelf) { return; }
        if (col.layer != Layer.Player) { return; }
        entered = true;
        ActivateTrigger(col);
    }

    public void OnCollisionStay(ICollidable col) {
        if (!activeSelf) { return; }
        if (col.layer != Layer.Player) { return; }
        ActivateTrigger(col);
    }

    public void OnCollisionExit(ICollidable col) {
        if (!activeSelf) { return; }
        if (col.layer != Layer.Player) { return; }
        entered = false;
        activatedThisVisit = false;

    }
    public void ActivateTrigger(ICollidable col) {
        if (!activeSelf || activatedThisVisit) { return; }
        //if the conditions are met
        if (activationCheck != null) {
            if (!activationCheck(col.BoxCollider, hitbox.BoxCollider)) { return; }
        }

        activatedThisVisit = true;
        if (OnlyActivateOnce) {
            SetActive(false);
        }
        trigger?.Invoke();
    }
}
