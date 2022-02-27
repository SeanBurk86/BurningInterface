
using System.Collections;
using UnityEngine;

namespace UnityCore
{
    namespace Menu
    {
        public class PageController : MonoBehaviour
        {
            public static PageController instance;

            public bool debug;
            public Page[] pages;

            private Hashtable m_Pages;

            #region Unity Functions

            private void Awake()
            {
                // instance
                if (!instance)
                {
                    Configure();
                }
                else
                {
                    Destroy(gameObject);
                }
            }

            #endregion

            #region Public Functions

            public void TurnPageOn(PageType _type)
            {
                if (_type == PageType.None)
                {
                    return;
                } 
                if (!PageExists(_type))
                {
                    LogWarning("You are trying to turn on page ["+_type+"] which is not registered.");
                    return;
                }

                Page _page = GetPage(_type);
                Log("Turning on page "+_type);
                _page.gameObject.SetActive(true);
                _page.Animate(true);
            }

            public void TurnPageOff(PageType _off, PageType _on = PageType.None, bool _waitForExit= false)
            {
                Log("Trying to turn off page "+_off);
                if (_off == PageType.None) return;
                if (!PageExists(_off))
                {
                    LogWarning("You are trying to turn off page ["+_off+"] which is not registered.");
                    return;
                }

                Page _offPage = GetPage(_off);
                
                if (_offPage.gameObject.activeSelf)
                {
                    _offPage.Animate(false);
                }

                if (_on != PageType.None)
                {
                    Page _onPage = GetPage(_on);
                    if (_waitForExit)
                    {
                        StopCoroutine("WaitForPageExit");
                        StartCoroutine(WaitForPageExit(_onPage, _offPage));
                    }
                    else
                    {
                        TurnPageOn(_on);
                    }
                }
            }

            public void TurnOffAllPages(bool _waitForExit = false)
            {
                foreach (Page _page in pages)
                {
                    TurnPageOff(_page.type, _waitForExit: _waitForExit);
                }
            }

            public bool PageIsOn(PageType _type)
            {
                if (!PageExists(_type))
                {
                    LogWarning("You are trying to detect if a page is on ["+_type+"], but it hasn't been registered.");
                    return false;
                }

                return GetPage(_type).isOn;
            }

            #endregion

            #region Private Functions

            private IEnumerator WaitForPageExit(Page _on, Page _off)
            {
                while (_off.targetState != Page.FLAG_NONE)
                {
                    Log("Waiting for "+_off.type+" to exit");
                    yield return null;
                }
                
                TurnPageOn(_on.type);
            }

            private void Configure()
            {
                instance = this;
                m_Pages = new Hashtable();
                RegisterAllPages();
                
                DontDestroyOnLoad(gameObject);
            }

            private void RegisterAllPages()
            {
                foreach (Page _page in pages)
                {
                    RegisterPage(_page);
                }
            }

            private void RegisterPage(Page _page)
            {
                if (_page == null) return;
                if (PageExists(_page.type))
                {
                    LogWarning("You are trying to register page ["+_page.type+"] that has already been registered: "+_page.gameObject.name);
                }
                
                m_Pages.Add(_page.type, _page);
                Log("Registered new page ["+_page.type+"]");
            }
            

            private Page GetPage(PageType _type)
            {
                if (!PageExists(_type))
                {
                    LogWarning("You are trying to get page ["+_type+"] that has not been registered.");
                    return null;
                }

                return (Page) m_Pages[_type];
            }

            private bool PageExists(PageType _type)
            {
                return m_Pages.Contains(_type);
            }

            private void Log(string _msg)
            {
                if (!debug) return;
                Debug.Log("[PageController]: "+_msg);
            }

            private void LogWarning(string _msg)
            {
                if (!debug) return;
                Debug.LogWarning("[PageController]: "+_msg);
            }

            #endregion
        }
        
    }
}
