using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallPlatform : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float tiempoTemblor = 0.5f;
    [SerializeField] private float tiempoDesvanecimiento = 0.5f;
    [SerializeField] private float tiempoParaReaparecer = 3f;
    [SerializeField] private bool caerConFisica = true;

    [Header("Efectos")]
    [SerializeField] private float intensidadTemblor = 0.05f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Collider2D col;
    private Vector3 posicionOriginal;
    private Quaternion rotacionOriginal;
    private Color colorOriginal;
    private bool yaActivada = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        // GUARDAR posición y rotación original
        posicionOriginal = transform.position;
        rotacionOriginal = transform.rotation;

        if (spriteRenderer != null)
            colorOriginal = spriteRenderer.color;

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !yaActivada)
        {
            if (collision.contacts[0].normal.y < -0.5f)
            {
                yaActivada = true;
                StartCoroutine(SecuenciaCompleta());
            }
        }
    }

    IEnumerator SecuenciaCompleta()
    {
        // Temblor
        float tiempoTranscurrido = 0f;
        while (tiempoTranscurrido < tiempoTemblor)
        {
            float offsetX = Random.Range(-intensidadTemblor, intensidadTemblor);
            float offsetY = Random.Range(-intensidadTemblor, intensidadTemblor);
            transform.position = posicionOriginal + new Vector3(offsetX, offsetY, 0);

            yield return new WaitForSeconds(0.05f);
            tiempoTranscurrido += 0.05f;
        }

        transform.position = posicionOriginal;

        // Desvanecimiento (cambiar alpha) - como en el video de referencia (copiado tal cual)
        tiempoTranscurrido = 0f;
        while (tiempoTranscurrido < tiempoDesvanecimiento)
        {
            float alpha = 1 - (tiempoTranscurrido / tiempoDesvanecimiento);

            if (spriteRenderer != null)
            {
                Color nuevoColor = colorOriginal;
                nuevoColor.a = alpha;
                spriteRenderer.color = nuevoColor;
            }

            tiempoTranscurrido += Time.deltaTime;
            yield return null;
        }

        // Caer
        if (caerConFisica && rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;

            if (col != null)
                col.enabled = false;
        }
        else
        {
            // Solo desaparecer si no queremos, segun el momentdo del gameplay
            if (col != null)
                col.enabled = false;

            if (spriteRenderer != null)
                spriteRenderer.enabled = false;
        }

        // REAPARECER después de un tiempo
        yield return new WaitForSeconds(tiempoParaReaparecer);
        Reaparecer();
    }

    void Reaparecer()
    {
        // funcuon donde reseteamos todo para que la plataforma vuelva a su estado original, para que pueda volver a caer si el jugador vuelve a pisarla
        // Resetear posición y rotación
        transform.position = posicionOriginal;
        transform.rotation = rotacionOriginal;

        // Resetear física
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        // Activar collider
        if (col != null)
            col.enabled = true;

        // Restaurar sprite
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
            spriteRenderer.color = colorOriginal;
        }

        yaActivada = false;
    }
}