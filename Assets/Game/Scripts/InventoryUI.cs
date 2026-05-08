using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;
    public GameObject slotPrefab;
    public Inventory inventory;

    private List<InventorySlot> _slots = new List<InventorySlot>();

    void Start()
    {
        // Создаем ячейки заранее под макс. емкость
        for (int i = 0; i < inventory.capacity; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, itemsParent);
            _slots.Add(newSlot.GetComponent<InventorySlot>());
        }
        inventory.onInventoryChanged += RefreshUI;
        RefreshUI(); // Первый запуск
    }

    void RefreshUI()
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            if (i < inventory.items.Count)
            {
                _slots[i].gameObject.SetActive(true);
                bool isSelected = i == inventory.selectedSlotIndex;

                // Просто берем данные и количество из слота
                var currentItem = inventory.items[i];
                _slots[i].SetItem(currentItem.data, currentItem.count, isSelected);
            }
            else
            {
                _slots[i].Clear();
                _slots[i].gameObject.SetActive(false);
            }
        }
    }

}

