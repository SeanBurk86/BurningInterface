
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeyholeController : MonoBehaviour
{
    public static KeyholeController instance;

    public bool debug;

    public List<Keyhole> keyholes = new List<Keyhole>();
    public List<Keyhole> danglingKeyholes = new List<Keyhole>();

    [SerializeField] private GameObject m_KeyholePrefab;
    [SerializeField] private Transform[] m_KeyholeMap;

    private string m_KeyholeValues = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
    private void Awake()
    {
        Configure();
    }
    
    private void Update()
    {
        foreach (Keyhole _keyhole in keyholes.ToList())
        {
            if (_keyhole.isTouched && !_keyhole.isConnected && !danglingKeyholes.Contains(_keyhole)) danglingKeyholes.Add(_keyhole);
        }
        
        foreach (Keyhole _keyhole in danglingKeyholes.ToList())
        {
            if (_keyhole.isConnected && danglingKeyholes.Contains(_keyhole)) danglingKeyholes.Remove(_keyhole);
            else if (!_keyhole.isTouched && danglingKeyholes.Contains(_keyhole)) danglingKeyholes.Remove(_keyhole);
        }
        
    }

    public Keyhole GetKeyholeByValue(string _value)
    {
        foreach (Keyhole _keyhole in keyholes)
        {
            if (_keyhole.value == _value)
            {
                return _keyhole;
            }
        }

        return null;
    }

    public void DestroyAllKeyholes()
    {
        foreach (Keyhole _keyhole in keyholes)
        {
            Destroy(_keyhole.gameObject);
        }
    }

    private void InitializeKeyholes()
    {
        Vector3 _pos = new Vector3(-5.5f, 4f, 0f);
        char[] _shuffledChars = KnuthShuffle(m_KeyholeValues.ToCharArray());
        for(int i=0;i<_shuffledChars.Length;i++)
        {
            char _char = _shuffledChars[i];
            GameObject _newObj = Instantiate(m_KeyholePrefab, _pos, Quaternion.identity);
            //increment pos
            _pos += new Vector3(1.25f, 0f, 0f);
            if ((i+1) % 6 == 0 && i != 0)
            {
                _pos -= new Vector3(7.5f, 1.25f, 0f);
            }
            
            Keyhole _keyhole = _newObj.GetComponent<Keyhole>();
            keyholes.Add(_keyhole);
            _keyhole.SetKeyholeValue(_char.ToString());
        }
    }

    private void Configure()
    {
        if (!instance)
        {
            instance = this;
            InitializeKeyholes();
        }
        else Destroy(gameObject);
    }
    
    private static T[] KnuthShuffle<T>(T[] array)
    {
        System.Random random = new System.Random();
        for (int i = 0; i < array.Length; i++)
        {
            int j = random.Next(i, array.Length);
            T temp = array[i]; 
            array[i] = array[j]; 
            array[j] = temp;
        }

        return array;
    }
    
    private void Log(string _msg)
    {
        if(debug) Debug.Log("[KeyholeController]: "+_msg);
    }
}
