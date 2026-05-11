using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [Scene, SerializeField] private string mainMenu;
    [Scene, SerializeField] private LevelsData levelsData;

    [SerializeField] private GameObject canvas;
    [SerializeField] private Animation anim;

    [SerializeField] private AnimationClip fadeIn;
    [SerializeField] private AnimationClip fadeOut;

    [SerializeField] private float fadeInTime;
    [SerializeField] private float fadeOutTime;

    private int currentIndex;

    [Foldout("Audio")][SerializeField] private AudioClip inGameTheme;

    private AudioPlayer inGameAudioPlayer;

    public static LevelLoader instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void NextLevel()
    {
        if (currentIndex + 1 >= levelsData.Levels.Count)
        {
            inGameAudioPlayer.FadeOut(0.5f, () => { inGameAudioPlayer.Abort(); inGameAudioPlayer = null; });
            SceneManager.LoadScene(mainMenu);
        }
        else
            LoadLevel(currentIndex + 1);
    }

    public void ReloadLevel()
    {
        LoadLevel(currentIndex, false);
    }

    public void LoadLevel(int index, bool useTransitions = true)
    {
        if (useTransitions)
            StartCoroutine(FadeInScene(index, fadeInTime));
        else
            SceneManager.LoadScene(levelsData.Levels[index].Scene);

        currentIndex = index;
    }

    private IEnumerator FadeInScene(int sceneIndex, float duration)
    {
        canvas.gameObject.SetActive(true);

        anim.clip = fadeIn;
        anim.Play();

        yield return new WaitForSeconds(duration);

        
        SceneManager.LoadScene(levelsData.Levels[sceneIndex].Scene);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "MainMenu")
        { 
            StartCoroutine(FadeOutLevelStart(fadeOutTime));
            if (inGameAudioPlayer == null)
            {
                inGameAudioPlayer = AudioManager.instance.Play(inGameTheme, AudioType.Music, true);
            }
            ScoreManager.Instance.RecordScore();
        }
        else 
        {
            ScoreManager.Instance.ResetScore();
        }
    }

    private IEnumerator FadeOutLevelStart(float duration)
    {
        MessageManager.instance.ShowGeneralMessage(levelsData.Levels[currentIndex].IntroText, levelsData.Levels[currentIndex].IntroDuration, true);

        yield return new WaitForSeconds(duration - fadeOut.length);

        anim.clip = fadeOut;
        anim.Play();

        yield return new WaitForSeconds(fadeOut.length);

        canvas.gameObject.SetActive(false);
    }
}