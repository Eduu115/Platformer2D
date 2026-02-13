using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private int dañoQueCausa = 1;
    [SerializeField] private string tagJugador = "Player";

    // Soporta tanto colisiones normales como triggers
    void OnCollisionEnter2D(Collision2D collision) => HandleHit(collision.gameObject);
    void OnTriggerEnter2D(Collider2D collider) => HandleHit(collider.gameObject);

    private void HandleHit(GameObject other)
    {
        if (!other.CompareTag(tagJugador)) return;

        // Buscar SistemaVida en el objeto, en sus padres o en sus hijos
        SistemaVida sistemaVida = other.GetComponent<SistemaVida>()
            ?? other.GetComponentInParent<SistemaVida>()
            ?? other.GetComponentInChildren<SistemaVida>();

        if (sistemaVida != null)
        {
            sistemaVida.RecibirDaño(dañoQueCausa, transform.position);
            return;
        }

        // Si no está directamente, intentar localizarlo vía el componente Character
        Character character = other.GetComponent<Character>()
            ?? other.GetComponentInParent<Character>()
            ?? other.GetComponentInChildren<Character>();

        if (character != null)
        {
            // Por si no esta en el mismo GameObject, lo busco en padres e hijos que si no no me funciona.
            SistemaVida sv = character.GetComponent<SistemaVida>()
                ?? character.GetComponentInParent<SistemaVida>()
                ?? character.GetComponentInChildren<SistemaVida>();

            if (sv != null)
            {
                sv.RecibirDaño(dañoQueCausa, transform.position);
                return;
            }
        }
    }
}