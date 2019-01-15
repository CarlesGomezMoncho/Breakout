using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public Transform centroRaqueta;

    public float impulseForce = 2;

    private bool iniciada = false;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!iniciada)
            transform.position = centroRaqueta.position;

    }

    public void Iniciar()
    {
        iniciada = true;

        rb.AddForce(new Vector2(0, 1 * impulseForce), ForceMode2D.Impulse);
    }
}
