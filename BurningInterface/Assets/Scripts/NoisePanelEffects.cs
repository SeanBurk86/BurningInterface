
using System;
using TMPro;
using UnityEngine;

public class NoisePanelEffects : MonoBehaviour
{
    public static NoisePanelEffects instance;
    public string garbageString;
    public int panelWidth  = 15;
    public int panelHeight = 10;
    [SerializeField] private TMP_Text m_NoisePanelText;
    [SerializeField] private float m_NoiseChangeInterval = 1f;
    private float noiseTimer = 0f;

    private void Awake()
    {
        Configure();
        garbageString = m_NoisePanelText.text;
    }

    private void Update()
    {
        noiseTimer += Time.deltaTime;
        if (noiseTimer > m_NoiseChangeInterval)
        {
            AddGarbage();
            CycleGarbage();
            TrimGarbage();
            FormatGarbage();
            m_NoisePanelText.text = garbageString;
            noiseTimer = 0f;
        }
    }

    private void AddGarbage()
    {
        garbageString = PlayerInputHandler.instance.garbage + garbageString;
        PlayerInputHandler.instance.garbage = "";
    }

    private void CycleGarbage()
    {
        garbageString = garbageString.Substring(garbageString.Length - 1, 1) +
                        garbageString.Substring(0, garbageString.Length - 1);
    }

    private void TrimGarbage()
    {
        garbageString = garbageString.Trim();
        garbageString = garbageString.Replace("\n", String.Empty);
        if (garbageString.Length > panelWidth*panelHeight) garbageString = garbageString.Substring(0, panelWidth*panelHeight);
    }

    private void FormatGarbage()
    {
        string _formattedGarbage = "";
        for (int i = 0; i < garbageString.Length; i++)
        {
            if (i != 0 && i % panelWidth == 0)
            {
                _formattedGarbage += "\n";
            }
            _formattedGarbage += garbageString.Substring(i, 1);
        }

        garbageString = _formattedGarbage;
    }

    private void Configure()
    {
        if (!instance) instance = this;
        else Destroy(gameObject);
    }
}
