using System.Collections;
using UnityEngine;

public class AudioFadeIn : MonoBehaviour
{
    public static AudioFadeIn Instance; // Singleton instance

    public AudioSource audioSource;
    public float fadeInDuration = 2.0f; // Duration in seconds for the fade-in effect
    public float targetVolume = 1.0f; // Target volume after fade-in

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep the GameObject across scenes
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // Destroy this instance because one already exists
            return; // Exit to prevent further execution
        }
    }

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Start with volume at 0 and start the fade-in coroutine
        audioSource.volume = 0;
        audioSource.Play();
        StartCoroutine(FadeIn(audioSource, fadeInDuration, targetVolume));
    }

    IEnumerator FadeIn(AudioSource audioSource, float duration, float targetVol)
    {
        float currentTime = 0;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVolume = Mathf.Lerp(0, targetVol, currentTime / duration);
            audioSource.volume = newVolume;
            yield return null;
        }

        audioSource.volume = targetVol; // Ensure the volume is set to the target value at the end
    }
}
