using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using BIS.UI;
using BIS.Core;
using BIS.Shared.Interface;
using static BIS.Core.Utility.Util;
using static BIS.Core.Utility.Extension;
namespace BIS.Manager
{

    public class UIManager//UIPopup관리하는 매니저
    {
        private int _order = 10;

        private Stack<IPopupUI> _popupStack = new Stack<IPopupUI>();

        private UIBase _sceneUI = null;
        public UIBase SceneUI
        {
            get { return _sceneUI; }
            set { _sceneUI = value; }
        }


        private GameObject _root;

        public GameObject Root
        {
            get
            {
                // 캐시된 _root가 없다면 찾거나 새로 생성
                if (_root == null)
                {
                    _root = GameObject.Find("@UI_Root");

                    if (_root == null)
                    {
                        _root = new GameObject { name = "@UI_Root" };
                    }
                }

                return _root;
            }
        }

        public void SetCanvas(GameObject go, bool sort = true, int sortOrder = 0)
        {
            Canvas canvas = GetOrAddComponent<Canvas>(go);
            if (canvas == null)
            {
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.overrideSorting = sort;
            }

            CanvasScaler cs = go.GetOrAddComponent<CanvasScaler>();
            if (cs != null)
            {
                cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                cs.referenceResolution = new Vector2(1920, 1080);
            }

            go.GetOrAddComponent<GraphicRaycaster>();

            if (sort)
            {
                canvas.sortingOrder = _order;
                _order++;
            }
            else
            {
                canvas.sortingOrder = sortOrder;
            }
        }

        public T GetSceneUI<T>() where T : UIBase
        {
            return _sceneUI as T;
        }

        public T MakeSupItem<T>(Transform parent, string name = null, bool pooling = true) where T : UIBase
        {
            if (string.IsNullOrEmpty(name))
                name = typeof(T).Name;

            GameObject go = Managers.Resource.Instantiate(name, parent, pooling);
            go.transform.SetParent(parent);

            return GetOrAddComponent<T>(go);
        }

        public T MakeWorldSpaceUI<T>(Transform parent = null, string name = null) where T : UIBase
        {
            if (string.IsNullOrEmpty(name))
                name = typeof(T).Name;

            GameObject go = Managers.Resource.Instantiate(name, parent);
            go.transform.SetParent(parent);
            if (parent != null)
                go.transform.SetParent(parent);

            Canvas canvas = go.GetOrAddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;

            return GetOrAddComponent<T>(go);
        }

        /// <summary>
        /// UI_GameScene
        /// Gold, Dia UI_Item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T ShowBaseUI<T>(string name = null) where T : UIBase
        {
            if (string.IsNullOrEmpty(name))
                name = typeof(T).Name;

            GameObject go = Managers.Resource.Instantiate(name);
            T baseUI = GetOrAddComponent<T>(go);

            go.transform.SetParent(Root.transform);

            return baseUI;
        }

        public T ShowSceneUI<T>(string name = null) where T : UIBase
        {
            if (string.IsNullOrEmpty(name))
                name = typeof(T).Name;

            GameObject go = Managers.Resource.Instantiate(name);

            T sceneUI = GetOrAddComponent<T>(go);

            _sceneUI = sceneUI;

            go.transform.SetParent(Root.transform);

            return sceneUI;
        }

        public T ShowPopup<T>(string name = null) where T : UIBase
        {
            if (string.IsNullOrEmpty(name))
                name = typeof(T).Name;

            GameObject go = Managers.Resource.Instantiate(name);
            if (go.TryGetComponent(out IPopupUI ipopupUI))
            {
                IPopupUI popupUI = ipopupUI;
                _popupStack.Push(popupUI);

                popupUI.OpenPopup();
            }

            T popup = GetOrAddComponent<T>(go);

            go.transform.SetParent(Root.transform);
            return popup;
        }

        public void ClosePopupUI(IPopupUI popup)
        {
            if (_popupStack.Count == 0)
                return;

            if (_popupStack.Contains(popup))
            {
                List<IPopupUI> tempList = new List<IPopupUI>(_popupStack);
                tempList.Remove(popup);
                _popupStack = new Stack<IPopupUI>(tempList);

                popup.ClosePopup(() => Managers.Resource.Destroy(popup.PopupGO));
                _order--;

                Debug.Log("Selected popup closed successfully.");
            }
            else
            {
                Debug.Log("Popup not found in the stack.");
            }

        }

        public void ClosePopupUI()
        {
            if (_popupStack.Count == 0)
                return;

            IPopupUI popup = _popupStack.Pop();
            popup.ClosePopup(() => Managers.Resource.Destroy(popup.PopupGO));
            _order--;

        }

        public void CloseAllPopupUI()
        {
            while (_popupStack.Count > 0)
                ClosePopupUI();

        }
        public int GetPopupCount()
        {
            return _popupStack.Count;
        }

        public void PopupStackClear()
        {
            _popupStack.Clear();
        }

        public void Clear()
        {
            CloseAllPopupUI();
            _sceneUI = null;
        }
    }
}