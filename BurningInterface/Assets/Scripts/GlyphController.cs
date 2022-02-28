
using System;
using UnityEngine;
using Random = System.Random;

public class GlyphController : MonoBehaviour
{
    public static GlyphController instance;

    private Random m_Random;

    private void Awake()
    {
        Configure();
    }

    public Glyph GenerateGlyph()
    {
        int _positiveCount = 0;
        Glyph _glyph = new Glyph();
        for (int i=0;i<_glyph.order.Length;i++)
        {
            _glyph.order[i] = NextBoolean(m_Random);
            if (_glyph.order[i]) _positiveCount++;
        }
        
        if(_positiveCount!=1) return _glyph;
        GenerateGlyph();
        return null;
    }

    private static bool NextBoolean(Random _random)
    {
        return _random.Next() > (Int32.MaxValue / 2);
    }

    private void Configure()
    {
        if (!instance)
        {
            instance = this;
            m_Random = new Random();
        }
        else Destroy(gameObject);
    }
}
