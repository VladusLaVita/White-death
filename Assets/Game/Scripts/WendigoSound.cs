using UnityEngine;

public class WendigoSound : MonoBehaviour
{
    public AudioClip wendigoCry;
    public AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public void Cry()
    {
        audioSource.PlayOneShot(wendigoCry);
    }
}
