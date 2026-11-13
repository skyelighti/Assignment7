using System;
using GameBase.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase;

public class PlayerMovementComponent : MovementComponent
{
    InputManager input;
    PlayerLocatorService playerLocator;

    public PlayerMovementComponent(float speed, GameObject o) : base(speed, o)
    {
    }


    public void OnKeyDown(GameAction action)
    {
        //i want this to happen evey update
        switch (action)
        {
            case (GameAction.MoveRight):
                right = true;
                break;
            case (GameAction.MoveLeft):
                left = true;
                break;
            case (GameAction.MoveUp):
                up = true;
                break;
            case (GameAction.MoveDown):
                down = true;
                break;
        }
    }

    public void OnKeyReleased(GameAction action)
    {
        switch (action)
        {
            case (GameAction.MoveRight):
                right = false;
                break;
            case (GameAction.MoveLeft):
                left = false;
                break;
            case (GameAction.MoveUp):
                up = false;
                break;
            case (GameAction.MoveDown):
                down = false;  
                break;
        }
    }

    public override void OnDestroy() {
        if (input != null)
        {
            input.ActionKeyDown -= OnKeyDown;
            input.ActionReleased -= OnKeyReleased;
        }

        if (playerLocator != null)
        {
            OnPositionMoved -= playerLocator.UpdatePlayerPosition;
            playerLocator.ClearPlayer();
        }
    }

    public override void Connect()
    {
        // wiring belongs here, not in the constructor: Connect runs via GameObject.Init
        // once the object is fully assembled. cache every service lookup.
        input = Services.Get<InputManager>();
        input.ActionKeyDown += OnKeyDown;
        input.ActionReleased += OnKeyReleased;

        // the locator listens to us; we never hand out a reference to ourselves.
        playerLocator = Services.Get<PlayerLocatorService>();
        OnPositionMoved += playerLocator.UpdatePlayerPosition;
        playerLocator.UpdatePlayerPosition(owner.transform.position);
    }
}
