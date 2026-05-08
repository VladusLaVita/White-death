using UnityEngine;

public class Item : Interactable
{
    public ItemData data;
    private Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        if (data != null)
            interactionName = data.itemName;
    }

    public override void Interact()
    {
        if (InventoryManager.Instance?.PlayerInventory.AddItem(data) == true)
        {
            Destroy(gameObject);
        }
    }

    // ←←← Главное изменение
    public override void Use()
    {
        // Этот метод вызывается только для предметов, которые находятся в мире 
        // (например, если можно использовать предмет не подбирая)
        Debug.Log($"Используем предмет в мире: {data.itemName}");
        // Можно вызвать UseItemFromInventory(data) через PlayerController
    }

    private void Strike(Animator anim)
    {
        if (data.isRanged)
        {
            Shoot();
        }
        else if (anim != null)
        {
            anim.SetTrigger("Strike");
        }
    }

    private void Eat(Animator anim)
    {
        // Можно добавить анимацию поедания
        if (anim != null)
            anim.SetTrigger("Eat");        // если у тебя есть такая

        Inventory inv = InventoryManager.Instance?.PlayerInventory;
        inv?.RemoveItem(data);

        // Если предмет был в мире — удаляем
        if (gameObject != null && gameObject.scene.IsValid())
            Destroy(gameObject);
    }

    private void Shoot()
    {
        Debug.Log($"Выстрел из {data.itemName}");
        // логика стрельбы...
    }
}