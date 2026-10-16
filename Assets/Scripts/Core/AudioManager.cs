using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private int nextHitSource;

    [SerializeField] private AudioSource sfxSource;
    [SerializeField, Range(0f, 0.5f)]
    private float pitchVariance = 0.1f;
    
    private float lastPickupsoundTime;
    private const float PickupSoundInterval = 0.08f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null)
            return;
        
        sfxSource.pitch = Random.Range(
            1f - pitchVariance,
            1f + pitchVariance
        );
        
        sfxSource.PlayOneShot(clip, volume);
    }

    public void PlayPickup(AudioClip clip)
    {
        if (Time.time - lastPickupsoundTime < PickupSoundInterval)
            return;
        
        lastPickupsoundTime = Time.time;
        
        PlaySFX(clip);
    }
}
