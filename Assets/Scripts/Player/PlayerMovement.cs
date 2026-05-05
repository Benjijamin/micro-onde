using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
        Look();
    }

    private void Move()
    {
        Vector2 moveDir = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) { moveDir += Vector2.up; }
        if (Input.GetKey(KeyCode.A)) { moveDir += Vector2.left; }
        if (Input.GetKey(KeyCode.S)) { moveDir += Vector2.down; }
        if (Input.GetKey(KeyCode.D)) { moveDir += Vector2.right; }
        rb.linearVelocity = moveDir.normalized * speed;
    }

    private void Look()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (Vector2)mousePos - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
