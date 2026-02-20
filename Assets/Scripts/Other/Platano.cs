using UnityEngine;

public class Platano : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            NivelManager.Instance.RecogerPlatano();
            Destroy(gameObject);
        }
    }
}