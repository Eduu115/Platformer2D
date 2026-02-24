using UnityEngine;
using UnityEngine.SceneManagement;

public class SueloBoss : MonoBehaviour
{
    [SerializeField] private string escenaCreditos = "Credits";
    [SerializeField] private GameObject boss;
    [SerializeField] private float delayDesaparecer = 2f;
    [SerializeField] private float delayCreditos = 4f; // tiempo tras caer para cargar créditos

    public void ActivarCaida()
    {
        StartCoroutine(SecuenciaCaida());
    }

    private System.Collections.IEnumerator SecuenciaCaida()
    {
        // Espera 2 segundos antes de que desaparezca el suelo
        yield return new WaitForSeconds(delayDesaparecer);

        // Desactiva el collider del suelo para que el boss caiga
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;

        // Si el boss tiene Rigidbody2D, activa la gravedad
        if (boss != null)
        {
            Rigidbody2D rb = boss.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.gravityScale = 3f; // cae rápido
            }
        }

        // Espera a que el boss caiga antes de cargar créditos
        yield return new WaitForSeconds(delayCreditos);

        SceneManager.LoadScene(escenaCreditos);
    }
}