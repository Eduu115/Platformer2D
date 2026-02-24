using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Creditos : MonoBehaviour
{
    [Header("Scroll")]
    [SerializeField] private RectTransform contenidoCreditos;
    [SerializeField] private float velocidadScroll = 60f;
    [SerializeField] private float posicionFinal = 1200f; // ajusta según la altura de tu texto

    [Header("Botones")]
    [SerializeField] private GameObject botonesFinales;
    [SerializeField] private string nombreMenuPrincipal = "MenuPrincipal";

    private bool scrollActivo = true;

    private void Start()
    {
        botonesFinales.SetActive(false);
    }

    private void Update()
    {
        if (!scrollActivo) return;

        contenidoCreditos.anchoredPosition += Vector2.up * velocidadScroll * Time.deltaTime;

        if (contenidoCreditos.anchoredPosition.y >= posicionFinal)
        {
            scrollActivo = false;
            botonesFinales.SetActive(true);
        }
    }

    public void VolverAlMenu()
    {
        SceneManager.LoadScene(nombreMenuPrincipal);
    }

    public void SalirDelJuego()
    {
        Application.Quit();
    }
}