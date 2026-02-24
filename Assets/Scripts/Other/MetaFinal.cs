using UnityEngine;

public class MetaFinal : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color colorBloqueada = Color.gray;
    [SerializeField] private Color colorDesbloqueada = Color.yellow;
    [SerializeField] private SueloBoss sueloBoss; // arrastra el suelo del boss aquí

    private void Start()
    {
        spriteRenderer.color = colorBloqueada;
    }

    private void Update()
    {
        if (NivelManager.Instance != null && NivelManager.Instance.TodosRecogidos())
            spriteRenderer.color = colorDesbloqueada;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (NivelManager.Instance == null || NivelManager.Instance.TodosRecogidos())
        {
            sueloBoss.ActivarCaida();
        }
    }
}