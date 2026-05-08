using UnityEngine;

public class EntityController : MonoBehaviour
{
    public bool deadFlag = false;

    void Update()
    {
        if (deadFlag == true)
        {
            Destroy(gameObject);
        }
    }
}
