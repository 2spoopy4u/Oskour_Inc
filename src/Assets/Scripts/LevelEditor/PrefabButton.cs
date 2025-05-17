using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;
public class PrefabButton : MonoBehaviour
{
    public int ID;
    public LevelEditorManager editor;
    public GameObject ItemPrefabs;
    public GameObject ItemImage;

    public void ButtonClicked()
    {
        editor.ResetPrefab();
        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        GameObject imageInstance = Instantiate(ItemImage, new Vector3(worldPosition.x, worldPosition.y, 0), Quaternion.identity);
        UnityEngine.Debug.Log(ID);
        UnityEngine.Debug.Log(editor);
        imageInstance.SetActive(true);
        editor.CurrentButtonPressed = ID;
        editor.ItemPrefabs = ItemPrefabs;
        editor.ItemImage = imageInstance;
    }
     

}
