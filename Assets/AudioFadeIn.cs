using System.Collections;
using UnityEngine;

public class AudioFadeIn : MonoBehaviour
{
    public static AudioFadeIn Instance; 

    public AudioSource audioSource;
    public float fadeInDuration = 2.0f;
    public float targetVolume = 1.0f; 

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else if (Instance != this)
        {
            Destroy(gameObject); 
            return; 
        }
    }

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Starts with volume at 0 and start the fade-in coroutine
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

        audioSource.volume = targetVol; 
    }
}
