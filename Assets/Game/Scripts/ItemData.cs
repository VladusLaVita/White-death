using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    [Header("General preferences")]
    public string itemName;
    [TextArea] public string description;
    public Sprite icon;
    public int price;

    [Header("Weapon preferences")]
    public ItemData ammo;
    public GameObject ammoPrefab; // Префаб пули
    public GameObject weaponModelPrefab; // Модель оружия, которая появится в руках
    public bool isStackable;
    public bool isWeapon;
    public bool isEdible;
    public bool isRanged;
    public bool ammoBox;
    public int ammoInside;

    [Header("Audio settings")]
    public AudioClip pickUpSound;
    public AudioClip useSound;
}
