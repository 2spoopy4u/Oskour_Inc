using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class MainMenuManager : MonoBehaviour
{
    public Transform levelListPanel;
    public GameObject levelButtonPrefab;
    public GameObject levelLabel;
    public Button playButton; 

    void Start()
    {
        playButton.interactable = false;
        LoadLevelButtons();
    }

    void LoadLevelButtons()
    {
        // Nettoyer anciens boutons
        foreach (Transform child in levelListPanel)
            Destroy(child.gameObject);

        // 1) Charger niveaux depuis Resources/Levels (fichiers TextAsset embarqués)
        TextAsset[] resourceLevelFiles = Resources.LoadAll<TextAsset>("Levels");

        foreach (TextAsset levelFile in resourceLevelFiles)
        {
            CreateLevelButton(levelFile.name, levelFile.text, isPersistent: false);
        }

        // 2) Charger niveaux depuis persistentDataPath/Levels (fichiers JSON sauvegardés)
        string levelsDir = Path.Combine(Application.persistentDataPath, "Levels");

        if (Directory.Exists(levelsDir))
        {
            string[] savedLevelFiles = Directory.GetFiles(levelsDir, "*.json");

            foreach (string filePath in savedLevelFiles)
            {
                try
                {
                    string jsonText = File.ReadAllText(filePath);
                    string fileName = Path.GetFileNameWithoutExtension(filePath);
                    CreateLevelButton(fileName, jsonText, isPersistent: true);
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Erreur lecture fichier niveau : " + filePath + " > " + e.Message);
                }
            }
        }
    }

    void CreateLevelButton(string levelName, string jsonText, bool isPersistent)
    {
        GameObject btn = Instantiate(levelButtonPrefab, levelListPanel);
        btn.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = levelName;

        string capturedLevelName = levelName;
        string capturedJson = jsonText;
        bool capturedIsPersistent = isPersistent;

        btn.GetComponent<Button>().onClick.AddListener(() =>
        {
            LoadLevel(capturedLevelName, capturedJson, capturedIsPersistent);
        });
    }

    public void LoadLevel(string levelName, string jsonText, bool fromPersistent)
    {
        levelLabel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = levelName;

        if (fromPersistent)
        {
            Debug.Log($"Chargement niveau sauvegardé : {levelName}");
            LevelLoaderData.selectedLevelJsonText = jsonText;
        }
        else
        {
            Debug.Log($"Chargement niveau Resources : {levelName}");
            LevelLoaderData.selectedLevelJsonText = jsonText;
        }
        playButton.interactable = true;
    }
}
