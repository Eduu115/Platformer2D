using UnityEngine;
using UnityEngine.SceneManagement;

public class Meta : MonoBehaviour
{
    [SerializeField] private string nombreSiguienteNivel;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color colorBloqueada = Color.gray;
    [SerializeField] private Color colorDesbloqueada = Color.yellow;

    private void Start()
    {
        spriteRenderer.color = colorBloqueada;
    }

    private void Update()
    {
        if (NivelManager.Instance.TodosRecogidos())
            spriteRenderer.color = colorDesbloqueada;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (NivelManager.Instance == null || NivelManager.Instance.TodosRecogidos())
        {
            SceneManager.LoadScene(nombreSiguienteNivel);
        }
    }
}