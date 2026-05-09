using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MuzzleFlash : MonoBehaviour
{
    [SerializeField]
    private float duration = 0.15f;
    private float intensity;
    private float falloff;
    private float timer = 0f;

    [SerializeField]
    private Light2D light2D;

    private void Awake()
    {
        intensity = light2D.intensity;
        falloff = light2D.falloffIntensity;
    }

    void Update()
    {
        timer += Time.deltaTime;

        light2D.intensity = (1 - timer / duration) * intensity;

        if (timer > duration) 
        {
            Destroy(gameObject);
        }
    }
}
