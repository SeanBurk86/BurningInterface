
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

    private Key GetKey(char _char)
    {
        if (Char.IsLetter(_char))
        {
            return (Key) Enum.Parse(typeof(Key), _char.ToString());
        }
        if (Char.IsDigit(_char))
        {
            if (_char == '0')
            {
                return (Key) Enum.Parse(typeof(Key), "50");
            }
            else
            {
                string _enumValue = (UInt32.Parse(_char.ToString()) + 40).ToString();
                return (Key) Enum.Parse(typeof(Key), _enumValue);
            }
        }
        return Key.Space;
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
                }
            }
            else
            {
                if (Char.IsDigit(_char) || Char.IsLetter(_char))
                {
                    AudioController.instance.PlayAudio(AudioType.KEYHOLEDISCONNECT_SFX);
                    ConnectionController.instance.DisconnectKeyhole(_char.ToString());
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
