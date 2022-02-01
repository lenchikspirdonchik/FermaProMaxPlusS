using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_MainMenu : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject CreditsMenu;

    // Start is called before the first frame update
    private void Start()
    {
        MainMenuButton();
    }

    public void PlayNowButton(string gameLevel)
    {
        // Play Now Button has been pressed, here you can initialize your game (For example Load a Scene called GameLevel etc.)
        SceneManager.LoadScene(gameLevel);
    }

    public void CreditsButton()
    {
        // Show Credits Menu
        MainMenu.SetActive(false);
        CreditsMenu.SetActive(true);
    }

    public void MainMenuButton()
    {
        // Show Main Menu
        MainMenu.SetActive(true);
        CreditsMenu.SetActive(false);
    }

    public void QuitButton()
    {
        // Quit Game
        Application.Quit();
    }

    // delete all save files ans start the game from the beginning
    public void deleteSave(string gameLevel)
    {
        var buffPath = "Save_Cube (1).json";
        string path;
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        path = Path.Combine(Application.persistentDataPath, buffPath);
#else
        path = Path.Combine(Application.dataPath, buffPath);
#endif
        if (File.Exists(path))
        {
            for (var i = 0; i < 97; i++)
            {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        path = Path.Combine(Application.persistentDataPath, "Save_Cube (" + i + ").json");
#else
                path = Path.Combine(Application.dataPath, "Save_Cube (" + i + ").json");
#endif
                File.Delete(path);
            }
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        path = Path.Combine(Application.persistentDataPath, "Save_data.json");
#else
            path = Path.Combine(Application.dataPath, "Save_data.json");
#endif
            File.Delete(path);
        }

        SceneManager.LoadScene(gameLevel);
    }
}