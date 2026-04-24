using UnityEngine;
using TMPro; // Если используешь TextMeshPro
using UnityEngine.UI;

public class InteractionRaycast : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float distance = 3f;
    [SerializeField] private LayerMask interactableLayer;

    [Header("UI Elements")]
    [SerializeField] private GameObject hintPanel;
    [SerializeField] private TextMeshProUGUI itemNameText;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance, interactableLayer))
        {
            if (hit.collider.CompareTag("Item"))
            {
                PickupItem item = hit.collider.GetComponent<PickupItem>();
                if (item != null)
                {
                    ShowHint(item.itemData.itemName);
                    return;
                }
            }
        }

        HideHint();
    }

    private void ShowHint(string name)
    {
        hintPanel.SetActive(true);
        itemNameText.text = name;
    }

    private void HideHint()
    {
        hintPanel.SetActive(false);
    }
}
