using UnityEngine;
using BIS.Shared;
using Main.Runtime.Manager;

namespace BIS.UI
{
    public class OptionSoundTextFillUI : OptionTextFillUI
    {
        [SerializeField] private ESoundType _soundType;

        public override bool Init()
        {
            if (base.Init() == false)
                return false;

            _valueChange += HandleValueChange;

            return true;
        }

        private void HandleValueChange(float currentVlaue)
        {
            switch (_soundType)
            {
                case ESoundType.Master:
                    Managers.FMODManager.SetMainVolume(currentVlaue);
                    break;
                case ESoundType.VFX:
                    Managers.FMODManager.SetSFXVolume(currentVlaue);
                    break;
                case ESoundType.BGM:
                    Managers.FMODManager.SetMusicVolume(currentVlaue);
                    break;
            }
        }

        private void OnDisable()
        {
            _valueChange -= HandleValueChange;
        }
    }
}
