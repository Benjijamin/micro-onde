using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Scene, SerializeField] private string mainMenu;
    [Scene, SerializeField] private string[] levels;

    private int currentIndex;

    public static GameManager instance { get; private set; }

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

    public void NextLevel()
    {
        if (currentIndex + 1 >= levels.Length)
            SceneManager.LoadScene(mainMenu);
        else
            LoadLevel(currentIndex + 1);
    }

    public void ReloadLevel()
    {
        LoadLevel(currentIndex);
    }

    public void LoadLevel(int index)
    {
        SceneManager.LoadScene(levels[index]);
        currentIndex = index;
    }
}