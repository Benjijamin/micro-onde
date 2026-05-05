using System.Collections;
using UnityEngine;

public class EchoEmitter : MonoBehaviour
{
    [SerializeField] private EchoEmitterSettings settings;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            StartCoroutine(EmitWaves(settings.NbWaves, settings.TimeInterval)); 
    }

    private IEnumerator EmitWaves(int count, float interval)
    {
        for (int i = 0; i < count; i++)
        {
            Echolocation.instance.EmitWave(transform.position, transform.right, settings.Speed, settings.ConeAngle, settings.Radius, settings.LifeTime, settings.MaxBounces, settings.NbNodePerWave);
            yield return new WaitForSeconds(interval);
        }
    }
}