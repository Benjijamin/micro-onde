using TMPro;
using UnityEngine;

public class InteractText : MonoBehaviour
{
    [SerializeField] private TMP_Text interactText;

    public void UpdateText(string text)
    {
        interactText.text = text;
    }
}
