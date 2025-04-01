using YTH.Enemies;
using Main.Runtime.Agents;
using Main.Runtime.Combat;
using System;
using System.Collections;
using UnityEngine;

namespace BIS.UI
{
    public class EnemyPostureProgressUI : PostureProgressUI
    {
        private AgentMomentumGauge _agentMomentumGauge;
        private Health _health;

        private void Start()
        {
            _health = transform.parent.parent.GetComponent<Health>();
            BaseEnemy enemy = transform.parent.parent.GetComponent<BaseEnemy>();
            enemy.OnResetItem += HandleResetItem;
            _agentMomentumGauge = enemy.GetCompo<AgentMomentumGauge>(true);

            if (_health)
                _health.OnDeath += HandleDeath;
            if (_agentMomentumGauge)
                _agentMomentumGauge.OnChangedMomentumGauge += SetUpProgress;
        }

        private void HandleResetItem()
        {
            gameObject.SetActive(true);
        }


        private void OnEnable()
        {
            if (_health)
                _health.OnDeath += HandleDeath;
            if (_agentMomentumGauge)
                _agentMomentumGauge.OnChangedMomentumGauge += SetUpProgress;
        }

        private void HandleDeath()
        {
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            if (_health)
                _health.OnDeath -= HandleDeath;
            if (_agentMomentumGauge)
                _agentMomentumGauge.OnChangedMomentumGauge -= SetUpProgress;
        }

        public override void SetUpProgress(float current, float max)
        {
            if (current > 0 && !gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }

            base.SetUpProgress(current, max);
        }
    }
}