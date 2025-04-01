using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using BIS.Core;

namespace BIS.UI
{
    public abstract class ProgressBaseUI : UIBase
    {
        [SerializeField] protected float _fillDuration = 0.25f;
        protected float _maxValue;
        protected float _minValue;
        protected float _currentValue;

        protected Image _fill;

        public event ValueChangeEvent OnValueChangeEvent;

        public override bool Init()
        {
            if (base.Init() == false)
                return false;

            SetUp();

            return true;
        }


        /// <summary>
        /// �̰����� �����̴� �� �־������
        /// </summary>
        public abstract void SetUp();


        /// <summary>
        /// �� �ʱ�ȭ
        /// </summary>
        protected void ValueInit()
        {
            _currentValue = _maxValue;
            ValueUpdate(_currentValue, _maxValue, _minValue);
        }

        /// <summary>
        /// �� ����
        /// </summary>
        /// <param name="value"></param>
        protected void Increase(float value)
        {
            _currentValue += value;
            _currentValue = Mathf.Min(_maxValue, _currentValue);
            ValueUpdate(_currentValue, _maxValue, _minValue);
        }

        /// <summary>
        /// �� ����
        /// </summary>
        /// <param name="value"></param>
        protected void Decrease(float value)
        {
            _currentValue -= value;
            _currentValue = Mathf.Max(_minValue, _currentValue);
            ValueUpdate(_currentValue, _maxValue, _minValue);
        }

        protected void ValueUpdate(float currentValue, float maxValue, float minValue)
        {
            _fill.DOFillAmount(currentValue / maxValue, _fillDuration);
            OnValueChangeEvent?.Invoke(currentValue, maxValue, minValue);
        }
    }
}