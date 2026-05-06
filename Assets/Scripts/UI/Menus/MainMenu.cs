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