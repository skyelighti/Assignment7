using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase.Items;
/// <summary>
/// holds the defintion for what an inventory looks like within this game
/// </summary>
public class Inventory
{
    //current collection of items
    public List<ItemInfo> items { get; private set; } = new();
    // TODO: seperate currency out of inventory
    public int currencyCount { get; private set; } = 0;

    public Action OnInvUpdate;
    public Inventory()
    {
    }
    public void OnEnable()
    {
    }

    public ItemInfo GetItem(ItemInfo info)
    {
        return items.Contains(info) ? info : null;
    }

    public void AddItem(ItemInfo info)
    {
        if (info.GetProperty<CurrencyInfo>() != null) {
            currencyCount += info.GetProperty<CurrencyInfo>().amt;
            OnInvUpdate?.Invoke();
            return;
        }
        //this needs refactoring
        if (!items.Contains(info))
        {
            items.Add(info);
            OnInvUpdate?.Invoke();
        }
    }
    public void ResetInventory()
    {
        currencyCount = 0;
        items.Clear();
        OnInvUpdate?.Invoke();
    }
    public ItemInfo GetItem(string name)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].name == name)
            {
                return items[i];
            }
        }
        return null;
    }
    public void RemoveItem(ItemInfo info)
    {
        items.Remove(info);
        OnInvUpdate?.Invoke();
    }

    public void RestoreState(IEnumerable<string> itemIds, int currency)
    {
        items.Clear();

        foreach (string id in itemIds)
        {
            ItemInfo info = Services.Get<GameDefinitions>().GetItemFromDict(id);
            items.Add(info);
        }

        currencyCount = currency;
        OnInvUpdate?.Invoke();
    }
}
