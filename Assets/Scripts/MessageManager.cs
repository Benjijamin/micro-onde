using UnityEngine;
using TMPro;

public class MessageManager : MonoBehaviour
{

    [SerializeField] private GameObject levelClearedMessage;    
    [SerializeField] private Animation levelClearedAnimation;
    
    [SerializeField] private TMP_Text generalMessageText;
    [SerializeField] private Animation generalMessageAnimation;

    [SerializeField] private AnimationClip FadeIn;
    [SerializeField] private AnimationClip FadeOut;

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
        }
    }

    public void ShowMessage(string message, float duration)
    {
        
    }

    public void ShowLevelCleared(float duration) 
    {
        levelClearedMessage.SetActive(true);
        levelClearedMessage.SetActive(true);
    }
}