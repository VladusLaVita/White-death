using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class ItemInteraction : MonoBehaviour
{
    [Header("Настройка объектов")]
    public GameObject hint;
    public GameObject playerCamera;
    public LayerMask interactionLayer; // Рекомендуется: исключите слой игрока
    public Inventory playerInventory; // Ссылка на скрипт инвентаря


    [Header("Теги для интерактива")]
    // Добавлен тег Closet, иначе он не будет распознан
    public List<string> interactables = new List<string> { "Ammo", "Weapon", "Interactable", "Food", "Closet" };

    [Header("Настройка интерактива")]
    public float maxHitDistance = 3f;
    public Animator playerAnimator;
    public TextMeshProUGUI itemNameLabel;

    private GameObject interactButton;
    private GameObject pickUpButton;
    private GameObject visibleObject;

    private bool interactableFlag;
    private bool pickableFlag;
    private bool isInside; // Замена для корректной логики шкафа

    void Start()
    {
        interactButton = hint.transform.Find("InteractButton").gameObject;
        pickUpButton = hint.transform.Find("PickUpButton").gameObject;
        hint.SetActive(false);
    }

    void Update()
    {
        SearchForInteractables();
        
        if (Input.GetKeyDown(KeyCode.E)) {
            // Разделяем логику, чтобы не вызывать оба метода одновременно
            if (pickableFlag) PickUp();
            else if (interactableFlag) Interact();
        }
    }

    private void SearchForInteractables() {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxHitDistance, interactionLayer)) {
            GameObject hitObj = hit.transform.gameObject;

            if (interactables.Contains(hitObj.tag)) {
                visibleObject = hitObj;
                itemNameLabel.text = visibleObject.name;

                // Если это предмет для подбора (не Interactable и не Closet)
                bool isPickable = hitObj.tag != "Interactable" && hitObj.tag != "Closet";
                SwitchButtons(!isPickable, isPickable);
                return;
            }
        }
        
        // Сброс, если луч ничего не нашел
        if (hint.activeSelf) DisableHint();
    }

    private void SwitchButtons(bool interFlag, bool pickFlag) {
        if (!hint.activeSelf) hint.SetActive(true);
        interactButton.SetActive(interFlag);
        pickUpButton.SetActive(pickFlag);
        interactableFlag = interFlag;
        pickableFlag = pickFlag;
    }

    private void DisableHint() {
        hint.SetActive(false);
        interactableFlag = false;
        pickableFlag = false;
        itemNameLabel.text = "";
        visibleObject = null;
    }

    // Внутри ItemInteraction.cs
    private void PickUp()
    {
        if (visibleObject != null)
        {
            ItemData data = visibleObject.GetComponent<ItemData>();
            if (data != null)
            {
                // Имя метода должно СТРОГО совпадать: AddItem
                bool success = playerInventory.AddItem(data.item);
                if (success)
                {
                    GameObject.Destroy(visibleObject);
                }
            }
        }
    }



    private void Interact() {
        if (visibleObject != null) {
            // Используем CompareTag для оптимизации (быстрее чем ==)
            if (visibleObject.CompareTag("Closet")) {
                isInside = !isInside; // Переключаем состояние (войти/выйти)
                playerAnimator.SetBool("inside", isInside);
            }
        }
    }
}
