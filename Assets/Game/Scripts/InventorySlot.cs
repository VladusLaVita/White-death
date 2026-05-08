using TMPro;
using UnityEngine;
using UnityEngine.UI; // Для RawImage
using UnityEngine.UIElements;

public class InventorySlot : MonoBehaviour
{
    public RawImage icon;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI countText;
    public UnityEngine.UI.Image background; // Перетащи сюда Image фона ячейки
    public Color selectedColor = Color.yellow;
    public Color defaultColor = Color.white;

    public void SetItem(ItemData data, int count, bool isSelected)
    {
        if (data == null) { Clear(); return; }

        // 1. Иконка
        if (data.icon != null)
        {
            icon.texture = data.icon.texture;
            icon.enabled = true;
        }

        // 2. Текст названия
        if (itemNameText != null) itemNameText.text = data.itemName;

        // 3. Логика отображения цифр
        if (countText != null)
        {
            // Если это коробка патронов, показываем сколько в ней пуль
            if (data.isRanged && data.ammo != null)
            {
                // Здесь можно выводить запас (например, для оружия)
                countText.text = "";
            }
            else if (data.isStackable)
            {
                // Показываем количество предметов в стаке (патроны, еда)
                countText.text = count > 1 ? count.ToString() : "";
            }
            else
            {
                countText.text = "";
            }

            countText.enabled = !string.IsNullOrEmpty(countText.text);
        }

        if (background != null) background.color = isSelected ? selectedColor : defaultColor;
    }

    public void Clear()
    {
        if (icon != null) icon.enabled = false;
        if (itemNameText != null) itemNameText.text = "";
        if (countText != null) countText.enabled = false; // Скрываем счетчик
        if (background != null) background.color = defaultColor;
    }
}
