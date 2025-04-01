using BIS.Core.Utility;
using Main.Runtime.Agents;
using Main.Runtime.Manager;
using PJH.Players;

namespace BIS.UI
{
    public class PlayerPostureProgressUI : PostureProgressUI
    {
        private void Start()
        {
            Player player = (PlayerManager.Instance?.Player as Player);
            if (player != null)
            {
                PlayerHealth health = player.GetComponent<PlayerHealth>();
                health.OnDeath += HandleDeath;
                AgentMomentumGauge momentumGaugeCompo = player.GetCompo<AgentMomentumGauge>(true);
                momentumGaugeCompo.OnChangedMomentumGauge += SetUpProgress;
            }
        }

        private void HandleDeath()
        {
            Util.UIFadeOut(gameObject, true);
        }


        private void OnDisable()
        {
            Player player = (PlayerManager.Instance?.Player as Player);
            if (player != null)
            {
                PlayerHealth health = player.GetComponent<PlayerHealth>();
                health.OnDeath += HandleDeath;
                player.GetCompo<AgentMomentumGauge>(true).OnChangedMomentumGauge -= SetUpProgress;
            }
        }
    }
}