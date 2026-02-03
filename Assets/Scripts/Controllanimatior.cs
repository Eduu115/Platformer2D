using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controllanimatior : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Actualizar velocidad para el animator
        float velocidadHorizontal = Mathf.Abs(rb.velocity.x);
        animator.SetFloat("speed", velocidadHorizontal);
    }
}
