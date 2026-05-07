using TMPro;
using UnityEngine;

public class ScoreMessage : MonoBehaviour
{
    [SerializeField]
    private float duration = 2.5f;
    private float timer = 0f;

    [SerializeField]
    TextMeshProUGUI tm;

    private void Update()
    {
        timer += Time.deltaTime / duration;

        tm.color = new Color(1f, 1f, 1f, 1 - timer * timer);

        if (timer > duration) 
        {
            Destroy(gameObject);
        }
    }

    public void SetMessage(string text, float emphasis)
    {
        tm.transform.localRotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-20f, 20f));
        tm.fontSize = tm.fontSize * emphasis;
        tm.SetText(text);
    }
}
