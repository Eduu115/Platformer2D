using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class SistemaVida : MonoBehaviour
{
    [Header("Configuracion de vida")]
    [SerializeField] private int vidaMaxima = 3;
    [SerializeField] private int vidaActual;

    [Header("UI Corazones")]
    [SerializeField] private Image[] corazones;
    [SerializeField] private Sprite corazonLleno;
    [SerializeField] private Sprite corazonVacio;

    [Header("Invencibilidad")]
    [SerializeField] private float tiempoInvencible = 2f;
    private bool esInvencible = false;

    [Header("Referencias")]
    [SerializeField] private Character scriptMovimiento;

    // Start is called before the first frame update
    void Start()
    {
        vidaActual = vidaMaxima;
        ActualizarCorazones();
    }

    public void RecibirDaño(int cantidad)
    {
        if (esInvencible) return;

        // restamos vida
        vidaActual -= cantidad;
        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMaxima);

        // mandamos actualizacion a la UI
        ActualizarCorazones();

        if (scriptMovimiento != null)
        {
            scriptMovimiento.RecibirGolpe();
        }

        // iniciamos la corrutina para el tiempo/frames de invencibilidad
        StartCoroutine(Invencibilidad());

        // procesamos la muerte
        if (vidaActual <= 0)
        {
            Morir();
        }
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
        yield return new WaitForSeconds(tiempoInvencible);  // Solo espera
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