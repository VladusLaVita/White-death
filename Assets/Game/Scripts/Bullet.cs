using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 100f;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Пуля попала в: {collision.gameObject.name}");

        Health hp = collision.gameObject.GetComponentInParent<Health>();
        Debug.Log($"Health найден: {hp != null}");

        if (hp != null)
        {
            Debug.Log($"Health на объекте: {hp.gameObject.name}, тег: {hp.gameObject.tag}");
            Vector3 impactPoint = collision.contacts[0].point;
            hp.GetDamage(damage, transform.forward, impactPoint);
            Destroy(gameObject);
        }
    }
}