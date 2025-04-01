using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace BIS.Core.Utility
{
    public static class Util
    {
        public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
        {
            T compoent = go.GetComponent<T>();
            if (compoent == null)
                compoent = go.AddComponent<T>();

            return compoent;
        }

        /// <summary>
        /// All Finding
        /// </summary>
        /// <param name="go">Target GameObject</param>
        /// <param name="name">GameObject Name</param>
        /// <param name="recursive">Find in Every Child</param>
        /// <returns></returns>
        public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
        {
            Transform transform = FindChild<Transform>(go, name, recursive);
            if (transform == null)
                return null;

            return transform.gameObject;
        }
        public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
        {
            if (go == null)
                return null;

            if (recursive == false)
            {
                for (int i = 0; i < go.transform.childCount; ++i)
                {
                    Transform transform = go.transform.GetChild(i);
                    if (string.IsNullOrEmpty(name) || transform.name == name)
                    {
                        T component = transform.GetComponent<T>();
                        if (component != null)
                            return component;
                    }
                }
            }
            else
            {
                foreach (T component in go.GetComponentsInChildren<T>())
                {
                    if (string.IsNullOrEmpty(name) || component.name == name)
                    {
                        return component;
                    }
                }
            }
            return null;
        }
        public static bool ValueNullCheck<T>(this T Value) where T : class
        {
            if (Value == null)
            {
                Type type = typeof(T);
                Debug.LogError($"{type.ToString()} Is Null");
                return false;
            }
            return true;
        }

        public static bool IntToBool(int value) => value == 0 ? false : true;

        public static int BoolToInt(bool value) => value ? 1 : 0;

        public static RaycastHit GetMouseToRay(Camera cam)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                return hit;
            }
            return hit;
        }

        public static void UIColorChange<T>(GameObject go, Color color, float durtaion = 0.2f, TweenCallback onComplete = null) where T : Graphic
            => go.GetComponent<T>().DOColor(color, durtaion).OnComplete(onComplete).SetUpdate(true);



        /// <summary>
        /// FadeOut => 1 ~ 0
        /// </summary>
        /// <param name="go"></param>
        /// <param name="isFadeOut"></param>
        /// <param name="duration"></param>
        public static void UIFadeOut(GameObject go, bool isFadeOut, float duration = 0.2f, Action onCompleteCallBack = null)
        {
            CanvasGroup canvas = GetOrAddComponent<CanvasGroup>(go);
            canvas.DOFade(isFadeOut == true ? 0 : 1, duration)
                .OnComplete(() =>
                {
                    canvas.interactable = !isFadeOut;
                    canvas.blocksRaycasts = !isFadeOut;
                    onCompleteCallBack?.Invoke();
                }).SetUpdate(true);
        }
    }
}

