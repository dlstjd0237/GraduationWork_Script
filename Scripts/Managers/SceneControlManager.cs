using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using Main.Runtime.Core.Events;
using BIS.Core;
using BIS.Events;
using Main.Core;
using Main.Runtime.Manager;
using UnityEngine.Serialization;

namespace BIS.Manager
{
    public class SceneControlManager : MonoSingleton<SceneControlManager>
    {
        [FormerlySerializedAs("_gmaeEventChannelSO")] [SerializeField]
        private GameEventChannelSO _gameEventChannelSO;

        [SerializeField] private Image _image;
        private Color _cr;
        private float _fadeCool = 0.5f;


        private IEnumerator Start()
        {
            yield return new WaitUntil(() => AddressableManager.IsLoaded);
            _gameEventChannelSO = AddressableManager.Load<GameEventChannelSO>("GameEventChannel");
            _gameEventChannelSO.AddListener<SceneChangeEvent>(HnadleChangeEvent);
        }

        private void HnadleChangeEvent(SceneChangeEvent evt)
        {
            LoadScene(evt.changeSceneName);
        }

        private void OnDestroy()
        {
            _gameEventChannelSO.RemoveListener<SceneChangeEvent>(HnadleChangeEvent);
        }

        /// <summary>
        /// 0 -> 1
        /// </summary>
        /// <param name="action"></param>
        public static void FadeOut(Action action, bool ignoreTimescale = true) =>
            Instance._fadeOut(action, ignoreTimescale);

        /// <summary>
        /// 1 -> 0
        /// </summary>
        /// <param name="action"></param>
        public static void FadeIn(Action action, bool ignoreTimescale = true) =>
            Instance._fadeIn(action, ignoreTimescale);


        private void _fadeIn(Action action, bool ignoreTimescale)
        {
            _image.raycastTarget = false;
            StartCoroutine(fadeInCo(action, ignoreTimescale));
        }

        private IEnumerator fadeInCo(Action action, bool ignoreTimescale)
        {
            _cr = _image.color;
            while (_image.color.a >= 0)
            {
                float deltaTime = ignoreTimescale ? Time.unscaledDeltaTime : Time.deltaTime;
                float f = deltaTime / _fadeCool;
                _cr.a -= f;
                _image.color = _cr;
                yield return null;
            }

            _cr.a = 0;
            _image.color = _cr;

            action?.Invoke();
        }

        private void _fadeOut(Action action, bool ignoreTimescale)
        {
            _image.raycastTarget = true;
            StopAllCoroutines();
            StartCoroutine(fadeOutCo(action, ignoreTimescale));
        }

        private IEnumerator fadeOutCo(Action action, bool ignoreTimescale)
        {
            _cr = _image.color;
            while (_image.color.a <= 1)
            {
                float deltaTime = ignoreTimescale ? Time.unscaledDeltaTime : Time.deltaTime;
                float f = deltaTime / _fadeCool;
                _cr.a += f;
                _image.color = _cr;

                yield return null;
            }

            _cr.a = 1;
            _image.color = _cr;
            action?.Invoke();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            DontDestroyOnLoad(gameObject);
        }

        public static void LoadScene(string sceneName) => FadeOut(() => { SceneManager.LoadScene(sceneName); }
        );

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            FadeIn(() => { });
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}