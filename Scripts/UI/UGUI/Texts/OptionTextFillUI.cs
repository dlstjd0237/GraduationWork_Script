using BIS.Core;
using BIS.Data;
using BIS.Shared.Interface;
using System;
using UnityEngine;

namespace BIS.UI
{
    public class OptionTextFillUI : UIBase, ISavable
    {
        private enum Texts
        {
            ResultText
        }

        private enum Buttons
        {
            LeftArrow,
            RightArrow
        }

        private const char FullIcon = '●';
        private const char NotFullIcon = '○';
        private const short _maxIndex = 5;
        private const short _minIndex = 0;

        private readonly float[] ResultValue = { 0.01f, 0.2f, 0.4f, 0.6f, 0.8f, 1 };

        protected SoundValueChangeEvent _valueChange;

        [SerializeField] private short _valueIndex = 3;
        [SerializeField] private SaveIDSO _idData;
        public SaveIDSO IdData => _idData;


        public override bool Init()
        {
            if (base.Init() == false)
                return false;

            BindButtons(typeof(Buttons));
            BindTexts(typeof(Texts));

            GetButton((int)Buttons.LeftArrow).onClick.AddListener(RemoveValue);
            GetButton((int)Buttons.RightArrow).onClick.AddListener(AddValue);

            UpdateView();

            return true;
        }

        public void AddValue()
        {
            _valueIndex = _maxIndex == _valueIndex ? _valueIndex : ++_valueIndex;
            UpdateView();
        }

        public void RemoveValue()
        {
            _valueIndex = _minIndex == _valueIndex ? _valueIndex : --_valueIndex;
            UpdateView();
        }

        public void UpdateView()
        {
            var text = GetText((int)Texts.ResultText);
            text.text = string.Empty;

            _valueChange?.Invoke(ResultValue[_valueIndex]);

            for (int i = 0; i < _valueIndex; ++i)
            {
                text.text += FullIcon;
            }

            for (int i = 0; i < _maxIndex - _valueIndex; ++i)
            {
                text.text += NotFullIcon;
            }
        }

        private void OnDisable()
        {
            GetButton((int)Buttons.LeftArrow).onClick.RemoveListener(RemoveValue);
            GetButton((int)Buttons.RightArrow).onClick.RemoveListener(AddValue);
        }

        public string GetSaveData()
        {
            Debug.Log("세이트");
            ValueData<short> saveData = new ValueData<short>(_valueIndex);
            return JsonUtility.ToJson(saveData);
        }

        public void RestoreData(string data)
        {
            ValueData<short> LoadData = JsonUtility.FromJson<ValueData<short>>(data);
            Debug.Log(LoadData.value);
            _valueIndex = LoadData.value;
            UpdateView();
        }
    }
}