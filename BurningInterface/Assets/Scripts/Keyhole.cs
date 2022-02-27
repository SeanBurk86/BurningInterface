
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Keyhole : MonoBehaviour
{
    public string value;
    public bool isConnected, isTouched;
    [SerializeField] private TMP_Text m_DisplayText;

    [SerializeField] private SpriteRenderer m_SpriteRenderer;
    public List<Keyhole> connectedNodes = new List<Keyhole>();

    private void OnEnable()
    {
        m_DisplayText.text = value;
    }

    private void Update()
    {
        if (connectedNodes.Count > 0) isConnected = true;
        else isConnected = false;
        
        if (PlayerInputHandler.instance.pathWriterString.Contains(value.ToUpper()))
        {
            m_SpriteRenderer.color = Color.red;
            isTouched = true;
            
        }
        else
        {
            m_SpriteRenderer.color = Color.white;
            isTouched = false;
        }

        if (isConnected)
        {
            m_SpriteRenderer.color = Color.green;
            m_DisplayText.gameObject.SetActive(false);
        }
        else
        {
            m_DisplayText.gameObject.SetActive(true);
        }
    }

    public void SetKeyholeValue(string _value)
    {
        value = _value;
        m_DisplayText.text = value;
    }
    
}
