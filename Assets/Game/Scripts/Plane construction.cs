using UnityEngine;
using System.Collections.Generic;

public class PlaneConstructor : Interactable
{
    [Header("Ссылки на игрока")]
    [SerializeField] private Inventory playerInventory; // Ссылка на инвентарь игрока

    [Header("Настройки моделей")]
    [SerializeField] private GameObject fullPlaneModel; // Целая готовая модель
    [SerializeField] private List<GameObject> planeParts; // Все отдельные детали

    private Dictionary<string, GameObject> partsDictionary;
    private int assembledPartsCount = 0;

    void Start()
    {
        interactionName = "Собирать самолет";
        InitComponents();
    }

    private void InitComponents()
    {
        partsDictionary = new Dictionary<string, GameObject>();
        if (fullPlaneModel != null) fullPlaneModel.SetActive(false);

        foreach (GameObject part in planeParts)
        {
            if (part != null)
            {
                part.SetActive(false); // Прячем детали до сборки
                partsDictionary.Add(part.name, part);
            }
        }
    }

    // Переопределяем метод Interact из вашего базового класса
    public override void Interact()
    {
        if (playerInventory == null)
        {
            Debug.LogError("Не назначена ссылка на инвентарь игрока!");
            return;
        }

        // Получаем предмет, который сейчас выбран у игрока в руках
        ItemData activeItem = playerInventory.GetActiveItem();

        if (activeItem == null)
        {
            Debug.Log("В руках ничего нет!");
            return;
        }

        // Проверяем, совпадает ли имя предмета из ItemData с деталью самолета
        // Предполагается, что activeItem.itemName или activeItem.name соответствует имени GameObject детали
        string itemName = activeItem.name;

        if (partsDictionary.TryGetValue(itemName, out GameObject part))
        {
            if (!part.activeSelf)
            {
                // Деталь найдена и еще не установлена
                part.SetActive(true);
                assembledPartsCount++;

                // Удаляем 1 единицу детали из инвентаря игрока
                playerInventory.RemoveItem(activeItem, 1);

                CheckAssemblyCompletion();
            }
            else
            {
                Debug.Log("Эта деталь уже установлена на самолет!");
            }
        }
        else
        {
            Debug.Log($"Предмет '{itemName}' не является деталью этого самолета.");
        }
    }

    private void CheckAssemblyCompletion()
    {
        if (assembledPartsCount >= planeParts.Count)
        {
            CompleteAssembly();
        }
    }

    private void CompleteAssembly()
    {
        // Прячем все отдельные детали
        foreach (GameObject part in planeParts)
        {
            if (part != null) part.SetActive(false);
        }

        // Активируем финальную целую модель
        if (fullPlaneModel != null) fullPlaneModel.SetActive(true);

        // Меняем текст подсказки, так как собирать больше нечего
        interactionName = "Самолет собран";
        Debug.Log("Сборка самолета полностью завершена!");
    }
}