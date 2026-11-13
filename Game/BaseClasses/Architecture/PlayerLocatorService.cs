using Microsoft.Xna.Framework;

namespace GameBase;

/// <summary>
/// Middleman service that lets anything read the player's position with ZERO
/// references to the player object. Deliberately has: no GameObject field, no
/// Update loop. The player pushes positions in (via an event); consumers pull.
/// Registered at startup — before any player exists — so consumers work even
/// when there is no player (they just read Vector2.Zero / HasPlayer == false).
/// </summary>
public class PlayerLocatorService
{
    public bool HasPlayer { get; private set; }

    Vector2 position;

    public Vector2 GetPlayerPosition()
    {
        return position;
    }

    public void UpdatePlayerPosition(Vector2 pos)
    {
        position = pos;
        HasPlayer = true;
    }

    /// <summary> call when the player is destroyed / leaves the scene </summary>
    public void ClearPlayer()
    {
        HasPlayer = false;
        position = Vector2.Zero;
    }
}
