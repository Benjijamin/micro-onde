using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;

    private const string masterKey = "Master";
    private const string sfxKey = "Sfx";
    private const string musicKey = "Music";

    private void Start()
    {
        LoadPlayerPrefs();

        masterSlider.onValueChanged.AddListener((float vol) => OnVolumeChanged(masterKey, vol));
        sfxSlider.onValueChanged.AddListener((float vol) => OnVolumeChanged(sfxKey, vol));
        musicSlider.onValueChanged.AddListener((float vol) => OnVolumeChanged(musicKey, vol));

        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        LoadPlayerPrefs();
    }

    private void OnVolumeChanged(string mixerGroup, float volume)
    {
        audioMixer.SetFloat(mixerGroup, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(mixerGroup, volume);
    }

    private void LoadPlayerPrefs()
    {
        loadVolume(masterKey, masterSlider);
        loadVolume(sfxKey, sfxSlider);
        loadVolume(musicKey, musicSlider);
    }

    private void loadVolume(string key, Slider slider)
    {
        if (PlayerPrefs.HasKey(key))
        {
            slider.value = PlayerPrefs.GetFloat(key);
            audioMixer.SetFloat(key, Mathf.Log10(slider.value) * 20);
        }
    }
}