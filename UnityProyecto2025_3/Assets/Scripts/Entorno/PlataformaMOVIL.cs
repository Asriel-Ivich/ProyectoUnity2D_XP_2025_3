using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaMOVIL: MonoBehaviour
{
    [SerializeField] private Transform[] puntosMovimiento;
    [SerializeField] private float velocidadMovimiento;

    private int siguientePlataforma = 1;
    private bool ordenPlataformas = true;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.freezeRotation = true;
        }
    }

    void FixedUpdate()
    {
        if (puntosMovimiento == null || puntosMovimiento.Length == 0) return;

        if (ordenPlataformas && siguientePlataforma + 1 >= puntosMovimiento.Length)
        {
            ordenPlataformas = false;
        }

        if (!ordenPlataformas && siguientePlataforma <= 0)
        {
            ordenPlataformas = true;
        }

        if (Vector2.Distance(transform.position, puntosMovimiento[siguientePlataforma].position) < 0.1f)
        {
            if (ordenPlataformas)
            {
                siguientePlataforma += 1;
            }
            else
            {
                siguientePlataforma -= 1;
            }
        }

        Vector2 nuevaPosicion = Vector2.MoveTowards(
            transform.position,
            puntosMovimiento[siguientePlataforma].position,
            velocidadMovimiento * Time.fixedDeltaTime
        );

        if (rb != null)
        {
            rb.MovePosition(nuevaPosicion);
        }
        else
        {
            transform.position = nuevaPosicion;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Dog"))
        {
            other.transform.SetParent(this.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Dog"))
        {
            other.transform.SetParent(null);
        }
    }
}
