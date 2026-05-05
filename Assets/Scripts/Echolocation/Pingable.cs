using UnityEngine;

public class Pingable : MonoBehaviour
{
    [SerializeField]
    private GameObject pingEffectPrefab;
    [SerializeField]
    private float cooldown = 2f;
    private float timer;

    private void Start() 
    {
        timer = cooldown;
    }

    private void Update()
    {
        timer += Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Echo") && timer > cooldown)
        {
            Instantiate(pingEffectPrefab, transform);
            timer = 0f;
        }
    }
}
