using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private InventorySlot[] slots;
    [SerializeField] private int selectedIndex = 0;

    private void Start()
    {
        UpdateSelection();
    }

    private void Update()
    {
        HandleScroll();
    }

    public void AddItem(ItemData item)
    {
        if (item.isStackable)
        {
            foreach (var slot in slots)
            {
                if (slot.CanAdd(item))
                {
                    slot.Add(1);
                    return;
                }
            }
        }

        foreach (var slot in slots)
        {
            if (slot.IsEmpty)
            {
                slot.SetItem(item, 1);
                return;
            }
        }

        Debug.Log("Inventory full");
    }

    private void HandleScroll()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f)
            selectedIndex = (selectedIndex + 1) % slots.Length;
        else if (scroll < 0f)
            selectedIndex = (selectedIndex - 1 + slots.Length) % slots.Length;

        UpdateSelection();
    }

    private void UpdateSelection()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].transform.localScale = (i == selectedIndex) ? Vector3.one * 1.2f : Vector3.one;
        }
    }

    public ItemData GetSelectedItem()
    {
        return slots[selectedIndex].GetItem();
    }
}