using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float velocidadMovimiento = 7f;
    [SerializeField] private float aceleracion = 10f;
    [SerializeField] private float desaceleracion = 10f;

    [Header("Salto")]
    [SerializeField] private float fuerzaSalto = 12f;
    [SerializeField] private float gravedadCaida = 2.5f;
    [SerializeField] private float gravedadSaltoCorto = 2f;

    [Header("Coyote Time y Jump Buffer")]
    [SerializeField] private float tiempoCoyote = 0.2f;
    [SerializeField] private float tiempoBuffer = 0.2f;

    [Header("Detección de Suelo")]
    [SerializeField] private Transform puntoDeteccionSuelo;
    [SerializeField] private Vector2 tamañoDeteccion = new Vector2(0.5f, 0.1f);
    [SerializeField] private LayerMask capaSuelo;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool enSuelo;
    private float movimientoHorizontal;
    private float contadorCoyote;
    private float contadorBuffer;
    private bool mirandoDerecha = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Input
        movimientoHorizontal = Input.GetAxisRaw("Horizontal");

        // Detección de suelo
        enSuelo = Physics2D.OverlapBox(puntoDeteccionSuelo.position, tamañoDeteccion, 0f, capaSuelo);

        // Coyote Time
        if (enSuelo)
            contadorCoyote = tiempoCoyote;
        else
            contadorCoyote -= Time.deltaTime;

        // Jump Buffer
        if (Input.GetButtonDown("Jump"))
            contadorBuffer = tiempoBuffer;
        else
            contadorBuffer -= Time.deltaTime;

        // SOLUCIÓN: Añadir verificación de velocidad vertical
        bool puedeEjecutarSalto = contadorBuffer > 0f && contadorCoyote > 0f && rb.velocity.y < 0.1f && rb.velocity.y > -0.1f;

        // Salto con coyote time y buffer
        if (puedeEjecutarSalto)
        {
            Saltar();
            contadorBuffer = 0f;
        }

        // Salto corto (soltar botón)
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        // Gravedad mejorada
        AplicarGravedadMejorada();

        // Actualizar animaciones
        ActualizarAnimaciones();

        // Voltear sprite
        Voltear();
    }

    void FixedUpdate()
    {
        // Movimiento suave
        float velocidadObjetivo = movimientoHorizontal * velocidadMovimiento;
        float diferencia = velocidadObjetivo - rb.velocity.x;
        float aceleracionAplicada = (Mathf.Abs(velocidadObjetivo) > 0.01f) ? aceleracion : desaceleracion;
        float movimiento = diferencia * aceleracionAplicada;

        rb.AddForce(movimiento * Vector2.right);
    }

    void Saltar()
    {
        rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
        contadorCoyote = 0f;
    }

    void AplicarGravedadMejorada()
    {
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = gravedadCaida;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.gravityScale = gravedadSaltoCorto;
        }
        else
        {
            rb.gravityScale = 1f;
        }
    }

    void ActualizarAnimaciones()
    {
        // Velocidad horizontal (para Run/Idle) - USANDO "speed"
        float velocidadX = Mathf.Abs(rb.velocity.x);
        animator.SetFloat("speed", velocidadX);

        // Velocidad vertical (para Jump/Fall)
        animator.SetFloat("velocidadY", rb.velocity.y);

        // Estado de suelo
        animator.SetBool("enSuelo", enSuelo);
    }

    void Voltear()
    {
        if (movimientoHorizontal > 0 && !mirandoDerecha)
        {
            mirandoDerecha = true;
            spriteRenderer.flipX = false;
        }
        else if (movimientoHorizontal < 0 && mirandoDerecha)
        {
            mirandoDerecha = false;
            spriteRenderer.flipX = true;
        }
    }

    // Método público para activar el golpe (Hit)
    public void RecibirGolpe()
    {
        animator.SetTrigger("golpeado");
    }

    void OnDrawGizmosSelected()
    {
        if (puntoDeteccionSuelo != null)
        {
            Gizmos.color = enSuelo ? Color.green : Color.red;
            Gizmos.DrawWireCube(puntoDeteccionSuelo.position, tamañoDeteccion);
        }
    }
}