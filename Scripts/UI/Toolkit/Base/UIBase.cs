using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static BIS.Shared.Define;

namespace BIS.UIToolkit
{
    public delegate void UIEventHandler();
    public class UIBase<T> where T : VisualElement
    {
        protected T _ui;
        private Dictionary<EUIEventType, UIEventHandler> _uiEvents;
        protected float _tweenDuration = 0.25f;
        public float TweenDuration
        {
            get => _tweenDuration;
            set => _tweenDuration = value;
        }

        protected UIBase()
        {
            _uiEvents = new Dictionary<EUIEventType, UIEventHandler>();
        }


        protected virtual void SetUpData()
        {
            _ui.RegisterCallback<MouseDownEvent>(HandleDownEvent);
            _ui.RegisterCallback<MouseMoveEvent>(HandleMoveEvent);
            _ui.RegisterCallback<MouseEnterEvent>(HandleEnterEvent);
            _ui.RegisterCallback<MouseOutEvent>(HandleExitEvent);
            _ui.RegisterCallback<ClickEvent>(HandleClickEvent);
        }



        public void RegisterEvent(EUIEventType eventType, UIEventHandler action)
        {
            if (_uiEvents.ContainsKey(eventType))
                _uiEvents[eventType] += action;
            else
                _uiEvents[eventType] = action;
        }
        public void UnregisterEvent(EUIEventType eventType, UIEventHandler action)
        {
            if (_uiEvents.ContainsKey(eventType))
            {
                _uiEvents[eventType] -= action;
                if (_uiEvents[eventType] == null)
                    _uiEvents.Remove(eventType);
            }
        }

        public void SetPos(Vector2 Pos)
        {
            _ui.style.left = Pos.x;
            _ui.style.top = Pos.y;
        }

        public void AddToClass(string ClassName)
        {
            _ui.AddToClassList(ClassName);
        }
        private void HandleMoveEvent(MouseMoveEvent evt)
        {
            if (_uiEvents.ContainsKey(EUIEventType.MOVE))
                _uiEvents[EUIEventType.MOVE]?.Invoke();
        }

        private void HandleDownEvent(MouseDownEvent evt)
        {
            if (_uiEvents.ContainsKey(EUIEventType.DOWN))
                _uiEvents[EUIEventType.DOWN]?.Invoke();
        }

        private void HandleExitEvent(MouseOutEvent evt)
        {
            if (_uiEvents.ContainsKey(EUIEventType.EXIT))
                _uiEvents[EUIEventType.EXIT]?.Invoke();
        }

        private void HandleEnterEvent(MouseEnterEvent evt)
        {
            if (_uiEvents.ContainsKey(EUIEventType.ENTER))
                _uiEvents[EUIEventType.ENTER]?.Invoke();
        }

        private void HandleClickEvent(ClickEvent evt)
        {
            if (_uiEvents.ContainsKey(EUIEventType.CLICK))
                _uiEvents[EUIEventType.CLICK]?.Invoke();
        }
    }
}
