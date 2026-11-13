using System;
using System.Collections.Generic;
using GameBase.Input;
using GameBase.Assets;
using GameBase.Dialogue;

namespace GameBase;

/// <summary>
/// Service locator: a registry holding one instance per type (same object-resolver
/// pattern as GameObject.GetComponent, but global).
/// Contract: call Get/TryGet ONCE (in a constructor or Connect()) and cache the
/// reference — never per frame.
/// - Get<T>() throws loudly if nothing is registered: a missing required service is a
///   setup bug and should fail fast.
/// - TryGet<T>() is for genuinely optional services (e.g. Dialogue when disabled).
/// </summary>
public static class Services
{
    static readonly Dictionary<Type, object> services = new();

    public static void Register<T>(T service) where T : class
    {
        if (service == null) throw new ArgumentNullException(nameof(service));
        services[typeof(T)] = service;
    }

    public static void Unregister<T>() where T : class
    {
        services.Remove(typeof(T));
    }

    public static T Get<T>() where T : class
    {
        if (services.TryGetValue(typeof(T), out object s))
        {
            return (T)s;
        }
        throw new InvalidOperationException(
            $"No service of type {typeof(T).Name} is registered. Did you forget to Register it in Services.Initialize?");
    }

    public static bool TryGet<T>(out T service) where T : class
    {
        if (services.TryGetValue(typeof(T), out object s))
        {
            service = (T)s;
            return true;
        }
        service = null;
        return false;
    }

    /// <summary> wipes every registration. for full game resets / tests. </summary>
    public static void Clear()
    {
        services.Clear();
    }

    public static void Initialize(SpriteLibrary sprites, GameDefinitions gameD, bool enableItems, bool enableDialogue)
    {
        var input = new InputManager();
        Register(input);
        Register(sprites);
        Register(new PersistenceManager());
        Register(gameD);
        Register(new PlayerLocatorService());

        if (enableItems)
        {
            var inventory = new Inventory();
            var inventoryUI = new InventoryUI(inventory, input);
            Register(inventory);
            Register(inventoryUI);
            input.ActionPressed += inventoryUI.InventoryToggle;
            gameD.InitItemList();
            inventory.OnEnable();
        }
        if (enableDialogue)
        {
            Register(new DialogueManager(input));
            gameD.InitDialogueLines();
            gameD.InitNPCDefinitions();
        }
    }
}
