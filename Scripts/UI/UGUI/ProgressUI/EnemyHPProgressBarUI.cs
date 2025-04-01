using Main.Runtime.Combat;
using UnityEngine.UI;
using System;
using System.Collections;
using UnityEngine;
using YTH.Enemies;

namespace BIS.UI
{
    public class EnemyHPProgressBarUI : ProgressBaseUI
    {
        public enum Images
        {
            Fill
        }

        private Health _health;

        private void Start()
        {
            BaseEnemy enemy = transform.parent.parent.GetComponent<BaseEnemy>();
            enemy.OnResetItem += HandleResetItem;
            _health = transform.parent.parent.GetComponent<Health>();
            _health.OnChangedHealth += HandleHitEvent;
            _health.OnDeath += HandleDeath;
        }

        private void HandleResetItem()
        {
            gameObject.SetActive(true);
        }

        private void OnEnable()
        {
            if (_health)
            {
                _health.OnChangedHealth += HandleHitEvent;
                _health.OnDeath += HandleDeath;
            }
        }

        private void HandleDeath()
        {
            gameObject.SetActive(false);
        }

        public override void SetUp()
        {
            Bind<Image>(typeof(Images));
            _fill = Get<Image>((int)Images.Fill);
        }

        private void OnDisable()
        {
            if (_health)
            {
                _health.OnChangedHealth -= HandleHitEvent;
                _health.OnDeath -= HandleDeath;
            }
        }

        public void HandleHitEvent(float currentHealth, float minHealth, float maxHealth)
        {
            if (currentHealth > 0 && !gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }

            ValueUpdate(currentHealth, maxHealth, minHealth);
        }
    }
}