using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    public Inventory PlayerInventory { get; private set; }

    void Awake()
    {
        Instance = this;
        PlayerInventory = GetComponent<Inventory>();
    }
}
