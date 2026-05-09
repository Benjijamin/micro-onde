using System.Collections;
using NaughtyAttributes;
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

    [Foldout("Audio")]
    [SerializeField] private AudioClip menuMusic;
    [Foldout("Audio")]
    [SerializeField] private AudioClip clickSound;

    private AudioPlayer menuMusicPlayer;

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

        Button[] buttons = GetComponentsInChildren<Button>(true);
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(OnButtonClick);
        }

        menuMusicPlayer = AudioManager.instance.Play(menuMusic, AudioManager.instance.musicVolume, true);
    }

    private void OnButtonClick()
    {
        AudioManager.instance.Play(clickSound, AudioManager.instance.SFXVolume, false);
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

    private void OnDestroy()
    {
        menuMusicPlayer.FadeOut(0.5f);
    }
}