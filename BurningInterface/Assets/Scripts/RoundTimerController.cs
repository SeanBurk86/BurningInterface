
using System;
using TMPro;
using UnityCore.Audio;
using UnityCore.Game;
using UnityCore.Menu;
using UnityEngine;
using AudioType = UnityCore.Audio.AudioType;

public class RoundTimerController : MonoBehaviour
{
    public static RoundTimerController instance;

    public bool debug; 

    public float roundLength = 300f;
    public float countDownLength = 10f;
    
    public float roundTime = 300.0f;
    [SerializeField] private float roundCountdown = 10.0f;
    [SerializeField] private float roundsCompletedTimeOffset = 10.0f;


    [SerializeField] private TMP_Text m_TimerDisplayText, m_CountdownText;

    private void Awake()
    {
        Configure();
    }

    private void OnEnable()
    {
        GameController.instance.hasRoundStarted = false;
        ResetTimers();
        roundTime = roundLength - (roundsCompletedTimeOffset * ScoreController.instance.roundsComplete);
    }

    private void OnDisable()
    {
        Destroy(gameObject);
    }

    private void Start()
    {
        m_CountdownText = PageController.instance.gameObject.GetComponent<NavigationUtil>().countdownText;
        m_TimerDisplayText = PageController.instance.gameObject.GetComponent<NavigationUtil>().roundTimerText;
    }

    private void Update()
    {
        if (!GameController.instance.isRoundOver)
        {
            if (GameController.instance.hasRoundStarted)
            {
                roundTime -= Time.deltaTime;
                m_TimerDisplayText.text = roundTime.ToString();
                if (roundTime <= 0f)
                {
                    GameController.instance.isRoundOver = true;
                    KeyholeController.instance.DestroyAllKeyholes();
                    PlayerInputHandler.instance.pathWriterString = "";
                    PageController.instance.TurnPageOn(PageType.RunOver);
                    AudioController.instance.PlayAudio(AudioType.RUNOVER_SFX);
                    AudioController.instance.PlayAudio(AudioType.RUNOVERSUNDER_ST);
                }
            }
            else
            {
                roundCountdown -= Time.deltaTime * 3;
                m_CountdownText.text = roundCountdown.ToString();
                if (roundCountdown <= 0f)
                {
                    Log("Countdown has ended starting match");
                    GameController.instance.hasRoundStarted = true;
                    PageController.instance.TurnPageOff(PageType.CountdownTimer);
                }
            }
        }
    }

    public void ResetTimers()
    {
        roundTime = roundLength;
        roundCountdown = countDownLength;
    }

    private void Configure()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Log(string _msg)
    {
        if(debug)Debug.Log("[RoundTimerController]: "+_msg);
    }
}
