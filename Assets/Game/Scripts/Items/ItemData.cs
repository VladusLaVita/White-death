using UnityEngine;

public class ItemData : MonoBehaviour
{
    [Header("Данные предмета")]
    public Item item;

    [Tooltip("Количество (например, для патронов)")]
    public int amount = 1;
}
