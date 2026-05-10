using System;
using UnityEngine;

public class EchoNode : MonoBehaviour
{
    public int waveId;
    public int MaxBounces { get; set; }
    public float Speed { get; set; }
    public int CurrentBounces { get; set; }

    public Action OnExpired;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = transform.up * Speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CurrentBounces++;
        if (CurrentBounces >= MaxBounces)
        {
            Expire();
        }

        Vector2 reflectDir = Vector2.Reflect(transform.up, collision.contacts[0].normal);
        transform.up = reflectDir;
    }

    public void Expire()
    {
        OnExpired?.Invoke();
        gameObject.SetActive(false);
        OnExpired = null;
    }
}