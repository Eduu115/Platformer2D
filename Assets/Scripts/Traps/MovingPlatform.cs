using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    [SerializeField] private float distanciaMovimiento = 5f;
    [SerializeField] private float velocidad = 2f;
    [SerializeField] private bool moverEnX = true;

    private Vector3 puntoInicial;
    private Vector3 puntoFinal;
    private Vector3 posicionAnterior;
    private float progreso = 0f;
    private bool moviendoHaciaFinal = true;

    // Referencias a jugadores que están encima
    private Transform jugadorEncima = null;

    void Start()
    {
        puntoInicial = transform.position;
        posicionAnterior = transform.position;

        if (moverEnX)
        {
            puntoFinal = puntoInicial + Vector3.right * distanciaMovimiento;
        }
        else
        {
            puntoFinal = puntoInicial + Vector3.up * distanciaMovimiento;
        }
    }

    void Update()
    {
        // Guardar posición anterior
        posicionAnterior = transform.position;

        // Mover la plataforma
        if (moviendoHaciaFinal)
        {
            progreso += Time.deltaTime * velocidad / distanciaMovimiento;

            if (progreso >= 1f)
            {
                progreso = 1f;
                moviendoHaciaFinal = false;
            }
        }
        else
        {
            progreso -= Time.deltaTime * velocidad / distanciaMovimiento;

            if (progreso <= 0f)
            {
                progreso = 0f;
                moviendoHaciaFinal = true;
            }
        }

        transform.position = Vector3.Lerp(puntoInicial, puntoFinal, progreso);

        // Mover al jugador si está encima
        if (jugadorEncima != null)
        {
            Vector3 movimientoPlataforma = transform.position - posicionAnterior;
            jugadorEncima.position += movimientoPlataforma;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificar que el jugador esté ENCIMA de la plataforma
        if (collision.gameObject.CompareTag("Player") && collision.contacts[0].normal.y < -0.5f)
        {
            jugadorEncima = collision.transform;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        // Verificar constantemente que sigue encima
        if (collision.gameObject.CompareTag("Player"))
        {
            bool estaEncima = false;

            foreach (ContactPoint2D contacto in collision.contacts)
            {
                if (contacto.normal.y < -0.5f)
                {
                    estaEncima = true;
                    break;
                }
            }

            if (estaEncima)
            {
                jugadorEncima = collision.transform;
            }
            else
            {
                jugadorEncima = null;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            jugadorEncima = null;
        }
    }

    void OnDrawGizmos()
    {
        Vector3 inicio = Application.isPlaying ? puntoInicial : transform.position;
        Vector3 final = moverEnX
            ? inicio + Vector3.right * distanciaMovimiento
            : inicio + Vector3.up * distanciaMovimiento;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(inicio, final);
        Gizmos.DrawWireSphere(inicio, 0.2f);
        Gizmos.DrawWireSphere(final, 0.2f);
    }
}