using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Serializable]
    private class Multiplier 
    {
        public float value;
        public float timer = 0f;
        public float duration = 2f;
    }

    [Serializable]
    private struct QueuedMessage
    {
        public string text;
        public float emphasis;

        public QueuedMessage(string message, float emphasis = 1f) : this()
        {
            this.text = message;
            this.emphasis = emphasis;
        }
    }

    [SerializeField]
    private ScoreMultipliers scoreMultis;

    [Space]

    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private int scoreDisplaySpeed;
    private float scoreDisplayTimer = 0f;
    private int oldScoreDisplay = 0;
    private int currentScoreDisplay = 0;
    private int scoreDisplayTarget = 0;

    [Space]

    [SerializeField]
    private GameObject multiDisplay;
    [SerializeField]
    private TextMeshProUGUI multiText;
    [SerializeField]
    private float multiDisplaySpeed;
    private float multiDisplayTimer = 0f;
    private float oldMultiDisplay = 1f;
    private float currentMultiDisplay = 1f;
    private float multiDisplayTarget = 1f;
    private Animation multiplierTextAnimation;

    [Space]

    [SerializeField]
    private RectTransform messages;
    [SerializeField]
    private RectTransform endOfLevel;
    [SerializeField]
    private GameObject scoreMessagePrefab;
    [SerializeField]
    private GameObject killMessagePrefab;
    [SerializeField]
    private float messageDelay = 1f;
    private float messageTimer;
    private Queue<QueuedMessage> messageQueue = new Queue<QueuedMessage>();

    private int score;
    private int Score { get => score; set { score = value; OnUpdateScore(); } }
    private int recordedScore = 0;

    [Header("Testing")]

    [SerializeField]
    private List<Multiplier> killMultis = new List<Multiplier>();
    [SerializeField]
    private List<Multiplier> bonusMultis = new List<Multiplier>();

    public static ScoreManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start() 
    {
        multiplierTextAnimation = multiText.transform.GetComponent<Animation>();
        messageTimer = messageDelay;
    }

    private void Update()
    {
        TickKillMultis();
        TickBonusMultis();
        TickMessages();

        UpdateScoreDisplay();
        UpdateMultiDisplay();

        //Testing
        if (Input.GetKeyDown(KeyCode.V)) 
        {
            ScoreKill(true, false, false, false, Vector3.zero);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            ScoreKill(true, true, false, false, Vector3.zero);
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            ScoreKill(true, true, true, false, Vector3.zero);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            ScoreKill(true, true, true, true, Vector3.zero);
        }
    }

    private void OnUpdateScore() 
    {
        scoreDisplayTarget = Score;
        scoreDisplayTimer = 0f;
        oldScoreDisplay = currentScoreDisplay;
    }

    private void OnUpdateMulti() 
    {
        multiDisplayTarget = GetMulti();
        multiDisplayTimer = 0f;
        oldMultiDisplay = currentMultiDisplay;
    }

    public void ScoreKill(bool swappedRecently, bool isMelee, bool isStealth, bool isBlind, Vector3 killPosition) 
    {
        int baseScore = scoreMultis.killScore;

        if (swappedRecently || isMelee || isStealth || isBlind)
        {
            RefreshBonusMultis();
        }

        RefreshKillMultis();

        ApplyKillMulti();

        messageQueue.Enqueue(new QueuedMessage("KILL"));

        if (swappedRecently) 
        {
            baseScore += scoreMultis.gunSwapScore;

            ApplyBonusMulti(scoreMultis.gunSwapMultiplier, scoreMultis.gunSwapDuration);

            messageQueue.Enqueue(new QueuedMessage("QUICK HANDS"));
        }

        if (isMelee) 
        {
            baseScore += scoreMultis.meleeScore;

            ApplyBonusMulti(scoreMultis.meleeMultiplier, scoreMultis.meleeDuration);

            messageQueue.Enqueue(new QueuedMessage("MELEE KILL!!!", 1.5f));
        }

        if (isStealth) 
        {
            baseScore += scoreMultis.stealthKillScore;

            ApplyBonusMulti(scoreMultis.stealthKillMultiplier, scoreMultis.stealthKillDuration);

            messageQueue.Enqueue(new QueuedMessage("STEALTHY!", 1.2f));
        }

        if (isBlind) 
        {
            baseScore += scoreMultis.blindKillScore;

            ApplyBonusMulti(scoreMultis.blindKillMultiplier, scoreMultis.blindKillDuration);

            messageQueue.Enqueue(new QueuedMessage("HOW?!??!!", 2f));
        }

        int killScore = (int)(baseScore * GetMulti());

        Score += killScore;

        ShowKillMessage("+ " + killScore + " pts", killPosition);
    }

    public void ScoreDodge() 
    {
        RefreshBonusMultis();

        ApplyBonusMulti(scoreMultis.dodgeMultiplier, scoreMultis.dodgeDuration);

        int dodgeScore = (int)(scoreMultis.dodgeScore * GetMulti()); 

        Score += dodgeScore;

        messageQueue.Enqueue(new QueuedMessage("DODGE!"));
    }

    public void ScoreMultiScan()
    {
        RefreshBonusMultis();

        ApplyBonusMulti(scoreMultis.multiScanMultiplier, scoreMultis.multiScanDuration);

        int multiScanScore = (int)(scoreMultis.multiScanScore * GetMulti());

        Score += multiScanScore;

        messageQueue.Enqueue(new QueuedMessage("MULTI SCAN"));
    }

    public void ScoreLastBullet()
    {
        RefreshBonusMultis();

        ApplyBonusMulti(scoreMultis.lastBulletMultiplier, scoreMultis.lastBulletDuration);

        int lastBulletScore = (int)(scoreMultis.lastBulletScore * GetMulti());

        Score += lastBulletScore;

        messageQueue.Enqueue(new QueuedMessage("LAST BULLET"));
    }

    private void ApplyKillMulti() 
    {
        killMultis.Add(new Multiplier 
        {
            value = scoreMultis.killMultiplier,
            duration = scoreMultis.killDuration / (1 + scoreMultis.killMultiSpeedup * killMultis.Count)
        });

        OnUpdateMulti();
    }

    private void ApplyBonusMulti(float multiplier, float duration) 
    {
        bonusMultis.Add(new Multiplier 
        {
            value = multiplier,
            duration = duration / (1 + scoreMultis.bonusMultiSpeedup * bonusMultis.Count)
        });

        OnUpdateMulti();
    }

    private void TickKillMultis() 
    {
        if (killMultis.Count > 0)
        {
            for (int i = killMultis.Count - 1; i >= 0; i--)
            {
                killMultis[i].timer += Time.deltaTime;
                if (killMultis[i].timer > killMultis[i].duration)
                {
                    killMultis.RemoveAt(i);
                    OnUpdateMulti();
                }
            }
        }
    }

    private void TickBonusMultis() 
    {
        if (bonusMultis.Count > 0)
        {
            for (int i = bonusMultis.Count - 1; i >= 0; i--)
            {
                bonusMultis[i].timer += Time.deltaTime;
                if (bonusMultis[i].timer > bonusMultis[i].duration)
                {
                    bonusMultis.RemoveAt(i);
                    OnUpdateMulti();
                }
            }
        }
    }

    private void TickMessages() 
    {
        if (messageQueue.Count > 0) {

            messageTimer += Time.deltaTime;
            if (messageTimer > messageDelay) 
            {
                ShowMessage(messageQueue.Dequeue());

                messageTimer = messageQueue.Count > 0 ? 0f : messageDelay;
            }
        }
    }

    private void RefreshKillMultis() 
    {
        for (int i = 0; i < killMultis.Count; i++) 
        {
            killMultis[i].timer = 0f;
        }
    }

    private void RefreshBonusMultis() 
    {
        for (int i = 0; i < bonusMultis.Count; i++) 
        {
            bonusMultis[i].timer = 0f;
        }
    }

    public float GetScore() 
    {
        return Score;
    }

    public float GetMulti() 
    {
        float killMulti = 1f;

        for (int i = 0; i < killMultis.Count; i++)
        {
            killMulti += killMultis[i].value;
        }

        float bonusMulti = 1f;

        for (int i = 0; i < bonusMultis.Count; i++)
        {
            bonusMulti += bonusMultis[i].value;
        }

        return killMulti * bonusMulti;
    }

    private void ResetKillMultis() 
    {
        killMultis.Clear();
        OnUpdateMulti();
    }

    private void ResetBonusMultis() 
    {
        bonusMultis.Clear();
        OnUpdateMulti();
    }

    public void RecordScore() 
    {
        recordedScore = Score;
    }

    public void RevertScore() 
    {
        Score = recordedScore;
    }

    public void ResetScore() 
    {
        ResetKillMultis();
        ResetBonusMultis();
        Score = 0;
        recordedScore = 0;
    }

    private void UpdateScoreDisplay() 
    {
        if (currentScoreDisplay != scoreDisplayTarget) 
        {
            scoreDisplayTimer += Time.deltaTime;

            currentScoreDisplay = Mathf.RoundToInt(Mathf.SmoothStep(oldScoreDisplay, scoreDisplayTarget, scoreDisplayTimer * scoreDisplaySpeed));

            scoreText.SetText(currentScoreDisplay.ToString() + " pts");
        }
    }

    private void UpdateMultiDisplay() 
    {
        if (currentMultiDisplay != multiDisplayTarget) 
        {
            multiDisplayTimer += Time.deltaTime;

            currentMultiDisplay = Mathf.SmoothStep(oldMultiDisplay, multiDisplayTarget, multiDisplayTimer * multiDisplaySpeed);

            multiText.SetText(currentMultiDisplay.ToString("0.#") + "x");
        }

        bool display = currentMultiDisplay > 1f;
        bool hype = currentMultiDisplay >= 3f;

        if (multiDisplay.activeSelf != display) 
        {
            multiDisplay.SetActive(display);
        }

        if (hype != multiplierTextAnimation.isPlaying) 
        {
            if (hype)
            {
                multiplierTextAnimation.Play();
            }
            else 
            { 
                multiplierTextAnimation.Stop();
                multiText.color = Color.white;
            }
        }
    }

    private void ShowMessage(QueuedMessage message) 
    {
        GameObject m = Instantiate(scoreMessagePrefab, messages);
        ScoreMessage sm = m.GetComponent<ScoreMessage>();

        sm.SetMessage(message.text, message.emphasis);
    }

    private void ShowKillMessage(string message, Vector3 killPosition) 
    {
        GameObject m = Instantiate(killMessagePrefab, killPosition, Quaternion.identity);
        ScoreMessage sm = m.GetComponent<ScoreMessage>();

        sm.SetMessage(message, 1f);
    }

    public IEnumerator ShowEndOfLevelScore(float delay) 
    {
        yield return new WaitForSeconds(delay);
        GameObject m = Instantiate(scoreMessagePrefab, endOfLevel);
        ScoreMessage sm = m.GetComponent<ScoreMessage>();

        sm.SetMessage(GetScore() + " pts!!!", 4f);
    }

    public void ShowSuicideMessage()
    {
        GameObject m = Instantiate(scoreMessagePrefab, endOfLevel);
        ScoreMessage sm = m.GetComponent<ScoreMessage>();

        sm.SetMessage("Suicide!", 4f);
    }
}
