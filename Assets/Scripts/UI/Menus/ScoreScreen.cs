using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreScreen : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI ScoreText;

    public void MainMenuButton() 
    {
        LevelLoader.instance.LoadMainMenu();
    }

    private void Start()
    {
        int score = ScoreManager.Instance.GetScore();

        ScoreText.SetText(score + " pts!!!");
    }
}