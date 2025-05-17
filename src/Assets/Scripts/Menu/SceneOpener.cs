using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

public class SceneOpener : MonoBehaviour
{
    public static void OpenGame()
    {
        SceneManager.LoadScene("LevelScene");
        AudioManager.Instance.StopMusic();
    }

    public static void OpenEditor()
    {
        SceneManager.LoadScene("Level_editor"); // nom de la sc�ne de l'�diteur
        AudioManager.Instance.StopMusic();
    }
    public static void OpenMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        AudioManager.Instance.StopMusic();
    }
}
