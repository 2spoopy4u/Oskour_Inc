using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject modalPanel;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    public Player player;

    private void Start()
    {
        sfxSlider.value = 0.2f;
        musicSlider.value = 0.005f;

        // Initialise les valeurs des sliders depuis le volume actuel
        AudioManager.Instance.SetSFXVolume(sfxSlider.value);
        AudioManager.Instance.SetMusicVolume(musicSlider.value);

        musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenModal();
        }
    }
    public void OpenModal()
    {
        AudioManager.Instance.PauseMusic();
        modalPanel.SetActive(true);
        PausePlayer();
    }

    public void CloseModal()
    {
        AudioManager.Instance.PlayMusic();
        modalPanel.SetActive(false);
        UnpausePlayer();
    }

    public void PausePlayer()
    {
        player.Pause();
    }
    public void UnpausePlayer()
    {
        player.UnPause();
    }

    private void OnMusicVolumeChanged(float volume)
    {
        AudioManager.Instance.SetMusicVolume(volume);
    }

    private void OnSFXVolumeChanged(float volume)
    {
        AudioManager.Instance.SetSFXVolume(volume);
    }
}