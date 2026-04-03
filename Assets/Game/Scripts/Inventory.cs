using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<RawImage> slots;
    // Создаем параллельный список или массив для хранения данных о предметах
    private Item[] itemsInSlots;

    public Transform player_model;
    public Transform player_weapon;
    public RawImage indicator;
    private int index = 0;

    void Awake()
    {
        // Инициализируем массив данных по размеру списка слотов
        itemsInSlots = new Item[slots.Count];
    }

    public bool AddItem(Item itemData)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].texture == null)
            {
                slots[i].texture = itemData.icon.texture;
                slots[i].color = Color.white;
                itemsInSlots[i] = itemData; // Запоминаем данные предмета!
                return true;
            }
        }
        HandleInput();

        return false;
    }

    void Update() => HandleInput();

    void HandleInput()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                index = i;
                UpdateIndicator();
                HandleItem(index);
            }
        }
    }

    void UpdateIndicator()
    {
        if (slots.Count > index && slots[index] != null)
            indicator.rectTransform.position = slots[index].rectTransform.position;
    }

    void HandleItem(int index)
    {
        Animator animator = player_model.GetComponent<Animator>();
        if (animator == null) return;

        // Проверяем, есть ли вообще предмет в этом слоте
        if (itemsInSlots[index] != null)
        {
            // Теперь берем itemName из данных Item, а не из картинки
            bool isWeapon = itemsInSlots[index].itemName == "Weapon";
            animator.SetBool("armed", isWeapon);

            player_weapon.gameObject.SetActive(isWeapon);
        }
        else
        {
            animator.SetBool("armed", false);
            player_weapon.gameObject.SetActive(false);

        }
    }
}
