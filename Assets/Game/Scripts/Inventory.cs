using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public ItemData data;
    public int count;
    public InventoryItem(ItemData data, int count) { this.data = data; this.count = count; }
}

public class Inventory : MonoBehaviour
{
    public int capacity = 10;
    public List<InventoryItem> items = new List<InventoryItem>();
    public int selectedSlotIndex = 0;

    public delegate void OnInventoryChanged();
    public OnInventoryChanged onInventoryChanged;

    public bool AddItem(ItemData data, int amount = 1)
    {
        if (data.ammoBox) amount = data.ammoInside;

        if (data.isStackable)
        {
            InventoryItem existing = items.Find(i => i.data == data);
            if (existing != null)
            {
                existing.count += amount;
                onInventoryChanged?.Invoke();
                return true;
            }
        }

        if (items.Count >= capacity) return false;

        items.Add(new InventoryItem(data, amount));
        onInventoryChanged?.Invoke(); // Это заставит PlayerController вызвать UpdateWeaponModel
        return true;
    }

    public ItemData GetActiveItem()
    {
        if (items.Count > 0 && selectedSlotIndex < items.Count)
            return items[selectedSlotIndex].data;
        return null;
    }

    public bool RemoveItem(ItemData data, int amount = 1)
    {
        InventoryItem item = items.Find(i => i.data == data);
        if (item == null) return false;

        item.count -= amount;
        if (item.count <= 0) items.Remove(item);

        onInventoryChanged?.Invoke();
        return true;
    }
}
