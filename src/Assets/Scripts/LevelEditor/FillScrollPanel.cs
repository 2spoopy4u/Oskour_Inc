using UnityEngine;
using UnityEditor;
using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.IO;
using TMPro; 

public class FillScrollPanel : MonoBehaviour
{
    public LevelEditorManager editor;
    public Transform contentPanel;
    public GameObject buttonPrefab;
    public GameObject imagePrefab;
    public GameObject eraserPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject[] prefabs = Resources.LoadAll<GameObject>("Prefabs/GamePrefab");

        int i = 0;

        foreach (GameObject prefab in prefabs)
        {
            //GameObject prefabInstance = Instantiate(prefab);

            GameObject imageInstance = Instantiate(imagePrefab);
            imageInstance.SetActive(false);
            SpriteRenderer sourceRenderer = prefab.GetComponent<SpriteRenderer>();
            SpriteRenderer targetRenderer = imageInstance.GetComponent<SpriteRenderer>();
            if (sourceRenderer != null && targetRenderer != null)
            {
                targetRenderer.sprite = sourceRenderer.sprite; // Copie du sprite
                Color color = targetRenderer.color;
                color.a = 0.5f; // 0 = totalement transparent, 1 = totalement opaque
                targetRenderer.color = color;
            }
            else
            {
                UnityEngine.Debug.LogWarning("Un des GameObjects n'a pas de SpriteRenderer !");
            }

            GameObject button = Instantiate(buttonPrefab, contentPanel);

            PrefabButton prefabButton = button.GetComponent<PrefabButton>();
            Transform imageChild = prefabButton.transform.Find("Image");
            if (imageChild != null)
            {
                imageChild.GetComponent<UnityEngine.UI.Image>().sprite = targetRenderer.sprite;
            }
            prefabButton.editor = editor;
            prefabButton.ID = i;
            prefabButton.ItemPrefabs = prefab;
            prefabButton.ItemImage = imageInstance;
            i++;

        }
    }
}
