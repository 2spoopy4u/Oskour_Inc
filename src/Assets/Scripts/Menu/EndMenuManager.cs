using UnityEngine;
using TMPro;

public class EndMenuManager : MonoBehaviour
{
    public GameObject modalPanel;

    public Player player;

    public static EndMenuManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    public void OpenModal()
    {
        modalPanel.SetActive(true);
        PausePlayer();
    }

    public void PausePlayer()
    {
        player.Pause();
    }
   
}