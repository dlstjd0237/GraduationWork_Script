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

    public class UIManager//UIPopup�����ϴ� �Ŵ���
    {
        private int _order = 10;

        private Stack<IPopupUI> _popupStack = new Stack<IPopupUI>(); // �˾� UI ����

        private UIBase _sceneUI = null; // ���� �� UI
        public UIBase SceneUI
        {
            get { return _sceneUI; }
            set { _sceneUI = value; }
        }


        private GameObject _root;

        // UI ��Ʈ ������Ʈ
        public GameObject Root
        {
            get
            {
                // ĳ�õ� _root�� ���ٸ� ã�ų� ���� ����
                if (_root == null)
                {
                    // ���� �ִ��� Ȯ��
                    _root = GameObject.Find("@UI_Root");

                    // ���� ���ٸ� ���� ����
                    if (_root == null)
                    {
                        // ���� �����Ƿ� ���� ����
                        _root = new GameObject { name = "@UI_Root" };
                    }
                }

                return _root;
            }
        }

        // UI�� Canvas�� �����ϴ� �޼���
        public void SetCanvas(GameObject go, bool sort = true, int sortOrder = 0)
        {
            // UI ������Ʈ�� Canvas ������Ʈ�� �߰��ϰų� ������
            Canvas canvas = GetOrAddComponent<Canvas>(go);
            if (canvas == null)
            {
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.overrideSorting = sort;
            }

            // UI ������Ʈ�� CanvasScaler ������Ʈ�� �߰��ϰų� ������
            CanvasScaler cs = go.GetOrAddComponent<CanvasScaler>();
            if (cs != null)
            {
                cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                cs.referenceResolution = new Vector2(1920, 1080);
            }

            // UI GraphicRaycaster �߰�
            go.GetOrAddComponent<GraphicRaycaster>();

            // ������ �������� ����
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

        // ���� �� UI�� �������� �޼���
        public T GetSceneUI<T>() where T : UIBase 
        {
            return _sceneUI as T;
        }

        //�ڽ� UI ����
        public T MakeSupItem<T>(Transform parent, string name = null, bool pooling = true) where T : UIBase  
        {
            if (string.IsNullOrEmpty(name))
                name = typeof(T).Name;

            GameObject go = Managers.Resource.Instantiate(name, parent, pooling);
            go.transform.SetParent(parent);

            return GetOrAddComponent<T>(go);
        }


        // ���� �����̽� UI ����
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

        // �� UI ����
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

        // �˾� UI ����
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

        // �˾� UI ����
        public void ClosePopupUI(IPopupUI popup)
        {
            // �˾� UI�� ������ ����
            if (_popupStack.Count == 0)
                return;

            // �˾� UI ���ÿ��� �˾� popup�� �ִ��� Ȯ��
            if (_popupStack.Contains(popup))
            {
                
                List<IPopupUI> tempList = new List<IPopupUI>(_popupStack);             // ������ ����Ʈ�� ��ȯ
                tempList.Remove(popup);                                                // popup�� ����Ʈ���� ����
                _popupStack = new Stack<IPopupUI>(tempList);                           // ����Ʈ�� �ٽ� �������� ��ȯ

                popup.ClosePopup(() => Managers.Resource.Destroy(popup.PopupGO));      // �˾� UI �ݱ�
                _order--;                                                              // ���� ���� ����

                Debug.Log("Selected popup closed successfully.");
            }
            else
            {
                Debug.Log("Popup not found in the stack.");
            }

        }

        // �˾� UI ����
        public void ClosePopupUI()
        {
            if (_popupStack.Count == 0)
                return;

            // �˾� UI ���ÿ��� �˾� UI�� ����
            IPopupUI popup = _popupStack.Pop();
            //ClosePopup() ȣ�� �� �˾� UI ����
            popup.ClosePopup(() => Managers.Resource.Destroy(popup.PopupGO));
            // ���� ���� ����
            _order--;

        }

        // �˾� UI ��� ����
        public void CloseAllPopupUI()
        {
            while (_popupStack.Count > 0)
                ClosePopupUI();

        }

        // �˾� UI�� �󸶳� �׿��ִ��� Ȯ��
        public int GetPopupCount()
        {
            return _popupStack.Count;
        }

        // �˾� UI ������ ���
        public void PopupStackClear()
        {
            _popupStack.Clear();
        }

        // �� �ʱ�ȭ
        public void Clear()
        {
            CloseAllPopupUI();
            _sceneUI = null;
        }
    }
}