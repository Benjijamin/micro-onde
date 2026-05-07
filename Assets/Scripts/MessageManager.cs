using UnityEngine;
using TMPro;
using System.Collections;

public class MessageManager : MonoBehaviour
{

    [SerializeField] private TMP_Text levelClearedText;
    [SerializeField] private Animation levelClearedAnimation;
    
    [SerializeField] private TMP_Text generalMessageText;
    [SerializeField] private Animation generalMessageAnimation;

    [SerializeField] private AnimationClip fadeIn;
    [SerializeField] private AnimationClip fadeOut;

    private bool isShowingMessage;

    public static MessageManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            ShowGeneralMessage("I can hear something...", 3);
    }

    public void ShowGeneralMessage(string message, float duration)
    {
        if(!isShowingMessage)
            StartCoroutine(ShowMessage(message, duration, generalMessageText, generalMessageAnimation));
    }

    public void ShowLevelCleared(float duration) 
    {
        if (!isShowingMessage)
            StartCoroutine(ShowMessage("Level Clear", duration, levelClearedText, levelClearedAnimation));
    }

    private IEnumerator ShowMessage(string message, float duration, TMP_Text text, Animation animation)
    {
        isShowingMessage = true;
        text.gameObject.SetActive(true);

        text.text = message;
        animation.clip = fadeIn;
        animation.Play();

        yield return new WaitForSeconds(duration - fadeOut.length);

        animation.clip = fadeOut;
        animation.Play();

        yield return new WaitForSeconds(fadeOut.length);

        isShowingMessage = false;
        text.gameObject.SetActive(false);
    }
}