using UnityEngine;
using TMPro;

public class SaveModalManager : MonoBehaviour
{
    public GameObject modalPanel;
    public TMP_InputField nameInput;
    private GameLevelSerializer levelSerializer;

    public void Start()
    {
        levelSerializer = gameObject.AddComponent<GameLevelSerializer>();
    }
    public void OpenModal() 
    {
        modalPanel.SetActive(true);
    }

    public void CloseModal()
    {
        modalPanel.SetActive(false);
    }

    public void OnValidate()
    {
        string levelName = nameInput.text.Trim();
        if (!string.IsNullOrEmpty(levelName))
        {
            CloseModal();
            levelSerializer.levelPath = $"Assets/Resources/Levels/{levelName}.json";
            levelSerializer.levelName = levelName;
            levelSerializer.SerializeLevel();
        }
    }
}