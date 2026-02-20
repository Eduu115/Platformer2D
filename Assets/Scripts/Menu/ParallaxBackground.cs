using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [Header("Configuración")]
    public float speed = 1f;        // Velocidad base de esta capa
    public bool loop = true;        // Si la capa hace loop infinito

    private float spriteWidth;
    private Transform cam;

    void Start()
    {
        cam = Camera.main.transform;
        // Obtiene el ancho del sprite para el loop
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            spriteWidth = sr.bounds.size.x;
    }

    void Update()
    {
        // Mueve la capa hacia la izquierda
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // Loop: cuando sale de pantalla, vuelve al inicio
        if (loop && transform.position.x < -spriteWidth)
        {
            Vector3 pos = transform.position;
            pos.x += spriteWidth * 2f;
            transform.position = pos;
        }
    }
}