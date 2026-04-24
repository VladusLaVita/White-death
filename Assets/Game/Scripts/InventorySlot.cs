using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Text amountText;

    private ItemData item;
    private int amount;

    public bool IsEmpty => item == null;

    public void Start()
    {
        icon.enabled = false;
        amountText.enabled = false;
    }
    public void SetItem(ItemData newItem, int count)
    {
        item = newItem;
        amount = count;

        icon.sprite = item.icon;
        icon.enabled = true;

        UpdateAmount();
    }

    public void Clear()
    {
        item = null;
        amount = 0;

        icon.enabled = false;
        amountText.text = "";
    }

    public bool CanAdd(ItemData newItem)
    {
        return item != null && item == newItem && item.isStackable && amount < item.maxStack;
    }

    public void Add(int count)
    {
        amount += count;
        UpdateAmount();
    }

    private void UpdateAmount()
    {
        amountText.text = item.isStackable ? amount.ToString() : "";
    }

    public ItemData GetItem() => item;
}