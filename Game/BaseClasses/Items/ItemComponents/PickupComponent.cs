namespace GameBase.Items;

/// <summary>Collects the item when the player enters the pickup trigger.</summary>
public class PickupComponent : Component
{
    HitboxComponent hitbox;
    ItemInfo info;
    Inventory inventory;

    public PickupComponent(GameObject owner, HitboxComponent hitbox, ItemInfo info) : base(owner)
    {
        this.hitbox = hitbox;
        this.info = info;
    }

    public override void Connect()
    {
        inventory = Services.Get<Inventory>();
        hitbox.CollisionEnter += OnCollisionEnter;
    }

    public override void OnDestroy()
    {
        hitbox.CollisionEnter -= OnCollisionEnter;
    }

    private void OnCollisionEnter(ICollidable other)
    {
        if (!activeSelf || other.layer != Layer.Player)
            return;

        inventory.AddItem(info);
        owner.ActiveOnSceneEnter = false;
        owner.SetActive(false);

        var instanceId = owner.GetComponent<PersistenceComponent>()?.InstanceId;
        EventSystem.OnPickup(info, instanceId);
    }
}
