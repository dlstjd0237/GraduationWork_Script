using BIS.Events;
using BIS.Manager;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using BIS.Data;
using Main.Runtime.Core.Events;

namespace BIS.UI.Popup
{
    public class RankingUpPopupUI : PopupUI
    {
        private GameEventChannelSO _gameEventChannel;

        private enum Images
        {
            Background,
            Fog
        }

        private enum Texts
        {
            Rank_Text,
            NewRank_Text
        }

        [SerializeField] private Material _whiteFog, blackFog;

        public override bool Init()
        {
            if (base.Init() == false)
                return false;
            _gameEventChannel = Managers.Resource.Load<GameEventChannelSO>("GameEventChannel");
            // ==== Bind UI ====
            BindImages(typeof(Images));
            BindTexts(typeof(Texts));
            // =================

            ChangeText(UIEvent.EnemyPrevieChoiceEvent.enemyPartySO);
            return true;
        }

        public override void OpenPopup()
        {
            Debug.Log("됬으");
            base.OpenPopup();
        }


        public void ChangeText(EnemyPartySO data)
        {
            float duration = 0.5f;

            var rankText = GetText((int)Texts.Rank_Text);
            rankText.text = $"{ Managers.Rank.Data.CurrentRank}위";
            var newRankText = GetText((int)Texts.NewRank_Text);
            string rankStr;
            if (data == null)
            {
                rankStr = "24235위";
            }
            else
            {
                int newRanking = data.GetEnemyMaxRanking();
                rankStr = $"{newRanking} 위";
                Managers.Rank.ChangeRank(newRanking);
            }

            newRankText.text = rankStr;

            rankText.rectTransform.DOShakePosition(7, 10, snapping: true, vibrato: 70).SetUpdate(true);
            Sequence sq = DOTween.Sequence();
            sq.SetUpdate(true);
            sq.AppendInterval(3);
            sq.AppendCallback(() =>
            {
                GetImage((int)Images.Background).color = Color.white;
                GetImage((int)Images.Fog).material = blackFog;
                var evt = GameEvents.DestroyDeadEnemy;
                _gameEventChannel.RaiseEvent(evt);
                newRankText.color = new Color(0, 0, 0, 0);
                newRankText.rectTransform.DOAnchorPosY(0, duration * 0.5f).SetUpdate(true);
                newRankText.DOFade(1, duration).SetUpdate(true);

                rankText.color = new Color(0, 0, 0, 1);
                rankText.rectTransform.DOAnchorPosY(-200, duration * 0.5f).SetUpdate(true);
                rankText.DOFade(0, duration).SetUpdate(true);
            });
            sq.AppendInterval(2);
            sq.AppendCallback(() => SceneControlManager.LoadScene("Lobby"));
        }
    }
}