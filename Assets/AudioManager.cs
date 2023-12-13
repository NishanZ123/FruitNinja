using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioClip bassDropClip;
    public AudioClip swordClip;
    public AudioClip clickClip;
    private AudioSource audioSource;

    public float bassDropVolume = 0.2f; // Adjust these values as needed
    public float swordVolume = 0.2f;
    public float clickVolume = 0.2f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }


    public void PlayBassDrop()
    {
        audioSource.PlayOneShot(bassDropClip, bassDropVolume);
    }

    public void PlaySwordSound()
    {
        audioSource.PlayOneShot(swordClip, swordVolume);
    }

    public void PlayClickSound()
    {
        if (audioSource == null)
        {
            Debug.LogError("AudioSource is null in AudioManager");
            return;
        }
        if (clickClip == null)
        {
            Debug.LogError("ClickClip is null in AudioManager");
            return;
        }
        audioSource.PlayOneShot(clickClip, clickVolume);
    }

}

