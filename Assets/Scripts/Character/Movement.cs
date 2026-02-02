using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float velocidadMovimiento = 5f;
    [SerializeField] private float fuerzaSalto = 10f;

    [Header("Detección de Suelo")]
    [SerializeField] private Transform puntoDeteccionSuelo;
    [SerializeField] private float radioDeteccion = 0.2f;
    [SerializeField] private LayerMask capaSuelo;

    private Rigidbody2D rb;
    private bool enSuelo;
    private float movimientoHorizontal;

    // Start is called before the first frame update
    void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() 
    {
        // Input de movimiento
        movimientoHorizontal = Input.GetAxis("Horizontal");

        // Detectar si está en el suelo
        enSuelo = Physics2D.OverlapCircle(puntoDeteccionSuelo.position, radioDeteccion, capaSuelo);

        // Salto
        if (Input.GetButtonDown("Jump") && enSuelo) {
            Saltar();
        }
    }

    void FixedUpdate() {
        // Aplicar movimiento
        rb.velocity = new Vector2(movimientoHorizontal * velocidadMovimiento, rb.velocity.y);
    }

    void Saltar() {
        rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
    }

    // Visualizar el área de detección de suelo en el editor
    void OnDrawGizmosSelected() {
        if (puntoDeteccionSuelo != null) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(puntoDeteccionSuelo.position, radioDeteccion);
        }
    }
}
