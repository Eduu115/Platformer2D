using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticSawTrap : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private int dañoQueCausa = 1;
    [SerializeField] private string tagJugador = "Player";

    // Detectar colisión con el jugador
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificar si colisionó con el jugador
        if (collision.gameObject.CompareTag(tagJugador))
        {
            // Obtener el script de vida del jugador
            SistemaVida sistemaVida = collision.gameObject.GetComponent<SistemaVida>();

            // Si tiene el componente, causarle daño
            if (sistemaVida != null)
            {
                sistemaVida.RecibirDaño(dañoQueCausa);
            }
        }
    }
}
