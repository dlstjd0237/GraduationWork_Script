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

        private Stack<IPopupUI> _popupStack = new Stack<IPopupUI>(); // 팝업 UI 스택

        private UIBase _sceneUI = null; // 현재 씬 UI
        public UIBase SceneUI
        {
            get { return _sceneUI; }
            set { _sceneUI = value; }
        }


        private GameObject _root;

        // UI 루트 오브젝트
        public GameObject Root
        {
            get
            {
                // 캐시된 _root가 없다면 찾거나 새로 생성
                if (_root == null)
                {
                    // 씬에 있는지 확인
                    _root = GameObject.Find("@UI_Root");

                    // 씬에 없다면 새로 생성
                    if (_root == null)
                    {
                        // 씬에 없으므로 새로 생성
                        _root = new GameObject { name = "@UI_Root" };
                    }
                }

                return _root;
            }
        }

        // UI의 Canvas를 설정하는 메서드
        public void SetCanvas(GameObject go, bool sort = true, int sortOrder = 0)
        {
            // UI 오브젝트에 Canvas 컴포넌트를 추가하거나 가져옴
            Canvas canvas = GetOrAddComponent<Canvas>(go);
            if (canvas == null)
            {
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.overrideSorting = sort;
            }

            // UI 오브젝트에 CanvasScaler 컴포넌트를 추가하거나 가져옴
            CanvasScaler cs = go.GetOrAddComponent<CanvasScaler>();
            if (cs != null)
            {
                cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                cs.referenceResolution = new Vector2(1920, 1080);
            }

            // UI GraphicRaycaster 추가
            go.GetOrAddComponent<GraphicRaycaster>();

            // 정렬을 설정할지 여부
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

        // 현재 씬 UI를 가져오는 메서드
        public T GetSceneUI<T>() where T : UIBase 
        {
            return _sceneUI as T;
        }

        //자식 UI 생성
        public T MakeSupItem<T>(Transform parent, string name = null, bool pooling = true) where T : UIBase  
        {
            if (string.IsNullOrEmpty(name))
                name = typeof(T).Name;

            GameObject go = Managers.Resource.Instantiate(name, parent, pooling);
            go.transform.SetParent(parent);

            return GetOrAddComponent<T>(go);
        }


        // 월드 스페이스 UI 생성
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

        // 씬 UI 생성
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

        // 팝업 UI 생성
        public T ShowPopup<T>(string name = null) where T : UIBase
        {
            if (string.IsNullOrEmpty(name))
                name = typeof(T).Name;

            GameObject go = Managers.Resource.Instantiate(name);
            if (go.TryGetComponent(out IPopupUI ipopupUI)) 
            {
                IPopupUI popupUI = ipopupUI
                _popupStack.Push(popupUI);

                popupUI.OpenPopup();
            }

            T popup = GetOrAddComponent<T>(go);

            go.transform.SetParent(Root.transform);
            return popup;
        }

        // 팝업 UI 삭제
        public void ClosePopupUI(IPopupUI popup)
        {
            // 팝업 UI가 없으면 리턴
            if (_popupStack.Count == 0)
                return;

            // 팝업 UI 스택에서 팝업 popup이 있는지 확인
            if (_popupStack.Contains(popup))
            {
                
                List<IPopupUI> tempList = new List<IPopupUI>(_popupStack);             // 스택을 리스트로 변환
                tempList.Remove(popup);                                                // popup을 리스트에서 제거
                _popupStack = new Stack<IPopupUI>(tempList);                           // 리스트를 다시 스택으로 변환

                popup.ClosePopup(() => Managers.Resource.Destroy(popup.PopupGO));      // 팝업 UI 닫기
                _order--;                                                              // 정렬 순서 감소

                Debug.Log("Selected popup closed successfully.");
            }
            else
            {
                Debug.Log("Popup not found in the stack.");
            }

        }

        // 팝업 UI 삭제
        public void ClosePopupUI()
        {
            if (_popupStack.Count == 0)
                return;

            // 팝업 UI 스택에서 팝업 UI를 꺼냄
            IPopupUI popup = _popupStack.Pop();
            //ClosePopup() 호출 후 팝업 UI 삭제
            popup.ClosePopup(() => Managers.Resource.Destroy(popup.PopupGO));
            // 정렬 순서 감소
            _order--;

        }

        // 팝업 UI 모두 삭제
        public void CloseAllPopupUI()
        {
            while (_popupStack.Count > 0)
                ClosePopupUI();

        }

        // 팝업 UI가 얼마나 쌓여있는지 확인
        public int GetPopupCount()
        {
            return _popupStack.Count;
        }

        // 팝업 UI 스택을 비움
        public void PopupStackClear()
        {
            _popupStack.Clear();
        }

        // 씬 초기화
        public void Clear()
        {
            CloseAllPopupUI();
            _sceneUI = null;
        }
    }
}