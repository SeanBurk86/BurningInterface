
using System;
using UnityCore.Game;
using UnityEngine;

namespace UnityCore
{
    namespace Session
    {
        public class SessionController : MonoBehaviour
        {
            public static SessionController instance;
            
            private long m_SessionStartTime;
            private float m_FPS;
            private bool m_IsPaused;
            private GameController m_Game;

            public long sessionStartTime
            {
                get
                {
                    return m_SessionStartTime;
                }
            }

            public float fps
            {
                get
                {
                    return m_FPS;
                }
            }

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
            }

            private void OnApplicationFocus(bool hasFocus)
            {
                //pause the game if the the app is not in focus
            }

            private void Update()
            {
                if (m_IsPaused) return;
                m_FPS = Time.frameCount / Time.time;
            }

            #endregion

            #region Public Functions

            public void InitializeGame(GameController _game)
            {
                m_Game = _game;
                m_Game.OnInit();
            }

            #endregion

            #region Private Functions
            
            private void Configure()
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                StartSession();
            }

            private void StartSession()
            {
                m_SessionStartTime = EpochSeconds();
            }

            private long EpochSeconds()
            {
                var _epoch = new DateTimeOffset(DateTime.UtcNow);
                return _epoch.ToUnixTimeSeconds();
            }

            #endregion
            
        }
    }
}