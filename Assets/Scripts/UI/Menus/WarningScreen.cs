using System.Collections;
using UnityEngine;

public class WarningScreen : MonoBehaviour
{
    public static WarningScreen instance;

    [SerializeField] private CanvasGroup warningScreen;
    [SerializeField] private float warningDuration;
    [SerializeField] private float warningFadeOutTime;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }

    private void Start()
    {
        StartCoroutine(FadeOutWarning());
    }

    private IEnumerator FadeOutWarning()
    {
        yield return new WaitForSeconds(warningDuration);
        float t = 0;
        while (warningScreen.alpha > 0)
        {
            yield return null;
            t += Time.deltaTime;
            warningScreen.alpha = Mathf.Lerp(1, 0, t / warningFadeOutTime);
        }
    }
}
