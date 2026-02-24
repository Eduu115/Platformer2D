using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Patrulla")]
    [SerializeField] private float velocidadPatrulla = 2f;
    [SerializeField] private Transform puntoIzquierda;
    [SerializeField] private Transform puntoDerecha;

    [Header("Persecución")]
    [SerializeField] private float velocidadPersecucion = 4f;
    [SerializeField] private float rangoDeteccion = 5f;

    [Header("Daño")]
    [SerializeField] private int danoAlJugador = 1;

    private Transform jugador;
    private Rigidbody2D rb;
    private Animator animator;
    private bool moviendoDerecha = true;
    private bool vivo = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        jugador = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (!vivo) return;

        float distanciaAlJugador = Vector2.Distance(transform.position, jugador.position);

        if (distanciaAlJugador <= rangoDeteccion)
            Perseguir();
        else
            Patrullar();
    }

    private void Patrullar()
    {
        animator.SetBool("corriendo", true);

        float velocidad = moviendoDerecha ? velocidadPatrulla : -velocidadPatrulla;
        rb.velocity = new Vector2(velocidad, rb.velocity.y);

        if (moviendoDerecha && transform.position.x >= puntoDerecha.position.x)
            Girar(false);
        else if (!moviendoDerecha && transform.position.x <= puntoIzquierda.position.x)
            Girar(true);
    }

    private void Perseguir()
    {
        animator.SetBool("corriendo", true);

        float direccion = jugador.position.x > transform.position.x ? 1f : -1f;
        rb.velocity = new Vector2(direccion * velocidadPersecucion, rb.velocity.y);

        Girar(direccion > 0);
    }

    private void Girar(bool derecha)
    {
        moviendoDerecha = derecha;
        Vector3 escala = transform.localScale;
        escala.x = derecha ? Mathf.Abs(escala.x) : -Mathf.Abs(escala.x);
        transform.localScale = escala;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<SistemaVida>()?.RecibirDaño(danoAlJugador, transform.position);
        }
    }

    public void Morir()
    {
        vivo = false;
        rb.velocity = Vector2.zero;
        animator.SetBool("corriendo", false);
        animator.SetTrigger("morir");
    }
}