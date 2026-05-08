using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string interactionName = "Объект"; // То, что увидит игрок в UI

    // Этот метод вызывается при нажатии на E (взаимодействие)
    public abstract void Interact();

    // Этот метод вызывается при нажатии на ЛКМ (использование в руках)
    public virtual void Use()
    {
        // По умолчанию ничего не делает, если не переопределено
    }
}
