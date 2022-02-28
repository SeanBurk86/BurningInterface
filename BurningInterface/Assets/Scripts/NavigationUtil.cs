
using TMPro;
using UnityCore.Audio;
using UnityCore.Game;
using UnityCore.Menu;
using UnityCore.Scene;
using UnityEngine;
using AudioType = UnityCore.Audio.AudioType;

public class NavigationUtil : MonoBehaviour
{

    public TMP_Text countdownText, roundTimerText, glyphDisplayText;
    public void ExitGame()
    {
        Application.Quit();
    }

    public void StartRun()
    {
        PageController.instance.TurnPageOff(PageType.StartMenu);
        SceneController.instance.Load(SceneType.MainGame);
        AudioController.instance.PlayAudio(AudioType.GAMEFIGHT_ST);
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
        AudioController.instance.PlayAudio(AudioType.GAMEFIGHT_ST);
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
        AudioController.instance.PlayAudio(AudioType.LOBBYVISION_ST);
    }
}
