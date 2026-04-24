using NUnit.Framework.Interfaces;
using UnityEngine;

public class PickupItem : MonoBehaviour, IPickup
{
    public ItemData itemData;

    public void OnPickup()
    {
        FindFirstObjectByType<Inventory>().AddItem(itemData);
        Destroy(gameObject);
    }
}

