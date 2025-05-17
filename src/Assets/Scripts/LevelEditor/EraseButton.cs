using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;
public class EraseButton : MonoBehaviour
{
    public LevelEditorManager editor;
    public GameObject ItemImage;

    public void ButtonClicked()
    {
        UnityEngine.Debug.Log("eraser");

        editor.ResetPrefab();
        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        //GameObject imageInstance = Instantiate(ItemImage, new Vector3(worldPosition.x, worldPosition.y, 0), Quaternion.identity);
        //imageInstance.SetActive(true);
        editor.CurrentButtonPressed = -2;
        editor.ItemPrefabs = null;
        editor.ItemImage = null;
    }


}