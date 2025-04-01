using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using BIS.Manager;
using BIS.Events;
using Main.Runtime.Core.Events;
using BIS.Data;
using BIS.Shared.Interface;
using UnityEngine.Events;
using System;
using Main.Core;
using Debug = UnityEngine.Debug;

namespace BIS.UI.Popup
{
    public class LoadingPopupUI : PopupUI, ISavable
    {
        private GameEventChannelSO _uiEvent;
        [SerializeField] private CurrencySO _dataSO;
        [field: SerializeField] public SaveIDSO IdData { get; set; }

        private enum Texts
        {
            Loading_Text
        }

        private enum Images
        {
            Loading_Image
        }

        public override bool Init()
        {
            FixedScreen.FixedScreenSet();
            Managers.UI.SetCanvas(gameObject, false, 1);
            // ====UI Binding====
            BindTexts(typeof(Texts));
            BindImages(typeof(Images));
            // ==================

            GetText((int)Texts.Loading_Text).SetText($" Data Loading");
            GetImage((int)Images.Loading_Image).transform.DORotate(new Vector3(0, 0, -360), 3, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Incremental)
                .SetEase(Ease.Linear);

            return true;
        }

        private async void Start()
        {
            await StartLoadAssetsPark();
            StartLoadAssetsBaek();
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.S))
            {
                _dataSO.ChangeValue(50000);
                Managers.Save.SaveGame();
            }
#endif
        }

        private void StartLoadAssetsBaek() //���ʿ��� �����������
        {
            BIS.Manager.Managers.Resource.LoadAllAsync<UnityEngine.Object>("PreLoad", (key, count, totalCount) =>
            {
#if UNITY_EDITOR
                Debug.Log($"{key} {count}/{totalCount}");
#endif
            });
        }

        private async UniTask StartLoadAssetsPark()
        {
            // Addressable asset �ε�
            await AddressableManager.LoadALlAsync<UnityEngine.Object>("PreLoad",
                (key, count, totalCount) =>
                {
                    GetText((int)Texts.Loading_Text).SetText($" Data Loading ({count}/{totalCount})");
                    // �ε��� �Ϸ�Ǹ�
                    if (count == totalCount)
                    {
                        Main.Runtime.Manager.Managers.FMODManager.Init();
                        LoadNextScene().Forget(); // �񵿱� �ε� �� �� �ε�
                    }
                });
        }

        private async UniTask LoadNextScene()
        {
            GetText((int)Texts.Loading_Text).SetText($"Enter Game");
            // 0.5�� ���
            await UniTask.Delay(500);

            GetImage((int)Images.Loading_Image).transform.DOKill();
            _uiEvent = Managers.Resource.Load<GameEventChannelSO>("UIEventChannelSO");
            LoadingUIEvent evt = UIEvent.LoadingUIEvent;
            evt.isComplete = true;
            ClosePopup();
        }

        public string GetSaveData()
        {
            MoneyData saveData = new MoneyData
            {
                CurrencyAmmount = _dataSO.CurrentAmmount
            };
            return JsonUtility.ToJson(saveData);
        }

        public void RestoreData(string data)
        {
            MoneyData loadedData = JsonUtility.FromJson<MoneyData>(data);
            _dataSO.ChangeValue(loadedData.CurrencyAmmount);
        }
    }
}