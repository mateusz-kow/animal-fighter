using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 0.3f);
    }

    public void PlaySound()
    {
        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null)
        {
            audio.Play();
        }
    }
}