using UnityEngine;
using static ItemData;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Inventory inventory;

    private float nextFireTime;

    void Update()
    {
        HandleShooting();
    }

    private void HandleShooting()
    {
        ItemData item = inventory.GetSelectedItem();

        if (item == null || item.itemType != ItemData.ItemType.Weapon)
        {
            return;
        }

        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Shoot(item);
            nextFireTime = Time.time + item.fireRate;
        }
    }

    private void Shoot(ItemData weapon)
    {
        if (weapon.projectilePrefab == null) return;

        // 1. Получаем направление взгляда камеры
        Quaternion shootRotation = Camera.main.transform.rotation;

        // 2. Создаем стрелу с поворотом камеры
        Instantiate(weapon.projectilePrefab, shootPoint.position, shootRotation);
    }

}