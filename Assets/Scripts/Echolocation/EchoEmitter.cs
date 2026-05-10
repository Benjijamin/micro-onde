using System.Collections;
using UnityEngine;

public class EchoEmitter : MonoBehaviour
{
    [SerializeField] private EchoEmitterSettings settings;
    [SerializeField] private float cooldown;

    private float timer;

    void Update()
    {
        timer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && timer < 0)
            StartCoroutine(EmitWaves(settings.NbWaves, settings.TimeInterval)); 
    }

    private IEnumerator EmitWaves(int count, float interval)
    {
        timer = cooldown;
        Vector3 dirSnapshot = -transform.up;

        for (int i = 0; i < count; i++)
        {
            Echolocation.instance.EmitWave(transform.position, dirSnapshot, settings.Speed, settings.ConeAngle, settings.Radius, settings.LifeTime, settings.MaxBounces, settings.NbNodePerWave);
            yield return new WaitForSeconds(interval);
        }
    }
}