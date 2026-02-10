using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class SistemaVida : MonoBehaviour
{
    [Header("Configuracion de vida")]
    [SerializeField] private int vidaMaxima = 3;
    [SerializeField] private int vidaActual;
    
    [Header("Invencibilidad")]
    [SerializeField] private float tiempoInvencible = 2f;
    private bool esInvencible = false;

    [Header("UI Corazones")]
    [SerializeField] private Image[] corazones;
    [SerializeField] private Sprite corazonLleno;
    [SerializeField] private Sprite corazonVacio;


    [Header("UI - Barra Invencibilidad")]
    [SerializeField] private GameObject barraInvencibilidad;  // El GameObject padre
    [SerializeField] private Image barraInvencibilidadRelleno;  // La imagen de relleno

    [Header("Efecto Visual")]
    [SerializeField] private GameObject efectoInvencibilidad;  // Panel rojo
    [SerializeField] private bool usarEfectoParpadeante = true;
    [SerializeField] private float velocidadParpadeo = 0.2f;

    [Header("Knockback")]
    [SerializeField] private float fuerzaKnockback = 10f;
    [SerializeField] private float tiempoKnockback = 0.2f;  // Tiempo sin poder moverse


    [Header("Referencias")]
    [SerializeField] private Character scriptMovimiento;
    private Rigidbody2D rb;



    // Start is called before the first frame update
    void Start()
    {
        vidaActual = vidaMaxima;
        ActualizarCorazones();

        rb = GetComponent<Rigidbody2D>();

        if (barraInvencibilidad != null)
            barraInvencibilidad.SetActive(false);  // Aseguramos que la barra esté oculta al inicio

        if (efectoInvencibilidad != null)
            efectoInvencibilidad.SetActive(false);  // Aseguramos que el efecto esté oculto al inicio

        if (scriptMovimiento == null)
        {
            Debug.LogWarning("No se encontró el script MovimientoAvanzado");
        }
    }

    public void RecibirDaño(int cantidad, Vector2 posicionTrampa)
    {
        if (esInvencible)
            return;

        vidaActual -= cantidad;
        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMaxima);
        ActualizarCorazones();

        if (scriptMovimiento != null)
            scriptMovimiento.RecibirGolpe();

        AplicarKnockback(posicionTrampa);

        StartCoroutine(Invencibilidad());

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    // Pa echar para atras el personaje al recibir daño, con una fuerza y dirección basada en la posición de la trampa
    void AplicarKnockback(Vector2 posicionTrampa)
    {
        // Calcular dirección: desde la trampa hacia el jugador
        Vector2 direccion = (transform.position - (Vector3)posicionTrampa).normalized;

        // Aplicar fuerza en esa dirección
        rb.velocity = new Vector2(direccion.x * fuerzaKnockback, direccion.y * fuerzaKnockback * 0.5f);

        // Opcional: Desactivar controles brevemente
        if (scriptMovimiento != null)
        {
            scriptMovimiento.enabled = false;
            Invoke("ReactivarMovimiento", tiempoKnockback);
        }
    }

    void ReactivarMovimiento()
    {
        if (scriptMovimiento != null)
            scriptMovimiento.enabled = true;
    }

    void ActualizarCorazones()
    {
        for (int i = 0; i < corazones.Length; i++)
        {
            if (i < vidaActual)
            {
                corazones[i].sprite = corazonLleno;
            }
            else
            {
                corazones[i].sprite = corazonVacio;
            }

        }
    }

    // Corrutina de invencibilidad
    IEnumerator Invencibilidad()
    {
        esInvencible = true;

        // Mostrar barra y efecto
        if (barraInvencibilidad != null)
            barraInvencibilidad.SetActive(true);

        if (efectoInvencibilidad != null)
            efectoInvencibilidad.SetActive(true);

        float tiempoTranscurrido = 0f;
        float tiempoParpadeo = 0f;
        bool efectoVisible = true;

        while (tiempoTranscurrido < tiempoInvencible)
        {
            tiempoTranscurrido += Time.deltaTime;
            tiempoParpadeo += Time.deltaTime;

            // Actualizar barra
            float porcentajeRestante = 1 - (tiempoTranscurrido / tiempoInvencible);
            if (barraInvencibilidadRelleno != null)
                barraInvencibilidadRelleno.fillAmount = porcentajeRestante;

            // NUEVO: Efecto parpadeante
            if (usarEfectoParpadeante && tiempoParpadeo >= velocidadParpadeo)
            {
                tiempoParpadeo = 0f;
                efectoVisible = !efectoVisible;

                if (efectoInvencibilidad != null)
                    efectoInvencibilidad.SetActive(efectoVisible);
            }

            yield return null;
        }

        // Ocultar todo
        if (barraInvencibilidad != null)
            barraInvencibilidad.SetActive(false);

        if (efectoInvencibilidad != null)
            efectoInvencibilidad.SetActive(false);

        esInvencible = false;
    }

    void Morir()
    {
        Debug.Log("El jugador ha muerto!");

        // reiniciamos la escena después de 2 segundos
        Invoke("ReiniciarNivel", 2f);
    }
    public void Curar(int cantidad)
    {
        vidaActual += cantidad;
        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMaxima);
        ActualizarCorazones();
    }

    void ReiniciarNivel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }

}