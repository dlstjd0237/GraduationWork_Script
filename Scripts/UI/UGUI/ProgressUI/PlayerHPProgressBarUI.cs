using UnityEngine.UI;
using Main.Runtime.Manager;
using Main.Shared;
using Main.Runtime.Combat;
using Main.Runtime.Agents;
using PJH.Players;
using System;
using System.Collections;
using BIS.Core.Utility;
using UnityEngine;

namespace BIS.UI
{
    public class PlayerHPProgressBarUI : ProgressBaseUI
    {
        public enum Progress
        {
            HealthBar
        }

        private void Start()
        {
            var player = (PlayerManager.Instance?.Player as Player);
            if (player != null)
            {
                Health health = player.HealthCompo;
                health.OnChangedHealth += HandleHitEvent;
                health.OnDeath += HandleDeath;
            }
        }


        private void HandleDeath()
        {
            Util.UIFadeOut(gameObject, true);
        }

        public override void SetUp()
        {
            Bind<Image>(typeof(Progress));
            _fill = Get<Image>((int)Progress.HealthBar);
        }

        private void OnDisable()
        {
            var player = (PlayerManager.Instance?.Player as Player);
            if (player != null)
            {
                Health health = player.HealthCompo;
                health.OnChangedHealth -= HandleHitEvent;
                health.OnDeath -= HandleDeath;
            }
        }

        public void HandleHitEvent(float currentHealth, float minHealth, float maxHealth) =>
            ValueUpdate(currentHealth, maxHealth, minHealth);
    }
}