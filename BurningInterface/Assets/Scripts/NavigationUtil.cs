
using TMPro;
using UnityCore.Game;
using UnityCore.Menu;
using UnityCore.Scene;
using UnityEngine;

public class NavigationUtil : MonoBehaviour
{

    public TMP_Text countdownText, roundTimerText, glyphDisplayText, scoreText;
    public void ExitGame()
    {
        Application.Quit();
    }

    public void StartRun()
    {
        PageController.instance.TurnPageOff(PageType.StartMenu);
        SceneController.instance.Load(SceneType.MainGame);
    }

    public void OpenSettings()
    {
        PageController.instance.TurnPageOff(PageType.StartMenu,PageType.Settings);
    }

    public void StartNextRound()
    {
        GameController.instance.isRoundOver = false;
        PageController.instance.TurnPageOff(PageType.RoundComplete);
        SceneController.instance.ReloadScene();
    }

    public void Instruction()
    {
        
    }

    public void BackToStart()
    {
        GameController.instance.isRoundOver = false;
        GameController.instance.hasRoundStarted = false;
        ScoreController.instance.ResetScores();
        PageController.instance.TurnPageOff(PageType.RunOver,PageType.StartMenu);
        SceneController.instance.Load(SceneType.MainMenu);
    }
}
