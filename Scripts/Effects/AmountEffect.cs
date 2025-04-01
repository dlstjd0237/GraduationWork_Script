using BIS.Shared.Interface;
using UnityEngine;
using UnityEngine.UI;
using BIS.Core.Utility;
using DG.Tweening;

namespace BIS.Effects
{
    public class AmountEffect : MonoBehaviour, IEffectable
    {
        private Image _frame;

        private void Awake()
        {
            _frame = Util.FindChild(gameObject, "Frame_Image", true).GetComponent<Image>();
        }

        public void PlayEffect()
        {
            _frame.rectTransform.DOScale(Vector3.one * 1.25f, 0.5f);
            _frame.DOFade(0f, 0.75f);
        }
    }
}
