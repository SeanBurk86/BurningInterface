
using System;
using UnityEngine;

namespace UnityCore
{
    namespace Data
    {
        public class DataController : MonoBehaviour
        {
            public static DataController instance = null;
            
            private static readonly string MASTER_VOLUME = "mastervolume";
            private static readonly string MUSIC_VOLUME = "musicvolume";
            private static readonly string SFX_VOLUME = "sfxvolume";
            private static readonly string HIGH_SCORE_SUFFIX = "_highscore";
            private static readonly int DEFAULT_INT = 0;

            #region Properties
            public int MasterVolume
            {
                get
                {
                    return GetInt(MASTER_VOLUME);
                }
                set
                {
                    SaveInt(MASTER_VOLUME, value);
                }
            }

            public int MusicVolume
            {
                get
                {
                    return GetInt(MUSIC_VOLUME);
                }
                set
                {
                    SaveInt(MUSIC_VOLUME, value);
                }
            }
            
            

            public int SfxVolume
            {
                get
                {
                    return GetInt(SFX_VOLUME);
                }
                set
                {
                    SaveInt(SFX_VOLUME, value);
                }
            }

            #endregion
                
            #region Unity Functions

            private void Awake()
            {
                if (!instance) Configure();
                else Destroy(gameObject);
            }

            #endregion

            #region Public Functions

            

            #endregion
            
            #region Private Functions

            private void SaveInt(string _data, int _value)
            {
                PlayerPrefs.SetInt(_data, _value);
            }

            private int GetInt(string _data)
            {
                return PlayerPrefs.GetInt(_data, DEFAULT_INT);
            }

            private void SaveString(string _data, string _value)
            {
                PlayerPrefs.SetString(_data, _value);
            }

            private string GetString(string _data)
            {
                return PlayerPrefs.GetString(_data);
            }

            private void Configure()
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }

            #endregion
        }
    }
}