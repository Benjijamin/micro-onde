using System;
using UnityEngine;

public class EchoNode : MonoBehaviour
{
    public int MaxBounces { get; set; }
    public float Speed { get; set; }
    public int CurrentBounces { get; set; }

    public Action OnExpired;

    private void FixedUpdate()
    {
        transform.position += transform.up * Speed * Time.fixedDeltaTime;
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