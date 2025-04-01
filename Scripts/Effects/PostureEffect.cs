using BIS.Shared.Interface;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace BIS.Effects
{
    public class PostureEffect : MonoBehaviour, IEffectable
    {
        [SerializeField] private Image _iconImage;
        private Image _effectIcon;
        private void Awake()
        {
            _effectIcon = GetComponent<Image>();
        }
        public void PlayEffect()
        {
            Color defualtColor = _iconImage.color;
            _iconImage.color = Color.white;
            _effectIcon.enabled = true;
            Sequence seq = DOTween.Sequence();
            seq.Append(_effectIcon.transform.DOScale(Vector3.one * 1.4f, 0.05f).SetEase(Ease.Linear));
            seq.Append(_effectIcon.transform.DOScale(Vector3.one * 1.0f, 0.1f)).SetEase(Ease.Linear);
            seq.Append(_effectIcon.transform.DOScale(Vector3.one * 1.8f, 0.05f).SetEase(Ease.Linear));
            seq.Append(_effectIcon.transform.DOScale(Vector3.one * 1.0f, 0.1f)).SetEase(Ease.Linear);
            seq.Append(_effectIcon.transform.DOScale(Vector3.one * 3.0f, 0.5f));
            seq.Join(_effectIcon.DOFade(0, 0.5f));
            seq.OnComplete(() =>
            {
                _effectIcon.DOFade(1, 0);
                _effectIcon.enabled = false;
                _iconImage.color = defualtColor;
                _effectIcon.transform.DOScale(1, 0);
            }
            );


        }
    }
}
