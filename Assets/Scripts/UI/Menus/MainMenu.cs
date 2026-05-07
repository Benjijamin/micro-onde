using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button levelSelectionButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitButton;

    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject levelSelectionMenu;
    [SerializeField] private GameObject OptionsMenu;
    [SerializeField] private CanvasGroup warningScreen;
    [SerializeField] private float warningDuration;
    [SerializeField] private float warningFadeOutTime;

    public static MainMenu instance { get; private set; }
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        levelSelectionButton.onClick.AddListener(GoToLevelSelection);
        quitButton.onClick.AddListener(Application.Quit);
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
        warningScreen.gameObject.SetActive(false);
    }

    public void GoToLevelSelection()
    {
        startMenu.SetActive(false);
        //OptionsMenu.SetActive(false);

        levelSelectionMenu.SetActive(true);
    }

    public void GoToStartMenu()
    {
        levelSelectionMenu.SetActive(false);
        //OptionsMenu.SetActive(false);

        startMenu.SetActive(true);
    }
}