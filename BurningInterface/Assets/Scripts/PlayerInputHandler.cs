
using System;
using UnityCore.Audio;
using UnityCore.Game;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using AudioType = UnityCore.Audio.AudioType;

public class PlayerInputHandler : MonoBehaviour
{
    public static PlayerInputHandler instance;

    public string pathWriterString = "";

    public bool m_IsPathWriterOpen;

    public string garbage;

    public void OnPathWriterToggleInput(InputAction.CallbackContext _context)
    {
        if (GameController.instance.hasRoundStarted && !GameController.instance.isRoundOver)
        {
            if (_context.started) m_IsPathWriterOpen = true;
            if (_context.canceled)
            {
                m_IsPathWriterOpen = false;
                SubmitPathString();
            }
        }
    }

    public void OnGlyphSubmit(InputAction.CallbackContext _context)
    {
        if (GameController.instance.hasRoundStarted && !GameController.instance.isRoundOver)
        {
            if (_context.started) RoundController.instance.SubmitGlyph();
        }
    }
    
    private void Awake()
    {
        Configure();
    }

    private void OnEnable()
    {
        Keyboard.current.onTextInput += OnTextInput;
    }

    private void OnDisable()
    {
        Keyboard.current.onTextInput -= OnTextInput;
    }

    private void Update()
    {
        
    }

    public void SubmitPathString()
    {
        if(pathWriterString.Length>1) ConnectionController.instance.CreateConnectionPathFromString(pathWriterString);
        pathWriterString = "";
    }

    private void OnTextInput(char _char)
    {
        if (GameController.instance.hasRoundStarted && !GameController.instance.isRoundOver)
        {
            if (m_IsPathWriterOpen)
            {
                if (!pathWriterString.Contains(_char.ToString().ToUpper())
                    && (Char.IsDigit(_char) || Char.IsLetter(_char)))
                {
                    AudioController.instance.PlayAudio(AudioType.KEYHOLETOUCH_SFX);
                    pathWriterString += _char.ToString().ToUpper();
                    garbage += _char.ToString();
                }
            }
            else
            {
                if (Char.IsDigit(_char) || Char.IsLetter(_char))
                {
                    ConnectionController.instance.DisconnectKeyhole(_char.ToString());
                    garbage += _char.ToString();
                }
            }
        }
    }

    private void Configure()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(gameObject);
    }
}
