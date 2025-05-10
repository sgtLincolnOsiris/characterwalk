using UnityEngine;

public class Collector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        iItems item = collision.GetComponent<iItems>();
        if(item != null)
        {
            item.Collect();
        }
    }
}
