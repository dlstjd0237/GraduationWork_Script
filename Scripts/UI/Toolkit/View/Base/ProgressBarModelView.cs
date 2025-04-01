using BIS.UI.Data;
using System;
using UnityEngine.UIElements;
using DG.Tweening;
namespace BIS.UIToolkit.ViewModel
{
    public class ProgressBarModelView : UIBase<ProgressBar>
    {
        private ProgressBar _progressBar;

        public event Action<float> ChangeValueEvent;
        //public event Action Value

        private float _maxAmount;
        private float _minAmount;
        private float _currentAmount;

        public float MaxAmount
        {
            get => _maxAmount;
            set
            {
                _maxAmount = value;
                ChangeValueEvent?.Invoke(_currentAmount);
            }
        }

        public float MinAmount
        {
            get => _minAmount;
            set
            {
                _minAmount = value;
                ChangeValueEvent?.Invoke(_currentAmount);
            }
        }

        public float CurrentAmount
        {
            get => _currentAmount;
            set
            {
                _currentAmount = value;
                ChangeValueEvent?.Invoke(_currentAmount);
            }
        }


        public ProgressBarModelView(ProgressBarData data)
        {
            _progressBar = data.progressBar;
            _maxAmount = data.maxAmount;
            _minAmount = data.minAmount;
            _currentAmount = data.currentAmount;

            SetUpData();

        }

        protected override void SetUpData()
        {
            _ui = _progressBar;

            _progressBar.lowValue = _minAmount;
            _progressBar.highValue = _maxAmount;
            _progressBar.value = _currentAmount;


            base.SetUpData();
        }


        public void SetFillAmount(float value)
        {
            _currentAmount = value;
            DOTween.To(() => _progressBar.value, x => _progressBar.value = x, _currentAmount, _tweenDuration);
            ChangeValueEvent?.Invoke(_currentAmount);
        }

        /// <summary>
        /// Run SetFillAmount on the last line. 
        /// </summary>
        /// <param name="amount"></param>
        public void IncreaseValue(float amount)
        {
            _currentAmount += amount;
            if (_currentAmount > _maxAmount)
                _currentAmount = _maxAmount;
            SetFillAmount(_currentAmount);
        }

        /// <summary>
        /// Run SetFillAmount on the last line. 
        /// </summary>
        /// <param name="amount"></param>
        public void decreaseValue(float amount)
        {
            _currentAmount -= amount;
            if (_currentAmount < _minAmount)
                _currentAmount = _minAmount;
            SetFillAmount(_currentAmount);
        }

    }
}
