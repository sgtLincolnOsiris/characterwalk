using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnTrigger2D : MonoBehaviour
{
    [Header("Scene Settings")]
    public string nextSceneName;

    [Header("Trigger Settings")]
    public string triggeringTag = "Player"; // Only this tag triggers the load

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(triggeringTag))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
