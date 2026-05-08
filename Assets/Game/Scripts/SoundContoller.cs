using UnityEngine;

public class FootstepSoundHandler : MonoBehaviour
{
    [Header("Настройки аудио")]
    public AudioSource audioSource;
    public AudioClip[] footstepClips;

    [Header("Вариативность")]
    public float minPitch = 0.9f;
    public float maxPitch = 1.1f;
    public float volume = 0.5f;

    public void Step()
    {
        if (footstepClips.Length > 0 && audioSource != null)
        {
            AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.PlayOneShot(clip, volume);
        }
    }
}
