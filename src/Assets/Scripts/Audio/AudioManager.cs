using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Clips")]
    public AudioClip backgroundMusic;
    private List<AudioClip> deathSFXClips = new();
    private void Awake()
    {
        StopMusic();
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadDeathSFX();
        }
        else
        {
            Destroy(gameObject); // Détruit les doublons
            return;
        }
    }

    public void LoadAndPlayMusic(string songName)
    {
        AudioClip clip = Resources.Load<AudioClip>("Sounds/" + songName);
        if (clip == null)
        {
            Debug.LogError("AudioManager: Music clip not found for song name: " + songName + ".mp3");
            return;
        }

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    private void LoadDeathSFX()
    {
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Sounds/Death");

        if (clips.Length == 0)
        {
            Debug.LogWarning("No death SFX found in Resources/Sounds/Death/");
        }

        deathSFXClips.AddRange(clips);
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip, volume);
        }
    }

    public void PlayRandomDeathSFX(float volume = 1f)
    {
        if (deathSFXClips.Count == 0) return;

        int index = Random.Range(0, deathSFXClips.Count);
        sfxSource.PlayOneShot(deathSFXClips[index], volume);
    }

    public void PlayMusic(AudioClip musicClip)
    {
        if (musicClip != null)
        {
            musicSource.clip = musicClip;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void RestartMusic()
    {
        if (musicSource.clip != null)
        {
            musicSource.Stop();
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        if (musicSource.clip != null)
        {
            musicSource.Stop();
        }
    }

    public void PauseMusic()
    {
        if (musicSource.clip != null)
        {
            musicSource.Pause();
        }
    }

    public void PlayMusic()
    {
        if (musicSource.clip != null)
        {
            musicSource.Play();
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }

    public float GetMusicVolume()
    {
        return musicSource != null ? musicSource.volume : 0f;
    }

    public float GetSFXVolume()
    {
        return sfxSource != null ? sfxSource.volume : 0f;
    }
}
