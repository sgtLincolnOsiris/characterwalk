using UnityEngine;
using TMPro;

public class ArrowUI : MonoBehaviour
{
    public PlayerMovement playerMovement; // Assign via Inspector
    public TextMeshProUGUI arrowText;

    void Update()
    {
        if (playerMovement != null && arrowText != null)
        {
            arrowText.text = playerMovement.GetArrowCount().ToString();
        }
    }
}
