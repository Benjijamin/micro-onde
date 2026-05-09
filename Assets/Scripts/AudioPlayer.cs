using System.Collections;
using UnityEngine;
using UnityEngine.Animations;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] public AudioSource source;
    [SerializeField] public ParentConstraint constraint;

    private Coroutine fadeCoroutine;

    public void Play(AudioClip clip, float volume, float pitch, bool loop, bool spatialBlend = false)
    {
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.loop = loop;
        source.spatialBlend = spatialBlend ? 1 : 0;
        source.Play();
    }

    public void Play(AudioClip clip, float volume, float pitch, bool loop, Vector2 position)
    {
        transform.position = position;
        Play(clip, volume, pitch, loop, true);
    }

    public void Play(AudioClip clip, float volume, float pitch, bool loop, Transform followTarget)
    {
        ConstraintSource parent = new ConstraintSource();
        parent.sourceTransform = followTarget;
        parent.weight = 1;
        constraint.AddSource(parent);
        constraint.enabled = true;
        Play(clip, volume, pitch, loop, true);
    }

    public void Abort()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }
        source.Stop();
        constraint.enabled = false;
    }

    public void FadeIn(float targetVolume, float fadeDuration)
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        if (fadeDuration <= 0f)
        {
            source.volume = targetVolume;
            return;
        }
        fadeCoroutine = StartCoroutine(FadeCoroutine(0f, targetVolume, fadeDuration));
    }

    public void FadeOut(float fadeDuration, System.Action onComplete = null)
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        if (fadeDuration <= 0f)
        {
            source.volume = 0f;
            onComplete?.Invoke();
            return;
        }
        fadeCoroutine = StartCoroutine(FadeCoroutine(source.volume, 0f, fadeDuration, onComplete));
    }

    private IEnumerator FadeCoroutine(float from, float to, float duration, System.Action onComplete = null)
    {
        float elapsed = 0f;
        source.volume = from;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            source.volume = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }
        source.volume = to;
        fadeCoroutine = null;
        onComplete?.Invoke();
    }
}
