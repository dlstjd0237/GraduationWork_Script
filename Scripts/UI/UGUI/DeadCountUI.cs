using BIS.Core.Utility;
using Main.Runtime.Manager;
using PJH.Players;
using System;
using Cysharp.Threading.Tasks;
using Main.Runtime.Combat;
using UnityEngine;

namespace BIS.UI
{
    public class DeadCountUI : UIBase
    {
        private enum Texts
        {
            DeadCountTexr
        }

        public override bool Init()
        {
            if (base.Init() == false)
                return false;

            SubscribeHealthCountEvent();
            BindTexts(typeof(Texts));
            GetText((int)Texts.DeadCountTexr).text = $"¸ñ¼û | {PlayerManager.Instance.CurrentHeartCount}";

            return true;
        }

        private async void SubscribeHealthCountEvent()
        {
            var IPlayer = PlayerManager.Instance.Player;
            if (IPlayer != null)
            {
                Player player = (IPlayer as Player);
                await UniTask.WaitUntil(() => player.HealthCompo != null);
                player.HealthCompo.OnDeath += HandleDeadEvent;
            }
        }

        private void HandleDeadEvent()
        {
            Util.UIFadeOut(gameObject, true);
        }

        private void OnDestroy()
        {
            var player = PlayerManager.Instance?.Player;
            if (player != null)
                (player as Player).HealthCompo.OnDeath -= HandleDeadEvent;
        }
    }
}