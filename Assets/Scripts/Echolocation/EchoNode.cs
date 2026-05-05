using UnityEngine;

public class EchoNode : MonoBehaviour
{
    public int MaxBounces { get; set; }
    public float Speed { get; set; }
    public int CurrentBounces { get; private set; }

    private void FixedUpdate()
    {
        transform.position += transform.up * Speed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CurrentBounces++;
        if(CurrentBounces >= MaxBounces)
            Destroy(gameObject);

        Vector2 reflectDir = Vector2.Reflect(transform.up, collision.contacts[0].normal);
        transform.up = reflectDir;
    }
}