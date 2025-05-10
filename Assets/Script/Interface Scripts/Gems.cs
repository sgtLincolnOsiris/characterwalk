using UnityEngine;

public class Gem : MonoBehaviour, iItems
{
    public void Collect()
    {
        Destroy(gameObject);
    }

    
}
