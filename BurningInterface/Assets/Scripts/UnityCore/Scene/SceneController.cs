
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityCore.Menu;

namespace UnityCore
{
    namespace Scene
    {
        public class SceneController : MonoBehaviour
        {
            public delegate void SceneLoadDelegate(SceneType _scene);

            public static SceneController instance;

            public bool debug;

            private PageController m_Menu;
            private SceneType m_TargetScene;
            private PageType m_LoadingPage;
            private SceneLoadDelegate m_SceneLoadDelegate;
            private bool m_SceneIsLoading;

            private PageController menu
            {
                get
                {
                    if (m_Menu == null)
                    {
                        m_Menu = PageController.instance;
                    }
                    if (m_Menu == null)
                    {
                        LogWarning("You are trying to access the PageController, but no instance was found.");
                    }

                    return m_Menu;
                }
            }

            private string currentSceneName
            {
                get
                {
                    return SceneManager.GetActiveScene().name;
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

            private void OnDisable()
            {
                Dispose();
            }

            #endregion

            #region Public Functions

            public void Load(SceneType _scene,
                bool _reload=false,
                PageType  _loadingPage=PageType.None,
                SceneLoadDelegate _sceneLoadDelegate=null)
            {
                if (_loadingPage != PageType.None && !menu)
                {
                    return;
                }

                if (!SceneCanBeLoaded(_scene, _reload))
                {
                    return;
                }

                m_SceneIsLoading = true;
                m_TargetScene = _scene;
                m_LoadingPage = _loadingPage;
                m_SceneLoadDelegate = _sceneLoadDelegate;
                StartCoroutine("LoadScene");
            }

            public void ReloadScene()
            {
                Load(StringToSceneType(currentSceneName), true);
            }
            

            #endregion

            #region Private Functions

            private void Configure()
            {
                instance = this;
                SceneManager.sceneLoaded += OnSceneLoaded;
                DontDestroyOnLoad(gameObject);
            }

            private void Dispose()
            {
                SceneManager.sceneLoaded -= OnSceneLoaded;
            }

            private async void OnSceneLoaded(UnityEngine.SceneManagement.Scene _scene, LoadSceneMode _mode)
            {
                if (m_TargetScene == SceneType.None)
                {
                    return;
                }

                SceneType _sceneType = StringToSceneType(_scene.name);
                if (m_TargetScene != _sceneType)
                {
                    return;
                }

                if (m_SceneLoadDelegate != null)
                {
                    try
                    {
                        m_SceneLoadDelegate(_sceneType);
                    }
                    catch (System.Exception)
                    {
                        LogWarning("Unable to respond with sceneLoadDelegate after scene ["+_sceneType+"] loaded.");
                    }
                }

                if (m_LoadingPage != PageType.None)
                {
                    await Task.Delay(1000);
                    menu.TurnPageOff(m_LoadingPage);
                }

                m_SceneIsLoading = false;
            }

            private IEnumerator LoadScene()
            {
                if (m_LoadingPage != PageType.None)
                {
                    menu.TurnPageOn(m_LoadingPage);
                    while (!menu.PageIsOn(m_LoadingPage))
                    {
                        yield return null;
                    }
                }

                string _targetSceneName = SceneTypeToString(m_TargetScene);
                SceneManager.LoadScene(_targetSceneName);
            }

            private bool SceneCanBeLoaded(SceneType _scene, bool _reload)
            {
                string _targetSceneName = SceneTypeToString(_scene);
                if (currentSceneName == _targetSceneName && !_reload)
                {
                    LogWarning("You are trying to load a scene ["+_scene+"] which is already active");
                    return false;
                } 
                else if (_targetSceneName == string.Empty)
                {
                    LogWarning("The scene you are trying to load ["+_scene+"] is not valid");
                    return false;
                } else if (m_SceneIsLoading)
                {
                    LogWarning("Unable to load scene ["+_scene+"]. Another scene ["+m_TargetScene+"] is loading.");
                    return false;
                }

                return true;
            }

            private string SceneTypeToString(SceneType _scene)
            {
                switch (_scene)
                {
                    case SceneType.MainGame: return "MainGame";
                    case SceneType.MainMenu: return "MainMenu";
                    default:
                        LogWarning("Scene ["+_scene+"] does not contain a string for a valid scene");
                        return String.Empty;
                }
            }

            private SceneType StringToSceneType(string _scene)
            {
                switch (_scene)
                {
                    case "MainGame": return SceneType.MainGame;
                    case "MainMenu": return SceneType.MainMenu;
                    default:
                        LogWarning("Scene ["+_scene+"] does not contain a string for a valid scene");
                        return SceneType.None;
                        
                }
            }

            private void Log(string _msg)
            {
                if (!debug) return;
                Debug.Log("[SceneController]: "+_msg);
            }

            private void LogWarning(string _msg)
            {
                if (!debug) return;
                Debug.LogWarning("[SceneController]: "+_msg);
            }
            

            #endregion
            
        }
    }
}
