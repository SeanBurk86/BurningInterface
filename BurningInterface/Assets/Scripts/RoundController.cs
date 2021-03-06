
using System;
using TMPro;
using UnityCore.Audio;
using UnityCore.Menu;
using UnityEngine;
using AudioType = UnityCore.Audio.AudioType;
using Random = System.Random;

public class RoundController : MonoBehaviour
{
    public static RoundController instance;

    public bool debug;

    public Glyph currentGlyph;
    private TMP_Text m_CurrentGlyphDisplay;

    private Random m_Random;
    private string m_PositiveChars = "1+T|";
    private string m_NegativeChars = "0-F_";

    

    private void Awake()
    {
        Configure();
    }
    
    private void OnEnable()
    {
        PageController.instance.TurnPageOn(PageType.PlayerScore);
        PageController.instance.TurnPageOn(PageType.CurrentGlyph);
        PageController.instance.TurnPageOn(PageType.RoundTimer);
        PageController.instance.TurnPageOn(PageType.CountdownTimer);
        PageController.instance.TurnPageOn(PageType.GlyphsLeft);
        PageController.instance.TurnPageOn(PageType.NoisePanel);
        MakeNewCurrentGlyph();
    }

    private void OnDisable()
    {
        PageController.instance.TurnPageOff(PageType.PlayerScore);
        PageController.instance.TurnPageOff(PageType.CurrentGlyph);
        PageController.instance.TurnPageOff(PageType.RoundTimer);
        PageController.instance.TurnPageOff(PageType.GlyphsLeft);
        PageController.instance.TurnPageOff(PageType.NoisePanel);
    }

    private void Update()
    {
    }

    public void SubmitGlyph()
    {
        if (EvaluatePlayerGlyph())
        {
            Log("Glyph accepted");
            AudioController.instance.PlayAudio(AudioType.GLYPHSOLVE_SFX);
            ScoreController.instance.IncrementGlyphsSolved();
            MakeNewCurrentGlyph();
        }
        else
        {
            Log("Glyph rejected");
            AudioController.instance.PlayAudio(AudioType.GLYPHREJECTED_SFX);
        }
    }

    public void MakeNewCurrentGlyph()
    {
        char _positiveNodeChar = RandomCharFromString(m_Random, m_PositiveChars);
        char _negativeNodeChar = RandomCharFromString(m_Random, m_NegativeChars);
        currentGlyph = GlyphController.instance.GenerateGlyph();
        string _glyphDisplayString = "";
        for(int i=0;i<currentGlyph.order.Length;i++)
        {
            if (currentGlyph.order[i]) _glyphDisplayString += _positiveNodeChar;
            else _glyphDisplayString += _negativeNodeChar;
            if ((i+1) % 6 == 0 && i != 0)
            {
                _glyphDisplayString += "\n";
            }
        }

        m_CurrentGlyphDisplay.text = _glyphDisplayString;
    }

    public bool EvaluatePlayerGlyph()
    {

        for(int i=0;i<currentGlyph.order.Length;i++)
        {
            if (KeyholeController.instance.keyholes[i].isConnected != currentGlyph.order[i]) return false;
        }
        
        return true;
    }
    
    private void Configure()
    {
        if (!instance)
        {
            instance = this;
            m_CurrentGlyphDisplay = PageController.instance.gameObject.GetComponent<NavigationUtil>().glyphDisplayText;
            m_Random = new Random();
        }
        else Destroy(gameObject);
    }
    
    private static char RandomCharFromString(Random _random, string _msg)
    {
        return _msg[_random.Next(_msg.Length)];
    }
    
    private void Log(string _msg)
    {
        if(debug) Debug.Log("[RoundController]: "+_msg);
    }
}
