
using TMPro;
using UnityCore.Audio;
using UnityCore.Game;
using UnityCore.Menu;
using UnityEngine;
using AudioType = UnityCore.Audio.AudioType;

public class ScoreController : MonoBehaviour
{
    public static ScoreController instance;

    public int pointsForSecondsLeft = 50;

    public int glyphsPerRound = 6;
    public int playerScore = 0;
    public int glyphsSolved = 0;
    public int connectionsMade = 0;
    public int roundsComplete = 0;

    [SerializeField] private TMP_Text m_PlayerScoreDisplayText;

    private void Awake()
    {
        Configure();
    }

    private void Start()
    {
        m_PlayerScoreDisplayText.text = playerScore.ToString();
    }

    public void IncrementPlayerScore(int _newPoints)
    {
        playerScore += _newPoints;
        m_PlayerScoreDisplayText.text = playerScore.ToString();
    }

    public void IncrementGlyphsSolved()
    {
        glyphsSolved++;
        if (glyphsSolved % glyphsPerRound == 0)
        {
            TabulateScoreForGlyphSolved();
            TabulateScoreForRound();
            EndRound();
        }
        else
        {
            TabulateScoreForRound();
        }
    }

    public void TabulateScoreForGlyphSolved()
    {
        int _glyphSolvePoints = 100;
        _glyphSolvePoints += ConnectionController.instance.connections.Count * 25;
        connectionsMade += ConnectionController.instance.connections.Count;
        IncrementPlayerScore(_glyphSolvePoints);
    }

    public void TabulateScoreForRound()
    {
        int _timeLeftBonus = (int) RoundTimerController.instance.roundTime * pointsForSecondsLeft;
        IncrementPlayerScore(_timeLeftBonus);
    }
    
    public void ResetScores()
    {
        playerScore = 0;
        glyphsSolved = 0;
        connectionsMade = 0;
        roundsComplete = 0;
        m_PlayerScoreDisplayText.text = playerScore.ToString();
    }

    private void EndRound()
    {
        roundsComplete++;
        GameController.instance.isRoundOver = true;
        KeyholeController.instance.DestroyAllKeyholes();
        PlayerInputHandler.instance.pathWriterString = "";
        PageController.instance.TurnPageOn(PageType.RoundComplete);
        AudioController.instance.PlayAudio(AudioType.ROUNDOVER_SFX);
        AudioController.instance.PlayAudio(AudioType.ROUNDCOMPLETENEONTRENCH_ST);
    }

    private void Configure()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
