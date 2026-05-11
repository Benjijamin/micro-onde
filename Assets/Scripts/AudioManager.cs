using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioMixerGroup sfxGroup;
    [SerializeField] private AudioMixerGroup musicGroup;

    [SerializeField] private AudioPlayer audioPlayerPrefab;
    [SerializeField] private float pitchShiftRadius;
    
    private Stack<AudioPlayer> availablePlayers = new Stack<AudioPlayer>();
    private Dictionary<AudioPlayer, Coroutine> audioPlaying = new Dictionary<AudioPlayer, Coroutine>();

    private AudioMixerGroup GetAudioMixerGroup(AudioType audioType) => audioType == AudioType.Music ? musicGroup : sfxGroup;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }

    public AudioPlayer Play(AudioClip clip, AudioType type, bool loop, bool shiftPitch = false, float delay = 0f) // Plays global audio (use for music)
    {
        AudioPlayer player = GetAudioPlayer();
        player.source.outputAudioMixerGroup = GetAudioMixerGroup(type);
        player.Play(clip, 1, GetPitch(shiftPitch), loop, delay: delay);
        audioPlaying.Add(player, loop || !clip ? null : StartCoroutine(ReturnAudioPlayerAfterClip(player, clip.length + delay)));
        return player;
    }

    public AudioPlayer Play(AudioClip clip, AudioType type, bool loop, bool shiftPitch, Vector2 position, float delay = 0f) // Plays audio targeted at position
    {
        AudioPlayer player = GetAudioPlayer();
        player.source.outputAudioMixerGroup = GetAudioMixerGroup(type);
        player.Play(clip, 1, GetPitch(shiftPitch), loop, position, delay);
        audioPlaying.Add(player, loop || !clip ? null : StartCoroutine(ReturnAudioPlayerAfterClip(player, clip.length + delay)));
        return player;
    }

    public AudioPlayer Play(AudioClip clip, AudioType type, bool loop, bool shiftPitch, Transform followTarget, float delay = 0f) // Plays audio targeted at target object (use to follow moving objects)
    {
        AudioPlayer player = GetAudioPlayer();
        player.source.outputAudioMixerGroup = GetAudioMixerGroup(type);
        player.Play(clip, 1, GetPitch(shiftPitch), loop, followTarget, delay);
        audioPlaying.Add(player, loop || !clip ? null : StartCoroutine(ReturnAudioPlayerAfterClip(player, clip.length + delay)));
        return player;
    }

    private float GetPitch(bool shift)
    {
        return shift ? Random.Range(1 - pitchShiftRadius, 1 + pitchShiftRadius) : 1;
    }

    private AudioPlayer GetAudioPlayer()
    {
        if (availablePlayers.Count > 0)
        {
            AudioPlayer audioPlayer = availablePlayers.Pop();
            audioPlayer.gameObject.SetActive(true);
            return audioPlayer;
        }
        return Instantiate(audioPlayerPrefab, transform);
    }

    private IEnumerator ReturnAudioPlayerAfterClip(AudioPlayer audioPlayer, float clipDuration)
    {
        yield return new WaitForSecondsRealtime(clipDuration);
        Abort(audioPlayer);
    }

    public void Abort(AudioPlayer audioPlayer)
    {
        if (!audioPlaying.ContainsKey(audioPlayer)) return;

        audioPlayer.Abort();

        Coroutine c = audioPlaying[audioPlayer];
        if (c != null) StopCoroutine(c);

        audioPlaying.Remove(audioPlayer);
        audioPlayer.gameObject.SetActive(false);
        availablePlayers.Push(audioPlayer);
    }
}

public enum AudioType
{
    Sfx,
    Music
}