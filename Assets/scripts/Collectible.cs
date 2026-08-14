using UnityEngine;

public class Collectible : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);
    }
}