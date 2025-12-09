using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    // This will hold the instance of our music player
    private static BackgroundMusic instance;

    void Awake()
    {
        // If we don't have an instance yet, make this one the instance
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Make sure audio is playing
            AudioSource audio = GetComponent<AudioSource>();
            if (audio != null && !audio.isPlaying)
            {
                audio.Play();
            }
        }
        else
        {
            // If we already have an instance, destroy this one
            Destroy(gameObject);
        }
    }
}