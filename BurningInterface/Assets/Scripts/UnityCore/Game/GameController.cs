
using UnityCore.Data;
using UnityCore.Menu;
using UnityCore.Session;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using AudioType = UnityCore.Audio.AudioType;

namespace UnityCore
{
    namespace Game
    {
        public class GameController : MonoBehaviour
        {
            public static GameController instance;
            
            public AudioMixer audioMixer;

            public bool hasRoundStarted, isRoundOver;
            
            #region Unity Functions

            private void Awake()
            {
                if (!instance)
                {
                    Configure();
                }
                else
                {
                    Destroy(gameObject);
                }

                hasRoundStarted = false;
                isRoundOver = false;
            }

            private void Start()
            {
                SessionController.instance.InitializeGame(instance);
                PageController.instance.TurnPageOn(PageType.StartMenu);
                SetVolumeSettings();
            }

            #endregion

            #region Public Functions
            
            public void OnInit()
            {
            }

            #endregion

            #region Private Functions

            private void Configure()
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            
            private void SetVolumeSettings()
            { 
            }

            #endregion
        }
        
    }
}
