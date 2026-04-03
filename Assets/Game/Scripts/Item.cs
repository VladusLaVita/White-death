using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName = "Предмет";
    public Sprite icon;          // Иконка для UI
    public GameObject prefab;    // Префаб, если нужно выбросить предмет
    public bool isStackable;     // Можно ли складывать в стак
}
