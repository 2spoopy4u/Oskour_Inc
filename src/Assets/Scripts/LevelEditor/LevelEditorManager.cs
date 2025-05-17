using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEditorManager : MonoBehaviour
{

    public GameObject ItemPrefabs;
    public GameObject ItemImage;
    private float currentRotation = 0f;
    public int CurrentButtonPressed;  

    private void Start()
    {
        CurrentButtonPressed = -1;
    }

    private void Update()
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        if(CurrentButtonPressed != -1)
        {
            if (CurrentButtonPressed != -2 && !(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
            {
                float scroll = Input.mouseScrollDelta.y;
                if (scroll != 0)
                {
                    currentRotation += scroll * 15f; // tu peux ajuster la vitesse de rotation
                }
                ItemImage.transform.rotation = Quaternion.Euler(0, 0, currentRotation);
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (CurrentButtonPressed == -2)
                {
                    Collider2D hit = Physics2D.OverlapPoint(worldPosition);
                    UnityEngine.Debug.Log(hit);
                    UnityEngine.Debug.Log(UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject());
                    if (hit != null && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) // Ajoute ce tag à tes objets placés
                    {
                        Destroy(hit.gameObject);
                    }
                }
                else
                {
                    float snappedX = Mathf.Round(worldPosition.x / 1f) * 1f;
                    float snappedY = Mathf.Round(worldPosition.y / 1f) * 1f;
                    Vector3 snappedPosition = new Vector3(snappedX, snappedY, 0f);
                    Instantiate(ItemPrefabs, snappedPosition, Quaternion.Euler(0, 0, currentRotation));
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                ResetPrefab();
            }
        }
     
    }

    public void ResetPrefab()
    {
        currentRotation = 0f;
        CurrentButtonPressed = -1;
        Destroy(ItemImage);
    }

}
 