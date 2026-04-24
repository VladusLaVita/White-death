using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public enum ItemType
    {
        Default,
        Weapon
    }

    public ItemType itemType;
    public string itemName;
    public Sprite icon;
    public bool isStackable;
    public int maxStack = 99;

    [Header("Weapon")]
    public GameObject projectilePrefab;
    public float fireRate = 0.3f;
}

