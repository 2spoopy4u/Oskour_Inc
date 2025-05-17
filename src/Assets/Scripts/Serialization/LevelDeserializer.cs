using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

class LevelDeserializer : MonoBehaviour
{
    public string levelPath = "Assets/Resources/Levels/Level1.json";
    public bool lauchPlay = true;
    public void Start()
    {
        DeserializeLevel();
    }

    private GameObject getTerrainPrefab(EnumTerrain terrainType)
    {
        return EnumToGameObject.terrainPrefabs[terrainType];
    }

    public void DeserializeLevel()
    {
        if (string.IsNullOrEmpty(levelPath))
        {
            GameObject player = Resources.Load<GameObject>("Prefabs/EditorPrefab/PlayerSpawn");
            GameObject playerGO = Instantiate(player, new Vector3(0, 0, 0), Quaternion.identity);
            return;
        }
        string json = LevelLoaderData.selectedLevelJsonText;

        LevelDataCompressed levelData = JsonConvert.DeserializeObject<LevelDataCompressed>(json);

        string levelName = levelData.LevelName;
        List<double> playerStartPosition = levelData.PlayerStart;

        if (lauchPlay)
        {
            GameObject player = Resources.Load<GameObject>("Prefabs/Player");
            GameObject playerGO = Instantiate(player, new Vector3((float)playerStartPosition[0], (float)playerStartPosition[1], (float)playerStartPosition[2]), Quaternion.identity);

            Player playerScript = playerGO.GetComponent<Player>();

            playerScript.settings = levelData.PlayerSettings;
            playerScript.UnPause();
            GameObject pauseMenuManagerGO = GameObject.FindGameObjectWithTag("PauseMenuManager");
            PauseMenuManager pauseMenuManager = pauseMenuManagerGO.GetComponent<PauseMenuManager>();
            pauseMenuManager.player = playerScript;
            GameObject endMenuManagerGO = GameObject.FindGameObjectWithTag("EndMenuManager");
            EndMenuManager endMenuManager = endMenuManagerGO.GetComponent<EndMenuManager>();
            endMenuManager.player = playerScript;
            GameObject camera = Resources.Load<GameObject>("Prefabs/Camera");
            GameObject createdCamera = Instantiate(camera, new Vector3(0, 0, -10), Quaternion.identity);

            if (!string.IsNullOrEmpty(levelData.SongName))
            {   
                AudioManager.Instance.LoadAndPlayMusic(levelData.SongName);
            }
            else
            {
                AudioManager.Instance.LoadAndPlayMusic("Cycles");
            }
        }
        else
        {
            GameObject player = Resources.Load<GameObject>("Prefabs/EditorPrefab/PlayerSpawn");
            GameObject playerGO = Instantiate(player, new Vector3((float)playerStartPosition[0], (float)playerStartPosition[1], (float)playerStartPosition[2]), Quaternion.identity);
        }
        List<object[]> terrainTemplatesRaw = levelData.Terrains;
        float maxX = float.MinValue;
        float lastY = float.MinValue;

        foreach (object[] terrainData in terrainTemplatesRaw)
        {
            TerrainTemplate terrainTemplate = new TerrainTemplate(terrainData);
            GameObject terrainPrefab = getTerrainPrefab(terrainTemplate.TerrainType);
            Vector3 position = new Vector3((float)terrainTemplate.X, (float)terrainTemplate.Y, (float)terrainTemplate.Z);
            GameObject terrain = Instantiate(terrainPrefab, position, Quaternion.Euler(0, 0, (float)terrainTemplate.Rotation));
            terrain.transform.localScale = new Vector3((float)terrainTemplate.Scale, (float)terrainTemplate.Scale, (float)terrainTemplate.Scale);
            if (position.x > maxX)
            {
                maxX = position.x;
                lastY = position.y;

            }
        }
        if (lauchPlay)
        {
            float offsetX = 2f;
            float offsetY = 10f;
            GameObject endPortalPrefab = Resources.Load<GameObject>("Prefabs/EndPortal");

            if (endPortalPrefab != null)
            {
                Vector3 portalPosition = new Vector3(maxX + offsetX, lastY + offsetY, 0f);
                Instantiate(endPortalPrefab, portalPosition, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("EndPortal prefab not found in Resources/Prefabs/EndPortal");
            }
        }
       
        Debug.Log("Level deserialized successfully.");
    }
}