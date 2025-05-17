using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameLevelSerializer : MonoBehaviour
{
    public List<GameObject> terrains = new List<GameObject>();
    public string levelPath = "Assets/Resources/Levels/Level2.json";
    public string levelName = "Level 2";

    public void Start()
    {
        // SerializeLevel();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SerializeLevel();
        }
    }

    public static List<GameObject> GetAllGameObjectsInScene()
    {
        Scene activeScene = SceneManager.GetActiveScene();

        if (!activeScene.isLoaded)
        {
            Debug.LogWarning("Active scene is not loaded.");
            return new List<GameObject>();
        }

        // Get all root GameObjects in the scene
        GameObject[] rootObjects = activeScene.GetRootGameObjects();

        // Create a list to store all GameObjects
        var allGameObjects = new List<GameObject>();

        // Recursively gather all child objects
        foreach (GameObject rootObject in rootObjects)
        {
            GatherGameObjectsRecursively(rootObject, allGameObjects);
        }

        return allGameObjects;
    }

    /// <summary>
    /// Recursively adds the GameObject and all its children to the list.
    /// </summary>
    private static void GatherGameObjectsRecursively(GameObject obj, List<GameObject> list)
    {
        list.Add(obj);

        foreach (Transform child in obj.transform)
        {
            GatherGameObjectsRecursively(child.gameObject, list);
        }
    }

    public void SerializeLevel()
    {
        terrains = GetAllGameObjectsInScene();
        List<TerrainTemplate> terrainTemplates = new();

        Vector3 playerStartPos = new(0, 1, 0); // Default start position

        foreach (GameObject obj in terrains)
        {
            if (!int.TryParse(obj.tag, out int terrainType))
            {
                Debug.LogWarning($"GameObject '{obj.name}' a un tag invalide et sera ignoré.");
                continue;
            }

            terrainTemplates.Add(new TerrainTemplate(
                (EnumTerrain)terrainType,
                obj.GetInstanceID(),
                obj.transform.position.x,
                obj.transform.position.y,
                obj.transform.position.z,
                obj.transform.rotation.eulerAngles.z,
                obj.transform.lossyScale.x
            ));
        }

        LevelDataCompressed levelData = new LevelDataCompressed
        {
            LevelName = levelName,
            PlayerStart = new List<double> { playerStartPos.x, playerStartPos.y, playerStartPos.z },
            SongName = "Cycles",
            PlayerSettings = new PlayerSettings
            {
                Gamemode = (int)MovementType.Cube,
                Gravity = 1
            },
            Terrains = new List<object[]>()
        };

        foreach (TerrainTemplate t in terrainTemplates)
        {
            levelData.Terrains.Add(new object[]
            {
                (int)t.TerrainType,
                t.GameObjectId,
                t.X,
                t.Y,
                t.Z,
                t.Rotation,
                t.Scale
            });
        }

        string levelsDir = Path.Combine(Application.persistentDataPath, "Levels");
        if (!Directory.Exists(levelsDir))
        {
            Debug.Log("Création dossier Levels : " + levelsDir);
            Directory.CreateDirectory(levelsDir);
        }

        string levelFileName = Path.GetFileName(levelPath);  // prends uniquement "Level2.json"

        string fullPath = Path.Combine(levelsDir, levelFileName);

        string json = JsonConvert.SerializeObject(levelData, Formatting.Indented);

        Debug.Log($"[SerializeLevel] Chemin complet fichier : {fullPath}");

        try
        {
            File.WriteAllText(fullPath, json);
            Debug.Log($"Niveau sauvegardé avec succès : {fullPath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Erreur lors de la sauvegarde du niveau : {e.Message}");
        }
    }
}