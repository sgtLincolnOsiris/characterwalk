using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour
{
    [Header("Scene Settings")]
    public string sceneToLoad;

    public void LoadScene()
    {
        Time.timeScale = 1f; // In case the game was paused
        SceneManager.LoadScene(sceneToLoad);
    }
}
