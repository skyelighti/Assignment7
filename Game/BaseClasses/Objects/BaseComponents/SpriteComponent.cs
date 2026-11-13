using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase;

public class SpriteComponent : Component, IUpdatable, IDrawable
{
    public bool spriteVisible = true;
    float invisibilityTimer = 0f;
    public Sprite sprite { get; private set; }
    public SpriteComponent(string spriteName, GameObject o) : base(o)
    {
        sprite = Services.Get<SpriteLibrary>().CreateSprite(spriteName);
        UpdateSize((int)owner.transform.scale.X,(int)owner.transform.scale.Y);
    }
    public SpriteComponent(Sprite sprite, GameObject o) : base(o)
    {
        this.sprite = sprite;
        UpdateSize((int)owner.transform.scale.X, (int)owner.transform.scale.Y);
    }
    public void HideFor(float seconds)
    {
        invisibilityTimer = seconds;
        SetSpriteVisbility(false);
    }
    public bool SetSpriteVisbility(bool state) {
        spriteVisible = state;
        return state;
    }
    public void Update(GameTime gameTime)
    {
        if (!spriteVisible)
        {
            invisibilityTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (invisibilityTimer <= 0)
            {
                spriteVisible = true;
            }
        }
        if (!activeSelf) { return; }
        sprite.Update(gameTime);
    }
    public void Draw(SpriteBatch spriteBatch) {
        if (activeSelf && spriteVisible)
        {
            sprite.transform.rotation = owner.transform.rotation;
            sprite.Draw(spriteBatch, owner.transform.position);
        }
    }
    public void UpdateSprite(Sprite newSprite)
    {
        //updates gameobject sprite
        sprite = newSprite;
        UpdateSize((int)owner.transform.scale.X, (int)owner.transform.scale.Y);
        //sets to the same pixel value as the og sprite

    }
    public void UpdateSize(int x, int y) {
        sprite.transform.scale.X = (float)x / sprite.Region.Width;
        sprite.transform.scale.Y = (float)y / sprite.Region.Height;
    }

    public override void OnDestroy()
    {
    }

    public override void Connect()
    {
    }
}