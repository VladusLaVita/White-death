using UnityEngine;

public class EntityController : MonoBehaviour
{
    public bool deadFlag = false;
    public RagdollController ragdollController;

     public void Die()
    {
        ragdollController.isRagdollActive=true;
    }

    
}
