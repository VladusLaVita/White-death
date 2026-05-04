using UnityEngine;

public class PickupItem : MonoBehaviour, IPickup
{
    public ItemData itemData;
    public interface IPickup
{
    void OnPickup();
}

    public void OnPickup()
    {
        Inventory inventory = FindFirstObjectByType<Inventory>();
        
        if (inventory != null && itemData != null)
        {
            inventory.AddItem(itemData);
        }
        else
        {
            Debug.LogWarning($"[PickupItem] Не удалось подобрать предмет: {(inventory == null ? "инвентарь не найден на сцене" : "itemData не назначен")}!", this);
        }
        
        Destroy(gameObject);
    }
}